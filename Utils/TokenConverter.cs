using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace Loan.Api.Utils;

public class TokenConverter
{
    public static string EncodeToken(string token)
    {
        var encodedToken = Encoding.UTF8.GetBytes(token);
        return WebEncoders.Base64UrlEncode(encodedToken);
    }

    public static string DecodeToken(string token)
    {
        var decodedToken = WebEncoders.Base64UrlDecode(token);
        return Encoding.UTF8.GetString(decodedToken);
    }
}