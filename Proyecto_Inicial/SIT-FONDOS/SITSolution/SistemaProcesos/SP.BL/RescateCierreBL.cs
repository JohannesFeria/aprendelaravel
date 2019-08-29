/*
* Fecha de Modificación		: 04/10/2017
* Modificado por			: Rosmery Contreras
* Numero de OT				: 10808
* Descripción del cambio	: Creacion.
* */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Procesos.DA;
using System.Data;
using System.Data.SqlClient;

using Procesos.TD;

namespace Procesos.BL
{
    public class RescateCierreBL
    {
        RescateCierreDA rescateCierreDA = new RescateCierreDA();

        public void GrabarCierrexFondo(RescateCierre rescateCierre)
        {
            RescateCierreDA da = new RescateCierreDA();               
            SqlConnection cn = da.GetConnection2();
            cn.Open();
            try
            {
                da.GrabarCierrexFondo(rescateCierre);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Dispose();
                cn.Close();
            } 

         }

		public DataTable ObtenerPrecierre_Fecha(int idFondo, DateTime fecha)
		{	
			RescateCierreDA da = new RescateCierreDA();
			SqlConnection cn = da.GetConnection2();
			return da.ObtenerPrecierre_Fecha(idFondo,fecha);
			
		}

		public DataTable ListarCierre_Fecha(string fechaRegistro, int idFondo)
		{
			RescateCierreDA rescateCierreDA = new RescateCierreDA();
			return rescateCierreDA.ListarCierre_Fecha(fechaRegistro,idFondo);
		}
     }
}
