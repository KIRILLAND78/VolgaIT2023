using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VolgaIT2023.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    Balance = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    ExpireTime = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false),
                    CanBeRented = table.Column<bool>(type: "boolean", nullable: false),
                    TransportType = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Identifier = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    MinutePrice = table.Column<double>(type: "double precision", nullable: true),
                    DayPrice = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transports", x => x.Id);
                    table.CheckConstraint("Transports", "\"TransportType\" IN ('Bike','Car','Scooter')");
                    table.ForeignKey(
                        name: "FK_Transports_Accounts_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransportId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TimeStart = table.Column<string>(type: "text", nullable: false),
                    TimeEnd = table.Column<string>(type: "text", nullable: true),
                    PriceOfUnit = table.Column<double>(type: "double precision", nullable: false),
                    PriceType = table.Column<string>(type: "text", nullable: false),
                    FinalPrice = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rents", x => x.Id);
                    table.CheckConstraint("Rents", "\"PriceType\" IN ('Days','Minutes')");
                    table.ForeignKey(
                        name: "FK_Rents_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rents_Transports_TransportId",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Username",
                table: "Accounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rents_TransportId",
                table: "Rents",
                column: "TransportId");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_UserId",
                table: "Rents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_AccountId",
                table: "Sessions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transports_OwnerId",
                table: "Transports",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rents");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Transports");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
