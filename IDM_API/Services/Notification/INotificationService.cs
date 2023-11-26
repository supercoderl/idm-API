namespace IDM_API.Services.Notification
{
	public interface INotificationService
	{
		Task SendMessage(string message);
	}
}
