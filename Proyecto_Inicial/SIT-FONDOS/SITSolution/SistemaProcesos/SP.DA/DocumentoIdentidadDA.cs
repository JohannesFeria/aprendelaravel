/*
 * Fecha de Modificación : 27/04/2017
 * Modificado por        : Anthony Joaquin
 * Nro. Orden de Trabajo : 10217
 * Descripción del cambio: Creación.
 * */
using System;
using System.Data;
using System.Data.SqlClient;
using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
	/// <summary>
    /// Descripción breve de DocumentoIdentidadDA.
	/// </summary>
	public class DocumentoIdentidadDA: INGFondos.Data.DA
	{
        public DocumentoIdentidadDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

	
        public DataTable ListarTipoDocumento(string codigoTabla)
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


        public DataTable ListarNombreParticipe(string tipoDocumento, string numeroDocumento)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_PARTICIPE_X_DOCUMENTO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmTipoDocumento = cmd.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar);
                prmTipoDocumento.Value = tipoDocumento;

                SqlParameter prmNumeroDocumento = cmd.Parameters.Add("@NUMERO_DOCUMENTO", SqlDbType.VarChar);
                prmNumeroDocumento.Value = numeroDocumento;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("NOMBRE_PARTICIPE");
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

				public void ActualizarDocumentoParticipeAdmCuentas(DocumentoIdentidad.DOCUMENTO_IDENTIDADRow drDocumentoIdentidad, SqlConnection conn, SqlTransaction trans)
				{
						SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_DOCUMENTO_PARTICIPE", conn, trans);
						cmd.CommandType = CommandType.StoredProcedure;
						try
						{
								cmd.Parameters.Add("@NUMERO_DOCUMENTO_ANTIGUO", SqlDbType.VarChar).Value = drDocumentoIdentidad.NUMERO_DOCUMENTO_ANTIGUO;
								cmd.Parameters.Add("@NUMERO_DOCUMENTO_NUEVO", SqlDbType.VarChar).Value = drDocumentoIdentidad.NUMERO_DOCUMENTO_NUEVO;
								cmd.Parameters.Add("@TIPO_DOCUMENTO_ANTIGUO", SqlDbType.VarChar).Value = drDocumentoIdentidad.TIPO_DOCUMENTO_ANTIGUO;
								cmd.Parameters.Add("@TIPO_DOCUMENTO_NUEVO", SqlDbType.VarChar).Value = drDocumentoIdentidad.TIPO_DOCUMENTO_NUEVO;
								cmd.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar).Value = drDocumentoIdentidad.USUARIO_MODIFICACION;
								cmd.ExecuteNonQuery();
						}
						catch(Exception e)
						{
								throw e;
						}
						finally
						{
								cmd.Dispose();
						}
				}

				public void ActualizarDocumentoParticipeComercial(DocumentoIdentidad.DOCUMENTO_IDENTIDADRow drDocumentoIdentidad, SqlConnection conn, SqlTransaction trans)
				{
						SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_DOCUMENTO_PARTICIPE", conn, trans);
						cmd.CommandType = CommandType.StoredProcedure;
						try
						{
								cmd.Parameters.Add("@NUMERO_DOCUMENTO_ANTIGUO", SqlDbType.VarChar).Value = drDocumentoIdentidad.NUMERO_DOCUMENTO_ANTIGUO;
								cmd.Parameters.Add("@NUMERO_DOCUMENTO_NUEVO", SqlDbType.VarChar).Value = drDocumentoIdentidad.NUMERO_DOCUMENTO_NUEVO;
								cmd.Parameters.Add("@TIPO_DOCUMENTO_ANTIGUO", SqlDbType.VarChar).Value = drDocumentoIdentidad.TIPO_DOCUMENTO_ANTIGUO;
								cmd.Parameters.Add("@TIPO_DOCUMENTO_NUEVO", SqlDbType.VarChar).Value = drDocumentoIdentidad.TIPO_DOCUMENTO_NUEVO;
								cmd.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar).Value = drDocumentoIdentidad.USUARIO_MODIFICACION;
								cmd.ExecuteNonQuery();
						}
						catch (Exception e)
						{
								throw e;
						}
						finally
						{
								cmd.Dispose();
						}
				}

				public void ActualizarDocumentoParticipeConasev(DocumentoIdentidad.DOCUMENTO_IDENTIDADRow drDocumentoIdentidad, SqlConnection conn, SqlTransaction trans)
				{
						SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_DOCUMENTO_PARTICIPE", conn, trans);
						cmd.CommandType = CommandType.StoredProcedure;
						try
						{
								cmd.Parameters.Add("@NUMERO_DOCUMENTO_ANTIGUO", SqlDbType.VarChar).Value = drDocumentoIdentidad.NUMERO_DOCUMENTO_ANTIGUO;
								cmd.Parameters.Add("@NUMERO_DOCUMENTO_NUEVO", SqlDbType.VarChar).Value = drDocumentoIdentidad.NUMERO_DOCUMENTO_NUEVO;
								cmd.Parameters.Add("@TIPO_DOCUMENTO_ANTIGUO", SqlDbType.VarChar).Value = drDocumentoIdentidad.TIPO_DOCUMENTO_ANTIGUO;
								cmd.Parameters.Add("@TIPO_DOCUMENTO_NUEVO", SqlDbType.VarChar).Value = drDocumentoIdentidad.TIPO_DOCUMENTO_NUEVO;
								cmd.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar).Value = drDocumentoIdentidad.USUARIO_MODIFICACION;
								cmd.ExecuteNonQuery();
						}
						catch (Exception e)
						{
								throw e;
						}
						finally
						{
								cmd.Dispose();
						}
				}

				public void ActualizarDocumentoParticipeDeteccionLavadoActivos(DocumentoIdentidad.DOCUMENTO_IDENTIDADRow drDocumentoIdentidad, SqlConnection conn, SqlTransaction trans)
				{
						SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_DOCUMENTO_PARTICIPE", conn, trans);
						cmd.CommandType = CommandType.StoredProcedure;
						try
						{
								cmd.Parameters.Add("@NUMERO_DOCUMENTO_ANTIGUO", SqlDbType.VarChar).Value = drDocumentoIdentidad.NUMERO_DOCUMENTO_ANTIGUO;
								cmd.Parameters.Add("@NUMERO_DOCUMENTO_NUEVO", SqlDbType.VarChar).Value = drDocumentoIdentidad.NUMERO_DOCUMENTO_NUEVO;
								cmd.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar).Value = drDocumentoIdentidad.USUARIO_MODIFICACION;
								cmd.ExecuteNonQuery();
						}
						catch (Exception e)
						{
								throw e;
						}
						finally
						{
								cmd.Dispose();
						}
				}

				public void ActualizarDocumentoParticipeTributacion(DocumentoIdentidad.DOCUMENTO_IDENTIDADRow drDocumentoIdentidad, SqlConnection conn, SqlTransaction trans)
				{
						SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_DOCUMENTO_PARTICIPE", conn, trans);
						cmd.CommandType = CommandType.StoredProcedure;
						try
						{
								cmd.Parameters.Add("@NUMERO_DOCUMENTO_ANTIGUO", SqlDbType.VarChar).Value = drDocumentoIdentidad.NUMERO_DOCUMENTO_ANTIGUO;
								cmd.Parameters.Add("@NUMERO_DOCUMENTO_NUEVO", SqlDbType.VarChar).Value = drDocumentoIdentidad.NUMERO_DOCUMENTO_NUEVO;
								cmd.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar).Value = drDocumentoIdentidad.USUARIO_MODIFICACION;
								cmd.ExecuteNonQuery();
						}
						catch (Exception e)
						{
								throw e;
						}
						finally
						{
								cmd.Dispose();
						}
				}
        
	}
}
