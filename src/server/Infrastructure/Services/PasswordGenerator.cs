using System.Security.Cryptography;
using System.Text;
using ERP.Application.Common.Interfaces;

namespace ERP.Infrastructure.Services;

public class PasswordGenerator : IPasswordGenerator
{
    private static readonly char[] Digits = "0123456789".ToCharArray();

    public string GenerateTemporaryPassword()
    {
        var passwordLength = 5; // 5 digits only
        var bytes = RandomNumberGenerator.GetBytes(passwordLength);
        var sb = new StringBuilder(passwordLength);
        for (var i = 0; i < passwordLength; i++)
        {
            var index = bytes[i] % Digits.Length;
            sb.Append(Digits[index]);
        }

        return sb.ToString();
    }
}

