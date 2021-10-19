using System.Security.Cryptography;

namespace UserManagement.Helpers;

public class PasswordHasher
{
    private readonly int _iterations;
    private readonly int _saltSize;
    private readonly int _keySize;

    public PasswordHasher(int saltSize = 16, int keySize = 32, int iterations = 10)
    {
        _saltSize = saltSize;
        _keySize = keySize;
        _iterations = iterations;
    }

    public string Hash(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            _saltSize,
            _iterations,
            HashAlgorithmName.SHA256);
        var key = Convert.ToBase64String(algorithm.GetBytes(_keySize));
        var salt = Convert.ToBase64String(algorithm.Salt);
        return $"{_iterations}.{salt}.{key}";
    }

    public (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
    {
        var parts = hash.Split('.', 3);

        if (parts.Length != 3)
        {
            throw new FormatException("Unexpected hash format. " +
                                      "Should be formatted as `{iterations}.{salt}.{hash}`");
        }

        var iterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        var needsUpgrade = iterations != _iterations;

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256);
        var keyToCheck = algorithm.GetBytes(_keySize);

        var verified = keyToCheck.SequenceEqual(key);

        return (verified, needsUpgrade);
    }
}