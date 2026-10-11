using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Streambit.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonTmdbFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Adult",
                table: "People",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "People",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "KnownForDepartment",
                table: "People",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Popularity",
                table: "People",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePath",
                table: "People",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PersonKnownFor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MediaType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Adult = table.Column<bool>(type: "boolean", nullable: false),
                    BackdropPath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    PosterPath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    OriginalLanguage = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    OriginalTitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    OriginalName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Overview = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FirstAirDate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Video = table.Column<bool>(type: "boolean", nullable: false),
                    VoteAverage = table.Column<double>(type: "double precision", nullable: false),
                    VoteCount = table.Column<int>(type: "integer", nullable: false),
                    GenreIds = table.Column<List<int>>(type: "integer[]", nullable: false),
                    OriginCountries = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonKnownFor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonKnownFor_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonSources",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    SourceId = table.Column<int>(type: "integer", nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonSources", x => new { x.PersonId, x.SourceId });
                    table.ForeignKey(
                        name: "FK_PersonSources_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonSources_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonKnownFor_PersonId_MediaType_ExternalId",
                table: "PersonKnownFor",
                columns: new[] { "PersonId", "MediaType", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonKnownFor_PersonId_SortOrder",
                table: "PersonKnownFor",
                columns: new[] { "PersonId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonSources_SourceId_ExternalId",
                table: "PersonSources",
                columns: new[] { "SourceId", "ExternalId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonKnownFor");

            migrationBuilder.DropTable(
                name: "PersonSources");

            migrationBuilder.DropColumn(
                name: "Adult",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "People");

            migrationBuilder.DropColumn(
                name: "KnownForDepartment",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "People");

            migrationBuilder.DropColumn(
                name: "ProfilePath",
                table: "People");
        }
    }
}
