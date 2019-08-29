/*
 * Fecha de Modificación: 22/05/2015
 * Modificado por: Alex Vega
 * Numero de OT: 7349
 * Descripción del cambio: Creación
 * */
/*
 * Fecha de Modificación: 06/08/2015
 * Modificado por: Robert Castillo
 * Numero de OT: 7349
 * Descripción del cambio: Se agrega el método ObtenerTablaGeneral, que se encarga de llamar 
 *                         al procedure INGF_LIS_TABLA_GENERAL.
 * */
/*
 * Fecha de Modificación: 26/08/2015
 * Modificado por: Juan Castro
 * Numero de OT: 7584
 * Descripción del cambio: Se implementa envío de comprobantes electrónicos.
 * */
/*
 * Fecha de Modificación: 20/11/2015
 * Modificado por: Robert Castillo
 * Numero de OT: 7999
 * Descripción del cambio: Se agrega el método ObtenerTipoCambio.
 * */
/*
 * Fecha de Modificación: 04/12/2015
 * Modificado por: Richard Valdez
 * Numero de OT: 7940-PSC002
 * Descripción del cambio: Se invoca al procedure FMPR_OBT_TIPO_CAMBIO
 *                          en el método ObtenerTipoCambio.
 * */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using INGFondos.Data;
using Procesos.TD;
using System.Configuration;
using Procesos.Constants;


namespace Procesos.DA
{
    public class FacturacionDA : INGFondos.Data.DA
    {
        private SqlConnection conn;
        private SqlTransaction trans;

        public SqlTransaction Trans
        {
            get { return trans; }
            set { trans = value; }
        }

        public SqlConnection Conn
        {
            get { return conn; }
            set { conn = value; }
        }

        public void CrearConexion()
        {
            conn = GetConnection();
        }

        public void AbrirConexion()
        {
            conn.Open();
        }

        public void AbrirTransaccion()
        {
            trans = conn.BeginTransaction();
        }

        public void CommitTransaccion()
        {
            trans.Commit();
        }

        public void RollbackTransaccion()
        {
            trans.Rollback();
        }

        public void LiberarConexion()
        {
            trans.Dispose();
            conn.Close();
            conn.Dispose();
        }

