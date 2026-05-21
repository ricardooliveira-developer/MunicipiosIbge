using MunicipiosIbge.Api.Features.Municipalities.GetAll.Models;

namespace MunicipiosIbge.Api.Features.Municipalities.GetById.Models;

/// <summary>
/// Resultado da consulta de município por ID.
/// </summary>
/// <param name="Municipality">Município encontrado.</param>
public sealed record GetMunicipalityByIdResponse(MunicipalityResponse Municipality);
