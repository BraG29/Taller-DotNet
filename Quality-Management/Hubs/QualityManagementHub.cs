using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace Quality_Management.Hubs;

[EnableCors("_myAllowSpecificOrigins")]
public class QualityManagementHub : Hub
{
}