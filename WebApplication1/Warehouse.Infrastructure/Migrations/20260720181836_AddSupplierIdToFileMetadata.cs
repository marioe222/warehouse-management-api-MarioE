using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierIdToFileMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileMetadata_Products_ProductId",
                table: "FileMetadata");

            migrationBuilder.DropIndex(
                name: "IX_FileMetadata_ProductId",
                table: "FileMetadata");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "FileMetadata",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "FileMetadata");

            migrationBuilder.CreateIndex(
                name: "IX_FileMetadata_ProductId",
                table: "FileMetadata",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileMetadata_Products_ProductId",
                table: "FileMetadata",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
