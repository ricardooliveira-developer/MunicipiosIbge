using MunicipiosIbge.Api.Common.Mediator;

namespace MunicipiosIbge.Api.Features.Municipalities.GetById.Models;

public sealed record GetMunicipalityByIdQuery(int Id) : IRequest<GetMunicipalityByIdResponse>;
