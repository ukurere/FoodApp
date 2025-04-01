﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDishModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "DishIngredients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "DishIngredients");
        }
    }
}
