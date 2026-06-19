using System.Text.Json;
using System.Windows;
using recycler_view_Lukashevich.Services;
using recycler_view_Lukashevich.ViewModels;

namespace recycler_view_Lukashevich.Views;

public partial class LoginWindow : Window
{
    private readonly ApiClient _apiClient;
    private readonly AuthService _authService;
    public LoginViewModel ViewModel { get; } = new();

    public LoginWindow(ApiClient apiClient, AuthService authService)
    {
        InitializeComponent();
        _apiClient = apiClient;
        _authService = authService;
        DataContext = ViewModel;
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        var email = ViewModel.Email;
        var password = PasswordBox.Password;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ViewModel.ErrorMessage = "Заполните все поля";
            ViewModel.IsErrorVisible = true;
            return;
        }

        ViewModel.IsErrorVisible = false;
        ViewModel.IsLoading = true;

        try
        {
            var result = await _apiClient.PostAsync<JsonElement>("auth/login", new { Email = email, Password = password });

            if (result.ValueKind != JsonValueKind.Undefined)
            {
                string token = result.GetProperty("token").GetString();
                int userId = result.GetProperty("id").GetInt32();
                string fullName = result.GetProperty("fullName").GetString();
                string role = result.GetProperty("role").GetString();

                _authService.SetToken(token, userId, email, fullName, role);

                var mainWindow = new MainWindow(_apiClient, _authService);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ViewModel.ErrorMessage = "Не удалось получить ответ от сервера";
                ViewModel.IsErrorVisible = true;
            }
        }
        catch (Exception ex)
        {
            ViewModel.ErrorMessage = "Ошибка входа: " + ex.Message;
            ViewModel.IsErrorVisible = true;
        }
        finally
        {
            ViewModel.IsLoading = false;
        }
    }
}