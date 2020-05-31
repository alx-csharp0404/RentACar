using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar
{
    static class GlobalData
    {
        public static MySqlConnection connection = null;
    }
}
