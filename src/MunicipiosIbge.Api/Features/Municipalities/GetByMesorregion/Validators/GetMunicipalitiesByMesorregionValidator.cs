using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Models;

namespace MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Validators;

public static class GetMunicipalitiesByMesorregionValidator
{
    public static void Validate(GetMunicipalitiesByMesorregionQuery query)
    {
        if (query.MesorregionId <= 0)
        {
            throw new InvalidOperationException("Mesorregion id must be greater than 0.");
        }

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
