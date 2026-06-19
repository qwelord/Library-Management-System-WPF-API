using System.ComponentModel.DataAnnotations;

namespace recycler_view_Lukashevich.Models;

public class Review
{
    public int Id { get; set; }

    [Required]
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Range(1, 5, ErrorMessage = "Рейтинг должен быть от 1 до 5")]
    public int Rating { get; set; }

    [MaxLength(1000)]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}