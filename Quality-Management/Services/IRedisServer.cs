using Quality_Management.Model;

namespace Quality_Management.Services;

public interface IRedisServer
{
    public Task SetValueAsync(string key, Office value);
    public Task<Office?> GetValueAsync(string key);
}