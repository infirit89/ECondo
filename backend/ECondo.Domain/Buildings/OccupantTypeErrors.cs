using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;

namespace ECondo.Domain.Buildings;

public static class OccupantTypeErrors
{
    public static Error Invalid(string occupantType) =>
        Error.NotFound(
            "OccupantTypes.NotFound",
            $"Occupant type with name = '{occupantType}' was not found");
}