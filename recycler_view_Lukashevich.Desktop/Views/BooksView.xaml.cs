using System;
using System.Windows;
using System.Windows.Controls;
using recycler_view_Lukashevich.ViewModels;

namespace recycler_view_Lukashevich.Views
{
    public partial class BooksView : UserControl
    {
        public BooksViewModel ViewModel => DataContext as BooksViewModel ?? new();

        public BooksView()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadBooksCommand?.Execute(ViewModel.SearchText);
        }

        private void SearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ViewModel.LoadBooksCommand?.Execute(ViewModel.SearchText);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditBookWindow(ViewModel.ApiClient);
            if (window.ShowDialog() == true)
            {
                ViewModel.LoadBooksCommand?.Execute(ViewModel.SearchText);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedBook == null)
            {
                MessageBox.Show("Выберите книгу для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var window = new AddEditBookWindow(ViewModel.ApiClient, ViewModel.SelectedBook);
            if (window.ShowDialog() == true)
            {
                ViewModel.LoadBooksCommand?.Execute(ViewModel.SearchText);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedBook == null)
            {
                MessageBox.Show("Выберите книгу для удаления", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show($"Удалить книгу '{ViewModel.SelectedBook.Title}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (ViewModel.ApiClient == null) return;
                try
                {
                    await ViewModel.ApiClient.DeleteAsync($"api/books/{ViewModel.SelectedBook.Id}");
                    ViewModel.LoadBooksCommand?.Execute(ViewModel.SearchText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}