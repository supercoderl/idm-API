using Microsoft.AspNetCore.SignalR;

namespace IDM_API.Services.Notification
{
	public class NotificationService : Hub
	{
		public async Task SendNotification(string message)
		{
			await Clients.All.SendAsync("Notify", message);
		}
	}
}
