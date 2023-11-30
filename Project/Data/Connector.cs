using Project.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Data;
using Project.DataBase;
using System.IO;
using Project.Enums;


namespace Project.Data
{
    public class Connector
    {
        private DatabaseDataSet dataSet = new DatabaseDataSet();
        public Connector()
        {
            Load();
            AddInitialData();
        }

        ~Connector()
        {
            Save();
        }

        private void Load()
        {

            if (File.Exists(Path.Combine("Data", "db.xml")))
            {
                dataSet.ReadXml(Path.Combine("Data", "db.xml"));

            }


        }
        private void Save()
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }


            dataSet.WriteXml(Path.Combine("Data", "db.xml"));

        }

        public void AddInitialData()
        {
            if (dataSet.Tables["User"].Rows.Count < 3)
            {
                Account account = new Account();

                account.UserId = 1;
                account.accountType = AccountType.Admin;
                account.FName = "Admin";
                account.LName = "Admin";
                account.Email = "admin";
                account.Password = "admin";

                Account account2 = new Account();

                account2.UserId = 2;
                account2.accountType = AccountType.Worker;
                account2.FName = "Worker";
                account2.LName = "Worker";
                account2.Email = "worker";
                account2.Password = "worker";

                Account account3 = new Account();

                account3.UserId = 3;
                account3.accountType = AccountType.Manager;
                account3.FName = "Manager";
                account3.LName = "Manager";
                account3.Email = "manager";
                account3.Password = "manager";

                AddUser(account);
                AddUser(account2);
                AddUser(account3);

            }

        }
        public void AddUser(Account account)
        {
            DataTable Userstable = dataSet.Tables["User"];

             
            string HashPassword = GetHashedPassword(account.Password);
            
            try
            {
                Userstable.Rows.Add(account.UserId, account.accountType.ToString(), account.FName, account.LName, account.Email, HashPassword);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public Account[] GetUsers()
        {
            DataTable Userstable = dataSet.Tables["User"];
            Account[] accounts = (from row in Userstable.AsEnumerable() select new Account()
                               {
                                   UserId = (int)row.ItemArray[0],
                                   accountType = (AccountType)Enum.Parse(typeof(AccountType), row.ItemArray[1].ToString()),
                                   FName = row.ItemArray[2].ToString(),
                                   LName = row.ItemArray[3].ToString(),
                                   Email = row.ItemArray[4].ToString(),
                                   Password = row.ItemArray[5].ToString()
                               }).ToArray();
            return accounts;
        }

        public void AddAnimal(Animal animal)
        {
            DataTable Animalstable = dataSet.Tables["Animal"];

            try
            {
                Animalstable.Rows.Add(animal.Id, animal.Type, animal.Gender, animal.Age, animal.Price);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void UpdateAnimal(Animal animal)
        {
            DataTable Animalstable = dataSet.Tables["Animal"];

            DataRow Animalrow = (from row in Animalstable.AsEnumerable() where (int)row.ItemArray[0]==animal.Id select row).FirstOrDefault();

            if (Animalrow != null)
            {
                Animalstable.Rows.Remove(Animalrow);

                AddAnimal(animal);
            }


        }
        public void DeleteAnimal(Animal animal)
        {
            DataTable Animalstable = dataSet.Tables["Animal"];

            DataRow Animalrow = (from row in Animalstable.AsEnumerable() where (int)row.ItemArray[0] == animal.Id select row).FirstOrDefault();

            if (Animalrow != null)
            {
                Animalstable.Rows.Remove(Animalrow);
            }
        }

        public Animal[] GetAnimals()
        {
            DataTable AnimalsTable = dataSet.Tables["Animal"];
            Animal[] animals = (from row in AnimalsTable.AsEnumerable()select new Animal()
                                  {
                                      Id = (int)row.ItemArray[0],
                                      Type  = row.ItemArray[1].ToString(),
                                      Gender = row.ItemArray[2].ToString(),
                                      Age = (int)row.ItemArray[3],
                                      Price = (float)row.ItemArray[4]
                                  }).ToArray();
            return animals;
        }
        public DataRow[] GetAnimalsRows()
        {
            DataTable AnimalsTable = dataSet.Tables["Animal"];
            DataRow[] animals = (from row in AnimalsTable.AsEnumerable() select row).ToArray();
                               
            return animals;
        }

        public Animal GetAnimal(int id)
        {
            DataTable AnimalsTable = dataSet.Tables["Animal"];
            Animal animal = (from row in AnimalsTable.AsEnumerable()
                             where (int)row.ItemArray[0] ==id
                                select new Animal()
                                {
                                    Id = (int)row.ItemArray[0],
                                    Type = row.ItemArray[1].ToString(),
                                    Gender = row.ItemArray[2].ToString(),
                                    Age = (int)row.ItemArray[3],
                                    Price = (float)row.ItemArray[4]
                                }).FirstOrDefault();
            return animal;
        }
        public int GetNextId<T>()
        {
            DataTable table;
            if(typeof(T) == typeof(Animal))
            {
                table = dataSet.Tables["Animal"];
            }
            else
            {
                table = dataSet.Tables["User"];
            }

            int maxid = 0;

            if(table.Rows.Count>0)
            {
                maxid = table.AsEnumerable().Max(row => (int)row.ItemArray[0]);
            }

            return maxid+1;
        }

        public Account Login(string email,string Password)
        {
            Account account = GetAccount(email);
            if (account != null)
            {
                if (!CheckPassword(account.Password, Password)) return null;
                else return account;

            }else return null;

        }
        /// <summary>
        /// Get Account from stored accounts
        /// </summary>
        private Account GetAccount(string email)
        {
            DataTable Userstable = dataSet.Tables["User"];

            Account account = (from row in Userstable.AsEnumerable() where row.ItemArray[4].ToString()==email select new Account() 
            {
                UserId = (int)row.ItemArray[0],
                accountType = (AccountType)Enum.Parse(typeof(AccountType), row.ItemArray[1].ToString()),
                FName = row.ItemArray[2].ToString(),
                LName= row.ItemArray[3].ToString(),
                Email = row.ItemArray[4].ToString(),
                Password = row.ItemArray[5].ToString()
            }).FirstOrDefault();

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
