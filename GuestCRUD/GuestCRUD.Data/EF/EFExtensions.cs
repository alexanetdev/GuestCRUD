using GuestCRUD.Data.Models;
using GuestCRUD.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GuestCRUD.Data.EF
{
    public static class EFExtensions
    {
        public static GuestDto ToDto(this Guest entity)
        {
            return new GuestDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                DateOfBirth = entity.DateOfBirth.Decrypt(),
                EmailAddress = entity.EmailAddress.Decrypt(),
                Address = entity.Address.Decrypt(),
                PhoneNumber = entity.PhoneNumber.Decrypt()
            };
        }

        public static Guest FromDto(this GuestDto dto)
        {
            return new Guest
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth.Crypt(),
                EmailAddress = dto.EmailAddress.Crypt(),
                Address = dto.Address.Crypt(),
                PhoneNumber = dto.PhoneNumber.Crypt()
            };
        }

        private static byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] iv = { 1, 2, 3, 4, 5, 6, 7, 8 };

        public static string Crypt(this string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

        public static string Decrypt(this string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }
    }
}
