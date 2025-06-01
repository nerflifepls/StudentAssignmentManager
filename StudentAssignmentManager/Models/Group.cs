using System.ComponentModel.DataAnnotations;

namespace StudentAssignmentManager.Models;

public class Group
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Group Name")]
    public string GroupName { get; set; }

    public virtual ICollection<GroupMember> GroupMemberships { get; set; }
    public virtual ICollection<Submission> Submissions { get; set; }

    public Group()
    {
        GroupMemberships = new HashSet<GroupMember>();
        Submissions = new HashSet<Submission>();
    }
}