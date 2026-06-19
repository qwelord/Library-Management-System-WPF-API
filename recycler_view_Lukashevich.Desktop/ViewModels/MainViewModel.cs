using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace recycler_view_Lukashevich.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private string _title = "Библиотечная система";
    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); }
    }

    private string _currentUser = string.Empty;
    public string CurrentUser
    {
        get => _currentUser;
        set { _currentUser = value; OnPropertyChanged(); }
    }

    private string _role = string.Empty;
    public string Role
    {
        get => _role;
        set { _role = value; OnPropertyChanged(); }
    }

    private bool _isAdmin;
    public bool IsAdmin
    {
        get => _isAdmin;
        set { _isAdmin = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}