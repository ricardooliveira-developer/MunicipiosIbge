using MunicipiosIbge.Api.Features.Municipalities.GetAll.Models;
using MunicipiosIbge.Api.Features.Municipalities.GetAll.Validators;
using MunicipiosIbge.Api.Features.Municipalities.GetById.Models;
using MunicipiosIbge.Api.Features.Municipalities.GetById.Validators;
using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Models;
using MunicipiosIbge.Api.Features.Municipalities.GetByMesorregion.Validators;

namespace MunicipiosIbge.Tests.Features.Municipalities.Validators;

public sealed class MunicipalityValidatorsTests
{
    [Fact]
    public void GetMunicipalitiesValidator_WhenPageIsInvalid_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
            GetMunicipalitiesValidator.Validate(new GetMunicipalitiesQuery { Page = 0 }));
    }

    [Fact]
    public void GetMunicipalitiesValidator_WhenPageSizeIsInvalid_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
            GetMunicipalitiesValidator.Validate(new GetMunicipalitiesQuery { PageSize = 201 }));
    }

    [Fact]
    public void GetMunicipalityByIdValidator_WhenIdIsInvalid_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
            GetMunicipalityByIdValidator.Validate(new GetMunicipalityByIdQuery(0)));
    }

    [Fact]
    public void GetMunicipalitiesByMesorregionValidator_WhenMesorregionIdIsInvalid_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
            GetMunicipalitiesByMesorregionValidator.Validate(new GetMunicipalitiesByMesorregionQuery
            {
                MesorregionId = 0
            }));
    }

    [Fact]
    public void GetMunicipalitiesByMesorregionValidator_WhenPageSizeIsInvalid_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
            GetMunicipalitiesByMesorregionValidator.Validate(new GetMunicipalitiesByMesorregionQuery
            {
                MesorregionId = 1,
                PageSize = 0
            }));
    }
}
