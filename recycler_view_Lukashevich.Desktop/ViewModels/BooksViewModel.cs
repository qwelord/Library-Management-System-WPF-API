using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using recycler_view_Lukashevich.Models;
using recycler_view_Lukashevich.Services;

namespace recycler_view_Lukashevich.ViewModels;

public class BooksViewModel : INotifyPropertyChanged
{
    private ApiClient? _apiClient;
    public ApiClient? ApiClient
    {
        get => _apiClient;
        set { _apiClient = value; OnPropertyChanged(); }
    }

    private ObservableCollection<BookModel> _books = new();
    public ObservableCollection<BookModel> Books
    {
        get => _books;
        set { _books = value; OnPropertyChanged(); }
    }

    private BookModel? _selectedBook;
    public BookModel? SelectedBook
    {
        get => _selectedBook;
        set { _selectedBook = value; OnPropertyChanged(); }
    }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set { _searchText = value; OnPropertyChanged(); }
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    private bool _isAdmin;
    public bool IsAdmin
    {
        get => _isAdmin;
        set { _isAdmin = value; OnPropertyChanged(); }
    }

    public ICommand? LoadBooksCommand { get; set; }

    public async Task LoadBooks(string? search = null)
    {
        if (ApiClient == null) return;
        IsLoading = true;
        try
        {
            var books = await ApiClient.GetAsync<List<BookModel>>("books");
            if (books != null)
            {
                if (!string.IsNullOrEmpty(search))
                    books = books.Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                              b.Author.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                Books.Clear();
                foreach (var book in books)
                    Books.Add(book);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}