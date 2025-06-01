using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAssignmentManager.Data;
using StudentAssignmentManager.Models.ViewModels;

namespace StudentAssignmentManager.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Reports/FinalGrades
    public ActionResult FinalGrades()
    {
        var allGroups = _context.Groups
        .Include(g => g.GroupMemberships)
            .ThenInclude(gm => gm.Student)
        .Include(g => g.Submissions)
            .ThenInclude(s => s.AssignmentDefinition)
        .OrderBy(g => g.GroupName)
        .ToList();

        // Get total max points for ALL defined assignments in the system
        double totalMaxPointsForAllDefinedAssignments = _context.AssignmentDefinitions.Sum(ad => (double?)ad.MaxPoints) ?? 0;
        if (totalMaxPointsForAllDefinedAssignments == 0) // Avoid division by zero if no assignments defined
        {
            // Handle this case - perhaps show a message or set grades to 0
            // For now, if no assignments, grade calculation isn't meaningful.
            // You could also set totalMaxPointsForAllDefinedAssignments to 1 to avoid div by zero,
            // but the grades would not be meaningful.
            // Better to show a message on the view if this is 0.
        }


        var viewModelList = new List<GroupFinalGradeViewModel>();

        foreach (var group in allGroups)
        {
            var groupVm = new GroupFinalGradeViewModel
            {
                GroupId = group.Id,
                GroupName = group.GroupName,
                Members = group.GroupMemberships.Select(gm => new StudentInfoViewModel
                {
                    FullName = gm.Student.FullName,
                    RegistrationNumber = gm.Student.RegistrationNumber
                }).ToList(),
                TotalMaxPointsPossibleForAllAssignments = totalMaxPointsForAllDefinedAssignments
            };

            double totalPointsScoredByGroup = 0;

            // Get all assignment definitions to list them, even if not submitted
            var allAssignmentDefinitions = _context.AssignmentDefinitions.ToList();

            foreach (var ad in allAssignmentDefinitions)
            {
                var submission = group.Submissions.FirstOrDefault(s => s.AssignmentDefinitionId == ad.Id);
                if (submission != null && submission.Grade.HasValue)
                {
                    totalPointsScoredByGroup += submission.Grade.Value;
                    groupVm.SubmissionDetails.Add(new SubmissionDetailViewModel
                    {
                        AssignmentTitle = ad.Title,
                        PointsScored = submission.Grade.Value,
                        MaxPointsForAssignment = ad.MaxPoints
                    });
                }
                else // No submission or not graded
                {
                    groupVm.SubmissionDetails.Add(new SubmissionDetailViewModel
                    {
                        AssignmentTitle = ad.Title,
                        PointsScored = null, // Or 0 if you prefer to count unsubmitted as 0 points
                        MaxPointsForAssignment = ad.MaxPoints
                    });
                }
            }

            groupVm.TotalPointsScored = totalPointsScoredByGroup;

            if (totalMaxPointsForAllDefinedAssignments > 0)
            {
                groupVm.FinalGrade = (totalPointsScoredByGroup / totalMaxPointsForAllDefinedAssignments) * 10;
            }
            else
            {
                groupVm.FinalGrade = 0; // Or NaN, or handle in view
            }

            viewModelList.Add(groupVm);
        }
        ViewBag.NoAssignmentsDefined = totalMaxPointsForAllDefinedAssignments == 0;
        return View(viewModelList);
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