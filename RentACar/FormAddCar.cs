using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    }
}
