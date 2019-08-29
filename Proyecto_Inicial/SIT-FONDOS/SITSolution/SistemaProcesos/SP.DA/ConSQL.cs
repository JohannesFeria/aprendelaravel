using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Configuration;

namespace SistemaProcesosDA
{
    public class ConSQL //: INGFondos.Data.DA
    {
        private string serverKey;
        private string databaseKey;

        public string Database { get {return databaseKey; } set { databaseKey = value;} }
        public string Server { get { return serverKey; } set { serverKey = value; } }

        public ConSQL(string _serverKey, string _databaseKey)
            //: base(serverKey, databaseKey)
        {
            serverKey = _serverKey;
            databaseKey = _databaseKey;
        }

        public DateTime GetServerDate()
        {
            return DateTime.Now;
        } 
        public SqlConnection GetConnection()
        {
            string i = ConfigurationManager.AppSettings[0];
            string server = ConfigurationManager.AppSettings[serverKey];
            string baseDatos = ConfigurationManager.AppSettings[databaseKey];
            string user = ConfigurationManager.AppSettings["BDUser"];
            string password = ConfigurationManager.AppSettings["BDPass"];

            // SqlConnection cn = new SqlConnection("Data Source=" + serverKey + ";Initial Catalog=" + databaseKey + "; user id = usuario_sit;password = usuario_sit;");
            SqlConnection cn = new SqlConnection("Data Source=" + server + ";Initial Catalog=" + baseDatos + "; user id = " + user + ";password = " + password + ";");
            return cn;
        }     
    }
}
