using System.Text.Json;
using Quality_Management.Model;
using StackExchange.Redis;

namespace Quality_Management.Services;

public class RedisService : IRedisServer
{
    private readonly IDatabase _database;
    
    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task SetValueAsync(string key, Office value)
    {
        string valueAsJson = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, valueAsJson);
    }

    public async Task<Office?> GetValueAsync(string key)
    {
        string? valueAsJson = await _database.StringGetAsync(key);

        return valueAsJson == null ? null : JsonSerializer.Deserialize<Office>(valueAsJson);
    }
}