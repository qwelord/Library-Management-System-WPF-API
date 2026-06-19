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

    public MainWindow(ApiClient apiClient, AuthService authService)
    {
        InitializeComponent();
        _apiClient = apiClient;
        _authService = authService;
        DataContext = this;

        ViewModel.CurrentUser = _authService.CurrentUser?.FullName ?? "Гость";
        ViewModel.Role = _authService.CurrentUser?.Role ?? "User";
        ViewModel.IsAdmin = ViewModel.Role == "Admin";

        BooksViewModel.ApiClient = _apiClient;
        BooksViewModel.IsAdmin = ViewModel.IsAdmin;
        BooksViewModel.LoadBooksCommand = new RelayCommand(async _ => await BooksViewModel.LoadBooks());
        BooksViewModel.LoadBooksCommand.Execute(null);
    }

    private async void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        _authService.Logout();
        var loginWindow = new LoginWindow(_apiClient, _authService);
        loginWindow.Show();
        this.Close();
    }
}