using MunicipiosIbge.Api.Domain.Entities;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Models;

namespace MunicipiosIbge.Api.Features.Municipalities.Common;

public static class MunicipalityResponseMapper
{
    public static MunicipalityResponse ToResponse(Municipality municipality)
    {
        return new MunicipalityResponse(
            municipality.Id,
            municipality.Name,
            municipality.ImmediateRegion?.IntermediateRegion?.State?.Acronym
                ?? municipality.Microregion?.Mesorregion?.State?.Acronym,
            municipality.ImmediateRegion?.IntermediateRegion?.StateId
                ?? municipality.Microregion?.Mesorregion?.StateId,
            municipality.ImmediateRegion?.IntermediateRegion?.State?.Name
                ?? municipality.Microregion?.Mesorregion?.State?.Name,
            municipality.ImmediateRegion?.IntermediateRegion?.State?.RegionId
                ?? municipality.Microregion?.Mesorregion?.State?.RegionId,
            municipality.ImmediateRegion?.IntermediateRegion?.State?.Region?.Acronym
                ?? municipality.Microregion?.Mesorregion?.State?.Region?.Acronym,
            municipality.ImmediateRegion?.IntermediateRegion?.State?.Region?.Name
                ?? municipality.Microregion?.Mesorregion?.State?.Region?.Name,
            municipality.Microregion?.MesorregionId,
            municipality.Microregion?.Mesorregion?.Name,
            municipality.MicroregionId,
            municipality.Microregion?.Name,
            municipality.ImmediateRegionId,
            municipality.ImmediateRegion?.Name
                ?? throw new InvalidOperationException($"Municipality '{municipality.Id}' has no immediate region."),
            municipality.ImmediateRegion.IntermediateRegionId,
            municipality.ImmediateRegion.IntermediateRegion?.Name
                ?? throw new InvalidOperationException($"Municipality '{municipality.Id}' has no intermediate region."));
    }
}
