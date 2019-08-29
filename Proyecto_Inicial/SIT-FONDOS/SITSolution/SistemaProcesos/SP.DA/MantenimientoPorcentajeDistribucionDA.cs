/*
 * Fecha de Modificación : 12/06/2017
 * Modificado por        : Anthony Joaquin
 * Nro. Orden de Trabajo : 10478
 * Descripción del cambio: Creación.
 * */

using System;
using System.Collections.Generic;
using System.Text;
using INGFondos.Data;
using System.Data.SqlClient;
using System.Data;
using Procesos.TD;
using System.Configuration;
using INGFondos.Constants;

namespace Procesos.DA
{
    /// <summary>
    /// Descripción breve de MantenimientoPorcentajeDistribucionDA.
    /// </summary>
    public class MantenimientoPorcentajeDistribucionDA:INGFondos.Data.DA
    {
           
        public MantenimientoPorcentajeDistribucionDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        public SqlConnection GetConnection2()
        {
            return base.GetConnection();
        }

        public DataTable ListarFondo()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_FONDO_PAGO_FLUJOS", cn);
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

        public DataTable ListaPorcentajeXFondo(int idFondo)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_PERIODO_DISTRIBUCION", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmTipoDocumento = cmd.Parameters.Add("@idFondo", SqlDbType.Int);
                prmTipoDocumento.Value = idFondo;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("DISTRIBUCION_PERIODOS");
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


        public DataTable ListarMes(string codTabla)
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_TABLA_GENERAL", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoTabla = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
                prmCodigoTabla.Value = codTabla;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
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


       

        public void ActualizarDistribucion(DistribucionXPeriodo dxp, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {
           
            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_PERIODO_DISTRIBUCION", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = dxp.ID;
                cmd.Parameters.Add("@PERIODO", SqlDbType.Int).Value = dxp.Periodo;
                cmd.Parameters.Add("@PORCENTAJE_DISTRIBUCION", SqlDbType.Decimal).Value = dxp.Porcentaje;
                cmd.Parameters.Add("@FECHA_CORTE", SqlDbType.VarChar).Value = dxp.FechaCorte;
                cmd.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar).Value = codigoUsuario;
                cmd.Parameters.Add("@AREA_MODIFICACION", SqlDbType.VarChar).Value = Constants.ConstantesING.AREA_DISTRIBUCION_X_PERIODO;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }

        }


        public void RegistarDistribucion(DistribucionXPeriodo dxp, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_PERIODO_DISTRIBUCION", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int).Value = dxp.IdFondo;
                cmd.Parameters.Add("@PERIODO", SqlDbType.Int).Value = dxp.Periodo;
                cmd.Parameters.Add("@PORCENTAJE", SqlDbType.Decimal).Value = dxp.Porcentaje;
                cmd.Parameters.Add("@FECHA_CORTE", SqlDbType.VarChar).Value = dxp.FechaCorte;
                cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = codigoUsuario;
                cmd.Parameters.Add("@AREA", SqlDbType.VarChar).Value = Constants.ConstantesING.AREA_DISTRIBUCION_X_PERIODO;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }

        }

        public void EliminarDistribucion(int idDistribucionxPeriodo, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_PERIODO_DISTRIBUCION", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_PERIODO_DISTRIBUCION", SqlDbType.Int).Value = idDistribucionxPeriodo;
                cmd.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar).Value = codigoUsuario;
                cmd.Parameters.Add("@AREA_MODIFICACION", SqlDbType.VarChar).Value = Constants.ConstantesING.AREA_DISTRIBUCION_X_PERIODO;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }

        }
     
    }
}
