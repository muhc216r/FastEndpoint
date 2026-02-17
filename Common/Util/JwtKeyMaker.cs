using System.Security.Cryptography;

namespace FastEndpoints;

public static class JwtKeyMaker
{
    public static void GenerateNewKeyPair()
    {
        using var rsa = RSA.Create(2048);
        var privateKey = rsa.ExportPkcs8PrivateKeyPem();
        var publicKey = rsa.ExportSubjectPublicKeyInfoPem();
    }
}