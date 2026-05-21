using MunicipiosIbge.Api.Features.Municipalities.GetAll.Models;

namespace MunicipiosIbge.Api.Features.Municipalities.GetAll.Validators;

public static class GetMunicipalitiesValidator
{
    public static void Validate(GetMunicipalitiesQuery query)
    {
        if (query.Page.HasValue && query.Page < 1)
        {
            throw new InvalidOperationException("Page must be greater than or equal to 1.");
        }

        if (query.PageSize is < 1 or > 200)
        {
            throw new InvalidOperationException("PageSize must be between 1 and 200.");
        }
    }
}
