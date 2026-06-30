using System.Windows;
using recycler_view_Lukashevich.Services;
using recycler_view_Lukashevich.Views;

namespace recycler_view_Lukashevich;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // БЕРУ ИЗ SWAGGER — ТОЧНО ТАКОЙ ЖЕ
        var apiBaseUrl = "http://localhost:5092/";
        var apiClient = new ApiClient(apiBaseUrl);
        var authService = new AuthService(apiClient);

        var loginWindow = new LoginWindow(apiClient, authService);
        loginWindow.Show();
    }
}