namespace Authentication.Models;

public interface IUserRepository
{
    public Task<User> Save(User user);
    public Task<User?> GetByUsernameAndPassword(string username, string password);
}