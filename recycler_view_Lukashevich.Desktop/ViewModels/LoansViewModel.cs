using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using recycler_view_Lukashevich.Models;
using recycler_view_Lukashevich.Services;

namespace recycler_view_Lukashevich.ViewModels;

public class LoansViewModel : INotifyPropertyChanged
{
    private ApiClient? _apiClient;
    public ApiClient? ApiClient
    {
        get => _apiClient;
        set { _apiClient = value; OnPropertyChanged(); }
    }

    private ObservableCollection<LoanModel> _loans = new();
    public ObservableCollection<LoanModel> Loans
    {
        get => _loans;
        set { _loans = value; OnPropertyChanged(); }
    }

    private LoanModel? _selectedLoan;
    public LoanModel? SelectedLoan
    {
        get => _selectedLoan;
        set { _selectedLoan = value; OnPropertyChanged(); }
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

    private int _currentUserId;
    public int CurrentUserId
    {
        get => _currentUserId;
        set { _currentUserId = value; OnPropertyChanged(); }
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    private bool _hasError;
    public bool HasError
    {
        get => _hasError;
        set { _hasError = value; OnPropertyChanged(); }
    }

    public ICommand? LoadLoansCommand { get; set; }
    public ICommand? ReturnBookCommand { get; set; }

    public async Task LoadLoans(string? search = null)
    {
        if (ApiClient == null) return;
        IsLoading = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            string endpoint = IsAdmin ? "loans" : $"loans/user/{CurrentUserId}";
            var loans = await ApiClient.GetAsync<List<LoanModel>>(endpoint);
            if (loans != null)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    loans = loans.Where(l =>
                        (l.Book?.Title?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (l.User?.FullName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (l.Status?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
                }

                Loans.Clear();
                foreach (var loan in loans)
                    Loans.Add(loan);
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = "Ошибка загрузки выдач: " + ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task ReturnBook(int loanId)
    {
        if (ApiClient == null) return;
        IsLoading = true;
        try
        {
            await ApiClient.PutAsync<object>($"loans/{loanId}/return", new { });
            await LoadLoans(SearchText);
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = "Ошибка возврата книги: " + ex.Message;
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