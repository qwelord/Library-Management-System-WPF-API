using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace recycler_view_Lukashevich.Models;

public class Book
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название обязательно")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Автор обязателен")]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "ISBN обязателен")]
    [MaxLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Range(0, 3000, ErrorMessage = "Год должен быть от 0 до 3000")]
    public int Year { get; set; }

    [MaxLength(100)]
    public string Publisher { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
    public int Quantity { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
    public int AvailableQuantity { get; set; }

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}