using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StudentAssignmentManager.Models.ViewModels;

public class GroupCreateEditViewModel
{
    public int GroupId { get; set; } // For editing, 0 for new

    [Required]
    [StringLength(100)]
    [Display(Name = "Group Name")]
    public string GroupName { get; set; }

    [Display(Name = "Select Students")]
    public List<int> SelectedStudentIds { get; set; }

    [BindNever]
    public MultiSelectList? AvailableStudents { get; set; } // For dropdown list

    public GroupCreateEditViewModel()
    {
        SelectedStudentIds = new List<int>();
    }
}