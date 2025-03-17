using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Migrations
{
    /// <inheritdoc />
    public partial class TasksList_CascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TasksLists_TasksListId",
                table: "Todos");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TasksLists_TasksListId",
                table: "Todos",
                column: "TasksListId",
                principalTable: "TasksLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TasksLists_TasksListId",
                table: "Todos");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TasksLists_TasksListId",
                table: "Todos",
                column: "TasksListId",
                principalTable: "TasksLists",
                principalColumn: "Id");
        }
    }
}
