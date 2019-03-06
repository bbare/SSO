using System;

namespace ServiceLayer.Services
{
    public interface IPasswordService
    {
        byte[] GenerateSalt();
        string HashPassword(string password, byte[] salt);
        int CheckPasswordPwned(string password);
        string HashPasswordSHA1(string password, byte[] salt);
        string[] QueryPwnedApi(string prefix, string url);
        bool VerifyPassword(string password, string hash, byte[] salt);
    }
}