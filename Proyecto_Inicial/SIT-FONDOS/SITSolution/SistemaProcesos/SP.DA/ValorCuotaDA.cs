/*
 * Fecha Modificación:		15/08/2012
 * Modificado por:			Robert Castillo
 * Nro de OT:				4792 PSC1
 * Descripción del cambio:	Se crea la clase DA para actualizar el valor cuota de los fondos.
* */
/*
 * Fecha de Modificación: 28/11/2012
 * Modificado por: Robert Castillo
 * Numero de OT: 5117
 * Descripción del cambio: Se cambia namespace
 * */
/*
 * Fecha de Modificación : 14/06/2016
 * Modificado por        : Juan Castro
 * Nro. Orden de Trabajo : 8844
 * Descripción del cambio: Se agrega parámetro para determinar si se graba el VC aprobado o no en los métodos de inserción y actualización.
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
// using Procesos.TD;

namespace SistemaProcesosDA
{
	/// <summary>
	/// Descripción breve de ValorCuotaDA.
	/// </summary>
    public class ValorCuotaDA : INGFondos.Data.DA // : ConSQL
	{
		public ValorCuotaDA(): base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) {}

        //public DataTable ObtenerFondosPrecierre()
        //{
        //    try
        //    {
        //        SqlConnection cn = GetConnection();
        //        SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_PRECIERRE_FONDOS", cn);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable("FONDO");
        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch
        //    { 
        //        return null;
        //    }
        //}

        public DataTable obtenerValorCuota(string fecha, int idFondo)
        {
            try
            {
                SqlConnection cn = GetConnection();
                SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_VALOR_CUOTA_FECHA", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.VarChar);
                prmFecha.Value = fecha;

                SqlParameter prmIdFondo = cmd.Parameters.Add("@idfondo", SqlDbType.Int);
                prmIdFondo.Value = idFondo;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("VALOR_CUOTA");
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
        }

        /*INICIO OT8844: Se agrega parámetro de aprobado al método*/
        public void actualizarValorCuota(string idFondo, double valorCuota, string fecha, string usuario, string area, bool aprobado)
        {
            SqlConnection cn = GetConnection();
            SqlTransaction tr = null;

            try
            {
                cn.Open();
                tr = cn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("dbo.FOND_ACT_VALOR_CUOTA", cn, tr);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.VarChar);
                prmIdFondo.Value = idFondo;

                SqlParameter prmValorCuota = cmd.Parameters.Add("@valorCuota", SqlDbType.Decimal);
                prmValorCuota.Value = valorCuota;

                SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.VarChar);
                prmFecha.Value = fecha;

                SqlParameter prmFlagNivel1 = cmd.Parameters.Add("@flagNivel1", SqlDbType.VarChar);
                /*INICIO 8844*/
                if (!aprobado)
                {
                    prmFlagNivel1.Value = "S";
                }
                else
                {
                    prmFlagNivel1.Value = "N";
                }
                /*FIN 8844*/

                SqlParameter prmFlagNivel2 = cmd.Parameters.Add("@flagNivel2", SqlDbType.VarChar);
                prmFlagNivel2.Value = "N";

                SqlParameter prmUser = cmd.Parameters.Add("@user", SqlDbType.VarChar);
                prmUser.Value = usuario;

                SqlParameter prmArea = cmd.Parameters.Add("@area", SqlDbType.VarChar);
                prmArea.Value = "";

                cmd.ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                throw ex;
            }
            finally
            {
                cn.Close();
            }
        }
        /*FIN OT8844*/

        /*INICIO OT8844: Se agrega parámetro de aprobado al método*/
        public void insertarValorCuota(string fecha, double valorCuota, string usuario, string area, int idFondo, bool aprobado)
        {
            SqlConnection cn = GetConnection();
            SqlTransaction tr = null;

            try
            {
                cn.Open();
                tr = cn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("dbo.FOND_INS_VALOR_CUOTA", cn, tr);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.VarChar);
                prmFecha.Value = fecha;

                SqlParameter prmValorCuota = cmd.Parameters.Add("@valorCuota", SqlDbType.Decimal);
                prmValorCuota.Value = valorCuota;

                SqlParameter prmFlagNivel1 = cmd.Parameters.Add("@flagNivel1", SqlDbType.VarChar);
                /*INICIO 8844*/
                if (!aprobado)
                {
                    prmFlagNivel1.Value = "S";
                }
                else
                {
                    prmFlagNivel1.Value = "N";
                }
                /*FIN 8844*/

                SqlParameter prmFlagNivel2 = cmd.Parameters.Add("@flagNivel2", SqlDbType.VarChar);
                prmFlagNivel2.Value = "N";

                SqlParameter prmUsuario = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
                prmUsuario.Value = usuario;

                SqlParameter prmArea = cmd.Parameters.Add("@areaCreacion", SqlDbType.VarChar);
                prmArea.Value = "";

                SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Int);
                prmIdFondo.Value = idFondo;

                cmd.ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                throw ex;
            }
            finally
            {
                cn.Close();
            }
        }
        /*FIN OT8844*/

        public DataTable obtenerUsuario(string usuario)
        {
            try
            {
                SqlConnection cn = GetConnection();
                SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_USUARIO", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
                prmUsuario.Value = usuario;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("USUARIO");
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
        }
	}
}
