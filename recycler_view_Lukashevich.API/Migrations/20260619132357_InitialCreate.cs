using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace recycler_view_Lukashevich.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Biography = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Author = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ISBN = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Publisher = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegistrationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BookAuthors",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthors", x => new { x.BookId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_BookAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthors_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BookGenres",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenres", x => new { x.BookId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_BookGenres_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Biography", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Русский писатель", "Лев", "Толстой" },
                    { 2, "Русский писатель", "Фёдор", "Достоевский" },
                    { 3, "Русский писатель", "Михаил", "Булгаков" },
                    { 4, "Британская писательница", "Джоан", "Роулинг" },
                    { 5, "Английский писатель", "Джон", "Толкин" },
                    { 6, "Американский писатель", "Энди", "Уир" },
                    { 7, "Английская писательница", "Джейн", "Остен" },
                    { 8, "Немецкий писатель", "Эрих Мария", "Ремарк" },
                    { 9, "Французский писатель", "Антуан", "де Сент-Экзюпери" },
                    { 10, "Американский писатель", "Фрэнк", "Герберт" },
                    { 11, "Русский писатель", "Сергей", "Лукьяненко" },
                    { 12, "Колумбийский писатель", "Габриэль Гарсиа", "Маркес" },
                    { 13, "Русский писатель", "Дмитрий", "Глуховский" },
                    { 14, "Русский поэт", "Александр", "Пушкин" },
                    { 15, "Русский писатель", "Николай", "Гоголь" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AvailableQuantity", "ISBN", "Publisher", "Quantity", "Title", "Year" },
                values: new object[,]
                {
                    { 1, "Лев Толстой", 5, "978-5-17-123456-7", "Эксмо", 5, "Война и мир", 1869 },
                    { 2, "Фёдор Достоевский", 2, "978-5-17-123457-4", "Азбука", 3, "Преступление и наказание", 1866 },
                    { 3, "Михаил Булгаков", 4, "978-5-17-123458-1", "АСТ", 4, "Мастер и Маргарита", 1967 },
                    { 4, "Лев Толстой", 4, "978-5-17-123459-8", "Эксмо", 6, "Анна Каренина", 1877 },
                    { 5, "Фёдор Достоевский", 3, "978-5-17-123460-4", "Азбука", 3, "Идиот", 1869 },
                    { 6, "Джоан Роулинг", 8, "978-5-17-123461-1", "Росмэн", 10, "Гарри Поттер и философский камень", 1997 },
                    { 7, "Джон Р.Р. Толкин", 6, "978-5-17-123462-8", "АСТ", 7, "Властелин колец", 1954 },
                    { 8, "Энди Уир", 5, "978-5-17-123463-5", "Эксмо", 5, "Марсианин", 2011 },
                    { 9, "Джейн Остен", 3, "978-5-17-123464-2", "Азбука", 4, "Гордость и предубеждение", 1813 },
                    { 10, "Эрих Мария Ремарк", 5, "978-5-17-123465-9", "АСТ", 6, "Три товарища", 1936 },
                    { 11, "Антуан де Сент-Экзюпери", 7, "978-5-17-123466-6", "Эксмо", 8, "Маленький принц", 1943 },
                    { 12, "Фрэнк Герберт", 5, "978-5-17-123467-3", "АСТ", 5, "Дюна", 1965 },
                    { 13, "Сергей Лукьяненко", 3, "978-5-17-123468-0", "Эксмо", 4, "Ночной дозор", 1998 },
                    { 14, "Габриэль Гарсиа Маркес", 5, "978-5-17-123469-7", "Азбука", 6, "Сто лет одиночества", 1967 },
                    { 15, "Дмитрий Глуховский", 8, "978-5-17-123470-3", "АСТ", 9, "Метро 2033", 2005 }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Литературный жанр", "Роман" },
                    { 2, "Жанр с расследованием", "Детектив" },
                    { 3, "Научная фантастика", "Фантастика" },
                    { 4, "Жанр с магией", "Фэнтези" },
                    { 5, "Стихотворные произведения", "Поэзия" },
                    { 6, "Серьёзные произведения", "Драма" },
                    { 7, "Юмористические произведения", "Комедия" },
                    { 8, "Приключенческий жанр", "Приключения" },
                    { 9, "Хоррор", "Ужасы" },
                    { 10, "Напряжённый сюжет", "Триллер" },
                    { 11, "Историческая проза", "Исторический роман" },
                    { 12, "Сатирические произведения", "Сатира" },
                    { 13, "Документальная проза", "Публицистика" },
                    { 14, "Жизнеописание", "Биография" },
                    { 15, "Волшебные истории", "Сказка" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "PasswordHash", "RegistrationDate", "Role" },
                values: new object[,]
                {
                    { 1, "admin@library.com", "Администратор", "admin123", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin" },
                    { 2, "ivanov@mail.ru", "Иван Иванов", "pass123", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 3, "petrova@mail.ru", "Петрова Анна", "pass123", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 4, "sidorov@mail.ru", "Сидоров Сергей", "pass123", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 5, "kozlov@mail.ru", "Козлов Дмитрий", "pass123", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 6, "morozova@mail.ru", "Морозова Елена", "pass123", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 7, "novikov@mail.ru", "Новиков Андрей", "pass123", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 8, "fedotova@mail.ru", "Федотова Ольга", "pass123", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 9, "mikhailov@mail.ru", "Михайлов Павел", "pass123", new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 10, "egorova@mail.ru", "Егорова Наталья", "pass123", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 11, "volkov@mail.ru", "Волков Алексей", "pass123", new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 12, "tarasova@mail.ru", "Тарасова Ирина", "pass123", new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 13, "zaitsev@mail.ru", "Зайцев Игорь", "pass123", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 14, "sokolova@mail.ru", "Соколова Мария", "pass123", new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" },
                    { 15, "kuzmin@mail.ru", "Кузьмин Артем", "pass123", new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" }
                });

            migrationBuilder.InsertData(
                table: "BookAuthors",
                columns: new[] { "AuthorId", "BookId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 1, 4 },
                    { 2, 5 },
                    { 4, 6 },
                    { 5, 7 },
                    { 6, 8 },
                    { 7, 9 },
                    { 8, 10 },
                    { 9, 11 },
                    { 10, 12 },
                    { 11, 13 },
                    { 12, 14 },
                    { 13, 15 }
                });

            migrationBuilder.InsertData(
                table: "BookGenres",
                columns: new[] { "BookId", "GenreId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 6 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 4 },
                    { 7, 4 },
                    { 8, 3 },
                    { 9, 1 },
                    { 10, 1 },
                    { 11, 15 },
                    { 12, 3 },
                    { 13, 4 },
                    { 14, 1 },
                    { 15, 3 }
                });

            migrationBuilder.InsertData(
                table: "Loans",
                columns: new[] { "Id", "BookId", "DueDate", "LoanDate", "ReturnDate", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 2 },
                    { 2, 2, new DateTime(2024, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 3 },
                    { 3, 3, new DateTime(2024, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Returned", 4 },
                    { 4, 4, new DateTime(2024, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Overdue", 5 },
                    { 5, 5, new DateTime(2024, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 6 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "BookId", "Comment", "CreatedAt", "Rating", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "Отличная книга!", new DateTime(2024, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2 },
                    { 2, 2, "Очень понравилась", new DateTime(2024, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 3 },
                    { 3, 6, "Лучшая книга!", new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 4 },
                    { 4, 7, "Неплохо, но затянуто", new DateTime(2024, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 5 },
                    { 5, 8, "Интересная идея", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_AuthorId",
                table: "BookAuthors",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_GenreId",
                table: "BookGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ISBN",
                table: "Books",
                column: "ISBN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                table: "Genres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BookId",
                table: "Loans",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserId",
                table: "Loans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookId",
                table: "Reviews",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthors");

            migrationBuilder.DropTable(
                name: "BookGenres");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
