using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StudentAssignmentManager.Models.ViewModels;

public class SubmissionCreateViewModel
{
    [Required]
    [Display(Name = "Group")]
    public int GroupId { get; set; }

    [Required]
    [Display(Name = "Assignment")]
    public int AssignmentDefinitionId { get; set; }

    public IEnumerable<SelectListItem>? AvailableGroups { get; set; }
    public IEnumerable<SelectListItem>? AvailableAssignmentDefinitions { get; set; }

    [Required(ErrorMessage = "Please select a .zip file.")]
    [Display(Name = "Submission File (.zip)")]
    public IFormFile ZipFile { get; set; } // For file upload
}