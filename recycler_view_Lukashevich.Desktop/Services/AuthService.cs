namespace recycler_view_Lukashevich.Services;

public class AuthService
{
    private readonly ApiClient _apiClient;
    public User? CurrentUser { get; private set; }

    public AuthService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public void SetToken(string token, int id, string email, string fullName, string role)
    {
        _apiClient.SetToken(token);
        CurrentUser = new User
        {
            Id = id,
            Email = email,
            FullName = fullName,
            Role = role
        };
    }

    public void Logout()
    {
        _apiClient.SetToken(string.Empty);
        CurrentUser = null;
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}