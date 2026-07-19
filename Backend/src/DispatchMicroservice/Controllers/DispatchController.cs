using DispatchMicroservice.DTO;
using DispatchMicroservice.Services;
using DispatchMicroservice.Validation;
using DispatchMicroservice.Validation.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using Shared.Constants;
using Shared.DTO.Azure;
using Shared.DTO.Contracts.ApiResponses;
using Shared.DTO.Contracts.ResponseData;
using Shared.Enumerable;
using System.Security.Claims;

namespace DispatchMicroservice.Controllers
{
    [Route("api/dispatch")]
    [ApiController]
    public class DispatchController : ControllerBase
    {
        private readonly ServiceBusPublisher _publisher;
        private readonly RecipientValidator _recipientValidator;
        private readonly NotificationOperationService _operationService;

        public DispatchController(ServiceBusPublisher publisher, RecipientValidator recipientValidator, NotificationOperationService operationService)
        {
            _publisher = publisher;
            _recipientValidator = recipientValidator;
            _operationService = operationService;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> HandleNotification([FromBody] ActionRequestDto requestDTO)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(ownerId))
            {
                return Unauthorized(ApiResponse.Fail(ResponseCodes.Auth.Unauthorized));
            }

            var actionType = requestDTO.ActionType!.Value;

            if (!_recipientValidator.IsValidRecipient(actionType, requestDTO.Recipient))
            {
                return BadRequest(ApiResponseFactory.Validation(
                    ResponseCodes.Validation.Fields.Recipient, 
                    ResponseCodes.Validation.Recipient.Invalid));
            }

            var correlationId = Guid.NewGuid().ToString("N");

            var message = new NotificationMessage
            {
                ActionType = actionType,
                Recipient = requestDTO.Recipient,
                Details = requestDTO.Details,
                CorrelationId = correlationId,
                OwnerId = ownerId
            };

            var operation = await _operationService.CreateQueuedOperationAsync(
                correlationId,
                ownerId
            );

            try
            {
                await _publisher.PublishAsync(message);
            }
            catch
            {
                await _operationService.UpdateStatusAsync(
                    correlationId,
                    ownerId,
                    NotificationOperationStatus.Failed
                );

                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    ApiResponse.Fail(ResponseCodes.Notification.QueueFailed)
                );
            }

            var response = new DispatchOperationResponse(
                operation.CreatedAtUtc,
                operation.CorrelationId,
                operation.Status.ToString().ToLower()
            );

            return Accepted(
                ApiResponse<DispatchOperationResponse>.Ok(
                    ResponseCodes.Notification.Queued,
                    response
                )
            );
        }
    }
}
