using System.ComponentModel.DataAnnotations;

namespace StudentAssignmentManager.Models.ViewModels;

public class SubmissionGradeViewModel
{
    public int SubmissionId { get; set; }
    public string GroupName { get; set; }
    public string AssignmentTitle { get; set; }
    public double MaxPoints { get; set; } // Display only

    [Range(0, double.MaxValue, ErrorMessage = "Grade must be a non-negative number.")]
    [Display(Name = "Awarded Points")]
    public double? CurrentGrade { get; set; } // The grade to be entered/updated

    public string FilePath { get; set; }
    public string FileName { get; set; }
}