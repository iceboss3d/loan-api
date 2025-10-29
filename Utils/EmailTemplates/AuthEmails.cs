namespace Loan.Api.Utils.EmailTemplates;

public class AuthEmails
{
    public static string ResetPassword(string confirmationUrl)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Reset Your Password</title>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            background-color: #f4f4f4;
        }}
        .container {{
            background-color: #ffffff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }}
        .header {{
            text-align: center;
            margin-bottom: 30px;
        }}
        .logo {{
            font-size: 24px;
            font-weight: bold;
            color: #2563eb;
            margin-bottom: 10px;
        }}
        .title {{
            color: #1f2937;
            font-size: 20px;
            margin-bottom: 20px;
        }}
        .content {{
            margin-bottom: 30px;
        }}
        .button {{
            display: inline-block;
            background-color: #2563eb;
            color: #ffffff;
            padding: 12px 30px;
            text-decoration: none;
            border-radius: 6px;
            font-weight: 600;
            margin: 20px 0;
        }}
        .button:hover {{
            background-color: #1d4ed8;
        }}
        .footer {{
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid #e5e7eb;
            font-size: 14px;
            color: #6b7280;
        }}
        .warning {{
            background-color: #fef3c7;
            border: 1px solid #f59e0b;
            border-radius: 6px;
            padding: 15px;
            margin: 20px 0;
            color: #92400e;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='logo'>Loan API</div>
            <h1 class='title'>Reset Your Password</h1>
        </div>
        
        <div class='content'>
            <p>Hello,</p>
            
            <p>We received a request to reset your password for your Eduvia account. If you didn't make this request, you can safely ignore this email.</p>
            
            <p>To reset your password, click the button below:</p>
            
            <div style='text-align: center;'>
                <a href='{confirmationUrl}' class='button'>Reset Password</a>
            </div>
            
            <div class='warning'>
                <strong>Security Notice:</strong> This link will expire in 1 hour for your security. If you need a new link, please request another password reset.
            </div>
            
            <p>If the button doesn't work, you can copy and paste this link into your browser:</p>
            <p style='word-break: break-all; color: #2563eb;'>{confirmationUrl}</p>
        </div>
        
        <div class='footer'>
            <p>This email was sent to you because someone requested a password reset for your Eduvia account.</p>
            <p>If you didn't request this, please ignore this email and your password will remain unchanged.</p>
            <p>Best regards,<br>The Eduvia Team</p>
        </div>
    </div>
</body>
</html>";
    }
}