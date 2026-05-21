using MunicipiosIbge.Api.Common.Mediator;

namespace MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Models;

public sealed class GetMunicipalitiesByMesorregionQuery : IRequest<GetMunicipalitiesByMesorregionResponse>
{
    public int MesorregionId { get; init; }
    public int? Page { get; init; }
    public int? PageSize { get; init; }
}
