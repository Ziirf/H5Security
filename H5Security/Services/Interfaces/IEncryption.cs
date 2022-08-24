using System.Security.Cryptography;
using System;

namespace H5Security.Services.Interfaces
{
    public interface IEncryption
    {
        byte[] GenerateSalt();

        string Hash(string input, byte[] salt, int Iterations);

        string HashSha256(string input, byte[] salt);

        int GetIteration();

        string Encrypt(string message);

        string Decrypt(string message);
    }
}
