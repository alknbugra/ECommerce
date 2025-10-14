using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWishlistSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ListType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsShareable = table.Column<bool>(type: "bit", nullable: false),
                    ShareCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PriceTrackingEnabled = table.Column<bool>(type: "bit", nullable: false),
                    StockTrackingEnabled = table.Column<bool>(type: "bit", nullable: false),
                    EmailNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wishlists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WishlistItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WishlistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceAtTime = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountedPriceAtTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WasInStock = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    TargetPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PriceTrackingEnabled = table.Column<bool>(type: "bit", nullable: false),
                    StockTrackingEnabled = table.Column<bool>(type: "bit", nullable: false),
                    EmailNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LastPriceNotificationAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastStockNotificationAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WishlistItems_Wishlists_WishlistId",
                        column: x => x.WishlistId,
                        principalTable: "Wishlists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WishlistShares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WishlistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SharedWithUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShareType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShareCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExpirationDays = table.Column<int>(type: "int", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    LastViewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistShares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishlistShares_Users_SharedWithUserId",
                        column: x => x.SharedWithUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WishlistShares_Wishlists_WishlistId",
                        column: x => x.WishlistId,
                        principalTable: "Wishlists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WishlistItemPriceHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WishlistItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceChangePercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ChangeType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ChangeReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistItemPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishlistItemPriceHistories_WishlistItems_WishlistItemId",
                        column: x => x.WishlistItemId,
                        principalTable: "WishlistItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WishlistItemStockHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WishlistItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldStockStatus = table.Column<bool>(type: "bit", nullable: false),
                    NewStockStatus = table.Column<bool>(type: "bit", nullable: false),
                    OldStockQuantity = table.Column<int>(type: "int", nullable: false),
                    NewStockQuantity = table.Column<int>(type: "int", nullable: false),
                    ChangeType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ChangeReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistItemStockHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishlistItemStockHistories_WishlistItems_WishlistItemId",
                        column: x => x.WishlistItemId,
                        principalTable: "WishlistItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItemPriceHistories_CreatedAt",
                table: "WishlistItemPriceHistories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItemPriceHistories_WishlistItemId",
                table: "WishlistItemPriceHistories",
                column: "WishlistItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_CreatedAt",
                table: "WishlistItems",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_Priority",
                table: "WishlistItems",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_ProductId",
                table: "WishlistItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_WishlistId",
                table: "WishlistItems",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItemStockHistories_CreatedAt",
                table: "WishlistItemStockHistories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItemStockHistories_WishlistItemId",
                table: "WishlistItemStockHistories",
                column: "WishlistItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_CreatedAt",
                table: "Wishlists",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_IsDefault",
                table: "Wishlists",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_ShareCode",
                table: "Wishlists",
                column: "ShareCode");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistShares_CreatedAt",
                table: "WishlistShares",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistShares_ShareCode",
                table: "WishlistShares",
                column: "ShareCode");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistShares_SharedWithUserId",
                table: "WishlistShares",
                column: "SharedWithUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistShares_WishlistId",
                table: "WishlistShares",
                column: "WishlistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WishlistItemPriceHistories");

            migrationBuilder.DropTable(
                name: "WishlistItemStockHistories");

            migrationBuilder.DropTable(
                name: "WishlistShares");

            migrationBuilder.DropTable(
                name: "WishlistItems");

            migrationBuilder.DropTable(
                name: "Wishlists");
        }
    }
}
