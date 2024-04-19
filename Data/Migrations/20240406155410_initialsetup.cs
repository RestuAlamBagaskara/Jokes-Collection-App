using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JokesApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialsetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Joke",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JokesQuestion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JokesAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(nullable: false, maxLength: 450),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Joke", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jokes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Joke");
        }
    }
}
