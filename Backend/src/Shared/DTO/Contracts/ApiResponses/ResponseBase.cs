using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Contracts.ApiResponses
{
    public abstract record ResponseBase(
    bool Success,
    string Code
    );
}
