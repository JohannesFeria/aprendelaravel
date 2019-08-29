/*
 * Fecha Creación: 14/06/2017
 * Creado por:	Anthony Joaquin
 * Nro de OT:	10478
 * Descripción del cambio:	Creación.
* */
/*
 * Fecha Modificación: 10/07/2017
 * Modificado por: Walter Rodriguez
 * Nro de OT: 10748 PSC001
 * Descripción del cambio:	Se agrega el método registarRentabilidad.
 * */
/*
 * Fecha Modificación: 07/08/2017
 * Modificado por: Anthony Joaquin
 * Nro de OT: 10675
 * Descripción del cambio:	Se cambia el parámetro de fecha corte por fecha_pago en el método registarRentabilidad.
 * */
/*
 * Fecha Modificación: 09/08/2017
 * Modificado por: Anthony Joaquin
 * Nro de OT: 10604
 * Descripción del cambio: Se agregan los siguientes métodos ObtenerDistribucionFondosFir, registarDistribucionFondosFIR
 *                         y ObtenerFondoMidas.
* */
/*
 * Fecha Modificación: 20/09/2017
 * Modificado por: Anthony Joaquin
 * Nro de OT: 10592
 * Descripción del cambio: Se agregan los siguientes métodos registrarPagoFlujosSobrante, listarPagoFlujosSobranteXPeriodo, eliminarPagoFlujoSobrantes.                  
* */
/*
 * Fecha Modificación: 27/11/2017
 * Modificado por: Anthony Joaquin
 * Nro de OT: 10944
 * Descripción del cambio: Se modifica el método ObtenerDistribucionFondosFir para agregar el parámetro patrimonioContable.
* */
/*
 * Fecha Modificación: 04/12/2017
 * Modificado por: Giancarlo Villanueva
 * Nro de OT: 10975
 * Descripción del cambio: Se modifica cambia procedimiento existente por uno nuevo.
* */