        public FacturacionDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        public DataTable ListaParametros(string strCodigo, string strItem)
        {
            // Conexion a base de datos Prem_Main
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Main"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Main"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_PARAMETRO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

            try
            {
                cmd.Parameters.Add("@COD_TAB", SqlDbType.VarChar).Value = strCodigo;
                cmd.Parameters.Add("@COD_ITEM", SqlDbType.VarChar).Value = strItem;

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

        public DataTable ListarTalonarios(string strTipo, string strOrden)
        {
            // Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_TALONARIO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

            try
            {
                cmd.Parameters.Add("@Tipo_Documento", SqlDbType.VarChar).Value = strTipo;
                cmd.Parameters.Add("@Orden", SqlDbType.VarChar).Value = strOrden.Trim();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("TALONARIO");
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

        public DataTable ListarComprobantes(string strTipoComprobante, string strSerie, string strOrden)
        {
            // Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

            try
            {
                cmd.Parameters.Add("@TIPODOC", SqlDbType.VarChar).Value = strTipoComprobante;
                cmd.Parameters.Add("@SERIE", SqlDbType.VarChar).Value = strSerie;
                cmd.Parameters.Add("@ORDEN", SqlDbType.VarChar).Value = strOrden.Trim();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("COMPROBANTE");
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

        public DataTable ObtenerTalonario(string strCodigo)
        {
            // Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_TALONARIO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigo = cmd.Parameters.Add("@COD_TALONARIO", SqlDbType.VarChar, 10);
                prmCodigo.Value = strCodigo;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("TALONARIO");
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

        public int CorrelativoTalonario()
        {
            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_CORRELATIVO_TALONARIO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("TALONARIO");
                da.Fill(dt);
                DataRow row = dt.Rows[0];
                return Convert.ToInt32(row["CORRELATIVO"]);
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

        public string EliminarTalonario(int intCodigoTalonario)
        {
            string strMensaje;
            int strResultado;

            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_TALONARIO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_TALONARIO", SqlDbType.Int).Value = intCodigoTalonario;
                cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
                cmd.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;

                strResultado = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();

                strMensaje = cmd.Parameters["@MENSAJE"].Value.ToString();

                if (strResultado > 0)
                {
                    return ConstantesING.EXITO;
                }
                else
                {
                    return strMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertarTalonario(string strTipo, string strSerie, int intInicio, int intFin, string strEstado, string strUsuario)
        {
            string strMensaje;
            int strResultado;

            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_TALONARIO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar).Value = strTipo;
                cmd.Parameters.Add("@SERIE", SqlDbType.VarChar).Value = strSerie;
                cmd.Parameters.Add("@NUMERO_INICIO", SqlDbType.Int).Value = intInicio;
                cmd.Parameters.Add("@NUMERO_FIN", SqlDbType.Int).Value = intFin;
                cmd.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = strEstado;
                cmd.Parameters.Add("@USUARIO_CREACION", SqlDbType.VarChar).Value = strUsuario;
                cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
                cmd.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;

                strResultado = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();

                strMensaje = cmd.Parameters["@MENSAJE"].Value.ToString();

                if (strResultado > 0)
                {
                    return ConstantesING.EXITO;
                }
                else
                {
                    return strMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ActualizarTalonario(int intCodigo, int intFin, string strEstado, string strUsuario)
        {
            string strMensaje;
            int strResultado;

            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_TALONARIO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_TALONARIO", SqlDbType.Int).Value = intCodigo;
                cmd.Parameters.Add("@NUMERO_FIN", SqlDbType.Int).Value = intFin;
                cmd.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = strEstado;
                cmd.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar).Value = strUsuario;
                cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
                cmd.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;

                strResultado = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();

                strMensaje = cmd.Parameters["@MENSAJE"].Value.ToString();

                if (strResultado > 0)
                {
                    return ConstantesING.EXITO;
                }
                else
                {
                    return strMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ObtenerComprobante(string strCodigo)
        {
            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigo = cmd.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int);
                prmCodigo.Value = Convert.ToInt32(strCodigo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("COMPROBANTE");
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

        public DataTable ObtenerComprobantePorUUID(string strCodigo)
        {
            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_COMPROBANTE_X_UUID", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigo = cmd.Parameters.Add("@UUID", SqlDbType.VarChar);
                prmCodigo.Value = strCodigo;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("COMPROBANTE");
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

        public DataTable ListarComprobanteDetalle()
        {
            // Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_COMPROBANTE_DET", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

            try
            {
                //cmd.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = Convert.ToInt32(strCodigo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("COMPROBANTE");
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

        public DataTable ObtenerCliente(string strRuc)
        {
            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_CLIENTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigo = cmd.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar);
                prmCodigo.Value = strRuc;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("COMPROBANTE");
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

        public string EliminarComprobante(string strCodigo, string strEstado, string strCodigoUsuario)
        {
            int intResultado;
            string strMensaje;

            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = Convert.ToInt32(strCodigo);
                cmd.Parameters.Add("@ESTADO ", SqlDbType.VarChar).Value = strEstado;
                cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = strCodigoUsuario;
                cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
                cmd.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;

                intResultado = cmd.ExecuteNonQuery();

                strMensaje = cmd.Parameters["@MENSAJE"].Value.ToString();

                if (intResultado > 0)
                {
                    return ConstantesING.EXITO;
                }
                else
                {
                    return strMensaje;
                }
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

        public string InsertarCliente(string strRuc, string strRazonSocial, string strDireccion,
                string strDistrito, string strProvincia, string strDepartamento, string codigoUsuario)
        {
            int intResultado;
            string strMensaje;

            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_CLIENTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar).Value = strRuc;
                cmd.Parameters.Add("@RAZON_SOCIAL", SqlDbType.VarChar).Value = strRazonSocial;
                cmd.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = strDireccion;
                cmd.Parameters.Add("@DISTRITO", SqlDbType.VarChar).Value = strDistrito;
                cmd.Parameters.Add("@PROVINCIA", SqlDbType.VarChar).Value = strProvincia;
                cmd.Parameters.Add("@DEPARTAMENTO", SqlDbType.VarChar).Value = strDepartamento;
                cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = codigoUsuario;
                cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
                cmd.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;

                intResultado = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();
                strMensaje = cmd.Parameters["@MENSAJE"].Value.ToString();

                if (intResultado > 0)
                {
                    return ConstantesING.EXITO;
                }
                else
                {
                    return strMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertarComprobante(string tipoComprobante, string moneda, DateTime fecha,
        string ruc, string proceso, string expediente, string concepto, string tipoDocRel, string numDocRel,
        DateTime fechaDocRel, string estado, string impreso, string enviado, string afecto, string glosa,
        int cantidad, double precioUnitario, double subTotal, double igv, double total, string codigoUsuario,
        string uuid)
        {
            int intResultado;
            string strMensaje;

            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar).Value = tipoComprobante;
                cmd.Parameters.Add("@MONEDA", SqlDbType.VarChar).Value = moneda;
                cmd.Parameters.Add("@FECHA_EMISION", SqlDbType.DateTime).Value = fecha;
                cmd.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar).Value = ruc;
                cmd.Parameters.Add("@PROCESO", SqlDbType.VarChar).Value = proceso;
                cmd.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = expediente;
                cmd.Parameters.Add("@COD_CONCEPTO", SqlDbType.VarChar).Value = concepto;
                cmd.Parameters.Add("@TIPO_DOCREL", SqlDbType.VarChar).Value = tipoDocRel;
                cmd.Parameters.Add("@NUMERO_DOCREL", SqlDbType.VarChar).Value = numDocRel;
                cmd.Parameters.Add("@FECHA_DOCREL", SqlDbType.DateTime).Value = fechaDocRel;
                cmd.Parameters.Add("@ESTADO", SqlDbType.Char).Value = estado;
                cmd.Parameters.Add("@FLAG_IMPRESION", SqlDbType.Char).Value = impreso;
                cmd.Parameters.Add("@FLAG_ENVIADO", SqlDbType.Char).Value = enviado;
                cmd.Parameters.Add("@FLAG_AFECTOIGV", SqlDbType.Char).Value = afecto;
                cmd.Parameters.Add("@GLOSA", SqlDbType.VarChar).Value = glosa;
                cmd.Parameters.Add("@CANTIDAD", SqlDbType.Int).Value = cantidad;
                cmd.Parameters.Add("@PRECIO_UNITARIO", SqlDbType.Decimal).Value = precioUnitario;
                cmd.Parameters.Add("@VALOR_VENTA", SqlDbType.Decimal).Value = subTotal;
                cmd.Parameters.Add("@IGV", SqlDbType.Decimal).Value = igv;
                cmd.Parameters.Add("@TOTAL", SqlDbType.Decimal).Value = total;
                cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = codigoUsuario;
                cmd.Parameters.Add("@CODIGO_UUID", SqlDbType.VarChar).Value = uuid;
                cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
                cmd.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ID_COMPROBANTE_OUT", SqlDbType.Int);
                cmd.Parameters["@ID_COMPROBANTE_OUT"].Direction = ParameterDirection.Output;

                intResultado = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();
                strMensaje = cmd.Parameters["@MENSAJE"].Value.ToString();

                if (intResultado > 0)
                {
                    return cmd.Parameters["@ID_COMPROBANTE_OUT"].Value.ToString();
                }
                else
                {
                    return strMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ActualizarComprobante(string accion, int codigo, string tipoComprobante, string moneda, DateTime fecha,
        string ruc, string proceso, string expediente, string concepto, string tipoDocRel, string numDocRel,
        DateTime fechaDocRel, string estado, string impreso, string enviado, string afecto, string glosa,
        int cantidad, double precioUnitario, double subTotal, double igv, double total, string codigoUsuario,
        string uuid)
        {
            int intResultado;
            string strMensaje;

            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar).Value = accion;
                cmd.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = codigo;
                cmd.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar).Value = tipoComprobante;
                cmd.Parameters.Add("@MONEDA", SqlDbType.VarChar).Value = moneda;
                cmd.Parameters.Add("@FECHA_EMISION", SqlDbType.DateTime).Value = fecha;
                cmd.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar).Value = ruc;
                cmd.Parameters.Add("@PROCESO", SqlDbType.VarChar).Value = proceso;
                cmd.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = expediente;
                cmd.Parameters.Add("@COD_CONCEPTO", SqlDbType.VarChar).Value = concepto;
                cmd.Parameters.Add("@TIPO_DOCREL", SqlDbType.VarChar).Value = tipoDocRel;
                cmd.Parameters.Add("@NUMERO_DOCREL", SqlDbType.VarChar).Value = numDocRel;
                cmd.Parameters.Add("@FECHA_DOCREL", SqlDbType.DateTime).Value = fechaDocRel;
                cmd.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = estado;
                cmd.Parameters.Add("@FLAG_IMPRESION", SqlDbType.VarChar).Value = impreso;
                cmd.Parameters.Add("@FLAG_ENVIADO", SqlDbType.VarChar).Value = enviado;
                cmd.Parameters.Add("@FLAG_AFECTOIGV", SqlDbType.VarChar).Value = afecto;
                cmd.Parameters.Add("@GLOSA", SqlDbType.VarChar).Value = glosa;
                cmd.Parameters.Add("@CANTIDAD", SqlDbType.Int).Value = cantidad;
                cmd.Parameters.Add("@PRECIO_UNITARIO", SqlDbType.Decimal).Value = precioUnitario;
                cmd.Parameters.Add("@VALOR_VENTA", SqlDbType.Decimal).Value = subTotal;
                cmd.Parameters.Add("@IGV", SqlDbType.Decimal).Value = igv;
                cmd.Parameters.Add("@TOTAL", SqlDbType.Decimal).Value = total;
                cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = codigoUsuario;
                cmd.Parameters.Add("@CODIGO_UUID", SqlDbType.VarChar).Value = uuid;
                cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
                cmd.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;

                intResultado = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();
                strMensaje = cmd.Parameters["@MENSAJE"].Value.ToString();

                if (intResultado > 0)
                {
                    return ConstantesING.EXITO;
                }
                else
                {
                    return strMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string ActualizarCorrelativoComprobante(string idComprobante, string numero)
        {
            int intResultado;
            string strMensaje;

            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_CORRELATIVO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = Convert.ToInt32(idComprobante);
                cmd.Parameters.Add("@NUMERO_NUEVO", SqlDbType.Int).Value = Convert.ToInt32(numero);
                cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
                cmd.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;

                intResultado = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();
                strMensaje = cmd.Parameters["@MENSAJE"].Value.ToString();

                if (intResultado > 0)
                {
                    return ConstantesING.EXITO;
                }
                else
                {
                    return strMensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void ActualizarDetalle(int id_comprobante, int orden_detalle, string descripcion,
            string cuenta, string afectoImpuesto, double importe, string user)
        {
            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_COMPROBANTE_DETALLE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = Convert.ToInt32(id_comprobante);
                cmd.Parameters.Add("@ORDEN_DETALLE", SqlDbType.Int).Value = Convert.ToInt32(orden_detalle);
                cmd.Parameters.Add("@CUENTA", SqlDbType.VarChar).Value = cuenta;
                cmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = descripcion;
                cmd.Parameters.Add("@AFECTO_IMPUESTO", SqlDbType.VarChar).Value = afectoImpuesto;
                cmd.Parameters.Add("@IMPORTE", SqlDbType.Decimal).Value = importe;
                cmd.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar).Value = user;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IngresarDetalle(int id_comprobante, string descripcion,
            string cuenta, string afectoImpuesto, double importe, string user)
        {
            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_COMPROBANTE_DETALLE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = id_comprobante;
                cmd.Parameters.Add("@CUENTA", SqlDbType.VarChar).Value = cuenta;
                cmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = descripcion;
                cmd.Parameters.Add("@AFECTO_IMPUESTO", SqlDbType.VarChar).Value = afectoImpuesto;
                cmd.Parameters.Add("@IMPORTE", SqlDbType.Decimal).Value = importe;
                cmd.Parameters.Add("@USUARIO_CREACION", SqlDbType.VarChar).Value = user;

                int resultado = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ListarComprobanteImpresion(string strTipoComprobante, string strSerie, int intInicio, int intFinal)
        {
            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_COMPROBANTE_IMPRESION", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

            try
            {
                cmd.Parameters.Add("@Tipo_Documento", SqlDbType.VarChar).Value = strTipoComprobante;
                cmd.Parameters.Add("@Serie", SqlDbType.VarChar).Value = strSerie;
                cmd.Parameters.Add("@NumeroInicio", SqlDbType.VarChar).Value = String.Format(intInicio.ToString().Trim(), "000000");
                cmd.Parameters.Add("@NumeroFinal", SqlDbType.VarChar).Value = String.Format(intFinal.ToString().Trim(), "000000");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
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

        public string ValidarRango(string strTipoComprobante, string strSerie, string strInicio, string strFinal)
        {
            string strMensaje;

            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_VAL_COMPROBANTE_IMPRESION", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@Tipo_Documento", SqlDbType.VarChar).Value = strTipoComprobante;
                cmd.Parameters.Add("@Serie", SqlDbType.VarChar).Value = strSerie;
                cmd.Parameters.Add("@NumeroInicio", SqlDbType.VarChar).Value = strInicio;
                cmd.Parameters.Add("@NumeroFinal", SqlDbType.VarChar).Value = strFinal;
                cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 200);
                cmd.Parameters["@Mensaje"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();
                strMensaje = cmd.Parameters["@Mensaje"].Value.ToString();

                return strMensaje;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ObtenerComprobanteDetalle(string strCodigo)
        {
            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_COMPROBANTE_DET", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigo = cmd.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int);
                prmCodigo.Value = Convert.ToInt32(strCodigo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("COMPROBANTE_DET");
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

        public int EliminarComprobanteDetalle(int id_comprobante, int cod_Detalle)
        {
            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_COMPROBANTE_DET", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = id_comprobante;
                cmd.Parameters.Add("@ORDEN_DETALLE", SqlDbType.Int).Value = cod_Detalle;

                int resultado = cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertarComprobanteDetalle(string strRuc, string strRazonSocial, string strDireccion,
        string strDistrito, string strProvincia, string strDepartamento, string tipoComprobante, string moneda,
        DateTime fecha, string ruc, string proceso, string expediente, string concepto, string tipoDocRel,
        string numDocRel, DateTime fechaDocRel, string estado, string impreso, string enviado, string afecto,
        string glosa, int cantidad, double precioUnitario, double subTotal, double igv, double total,
        string codigoUsuario, string uuid, List<DetalleFactura> listDetalle)
        {
            int intResultado, intResultadoComprobante, identity;
            string strMensaje, strMensajeComprobante;
            int intResultadoDet = 0;

            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();
            SqlTransaction tr = null;

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();


            try
            {
                tr = cn.BeginTransaction();

                // Inserta cliente
                SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_CLIENTE", cn, tr);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar).Value = strRuc;
                cmd.Parameters.Add("@RAZON_SOCIAL", SqlDbType.VarChar).Value = strRazonSocial;
                cmd.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = strDireccion;
                cmd.Parameters.Add("@DISTRITO", SqlDbType.VarChar).Value = strDistrito;
                cmd.Parameters.Add("@PROVINCIA", SqlDbType.VarChar).Value = strProvincia;
                cmd.Parameters.Add("@DEPARTAMENTO", SqlDbType.VarChar).Value = strDepartamento;
                cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = codigoUsuario;
                cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
                cmd.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;

                intResultado = cmd.ExecuteNonQuery();

                strMensaje = cmd.Parameters["@MENSAJE"].Value.ToString();

                // Inserta cabecera comprobante
                SqlCommand cmdCom = new SqlCommand("dbo.FMPR_INS_COMPROBANTE", cn, tr);
                cmdCom.CommandType = CommandType.StoredProcedure;

                cmdCom.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar).Value = tipoComprobante;
                cmdCom.Parameters.Add("@MONEDA", SqlDbType.VarChar).Value = moneda;
                cmdCom.Parameters.Add("@FECHA_EMISION", SqlDbType.DateTime).Value = fecha;
                cmdCom.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar).Value = ruc;
                cmdCom.Parameters.Add("@PROCESO", SqlDbType.VarChar).Value = proceso;
                cmdCom.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = expediente;
                cmdCom.Parameters.Add("@COD_CONCEPTO", SqlDbType.VarChar).Value = concepto;
                cmdCom.Parameters.Add("@TIPO_DOCREL", SqlDbType.VarChar).Value = tipoDocRel;
                cmdCom.Parameters.Add("@NUMERO_DOCREL", SqlDbType.VarChar).Value = numDocRel;
                cmdCom.Parameters.Add("@FECHA_DOCREL", SqlDbType.DateTime).Value = fechaDocRel;
                cmdCom.Parameters.Add("@ESTADO", SqlDbType.Char).Value = estado;
                cmdCom.Parameters.Add("@FLAG_IMPRESION", SqlDbType.Char).Value = impreso;
                cmdCom.Parameters.Add("@FLAG_ENVIADO", SqlDbType.Char).Value = enviado;
                cmdCom.Parameters.Add("@FLAG_AFECTOIGV", SqlDbType.Char).Value = afecto;
                cmdCom.Parameters.Add("@GLOSA", SqlDbType.VarChar).Value = glosa;
                cmdCom.Parameters.Add("@CANTIDAD", SqlDbType.Int).Value = cantidad;
                cmdCom.Parameters.Add("@PRECIO_UNITARIO", SqlDbType.Decimal).Value = precioUnitario;
                cmdCom.Parameters.Add("@VALOR_VENTA", SqlDbType.Decimal).Value = subTotal;
                cmdCom.Parameters.Add("@IGV", SqlDbType.Decimal).Value = igv;
                cmdCom.Parameters.Add("@TOTAL", SqlDbType.Decimal).Value = total;
                cmdCom.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = codigoUsuario;
                cmdCom.Parameters.Add("@CODIGO_UUID", SqlDbType.VarChar).Value = uuid;
                cmdCom.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
                cmdCom.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;
                cmdCom.Parameters.Add("@ID_COMPROBANTE_OUT", SqlDbType.Int);
                cmdCom.Parameters["@ID_COMPROBANTE_OUT"].Direction = ParameterDirection.Output;

                intResultadoComprobante = cmdCom.ExecuteNonQuery();

                identity = Convert.ToInt32(cmdCom.Parameters["@ID_COMPROBANTE_OUT"].Value);

                strMensajeComprobante = cmdCom.Parameters["@MENSAJE"].Value.ToString();

                // Inserta detalle comprobante
                foreach (DetalleFactura item in listDetalle)
                {
                    SqlCommand cmdDet = new SqlCommand("dbo.FMPR_INS_COMPROBANTE_DETALLE", cn, tr);
                    cmdDet.CommandType = CommandType.StoredProcedure;

                    cmdDet.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = identity;
                    cmdDet.Parameters.Add("@CUENTA", SqlDbType.VarChar).Value = item.cuenta;
                    cmdDet.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = item.descripcion;
                    cmdDet.Parameters.Add("@AFECTO_IMPUESTO", SqlDbType.VarChar).Value = item.afectoImpuesto;
                    cmdDet.Parameters.Add("@IMPORTE", SqlDbType.Decimal).Value = item.importe;
                    cmdDet.Parameters.Add("@USUARIO_CREACION", SqlDbType.VarChar).Value = codigoUsuario;

                    intResultadoDet = cmdDet.ExecuteNonQuery();
                }
                
                if (intResultadoComprobante > 0 && intResultadoDet > 0)
                {
                    tr.Commit();
                    return identity.ToString();
                }
                else
                {
                    tr.Rollback();
                    return strMensaje;
                }

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

        public string ActualizarComprobanteDetalle(string strRuc, string strRazonSocial, string strDireccion,
        string strDistrito, string strProvincia, string strDepartamento, string accion, int codigo, string tipoComprobante, string moneda,
        DateTime fecha, string ruc, string proceso, string expediente, string concepto, string tipoDocRel,
        string numDocRel, DateTime fechaDocRel, string estado, string impreso, string enviado, string afecto,
        string glosa, int cantidad, double precioUnitario, double subTotal, double igv, double total,
        string codigoUsuario, string uuid, List<DetalleFactura> listDetalle)
        {
            int intResultado, intResultadoComprobante;
            string strMensaje, strMensajeComprobante;
            int intResultadoDet = 0;
            int intResultadoDet2 = 0;

            //Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();
            SqlTransaction tr = null;

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();


            try
            {
                tr = cn.BeginTransaction();

                // Inserta cliente
                SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_CLIENTE", cn, tr);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar).Value = strRuc;
                cmd.Parameters.Add("@RAZON_SOCIAL", SqlDbType.VarChar).Value = strRazonSocial;
                cmd.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = strDireccion;
                cmd.Parameters.Add("@DISTRITO", SqlDbType.VarChar).Value = strDistrito;
                cmd.Parameters.Add("@PROVINCIA", SqlDbType.VarChar).Value = strProvincia;
                cmd.Parameters.Add("@DEPARTAMENTO", SqlDbType.VarChar).Value = strDepartamento;
                cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = codigoUsuario;
                cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
                cmd.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;

                intResultado = cmd.ExecuteNonQuery();

                strMensaje = cmd.Parameters["@MENSAJE"].Value.ToString();

                // Inserta cabecera comprobante
                SqlCommand cmdCom = new SqlCommand("dbo.FMPR_ACT_COMPROBANTE", cn, tr);
                cmdCom.CommandType = CommandType.StoredProcedure;

                cmdCom.Parameters.Add("@ACCION", SqlDbType.VarChar).Value = accion;
                cmdCom.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = codigo;
                cmdCom.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar).Value = tipoComprobante;
                cmdCom.Parameters.Add("@MONEDA", SqlDbType.VarChar).Value = moneda;
                cmdCom.Parameters.Add("@FECHA_EMISION", SqlDbType.DateTime).Value = fecha;
                cmdCom.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar).Value = ruc;
                cmdCom.Parameters.Add("@PROCESO", SqlDbType.VarChar).Value = proceso;
                cmdCom.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = expediente;
                cmdCom.Parameters.Add("@COD_CONCEPTO", SqlDbType.VarChar).Value = concepto;
                cmdCom.Parameters.Add("@TIPO_DOCREL", SqlDbType.VarChar).Value = tipoDocRel;
                cmdCom.Parameters.Add("@NUMERO_DOCREL", SqlDbType.VarChar).Value = numDocRel;
                cmdCom.Parameters.Add("@FECHA_DOCREL", SqlDbType.DateTime).Value = fechaDocRel;
                cmdCom.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = estado;
                cmdCom.Parameters.Add("@FLAG_IMPRESION", SqlDbType.VarChar).Value = impreso;
                cmdCom.Parameters.Add("@FLAG_ENVIADO", SqlDbType.VarChar).Value = enviado;
                cmdCom.Parameters.Add("@FLAG_AFECTOIGV", SqlDbType.VarChar).Value = afecto;
                cmdCom.Parameters.Add("@GLOSA", SqlDbType.VarChar).Value = glosa;
                cmdCom.Parameters.Add("@CANTIDAD", SqlDbType.Int).Value = cantidad;
                cmdCom.Parameters.Add("@PRECIO_UNITARIO", SqlDbType.Decimal).Value = precioUnitario;
                cmdCom.Parameters.Add("@VALOR_VENTA", SqlDbType.Decimal).Value = subTotal;
                cmdCom.Parameters.Add("@IGV", SqlDbType.Decimal).Value = igv;
                cmdCom.Parameters.Add("@TOTAL", SqlDbType.Decimal).Value = total;
                cmdCom.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = codigoUsuario;
                cmdCom.Parameters.Add("@CODIGO_UUID", SqlDbType.VarChar).Value = uuid;
                cmdCom.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
                cmdCom.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;                

                intResultadoComprobante = cmdCom.ExecuteNonQuery();

                strMensajeComprobante = cmdCom.Parameters["@MENSAJE"].Value.ToString();

                // Actualiza detalle comprobante         
               
                foreach (DetalleFactura item in listDetalle)
                {
                    if (item.orden_detalle != 0)
                    {
                        SqlCommand cmdDet = new SqlCommand("dbo.FMPR_ACT_COMPROBANTE_DETALLE", cn, tr);
                        cmdDet.CommandType = CommandType.StoredProcedure;        

                        cmdDet.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = item.id_comprobante;
                        cmdDet.Parameters.Add("@ORDEN_DETALLE", SqlDbType.Int).Value = item.orden_detalle;
                        cmdDet.Parameters.Add("@CUENTA", SqlDbType.VarChar).Value = item.cuenta;
                        cmdDet.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = item.descripcion;
                        cmdDet.Parameters.Add("@AFECTO_IMPUESTO", SqlDbType.VarChar).Value = item.afectoImpuesto;
                        cmdDet.Parameters.Add("@IMPORTE", SqlDbType.Decimal).Value = item.importe;
                        cmdDet.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar).Value = codigoUsuario;
                       
                        intResultadoDet = cmdDet.ExecuteNonQuery();
                    }
                    
                }
                               

                foreach (DetalleFactura item in listDetalle)
                {
                    if (item.orden_detalle == 0)
                    {
                        SqlCommand cmdDet2 = new SqlCommand("dbo.FMPR_INS_COMPROBANTE_DETALLE", cn, tr);
                        cmdDet2.CommandType = CommandType.StoredProcedure;

                        cmdDet2.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = item.id_comprobante;
                        cmdDet2.Parameters.Add("@CUENTA", SqlDbType.VarChar).Value = item.cuenta;
                        cmdDet2.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = item.descripcion;
                        cmdDet2.Parameters.Add("@AFECTO_IMPUESTO", SqlDbType.VarChar).Value = item.afectoImpuesto;
                        cmdDet2.Parameters.Add("@IMPORTE", SqlDbType.Decimal).Value = item.importe;
                        cmdDet2.Parameters.Add("@USUARIO_CREACION", SqlDbType.VarChar).Value = codigoUsuario;

                        intResultadoDet2 = cmdDet2.ExecuteNonQuery();                    
                    }                                             

                }

                if (intResultadoComprobante > 0)
                {
                    if (intResultadoDet > 0 || intResultadoDet2 > 0)
                    {
                        tr.Commit();
                        return ConstantesING.EXITO;
                    }
                    else
                    {
                        tr.Rollback();
                        return strMensaje;
                    }
                }
                else
                {
                    tr.Rollback();
                    return strMensaje;
                }
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


        public void ActualizarComprobanteIdentificador(int idComprobante, string uuid)
        {
            ////Conexion a base de datos Prem_Banco13
            //string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            //string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            //SqlConnection cn = new SqlConnection();
            //SqlTransaction tr = null;

            //string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            //cn.ConnectionString = strCn;
            //cn.Open();
            //try
            //{
            //    tr = cn.BeginTransaction();

            SqlCommand cmdCom = new SqlCommand("dbo.FMPR_ACT_COMPROBANTE_UUID", conn, trans);
            cmdCom.CommandType = CommandType.StoredProcedure;

            cmdCom.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = idComprobante;
            cmdCom.Parameters.Add("@UUID", SqlDbType.VarChar).Value = uuid;

            cmdCom.ExecuteNonQuery();
            //    tr.Commit();
            //}
            //catch (Exception ex)
            //{
            //    tr.Rollback();
            //    throw ex;
            //}
            //finally
            //{
            //    cn.Close();
            //}
        }

        //OT7349 INI
        public DataTable ObtenerTablaGeneral(string codigoTabla)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_TABLA_GENERAL", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

            SqlParameter prmCod = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
            prmCod.Value = codigoTabla;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("TablaGeneral");
            da.Fill(dt);
            return dt;
        }
        //OT7349 FIN



        public DataTable ObtenerFacturaParaEnvio(string serie, string numero)
        {
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_FACTURA_ENVIO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

            try
            {
                cmd.Parameters.Add("@serie", SqlDbType.VarChar).Value = serie;
                cmd.Parameters.Add("@numero", SqlDbType.VarChar).Value = numero;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
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

        public DataTable ObtenerBoletaParaEnvio(string serie, string numero)
        {
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_BOLETA_ENVIO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

            try
            {
                cmd.Parameters.Add("@serie", SqlDbType.VarChar).Value = serie;
                cmd.Parameters.Add("@numero", SqlDbType.VarChar).Value = numero;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
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

        public DataTable ObtenerNCParaEnvio(string serie, string numero)
        {
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_NOTA_CREDITO_ENVIO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

            try
            {
                cmd.Parameters.Add("@serie", SqlDbType.VarChar).Value = serie;
                cmd.Parameters.Add("@numero", SqlDbType.VarChar).Value = numero;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
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

        public DataTable ListarPaises()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_PAIS", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable ListarDepartamentos(int idPais)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_DEPARTAMENTO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@idPais", SqlDbType.Decimal).Value = idPais;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable ListarCiudades(int idDepartamento)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_CIUDAD", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@idDepartamento", SqlDbType.Decimal).Value = idDepartamento;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable ListarDistritos(int idCiudad)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_DISTRITO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@idCiudad", SqlDbType.Decimal).Value = idCiudad;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public int RegistrarCliente(Cliente cliente)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_CLIENTE", conn, trans);
            
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar).Value = cliente.CodigoCliente;
            cmd.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar).Value = cliente.TipoDocumento;
            cmd.Parameters.Add("@RAZON_SOCIAL", SqlDbType.VarChar).Value = cliente.RazonSocial;
            cmd.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = cliente.Direccion;
            cmd.Parameters.Add("@ID_PAIS", SqlDbType.Decimal).Value = cliente.IdPais;
            cmd.Parameters.Add("@PAIS", SqlDbType.VarChar).Value = cliente.Pais;
            if (cliente.IdPais.ToString().Equals(ConstantesING.CODIGO_PAIS_PERU))
            {
                cmd.Parameters.Add("@ID_DEPARTAMENTO", SqlDbType.Decimal).Value = cliente.IdDepartamento;
                cmd.Parameters.Add("@DEPARTAMENTO", SqlDbType.VarChar).Value = cliente.Departamento;
                cmd.Parameters.Add("@ID_CIUDAD", SqlDbType.Decimal).Value = cliente.IdCiudad;
                cmd.Parameters.Add("@PROVINCIA", SqlDbType.VarChar).Value = cliente.Ciudad;
                cmd.Parameters.Add("@ID_DISTRITO", SqlDbType.Decimal).Value = cliente.IdDistrito;
                cmd.Parameters.Add("@DISTRITO", SqlDbType.VarChar).Value = cliente.Distrito;
                cmd.Parameters.Add("@UBIGEO", SqlDbType.VarChar).Value = cliente.Ubigeo;
                cmd.Parameters.Add("@CODIGO_POSTAL", SqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@ID_DEPARTAMENTO", SqlDbType.Decimal).Value = DBNull.Value;
                cmd.Parameters.Add("@DEPARTAMENTO", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@ID_CIUDAD", SqlDbType.Decimal).Value = DBNull.Value;
                cmd.Parameters.Add("@PROVINCIA", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@ID_DISTRITO", SqlDbType.Decimal).Value = DBNull.Value;
                cmd.Parameters.Add("@DISTRITO", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@UBIGEO", SqlDbType.VarChar).Value = DBNull.Value;
                cmd.Parameters.Add("@CODIGO_POSTAL", SqlDbType.VarChar).Value = cliente.CodigoPostal;
            }
            
            cmd.Parameters.Add("@CORREO", SqlDbType.VarChar).Value = cliente.Correo;
            cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = cliente.Usuario;

            return cmd.ExecuteNonQuery();

        }

        public int RegistrarComprobante(Comprobante comprobante)
        {
            SqlCommand cmdCom = new SqlCommand("dbo.FMPR_INS_COMPROBANTE", conn, trans);
            cmdCom.CommandType = CommandType.StoredProcedure;

            cmdCom.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar).Value = comprobante.TipoDocumento;
            cmdCom.Parameters.Add("@MONEDA", SqlDbType.VarChar).Value = comprobante.Moneda;
            cmdCom.Parameters.Add("@FECHA_EMISION", SqlDbType.DateTime).Value = comprobante.FechaEmision;
            cmdCom.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar).Value = comprobante.Cliente.CodigoCliente;
            cmdCom.Parameters.Add("@PROCESO", SqlDbType.VarChar).Value = comprobante.Proceso;
            cmdCom.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = comprobante.Expediente;
            cmdCom.Parameters.Add("@COD_CONCEPTO", SqlDbType.VarChar).Value = comprobante.CodigoConcepto;
            cmdCom.Parameters.Add("@TIPO_DOCREL", SqlDbType.VarChar).Value = comprobante.TipoDocumentoRelacionado;
            cmdCom.Parameters.Add("@NUMERO_DOCREL", SqlDbType.VarChar).Value = comprobante.NumeroDocumentoRelacionado;
            cmdCom.Parameters.Add("@FECHA_DOCREL", SqlDbType.DateTime).Value = comprobante.FechaDocumentoRelacionado;
            cmdCom.Parameters.Add("@ESTADO", SqlDbType.Char).Value = comprobante.Estado;
            cmdCom.Parameters.Add("@FLAG_IMPRESION", SqlDbType.Char).Value = comprobante.FlagImpresion;
            cmdCom.Parameters.Add("@FLAG_ENVIADO", SqlDbType.Char).Value = comprobante.FlagEnviado;
            cmdCom.Parameters.Add("@FLAG_AFECTOIGV", SqlDbType.Char).Value = comprobante.FlagAfectoIgv;
            cmdCom.Parameters.Add("@GLOSA", SqlDbType.VarChar).Value = comprobante.Glosa;
            cmdCom.Parameters.Add("@CANTIDAD", SqlDbType.Int).Value = comprobante.Cantidad;
            cmdCom.Parameters.Add("@PRECIO_UNITARIO", SqlDbType.Decimal).Value = comprobante.PrecioUnitario;
            cmdCom.Parameters.Add("@VALOR_VENTA", SqlDbType.Decimal).Value = comprobante.ValorVenta;
            cmdCom.Parameters.Add("@IGV", SqlDbType.Decimal).Value = comprobante.Igv;
            cmdCom.Parameters.Add("@TOTAL", SqlDbType.Decimal).Value = comprobante.Total;
            cmdCom.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = comprobante.Usuario;
            //cmdCom.Parameters.Add("@CODIGO_UUID", SqlDbType.VarChar).Value = comprobante.Uuid;
            cmdCom.Parameters.Add("@TIPO_NC", SqlDbType.VarChar).Value = comprobante.TipoNotaCredito;

            cmdCom.Parameters.Add("@PORCENTAJE_DETRACCION", SqlDbType.Decimal).Value = comprobante.PorcentajeDetraccion; //OT7999
            cmdCom.Parameters.Add("@MONTO_DETRACCION", SqlDbType.Decimal).Value = comprobante.MontoDetraccion; //OT7999

            cmdCom.Parameters.Add("@ID_COMPROBANTE_OUT", SqlDbType.Int);
            cmdCom.Parameters["@ID_COMPROBANTE_OUT"].Direction = ParameterDirection.Output;

            cmdCom.Parameters.Add("@SERIE", SqlDbType.VarChar, 3);
            cmdCom.Parameters["@SERIE"].Direction = ParameterDirection.Output;

            cmdCom.Parameters.Add("@NUMERO", SqlDbType.VarChar, 6);
            cmdCom.Parameters["@NUMERO"].Direction = ParameterDirection.Output;

            int intResultadoComprobante = cmdCom.ExecuteNonQuery();

            comprobante.IdComprobante = Convert.ToInt32(cmdCom.Parameters["@ID_COMPROBANTE_OUT"].Value);
            comprobante.Serie = cmdCom.Parameters["@SERIE"].Value.ToString();
            comprobante.Numero = cmdCom.Parameters["@NUMERO"].Value.ToString();

            return comprobante.IdComprobante;
        }

        public int ValidarTalonario(string tipoComprobante)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_VAL_TIPO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@tipoComprobante", SqlDbType.VarChar).Value = tipoComprobante;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return Convert.ToInt32(dt.Rows[0][0]);
        }

        public void RegistrarDetalleComprobante(DetalleFactura item, string codigoUsuario)
        {
            SqlCommand cmdDet2 = new SqlCommand("dbo.FMPR_INS_COMPROBANTE_DETALLE", conn, trans);
            cmdDet2.CommandType = CommandType.StoredProcedure;

            cmdDet2.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = item.id_comprobante;
            cmdDet2.Parameters.Add("@CUENTA", SqlDbType.VarChar).Value = item.cuenta;
            cmdDet2.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = item.descripcion;
            cmdDet2.Parameters.Add("@AFECTO_IMPUESTO", SqlDbType.VarChar).Value = item.afectoImpuesto;
            cmdDet2.Parameters.Add("@IMPORTE", SqlDbType.Decimal).Value = item.importe;
            cmdDet2.Parameters.Add("@USUARIO_CREACION", SqlDbType.VarChar).Value = codigoUsuario;

            cmdDet2.ExecuteNonQuery();
        }

        public void ActualizarComprobante(Comprobante comprobante, string accion)
        {
            SqlCommand cmdCom = new SqlCommand("dbo.FMPR_ACT_COMPROBANTE", conn, trans);
            cmdCom.CommandType = CommandType.StoredProcedure;

            cmdCom.Parameters.Add("@ACCION", SqlDbType.VarChar).Value = accion;
            cmdCom.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = comprobante.IdComprobante;
            cmdCom.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar).Value = comprobante.TipoDocumento;
            cmdCom.Parameters.Add("@MONEDA", SqlDbType.VarChar).Value = comprobante.Moneda;
            cmdCom.Parameters.Add("@FECHA_EMISION", SqlDbType.DateTime).Value = comprobante.FechaEmision;
            cmdCom.Parameters.Add("@CODIGO_CLIENTE", SqlDbType.VarChar).Value = comprobante.Cliente.CodigoCliente;
            cmdCom.Parameters.Add("@PROCESO", SqlDbType.VarChar).Value = comprobante.Proceso;
            cmdCom.Parameters.Add("@EXPEDIENTE", SqlDbType.VarChar).Value = comprobante.Expediente;
            cmdCom.Parameters.Add("@COD_CONCEPTO", SqlDbType.VarChar).Value = comprobante.CodigoConcepto;
            cmdCom.Parameters.Add("@TIPO_DOCREL", SqlDbType.VarChar).Value = comprobante.TipoDocumentoRelacionado;
            cmdCom.Parameters.Add("@NUMERO_DOCREL", SqlDbType.VarChar).Value = comprobante.NumeroDocumentoRelacionado;
            cmdCom.Parameters.Add("@FECHA_DOCREL", SqlDbType.DateTime).Value = comprobante.FechaDocumentoRelacionado;
            cmdCom.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = comprobante.Estado;
            cmdCom.Parameters.Add("@FLAG_IMPRESION", SqlDbType.VarChar).Value = comprobante.FlagImpresion;
            cmdCom.Parameters.Add("@FLAG_ENVIADO", SqlDbType.VarChar).Value = comprobante.FlagEnviado;
            cmdCom.Parameters.Add("@FLAG_AFECTOIGV", SqlDbType.VarChar).Value = comprobante.FlagAfectoIgv;
            cmdCom.Parameters.Add("@GLOSA", SqlDbType.VarChar).Value = comprobante.Glosa;
            cmdCom.Parameters.Add("@CANTIDAD", SqlDbType.Int).Value = comprobante.Cantidad;
            cmdCom.Parameters.Add("@PRECIO_UNITARIO", SqlDbType.Decimal).Value = comprobante.PrecioUnitario;
            cmdCom.Parameters.Add("@VALOR_VENTA", SqlDbType.Decimal).Value = comprobante.ValorVenta;
            cmdCom.Parameters.Add("@IGV", SqlDbType.Decimal).Value = comprobante.Igv;
            cmdCom.Parameters.Add("@TOTAL", SqlDbType.Decimal).Value = comprobante.Total;
            cmdCom.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = comprobante.Usuario;
            cmdCom.Parameters.Add("@TIPO_NC", SqlDbType.VarChar).Value = comprobante.TipoNotaCredito;
            cmdCom.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
            cmdCom.Parameters.Add("@PORCENTAJE_DETRACCION", SqlDbType.Decimal).Value = comprobante.PorcentajeDetraccion; //OT7999
            cmdCom.Parameters.Add("@MONTO_DETRACCION", SqlDbType.Decimal).Value = comprobante.MontoDetraccion; //OT7999

            cmdCom.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;

            cmdCom.ExecuteNonQuery();
        }

        public void ActualizarComprobanteDetalle(DetalleFactura item, string codigoUsuario)
        {
            SqlCommand cmdDet = new SqlCommand("dbo.FMPR_ACT_COMPROBANTE_DETALLE", conn, trans);
            cmdDet.CommandType = CommandType.StoredProcedure;

            cmdDet.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = item.id_comprobante;
            cmdDet.Parameters.Add("@ORDEN_DETALLE", SqlDbType.Int).Value = item.orden_detalle;
            cmdDet.Parameters.Add("@CUENTA", SqlDbType.VarChar).Value = item.cuenta;
            cmdDet.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = item.descripcion;
            cmdDet.Parameters.Add("@AFECTO_IMPUESTO", SqlDbType.VarChar).Value = item.afectoImpuesto;
            cmdDet.Parameters.Add("@IMPORTE", SqlDbType.Decimal).Value = item.importe;
            cmdDet.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar).Value = codigoUsuario;

            cmdDet.ExecuteNonQuery();
        }

        public void AnularComprobante(Comprobante comprobante)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_COMPROBANTE", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ID_COMPROBANTE", SqlDbType.Int).Value = comprobante.IdComprobante;
            cmd.Parameters.Add("@ESTADO ", SqlDbType.VarChar).Value = ConstantesING.ESTADO_INACTIVO;
            cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = comprobante.Usuario;
            cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 200);
            cmd.Parameters["@MENSAJE"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

        }

        public void ActualizarEstadoEnvio(decimal idComprobante, string codigoUsuario)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_COMPROBANTE_ENVIO", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@idComprobante", SqlDbType.Int).Value = idComprobante;
            cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = codigoUsuario;
            
            cmd.ExecuteNonQuery();
        }

        /* Inicio OT7999*/

        /// <summary>
        /// Obtiene el tipo de cambio de la fecha de proceso
        /// </summary>
        /// <param name="strFecha">Fecha de proceso</param>
        /// <returns>Registro de tipo de cambio</returns>
        public DataTable ObtenerTipoCambio(string strFecha)
        {
            //Conexion a base de datos Prem_Main
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Main"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Main"];

            SqlConnection cn = new SqlConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            try
            {
                //SqlConnection cn = GetConnection();
                //OT7940-PSC002 INI
                //SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_TIPO_CAMBIO", cn);
                SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_TIPO_CAMBIO", cn);
                //OT7940-PSC002 FIN
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmFecha = cmd.Parameters.Add("@cFechaInicio", SqlDbType.VarChar);
                prmFecha.Value = strFecha;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("TiCambio");
                da.Fill(dt);
                return dt;

            }
            catch
            {
                return null;
            }
            finally
            {
                cn.Close();
            }
        }
        /* Fin OT7999*/
    }

}