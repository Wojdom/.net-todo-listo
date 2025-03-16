using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Migrations
{
    /// <inheritdoc />
    public partial class Renaming_next_part : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TasksLists_TodoListId",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "TodoListId",
                table: "Todos",
                newName: "TasksListId");

            migrationBuilder.RenameIndex(
                name: "IX_Todos_TodoListId",
                table: "Todos",
                newName: "IX_Todos_TasksListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TasksLists_TasksListId",
                table: "Todos",
                column: "TasksListId",
                principalTable: "TasksLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TasksLists_TasksListId",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "TasksListId",
                table: "Todos",
                newName: "TodoListId");

            migrationBuilder.RenameIndex(
                name: "IX_Todos_TasksListId",
                table: "Todos",
                newName: "IX_Todos_TodoListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TasksLists_TodoListId",
                table: "Todos",
                column: "TodoListId",
                principalTable: "TasksLists",
                principalColumn: "Id");
        }
    }
}
