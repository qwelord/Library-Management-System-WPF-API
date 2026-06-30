using System;
using System.Windows;
using recycler_view_Lukashevich.Models;
using recycler_view_Lukashevich.Services;

namespace recycler_view_Lukashevich.Views
{
    public partial class AddEditBookWindow : Window
    {
        private readonly ApiClient _apiClient;
        private readonly BookModel _editBook;
        public BookModel ViewModel { get; } = new();
        public string WindowTitle { get; set; } = "Добавление книги";

        public AddEditBookWindow(ApiClient apiClient, BookModel editBook = null)
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
            if (string.IsNullOrWhiteSpace(ViewModel.Title))
            {
                MessageBox.Show("Введите название книги!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                TitleBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(ViewModel.Author))
            {
                MessageBox.Show("Введите автора книги!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                AuthorBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(ViewModel.ISBN))
            {
                MessageBox.Show("Введите ISBN книги!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                ISBNBox.Focus();
                return;
            }

            if (ViewModel.Year < 0 || ViewModel.Year > DateTime.Now.Year)
            {
                MessageBox.Show($"Год издания должен быть от 0 до {DateTime.Now.Year}!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                YearBox.Focus();
                return;
            }

            if (ViewModel.Quantity < 0)
            {
                MessageBox.Show("Количество не может быть отрицательным!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                QuantityBox.Focus();
                return;
            }

            if (_apiClient == null)
            {
                MessageBox.Show("Ошибка подключения к серверу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (_editBook == null)
                {
                    ViewModel.AvailableQuantity = ViewModel.Quantity;
                    await _apiClient.PostAsync<BookModel>("api/books", ViewModel);
                }
                else
                {
                    await _apiClient.PutAsync<BookModel>($"api/books/{_editBook.Id}", ViewModel);
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}