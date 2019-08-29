/*
 * Fecha de Modificación: 29/01/2013
 * Modificado por: Davis Rixi
 * Numero de OT: 5232
 * Descripción del cambio: Creación
 * */
/*
 * Fecha de Modificación: 24/07/2013
 * Modificado por: Robert Castillo
 * Numero de OT: 5663
 * Descripción del cambio: Se agregó el parámetro flagSegundoGrupoDetalle en el método InsertarDetallesPie.
 * */
/*
 * Fecha de Modificación: 25/03/2014
 * Modificado por: Giovana Veliz
 * Numero de OT: 6304
 * Descripción del cambio: Se modificará el método InsertarDetallesPie, se agregará el campo FlagAprobado.
 * */
/* 
* Fecha Modificación:		23/07/2015
* Modificado por:			Alex Vega
* Nro de OT:				7542
* Descripción del cambio:	Se añade CommandTimeout a los metodos de la clase.
*/
/* 
* Fecha Modificación:		10/11/2016
* Modificado por:			Robert Castillo
* Nro de OT:				9488
* Descripción del cambio:	Se añaden métodos ObtenerDataCargosEECC_FIR y ObtenerDataEECC_FIR para EECC masivo de fondos FIR.
*/
/*
/* Fecha Modificación:		09/01/2017
* Modificado por:			Irene Reyes
* Nro de OT:				9772
* Descripción del cambio:	Se agregan métodos que permitan realizar el proceso EECC masivo para participes con fondos privados.
* */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
	public class EECCMasivoDA : INGFondos.Data.DA
	{
		public EECCMasivoDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) {}

		public DataTable ObtenerFondos()
		{

			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_FONDO", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			try
			{
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable("FONDOS");
				da.Fill(dt);
				return dt;		
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmd.Dispose();
				cn.Close();
			}
		}
	
		public DataTable ObtenerMovimientos(decimal idFondo, decimal idCuentaParticipacion, DateTime fechaInicio, DateTime fechaFin)
		{
		
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_OPERACIONES_EECC", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			prmFondo.Value = idFondo;

			SqlParameter prmCuenta = cmd.Parameters.Add("@idCuentaParticipacion", SqlDbType.Decimal);
			prmCuenta.Value = idCuentaParticipacion;

			SqlParameter prmFecIni = cmd.Parameters.Add("@fechaInicio", SqlDbType.DateTime);
			prmFecIni.Value = fechaInicio;

			SqlParameter prmFecFin = cmd.Parameters.Add("@fechaFin", SqlDbType.DateTime);
			prmFecFin.Value = fechaFin;
         
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("MOVIMIENTOS");
			da.Fill(dt);
			return dt;			
		}

		public DataTable ObtenerParticipeIndividual(decimal idFondo, DateTime fechaInicio, DateTime fechaFin, decimal idParticipe)
		{
		
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_PARTICIPE_EECC", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			prmFondo.Value = idFondo;

			SqlParameter prmParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
			prmParticipe.Value = idParticipe;

			SqlParameter prmFecIni = cmd.Parameters.Add("@fechaInicio", SqlDbType.DateTime);
			prmFecIni.Value = fechaInicio;

			SqlParameter prmFecFin = cmd.Parameters.Add("@fechaFin", SqlDbType.DateTime);
			prmFecFin.Value = fechaFin;
         
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("PARTICIPESIND");
			da.Fill(dt);
			return dt;	
		}


		public decimal ObtenerSaldoEnCuenta(decimal idFondo, decimal idCuentaParticipacion, DateTime fecha)
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_SALDO_CUENTA_EECC", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			prmFondo.Value = idFondo;

			SqlParameter prmCuenta = cmd.Parameters.Add("@idCuentaParticipacion", SqlDbType.Decimal);
			prmCuenta.Value = idCuentaParticipacion;

			SqlParameter prmFecha = cmd.Parameters.Add("@fechaCierre", SqlDbType.DateTime);
			prmFecha.Value = fecha;
         
			decimal saldo = 0;

			SqlDataAdapter da = new SqlDataAdapter(cmd);			
			cn.Open();
			
			try
			{ 
				saldo = Convert.ToDecimal(cmd.ExecuteScalar());								
				
			}
			finally
			{
				cmd.Dispose();
				cn.Close();
			}

			return saldo;
		}


		public decimal ObtenerSaldoEnFondo(decimal idFondo, decimal idParticipe, DateTime fecha)
		{		

			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_SALDO_FONDO_EECC", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			prmFondo.Value = idFondo;

			SqlParameter prmParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
			prmParticipe.Value = idParticipe;

			SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
			prmFecha.Value = fecha;
         
			decimal saldo = 0;

			cn.Open();		
			
			try
			{
				saldo = Convert.ToDecimal(cmd.ExecuteScalar());												
			}
			finally
			{
				cmd.Dispose();
				cn.Close();
			}

			return saldo;
		}


		public DataTable ObtenerMacomunos(int codigoParticipe)
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_PARTICIPES_MANCOMUNOS", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmCodigo = cmd.Parameters.Add("@codigoCuentaMancomunada", SqlDbType.Int);
			prmCodigo.Value = codigoParticipe;
         
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("MANCOMUNOS");
			da.Fill(dt);
			return dt;
		}


		public decimal ObtenerValorCuota(decimal idFondo, DateTime fecha)
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_VALOR_CUOTA_PRECIERRE", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			prmIdFondo.Value = idFondo;

			SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
			prmFecha.Value = fecha;
         
			decimal valorCuota = 0;
			try
			{
				cn.Open();
				valorCuota = Convert.ToDecimal(cmd.ExecuteScalar());
			}
			finally
			{
				cmd.Dispose();
				cn.Close();
			}

			return valorCuota;
		}

		public decimal CargaParticipesEECC(decimal idFondo,DateTime fechainicial,DateTime fechafinal)
		{
			decimal indicadorCargaParticipe=0;

			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("DBO.INGF_CARGA_PARTICIPES_EECC", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmFechaInicial = cmd.Parameters.Add("@fechainicio", SqlDbType.DateTime);
			prmFechaInicial.Value = fechainicial;

			SqlParameter prmFechaFinal = cmd.Parameters.Add("@fechafin", SqlDbType.DateTime);
			prmFechaFinal.Value = fechafinal;

			SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			prmIdFondo.Value = idFondo;			         
						
			try
			{
				cn.Open();
				cmd.ExecuteNonQuery();
				indicadorCargaParticipe=1;
			}
			finally
			{
				cmd.Dispose();
				cn.Close();
			}

			return indicadorCargaParticipe;
		}

		
		public decimal CargarDatosEECC(decimal idFondo, DateTime fechainicial, DateTime fechafinal)
		{
			decimal valorIndicador=0;

			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_CARGA_DATA_EECC", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmFechaInicial = cmd.Parameters.Add("@fechainicial", SqlDbType.DateTime);
			prmFechaInicial.Value = Convert.ToDateTime(fechainicial.ToShortDateString());

			SqlParameter prmFechaFinal = cmd.Parameters.Add("@fechafinal", SqlDbType.DateTime);
			prmFechaFinal.Value = Convert.ToDateTime(fechafinal.ToShortDateString());

			SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			prmIdFondo.Value = idFondo;			         

			try
			{
				cn.Open();
				cmd.ExecuteNonQuery();
				valorIndicador=1;
			}
			finally
			{
				cmd.Dispose();
				cn.Close();
			}

			return valorIndicador;
		}
		
		public DataSet ObtenerDataEECC(DateTime fechaInicio, DateTime fechaFin, string strTipoEECC,int indicadorMasiva)
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_GENERAR_DATA_EECC", cn);
			try
			{				
				cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5000;

				SqlParameter prmFechaInicio = cmd.Parameters.Add("@fechainicial", SqlDbType.DateTime);
				prmFechaInicio.Value = Convert.ToDateTime(fechaInicio.ToShortDateString());

				SqlParameter prmFechaFin = cmd.Parameters.Add("@fechafinal", SqlDbType.DateTime);
				prmFechaFin.Value = Convert.ToDateTime(fechaFin.ToShortDateString());

				SqlParameter prmTipoEECC = cmd.Parameters.Add("@tipoEECC", SqlDbType.VarChar);
				prmTipoEECC.Value = strTipoEECC;

				SqlParameter prmIndicador = cmd.Parameters.Add("@indicador", SqlDbType.Int);
				prmIndicador.Value = indicadorMasiva;

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataSet ds = new DataSet();
				da.Fill(ds);
				ds.Tables[0].TableName = "PARTICIPES";
				ds.Tables[1].TableName = "MANCOMUNOS";
				ds.Tables[2].TableName = "SALDOS";
				ds.Tables[3].TableName = "MOVIMIENTOS";
				ds.Tables[4].TableName = "CUENTAS_PARTICIPACION";
				return ds;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmd.Dispose();
				cn.Close();
			}
		}


		public DataTable ObtenerDataCargosEECC(DateTime fechaInicio, DateTime fechaFin, string strTipoEECC)
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_CARGOS_EECC", cn);
			try
			{
				cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5000;

				SqlParameter prmFechaInicio = cmd.Parameters.Add("@fechainicial", SqlDbType.DateTime);
				prmFechaInicio.Value = Convert.ToDateTime(fechaInicio.ToShortDateString());

				SqlParameter prmFechaFin = cmd.Parameters.Add("@fechafinal", SqlDbType.DateTime);
				prmFechaFin.Value = Convert.ToDateTime(fechaFin.ToShortDateString());

				
				SqlParameter prmTipoEECC = cmd.Parameters.Add("@tipoEECC", SqlDbType.VarChar);
				prmTipoEECC.Value = strTipoEECC;
         
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable("CARGOS");
				da.Fill(dt);
			
				return dt;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmd.Dispose();
				cn.Close();
			}
		}

		public DataTable ObtenerMensaje()
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_MENSAJE_EECC", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("MENSAJE");
			da.Fill(dt);
			return dt;
		}

		public DataTable ObtenerMensajexPeriodo(int mes,int anio)
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_OBT_MENSAJE_PERIODO_EECC", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmMes = cmd.Parameters.Add("@idMes", SqlDbType.Int);
			prmMes.Value = mes;
			SqlParameter prmAnio = cmd.Parameters.Add("@anio", SqlDbType.Int);
			prmAnio.Value = anio;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("MENSAJE");
			da.Fill(dt);
			return dt;
		}

		public DataTable ObtenerPiesPaginaxPeriodo(int mes,int anio)
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_MENSAJE_EECC_DETALLE", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmMes = cmd.Parameters.Add("@idMes", SqlDbType.Int);
			prmMes.Value = mes;
			SqlParameter prmAnio = cmd.Parameters.Add("@anio", SqlDbType.Int);
			prmAnio.Value = anio;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("PIES");
			da.Fill(dt);
			return dt;
		}

		public DataTable ObtenerMeses()
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_TABLA_GENERAL", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmCod = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
			prmCod.Value = "MES";
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("MESES");
			da.Fill(dt);
			return dt;
		}

		public decimal ActualizarMensaje(int mes,int anio, string titulo,string mensaje,string pie,string usuario)
		{
			decimal valorIndicador=0;

			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_ACT_MENSAJE_EECC", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmMes = cmd.Parameters.Add("@mes", SqlDbType.Int);
			prmMes.Value = mes;

			SqlParameter prmAnio = cmd.Parameters.Add("@anio", SqlDbType.Int);
			prmAnio.Value = anio;

			SqlParameter prmTitulo = cmd.Parameters.Add("@titulo", SqlDbType.VarChar);
			prmTitulo.Value = titulo;

			SqlParameter prmMensaje = cmd.Parameters.Add("@mensaje", SqlDbType.Text);
			prmMensaje.Value = mensaje;

			SqlParameter prmPie = cmd.Parameters.Add("@pie", SqlDbType.VarChar);
			prmPie.Value = pie;
			
			SqlParameter prmUsuario = cmd.Parameters.Add("@usuariomodificacion", SqlDbType.VarChar);
			prmUsuario.Value = usuario;

			try
			{
				cn.Open();
				cmd.ExecuteNonQuery();
				valorIndicador=1;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cn.Close();
			}

			return valorIndicador;
		}

		public decimal InsertarMensaje(int mes,int anio,string titulo,string mensaje,string pie,string usuario)
		{
			decimal valorIndicador=0;

			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_INS_MENSAJE_EECC", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmMes = cmd.Parameters.Add("@mes", SqlDbType.Int);
			prmMes.Value = mes;

			SqlParameter prmAnio = cmd.Parameters.Add("@anio", SqlDbType.Int);
			prmAnio.Value = anio;

			SqlParameter prmTitulo = cmd.Parameters.Add("@titulo", SqlDbType.VarChar);
			prmTitulo.Value = titulo;

			SqlParameter prmMensaje = cmd.Parameters.Add("@mensaje", SqlDbType.Text);
			prmMensaje.Value = mensaje;

			SqlParameter prmPie = cmd.Parameters.Add("@pie", SqlDbType.VarChar);
			prmPie.Value = pie;

			SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
			prmUsuario.Value = usuario;

			try
			{
				cn.Open();
				cmd.ExecuteNonQuery();
				valorIndicador=1;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cn.Close();
			}

			return valorIndicador;
		}


        public decimal InsertarDetallesPie(int mes, int anio, string detalle1, string detalle2, string detalle3, string detalle4, string detalle5, string detalle6, string usuario, string flagSegundoGrupoDetalle, string flagAprobado)
		{
			decimal valorIndicador=0;

			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_INS_MENSAJE_EECC_DETALLE", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmMes = cmd.Parameters.Add("@idMes", SqlDbType.Int);
			prmMes.Value = mes;

			SqlParameter prmAnio = cmd.Parameters.Add("@anio", SqlDbType.Int);
			prmAnio.Value = anio;

			SqlParameter prmDetalle1 = cmd.Parameters.Add("@detalle1", SqlDbType.Text);
			prmDetalle1.Value = detalle1;

			SqlParameter prmDetalle2 = cmd.Parameters.Add("@detalle2", SqlDbType.Text);
			prmDetalle2.Value = detalle2;

			SqlParameter prmDetalle3 = cmd.Parameters.Add("@detalle3", SqlDbType.Text);
			prmDetalle3.Value = detalle3;

			SqlParameter prmDetalle4 = cmd.Parameters.Add("@detalle4", SqlDbType.Text);
			prmDetalle4.Value = detalle4;

			SqlParameter prmDetalle5 = cmd.Parameters.Add("@detalle5", SqlDbType.Text);
			prmDetalle5.Value = detalle5;

			SqlParameter prmDetalle6 = cmd.Parameters.Add("@detalle6", SqlDbType.Text);
			prmDetalle6.Value = detalle6;

			SqlParameter prmUsuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
			prmUsuario.Value = usuario;

            SqlParameter prmFlagSegundoGrupoDetalle = cmd.Parameters.Add("@FLAG_SEGUNDO_GRUPO_DETALLE", SqlDbType.VarChar);
            prmFlagSegundoGrupoDetalle.Value = flagSegundoGrupoDetalle;

            //OT 6304 INI
            SqlParameter prmFlagAprobado = cmd.Parameters.Add("@Flag_Aprobado", SqlDbType.VarChar);
            prmFlagAprobado.Value = flagAprobado;
            //OT 6304 FIN

			try
			{
				cn.Open();
				cmd.ExecuteNonQuery();
				valorIndicador=1;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cn.Close();
			}

			return valorIndicador;
		}

		public decimal CargarCucsMuestra(DateTime fechaInicio,DateTime fechaFinal, int indicador)
		{
			decimal valorIndicador=0;

			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_CARGA_CUC_EECCMUESTRA", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmFechaInicial = cmd.Parameters.Add("@fechainicial", SqlDbType.DateTime);
			prmFechaInicial.Value = fechaInicio;

			SqlParameter prmFechaFinal = cmd.Parameters.Add("@fechafinal", SqlDbType.DateTime);
			prmFechaFinal.Value = fechaFinal;

			SqlParameter prmIndicador = cmd.Parameters.Add("@indicador", SqlDbType.Int);
			prmIndicador.Value = indicador;
			try
			{
				cn.Open();
				cmd.ExecuteNonQuery();
				valorIndicador=1;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cn.Close();
			}

			return valorIndicador;
		}

        //OT9772 INI
        public decimal CargarCucsMuestra_PRI(DateTime fechaInicio, DateTime fechaFinal, int indicador)
        {
            decimal valorIndicador = 0;

            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_CARGA_CUC_EECCMUESTRA_FONDOS_PRIVADOS", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

            SqlParameter prmFechaInicial = cmd.Parameters.Add("@fechainicial", SqlDbType.DateTime);
            prmFechaInicial.Value = fechaInicio;

            SqlParameter prmFechaFinal = cmd.Parameters.Add("@fechafinal", SqlDbType.DateTime);
            prmFechaFinal.Value = fechaFinal;

            SqlParameter prmIndicador = cmd.Parameters.Add("@indicador", SqlDbType.Int);
            prmIndicador.Value = indicador;
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                valorIndicador = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Close();
            }

            return valorIndicador;
        }
        //OT9772 FIN

		public DataTable ObtenerTablaGeneralByCodigo(string codigo)
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_TABLA_GENERAL", cn);
			cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 5000;

			SqlParameter prmCod = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
			prmCod.Value = codigo;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("TablaGeneral");
			da.Fill(dt);
			return dt;
		}
        //Inicio OT9488
        public DataSet ObtenerDataEECC_FIR(DateTime fechaInicio, DateTime fechaFin, string strTipoEECC, int indicadorMasiva)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_GENERAR_DATA_EECC_FONDOS_FIRBI", cn);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5000;

                SqlParameter prmFechaInicio = cmd.Parameters.Add("@fechainicial", SqlDbType.DateTime);
                prmFechaInicio.Value = Convert.ToDateTime(fechaInicio.ToShortDateString());

                SqlParameter prmFechaFin = cmd.Parameters.Add("@fechafinal", SqlDbType.DateTime);
                prmFechaFin.Value = Convert.ToDateTime(fechaFin.ToShortDateString());

                SqlParameter prmTipoEECC = cmd.Parameters.Add("@tipoEECC", SqlDbType.VarChar);
                prmTipoEECC.Value = strTipoEECC;

                SqlParameter prmIndicador = cmd.Parameters.Add("@indicador", SqlDbType.Int);
                prmIndicador.Value = indicadorMasiva;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                ds.Tables[0].TableName = "PARTICIPES";
                ds.Tables[1].TableName = "MANCOMUNOS";
                ds.Tables[2].TableName = "SALDOS";
                ds.Tables[3].TableName = "MOVIMIENTOS";
                ds.Tables[4].TableName = "SERIE";
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

        public DataTable ObtenerDataCargosEECC_FIR(DateTime fechaInicio, DateTime fechaFin, string strTipoEECC)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_CARGOS_EECC_FONDOS_FIRBI", cn);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5000;

                SqlParameter prmFechaInicio = cmd.Parameters.Add("@fechainicial", SqlDbType.DateTime);
                prmFechaInicio.Value = Convert.ToDateTime(fechaInicio.ToShortDateString());

                SqlParameter prmFechaFin = cmd.Parameters.Add("@fechafinal", SqlDbType.DateTime);
                prmFechaFin.Value = Convert.ToDateTime(fechaFin.ToShortDateString());


                SqlParameter prmTipoEECC = cmd.Parameters.Add("@tipoEECC", SqlDbType.VarChar);
                prmTipoEECC.Value = strTipoEECC;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("CARGOS");
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
        //Fin OT9488

        //INI OT9772

        public DataSet ObtenerDataEECC_PRI(DateTime fechaInicio, DateTime fechaFin, string strTipoEECC, int indicadorMasiva)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_GENERAR_DATA_EECC_FONDOS_PRIVADOS", cn);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5000;

                SqlParameter prmFechaInicio = cmd.Parameters.Add("@fechainicial", SqlDbType.DateTime);
                prmFechaInicio.Value = Convert.ToDateTime(fechaInicio.ToShortDateString());

                SqlParameter prmFechaFin = cmd.Parameters.Add("@fechafinal", SqlDbType.DateTime);
                prmFechaFin.Value = Convert.ToDateTime(fechaFin.ToShortDateString());

                SqlParameter prmTipoEECC = cmd.Parameters.Add("@tipoEECC", SqlDbType.VarChar);
                prmTipoEECC.Value = strTipoEECC;

                SqlParameter prmIndicador = cmd.Parameters.Add("@indicador", SqlDbType.Int);
                prmIndicador.Value = indicadorMasiva;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                ds.Tables[0].TableName = "PARTICIPES";
                ds.Tables[1].TableName = "MANCOMUNOS";
                ds.Tables[2].TableName = "SALDOS";
                ds.Tables[3].TableName = "MOVIMIENTOS";
                ds.Tables[4].TableName = "SERIE";
                ds.Tables[5].TableName = "FONDO";
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

        public DataTable ObtenerDataCargosEECC_PRI(DateTime fechaInicio, DateTime fechaFin, string strTipoEECC)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_OBT_CARGOS_EECC_FONDOS_PRIVADOS", cn);
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 5000;

                SqlParameter prmFechaInicio = cmd.Parameters.Add("@fechainicial", SqlDbType.DateTime);
                prmFechaInicio.Value = Convert.ToDateTime(fechaInicio.ToShortDateString());

                SqlParameter prmFechaFin = cmd.Parameters.Add("@fechafinal", SqlDbType.DateTime);
                prmFechaFin.Value = Convert.ToDateTime(fechaFin.ToShortDateString());


                SqlParameter prmTipoEECC = cmd.Parameters.Add("@tipoEECC", SqlDbType.VarChar);
                prmTipoEECC.Value = strTipoEECC;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("CARGOS");
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
        //FIN OT9772

	}
}
