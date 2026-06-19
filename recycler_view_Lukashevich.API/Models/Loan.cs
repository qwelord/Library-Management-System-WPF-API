using System.ComponentModel.DataAnnotations;

namespace recycler_view_Lukashevich.Models;

public class Loan
{
    public int Id { get; set; }

    [Required]
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public DateTime LoanDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    [Required]
    public string Status { get; set; } = "Active";
}