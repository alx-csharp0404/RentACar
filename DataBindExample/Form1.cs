using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBindExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Person> persons = new List<Person>();
        private void Form1_Load(object sender, EventArgs e)
        {
            persons.Add(new Person("Jan", "Kowalski", 55, "Zdun", false));
            persons.Add(new Person("Marek", "Nowak", 35, "Social media ninja", true));
            persons.Add(new Person("Emil", "Zatopek", 66, "Biegacz", true));
            persons.Add(new Person("Zenek", "Martyniuk", 52, "Piosenkarz", false));

            //foreach (Person item in persons)
            //{
            //    lbPersons.Items.Add(item.Fname + " " + item.Lname);
            //}

            // DataBinding - listbox
            lbPersons.DataSource = persons;
            lbPersons.DisplayMember = "FullName";

            // DataBinding - dla textbox'ów
            tbFname.DataBindings.Add(new Binding("Text", lbPersons.DataSource, "Fname"));
            tbFname.DataBindings.Add(new Binding("Enabled", lbPersons.DataSource, "Active"));

            tbLname.DataBindings.Add(new Binding("Text", lbPersons.DataSource, "Lname"));
            tbAge.DataBindings.Add(new Binding("Text", lbPersons.DataSource, "Age"));
            tbJob.DataBindings.Add(new Binding("Text", lbPersons.DataSource, "Job"));
        }

        private void lbPersons_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int index = lbPersons.SelectedIndex;
            //if (index >= 0)
            //{
            //    tbFname.Text = persons[index].Fname;
            //    tbLname.Text = persons[index].Lname;
            //    tbAge.Text = persons[index].Age.ToString();
            //    tbJob.Text = persons[index].Job;
            //}
        }
    }
}
