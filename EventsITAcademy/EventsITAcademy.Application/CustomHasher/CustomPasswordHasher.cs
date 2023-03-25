//using EventsITAcademy.Domain.Users;
//using Microsoft.AspNetCore.Identity;
//using System.Security.Cryptography;
//using System.Text;

//namespace EventsITAcademy.Application.CustomHasher
//{
//    public class CustomPasswordHasher : IPasswordHasher<User>
//    {
//        public string HashPassword(User user, string password)
//        {
//            return GenerateHash(password);
//        }

//        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
//        {
//            if (hashedPassword == GenerateHash(providedPassword))
//            {
//                return PasswordVerificationResult.Success;
//            }

//            return PasswordVerificationResult.Failed;
//        }

//        private string GenerateHash(string input)
//        {
//            const string SECRET_KEY = "lfherffg324";
//            using (SHA512 sha = SHA512.Create())
//            {
//                byte[] bytes = Encoding.ASCII.GetBytes(input + SECRET_KEY);
//                byte[] hashBytes = sha.ComputeHash(bytes);

//                StringBuilder sb = new StringBuilder();

//                for (int i = 0; i < hashBytes.Length; i++)
//                {
//                    sb.Append(hashBytes[i].ToString("X2"));
//                }

//                return sb.ToString();
//            }
//        }
//    }
//}
