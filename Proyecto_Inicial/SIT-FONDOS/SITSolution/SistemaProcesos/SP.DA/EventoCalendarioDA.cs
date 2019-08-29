/*
 * Fecha de Modificación: 16/07/2013
 * Modificado por: Davis Rixi
 * Numero de OT: 5643
 * Descripción del cambio: Creación
 * */

using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{

    public class EventoCalendarioDA : INGFondos.Data.DA
    {
        public EventoCalendarioDA() : base(INGFondos.Constants.Conexiones.ServidorComercial, INGFondos.Constants.Conexiones.BaseDeDatosComercial) { }

        public DataTable ObtenerPromotoresEvento()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("WMProsp.WMPR_OBT_ASESORES_EVENTO_CALENDARIO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("LISTA");
            da.Fill(dt);
            return dt;
        }

        public void ActualizarActividadIdCalendar(int idActividad, string idCalendar)
        {

            SqlConnection cn = GetConnection();
      
            SqlCommand cmd = new SqlCommand("WMProsp.WMPR_ACT_ACTIVIDAD_ID_CALENDAR", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdActividad = cmd.Parameters.Add("@CORRELATIVO_ACTIVIDAD", SqlDbType.Int);
            prmIdActividad.Value = idActividad;

            SqlParameter prmIdCalendar = cmd.Parameters.Add("@ID_CALENDAR", SqlDbType.VarChar);
            prmIdCalendar.Value = idCalendar;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Dispose();
                cn.Close();
            }

        }


    }
}
