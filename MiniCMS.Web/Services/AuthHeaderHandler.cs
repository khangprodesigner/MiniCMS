using System.Net.Http.Headers;

namespace MiniCMS.Web.Services;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly TokenStorageService _tokenService;

    public AuthHeaderHandler(TokenStorageService tokenService)
    {
        _tokenService = tokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenService.GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}