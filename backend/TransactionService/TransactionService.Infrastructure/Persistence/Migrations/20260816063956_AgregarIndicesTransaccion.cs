using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregarIndicesTransaccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_Fecha",
                schema: "transactions",
                table: "Transaccion",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_ProductoId",
                schema: "transactions",
                table: "Transaccion",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_Tipo",
                schema: "transactions",
                table: "Transaccion",
                column: "Tipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transaccion_Fecha",
                schema: "transactions",
                table: "Transaccion");

            migrationBuilder.DropIndex(
                name: "IX_Transaccion_ProductoId",
                schema: "transactions",
                table: "Transaccion");

            migrationBuilder.DropIndex(
                name: "IX_Transaccion_Tipo",
                schema: "transactions",
                table: "Transaccion");
        }
    }
}
