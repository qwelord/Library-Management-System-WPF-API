namespace recycler_view_Lukashevich.Models;

public class LoanModel
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public BookModel? Book { get; set; }
    public UserModel? User { get; set; }
}