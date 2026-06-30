using System;
using System.Windows;
using recycler_view_Lukashevich.Models;
using recycler_view_Lukashevich.Services;

namespace recycler_view_Lukashevich.Views;

public partial class AddEditUserWindow : Window
{
    private readonly ApiClient? _apiClient;
    private readonly UserModel? _editUser;
    public UserModel ViewModel { get; } = new();
    public string WindowTitle { get; set; } = "Добавление пользователя";

    public AddEditUserWindow(ApiClient? apiClient, UserModel? editUser = null)
    {
        InitializeComponent();
        _apiClient = apiClient;
        _editUser = editUser;
        DataContext = this;

        if (editUser != null)
        {
            ViewModel.Id = editUser.Id;
            ViewModel.FullName = editUser.FullName;
            ViewModel.Email = editUser.Email;
            ViewModel.Role = editUser.Role;
            WindowTitle = "Редактирование пользователя";
        }
        else
        {
            ViewModel.Role = "User";
        }
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ViewModel.FullName) || string.IsNullOrWhiteSpace(ViewModel.Email))
        {
            MessageBox.Show("Заполните все поля", "Ошибка");
            return;
        }

        if (_apiClient == null) return;

        try
        {
            var password = PasswordBox.Password;
            if (_editUser == null && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль", "Ошибка");
                return;
            }

            var userToSave = new UserModel
            {
                Id = ViewModel.Id,
                FullName = ViewModel.FullName,
                Email = ViewModel.Email,
                Role = ViewModel.Role,
                PasswordHash = string.IsNullOrWhiteSpace(password) ? _editUser?.PasswordHash ?? "" : password
            };

            if (_editUser == null)
            {
                await _apiClient.PostAsync<UserModel>("api/users", userToSave);
            }
            else
            {
                await _apiClient.PutAsync<UserModel>($"api/users/{_editUser.Id}", userToSave);
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