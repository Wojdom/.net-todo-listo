using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Migrations
{
    /// <inheritdoc />
    public partial class Renaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoLists_TodoListId",
                table: "Todos");

            migrationBuilder.DropTable(
                name: "TodoLists");

            migrationBuilder.CreateTable(
                name: "TasksLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasksLists", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TasksLists_TodoListId",
                table: "Todos",
                column: "TodoListId",
                principalTable: "TasksLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TasksLists_TodoListId",
                table: "Todos");

            migrationBuilder.DropTable(
                name: "TasksLists");

            migrationBuilder.CreateTable(
                name: "TodoLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoLists", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoLists_TodoListId",
                table: "Todos",
                column: "TodoListId",
                principalTable: "TodoLists",
                principalColumn: "Id");
        }
    }
}
