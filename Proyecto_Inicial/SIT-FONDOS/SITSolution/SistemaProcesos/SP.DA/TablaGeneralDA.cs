/*
 * Fecha de Modificación: 02/06/2016
 * Modificado por: Juan Castro
 * Numero de OT: 8844
 * Descripción del cambio: Creación
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SistemaProcesosDA
{
    public class TablaGeneralDA : INGFondos.Data.DA // : ConSQL
    {
        public TablaGeneralDA() : base(INGFondos.Constants.Conexiones.ServidorTributacion, INGFondos.Constants.Conexiones.BaseDeDatosTributacion) { }

        /// <summary>
        /// Obtiene la lista de registros de una tabla lógica.
        /// </summary>
        /// <param name="codigoTabla">Código de la tabla lógica que se desea consultar.</param>
        /// <returns>DataTable: Tabla con los registros de la tabla lógica.</returns>
        public DataTable ListarTablaGeneral(string codigoTabla)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMTR_LIS_TABLA_GENERAL", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmCodigo = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar, 20);
            prmCodigo.Value = codigoTabla;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("LISTA");
            da.Fill(dt);
            return dt;
        }
    }
}
