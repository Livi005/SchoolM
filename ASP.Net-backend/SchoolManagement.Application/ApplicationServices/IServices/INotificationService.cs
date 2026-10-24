using SchoolManagement.Application.ApplicationServices.Maps_Dto.Notification;
using SchoolManagement.Application.Common;

namespace SchoolManagement.Application.ApplicationServices.IServices;

public interface INotificationService
{
    Task<Result<NotificationResponse>> GetNotificationsAsync(
        string userId, CancellationToken cancellationToken = default);
}