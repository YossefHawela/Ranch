using Admin;
using Project;
using Project.Data;
using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Worker;

namespace Login
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Loginbutton_Click(object sender, EventArgs e)
        {
            
            //Account acc = Program.conn.Login(EmailTextBox.Text.Trim(),PasswordTextBox.Text);
            Account acc = Program.connK.Login(EmailTextBox.Text.Trim(), PasswordTextBox.Text);

            if (acc != null)
            {
                Form form= new Worker.Worker();

                switch (acc.accountType)
                {
                    case Project.Enums.AccountType.Worker:
                        form = new Worker.Worker();
                        break;
                    case Project.Enums.AccountType.Admin:
                        form = new Admin.Admin();

                        break;
                    case Project.Enums.AccountType.Manager:
                        form = new Manager.Manager();
                        break;
                    case Project.Enums.AccountType.Owner:
                        break;

                }

                
                Form1.Instance.Hide();

                //to close Application after closing the from
                form.FormClosed += delegate
                {
                    Application.Exit();
                };

                form.Show();
                Close();
                
            }
            else
            {
                MessageBox.Show("Please check login data!");
            }

        }
    }
}
