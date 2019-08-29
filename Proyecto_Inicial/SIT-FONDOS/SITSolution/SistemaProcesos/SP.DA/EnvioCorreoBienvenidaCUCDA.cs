#region Descripción
/*
 * Fecha de Modificación: 26/02/2016
 * Modificado por: Irene Reyes
 * Numero de OT: 8365
 * Descripción del cambio: Creación
 * */
 /*
 * Fecha de Modificación: 03/04/2016
 * Modificado por: Irene Reyes
 * Numero de OT: 8540
 * Descripción del cambio: Modificar el tipo de parametro del código del partícipe en el método registrarEnvioCorreoBienvenida().
 * */
 
#endregion

using System;
using System.Data;
using System.Data.SqlClient;
using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
    public class EnvioCorreoBienvenidaCUCDA : INGFondos.Data.DA
    {
        public EnvioCorreoBienvenidaCUCDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        public DataTable ListarParticipesXPrimerNumeroDocumento(string fecha)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_PARTICIPES_X_FECHA_PRIMER_DOCUMENTO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
                prmFecha.Value = fecha;

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

        public void registrarEnvioCorreoBienvenida( int cuc, string usuario)
        {
            AprobacionCUCDA da = new AprobacionCUCDA();
            SqlConnection cn = GetConnection();
            cn.Open();
            SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_PARTICIPE_ACCESO_X_CORREO_BIENVENIDA", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCuc = cmd.Parameters.Add("@cuc", SqlDbType.Int);
                prmCuc.Value = cuc;

                SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
                prmUsuario.Value = usuario;

                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                cn.Close();
            }
        }

        public DataTable ObtenerDatosMailBienvenida(string cuc)
        {

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_DATOS_EMAIL_BIENVENIDA", cn);
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
        //obteniendo tabla encriptada

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

        

    }
}
