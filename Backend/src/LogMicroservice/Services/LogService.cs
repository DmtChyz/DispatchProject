using LogMicroservice.Data;
using LogMicroservice.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.DTO.Azure;
using Shared.DTO.Generic;

namespace LogMicroservice.Services
{
    public class LogService
    {
        private readonly AppDbContext _dbContext;
        private readonly ServiceBusPublisher _publisher;
        private readonly ILogger<LogService> _logger;

        public LogService(AppDbContext dbContext, ServiceBusPublisher publisher, ILogger<LogService> logger)
        {
            _dbContext = dbContext;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task CreateLog(LogMessage message)
        {
            if (message == null)
            {
                _logger.LogWarning("CreateLog received a null message.");
                return;
            }

            try
            {
                await _dbContext.ServiceLogs.AddAsync(
                    MapServiceLogToDbEntity(message)
                );

                await _dbContext.SaveChangesAsync();

                await _publisher.SendResponseLogAsync(
                    message.CorrelationId,
                    message.OwnerId,
                    message.CreatedAt,
                    message.Status,
                    message.ActionType
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create log or publish its response. CorrelationId: {CorrelationId}",
                    message.CorrelationId
                );

                throw;
            }
        }

        public async Task CreateDeadLetterLog(DlqLogMessage message)
        {
            if (message == null)
            {
                _logger.LogWarning("CreateDeadLetterLog received a null message.");
                return;
            }

            try
            {
                await _dbContext.DeadLetterLogs.AddAsync(
                    MapDlqLogToDbEntity(message)
                );

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create dead-letter log. CorrelationId: {CorrelationId}, QueueName: {QueueName}",
                    message.CorrelationId,
                    message.QueueName
                );

                throw;
            }
        }

        public async Task<PagedResult<UserActionLog>> GetPaginatedLogByOwnerId(
            string ownerId,
            int page,
            int pageSize)
        {
            if (string.IsNullOrWhiteSpace(ownerId))
            {
                _logger.LogError(
                    "GetPaginatedLogByOwnerId received an empty owner ID."
                );

                return PagedResult<UserActionLog>.Fail(
                    ResponseCodes.System.UnexpectedError
                );
            }

            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning(
                    "Invalid pagination values. Page: {Page}, PageSize: {PageSize}",
                    page,
                    pageSize
                );

                return PagedResult<UserActionLog>.Fail(
                    ResponseCodes.Validation.InvalidRequest
                );
            }

            if (!Guid.TryParse(ownerId, out var ownerGuid))
            {
                _logger.LogError(
                    "Owner ID could not be parsed as a GUID. OwnerId: {OwnerId}",
                    ownerId
                );

                return PagedResult<UserActionLog>.Fail(
                    ResponseCodes.System.UnexpectedError
                );
            }

            try
            {
                var query = _dbContext.ServiceLogs
                    .AsNoTracking()
                    .Where(log => log.OwnerId == ownerGuid);

                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return PagedResult<UserActionLog>.Success(
                        new List<UserActionLog>(),
                        totalCount,
                        page,
                        pageSize
                    );
                }

                var logs = await query
                    .OrderByDescending(log => log.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var mappedLogs = logs
                    .Select(MapDbLogToUserActionLog)
                    .ToList();

                return PagedResult<UserActionLog>.Success(
                    mappedLogs,
                    totalCount,
                    page,
                    pageSize
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to retrieve paginated logs. OwnerId: {OwnerId}, Page: {Page}, PageSize: {PageSize}",
                    ownerId,
                    page,
                    pageSize
                );

                return PagedResult<UserActionLog>.Fail(
                    ResponseCodes.System.UnexpectedError
                );
            }
        }

        private static UserActionLog MapDbLogToUserActionLog(ServiceLog log)
        {
            return new UserActionLog
            {
                ActionType = log.ActionType,
                Recipient = log.Recipient,
                Details = log.Details,
                CreatedAt = log.CreatedAt,
                Status = log.Status
            };
        }

        private static ServiceLog MapServiceLogToDbEntity(LogMessage message)
        {
            return new ServiceLog
            {
                OwnerId = Guid.TryParse(message.OwnerId, out var ownerId)
                    ? ownerId
                    : null,
                ActionType = message.ActionType,
                Recipient = message.Recipient,
                Details = message.Details,
                CorrelationId = message.CorrelationId,
                CreatedAt = message.CreatedAt,
                Status = message.Status
            };
        }

        private static DeadLetterLog MapDlqLogToDbEntity(DlqLogMessage message)
        {
            return new DeadLetterLog
            {
                CorrelationId = message.CorrelationId,
                QueueName = message.QueueName,
                MessageBody = message.MessageBody,
                Reason = message.Reason,
                ErrorDescription = message.ErrorDescription,
                EnqueuedAt = message.EnqueuedAt,
                DeadLetteredAt = message.DeadLetteredAt
            };
        }
    }
}