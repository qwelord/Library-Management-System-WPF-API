using System;
using System.Windows;
using System.Windows.Controls;
using recycler_view_Lukashevich.ViewModels;

namespace recycler_view_Lukashevich.Views;

public partial class UsersView : UserControl
{
    public UsersViewModel ViewModel => DataContext as UsersViewModel ?? new();

    public UsersView()
    {
        InitializeComponent();
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.LoadUsersCommand?.Execute(ViewModel.SearchText);
    }

    private void SearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Enter)
        {
            ViewModel.LoadUsersCommand?.Execute(ViewModel.SearchText);
        }
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new AddEditUserWindow(ViewModel.ApiClient);
        if (window.ShowDialog() == true)
        {
            ViewModel.LoadUsersCommand?.Execute(ViewModel.SearchText);
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedUser == null)
        {
            MessageBox.Show("Выберите пользователя для редактирования", "Информация");
            return;
        }
        var window = new AddEditUserWindow(ViewModel.ApiClient, ViewModel.SelectedUser);
        if (window.ShowDialog() == true)
        {
            ViewModel.LoadUsersCommand?.Execute(ViewModel.SearchText);
        }
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedUser == null)
        {
            MessageBox.Show("Выберите пользователя для удаления", "Информация");
            return;
        }
        if (MessageBox.Show($"Удалить пользователя {ViewModel.SelectedUser.FullName}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            await ViewModel.DeleteUser(ViewModel.SelectedUser.Id);
        }
    }
}