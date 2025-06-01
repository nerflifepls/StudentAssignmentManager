using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAssignmentManager.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required]
    [StringLength(20)]
    [Display(Name = "Registration Number")]
    public string RegistrationNumber { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    public virtual ICollection<GroupMember> GroupMemberships { get; set; }

    public Student()
    {
        GroupMemberships = new HashSet<GroupMember>();
    }

    [NotMapped]
    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}";
}