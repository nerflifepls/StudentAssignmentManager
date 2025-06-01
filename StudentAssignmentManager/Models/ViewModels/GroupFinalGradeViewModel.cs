using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentAssignmentManager.Models.ViewModels
{
    public class StudentInfoViewModel // DTO for student details in group
    {
        public string FullName { get; set; }
        public string RegistrationNumber { get; set; }
    }

    public class SubmissionDetailViewModel // DTO for individual submission details
    {
        public string AssignmentTitle { get; set; }
        public double? PointsScored { get; set; }
        public double MaxPointsForAssignment { get; set; }
        public bool IsGraded => PointsScored.HasValue;
    }

    public class GroupFinalGradeViewModel
    {
        public int GroupId { get; set; }

        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        public List<StudentInfoViewModel> Members { get; set; }

        [Display(Name = "Total Points Scored")]
        public double TotalPointsScored { get; set; }

        [Display(Name = "Overall Max Points Possible")]
        public double TotalMaxPointsPossibleForAllAssignments { get; set; } // Sum of MaxPoints of ALL defined assignments

        [Display(Name = "Final Grade (0-10 Scale)")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double FinalGrade { get; set; }

        public List<SubmissionDetailViewModel> SubmissionDetails { get; set; }

        public GroupFinalGradeViewModel()
        {
            Members = new List<StudentInfoViewModel>();
            SubmissionDetails = new List<SubmissionDetailViewModel>();
        }
    }
}