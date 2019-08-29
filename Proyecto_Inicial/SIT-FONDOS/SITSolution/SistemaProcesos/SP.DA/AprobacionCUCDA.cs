/*
 * Fecha de Modificación: 08/07/2015
 * Modificado por: Walter Rodríguez
 * Numero de OT: 7235
 * Descripción del cambio: Creación de la clase.
 * */
/*
 * Fecha de Modificación: 06/11/2015
 * Modificado por: Robert Castillo
 * Numero de OT: 7940
 * Descripción del cambio: Se realizan las siguientes modificaciones:
                           Se agrega método ObtenerTablaGeneral que se encarga de llamar al 
 *                         procedure FMPR_OBT_TABLA_GENERAL.
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
    public class AprobacionCUCDA : INGFondos.Data.DA
    {
        public AprobacionCUCDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        public DataTable ListarParticipesPendientes()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_PARTICIPE_PENDIENTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("PARTICIPES");
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void ActualizarAprobacionParticipe(DataRow dr, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_APROBACION_PARTICIPE", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmIdentificador = cmd.Parameters.Add("@id", SqlDbType.Int);
                prmIdentificador.Value = Convert.ToInt32(dr["ID"]);

                SqlParameter prmFlagOperaciones = cmd.Parameters.Add("@flagOperaciones", SqlDbType.VarChar);
                prmFlagOperaciones.Value = dr["FLAG_APRUEBA_OPERACIONES"].ToString();

                SqlParameter prmFlagOFC = cmd.Parameters.Add("@flagOFC", SqlDbType.VarChar);
                prmFlagOFC.Value = dr["FLAG_APRUEBA_OFC"].ToString();

                SqlParameter prmObsOperaciones = cmd.Parameters.Add("@obsOperaciones", SqlDbType.VarChar);
                prmObsOperaciones.Value = dr["OBSERVACIONES_OPERACIONES"].ToString();

                SqlParameter prmObsOFC = cmd.Parameters.Add("@obsOFC", SqlDbType.VarChar);
                prmObsOFC.Value = dr["OBSERVACIONES_OFC"].ToString();

                SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
                prmUsuario.Value = codigoUsuario;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String ObtenerClaveEncriptada()
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_LLAVE_3DES", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cn.Open();
                SqlParameter prmClaveEncriptada = cmd.Parameters.Add("@VAL", SqlDbType.VarChar, 200);
                prmClaveEncriptada.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                return prmClaveEncriptada.Value.ToString();
            }
            catch (Exception e)
            {
                //return null;

                throw e;
            }
            finally
            {
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
            }
        }

        public DataTable ObtenerDatosBienvenida(string cuc)
        {

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_DATOS_PARTICIPE_APROBADO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCuc = cmd.Parameters.Add("@cuc", SqlDbType.VarChar);
                prmCuc.Value = cuc;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("PARTICIPES");
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
            }

        }

        public DataTable ObtenerTablaGeneral(string codigoTabla, string llaveTabla)
        {

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_TABLA_GENERAL", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoTabla = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
                prmCodigoTabla.Value = codigoTabla;

                SqlParameter prmLlaveTabla = cmd.Parameters.Add("@llaveTabla", SqlDbType.VarChar);
                prmLlaveTabla.Value = llaveTabla;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("TABLA_GENERAL");
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
            }

        }
    }
}
