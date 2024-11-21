﻿namespace API_Gateway.DTOS;

public class RegisterRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Role { get; set; }
    
}