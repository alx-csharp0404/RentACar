using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentACar
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //String cs = "Server=127.0.0.1;Port=3306;Database=rent_a_car;Uid={0};Pwd={1}";
            String cs = ConfigurationManager.AppSettings["cs"];

            try
            {

                if ( String.IsNullOrWhiteSpace(tbLogin.Text) ||
                    String.IsNullOrWhiteSpace(tbPassword.Text))
                {
                    DialogHelper.E("Podaj dane do logowania");
                    return;
                }

                //zmiana kursora na "kręciołkę"
                Cursor.Current = Cursors.WaitCursor;

                cs = String.Format(cs, tbLogin.Text.Trim(), tbPassword.Text.Trim());
                GlobalData.connection = new MySqlConnection(cs);
                GlobalData.connection.Open();

                this.DialogResult = DialogResult.OK;

                Close();
            } catch (Exception exc)
            {
                DialogHelper.E(exc.Message);
            } finally
            {
                // przywrocic kursor
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
