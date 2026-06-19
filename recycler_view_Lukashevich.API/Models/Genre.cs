using System.ComponentModel.DataAnnotations;

namespace recycler_view_Lukashevich.Models;

public class Genre
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Description { get; set; }

    public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}