using Project;
using Project.Models;
using Show;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Worker
{
    public partial class Worker : Form
    {
        public Worker()
        {
            InitializeComponent();
        }

        private void Addbutton_Click(object sender, EventArgs e)
        {
            Animal animal = new Animal();   
            animal.Id = Program.conn.GetNextId<Animal>();
            animal.Type = TypetextBox.Text;
            animal.Age = Convert.ToInt32(numericUpDown1Age.Value);
            animal.Gender = radioButton1Male.Checked ? "Male" : "Female";
            animal.Price = Convert.ToSingle(numericUpDown1PriceA.Value);
            Program.conn.AddAnimal(animal);
            MessageBox.Show("Added!");
            TypetextBox.Text = string.Empty;
            numericUpDown1Age.Value = 0;
            numericUpDown1PriceA.Value = 0;

            SetAnimalsCount();

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            Animal animal = Program.conn.GetAnimal(Convert.ToInt32(numericUpDown1IDE.Value));

            if (animal != null)
            {

                Program.conn.DeleteAnimal(animal);

                MessageBox.Show(animal.Type + " Deleted");

                SetAnimalsCount();

            }


        }
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            Animal animal = Program.conn.GetAnimal(Convert.ToInt32(numericUpDown1IDE.Value));

            if (animal != null)
            {
                animal.Price = Convert.ToSingle(numericUpDown1PriceE.Value);

                Program.conn.UpdateAnimal(animal);

                MessageBox.Show(animal.Type + " Price edited");

            }
        }

        private void buttonEditAge_Click(object sender, EventArgs e)
        {
            Animal animal = Program.conn.GetAnimal(Convert.ToInt32(numericUpDown1IDE.Value));

            if (animal != null)
            {
                animal.Age = Convert.ToInt32(numericUpDownAgeE.Value);

                Program.conn.UpdateAnimal(animal);

                MessageBox.Show(animal.Type + " age edited");

            }


        }
        private void Worker_Load(object sender, EventArgs e)
        {
            SetAnimalsCount();
        }

        private void SetAnimalsCount()
        {
            int max = Program.conn.GetNextId<Animal>() - 1;

            numericUpDown1IDE.Maximum = max;
            numericUpDown1IDEA.Maximum = max;
            numericUpDown1IDD.Maximum = max;

        }


        private void numericUpDown1IDE_ValueChanged(object sender, EventArgs e)
        {

            Animal animal = Program.conn.GetAnimal(Convert.ToInt32(numericUpDown1IDE.Value));

            if(animal != null)
                numericUpDown1PriceE.Value = Convert.ToDecimal(animal.Price);
        }

        private void numericUpDown1IDEA_ValueChanged(object sender, EventArgs e)
        {
            Animal animal = Program.conn.GetAnimal(Convert.ToInt32(numericUpDown1IDEA.Value));
            
            if(animal!= null)
                numericUpDownAgeE.Value = Convert.ToDecimal(animal.Age);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Show.Show show = new Show.Show();

            show.ShowDialog();
        }
    }
}
