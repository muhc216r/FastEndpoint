using System.Text;
using System.Security.Cryptography;

namespace FastEndpoints;

public static class JwtKeyMaker
{
    public static void Generate(int keySize = 2048)
    {
        using var rsa = RSA.Create(keySize);

        var privateKey = rsa.ExportPkcs8PrivateKey();
        string privatePem = ToPem("PRIVATE KEY", privateKey);

        var publicKey = rsa.ExportSubjectPublicKeyInfo();
        string publicPem = ToPem("PUBLIC KEY", publicKey);

        File.WriteAllText("Common/jwt-private-key.pem", privatePem, Encoding.ASCII);
        File.WriteAllText("Common/jwt-public-key.pem", publicPem, Encoding.ASCII);
    }

    private static string ToPem(string label, byte[] derBytes)
    {
        // Base64 with 64-char lines
        string base64 = Convert.ToBase64String(derBytes);
        var sb = new StringBuilder();
        sb.AppendLine($"-----BEGIN {label}-----");

        for (int i = 0; i < base64.Length; i += 64)
            sb.AppendLine(base64.Substring(i, Math.Min(64, base64.Length - i)));

        sb.AppendLine($"-----END {label}-----");
        return sb.ToString();
    }
}