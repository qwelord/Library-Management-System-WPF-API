using System.Windows;
using recycler_view_Lukashevich.Services;
using recycler_view_Lukashevich.ViewModels;

namespace recycler_view_Lukashevich.Views;

public partial class MainWindow : Window
{
    private readonly ApiClient _apiClient;
    private readonly AuthService _authService;
    public MainViewModel ViewModel { get; } = new();
    public BooksViewModel BooksViewModel { get; } = new();
    public UsersViewModel UsersViewModel { get; } = new();
    public LoansViewModel LoansViewModel { get; } = new();

    public MainWindow(ApiClient apiClient, AuthService authService)
    {
        InitializeComponent();
        _apiClient = apiClient;
        _authService = authService;
        DataContext = this;

        var currentUser = _authService.CurrentUser;
        ViewModel.CurrentUser = currentUser?.FullName ?? "Гость";
        ViewModel.Role = currentUser?.Role ?? "User";
        ViewModel.IsAdmin = ViewModel.Role == "Admin" || ViewModel.Role == "Librarian";

        BooksViewModel.ApiClient = _apiClient;
        BooksViewModel.IsAdmin = ViewModel.IsAdmin;
        BooksViewModel.LoadBooksCommand = new RelayCommand(async (param) =>
        {
            string? search = param as string;
            await BooksViewModel.LoadBooks(search);
        });
        BooksViewModel.LoadBooksCommand.Execute(null);

        UsersViewModel.ApiClient = _apiClient;
        UsersViewModel.LoadUsersCommand = new RelayCommand(async (param) =>
        {
            string? search = param as string;
            await UsersViewModel.LoadUsers(search);
        });
        UsersViewModel.LoadUsersCommand.Execute(null);

        LoansViewModel.ApiClient = _apiClient;
        LoansViewModel.IsAdmin = ViewModel.IsAdmin;
        LoansViewModel.CurrentUserId = currentUser?.Id ?? 0;
        LoansViewModel.LoadLoansCommand = new RelayCommand(async (param) =>
        {
            string? search = param as string;
            await LoansViewModel.LoadLoans(search);
        });
        LoansViewModel.LoadLoansCommand.Execute(null);
    }

    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        _authService.Logout();
        var loginWindow = new LoginWindow(_apiClient, _authService);
        loginWindow.Show();
        Close();
    }
}