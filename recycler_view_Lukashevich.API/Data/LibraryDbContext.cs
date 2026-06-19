using Microsoft.EntityFrameworkCore;
using recycler_view_Lukashevich.Models;
using System.Reflection.Emit;

namespace recycler_view_Lukashevich.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<BookAuthor> BookAuthors { get; set; }
    public DbSet<BookGenre> BookGenres { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Book>()
            .HasIndex(b => b.ISBN)
            .IsUnique();

        modelBuilder.Entity<Genre>()
            .HasIndex(g => g.Name)
            .IsUnique();

        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        modelBuilder.Entity<BookGenre>()
            .HasKey(bg => new { bg.BookId, bg.GenreId });

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookGenre>()
            .HasOne(bg => bg.Book)
            .WithMany(b => b.BookGenres)
            .HasForeignKey(bg => bg.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookGenre>()
            .HasOne(bg => bg.Genre)
            .WithMany(g => g.BookGenres)
            .HasForeignKey(bg => bg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Book)
            .WithMany(b => b.Loans)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Loan>()
            .HasOne(l => l.User)
            .WithMany(u => u.Loans)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Book)
            .WithMany(b => b.Reviews)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var users = new List<User>
        {
            new User { Id = 1, Email = "admin@library.com", PasswordHash = "admin123", FullName = "Администратор", Role = "Admin", RegistrationDate = new DateTime(2024,1,1) },
            new User { Id = 2, Email = "ivanov@mail.ru", PasswordHash = "pass123", FullName = "Иван Иванов", Role = "User", RegistrationDate = new DateTime(2024,1,5) },
            new User { Id = 3, Email = "petrova@mail.ru", PasswordHash = "pass123", FullName = "Петрова Анна", Role = "User", RegistrationDate = new DateTime(2024,1,10) },
            new User { Id = 4, Email = "sidorov@mail.ru", PasswordHash = "pass123", FullName = "Сидоров Сергей", Role = "User", RegistrationDate = new DateTime(2024,1,15) },
            new User { Id = 5, Email = "kozlov@mail.ru", PasswordHash = "pass123", FullName = "Козлов Дмитрий", Role = "User", RegistrationDate = new DateTime(2024,1,20) },
            new User { Id = 6, Email = "morozova@mail.ru", PasswordHash = "pass123", FullName = "Морозова Елена", Role = "User", RegistrationDate = new DateTime(2024,2,1) },
            new User { Id = 7, Email = "novikov@mail.ru", PasswordHash = "pass123", FullName = "Новиков Андрей", Role = "User", RegistrationDate = new DateTime(2024,2,5) },
            new User { Id = 8, Email = "fedotova@mail.ru", PasswordHash = "pass123", FullName = "Федотова Ольга", Role = "User", RegistrationDate = new DateTime(2024,2,10) },
            new User { Id = 9, Email = "mikhailov@mail.ru", PasswordHash = "pass123", FullName = "Михайлов Павел", Role = "User", RegistrationDate = new DateTime(2024,2,15) },
            new User { Id = 10, Email = "egorova@mail.ru", PasswordHash = "pass123", FullName = "Егорова Наталья", Role = "User", RegistrationDate = new DateTime(2024,3,1) },
            new User { Id = 11, Email = "volkov@mail.ru", PasswordHash = "pass123", FullName = "Волков Алексей", Role = "User", RegistrationDate = new DateTime(2024,3,5) },
            new User { Id = 12, Email = "tarasova@mail.ru", PasswordHash = "pass123", FullName = "Тарасова Ирина", Role = "User", RegistrationDate = new DateTime(2024,3,10) },
            new User { Id = 13, Email = "zaitsev@mail.ru", PasswordHash = "pass123", FullName = "Зайцев Игорь", Role = "User", RegistrationDate = new DateTime(2024,3,15) },
            new User { Id = 14, Email = "sokolova@mail.ru", PasswordHash = "pass123", FullName = "Соколова Мария", Role = "User", RegistrationDate = new DateTime(2024,4,1) },
            new User { Id = 15, Email = "kuzmin@mail.ru", PasswordHash = "pass123", FullName = "Кузьмин Артем", Role = "User", RegistrationDate = new DateTime(2024,4,5) }
        };
        modelBuilder.Entity<User>().HasData(users);

        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Война и мир", Author = "Лев Толстой", ISBN = "978-5-17-123456-7", Year = 1869, Publisher = "Эксмо", Quantity = 5, AvailableQuantity = 5 },
            new Book { Id = 2, Title = "Преступление и наказание", Author = "Фёдор Достоевский", ISBN = "978-5-17-123457-4", Year = 1866, Publisher = "Азбука", Quantity = 3, AvailableQuantity = 2 },
            new Book { Id = 3, Title = "Мастер и Маргарита", Author = "Михаил Булгаков", ISBN = "978-5-17-123458-1", Year = 1967, Publisher = "АСТ", Quantity = 4, AvailableQuantity = 4 },
            new Book { Id = 4, Title = "Анна Каренина", Author = "Лев Толстой", ISBN = "978-5-17-123459-8", Year = 1877, Publisher = "Эксмо", Quantity = 6, AvailableQuantity = 4 },
            new Book { Id = 5, Title = "Идиот", Author = "Фёдор Достоевский", ISBN = "978-5-17-123460-4", Year = 1869, Publisher = "Азбука", Quantity = 3, AvailableQuantity = 3 },
            new Book { Id = 6, Title = "Гарри Поттер и философский камень", Author = "Джоан Роулинг", ISBN = "978-5-17-123461-1", Year = 1997, Publisher = "Росмэн", Quantity = 10, AvailableQuantity = 8 },
            new Book { Id = 7, Title = "Властелин колец", Author = "Джон Р.Р. Толкин", ISBN = "978-5-17-123462-8", Year = 1954, Publisher = "АСТ", Quantity = 7, AvailableQuantity = 6 },
            new Book { Id = 8, Title = "Марсианин", Author = "Энди Уир", ISBN = "978-5-17-123463-5", Year = 2011, Publisher = "Эксмо", Quantity = 5, AvailableQuantity = 5 },
            new Book { Id = 9, Title = "Гордость и предубеждение", Author = "Джейн Остен", ISBN = "978-5-17-123464-2", Year = 1813, Publisher = "Азбука", Quantity = 4, AvailableQuantity = 3 },
            new Book { Id = 10, Title = "Три товарища", Author = "Эрих Мария Ремарк", ISBN = "978-5-17-123465-9", Year = 1936, Publisher = "АСТ", Quantity = 6, AvailableQuantity = 5 },
            new Book { Id = 11, Title = "Маленький принц", Author = "Антуан де Сент-Экзюпери", ISBN = "978-5-17-123466-6", Year = 1943, Publisher = "Эксмо", Quantity = 8, AvailableQuantity = 7 },
            new Book { Id = 12, Title = "Дюна", Author = "Фрэнк Герберт", ISBN = "978-5-17-123467-3", Year = 1965, Publisher = "АСТ", Quantity = 5, AvailableQuantity = 5 },
            new Book { Id = 13, Title = "Ночной дозор", Author = "Сергей Лукьяненко", ISBN = "978-5-17-123468-0", Year = 1998, Publisher = "Эксмо", Quantity = 4, AvailableQuantity = 3 },
            new Book { Id = 14, Title = "Сто лет одиночества", Author = "Габриэль Гарсиа Маркес", ISBN = "978-5-17-123469-7", Year = 1967, Publisher = "Азбука", Quantity = 6, AvailableQuantity = 5 },
            new Book { Id = 15, Title = "Метро 2033", Author = "Дмитрий Глуховский", ISBN = "978-5-17-123470-3", Year = 2005, Publisher = "АСТ", Quantity = 9, AvailableQuantity = 8 }
        };
        modelBuilder.Entity<Book>().HasData(books);

        var authors = new List<Author>
        {
            new Author { Id = 1, FirstName = "Лев", LastName = "Толстой", Biography = "Русский писатель" },
            new Author { Id = 2, FirstName = "Фёдор", LastName = "Достоевский", Biography = "Русский писатель" },
            new Author { Id = 3, FirstName = "Михаил", LastName = "Булгаков", Biography = "Русский писатель" },
            new Author { Id = 4, FirstName = "Джоан", LastName = "Роулинг", Biography = "Британская писательница" },
            new Author { Id = 5, FirstName = "Джон", LastName = "Толкин", Biography = "Английский писатель" },
            new Author { Id = 6, FirstName = "Энди", LastName = "Уир", Biography = "Американский писатель" },
            new Author { Id = 7, FirstName = "Джейн", LastName = "Остен", Biography = "Английская писательница" },
            new Author { Id = 8, FirstName = "Эрих Мария", LastName = "Ремарк", Biography = "Немецкий писатель" },
            new Author { Id = 9, FirstName = "Антуан", LastName = "де Сент-Экзюпери", Biography = "Французский писатель" },
            new Author { Id = 10, FirstName = "Фрэнк", LastName = "Герберт", Biography = "Американский писатель" },
            new Author { Id = 11, FirstName = "Сергей", LastName = "Лукьяненко", Biography = "Русский писатель" },
            new Author { Id = 12, FirstName = "Габриэль Гарсиа", LastName = "Маркес", Biography = "Колумбийский писатель" },
            new Author { Id = 13, FirstName = "Дмитрий", LastName = "Глуховский", Biography = "Русский писатель" },
            new Author { Id = 14, FirstName = "Александр", LastName = "Пушкин", Biography = "Русский поэт" },
            new Author { Id = 15, FirstName = "Николай", LastName = "Гоголь", Biography = "Русский писатель" }
        };
        modelBuilder.Entity<Author>().HasData(authors);

        var genres = new List<Genre>
        {
            new Genre { Id = 1, Name = "Роман", Description = "Литературный жанр" },
            new Genre { Id = 2, Name = "Детектив", Description = "Жанр с расследованием" },
            new Genre { Id = 3, Name = "Фантастика", Description = "Научная фантастика" },
            new Genre { Id = 4, Name = "Фэнтези", Description = "Жанр с магией" },
            new Genre { Id = 5, Name = "Поэзия", Description = "Стихотворные произведения" },
            new Genre { Id = 6, Name = "Драма", Description = "Серьёзные произведения" },
            new Genre { Id = 7, Name = "Комедия", Description = "Юмористические произведения" },
            new Genre { Id = 8, Name = "Приключения", Description = "Приключенческий жанр" },
            new Genre { Id = 9, Name = "Ужасы", Description = "Хоррор" },
            new Genre { Id = 10, Name = "Триллер", Description = "Напряжённый сюжет" },
            new Genre { Id = 11, Name = "Исторический роман", Description = "Историческая проза" },
            new Genre { Id = 12, Name = "Сатира", Description = "Сатирические произведения" },
            new Genre { Id = 13, Name = "Публицистика", Description = "Документальная проза" },
            new Genre { Id = 14, Name = "Биография", Description = "Жизнеописание" },
            new Genre { Id = 15, Name = "Сказка", Description = "Волшебные истории" }
        };
        modelBuilder.Entity<Genre>().HasData(genres);

        var bookAuthors = new List<BookAuthor>
        {
            new BookAuthor { BookId = 1, AuthorId = 1 },
            new BookAuthor { BookId = 4, AuthorId = 1 },
            new BookAuthor { BookId = 2, AuthorId = 2 },
            new BookAuthor { BookId = 5, AuthorId = 2 },
            new BookAuthor { BookId = 3, AuthorId = 3 },
            new BookAuthor { BookId = 6, AuthorId = 4 },
            new BookAuthor { BookId = 7, AuthorId = 5 },
            new BookAuthor { BookId = 8, AuthorId = 6 },
            new BookAuthor { BookId = 9, AuthorId = 7 },
            new BookAuthor { BookId = 10, AuthorId = 8 },
            new BookAuthor { BookId = 11, AuthorId = 9 },
            new BookAuthor { BookId = 12, AuthorId = 10 },
            new BookAuthor { BookId = 13, AuthorId = 11 },
            new BookAuthor { BookId = 14, AuthorId = 12 },
            new BookAuthor { BookId = 15, AuthorId = 13 }
        };
        modelBuilder.Entity<BookAuthor>().HasData(bookAuthors);

        var bookGenres = new List<BookGenre>
        {
            new BookGenre { BookId = 1, GenreId = 1 },
            new BookGenre { BookId = 2, GenreId = 1 },
            new BookGenre { BookId = 3, GenreId = 6 },
            new BookGenre { BookId = 4, GenreId = 1 },
            new BookGenre { BookId = 5, GenreId = 1 },
            new BookGenre { BookId = 6, GenreId = 4 },
            new BookGenre { BookId = 7, GenreId = 4 },
            new BookGenre { BookId = 8, GenreId = 3 },
            new BookGenre { BookId = 9, GenreId = 1 },
            new BookGenre { BookId = 10, GenreId = 1 },
            new BookGenre { BookId = 11, GenreId = 15 },
            new BookGenre { BookId = 12, GenreId = 3 },
            new BookGenre { BookId = 13, GenreId = 4 },
            new BookGenre { BookId = 14, GenreId = 1 },
            new BookGenre { BookId = 15, GenreId = 3 }
        };
        modelBuilder.Entity<BookGenre>().HasData(bookGenres);

        var loans = new List<Loan>
        {
            new Loan { Id = 1, BookId = 1, UserId = 2, LoanDate = new DateTime(2024,4,1), DueDate = new DateTime(2024,4,15), Status = "Active" },
            new Loan { Id = 2, BookId = 2, UserId = 3, LoanDate = new DateTime(2024,4,2), DueDate = new DateTime(2024,4,16), Status = "Active" },
            new Loan { Id = 3, BookId = 3, UserId = 4, LoanDate = new DateTime(2024,3,20), DueDate = new DateTime(2024,4,3), ReturnDate = new DateTime(2024,4,1), Status = "Returned" },
            new Loan { Id = 4, BookId = 4, UserId = 5, LoanDate = new DateTime(2024,3,25), DueDate = new DateTime(2024,4,8), Status = "Overdue" },
            new Loan { Id = 5, BookId = 5, UserId = 6, LoanDate = new DateTime(2024,4,1), DueDate = new DateTime(2024,4,15), Status = "Active" }
        };
        modelBuilder.Entity<Loan>().HasData(loans);

        var reviews = new List<Review>
        {
            new Review { Id = 1, BookId = 1, UserId = 2, Rating = 5, Comment = "Отличная книга!", CreatedAt = new DateTime(2024,4,2) },
            new Review { Id = 2, BookId = 2, UserId = 3, Rating = 4, Comment = "Очень понравилась", CreatedAt = new DateTime(2024,4,3) },
            new Review { Id = 3, BookId = 6, UserId = 4, Rating = 5, Comment = "Лучшая книга!", CreatedAt = new DateTime(2024,4,1) },
            new Review { Id = 4, BookId = 7, UserId = 5, Rating = 3, Comment = "Неплохо, но затянуто", CreatedAt = new DateTime(2024,3,28) },
            new Review { Id = 5, BookId = 8, UserId = 6, Rating = 4, Comment = "Интересная идея", CreatedAt = new DateTime(2024,4,4) }
        };
        modelBuilder.Entity<Review>().HasData(reviews);
    }
}