using System.Windows;
using System.Windows.Controls;
using recycler_view_Lukashevich.ViewModels;

namespace recycler_view_Lukashevich.Views;

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

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Окно добавления книги (в разработке)", "Информация");
    }
}