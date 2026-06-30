using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using recycler_view_Lukashevich.Models;
using recycler_view_Lukashevich.Services;

namespace recycler_view_Lukashevich.ViewModels;

public class UsersViewModel : INotifyPropertyChanged
{
    private ApiClient? _apiClient;
    public ApiClient? ApiClient
    {
        get => _apiClient;
        set { _apiClient = value; OnPropertyChanged(); }
    }

    private ObservableCollection<UserModel> _users = new();
    public ObservableCollection<UserModel> Users
    {
        get => _users;
        set { _users = value; OnPropertyChanged(); }
    }

    private UserModel? _selectedUser;
    public UserModel? SelectedUser
    {
        get => _selectedUser;
        set { _selectedUser = value; OnPropertyChanged(); }
    }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set { _searchText = value; OnPropertyChanged(); }
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    private bool _hasError;
    public bool HasError
    {
        get => _hasError;
        set { _hasError = value; OnPropertyChanged(); }
    }

    public ICommand? LoadUsersCommand { get; set; }
    public ICommand? DeleteUserCommand { get; set; }

    public async Task LoadUsers(string? search = null)
    {
        if (ApiClient == null) return;
        IsLoading = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            var users = await ApiClient.GetAsync<List<UserModel>>("api/users");
            if (users != null)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    users = users.Where(u =>
                        (u.FullName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                        (u.Email?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
                }

                Users.Clear();
                foreach (var user in users)
                    Users.Add(user);
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = "Ошибка загрузки пользователей: " + ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task DeleteUser(int userId)
    {
        if (ApiClient == null) return;
        IsLoading = true;
        try
        {
            await ApiClient.DeleteAsync($"api/users/{userId}");
            await LoadUsers(SearchText);
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = "Ошибка удаления пользователя: " + ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}