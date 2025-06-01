using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAssignmentManager.Models;

public class GroupMember
{
    [Key]
    [Column(Order = 1)]
    public int StudentId { get; set; }
    public virtual Student Student { get; set; }

    [Key]
    [Column(Order = 2)]
    public int GroupId { get; set; }
    public virtual Group Group { get; set; }
}