using System.ComponentModel.DataAnnotations;

namespace StudentAssignmentManager.Models;

public class AssignmentDefinition
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    [Required]
    [Range(0.1, double.MaxValue, ErrorMessage = "Max Points must be greater than 0.")]
    [Display(Name = "Max Points")]
    public double MaxPoints { get; set; }

    public virtual ICollection<Submission> Submissions { get; set; }

    public AssignmentDefinition()
    {
        Submissions = new HashSet<Submission>();
    }
}