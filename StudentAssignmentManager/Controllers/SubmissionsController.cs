using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAssignmentManager.Data;
using StudentAssignmentManager.Models;
using StudentAssignmentManager.Models.ViewModels;

namespace StudentAssignmentManager.Controllers;

[Authorize]
public class SubmissionsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly string _submissionsBaseFolder = "SubmissionsData";

    public SubmissionsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
    }

    // GET: Submissions
    public IActionResult Index()
    {
        var submissions = _context.Submissions
            .Include(s => s.AssignmentDefinition)
            .Include(s => s.Group)
            .OrderByDescending(s => s.SubmissionDateTime)
            .ToList();
        return View(submissions);
    }

    private void PopulateDropdowns(SubmissionCreateViewModel viewModel)
    {
        viewModel.AvailableGroups = new SelectList(_context.Groups.OrderBy(g => g.GroupName), "Id", "GroupName", viewModel.GroupId);
        viewModel.AvailableAssignmentDefinitions = new SelectList(_context.AssignmentDefinitions.OrderBy(a => a.Title), "Id", "Title", viewModel.AssignmentDefinitionId);
    }

    // GET: Submissions/Create
    public IActionResult Create()
    {
        var viewModel = new SubmissionCreateViewModel();
        PopulateDropdowns(viewModel);
        return View(viewModel);
    }

    // POST: Submissions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubmissionCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns(viewModel);
            return View(viewModel);
        }

        // Check for existing submission for this group and assignment
        bool submissionExists = await _context.Submissions.AnyAsync(s =>
            s.GroupId == viewModel.GroupId &&
            s.AssignmentDefinitionId == viewModel.AssignmentDefinitionId);

        if (submissionExists)
        {
            ModelState.AddModelError(string.Empty, "A submission for this group and assignment already exists. You can edit or grade the existing one.");
        }

        string relativeFilePath = null;
        string fullDirectoryPath = null;

        if (viewModel.ZipFile == null || viewModel.ZipFile.Length == 0)
        {
            ModelState.AddModelError(nameof(viewModel.ZipFile), "A .zip file is required for submission.");
        }
        else if (!viewModel.ZipFile.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(nameof(viewModel.ZipFile), "Only .zip files are allowed.");
        }
        else
        {
            var group = await _context.Groups.FindAsync(viewModel.GroupId);
            var assignment = await _context.AssignmentDefinitions.FindAsync(viewModel.AssignmentDefinitionId);

            if (group == null || assignment == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Group or Assignment selected. Please try again.");
            }
            else if (ModelState.IsValid)
            {
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff"); ;

                string contentRootPath = _hostingEnvironment.ContentRootPath;
                string submissionsRoot = Path.Combine(contentRootPath, _submissionsBaseFolder);

                fullDirectoryPath = Path.Combine(submissionsRoot,
                                                   group.Id.ToString(),
                                                   assignment.Id.ToString());

                Directory.CreateDirectory(fullDirectoryPath);

                string fileName = $"submission_{group.Id}_{assignment.Id}_{timestamp}.zip";
                string fullFilePath = Path.Combine(fullDirectoryPath, fileName);

                try
                {
                    using (var stream = new FileStream(fullFilePath, FileMode.Create))
                    {
                        await viewModel.ZipFile.CopyToAsync(stream);
                    }

                    relativeFilePath = Path.Combine(group.Id.ToString(), assignment.Id.ToString(), fileName);
                }
                catch (Exception ex)
                {
                    // Log the exception (ex)
                    ModelState.AddModelError(string.Empty, "An error occurred while uploading the file. Please try again.");
                }
            }
        }


        if (!ModelState.IsValid)
        {
            PopulateDropdowns(viewModel);
            return View(viewModel);
        }

        var submission = new Submission
        {
            GroupId = viewModel.GroupId,
            AssignmentDefinitionId = viewModel.AssignmentDefinitionId,
            SubmissionDateTime = DateTime.Now,
            FilePath = relativeFilePath
        };

        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Submission recorded successfully.";
        return RedirectToAction("Index");
    }

    public ActionResult DownloadFile(int submissionId)
    {
        var submission = _context.Submissions.Find(submissionId);
        if (submission == null || string.IsNullOrEmpty(submission.FilePath))
        {
            return NotFound("Submission or file not found.");
        }

        string contentRootPath = _hostingEnvironment.ContentRootPath;
        string submissionsRoot = Path.Combine(contentRootPath, _submissionsBaseFolder);
        string physicalPath = Path.Combine(submissionsRoot, submission.FilePath);

        if (!System.IO.File.Exists(physicalPath))
        {
            return NotFound("File not found on server. It may have been moved or deleted.");
        }

        byte[] fileBytes;
        try
        {
            fileBytes = System.IO.File.ReadAllBytes(physicalPath);
        }
        catch (IOException ex)
        {
            // Log the exception (ex)
            return StatusCode(StatusCodes.Status500InternalServerError, "Error reading the file.");
        }

        string fileName = Path.GetFileName(physicalPath);
        return File(fileBytes, "application/zip", fileName);
    }

    // GET: Submissions/Grade/5
    public ActionResult Grade(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        Submission submission = _context.Submissions
            .Include(s => s.Group)
            .Include(s => s.AssignmentDefinition)
            .FirstOrDefault(s => s.Id == id);

        if (submission == null)
        {
            return NotFound();
        }

        var viewModel = new SubmissionGradeViewModel
        {
            SubmissionId = submission.Id,
            GroupName = submission.Group.GroupName,
            AssignmentTitle = submission.AssignmentDefinition.Title,
            MaxPoints = submission.AssignmentDefinition.MaxPoints,
            CurrentGrade = submission.Grade,
            FilePath = submission.FilePath,
            FileName = !string.IsNullOrEmpty(submission.FilePath) ? Path.GetFileName(submission.FilePath) : "No file"
        };

        return View(viewModel);
    }

    // POST: Submissions/Grade/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Grade(SubmissionGradeViewModel viewModel)
    {
        Submission submissionToUpdate = _context.Submissions
            .Include(s => s.AssignmentDefinition) // Needed for MaxPoints validation
            .FirstOrDefault(s => s.Id == viewModel.SubmissionId);

        if (submissionToUpdate == null)
        {
            return NotFound();
        }

        if (viewModel.CurrentGrade.HasValue)
        {
            if (viewModel.CurrentGrade < 0)
            {
                ModelState.AddModelError("CurrentGrade", "Grade cannot be negative.");
            }
            if (viewModel.CurrentGrade > submissionToUpdate.AssignmentDefinition.MaxPoints)
            {
                ModelState.AddModelError("CurrentGrade", $"Grade cannot exceed the maximum points for this assignment ({submissionToUpdate.AssignmentDefinition.MaxPoints}).");
            }
        }


        if (ModelState.IsValid)
        {
            submissionToUpdate.Grade = viewModel.CurrentGrade;
            _context.Entry(submissionToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Grade updated successfully.";
            return RedirectToAction("Index");
        }

        // Repopulate non-POSTed fields if returning to view
        viewModel.GroupName = submissionToUpdate.Group.GroupName; // Requires loading Group if not already
        viewModel.AssignmentTitle = submissionToUpdate.AssignmentDefinition.Title;
        viewModel.MaxPoints = submissionToUpdate.AssignmentDefinition.MaxPoints;
        viewModel.FilePath = submissionToUpdate.FilePath;
        viewModel.FileName = !string.IsNullOrEmpty(submissionToUpdate.FilePath) ? Path.GetFileName((submissionToUpdate.FilePath)) : "No file";

        return View(viewModel);
    }

    // GET: Submissions/Delete/5
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        Submission submission = _context.Submissions.Include(s => s.Group).Include(s => s.AssignmentDefinition).FirstOrDefault(s => s.Id == id.Value);
        if (submission == null)
        {
            return NotFound();
        }
        return View(submission);
    }

    // POST: Submissions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        Submission submission = _context.Submissions.Find(id);
        if (submission == null)
        {
            return NotFound();
        }

        // Delete associated file if it exists
        if (!string.IsNullOrEmpty(submission.FilePath))
        {
            string physicalPath = submission.FilePath;
            if (System.IO.File.Exists(physicalPath))
            {
                try
                {
                    System.IO.File.Delete(physicalPath);
                    // Optionally, clean up empty directories if this was the last file
                    // string directory = Path.GetDirectoryName(physicalPath);
                    // if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
                    // {
                    //    Directory.Delete(directory);
                    // }
                }
                catch (IOException ex)
                {
                    // Log error or notify user, but proceed with DB deletion
                    TempData["WarningMessage"] = "Could not delete the submission file: " + ex.Message;
                }
            }
        }

        _context.Submissions.Remove(submission);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Submission deleted successfully.";
        return RedirectToAction("Index");
    }


    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
        base.Dispose(disposing);
    }
}