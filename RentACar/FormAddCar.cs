using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentACar
{
    public partial class FormAddCar : Form
    {
        public FormAddCar()
        {
            InitializeComponent();
        }

        private void FormAddCar_Load(object sender, EventArgs e)
        {
            LoadDictionaryData();
        }

        BindingSource bsBrands = new BindingSource();
        BindingSource bsModels = new BindingSource();
        BindingSource bsTypes = new BindingSource();
        private void LoadDictionaryData()
        {
            try
            {
                // ładowanie słownika marek (brendów)
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                String sql = " select id, name FROM car_brands ORDER BY name  ";
                adapter.SelectCommand = new MySqlCommand(sql, GlobalData.connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                bsBrands.DataSource = dt;
                cbBrands.DataSource = bsBrands;
                cbBrands.DisplayMember = "name";
                cbBrands.ValueMember = "id";
                cbBrands.SelectedIndex = -1;
                cbBrands.SelectedIndexChanged += CbBrands_SelectedIndexChanged;

                //ładowanie słownika modeli
                sql = "SELECT * FROM car_models ORDER BY NAME ";
                adapter = new MySqlDataAdapter();
                adapter.SelectCommand = new MySqlCommand(sql, GlobalData.connection);
                dt = new DataTable();
                adapter.Fill(dt);

                bsModels.DataSource = dt;
                cbModels.DataSource = bsModels;
                cbModels.DisplayMember = "name";
                cbModels.ValueMember = "id";
                cbModels.SelectedIndex = -1;
                cbModels.Enabled = false;

                //ładowanie słownika własnosci
                sql = "SELECT * FROM car_types ORDER BY NAME ";
                adapter = new MySqlDataAdapter();
                adapter.SelectCommand = new MySqlCommand(sql, GlobalData.connection);
                dt = new DataTable();
                adapter.Fill(dt);

                bsTypes.DataSource = dt;
                cbTypes.DataSource = bsTypes;
                cbTypes.DisplayMember = "name";
                cbTypes.ValueMember = "id";
                cbTypes.SelectedIndex = -1;
                
            } catch (Exception exc)
            {
                DialogHelper.E(exc.Message);
            }
        }

        private void CbBrands_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBrands.SelectedIndex>-1)
            {
                bsModels.Filter = "brand_id=" + cbBrands.SelectedValue;
                cbModels.DataSource = bsModels;
                cbModels.Enabled = true;
                cbModels.SelectedIndex = -1;
            }
        }

        private void tbRegPlate_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private String pictureFileName = null;
        private void btnInsertPic_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Pliki graficzne|*.png;*.jpg;*.jpeg;*.bmp|Pliki GIF|*.gif";
            if (dialog.ShowDialog()==DialogResult.OK)
            {
                //ladujemy plik
                picCar.Image = new Bitmap(dialog.FileName);
                pictureFileName = dialog.FileName;
            }
        }

        private void btnRemovePic_Click(object sender, EventArgs e)
        {
            if (picCar.Image!=null)
            {
                picCar.Image.Dispose();
                picCar.Image = null;
                pictureFileName = null;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                // zapis do bazy
                SaveData();
            }

        }

        private void SaveData()
        {
            try
            {
                String sql = @"
                INSERT INTO cars
                 (model_id, type_id, registration_plate, engine, manufacturer_year, avail, image, fuel)
                 VALUES
                 (@model_id, @type_id, @reg_plate, @engine, @year, 1, @image, @fuel)
                ";
                MySqlCommand cmd = new MySqlCommand(sql, GlobalData.connection);
                cmd.Parameters.Add("@model_id", MySqlDbType.Int32);
                cmd.Parameters.Add("@type_id", MySqlDbType.Int32);
                cmd.Parameters.Add("@reg_plate", MySqlDbType.VarChar, 50);
                cmd.Parameters.Add("@year", MySqlDbType.Int32);
                cmd.Parameters.Add("@engine", MySqlDbType.Int32);
                cmd.Parameters.Add("@fuel", MySqlDbType.VarChar, 10);
                cmd.Parameters.Add("@image", MySqlDbType.MediumBlob);

                cmd.Parameters["@model_id"].Value = cbModels.SelectedValue;
                cmd.Parameters["@type_id"].Value = cbTypes.SelectedValue;
                cmd.Parameters["@reg_plate"].Value = tbRegPlate.Text.Trim();
                cmd.Parameters["@year"].Value = numYear.Value;
                cmd.Parameters["@engine"].Value = numEngine.Value;
                cmd.Parameters["@fuel"].Value = cbFuel.SelectedItem; //!!!!

                if (pictureFileName != null && File.Exists(pictureFileName))
                {
                    cmd.Parameters["@image"].Value = File.ReadAllBytes(pictureFileName);
                }
                else
                {
                    cmd.Parameters["@image"].Value = null;
                }

                cmd.ExecuteNonQuery();

                DialogResult = DialogResult.OK;
                Close();
            } catch (Exception exc)
            {
                DialogHelper.E(exc.Message);
            }

        }

        private bool ValidateData()
        {
            if (cbModels.SelectedIndex>-1 &&
                cbTypes.SelectedIndex>-1 &&
                cbFuel.SelectedIndex>-1 &&
                tbRegPlate.Text.Replace(" ","").Length>=3 )
            {
                return true;
            } else
            {
                DialogHelper.E("Sprawdź pola formularza");
                return false;
            }
        }
    }
}
