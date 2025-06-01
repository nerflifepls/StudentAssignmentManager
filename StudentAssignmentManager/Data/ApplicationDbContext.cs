using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentAssignmentManager.Models;

namespace StudentAssignmentManager.Data;

public class ApplicationDbContext : IdentityDbContext
{

    public DbSet<Student> Students { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<AssignmentDefinition> AssignmentDefinitions { get; set; }
    public DbSet<Submission> Submissions { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
        }

        modelBuilder.Entity<GroupMember>()
            .HasKey(gm => new { gm.StudentId, gm.GroupId });

        modelBuilder.Entity<GroupMember>()
            .HasOne(gm => gm.Student)
            .WithMany(s => s.GroupMemberships)
            .HasForeignKey(gm => gm.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupMember>()
            .HasOne(gm => gm.Group)
            .WithMany(g => g.GroupMemberships)
            .HasForeignKey(gm => gm.GroupId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Submission>()
           .HasIndex(s => new { s.GroupId, s.AssignmentDefinitionId }, "IX_GroupAssignmentUnique") // Name can be specified here
           .IsUnique(true);

        modelBuilder.Entity<Group>()
            .HasMany(g => g.Submissions)
            .WithOne(s => s.Group)
            .HasForeignKey(s => s.GroupId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssignmentDefinition>()
           .HasMany(ad => ad.Submissions)
           .WithOne(s => s.AssignmentDefinition)
           .HasForeignKey(s => s.AssignmentDefinitionId)
           .IsRequired()
           .OnDelete(DeleteBehavior.Cascade);
    }

}
