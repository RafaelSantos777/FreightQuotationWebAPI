using System.Collections.Specialized;
using System.Web;

namespace QuotationService.HTTPInfrastructure;

public class WebCargoHTTPMessageHandler : DelegatingHandler {

    private readonly string _apiKey1;

    private readonly string _apiKey2;

    public WebCargoHTTPMessageHandler(IConfiguration configuration) {
        IConfigurationSection configurationSection = configuration.GetSection("WebCargo");
        _apiKey1 = configurationSection["APIKey1"]!;
        _apiKey2 = configurationSection["APIKey2"]!;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken) {
        if (requestMessage.RequestUri is null)
            return await base.SendAsync(requestMessage, cancellationToken);
        UriBuilder uriBuilder = new(requestMessage.RequestUri);
        NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query.Add("k1", _apiKey1);
        query.Add("k2", _apiKey2);
        uriBuilder.Query = query.ToString();
        requestMessage.RequestUri = uriBuilder.Uri;
        return await base.SendAsync(requestMessage, cancellationToken);
    }

}
