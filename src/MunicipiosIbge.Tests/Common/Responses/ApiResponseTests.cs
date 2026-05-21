using MunicipiosIbge.Api.Common.Responses;

namespace MunicipiosIbge.Tests.Common.Responses;

public sealed class ApiResponseTests
{
    [Fact]
    public void Ok_ReturnsSuccessfulResponseWithDataAndMessage()
    {
        var response = ApiResponse<string>.Ok("data", "message");

        Assert.True(response.Success);
        Assert.Equal("data", response.Data);
        Assert.Equal("message", response.Message);
    }
}
