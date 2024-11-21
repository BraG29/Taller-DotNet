using API_Gateway.DTOS;

namespace API_Gateway.Services;

public class AuthenticationService(HttpClient httpClient)
{
    public async Task<string> CallLogin(LoginRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/auth/login", request);

        return await response.Content.ReadAsStringAsync();
    }
}