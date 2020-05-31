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

    public partial class FormOperation : Form
    {
        public int CarId { get; set; } = 0;
        public String RegPlate { get; set; } = "";
        public bool OperBack { get; set; } = false;
        public FormOperation()
        {
            InitializeComponent();
        }

        int lastRecordId;
        private void FormOperation_Load(object sender, EventArgs e)
        {
            if (OperBack)
            {
                //zwrot pojazdu
                Text = "Zwrot pojazdu " + RegPlate;
                String sql = "SELECT id, description FROM operations where car_id={0} ORDER BY id DESC LIMIT 0,1 ";
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                sql = String.Format(sql, CarId);
                adapter.SelectCommand = new MySqlCommand(sql, GlobalData.connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count>0)
                {
                    tbDescr.Text = dt.Rows[0]["description"].ToString();
                    lastRecordId = Convert.ToInt32(dt.Rows[0]["id"]);
                }
            } else
            {
                //wydanie pojazdu
                Text = "Wydanie pojazdu " + RegPlate;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            String sql;
            int avail;
            if (OperBack==false)
            {
                // wydaj auto - dodaj rekord
                sql = @"INSERT INTO operations
                     (car_id, ts_out, mileage_out, description)
                        VALUES
                     (@car_id, @ts, @mileage, @descr)";
                avail = 0;
            } else
            {
                // przyjmij auto - zaktualizuj ostatni rekord operacji dla auta
                sql = @"UPDATE operations SET
                        ts_in=@ts, mileage_in=@mileage, description=@descr
                        WHERE id=@id";
                avail = 1;
            }

            MySqlCommand cmd = new MySqlCommand(sql, GlobalData.connection);
            cmd.Parameters.Add("@car_id", MySqlDbType.Int32).Value = CarId;
            cmd.Parameters.Add("@ts", MySqlDbType.DateTime).Value = dtDate.Value;
            cmd.Parameters.Add("@mileage", MySqlDbType.Int32).Value = numMileage.Value;
            cmd.Parameters.Add("@descr", MySqlDbType.Text).Value = tbDescr.Text;
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = lastRecordId;
            cmd.ExecuteNonQuery();

            sql = "UPDATE cars SET avail=@avail WHERE id=@id";
            cmd = new MySqlCommand(sql, GlobalData.connection);
            cmd.Parameters.Add("@avail", MySqlDbType.Int32).Value = avail;
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = CarId;
            cmd.ExecuteNonQuery();

            DialogResult = DialogResult.OK;
            Close();
            
        }
    }
}
