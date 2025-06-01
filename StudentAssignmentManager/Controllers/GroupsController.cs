using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAssignmentManager.Data;
using StudentAssignmentManager.Models;
using StudentAssignmentManager.Models.ViewModels;

namespace StudentAssignmentManager.Controllers;

[Authorize]
public class GroupsController : Controller
{
    private readonly ApplicationDbContext _context;

    public GroupsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Groups
    public IActionResult Index()
    {
        var groups = _context.Groups.Include(g => g.GroupMemberships).ToList();
        return View(groups);
    }

    // GET: Groups/Details/5
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        Group group = _context.Groups
        .Include(g => g.GroupMemberships) // First, include the collection itself
            .ThenInclude(gm => gm.Student)   // Then, for each item in GroupMemberships (gm), include its Student
        .FirstOrDefault(g => g.Id == id);
        if (group == null)
        {
            return NotFound();
        }
        return View(group);
    }

    private void PopulateAvailableStudents(GroupCreateEditViewModel viewModel)
    {
        var allStudents = _context.Students.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();
        viewModel.AvailableStudents = new MultiSelectList(allStudents, "Id", "FullName", viewModel.SelectedStudentIds);
    }

    // GET: Groups/Create
    public IActionResult Create()
    {
        var viewModel = new GroupCreateEditViewModel();
        PopulateAvailableStudents(viewModel);
        return View(viewModel);
    }

    // POST: Groups/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("GroupId, GroupName, SelectedStudentIds")] GroupCreateEditViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var group = new Group { GroupName = viewModel.GroupName };

            if (viewModel.SelectedStudentIds != null && viewModel.SelectedStudentIds.Any())
            {
                foreach (var studentId in viewModel.SelectedStudentIds)
                {
                    // Ensure student exists if paranoid, though list is from DB
                    var student = _context.Students.Find(studentId);
                    if (student != null)
                    {
                        group.GroupMemberships.Add(new GroupMember
                        {
                            StudentId = studentId
                        });
                    }
                }
            }

            _context.Groups.Add(group);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Group created successfully.";
            return RedirectToAction("Index");
        }
        PopulateAvailableStudents(viewModel);
        return View(viewModel);
    }

    // GET: Groups/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        Group group = _context.Groups.Include(g => g.GroupMemberships).FirstOrDefault(g => g.Id == id);
        if (group == null)
        {
            return NotFound();
        }

        var viewModel = new GroupCreateEditViewModel
        {
            GroupId = group.Id,
            GroupName = group.GroupName,
            SelectedStudentIds = group.GroupMemberships.Select(gm => gm.StudentId).ToList()
        };
        PopulateAvailableStudents(viewModel);
        return View(viewModel);
    }

    // POST: Groups/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(GroupCreateEditViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var groupToUpdate = _context.Groups.Include(g => g.GroupMemberships).FirstOrDefault(g => g.Id == viewModel.GroupId);
            if (groupToUpdate == null)
            {
                return NotFound();
            }

            groupToUpdate.GroupName = viewModel.GroupName;

            // Update members: Easiest is to remove all and re-add selected
            // More performant is to find differences, but for typical group sizes, this is okay.
            groupToUpdate.GroupMemberships.Clear(); // EF will handle deleting from GroupMembers table
                                                    // Or db.GroupMembers.RemoveRange(groupToUpdate.GroupMemberships); then re-add if clear doesn't cascade correctly.
                                                    // In this setup, clearing the collection and re-adding should work when SaveChanges is called.

            if (viewModel.SelectedStudentIds != null)
            {
                foreach (var studentId in viewModel.SelectedStudentIds)
                {
                    groupToUpdate.GroupMemberships.Add(new GroupMember { StudentId = studentId, GroupId = groupToUpdate.Id });
                }
            }

            _context.Entry(groupToUpdate).State = EntityState.Modified; // Mark group as modified
                                                                        // EF change tracking handles changes to the GroupMemberships collection.
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Group updated successfully.";
            return RedirectToAction("Index");
        }
        PopulateAvailableStudents(viewModel);
        return View(viewModel);
    }


    // GET: Groups/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        Group group = _context.Groups
        .Include(g => g.GroupMemberships)
            .ThenInclude(m => m.Student)
        .FirstOrDefault(g => g.Id == id);
        if (group == null)
        {
            return NotFound();
        }
        return View(group);
    }

    // POST: Groups/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        Group group = _context.Groups.Include(g => g.GroupMemberships)
                               .Include(g => g.Submissions) // Eager load submissions for cascade delete
                               .SingleOrDefault(g => g.Id == id);
        if (group == null)
        {
            return NotFound();
        }

        // EF will handle deleting related GroupMembers due to collection management
        // EF will handle deleting related Submissions if cascade delete is configured in OnModelCreating (which it is)
        _context.GroupMembers.RemoveRange(group.GroupMemberships); // Explicitly remove join table entries
                                                                   // Submissions will be handled by cascade delete on Group
        _context.Groups.Remove(group);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Group deleted successfully.";
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