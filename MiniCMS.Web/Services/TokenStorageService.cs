using Microsoft.JSInterop;

namespace MiniCMS.Web.Services;

public class TokenStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "CEP_De_Thi_So_Tuyen_Chuyen_Vien_Phat_Trien_Phan_Mem_Phong_CNTT";
    private static string? _cachedToken; // Lưu trực tiếp trong RAM của session

    public TokenStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetTokenAsync(string token)
    {
        _cachedToken = token;
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        }
        catch
        {
            // Bỏ qua lỗi nếu JS chưa sẵn sàng
        }
    }

    public async Task<string?> GetTokenAsync()
    {
        if (!string.IsNullOrWhiteSpace(_cachedToken))
        {
            return _cachedToken;
        }

        try
        {
            _cachedToken = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
            return _cachedToken;
        }
        catch
        {
            return null;
        }
    }

    public async Task RemoveTokenAsync()
    {
        _cachedToken = null;
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        }
        catch
        {
            // Bỏ qua lỗi nếu JS chưa sẵn sàng
        }
    }
}