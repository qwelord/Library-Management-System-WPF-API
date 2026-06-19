using System.Windows;
using recycler_view_Lukashevich.Models;
using recycler_view_Lukashevich.Services;

namespace recycler_view_Lukashevich.Views;

public partial class AddEditBookWindow : Window
{
    private readonly ApiClient? _apiClient;
    private readonly BookModel? _editBook;
    public BookModel ViewModel { get; } = new();
    public string WindowTitle { get; set; } = "Добавление книги";

    public AddEditBookWindow(ApiClient? apiClient, BookModel? editBook = null)
    {
        InitializeComponent();
        _apiClient = apiClient;
        _editBook = editBook;
        DataContext = this;

        if (editBook != null)
        {
            ViewModel.Id = editBook.Id;
            ViewModel.Title = editBook.Title;
            ViewModel.Author = editBook.Author;
            ViewModel.ISBN = editBook.ISBN;
            ViewModel.Year = editBook.Year;
            ViewModel.Publisher = editBook.Publisher;
            ViewModel.Quantity = editBook.Quantity;
            ViewModel.AvailableQuantity = editBook.AvailableQuantity;
            WindowTitle = "Редактирование книги";
        }
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ViewModel.Title) || string.IsNullOrWhiteSpace(ViewModel.Author))
        {
            MessageBox.Show("Заполните название и автора", "Ошибка");
            return;
        }

        if (_apiClient == null) return;

        try
        {
            if (_editBook == null)
            {
                await _apiClient.PostAsync<BookModel>("books", ViewModel);
            }
            else
            {
                await _apiClient.PutAsync<BookModel>($"books/{_editBook.Id}", ViewModel);
            }
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка сохранения: " + ex.Message, "Ошибка");
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}