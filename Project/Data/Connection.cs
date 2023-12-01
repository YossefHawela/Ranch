using Project.DataBase.DatabaseDataSetTableAdapters;
using Project.DataBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Models;
using System.Data;
using System.Windows.Forms;
using Project.Enums;

namespace Project.Data
{
    class Connection
    {
        SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Try\\Ranch\\Project\\DataBase\\Database.mdf;Integrated Security=True");
        DatabaseDataSet dataSet = new DatabaseDataSet();
        UserTableAdapter UserQuery = new UserTableAdapter();
        
        public void AddUser(Account account)
        {
            try
            {
                using (UserQuery.Connection = conn   /*conn*/) 
                {
                    //conn.Open();
                    //string query = $"INSERT INTO [User] VALUES('{account.accountType.ToString()}', '{account.FName}', '{account.LName}', '{account.Email}', '{account.Password}');";

                    //using (SqlCommand cmd = new SqlCommand(query, conn))
                    //{
                    //    cmd.ExecuteNonQuery();
                    //}

                    UserQuery.Connection.Open();
                    DatabaseDataSet.UserRow userRow = dataSet.User.NewUserRow();

                    userRow.Type = account.accountType.ToString();
                    userRow.Fname = account.FName;
                    userRow.Lname = account.LName;
                    userRow.Email = account.Email;
                    userRow.Password = account.Password;

                    dataSet.User.AddUserRow(userRow);

                    UserQuery.Update(dataSet.User);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void EditUser(Account account)
        {
            try
            {
                using (UserQuery.Connection = conn   /*conn*/)
                {
                    UserQuery.Connection.Open();


                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public Account Login(string email, string password) 
        {
            Account account = new Account();

            try
            {
                UserQuery.Fill(dataSet.User);

                DataRow[] foundRows = dataSet.User.Select($"Email = '{email}' AND Password = '{password}'");

                if (foundRows.Length > 0)
                {
                    account.accountType = (AccountType)getType(foundRows[0].ItemArray[1].ToString());
                    account.FName = foundRows[0].ItemArray[2].ToString();
                    account.LName = foundRows[0].ItemArray[3].ToString();
                    account.Email = foundRows[0].ItemArray[4].ToString();
                    account.Password = foundRows[0].ItemArray[5].ToString();

                    return account;
                }else return null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        public int getType(string s) 
        {
            switch (s) 
            {
                case "Worker":
                    return 0;
                    break;
                case "Admin":
                    return 1;
                    break;
                case "Manager":
                    return 2;
                    break;
                case "Owner":
                    return 3;
                    break;
                default: return -1;
            }
        }
    }
}
