using Project;
using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CreatU
{
    public partial class CreatU : Form
    {
        public CreatU()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            Account account = new Account();

            account.FName = textBox1.Text;
            account.LName = textBox5.Text;
            account.Email = textBox3.Text;
            account.Password = textBox2.Text;
            account.accountType = (Project.Enums.AccountType)comboBox1.SelectedIndex;

            Program.connK.AddUser(account);
        }
    }
}
