using Authentication.DataAccess;
using Authentication.Models;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace Authentication.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthenticationDbContext _context;

    public UserRepository(AuthenticationDbContext context)
    {
        _context = context;
    }

    public async Task<User> Save(User user)
    {
        try
        {
            user.Password = HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Error: {e.GetType()}: {e.Message}");
            throw;
        }
    }

    public async Task<User?> GetByUsernameAndPassword(string username, string password)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(user => user.Username == username);

        if (user != null && Verify(password, user.Password))
        {
            return user;
        }

        return null;
    }
}