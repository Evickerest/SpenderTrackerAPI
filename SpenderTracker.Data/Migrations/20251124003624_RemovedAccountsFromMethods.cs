using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpenderTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedAccountsFromMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionMethods_Accounts_AccountId",
                table: "TransactionMethods");

            migrationBuilder.DropIndex(
                name: "IX_TransactionMethods_AccountId",
                table: "TransactionMethods");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "TransactionMethods");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "TransactionMethods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionMethods_AccountId",
                table: "TransactionMethods",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionMethods_Accounts_AccountId",
                table: "TransactionMethods",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
