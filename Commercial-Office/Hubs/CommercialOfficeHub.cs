using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace Commercial_Office.Hubs
{
    [EnableCors("_myAllowSpecificOrigins")]
    public class CommercialOfficeHub : Hub
    {
    }
}
