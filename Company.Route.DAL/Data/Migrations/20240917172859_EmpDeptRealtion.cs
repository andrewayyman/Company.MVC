using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.Route.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmpDeptRealtion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkForID",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_WorkForID",
                table: "Employees",
                column: "WorkForID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_WorkForID",
                table: "Employees",
                column: "WorkForID",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_WorkForID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_WorkForID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WorkForID",
                table: "Employees");
        }
    }
}
