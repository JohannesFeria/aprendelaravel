/*
 * Fecha Modificación:		16/09/2013
 * Modificado por:			Robert Castillo
 * Nro de OT:				5767
 * Descripción del cambio:	Se crea la clase DA para generar el archivo de Convenios para Plan Empleador.
* */
/*
 * Fecha de Modificación:	27/02/2014
 * Modificado por:			Giovana Véliz
 * Nro. Orden de trabajo:   6110
 * Descripción del cambio:	Se agrega método ObtenerDetalleConvenios para obtener los detalles por concepto de los convenios.
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;
using System.Configuration;

namespace Procesos.DA
{
    /// <summary>
    /// Descripción breve de ValorCuotaDA.
    /// </summary>
    public class ConveniosPlanEmpleadorDA : INGFondos.Data.DA
    {
        public ConveniosPlanEmpleadorDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        public DataTable ObtenerTablaGeneral(string codigoTabla)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_TABLA_GENERAL", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter prmCod = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
            prmCod.Value = codigoTabla;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("TablaGeneral");
            da.Fill(dt);
            return dt;
        }

        public DataTable ListarFondos(string estado)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_FONDO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter prmCod = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
            prmCod.Value = estado;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("TablaGeneral");
            da.Fill(dt);
            return dt;
        }

        public DataTable ObtenerTablaGeneralDepositos(string codigoTabla)
        {
            ConnectionManager cm = new ConnectionManager();

            string server = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorBancos];
            string database = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosBancos];

            SqlConnection cn = cm.GetTrustedConnection(server, database);

            SqlCommand cmd = new SqlCommand("dbo.DEPO_LIS_TABLA_GENERAL", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmCod = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
            prmCod.Value = codigoTabla;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("TablaGeneral");
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Obtiene los detalles de los convenios a reportar en un periodo. El detalle generado es por concepto de pago.
        /// </summary>
        /// <param name="codigoEmpresa">Especifica el código de la empresa cuyos convenios se desea detallar.</param>
        /// <param name="mes">Mes de consulta</param>
        /// <param name="anio">Año de consulta</param>
        /// <returns>Datatable: Tabla con los detalles por concepto de los convenios reportados.</returns>
        public DataTable ObtenerDetalleConvenios(string codigoEmpresa, int mes, int anio)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_CONVENIO_DETALLE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmCod = cmd.Parameters.Add("@codigoEmpresa", SqlDbType.VarChar);
            prmCod.Value = codigoEmpresa;

            SqlParameter prmMes = cmd.Parameters.Add("@mes", SqlDbType.Int);
            prmMes.Value = mes;

            SqlParameter prmAnio = cmd.Parameters.Add("@anio", SqlDbType.Int);
            prmAnio.Value = anio;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("DETALLE");
            da.Fill(dt);
            return dt;
        }
    }

}