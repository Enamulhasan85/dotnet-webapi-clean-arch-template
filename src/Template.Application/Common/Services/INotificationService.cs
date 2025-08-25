namespace Template.Application.Common.Services;

public interface INotificationService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    Task SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
    Task SendPushNotificationAsync(string userId, string title, string message, CancellationToken cancellationToken = default);
}
