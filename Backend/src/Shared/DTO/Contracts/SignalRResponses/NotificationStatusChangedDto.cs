using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Contracts.SignalRResponses
{
    public sealed record NotificationStatusChangedDto(
    string NotificationId,
    string Status
    );
}
