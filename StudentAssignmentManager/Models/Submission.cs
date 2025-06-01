using System.ComponentModel.DataAnnotations;

namespace StudentAssignmentManager.Models;

public class Submission
{
    public int Id { get; set; }

    [Required]
    public int GroupId { get; set; }
    public virtual Group Group { get; set; }

    [Required]
    public int AssignmentDefinitionId { get; set; }
    public virtual AssignmentDefinition AssignmentDefinition { get; set; }

    [Required]
    [Display(Name = "Submission Date")]
    public DateTime SubmissionDateTime { get; set; }

    [StringLength(500)]
    [Display(Name = "File Path")]
    public string FilePath { get; set; } // Path to the uploaded .zip file

    [Display(Name = "Grade (Points)")]
    public double? Grade { get; set; } // Nullable, points awarded
}