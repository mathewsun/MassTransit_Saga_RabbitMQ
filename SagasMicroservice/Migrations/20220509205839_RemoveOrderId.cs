using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SagasMicroservice.Migrations
{
    public partial class RemoveOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "BuyItemsSagaState");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "BuyItemsSagaState",
                type: "uuid",
                nullable: true);
        }
    }
}
