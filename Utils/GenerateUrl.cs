namespace Loan.Api.Utils;

public class GenerateUrl(IHttpContextAccessor httpContext, IConfiguration configuration)
{
    private readonly IHttpContextAccessor _httpContext = httpContext;
    private readonly IConfiguration _configuration = configuration;

    public string Generate(string path, string query)
    {
        string baseUrl =
            _configuration["Frontend:BaseUrl"];
        var siteUrl = new UriBuilder(baseUrl)
        {
            Query = query,
            Path = path
        };
        return siteUrl.ToString();
    }
}