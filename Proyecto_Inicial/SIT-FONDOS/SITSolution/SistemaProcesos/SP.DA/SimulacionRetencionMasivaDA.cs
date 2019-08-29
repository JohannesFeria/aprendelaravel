/*
 * Fecha de Creación: 05/07/2017
 * Creado por: Anthony Joaquin
 * Numero de OT: 10563
 * Descripción del cambio: Creación
 * */

using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
	public class SimulacionRetencionMasivaDA : INGFondos.Data.DA
	{
		public SimulacionRetencionMasivaDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

		public DataTable ObtenerFondos()
		{
			SqlConnection cn = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_FONDO", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmArea = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
			prmArea.Value = "ACT";

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("FONDOS");
			da.Fill(dt);
			return dt;
		}

		public void eliminarOperacionSimulacion( SqlConnection cn, SqlTransaction trans)
		{
			DataSet dt = new DataSet();
			SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_OPERACION_SIMULACION", cn, trans);
			cmd.CommandTimeout = 10000;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.ExecuteNonQuery();
		}

		public void eliminarRetencionSimulacion(SqlConnection cn, SqlTransaction trans)
		{
			DataSet dt = new DataSet();
			SqlCommand cmd = new SqlCommand("dbo.FMPR_ELI_RETENCION_SIMULACION", cn, trans);
			cmd.CommandTimeout = 10000;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.ExecuteNonQuery();
		}

		#region
		/// <summary>
		/// Listar fechas feriado
		/// </summary>
		public DataTable ListarFechasFeriados(DateTime fecha, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_FERIADO", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.VarChar);
			prmFecha.Value = fecha.ToShortDateString();

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}
		#endregion

		#region
		/// <summary>
		/// Listar fondos
		/// </summary>
		public DataTable ListarFondos(SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_BUSCAR_FONDO", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmCodigo = cmd.Parameters.Add("@codigo", SqlDbType.VarChar);
			prmCodigo.Value = DBNull.Value;
			SqlParameter prmNombreComercial = cmd.Parameters.Add("@nombreComercial", SqlDbType.VarChar);
			prmNombreComercial.Value = DBNull.Value;
			SqlParameter prmtipoFondo = cmd.Parameters.Add("@tipoFondo", SqlDbType.VarChar);
			prmtipoFondo.Value = DBNull.Value;
			SqlParameter prmTipoParticipe = cmd.Parameters.Add("@tipoParticipe", SqlDbType.VarChar);
			prmTipoParticipe.Value = DBNull.Value;
			SqlParameter prmMoneda = cmd.Parameters.Add("@moneda", SqlDbType.VarChar);
			prmMoneda.Value = DBNull.Value;
			SqlParameter prmEstado = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
			prmEstado.Value = DBNull.Value;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}
		#endregion


		public DataTable ListarTablaGeneral(string codigoTabla)
		{
			SqlConnection con = GetConnection();
			SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_TABLA_GENERAL", con);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmCodigoTabla = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
			prmCodigoTabla.Value = codigoTabla;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;

		}

		#region
		/// <summary>
		/// Listar cuenta participación
		/// </summary>
		public DataTable ListarCuentasParticipacion(int idParticipe, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_CUENTA_PARTICIPACION", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmIdParticipe = cmd.Parameters.Add("@IdParticipe", SqlDbType.Decimal);
			prmIdParticipe.Value = idParticipe;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}
		#endregion

		#region
		/// <summary>
		/// Obtener fondos
		/// </summary>
		public DataTable ObtenerFondo(int id, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_FONDO_XID", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmId = cmd.Parameters.Add("@id", SqlDbType.Decimal);
			prmId.Value = id;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}
		#endregion

		#region
		/// <summary>
		/// Listar cuentas bancarias
		/// </summary>
		public DataTable ListarCuentasBancarias(int idParticipe, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_CUENTA_BANCARIA_XPARTICIPE", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
			prmParticipe.Value = idParticipe;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}
		#endregion

		#region
		/// <summary>
		/// Obtener promotor
		/// </summary>
		public string ObtenerPromotor(int idParticipe, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_PARTICIPE_PROMOTOR", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmIdParticipe = cmd.Parameters.Add("@IdParticipe", SqlDbType.Decimal);
			prmIdParticipe.Value = idParticipe;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			string promotor = string.Empty;
			DataRow[] drPromotores = dt.Select("FLAG_ACTUAL = 'S'", "ID DESC");
			if (drPromotores.Length > 0)
			{
				promotor = drPromotores[0]["CODIGO"].ToString();
			}
			return promotor;
		}
		#endregion


		#region
		/// <summary>
		/// Obtener último Valor Cuota
		/// </summary>
		public decimal ObtenerValorCuotaFecha(string fecha, int idFondo, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_VALOR_CUOTA_FECHA", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.VarChar);
			prmFecha.Value = fecha;

			SqlParameter prmIdFondo = cmd.Parameters.Add("@idfondo", SqlDbType.Decimal);
			prmIdFondo.Value = idFondo;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			decimal vc = 0;

			if (dt.Rows.Count > 0)
			{
				vc = Convert.ToDecimal(dt.Rows[0]["VALOR_CUOTA"]);
			}

			return vc;
		}
		#endregion

		#region
		/// <summary>
		/// Obtener saldo cuotas por cuenta participación
		/// </summary>
		public decimal ObtenerSaldoCuotasxCuentaParticipacion(int idFondo, int idCuentaParticipacion, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_CUENTA_PARTICIPACION_X_FONDO_MONTO_CUOTAS", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			prmIdFondo.Value = idFondo;

			SqlParameter prmIdCuenta = cmd.Parameters.Add("@idCuenta", SqlDbType.Decimal);
			prmIdCuenta.Value = idCuentaParticipacion;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			decimal saldoCuotas = 0;

			if (dt.Rows.Count > 0)
			{
				saldoCuotas = Convert.ToDecimal(dt.Rows[0]["TOTAL_CUOTAS"]);
			}

			return saldoCuotas;
		}
		#endregion

		#region
		/// <summary>
		/// Insertar rescate
		/// </summary>
		public void InsertarOperacionSimulacion(int participeOrigen, int fondoOrigen, int cuentaParticipacionOrigen, int idTraspaso, string tipoRescate, decimal montoRescate, decimal numeroCuotas, decimal vc, string promotor, string codigo, string cuentaRecaudo, DateTime fechaSolicitudRescate, DateTime fechaPrecierre, DateTime fechaPagoRescate, string viaPago, string formaPago, decimal idCuentaBan, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_OPERACION_SIMULACION_RESCATE", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;
			#region parametros
			SqlParameter tipoOperacion = cmd.Parameters.Add("@tipoOperacion", SqlDbType.VarChar);
			SqlParameter estado = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
			SqlParameter nroCuotas = cmd.Parameters.Add("@nroCuotas", SqlDbType.Decimal);
			SqlParameter valorCuota = cmd.Parameters.Add("@valorCuota", SqlDbType.Decimal);
			SqlParameter fechaOperacion = cmd.Parameters.Add("@fechaOperacion", SqlDbType.DateTime);
			SqlParameter idParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Decimal);
			SqlParameter idCtaOrigen = cmd.Parameters.Add("@idCtaOrigen", SqlDbType.Decimal);
			SqlParameter idCtaDestino = cmd.Parameters.Add("@idCtaDestino", SqlDbType.Decimal);
			SqlParameter prmIdFondo = cmd.Parameters.Add("@idFondo", SqlDbType.Decimal);
			SqlParameter monto = cmd.Parameters.Add("@monto", SqlDbType.Decimal);
			SqlParameter montoNeto = cmd.Parameters.Add("@montoNeto", SqlDbType.Decimal);
			SqlParameter codigoPromotor = cmd.Parameters.Add("@codigoPromotor", SqlDbType.VarChar);
			SqlParameter codigoViaPago = cmd.Parameters.Add("@codigoViaPago", SqlDbType.VarChar);
			SqlParameter codigoFormaPago = cmd.Parameters.Add("@codigoFormaPago", SqlDbType.VarChar);
			SqlParameter fechaAbonoContable = cmd.Parameters.Add("@fechaAbonoContable", SqlDbType.DateTime);
			SqlParameter fechaAbonoDisponible = cmd.Parameters.Add("@fechaAbonoDisponible", SqlDbType.DateTime);
			SqlParameter importePago = cmd.Parameters.Add("@importePago", SqlDbType.Decimal);
			SqlParameter nroOperacionDeposito = cmd.Parameters.Add("@nroOperacionDeposito", SqlDbType.VarChar);
			SqlParameter nroCheque = cmd.Parameters.Add("@nroCheque", SqlDbType.VarChar);
			SqlParameter nroCuentaCheque = cmd.Parameters.Add("@nroCuentaCheque", SqlDbType.VarChar);
			SqlParameter fechaCheque = cmd.Parameters.Add("@fechaCheque", SqlDbType.DateTime);
			SqlParameter codigoBancoCheque = cmd.Parameters.Add("@codigoBancoCheque", SqlDbType.VarChar);
			SqlParameter flagRescateTotal = cmd.Parameters.Add("@flagRescateTotal", SqlDbType.VarChar);
			SqlParameter flagRescateEnCuotas = cmd.Parameters.Add("@flagRescateEnCuotas", SqlDbType.VarChar);
			SqlParameter flagRescateSignificativo = cmd.Parameters.Add("@flagRescateSignificativo", SqlDbType.VarChar);
			SqlParameter fechaProceso = cmd.Parameters.Add("@fechaProceso", SqlDbType.DateTime);
			SqlParameter idCuentaBancaria = cmd.Parameters.Add("@idCuentaBancaria", SqlDbType.Decimal);
			SqlParameter fechaCargo = cmd.Parameters.Add("@fechaCargo", SqlDbType.DateTime);
			SqlParameter flagNivel1 = cmd.Parameters.Add("@flagNivel1", SqlDbType.VarChar);
			SqlParameter flagNivel2 = cmd.Parameters.Add("@flagNivel2", SqlDbType.VarChar);
			SqlParameter flagEliminado = cmd.Parameters.Add("@flagEliminado", SqlDbType.VarChar);
			SqlParameter adicional1 = cmd.Parameters.Add("@adicional1", SqlDbType.VarChar);
			SqlParameter adicional2 = cmd.Parameters.Add("@adicional2", SqlDbType.VarChar);
			SqlParameter adicional3 = cmd.Parameters.Add("@adicional3", SqlDbType.VarChar);
			SqlParameter idOperacionReferencia = cmd.Parameters.Add("@idOperacionReferencia", SqlDbType.Decimal);
			SqlParameter idPreCierre = cmd.Parameters.Add("@idPreCierre", SqlDbType.Decimal);
			SqlParameter codigoViaSolicitud = cmd.Parameters.Add("@codigoViaSolicitud", SqlDbType.VarChar);
			SqlParameter idFondoDestino = cmd.Parameters.Add("@idFondoDestino", SqlDbType.Decimal);
			SqlParameter numeroSolicitud = cmd.Parameters.Add("@numeroSolicitud", SqlDbType.VarChar);
			SqlParameter idParticipeDestino = cmd.Parameters.Add("@idParticipeDestino", SqlDbType.Decimal);
			SqlParameter porcentajeComision = cmd.Parameters.Add("@porcentajeComision", SqlDbType.Decimal);
			SqlParameter comision = cmd.Parameters.Add("@comision", SqlDbType.Decimal);
			SqlParameter porcentajeComisionEspecial = cmd.Parameters.Add("@porcentajeComisionEspecial", SqlDbType.Decimal);
			SqlParameter comisionEspecial = cmd.Parameters.Add("@comisionEspecial", SqlDbType.Decimal);
			SqlParameter igv = cmd.Parameters.Add("@igv", SqlDbType.Decimal);
			SqlParameter flagProcesoComisiones = cmd.Parameters.Add("@flagProcesoComisiones", SqlDbType.VarChar);
			SqlParameter montoAjustadoBruto = cmd.Parameters.Add("@montoAjustadoBruto", SqlDbType.Decimal);
			SqlParameter montoAjustadoNeto = cmd.Parameters.Add("@montoAjustadoNeto", SqlDbType.Decimal);
			SqlParameter concepto = cmd.Parameters.Add("@concepto", SqlDbType.VarChar);
			SqlParameter usuarioCreacion = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
			SqlParameter usuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
			SqlParameter areaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
			SqlParameter estadoAnterior = cmd.Parameters.Add("@estadoAnterior", SqlDbType.VarChar);
			SqlParameter idFondoOrigen = cmd.Parameters.Add("@idFondoOrigen", SqlDbType.Decimal);
			SqlParameter idParticipeOrigen = cmd.Parameters.Add("@idParticipeOrigen", SqlDbType.Decimal);
			SqlParameter numeroCertificado = cmd.Parameters.Add("@numeroCertificado", SqlDbType.VarChar);
			SqlParameter fechaSolicitud = cmd.Parameters.Add("@fechaSolicitud", SqlDbType.VarChar);
			SqlParameter fechaPago = cmd.Parameters.Add("@fechaPago", SqlDbType.VarChar);
			SqlParameter flagRescateProgramado = cmd.Parameters.Add("@flagRescateProgramado", SqlDbType.VarChar);

			#endregion

			#region Valores en Común de los rescates
			tipoOperacion.Value = Constants.ConstantesING.TIPO_OPERACION_RESCATE;
            estado.Value = Constants.ConstantesING.ESTADO_OPERACION_SIMULACION;
			fechaOperacion.Value = DateTime.Now.ToString(Constants.ConstantesING.FORMATO);
			idCtaDestino.Value = DBNull.Value;

			flagRescateSignificativo.Value = Constants.ConstantesING.NO;

			fechaCargo.Value = DBNull.Value;
			flagNivel1.Value = Constants.ConstantesING.NO;
			flagNivel2.Value = Constants.ConstantesING.NO;
			flagEliminado.Value = Constants.ConstantesING.NO;
			adicional1.Value = DBNull.Value;
			if (!cuentaRecaudo.Equals(string.Empty))
			{
				adicional2.Value = Constants.ConstantesING.ADICIONAL2;
				adicional3.Value = cuentaRecaudo;
			}
			else
			{
				adicional2.Value = DBNull.Value;
				adicional3.Value = DBNull.Value;
			}
			idOperacionReferencia.Value = DBNull.Value;
			idPreCierre.Value = DBNull.Value;
			codigoViaSolicitud.Value = Constants.ConstantesING.CODIGO_VIA_SOLICITUD_AGENCIA;
			idFondoDestino.Value = DBNull.Value;
			numeroSolicitud.Value = DBNull.Value;
			idParticipeDestino.Value = DBNull.Value;
			porcentajeComision.Value = DBNull.Value;
			comision.Value = DBNull.Value;
			porcentajeComisionEspecial.Value = DBNull.Value;
			comisionEspecial.Value = DBNull.Value;
			igv.Value = Constants.ConstantesING.IGV_VALOR;
			flagProcesoComisiones.Value = Constants.ConstantesING.NO;
			montoAjustadoBruto.Value = DBNull.Value;
			montoAjustadoNeto.Value = DBNull.Value;
			if (idTraspaso == 0)
			{
				concepto.Value = DBNull.Value;
			}
			else
			{
				concepto.Value = idTraspaso.ToString();
			}
			usuarioCreacion.Value = codigo;
			usuarioModificacion.Value = codigo;
			areaModificacion.Value = Constants.ConstantesING.AREA_TRASPASO_FONDO;
			estadoAnterior.Value = Constants.ConstantesING.ESTADO_OPERACION_PENDIENTE;
			flagRescateProgramado.Value = Constants.ConstantesING.NO;
			fechaSolicitud.Value = fechaSolicitudRescate.ToString(Constants.ConstantesING.FORMATO);//DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//PONER FECHA Y HORA QUE CORRESPONDE PARA EL SIGUIENTE CIERRE EN TESTING
			fechaPago.Value = fechaPagoRescate.ToString(Constants.ConstantesING.FORMATO);
			#endregion

			#region variables
			if (numeroCuotas > 0)
			{
				nroCuotas.Value = numeroCuotas;
			}
			else
			{
				nroCuotas.Value = DBNull.Value;
			}
			valorCuota.Value = DBNull.Value;
			idParticipe.Value = participeOrigen;
			idCtaOrigen.Value = cuentaParticipacionOrigen;
			prmIdFondo.Value = fondoOrigen;
			if (tipoRescate == Constants.ConstantesING.TIPO_RESCATE_TOTAL)
			{
				flagRescateTotal.Value = Constants.ConstantesING.SI;
				flagRescateEnCuotas.Value = Constants.ConstantesING.SI;
			}
			else
			{
				flagRescateTotal.Value = Constants.ConstantesING.NO;
				if (numeroCuotas > 0)
				{
					flagRescateEnCuotas.Value = Constants.ConstantesING.SI;
				}
				else
				{
					flagRescateEnCuotas.Value = Constants.ConstantesING.NO;
				}
			}

			if (montoRescate > 0)
			{
				monto.Value = montoRescate;
				montoNeto.Value = montoRescate;
			}
			else
			{
				monto.Value = numeroCuotas * vc;
				montoNeto.Value = numeroCuotas * vc;
			}

			codigoPromotor.Value = promotor;
			if (codigoPromotor.Value.Equals(""))
				codigoPromotor.Value = DBNull.Value;

			codigoViaPago.Value = viaPago;
			codigoFormaPago.Value = formaPago;
			if (formaPago.Equals(Constants.ConstantesING.CODIGO_FORMA_PAGO_TRASPASO_FONDOS) && cuentaRecaudo.Equals(string.Empty)) //preguntar si es traspasos a fondo
			{
				idCuentaBancaria.Value = idCuentaBan;
			}
			else
			{
				idCuentaBancaria.Value = DBNull.Value;
			}
			fechaAbonoContable.Value = DBNull.Value;
			fechaAbonoDisponible.Value = DBNull.Value;
			importePago.Value = DBNull.Value;
			nroOperacionDeposito.Value = DBNull.Value;
			nroCheque.Value = DBNull.Value;
			nroCuentaCheque.Value = DBNull.Value;
			fechaCheque.Value = DBNull.Value;
			codigoBancoCheque.Value = DBNull.Value;
			fechaProceso.Value = fechaPrecierre.ToString(Constants.ConstantesING.FORMATO);
			idFondoOrigen.Value = fondoOrigen;
			idParticipeOrigen.Value = participeOrigen;
			numeroCertificado.Value = DBNull.Value;

			#endregion
			cmd.ExecuteNonQuery();

		}
		#endregion

        public DataTable listarTablaGeneralXCodigo(string codigoTabla, SqlConnection cn, SqlTransaction trans )
		{
            SqlCommand cmd = new SqlCommand("dbo.FMTR_LIS_TABLA_GENERAL_X_CODIGO", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmCodigoTabla = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
			prmCodigoTabla.Value = codigoTabla;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("TABLA_GENERAL");
			da.Fill(dt);
			return dt;
		}

		public void actualizarTablaGeneralXId(TablaGeneralTD.TABLA_GENERALRow tablaGeneral, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FMTR_ACT_TABLA_GENERAL", cn, trans);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlParameter prmId = cmd.Parameters.Add("@ID", SqlDbType.Decimal);
			prmId.Value = tablaGeneral.ID;

			SqlParameter prmLlaveTabla = cmd.Parameters.Add("@LLAVE_TABLA", SqlDbType.VarChar);
			prmLlaveTabla.Value = tablaGeneral.LLAVE_TABLA;

			SqlParameter prmNumeroOrden = cmd.Parameters.Add("@NUMERO_ORDEN", SqlDbType.Decimal);
			prmNumeroOrden.Value = tablaGeneral.NUMERO_ORDEN;

			SqlParameter prmDescripcionCorta = cmd.Parameters.Add("@DESCRIPCION_CORTA", SqlDbType.VarChar);
			prmDescripcionCorta.Value = tablaGeneral.DESCRIPCION_CORTA;

			SqlParameter prmDescripcionLarga = cmd.Parameters.Add("@DESCRIPCION_LARGA", SqlDbType.VarChar);
			prmDescripcionLarga.Value = tablaGeneral.DESCRIPCION_LARGA;

			SqlParameter prmFlagEliminado = cmd.Parameters.Add("@FLAG_ELIMINADO", SqlDbType.VarChar);
			prmFlagEliminado.Value = tablaGeneral.FLAG_ELIMINADO;

			SqlParameter prmUsuario = cmd.Parameters.Add("@USUARIO_MODIFICACION", SqlDbType.VarChar);
			prmUsuario.Value = tablaGeneral.USUARIO;

			cmd.ExecuteNonQuery();
		}


        public DataTable listarTablaGeneralAdmCuentas(string codigoTabla, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.[FOND_LIS_TABLA_GENERAL_X_CODIGO_TABLA]", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmCodigoTabla = cmd.Parameters.Add("@codigoTabla", SqlDbType.VarChar);
            prmCodigoTabla.Value = codigoTabla;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("TABLA_GENERAL");
            da.Fill(dt);
            return dt;
        }

        public void actualizarTablaGeneralAdmCuentas(TablaGeneralAdmCuenta tga, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_ACT_TABLA_GENERAL", cn, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmId = cmd.Parameters.Add("@id", SqlDbType.Int);
            prmId.Value = tga.Id;

            SqlParameter prmLlaveTabla = cmd.Parameters.Add("@llaveTabla", SqlDbType.VarChar);
            prmLlaveTabla.Value = tga.LlaveTabla;

            SqlParameter prmDescripcionCorta = cmd.Parameters.Add("@descripcionCorta", SqlDbType.VarChar);
            prmDescripcionCorta.Value = tga.DescripcionCorta;

            SqlParameter prmDescripcionLarga = cmd.Parameters.Add("@descripcionLarga", SqlDbType.VarChar);
            prmDescripcionLarga.Value = tga.DescripcionLarga;

            SqlParameter prmAbreviacion = cmd.Parameters.Add("@abreviacion", SqlDbType.VarChar);
            prmAbreviacion.Value = tga.Abreviacion;

            SqlParameter prmEstado = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
            prmEstado.Value = tga.Estado;

            cmd.ExecuteNonQuery();
        }

		public DataTable ObtenerFondosPrecierre()
		{
			SqlConnection cn = GetConnection();

			SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_PRECIERRE_FONDOS", cn);
			cmd.CommandType = CommandType.StoredProcedure;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable("FONDO");
			da.Fill(dt);
			return dt;
		}
	}
}
