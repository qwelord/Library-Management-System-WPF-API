using System.Windows;
using System.Windows.Controls;
using recycler_view_Lukashevich.ViewModels;

namespace recycler_view_Lukashevich.Views;

public partial class LoansView : UserControl
{
    public LoansViewModel ViewModel => DataContext as LoansViewModel ?? new();

    public LoansView()
    {
        InitializeComponent();
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.LoadLoansCommand?.Execute(ViewModel.SearchText);
    }

    private void SearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Enter)
        {
            ViewModel.LoadLoansCommand?.Execute(ViewModel.SearchText);
        }
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new AddLoanWindow(ViewModel.ApiClient);
        if (window.ShowDialog() == true)
        {
            ViewModel.LoadLoansCommand?.Execute(ViewModel.SearchText);
        }
    }

    private async void ReturnButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedLoan == null)
        {
            MessageBox.Show("Выберите выдачу для возврата", "Информация");
            return;
        }
        if (ViewModel.SelectedLoan.Status == "Returned")
        {
            MessageBox.Show("Книга уже возвращена", "Информация");
            return;
        }
        if (MessageBox.Show($"Вернуть книгу '{ViewModel.SelectedLoan.Book?.Title}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            await ViewModel.ReturnBook(ViewModel.SelectedLoan.Id);
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.LoadLoansCommand?.Execute(ViewModel.SearchText);
    }
}