using DispatchMicroservice.Data.Entities;
using DispatchProject.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Enumerable;

namespace DispatchMicroservice.Services
{
    public class NotificationOperationService
    {
        private readonly AppDbContext _dbContext;

        public NotificationOperationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<NotificationOperation> CreateQueuedOperationAsync(
            string correlationId,
            string ownerId)
        {
            var now = DateTime.UtcNow;

            var operation = new NotificationOperation
            {
                CorrelationId = correlationId,
                UserId = ownerId,
                Status = NotificationOperationStatus.Queued,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            _dbContext.NotificationOperations.Add(operation);

            await _dbContext.SaveChangesAsync();

            return operation;
        }

        public async Task<bool> UpdateStatusAsync(
            string correlationId,
            string ownerId,
            NotificationOperationStatus status)
        {
            var operation = await _dbContext.NotificationOperations
                .FirstOrDefaultAsync(x =>
                    x.CorrelationId == correlationId &&
                    x.UserId == ownerId);

            if (operation == null)
            {
                return false;
            }

            operation.Status = status;
            operation.UpdatedAtUtc = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}