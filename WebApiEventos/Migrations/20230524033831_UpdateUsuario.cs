using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiEventos.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Usuarios_AdministradorId",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_AdministradorId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "AdministradorId",
                table: "Eventos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdministradorId",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_AdministradorId",
                table: "Eventos",
                column: "AdministradorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Usuarios_AdministradorId",
                table: "Eventos",
                column: "AdministradorId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
