using DGVPrinterHelper;
using Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Show
{
    public partial class Show : Form
    {
        public Show()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Program.conn.GetAnimals();
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            DGVPrinter dGVPrinter = new DGVPrinter();
            dGVPrinter.Title = "Animals Report";
            dGVPrinter.SubTitle = "Date: "+DateTime.Now.ToShortDateString();
            dGVPrinter.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            dGVPrinter.PageNumbers = true;
            dGVPrinter.PageNumberInHeader = false;
            dGVPrinter.PorportionalColumns= true;
            dGVPrinter.HeaderCellAlignment = StringAlignment.Near;
            dGVPrinter.Footer = "Wandering Codes Team";
            dGVPrinter.FooterSpacing = 15;
            dGVPrinter.PrintDataGridView(dataGridView1);
        }
    }
}
