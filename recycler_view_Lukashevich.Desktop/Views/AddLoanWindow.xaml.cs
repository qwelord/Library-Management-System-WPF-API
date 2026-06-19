using System.Windows;
using System.Windows.Controls;
using recycler_view_Lukashevich.Models;
using recycler_view_Lukashevich.Services;

namespace recycler_view_Lukashevich.Views;

public partial class AddLoanWindow : Window
{
    private readonly ApiClient? _apiClient;
    private List<BookModel> _books = new();
    private List<UserModel> _users = new();

    public AddLoanWindow(ApiClient? apiClient)
    {
        InitializeComponent();
        _apiClient = apiClient;
        LoadData();
    }

    private async void LoadData()
    {
        if (_apiClient == null) return;
        try
        {
            _books = await _apiClient.GetAsync<List<BookModel>>("books") ?? new List<BookModel>();
            _users = await _apiClient.GetAsync<List<UserModel>>("users") ?? new List<UserModel>();

            BookCombo.ItemsSource = _books;
            UserCombo.ItemsSource = _users;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка");
        }
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (_apiClient == null) return;

        if (BookCombo.SelectedItem == null || UserCombo.SelectedItem == null)
        {
            MessageBox.Show("Выберите книгу и читателя", "Ошибка");
            return;
        }

        int bookId = (int)BookCombo.SelectedValue;
        int userId = (int)UserCombo.SelectedValue;
        int days = int.Parse(((ComboBoxItem)DaysCombo.SelectedItem).Tag.ToString());

        var loan = new LoanModel
        {
            BookId = bookId,
            UserId = userId,
            DueDate = DateTime.UtcNow.AddDays(days)
        };

        try
        {
            await _apiClient.PostAsync<LoanModel>("loans", loan);
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка выдачи: " + ex.Message, "Ошибка");
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}