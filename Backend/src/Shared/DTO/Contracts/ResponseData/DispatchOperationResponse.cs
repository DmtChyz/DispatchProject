using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Contracts.ResponseData
{
    public sealed record DispatchOperationResponse(
        DateTime CreatedAtUtc,
        string CorrelationId,
        string Status
    );
}
