using Commercial_Office.Model;

namespace Commercial_Office.Services.Interfaces;

public interface IRedisService
{
    public Task SetValueAsync(string key, Office value);
    public Task<Office?> GetValueAsync(string key);
    // public Task<bool> Exist(string key);
}