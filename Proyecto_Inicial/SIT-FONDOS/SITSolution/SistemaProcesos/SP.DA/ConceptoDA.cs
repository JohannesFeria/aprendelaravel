/*
 * Fecha de Modificación: 26/05/2015
 * Modificado por:        Alex Vega
 * Numero de OT:          7349
 * Descripción del cambio: Creación. 
 * */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using INGFondos.Data;
using Procesos.TD;
using Procesos.Constants;
using System.Configuration;


namespace Procesos.DA
{
    public class ConceptoDA : INGFondos.Data.DA
    {
        public ConceptoDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        public DataTable ListarConceptoEstado(string strEstado, string strOrden)
        {
            // Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = GetConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_CONCEPTO_COMPROBANTE_ESTADO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

            try
            {
                cmd.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = strEstado;
                cmd.Parameters.Add("@ORDEN", SqlDbType.VarChar).Value = strOrden;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("CONCEPTO");
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

        public DataTable ObtenerConcepto(string strCodigo)
        {
            // Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = GetConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_CONCEPTO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigo = cmd.Parameters.Add("@COD_CONCEPTO", SqlDbType.VarChar, 10);
                prmCodigo.Value = strCodigo;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("CONCEPTO");
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

        public DataTable ListarCuentas()
        {
            // Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = GetConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_CUENTA_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandTimeout = 5000;

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("CUENTA");
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

        public string InsertarConcepto(string strCuenta, string strDescripcion, string strTipo, string strMoneda, string strEstado, string strUsuario)
        {
            // Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = GetConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_CONCEPTO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@COD_CUENTA_COMPROBANTE", SqlDbType.VarChar).Value = strCuenta;
                cmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = strDescripcion;
                cmd.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar).Value = strTipo;
                cmd.Parameters.Add("@MONEDA", SqlDbType.VarChar).Value = strMoneda;
                cmd.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = strEstado;
                cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = strUsuario;
                cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 200);
                cmd.Parameters["@Mensaje"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cn.Close();
                string strMensaje = cmd.Parameters["@Mensaje"].Value.ToString();

                return strMensaje;

            }
            catch (Exception ex)
            {
                throw ex;
            }        
        }

        public int ActualizarConcepto(int intCaso, string strCuenta, string strDescripcion, string strTipo, string strMoneda, string strEstado, string strUsuario, string strCodigo)
        {
            // Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = GetConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_CONCEPTO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@CASO", SqlDbType.Int).Value = intCaso;
                cmd.Parameters.Add("@COD_CUENTA_COMPROBANTE", SqlDbType.VarChar).Value = strCuenta;
                cmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar).Value = strDescripcion;
                cmd.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar).Value = strTipo;
                cmd.Parameters.Add("@MONEDA", SqlDbType.VarChar).Value = strMoneda;
                cmd.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = strEstado;
                cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = strUsuario;
                cmd.Parameters.Add("@COD_CONCEPTO", SqlDbType.VarChar).Value = strCodigo;
                                
                int numero = Convert.ToInt32(cmd.ExecuteNonQuery());
                return numero;
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

        public DataTable EliminarConcepto(string strCodigoConcepto, string strUsuario)
        {
            // Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = GetConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_CONCEPTO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@COD_CONCEPTO", SqlDbType.VarChar).Value = strCodigoConcepto;
                cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = strUsuario;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("CONCEPTO");
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

        public int CorrelativoConcepto()
        {
            // Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = GetConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_CORRELATIVO_CONCEPTO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("CONCEPTO");
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

        public DataTable ListarConceptos(string strTipoComprobante, string strEstado)
        {
            // Conexion a base de datos Prem_Banco13
            string strServidor = ConfigurationSettings.AppSettings["ServidorPrem_Banco13"];
            string strBD = ConfigurationSettings.AppSettings["BDPrem_Banco13"];

            SqlConnection cn = GetConnection();

            string strCn = @"data source=" + strServidor + ";initial catalog=" + strBD + ";integrated security=SSPI";
            cn.ConnectionString = strCn;
            cn.Open();

            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_CONCEPTO_COMPROBANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@COD_CONCEPTO", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@TIPO_DOCUMENTO", SqlDbType.VarChar).Value = (ConstantesING.TIPO_DOCUMENTO_BOLETA.Equals(strTipoComprobante)) ? strTipoComprobante : ConstantesING.TIPO_DOCUMENTO_FACTURA;
                cmd.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = strEstado;
                cmd.Parameters.Add("@ORDEN", SqlDbType.VarChar).Value = ConstantesING.COD_CONCEPTO_ORDEN_ASC;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("CONCEPTO");
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