using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using Procesos.Constants;
using System.Text;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
	/// <summary>
    /// Descripción breve de DistribucionCalculoInteresxFondoDA.
	/// </summary>
	public class DistribucionCalculoInteresxFondoDA: INGFondos.Data.DA
	{
        public DistribucionCalculoInteresxFondoDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        public SqlConnection GetConnection2()
        {
            return base.GetConnection();
        }

		public DataTable ObtenerFondosPrecierre()
		{
			try
			{
				SqlConnection cn = GetConnection();
                SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_FONDO_PAGO_FLUJOS", cn);
				cmd.CommandType = CommandType.StoredProcedure;

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable("FONDO");
                da.Fill(dt);
                return dt;
			}
			catch
			{ 
				return null;
			}
		}   

		
        public DataTable buscarPrecierreXFecha(int idFondo, string fechaCorte)
        {
            
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_DATOS_DISTRIBUCION", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
            SqlParameter prmIdFondo = cmd.Parameters.Add("@id_fondo", SqlDbType.Int);
            prmIdFondo.Value = idFondo;

            SqlParameter prmFechaCorte = cmd.Parameters.Add("@fecha", SqlDbType.VarChar);
            prmFechaCorte.Value = fechaCorte;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtPrecierre = new DataTable("PRECIERRE_X_FECHA");
            da.Fill(dtPrecierre);
            return dtPrecierre;

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

        public void RegistarMontoDistribucion(int idFondo, double total, double montoSobrante, int idDistribucion, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_MONTO_TOTAL_DISTRIBUCION", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int).Value = idFondo;
                cmd.Parameters.Add("@MONTO", SqlDbType.Decimal).Value = total;
                cmd.Parameters.Add("@SOBRANTE", SqlDbType.Decimal).Value = montoSobrante;
                cmd.Parameters.Add("@ID_DISTRIBUCION", SqlDbType.Int).Value = idDistribucion;
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

        // OT10478 PSC001 INI
        public void registarRentabilidad(int idFondo, string mnemonico, string moneda, string intermediario, string fechaPago, double interes, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMTR_INS_RENTABILIDAD_DISTRIBUCION_FLUJO", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int).Value = idFondo;
                cmd.Parameters.Add("@MNEMONICO", SqlDbType.VarChar).Value = mnemonico;
                cmd.Parameters.Add("@MONEDA", SqlDbType.VarChar).Value = moneda;
                cmd.Parameters.Add("@INTERMEDIARIO", SqlDbType.VarChar).Value = intermediario;
                cmd.Parameters.Add("@FECHA_PAGO", SqlDbType.DateTime).Value = fechaPago;//OT10675
                cmd.Parameters.Add("@INTERESES", SqlDbType.Decimal).Value = interes;
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
        // OT10478 PSC001 FIN

        // OT10604 INI
        public DataSet ObtenerDistribucionFondosFir(DateTime fechaInicio, DateTime fechaFin, double valorRazonable, double patrimonioContable)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_DISTRIBUCION_FIR", cn);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter prmFechaInicio = cmd.Parameters.Add("@fechaInicio", SqlDbType.DateTime);
                prmFechaInicio.Value = fechaInicio.ToShortDateString();

                SqlParameter prmFechaFin = cmd.Parameters.Add("@fechaFin", SqlDbType.DateTime);
                prmFechaFin.Value = fechaFin.ToShortDateString();

                SqlParameter prmValorRazonable = cmd.Parameters.Add("@valorRazonable", SqlDbType.Decimal);
                prmValorRazonable.Value = valorRazonable;
                //OT10944 INI
                SqlParameter prmPatrimonioContable = cmd.Parameters.Add("@patrimonioContable", SqlDbType.Decimal);
                prmPatrimonioContable.Value = patrimonioContable;
                //OT10944 FIN
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                //ds.Tables[0].TableName = "PARTICIPACION_INICIO";OT10944
                ds.Tables[0].TableName = "PARTICIPACION_FIN";
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


        public void registarDistribucionFondosFIR(int idFondo, decimal monto, decimal valuacion, decimal adicion, decimal deduccion, string codigoUsuario, SqlConnection cn, SqlTransaction trans, string fechaCorte, string fechaPago)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_DISTRIBUCION_FIR", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int).Value = idFondo;
                cmd.Parameters.Add("@MONTO", SqlDbType.Decimal).Value = monto;
                cmd.Parameters.Add("@SOBRANTE", SqlDbType.Decimal).Value = 0;
                cmd.Parameters.Add("@VALUACION", SqlDbType.Decimal).Value = valuacion;
                cmd.Parameters.Add("@ADICIONES", SqlDbType.Decimal).Value = adicion;
                cmd.Parameters.Add("@DEDUCCIONES", SqlDbType.Decimal).Value = deduccion;
                cmd.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = codigoUsuario;
                cmd.Parameters.Add("@AREA_MODIFICACION", SqlDbType.VarChar).Value = Constants.ConstantesING.AREA_DISTRIBUCION_X_PERIODO;
                cmd.Parameters.Add("@FECHA_CORTE", SqlDbType.DateTime).Value = fechaCorte;
                cmd.Parameters.Add("@FECHA_PAGO", SqlDbType.DateTime).Value = fechaPago;
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


        public DataTable ValidarRentabilidad(DateTime fecha, string idFondo, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMTR_OBT_REGISTROS_RENTABILIDAD", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmFecha = cmd.Parameters.Add("@FECHA", SqlDbType.DateTime);
                prmFecha.Value = fecha;

                SqlParameter prmIdFondo = cmd.Parameters.Add("@FONDO", SqlDbType.VarChar);
                prmIdFondo.Value = idFondo;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("RENTABILIDAD");
                da.Fill(dt);
                return dt;
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

        public void EliminarRentabilidad(DateTime fecha, string idFondo, SqlConnection cn, SqlTransaction trans)
        {
            //OT10975 INI
            //SqlCommand cmd = new SqlCommand("dbo.FMTR_ELI_RENTABILIDAD_X_FECHA_FONDO", cn, trans);
            SqlCommand cmd = new SqlCommand("dbo.FMTR_ELI_RENTABILIDAD_DISTRIBUCION", cn, trans);
            //OT10975 FIN
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = fecha;
                cmd.Parameters.Add("@Fondo", SqlDbType.VarChar).Value = idFondo;
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


        public DataTable ObtenerFondoMidas(string codigoTabla, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMTR_LIS_TABLA_GENERAL_X_CODIGO", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmCodigoTabla = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
                prmCodigoTabla.Value = codigoTabla;


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("FONDOS_MIDAS");
                da.Fill(dt);
                return dt;
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

        // OT10604 FIN

        //OT10592 INI
        public void registrarPagoFlujosSobrante(PagoFlujoSobranteTD sobrante, SqlConnection cn, SqlTransaction trans)
        {
            
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_PAGO_FLUJOS_SOBRANTE", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int).Value = sobrante.IdFondo;
                cmd.Parameters.Add("@PERIODO", SqlDbType.Int).Value = sobrante.Periodo;
                cmd.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = sobrante.Fecha;
                cmd.Parameters.Add("@ORDEN", SqlDbType.VarChar).Value = sobrante.Orden;
                cmd.Parameters.Add("@LIQUIDACION", SqlDbType.DateTime).Value = sobrante.Liquidacion;
                cmd.Parameters.Add("@FIN_CONTRATO", SqlDbType.DateTime).Value = sobrante.FinContrato;
                cmd.Parameters.Add("@MONEDA", SqlDbType.VarChar).Value = sobrante.Moneda;
                cmd.Parameters.Add("@MNEMONICO", SqlDbType.VarChar).Value = sobrante.Mnemonico;
                cmd.Parameters.Add("@OPERACION", SqlDbType.VarChar).Value = sobrante.Operacion;
                cmd.Parameters.Add("@TASA", SqlDbType.Decimal).Value = sobrante.Tasa;
                cmd.Parameters.Add("@TIPO_CAMBIO", SqlDbType.Decimal).Value = sobrante.TipoCambio;
                cmd.Parameters.Add("@PRECIO", SqlDbType.Decimal).Value = sobrante.Precio;
                cmd.Parameters.Add("@MONTO", SqlDbType.Decimal).Value = sobrante.Monto;
                cmd.Parameters.Add("@CANTIDAD", SqlDbType.Decimal).Value = sobrante.Cantidad;
                cmd.Parameters.Add("@INTERMEDIARIO", SqlDbType.VarChar).Value = sobrante.Intermediario;
                cmd.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = sobrante.Estado;
                cmd.Parameters.Add("@DIAS", SqlDbType.Int).Value = sobrante.Dias;
                cmd.Parameters.Add("@FACTOR", SqlDbType.Decimal).Value = sobrante.Factor;
                cmd.Parameters.Add("@MONTO_FINAL", SqlDbType.Decimal).Value = sobrante.MontoFinal;
                cmd.Parameters.Add("@INTERES_DPZ", SqlDbType.Decimal).Value = sobrante.InteresDPZ;
                cmd.Parameters.Add("@INTERES_CUPONES", SqlDbType.Decimal).Value = sobrante.InteresCupones;
                cmd.Parameters.Add("@DISTRIBUCION_FLUJOS", SqlDbType.Decimal).Value = sobrante.DistribucionFlujo;
                cmd.Parameters.Add("@SALDO_PENDIENTE", SqlDbType.Decimal).Value = sobrante.SaldoPendiente;
                cmd.Parameters.Add("@USUARIO_CREACION", SqlDbType.VarChar).Value = sobrante.Usuario;
                cmd.Parameters.Add("@AREA_MODIFICACION", SqlDbType.VarChar).Value = sobrante.Area;
                
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

        public DataTable listarPagoFlujosSobranteXPeriodo(int periodoAnterior, int idFondo)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_PAGO_FLUJOS_SOBRANTE_X_PERIODO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                SqlParameter prmPeriodo = cmd.Parameters.Add("@PERIODO", SqlDbType.Int);
                prmPeriodo.Value = periodoAnterior;

                SqlParameter prmIdFondo= cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int);
                prmIdFondo.Value = idFondo;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("PAGO_FLUJO_SOBRANTES");
                da.Fill(dt);
                return dt;
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

        public void eliminarPagoFlujoSobrantes(int periodo, int idFondo, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_PAGO_FLUJOS_SOBRANTE_X_PERIODO", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("@PERIODO", SqlDbType.Int).Value = periodo;
                cmd.Parameters.Add("@ID_FONDO", SqlDbType.Int).Value = idFondo;
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


        //OT10592 FIN
    }
}
