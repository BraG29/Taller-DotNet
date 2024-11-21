using API_Gateway.DTOS;
using Microsoft.IdentityModel.Tokens;

namespace API_Gateway.Services;

public class AuthenticationService(HttpClient httpClient)
{
    public async Task<string> CallLogin(LoginRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/auth/login", request);

        var task = response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode) throw new ArgumentException(await task);

        return await task;
    }

    public async Task<string> CallRegister(RegisterRequest request)
    {
        if (request.Role < 0 || request.Role > 1) throw new ArgumentException("No existe el rol eligido");
        if (string.IsNullOrWhiteSpace(request.Username)) throw new ArgumentException("El username es requerido");
        if( request.Password == null || request.Password.Length < 4) 
            throw new ArgumentException("La contraseña es demasiado corta");

        var response = await httpClient.PostAsJsonAsync("api/auth/register", request);
        var task = response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode) throw new ArgumentException(await task);

        return await task;
    }
}