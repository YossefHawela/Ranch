using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project.Data
{
    public class Connector
    {
       
        public Account Login(string username,string Password)
        {
            Clipboard.SetText(GetHashedPassword(username));

            Account account = GetAccount(username);
            if (account != null)
            {
                if (!CheckPassword(account.Password, Password)) return null;
                else return account;

            }else return null;

        }
        /// <summary>
        /// Get Account from stored accounts
        /// </summary>
        private Account GetAccount(string username)
        {
            Account[] accounts = new Account[]
            {
                new Account(){UserName="admin", Password="EFflJXdh8SrUs/R7xFLWHOXDxYPNih0FyJVHfnOcfwbEKwNU",accountType = Enums.AccountType.Admin },
                new Account(){UserName="worker", Password="04A9BAGAz+rnuAPgXS+iCHV2IAfT3uLPp0JtOg1vKmxzVxr6" },
                new Account(){UserName="manager", Password="lihez4f+88SLPkZNGxSH+CVPdBg1eCSe3+9yHpgvJ3M0YIH/",accountType = Enums.AccountType.Manager }


            };
            Account account = accounts.Where(ac => ac.UserName == username).FirstOrDefault();

            return account;
        }
        /// <summary>
        /// return true if the Stored HashedPassword equals the password user entered.
        /// </summary>
        public bool CheckPassword(string HashedPassword,string Password)
        {
            byte[] hashBytes = Convert.FromBase64String(HashedPassword);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(Password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;

        }
        private string GetHashedPassword(string Password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(Password, salt, 100000);
            
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string PasswordHash = Convert.ToBase64String(hashBytes);

            return PasswordHash;
        }
    }
}
