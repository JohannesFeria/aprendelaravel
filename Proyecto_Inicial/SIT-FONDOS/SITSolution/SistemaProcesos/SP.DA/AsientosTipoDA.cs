/*
 * Fecha de Modificación: 03/12/2014
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
    public class AsientosTipoDA : INGFondos.Data.DA
    {
        public AsientosTipoDA(): base(INGFondos.Constants.Conexiones.ServidorSAP, INGFondos.Constants.Conexiones.BaseDeDatosSAP) { }

        public SqlConnection GetConnection2()
        {
            return GetConnection();
        }

        public DataTable ListarParametro(string CodTabla)
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_PARAMETRO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoTabla = cmd.Parameters.Add("@CODIGOTABLA", SqlDbType.VarChar);
                prmCodigoTabla.Value = CodTabla;

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

        public DataTable RegistrarAsientoCabecera(DataTable pdtAsiento, SqlConnection cn, SqlTransaction trans)
        {
            
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_ASIENTO_TIPO_CABECERA", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlParameter prmCodigo = cmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar);
                prmCodigo.Value = Convert.ToString(pdtAsiento.Rows[0]["DESCRIPCION"]);
                SqlParameter prmDescripcion = cmd.Parameters.Add("@CLASEDOCUMENTO", SqlDbType.VarChar);
                prmDescripcion.Value = Convert.ToString(pdtAsiento.Rows[0]["CLASE_DOCUMENTO"]);
                SqlParameter prmCodigoUsuario = cmd.Parameters.Add("@SOCIEDADFI", SqlDbType.VarChar);
                SqlParameter prmMoneda = cmd.Parameters.Add("@MONEDA", SqlDbType.VarChar);
                prmMoneda.Value = Convert.ToString(pdtAsiento.Rows[0]["MONEDA_DOCUMENTO"]);
                prmCodigoUsuario.Value = Convert.ToString(pdtAsiento.Rows[0]["SOCIEDAD_FI"]);
                SqlParameter prmReferencia = cmd.Parameters.Add("@REFERENCIA", SqlDbType.VarChar);
                prmReferencia.Value = Convert.ToString(pdtAsiento.Rows[0]["REFERENCIA"]);
                SqlParameter prmTextoCabecera = cmd.Parameters.Add("@TEXTOCABECERA", SqlDbType.VarChar);
                prmTextoCabecera.Value = Convert.ToString(pdtAsiento.Rows[0]["TEXTO_CABECERA"]);
                SqlParameter prmLibroContable = cmd.Parameters.Add("@LIBROCONTABLE", SqlDbType.VarChar);
                prmLibroContable.Value = Convert.ToString(pdtAsiento.Rows[0]["LIBRO_CONTABLE"]);
                SqlParameter prmFondo = cmd.Parameters.Add("@FONDO", SqlDbType.VarChar);
                prmFondo.Value = Convert.ToString(pdtAsiento.Rows[0]["FONDO"]);
                SqlParameter prmUsuarioCreacion = cmd.Parameters.Add("@USUARIOCREACION", SqlDbType.VarChar);
                prmUsuarioCreacion.Value = Convert.ToString(pdtAsiento.Rows[0]["USUARIO_CREACION"]);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                return dt;

            }
            catch (Exception ex) { throw ex; }
            finally { cmd.Dispose(); }
            
        }

        public DataTable RegistrarAsientoDetalle(DataTable pdtAsiento, Int64 CodCorrelativo, SqlConnection cn, SqlTransaction trans)
        {
            
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_ASIENTO_TIPO_DETALLE", cn, trans);
            SqlCommand cmd2 = new SqlCommand("dbo.FMPR_ACT_ASIENTO_TIPO_DETALLE", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd2.CommandType = CommandType.StoredProcedure;
            try
            {
                foreach (DataRow fila in pdtAsiento.Rows)
                {
                    if (fila["COD_ASIENTO_DET"].ToString() == "0")
                    {
                        SqlParameter prmCODIGOASIENTO = cmd.Parameters.Add("@CODIGOASIENTO", SqlDbType.BigInt);
                        prmCODIGOASIENTO.Value = CodCorrelativo;
                        SqlParameter prmClaveContabilizacion = cmd.Parameters.Add("@ClaveContabilizacion", SqlDbType.VarChar);
                        prmClaveContabilizacion.Value = Convert.ToString(fila["CLAVE_CONTABILIZACION"]);
                        SqlParameter prmCuenta = cmd.Parameters.Add("@Cuenta", SqlDbType.VarChar);
                        prmCuenta.Value = Convert.ToString(fila["CUENTA"]);
                        SqlParameter prmIndicadorCME = cmd.Parameters.Add("@IndicadorCME", SqlDbType.VarChar);
                        prmIndicadorCME.Value = Convert.ToString(fila["INDICADOR_CME"]);
                        SqlParameter prmImporteMndaDoc = cmd.Parameters.Add("@textoPosicion", SqlDbType.VarChar);
                        prmImporteMndaDoc.Value = Convert.ToString(fila["TEXTO_POSICION"]);

                        SqlParameter prmIndicadorIGV = cmd.Parameters.Add("@IndicadorIGV", SqlDbType.VarChar);
                        prmIndicadorIGV.Value = Convert.ToString(fila["INDICADOR_IGV"]);
                        SqlParameter prmAsignacion = cmd.Parameters.Add("@Asignacion", SqlDbType.VarChar);
                        prmAsignacion.Value = Convert.ToString(fila["ASIGNACION"]);

                        SqlParameter prmCondicionPago = cmd.Parameters.Add("@CondicionPago", SqlDbType.VarChar);
                        prmCondicionPago.Value = Convert.ToString(fila["CONDICION_PAGO"]);
                        SqlParameter prmViaPago = cmd.Parameters.Add("@ViaPago", SqlDbType.VarChar);
                        prmViaPago.Value = Convert.ToString(fila["VIA_PAGO"]);

                        SqlParameter prmBloqueoPago = cmd.Parameters.Add("@BloqueoPago", SqlDbType.VarChar);
                        prmBloqueoPago.Value = Convert.ToString(fila["BLOQUEO_PAGO"]);
                        SqlParameter prmCentroCosto = cmd.Parameters.Add("@tipoObjetoImputacion", SqlDbType.VarChar);
                        prmCentroCosto.Value = Convert.ToString(fila["TIPO_OBJETO_IMPUTACION"]);
                        SqlParameter prmCentroBeneficio = cmd.Parameters.Add("@objetoImputacion", SqlDbType.VarChar);
                        prmCentroBeneficio.Value = Convert.ToString(fila["OBJETO_IMPUTACION"]);
                        SqlParameter prmReferencia1 = cmd.Parameters.Add("@Referencia1", SqlDbType.VarChar);
                        prmReferencia1.Value = Convert.ToString(fila["REFERENCIA1"]);
                        SqlParameter prmReferencia3 = cmd.Parameters.Add("@Referencia3", SqlDbType.VarChar);
                        prmReferencia3.Value = Convert.ToString(fila["REFERENCIA3"]);
                        SqlParameter prmUsuarioCreacion = cmd.Parameters.Add("@UsuarioCreacion", SqlDbType.VarChar);
                        prmUsuarioCreacion.Value = Convert.ToString(fila["USUARIO_CREACION"]);


                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        da.Fill(dt);

                        cmd.Parameters.Clear();
                    }
                    else
                    {
                        SqlParameter prmCODIGOASIENTO = cmd2.Parameters.Add("@CODIGOASIENTO", SqlDbType.BigInt);
                        prmCODIGOASIENTO.Value = CodCorrelativo;
                        SqlParameter prmCodigoDetalle = cmd2.Parameters.Add("@CODIGOASIENTODET", SqlDbType.BigInt);
                        prmCodigoDetalle.Value = Convert.ToInt64(fila["COD_ASIENTO_DET"]);
                        SqlParameter prmClaveContabilizacion = cmd2.Parameters.Add("@ClaveContabilizacion", SqlDbType.VarChar);
                        prmClaveContabilizacion.Value = Convert.ToString(fila["CLAVE_CONTABILIZACION"]);
                        SqlParameter prmCuenta = cmd2.Parameters.Add("@Cuenta", SqlDbType.VarChar);
                        prmCuenta.Value = Convert.ToString(fila["CUENTA"]);
                        SqlParameter prmIndicadorCME = cmd2.Parameters.Add("@IndicadorCME", SqlDbType.VarChar);
                        prmIndicadorCME.Value = Convert.ToString(fila["INDICADOR_CME"]);
                        SqlParameter prmImporteMndaDoc = cmd2.Parameters.Add("@textoPosicion", SqlDbType.VarChar);
                        prmImporteMndaDoc.Value = Convert.ToString(fila["TEXTO_POSICION"]);

                        SqlParameter prmIndicadorIGV = cmd2.Parameters.Add("@IndicadorIGV", SqlDbType.VarChar);
                        prmIndicadorIGV.Value = Convert.ToString(fila["INDICADOR_IGV"]);
                        SqlParameter prmAsignacion = cmd2.Parameters.Add("@Asignacion", SqlDbType.VarChar);
                        prmAsignacion.Value = Convert.ToString(fila["ASIGNACION"]);

                        SqlParameter prmCondicionPago = cmd2.Parameters.Add("@CondicionPago", SqlDbType.VarChar);
                        prmCondicionPago.Value = Convert.ToString(fila["CONDICION_PAGO"]);
                        SqlParameter prmViaPago = cmd2.Parameters.Add("@ViaPago", SqlDbType.VarChar);
                        prmViaPago.Value = Convert.ToString(fila["VIA_PAGO"]);

                        SqlParameter prmBloqueoPago = cmd2.Parameters.Add("@BloqueoPago", SqlDbType.VarChar);
                        prmBloqueoPago.Value = Convert.ToString(fila["BLOQUEO_PAGO"]);
                        SqlParameter prmCentroCosto = cmd2.Parameters.Add("@tipoObjetoImputacion", SqlDbType.VarChar);
                        prmCentroCosto.Value = Convert.ToString(fila["TIPO_OBJETO_IMPUTACION"]);
                        SqlParameter prmCentroBeneficio = cmd2.Parameters.Add("@objetoImputacion", SqlDbType.VarChar);
                        prmCentroBeneficio.Value = Convert.ToString(fila["OBJETO_IMPUTACION"]);
                        SqlParameter prmReferencia1 = cmd2.Parameters.Add("@Referencia1", SqlDbType.VarChar);
                        prmReferencia1.Value = Convert.ToString(fila["REFERENCIA1"]);
                        SqlParameter prmReferencia3 = cmd2.Parameters.Add("@Referencia3", SqlDbType.VarChar);
                        prmReferencia3.Value = Convert.ToString(fila["REFERENCIA3"]);
                        SqlParameter prmUsuarioCreacion = cmd2.Parameters.Add("@Usuario", SqlDbType.VarChar);
                        prmUsuarioCreacion.Value = Convert.ToString(fila["USUARIO_CREACION"]);

                        SqlDataAdapter da = new SqlDataAdapter(cmd2);

                        da.Fill(dt);

                        cmd2.Parameters.Clear();
                    }

                }


                return dt;

            }
            catch (Exception ex) { throw ex; }
            finally
            {
                cmd.Dispose();
                cmd2.Dispose();
            }
        }

        public DataTable ActualizarAsientoCabecera(DataTable pdtAsiento, Int64 CodCorrelativo, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_ASIENTO_TIPO_CABECERA", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlParameter prmCODIGOASIENTO = cmd.Parameters.Add("@CODIGOASIENTO", SqlDbType.VarChar);
                prmCODIGOASIENTO.Value = Convert.ToString(pdtAsiento.Rows[0]["CODIGO_ASIENTO"]);
                SqlParameter prmCodigo = cmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar);
                prmCodigo.Value = Convert.ToString(pdtAsiento.Rows[0]["DESCRIPCION"]);
                SqlParameter prmDescripcion = cmd.Parameters.Add("@CLASEDOCUMENTO", SqlDbType.VarChar);
                prmDescripcion.Value = Convert.ToString(pdtAsiento.Rows[0]["CLASE_DOCUMENTO"]);
                SqlParameter prmCodigoUsuario = cmd.Parameters.Add("@SOCIEDADFI", SqlDbType.VarChar);
                prmCodigoUsuario.Value = Convert.ToString(pdtAsiento.Rows[0]["SOCIEDAD_FI"]);
                SqlParameter prmReferencia = cmd.Parameters.Add("@REFERENCIA", SqlDbType.VarChar);
                prmReferencia.Value = Convert.ToString(pdtAsiento.Rows[0]["REFERENCIA"]);
                SqlParameter prmTextoCabecera = cmd.Parameters.Add("@TEXTOCABECERA", SqlDbType.VarChar);
                prmTextoCabecera.Value = Convert.ToString(pdtAsiento.Rows[0]["TEXTO_CABECERA"]);
                SqlParameter prmCalcImpuestos = cmd.Parameters.Add("@MONEDA", SqlDbType.VarChar);
                prmCalcImpuestos.Value = Convert.ToString(pdtAsiento.Rows[0]["MONEDA_DOCUMENTO"]);
                SqlParameter prmLibroContable = cmd.Parameters.Add("@LIBROCONTABLE", SqlDbType.VarChar);
                prmLibroContable.Value = Convert.ToString(pdtAsiento.Rows[0]["LIBRO_CONTABLE"]);
                SqlParameter prmFondo = cmd.Parameters.Add("@FONDO", SqlDbType.VarChar);
                prmFondo.Value = Convert.ToString(pdtAsiento.Rows[0]["FONDO"]);
                SqlParameter prmUsuarioCreacion = cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar);
                prmUsuarioCreacion.Value = Convert.ToString(pdtAsiento.Rows[0]["USUARIO_CREACION"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                return dt;

            }
            catch (Exception ex) { throw ex; }
            finally { cmd.Dispose(); }
        }

        public void EliminarAsientoDetalle(Int64 pCodigo)
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_ASIENTOS_TIPO_DETALLE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoAsiento = cmd.Parameters.Add("@CODIGOASIENTO", SqlDbType.BigInt);
                prmCodigoAsiento.Value = Convert.ToString(pCodigo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
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

        public DataTable EliminarAsiento(int pCodigo, string usuario)
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_ASIENTO_TIPO_CABECERA", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoAsiento = cmd.Parameters.Add("@CODIGOASIENTO", SqlDbType.BigInt);
                prmCodigoAsiento.Value = Convert.ToString(pCodigo);

                SqlParameter prmUsuario = cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar);
                prmUsuario.Value = Convert.ToString(usuario);

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

        public DataTable ObtenerCabeceraAsiento(Int64 pCodigoAsiento)
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_CABECERA_ASIENTO_TIPO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoAsiento = cmd.Parameters.Add("@CODIGOASIENTO", SqlDbType.BigInt);
                prmCodigoAsiento.Value = Convert.ToString(pCodigoAsiento);

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

        public DataTable ObtenerDetalleAsiento(Int64 pCodigoAsiento)
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_DETALLE_ASIENTO_TIPO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoAsiento = cmd.Parameters.Add("@CODIGOASIENTO", SqlDbType.BigInt);
                prmCodigoAsiento.Value = Convert.ToString(pCodigoAsiento);

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

        public DataTable ListarCabeceraAsientos()
        {
            SqlConnection cn = GetConnection();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_CONSULTA_ASIENTOS_TIPO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
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

    }
}
