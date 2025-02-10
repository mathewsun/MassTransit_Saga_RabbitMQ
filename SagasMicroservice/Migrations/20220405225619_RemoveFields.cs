using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SagasMicroservice.Migrations
{
    public partial class RemoveFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "BuyItemsSagaState");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "BuyItemsSagaState");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "BuyItemsSagaState",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "BuyItemsSagaState",
                type: "integer",
                nullable: true);
        }
    }
}
