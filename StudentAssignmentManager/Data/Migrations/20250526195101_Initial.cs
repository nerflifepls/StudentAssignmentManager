using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentAssignmentManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "IdentityUserToken`1");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "IdentityUser");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "IdentityUserRole`1");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "IdentityUserLogin`1");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "IdentityUserClaim`1");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "IdentityRole");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "IdentityRoleClaim`1");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "IdentityUserRole`1",
                newName: "IX_IdentityUserRole`1_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "IdentityUserLogin`1",
                newName: "IX_IdentityUserLogin`1_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "IdentityUserClaim`1",
                newName: "IX_IdentityUserClaim`1_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "IdentityRoleClaim`1",
                newName: "IX_IdentityRoleClaim`1_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserToken`1",
                table: "IdentityUserToken`1",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUser",
                table: "IdentityUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserRole`1",
                table: "IdentityUserRole`1",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserLogin`1",
                table: "IdentityUserLogin`1",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserClaim`1",
                table: "IdentityUserClaim`1",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRole",
                table: "IdentityRole",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRoleClaim`1",
                table: "IdentityRoleClaim`1",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AssignmentDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxPoints = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Submission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    AssignmentDefinitionId = table.Column<int>(type: "int", nullable: false),
                    SubmissionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Grade = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submission_AssignmentDefinition_AssignmentDefinitionId",
                        column: x => x.AssignmentDefinitionId,
                        principalTable: "AssignmentDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submission_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMember",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMember", x => new { x.StudentId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GroupMember_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupMember_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_GroupId",
                table: "GroupMember",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAssignmentUnique",
                table: "Submission",
                columns: new[] { "GroupId", "AssignmentDefinitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submission_AssignmentDefinitionId",
                table: "Submission",
                column: "AssignmentDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim`1_IdentityRole_RoleId",
                table: "IdentityRoleClaim`1",
                column: "RoleId",
                principalTable: "IdentityRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim`1_IdentityUser_UserId",
                table: "IdentityUserClaim`1",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin`1_IdentityUser_UserId",
                table: "IdentityUserLogin`1",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole`1_IdentityRole_RoleId",
                table: "IdentityUserRole`1",
                column: "RoleId",
                principalTable: "IdentityRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole`1_IdentityUser_UserId",
                table: "IdentityUserRole`1",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserToken`1_IdentityUser_UserId",
                table: "IdentityUserToken`1",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityRoleClaim`1_IdentityRole_RoleId",
                table: "IdentityRoleClaim`1");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserClaim`1_IdentityUser_UserId",
                table: "IdentityUserClaim`1");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserLogin`1_IdentityUser_UserId",
                table: "IdentityUserLogin`1");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserRole`1_IdentityRole_RoleId",
                table: "IdentityUserRole`1");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserRole`1_IdentityUser_UserId",
                table: "IdentityUserRole`1");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserToken`1_IdentityUser_UserId",
                table: "IdentityUserToken`1");

            migrationBuilder.DropTable(
                name: "GroupMember");

            migrationBuilder.DropTable(
                name: "Submission");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "AssignmentDefinition");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserToken`1",
                table: "IdentityUserToken`1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserRole`1",
                table: "IdentityUserRole`1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserLogin`1",
                table: "IdentityUserLogin`1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserClaim`1",
                table: "IdentityUserClaim`1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUser",
                table: "IdentityUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRoleClaim`1",
                table: "IdentityRoleClaim`1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRole",
                table: "IdentityRole");

            migrationBuilder.RenameTable(
                name: "IdentityUserToken`1",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "IdentityUserRole`1",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "IdentityUserLogin`1",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim`1",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "IdentityUser",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "IdentityRoleClaim`1",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "IdentityRole",
                newName: "AspNetRoles");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityUserRole`1_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityUserLogin`1_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityUserClaim`1_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityRoleClaim`1_RoleId",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
