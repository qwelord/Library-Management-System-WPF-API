using System.Windows;
using recycler_view_Lukashevich.Services;
using recycler_view_Lukashevich.Views;

namespace recycler_view_Lukashevich;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var apiBaseUrl = "http://localhost:5092/api/";
        var apiClient = new ApiClient(apiBaseUrl);
        var authService = new AuthService(apiClient);

        var loginWindow = new LoginWindow(apiClient, authService);
        loginWindow.Show();
    }
}