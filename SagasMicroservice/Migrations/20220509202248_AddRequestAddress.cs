using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SagasMicroservice.Migrations
{
    public partial class AddRequestAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RequestId",
                table: "BuyItemsSagaState",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseAddress",
                table: "BuyItemsSagaState",
                type: "text",
                nullable: true,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "BuyItemsSagaState");

            migrationBuilder.DropColumn(
                name: "ResponseAddress",
                table: "BuyItemsSagaState");
        }
    }
}
