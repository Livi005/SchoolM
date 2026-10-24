using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Notification;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Notifications;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;
using SchoolManagement.Infrastructure.Identity;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityManager _identityManager;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IUnitOfWork unitOfWork,
        IIdentityManager identityManager,
        ILogger<NotificationService> logger)
    {
        _unitOfWork = unitOfWork;
        _identityManager = identityManager;
        _logger = logger;
    }

    public async Task<Result<NotificationResponse>> GetNotificationsAsync(
        string userId, CancellationToken cancellationToken = default)
    {
        var user = await _identityManager.FindByUserNameAsync(userId);
        if (user is null)
            return Result.NotFound<NotificationResponse>("User", userId);

        var response = new NotificationResponse();

        if (await _identityManager.IsInRoleAsync(user.Id, Roles.Admin))
        {
            await RefreshMeanNotificationsAsync(cancellationToken);
            await AppendPendingMeanNotificationsAsync(response, cancellationToken);
        }
        else if (await _identityManager.IsInRoleAsync(user.Id, Roles.SuperAdmin))
        {
            await RefreshProfessorNotificationsAsync(cancellationToken);
            await AppendPendingProfessorNotificationsAsync(response, cancellationToken);
        }

        return Result.Success(response);
    }

    // ---------- Medios ----------

    private async Task RefreshMeanNotificationsAsync(CancellationToken cancellationToken)
    {
        var oneYearAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1));

        var problematicMeans = await _unitOfWork.Repository<Maintenance>().Query()
            .Where(m => m.MaintenanceDate >= oneYearAgo)
            .GroupBy(m => m.TypeOfMean == MeanType.Technological ? m.IdTechMean : m.IdAuxMean)
            .Select(g => new { MeanId = g.Key, Count = g.Count() })
            .Where(x => x.MeanId.HasValue && x.Count >= 3)
            .ToListAsync(cancellationToken);

        var existing = await _unitOfWork.Repository<MeanNotification>().Query()
            .Select(n => n.IdMean)
            .ToListAsync(cancellationToken);
        var existingSet = existing.ToHashSet();

        foreach (var item in problematicMeans)
        {
            if (item.MeanId is null || existingSet.Contains(item.MeanId.Value)) continue;

            var meanName = await GetMeanNameAsync(item.MeanId.Value, cancellationToken);
            var meanType = await GetMeanTypeAsync(item.MeanId.Value, cancellationToken);

            await _unitOfWork.Repository<MeanNotification>().AddAsync(new MeanNotification
            {
                IdMean = item.MeanId.Value,
                MeanName = meanName,
                MeanType = meanType.ToString(),
                BeenSended = false
            }, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task AppendPendingMeanNotificationsAsync(
        NotificationResponse response, CancellationToken cancellationToken)
    {
        var pending = await _unitOfWork.Repository<MeanNotification>().Query()
            .Where(n => !n.BeenSended)
            .ToListAsync(cancellationToken);

        foreach (var n in pending)
        {
            response.Messages.Add(
                $"El medio {n.MeanName} ha tenido más de 3 mantenimientos en el último año. Su id es {n.IdMean}.");
            n.BeenSended = true;
            _unitOfWork.Repository<MeanNotification>().Update(n);
        }

        if (pending.Count > 0) await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<string> GetMeanNameAsync(int meanId, CancellationToken cancellationToken)
    {
        var tech = await _unitOfWork.Repository<TechnologicalMeans>().GetByIdAsync(meanId, cancellationToken);
        if (tech is not null) return tech.NameMean;

        var aux = await _unitOfWork.Repository<AuxiliaryMeans>().GetByIdAsync(meanId, cancellationToken);
        return aux?.NameMean ?? string.Empty;
    }

    private async Task<MeanType> GetMeanTypeAsync(int meanId, CancellationToken cancellationToken)
    {
        var tech = await _unitOfWork.Repository<TechnologicalMeans>().GetByIdAsync(meanId, cancellationToken);
        return tech is not null ? MeanType.Technological : MeanType.Auxiliary;
    }

    // ---------- Profesores ----------

    private async Task RefreshProfessorNotificationsAsync(CancellationToken cancellationToken)
    {
        var allEvaluations = await _unitOfWork.Repository<ProfStudSubCourse>().Query()
            .Select(e => new { e.IdProf, e.Evaluation })
            .ToListAsync(cancellationToken);

        var problematic = allEvaluations
            .GroupBy(e => e.IdProf)
            .Where(g => g.Count() >= 5 && g.All(x => x.Evaluation < 3))
            .Select(g => g.Key)
            .ToList();

        var existing = await _unitOfWork.Repository<ProfessorNotification>().Query()
            .Select(n => n.IdProf)
            .ToListAsync(cancellationToken);
        var existingSet = existing.ToHashSet();

        foreach (var professorId in problematic)
        {
            if (existingSet.Contains(professorId)) continue;

            var professor = await _unitOfWork.Repository<Professor>()
                .GetByIdAsync(professorId, cancellationToken);
            if (professor is null) continue;

            await _unitOfWork.Repository<ProfessorNotification>().AddAsync(new ProfessorNotification
            {
                IdProf = professorId,
                ProfName = professor.NameProf,
                Professor = professor,
                BeenSended = false
            }, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task AppendPendingProfessorNotificationsAsync(
        NotificationResponse response, CancellationToken cancellationToken)
    {
        var pending = await _unitOfWork.Repository<ProfessorNotification>().Query()
            .Where(n => !n.BeenSended)
            .ToListAsync(cancellationToken);

        foreach (var n in pending)
        {
            response.Messages.Add(
                $"El profesor {n.ProfName} tiene 5 o más evaluaciones todas por debajo de 3. Su Id={n.IdProf}.");
            n.BeenSended = true;
            _unitOfWork.Repository<ProfessorNotification>().Update(n);
        }

        if (pending.Count > 0) await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}