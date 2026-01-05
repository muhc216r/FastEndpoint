using System.Text;
using System.Security.Cryptography;

namespace IdentityServer.Feature.Domain;
public class User
{
    public User(string? firstName, string? lastName, string userName, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;

        PasswordSalt = RandomNumberGenerator.GetBytes(16);
        PasswordHash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), PasswordSalt, 100000, HashAlgorithmName.SHA256, 32);
    }

    private User() { }
    public int Id { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

    public bool VerifyPassword(string password)
    {
        var computed = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),PasswordSalt, 100000, HashAlgorithmName.SHA256,32);
        return CryptographicOperations.FixedTimeEquals(computed, PasswordHash);
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(1000);
        builder.Property(x => x.LastName).HasMaxLength(1000);
        builder.Property(x => x.UserName).HasMaxLength(1000);
    }
}