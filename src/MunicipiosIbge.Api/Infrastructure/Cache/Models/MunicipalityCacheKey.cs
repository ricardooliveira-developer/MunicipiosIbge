namespace MunicipiosIbge.Api.Infrastructure.Cache.Models;

public static class MunicipalityCacheKey
{
    private const string Prefix = "municipalities";

    public static string All() => $"{Prefix}:all";

    public static string ById(int id) => $"{Prefix}:id:{id}";

    public static string ByMesorregion(int mesorregionId) => $"{Prefix}:mesorregion:{mesorregionId}";
}
