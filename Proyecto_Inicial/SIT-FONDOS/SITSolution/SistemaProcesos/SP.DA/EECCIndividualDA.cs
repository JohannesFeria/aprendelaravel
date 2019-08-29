/*
 * Fecha de Creación    : 30/05/2016
 * Modificado por           : Irene Reyes
 * Numero de OT             : 8829
 * Descripción del cambio   : Creación
 *                            
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
    public class EECCIndividualDA : INGFondos.Data.DA
    {
        public EECCIndividualDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }


        public DataTable ObtenerFondos()
        {

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_FONDO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("FONDOS");
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                cn.Close();
            }
        }

        public decimal ObtenerValorCuota(decimal idFondo, DateTime fecha)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_VALOR_CUOTA_PRECIERRE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            decimal valorCuota = 0;
            try
            {
                cn.Open();
                valorCuota = Convert.ToDecimal(cmd.ExecuteScalar());
            }
            finally
            {
                cmd.Dispose();
                cn.Close();
            }

            return valorCuota;
        }

        public DataTable ObtenerParticipeIndividual(decimal idFondo, DateTime fechaInicio, DateTime fechaFin, decimal idParticipe)
        {

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_PARTICIPE_EECC", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmFondo.Value = idFondo;

            SqlParameter prmParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
            prmParticipe.Value = idParticipe;

            SqlParameter prmFecIni = cmd.Parameters.Add("@fechaInicio", SqlDbType.DateTime);
            prmFecIni.Value = fechaInicio;

            SqlParameter prmFecFin = cmd.Parameters.Add("@fechaFin", SqlDbType.DateTime);
            prmFecFin.Value = fechaFin;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("PARTICIPESIND");
            da.Fill(dt);
            return dt;
        }

        public DataTable ObtenerMacomunos(int codigoParticipe)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_PARTICIPES_MANCOMUNOS", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmCodigo = cmd.Parameters.Add("@codigoCuentaMancomunada", SqlDbType.Int);
            prmCodigo.Value = codigoParticipe;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("MANCOMUNOS");
            da.Fill(dt);
            return dt;
        }

        public decimal ObtenerSaldoEnFondo(decimal idFondo, decimal idParticipe, DateTime fecha)
        {

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_SALDO_FONDO_EECC", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmFondo.Value = idFondo;

            SqlParameter prmParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
            prmParticipe.Value = idParticipe;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            decimal saldo = 0;

            cn.Open();

            try
            {
                saldo = Convert.ToDecimal(cmd.ExecuteScalar());
            }
            finally
            {
                cmd.Dispose();
                cn.Close();
            }

            return saldo;
        }

        public DataTable ObtenerCuentas(decimal idParticipe)
        {

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_CUENTA_PARTICIPE_EECC", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
            prmParticipe.Value = idParticipe;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("CUENTAS");
            da.Fill(dt);
            return dt;
        }

        public DataTable ObtenerMovimientos(decimal idFondo, decimal idCuentaParticipacion, DateTime fechaInicio, DateTime fechaFin)
        {

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_OPERACIONES_EECC", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmFondo.Value = idFondo;

            SqlParameter prmCuenta = cmd.Parameters.Add("@idCuentaParticipacion", SqlDbType.Decimal);
            prmCuenta.Value = idCuentaParticipacion;

            SqlParameter prmFecIni = cmd.Parameters.Add("@fechaInicio", SqlDbType.DateTime);
            prmFecIni.Value = fechaInicio;

            SqlParameter prmFecFin = cmd.Parameters.Add("@fechaFin", SqlDbType.DateTime);
            prmFecFin.Value = fechaFin;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("MOVIMIENTOS");
            da.Fill(dt);
            return dt;
        }

        public decimal ObtenerSaldoEnCuenta(decimal idFondo, decimal idCuentaParticipacion, DateTime fecha)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_SALDO_CUENTA_EECC", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
            prmFondo.Value = idFondo;

            SqlParameter prmCuenta = cmd.Parameters.Add("@idCuentaParticipacion", SqlDbType.Decimal);
            prmCuenta.Value = idCuentaParticipacion;

            SqlParameter prmFecha = cmd.Parameters.Add("@fechaCierre", SqlDbType.DateTime);
            prmFecha.Value = fecha;

            decimal saldo = 0;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cn.Open();

            try
            {
                saldo = Convert.ToDecimal(cmd.ExecuteScalar());

            }
            finally
            {
                cmd.Dispose();
                cn.Close();
            }

            return saldo;
        }


    }
}
