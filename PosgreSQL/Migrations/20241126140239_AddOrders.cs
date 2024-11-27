using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerGame_Customers_CustomerId",
                table: "CustomerGame");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerGame_Games_GamesId",
                table: "CustomerGame");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerGame",
                table: "CustomerGame");

            migrationBuilder.RenameTable(
                name: "CustomerGame",
                newName: "CustomerGames");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerGame_GamesId",
                table: "CustomerGames",
                newName: "IX_CustomerGames_GamesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerGames",
                table: "CustomerGames",
                columns: new[] { "CustomerId", "GamesId" });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderGames",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "integer", nullable: false),
                    OrdersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderGames", x => new { x.GamesId, x.OrdersId });
                    table.ForeignKey(
                        name: "FK_OrderGames_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderGames_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderGames_OrdersId",
                table: "OrderGames",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerGames_Customers_CustomerId",
                table: "CustomerGames",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerGames_Games_GamesId",
                table: "CustomerGames",
                column: "GamesId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerGames_Customers_CustomerId",
                table: "CustomerGames");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerGames_Games_GamesId",
                table: "CustomerGames");

            migrationBuilder.DropTable(
                name: "OrderGames");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerGames",
                table: "CustomerGames");

            migrationBuilder.RenameTable(
                name: "CustomerGames",
                newName: "CustomerGame");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerGames_GamesId",
                table: "CustomerGame",
                newName: "IX_CustomerGame_GamesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerGame",
                table: "CustomerGame",
                columns: new[] { "CustomerId", "GamesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerGame_Customers_CustomerId",
                table: "CustomerGame",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerGame_Games_GamesId",
                table: "CustomerGame",
                column: "GamesId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
