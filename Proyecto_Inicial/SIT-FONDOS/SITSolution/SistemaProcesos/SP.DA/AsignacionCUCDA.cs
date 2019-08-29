/*
 * Fecha de Modificación: 06/05/2015
 * Modificado por: Walter Rodríguez
 * Numero de OT: 7235
 * Descripción del cambio: Creación de la clase.
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
    public class AsignacionCUCDA : INGFondos.Data.DA
    {
        public AsignacionCUCDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        public DataTable ListarProspectosPorLote(string codigoLote)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_PROSPECTOS_X_LOTE", cn);            
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoLote = cmd.Parameters.Add("@codigoLote", SqlDbType.VarChar);
                prmCodigoLote.Value = codigoLote;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("LOTE");
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

        public DataTable ListarLotes()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_LOTE_GENERACION", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("LOTES");
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

        public DataTable ListarTablaGeneral(string codigoTabla)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_TABLA_GENERAL", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoTabla = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
                prmCodigoTabla.Value = codigoTabla;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("TABLA");
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

        public DataTable ListarPromotores()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_PROMOTOR", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("PROMOTORES");
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

        public DataTable ListarPromotoresPorAgencia(string codigoAgencia)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_PROMOTOR_X_AGENCIA_ORIGEN", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoAgencia = cmd.Parameters.Add("@codigoAgenciaOrigen", SqlDbType.VarChar);
                prmCodigoAgencia.Value = codigoAgencia;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("TABLA");
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

        public void InsertarAgenciaOrigenTemp(string agencia, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_AGENCIA_ORIGEN_TEMP", cn);
			cmd.Transaction = trans;
			cmd.CommandType = CommandType.StoredProcedure;
            
            try
            {
                SqlParameter prmAgencia = cmd.Parameters.Add("@codigo", SqlDbType.VarChar);
                prmAgencia.Value = agencia;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GrabarAsignacionMasiva(int cucInicio, int cucFin, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_PARTICIPE_X_PROMOTOR_MASIVO_X_AGENCIA", cn);
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 10000;
            try
            {
                SqlParameter prmCucInicio = cmd.Parameters.Add("@cucInicio", SqlDbType.Decimal);
                prmCucInicio.Value = cucInicio;

                SqlParameter prmCucFin = cmd.Parameters.Add("@cucFin", SqlDbType.Decimal);
                prmCucFin.Value = cucFin;

                SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
                prmUsuario.Value = codigoUsuario;

                int filasAfectadas = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EliminarAgenciasOrigenTemp(SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_AGENCIA_ORIGEN_TEMP", cn);
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LimpiarAsignacionMasiva(int cucInicio, int cucFin, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_PARTICIPE_X_PROMOTOR_MASIVO_X_AGENCIA", cn);
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 10000;
            try
            {
                SqlParameter prmCucInicio = cmd.Parameters.Add("@cucInicio", SqlDbType.Decimal);
                prmCucInicio.Value = cucInicio;

                SqlParameter prmCucFin = cmd.Parameters.Add("@cucFin", SqlDbType.Decimal);
                prmCucFin.Value = cucFin;

                SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
                prmUsuario.Value = codigoUsuario;

                int filasAfectadas = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GrabarAsignacionAsesor(string codigoAsesor, int cucInicio, int cucFin, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_PARTICIPE_X_PROMOTOR_MASIVO", cn);
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 10000;
            try
            {
                SqlParameter prmCodigoAsesor = cmd.Parameters.Add("@codigoPromotor", SqlDbType.VarChar);
                prmCodigoAsesor.Value = codigoAsesor;

                SqlParameter prmCucInicio = cmd.Parameters.Add("@cucInicio", SqlDbType.Decimal);
                prmCucInicio.Value = cucInicio;

                SqlParameter prmCucFin = cmd.Parameters.Add("@cucFin", SqlDbType.Decimal);
                prmCucFin.Value = cucFin;

                SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
                prmUsuario.Value = codigoUsuario;

                int filasAfectadas = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LimpiarAsignacionAsesor(string codigoAsesor, int cucInicio, int cucFin, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_PARTICIPE_X_PROMOTOR_MASIVO", cn);
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 10000;
            try
            {
                SqlParameter prmCodigoAsesor = cmd.Parameters.Add("@codigoPromotor", SqlDbType.VarChar);
                prmCodigoAsesor.Value = codigoAsesor;

                SqlParameter prmCucInicio = cmd.Parameters.Add("@cucInicio", SqlDbType.Decimal);
                prmCucInicio.Value = cucInicio;

                SqlParameter prmCucFin = cmd.Parameters.Add("@cucFin", SqlDbType.Decimal);
                prmCucFin.Value = cucFin;

                SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
                prmUsuario.Value = codigoUsuario;

                int filasAfectadas = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidarRangoCucEnLote(string codigoLote, int cucInicio, int cucFin)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_VALIDAR_RANGO_CUC_X_LOTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoLote = cmd.Parameters.Add("@codigoLote", SqlDbType.VarChar);
                prmCodigoLote.Value = codigoLote;

                SqlParameter prmCucInicio = cmd.Parameters.Add("@cucInicio", SqlDbType.Decimal);
                prmCucInicio.Value = cucInicio;

                SqlParameter prmCucFin = cmd.Parameters.Add("@cucFin", SqlDbType.Decimal);
                prmCucFin.Value = cucFin;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("LOTE");
                da.Fill(dt);
                return !(dt.Rows.Count == 0);
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                cmd.Dispose();
            }
        }
    }
}
