using MunicipiosIbge.Api.Features.Municipalities.GetById.Models;

namespace MunicipiosIbge.Api.Features.Municipalities.GetById.Validators;

public static class GetMunicipalityByIdValidator
{
    public static void Validate(GetMunicipalityByIdQuery query)
    {
        if (query.Id <= 0)
        {
            throw new InvalidOperationException("Municipality id must be greater than 0.");
        }
    }
}
