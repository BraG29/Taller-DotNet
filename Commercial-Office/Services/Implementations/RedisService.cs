using System.Text.Json;
using Commercial_Office.Model;
using Commercial_Office.Services.Interfaces;
using StackExchange.Redis;
using NullReferenceException = System.NullReferenceException;

namespace Commercial_Office.Services.Implementations;

public class RedisService : IRedisService
{

    private readonly IDatabase _database;
    
    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }
    
    public async Task SetValueAsync(string key, Office value)
    {
        Console.WriteLine($"-----[GUARDANDO OFICINA '{key}' EN EL REDIS]-----");
        string valueAsJson = JsonSerializer.Serialize(value);
        
        Console.WriteLine($"-----\nOFICINA COMO JSON: {valueAsJson}\n-----\n");
        await _database.StringSetAsync(key, valueAsJson);
    }

    public async Task<Office?> GetValueAsync(string key)
    {
        string? valueAsJson = await _database.StringGetAsync(key);
        
        return valueAsJson == null ? null : JsonSerializer.Deserialize<Office>(valueAsJson);
    }

    // public async Task<bool> Exist(string key)
    // {
    //     return await _database.KeyExistsAsync(key);
    // }
}