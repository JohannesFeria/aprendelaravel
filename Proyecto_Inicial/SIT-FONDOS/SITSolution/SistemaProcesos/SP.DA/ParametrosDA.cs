/*
 * Fecha de Modificación: 28/11/2014
 * Modificado por: Alejandro Quiñones Rojas
 * Numero de OT: 6986
 * Descripción del cambio: Creación
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
    public class ParametrosDA : INGFondos.Data.DA
    {
        public ParametrosDA() : base(INGFondos.Constants.Conexiones.ServidorSAP, INGFondos.Constants.Conexiones.BaseDeDatosSAP) { }

        public DataTable RegistrarParametro(DataTable pTablaParametros)
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_PARAMETROS_GENINTEERFACES", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.Parameters.Add("@CODTABLA", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["CODIGOTABLA"]);
                
                cmd.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["CODIGOUNICO"]);
                cmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["DESCRIPCION"]);
                cmd.Parameters.Add("@CAMPO1", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["CAMPO1"]);
                cmd.Parameters.Add("@CAMPO2", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["CAMPO2"]);
                cmd.Parameters.Add("@CAMPO3", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["CAMPO3"]);
                cmd.Parameters.Add("@CODIGOUSUARIO", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["USUARIO_CREACION"]);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("PARAMETROSGI");

                da.Fill(dt);

                return dt;

            }
            catch (Exception ex) { throw ex; }
            finally { cmd.Dispose(); }
        }

        public DataTable ActualizarParametro(DataTable pTablaParametros)
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_PARAMETROS_GENINTEERFACES", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.Parameters.Add("@CODTABLA", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["CODIGOTABLA"]);

                cmd.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["CODIGOUNICO"]);
                cmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["DESCRIPCION"]);
                cmd.Parameters.Add("@CAMPO1", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["CAMPO1"]);
                cmd.Parameters.Add("@CAMPO2", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["CAMPO2"]);
                cmd.Parameters.Add("@CAMPO3", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["CAMPO3"]);
                cmd.Parameters.Add("@CODIGOUSUARIO", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["USUARIO_CREACION"]);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("PARAMETROSGI");

                da.Fill(dt);

                return dt;

            }
            catch (Exception ex) { throw ex; }
            finally { cmd.Dispose(); }
        }

        public DataTable EliminarParametro(DataTable pTablaParametros)
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_PARAMETROS_GENINTEERFACES", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.Parameters.Add("@CODTABLA", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["CODIGOTABLA"]);
                cmd.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["CODIGOUNICO"]);
                cmd.Parameters.Add("@CODIGOUSUARIO", SqlDbType.VarChar).Value = Convert.ToString(pTablaParametros.Rows[0]["USUARIO_CREACION"]);


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("PARAMETROSGI");

                da.Fill(dt);
                return dt;
            }
            catch (Exception ex) { throw ex; }
            finally { cmd.Dispose(); }
        }

        public DataTable ObtenerParametros(string CodTabla)
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_PARAMETROS_GENINTEERFACES", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodTabla = cmd.Parameters.Add("@CODTABLA", SqlDbType.VarChar);
                prmCodTabla.Value = CodTabla;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("PARAMETRO_GI");
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

        public DataTable ObtenerValoresParametros(string codigoTabla, string codigoUnico)
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_VALORES_PARAMETRO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar).Value = codigoTabla;
                cmd.Parameters.Add("@codigoUnico", SqlDbType.VarChar).Value = codigoUnico;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("PARAMETRO");
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

    }
}
