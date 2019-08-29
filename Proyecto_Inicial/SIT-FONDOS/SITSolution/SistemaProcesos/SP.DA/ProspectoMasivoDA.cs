/*
 * Fecha de Modificación: 20/06/2013
 * Modificado por: Robert Castillo
 * Numero de OT: 5526
 * Descripción del cambio: Creación de clase DA para la creación de Prospectos Masivos.
 * */
/*
 * Fecha de Modificación: 18/11/2014
 * Modificado por: Robert Castillo
 * Numero de OT: 6868
 * Descripción del cambio: Se agrega parámetro del sector económico al registro de partícipes.
 * */
/*
 * Fecha de Modificación: 04/12/2014
 * Modificado por: Leslie Valerio
 * Numero de OT: 6891
 * Descripción del cambio: Se agrego parametro 'prmchvSectorEconomicoDesc' para el metodo grabarDatosParticipe 
 * */
/*
 * Fecha de Modificación: 06/05/2015
 * Modificado por: Walter Rodríguez
 * Numero de OT: 7235
 * Descripción del cambio: Se agrega método InsertarLoteGeneracion.
 * */
/*
 * Fecha de Modificación: 16/07/2018
 * Modificado por: Robert Castillo
 * Numero de OT: OT11436
 * Descripción del cambio: En el método grabarDatosParticipe se agregan los parámetros prmchvCodigoCanalCliente
 *						   y prmchvFlagCertificadoRetencion.
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using SistemaProcesosTD;

namespace SistemaProcesosDA
{
	/// <summary>
	/// Descripción breve de ProspectoMasivoDA.
	/// </summary>
	public class ProspectoMasivoDA: INGFondos.Data.DA
	{
		public ProspectoMasivoDA(): base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) {}

		public DataTable ObtenerParametroSistema(string nombre, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_PARAMETRO_SISTEMA", cn);
			cmd.Transaction = trans;
			cmd.CommandType = CommandType.StoredProcedure;

			try
			{
				SqlParameter prmFecha = cmd.Parameters.Add("@nombre", SqlDbType.VarChar);
				prmFecha.Value = nombre;

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable("FONDO");
				da.Fill(dt);
				return dt;
			}
			catch(Exception e)
			{ 
				return null;
			}
			finally
			{
				cmd.Dispose();
			}
		}

		public DataTable grabarDatosParticipe(string codigoUsuario, string contrasenaCifrada, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_INS_PARTICIPE", cn);
			cmd.Transaction = trans;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 50000;

			try
			{
				SqlParameter prmChvTipoPersona = cmd.Parameters.Add("@chvTipoPersona", SqlDbType.VarChar);
				prmChvTipoPersona.Value = "NAT";

				SqlParameter prmChvRazonSocial = cmd.Parameters.Add("@chvRazonSocial", SqlDbType.VarChar);
				prmChvRazonSocial.Value = System.DBNull.Value;

				SqlParameter prmChvCodigoSectorEconomico = cmd.Parameters.Add("@chvCodigoSectorEconomico", SqlDbType.VarChar);
				prmChvCodigoSectorEconomico.Value = System.DBNull.Value;

				SqlParameter prmChvNombre1 = cmd.Parameters.Add("@chvNombre1", SqlDbType.VarChar);
				prmChvNombre1.Value = System.DBNull.Value;

				SqlParameter prmChvNombre2 = cmd.Parameters.Add("@chvNombre2", SqlDbType.VarChar);
				prmChvNombre2.Value = System.DBNull.Value;

				SqlParameter prmChvNombre3 = cmd.Parameters.Add("@chvNombre3", SqlDbType.VarChar);
				prmChvNombre3.Value = System.DBNull.Value;

				SqlParameter prmChvApePaterno = cmd.Parameters.Add("@chvApePaterno", SqlDbType.VarChar);
				prmChvApePaterno.Value = System.DBNull.Value;

				SqlParameter prmChvApeMaterno = cmd.Parameters.Add("@chvApeMaterno", SqlDbType.VarChar);
				prmChvApeMaterno.Value = System.DBNull.Value;

				SqlParameter prmDateFechaNacimiento = cmd.Parameters.Add("@dateFechaNacimiento", SqlDbType.DateTime);
				prmDateFechaNacimiento.Value =System.DBNull.Value;

				SqlParameter prmChvTipoDocumento = cmd.Parameters.Add("@chvTipoDocumento", SqlDbType.VarChar);
				prmChvTipoDocumento.Value =System.DBNull.Value;

				SqlParameter prmChvNumeroDocumento = cmd.Parameters.Add("@chvNumeroDocumento", SqlDbType.VarChar);
				prmChvNumeroDocumento.Value =System.DBNull.Value;

				SqlParameter prmChvTipoFirma = cmd.Parameters.Add("@chvTipoFirma", SqlDbType.VarChar);
				prmChvTipoFirma.Value = "IND";

				SqlParameter prmChvTipoParticipe = cmd.Parameters.Add("@chvTipoParticipe", SqlDbType.VarChar);
				prmChvTipoParticipe.Value =System.DBNull.Value;

				SqlParameter prmChvFlagProspecto = cmd.Parameters.Add("@chvFlagProspecto", SqlDbType.VarChar);
				prmChvFlagProspecto.Value = "S";

				SqlParameter prmChvIdPais = cmd.Parameters.Add("@chvIdPais", SqlDbType.Int);
				prmChvIdPais.Value =System.DBNull.Value;

				SqlParameter prmChvCodEstadoCivil = cmd.Parameters.Add("@chvCodEstadoCivil", SqlDbType.VarChar);
				prmChvCodEstadoCivil.Value =System.DBNull.Value;

				SqlParameter prmChvCodProfesion = cmd.Parameters.Add("@chvCodProfesion", SqlDbType.VarChar);
				prmChvCodProfesion.Value =System.DBNull.Value;

				SqlParameter prmChvCodCompania = cmd.Parameters.Add("@chvCodCompania", SqlDbType.VarChar);
				prmChvCodCompania.Value =System.DBNull.Value;

				SqlParameter prmChvCodCategoriaCompañia = cmd.Parameters.Add("@chvCodCategoriaCompañia", SqlDbType.VarChar);
				prmChvCodCategoriaCompañia.Value =System.DBNull.Value;

				SqlParameter prmChvEstado = cmd.Parameters.Add("@chvEstado", SqlDbType.VarChar);
				prmChvEstado.Value = "ACT";

				SqlParameter prmChvFlagNoPublicidad = cmd.Parameters.Add("@chvFlagNoPublicidad", SqlDbType.VarChar);
				prmChvFlagNoPublicidad.Value = "N";

				SqlParameter prmChvFlagCorrespondenciaEMail = cmd.Parameters.Add("@chvFlagCorrespondenciaEMail", SqlDbType.VarChar);
				prmChvFlagCorrespondenciaEMail.Value = "N";

				SqlParameter prmChvFlagCorrespondenciaCorreo = cmd.Parameters.Add("@chvFlagCorrespondenciaCorreo", SqlDbType.VarChar);
				prmChvFlagCorrespondenciaCorreo.Value = "N";

				SqlParameter prmChvUsuarioCreacion = cmd.Parameters.Add("@chvUsuarioCreacion", SqlDbType.VarChar);
				prmChvUsuarioCreacion.Value = codigoUsuario;

				SqlParameter prmChvUsuarioModificacion = cmd.Parameters.Add("@chvUsuarioModificacion", SqlDbType.VarChar);
				prmChvUsuarioModificacion.Value = codigoUsuario;

				SqlParameter prmChvAreaModificacion = cmd.Parameters.Add("@chvAreaModificacion", SqlDbType.VarChar);
				prmChvAreaModificacion.Value = "OPE";

				SqlParameter prmChvFlagBloqueo = cmd.Parameters.Add("@chvFlagBloqueo", SqlDbType.VarChar);
				prmChvFlagBloqueo.Value = "N";

				SqlParameter prmChvFlagNivel1 = cmd.Parameters.Add("@chvFlagNivel1", SqlDbType.VarChar);
				prmChvFlagNivel1.Value = "N";

				SqlParameter prmChvFlagNivel2 = cmd.Parameters.Add("@chvFlagNivel2", SqlDbType.VarChar);
				prmChvFlagNivel2.Value = "N";

				SqlParameter prmChvFlagEliminado = cmd.Parameters.Add("@chvFlagEliminado", SqlDbType.VarChar);
				prmChvFlagEliminado.Value = "N";

				SqlParameter prmChvFlagAutogenerado = cmd.Parameters.Add("@chvFlagAutogenerado", SqlDbType.VarChar);
				prmChvFlagAutogenerado.Value = "S";

				SqlParameter prmChvEstadoAnterior = cmd.Parameters.Add("@chvEstadoAnterior", SqlDbType.VarChar);
				prmChvEstadoAnterior.Value = "ACT";

				SqlParameter prmChvContrasena = cmd.Parameters.Add("@chvContrasena", SqlDbType.VarChar);
				prmChvContrasena.Value = contrasenaCifrada;

				SqlParameter prmNumIdTablaAcceso = cmd.Parameters.Add("@numIdTablaAcceso", SqlDbType.Int);
				prmNumIdTablaAcceso.Direction = ParameterDirection.Output;

				SqlParameter prmChvFlagRelacionadoING = cmd.Parameters.Add("@chvFlagRelacionadoING", SqlDbType.VarChar);
				prmChvFlagRelacionadoING.Value = "N";

				SqlParameter prmSexo = cmd.Parameters.Add("@sexo", SqlDbType.VarChar);
				prmSexo.Value = "N";

				SqlParameter prmIdTitular = cmd.Parameters.Add("@idTitular", SqlDbType.Int);
				prmIdTitular.Value =System.DBNull.Value;

				SqlParameter prmFlagAsociado = cmd.Parameters.Add("@flagAsociado", SqlDbType.VarChar);
				prmFlagAsociado.Value = "N";

				SqlParameter prmCodigosMancomuno = cmd.Parameters.Add("@codigosMancomuno", SqlDbType.VarChar);
				prmCodigosMancomuno.Value =System.DBNull.Value;

				SqlParameter prmFlagVerOperacion = cmd.Parameters.Add("@flagVerOperacion", SqlDbType.VarChar);
				prmFlagVerOperacion.Value = "S";

				SqlParameter prmFlagAccesoWeb = cmd.Parameters.Add("@flagAccesoWeb", SqlDbType.VarChar);
				prmFlagAccesoWeb.Value = "S";

				SqlParameter prmFechaVencimientoContrasena = cmd.Parameters.Add("@fechaVencimientoContrasena", SqlDbType.DateTime);
				prmFechaVencimientoContrasena.Value =System.DBNull.Value;

				SqlParameter prmChvCargoCompania = cmd.Parameters.Add("@chvCargoCompania", SqlDbType.VarChar);
				prmChvCargoCompania.Value =System.DBNull.Value;

				SqlParameter prmChvFlagNoMedioTelefono = cmd.Parameters.Add("@chvFlagNoMedioTelefono", SqlDbType.VarChar);
				prmChvFlagNoMedioTelefono.Value = "N";

				SqlParameter prmChvFlagNoMedioAccesoWeb = cmd.Parameters.Add("@chvFlagNoMedioAccesoWeb", SqlDbType.VarChar);
				prmChvFlagNoMedioAccesoWeb.Value = "S";

				SqlParameter prmChvProfesionDesc = cmd.Parameters.Add("@chvProfesionDesc", SqlDbType.VarChar);
				prmChvProfesionDesc.Value =System.DBNull.Value;

				SqlParameter prmChvCompaniaDesc = cmd.Parameters.Add("@chvCompaniaDesc", SqlDbType.VarChar);
				prmChvCompaniaDesc.Value =System.DBNull.Value;

				SqlParameter prmChvCodigoAgenciaOrigen = cmd.Parameters.Add("@chvCodigoAgenciaOrigen", SqlDbType.VarChar);
				prmChvCodigoAgenciaOrigen.Value =System.DBNull.Value;

				SqlParameter prmChvTipoPersoneria = cmd.Parameters.Add("@chvTipoPersoneria", SqlDbType.VarChar);
				prmChvTipoPersoneria.Value =System.DBNull.Value;

				SqlParameter prmChvNivelCargo = cmd.Parameters.Add("@chvNivelCargo", SqlDbType.VarChar);
				prmChvNivelCargo.Value =System.DBNull.Value;

				SqlParameter prmChvGradoInstruccion = cmd.Parameters.Add("@chvGradoInstruccion", SqlDbType.VarChar);
				prmChvGradoInstruccion.Value =System.DBNull.Value;

				SqlParameter prmChvRangoSueldo = cmd.Parameters.Add("@chvRangoSueldo", SqlDbType.VarChar);
				prmChvRangoSueldo.Value =System.DBNull.Value;

				SqlParameter prmNumeroHijos = cmd.Parameters.Add("@numeroHijos", SqlDbType.Int);
				prmNumeroHijos.Value = 0;

				SqlParameter prmFlagCourier = cmd.Parameters.Add("@flagCourier", SqlDbType.VarChar);
				prmFlagCourier.Value = System.DBNull.Value;

				SqlParameter prmFlagOficinaING = cmd.Parameters.Add("@flagOficinaING", SqlDbType.VarChar);
				prmFlagOficinaING.Value = System.DBNull.Value;

				SqlParameter prmFlagCapacidadLegal = cmd.Parameters.Add("@flagCapacidadLegal", SqlDbType.VarChar);
				prmFlagCapacidadLegal.Value = System.DBNull.Value;

				SqlParameter prmSenasParticulares = cmd.Parameters.Add("@senasParticulares", SqlDbType.VarChar);
				prmSenasParticulares.Value =System.DBNull.Value;

				SqlParameter prmFlagSujetoRenta = cmd.Parameters.Add("@flagSujetoRenta", SqlDbType.VarChar);
				prmFlagSujetoRenta.Value = System.DBNull.Value;

				SqlParameter prmFlagMedioConsultas = cmd.Parameters.Add("@flagMedioConsultas", SqlDbType.VarChar);
				prmFlagMedioConsultas.Value = System.DBNull.Value;

				SqlParameter prmFlagMedioOperaciones = cmd.Parameters.Add("@flagMedioOperaciones", SqlDbType.VarChar);
				prmFlagMedioOperaciones.Value = System.DBNull.Value;

				SqlParameter prmChvIdPaisResidencia = cmd.Parameters.Add("@chvIdPaisResidencia", SqlDbType.Int);
				prmChvIdPaisResidencia.Value =System.DBNull.Value;

				SqlParameter prmChvIdCiudadLugarNacimiento = cmd.Parameters.Add("@chvIdCiudadLugarNacimiento", SqlDbType.Int);
				prmChvIdCiudadLugarNacimiento.Value =System.DBNull.Value;

				SqlParameter prmChvLugarNacimientoOtro = cmd.Parameters.Add("@chvLugarNacimientoOtro", SqlDbType.VarChar);
				prmChvLugarNacimientoOtro.Value =System.DBNull.Value;

				SqlParameter prmDateFechaCreacionEmpresa = cmd.Parameters.Add("@dateFechaCreacionEmpresa", SqlDbType.DateTime);
				prmDateFechaCreacionEmpresa.Value =System.DBNull.Value;

				SqlParameter prmDateFechaInicioOperaciones = cmd.Parameters.Add("@dateFechaInicioOperaciones", SqlDbType.DateTime);
				prmDateFechaInicioOperaciones.Value =System.DBNull.Value;

				SqlParameter prmFlagCuotasGarantia = cmd.Parameters.Add("@flagCuotasGarantia", SqlDbType.VarChar);
				prmFlagCuotasGarantia.Value = System.DBNull.Value;

				SqlParameter prmFlagExcluidoRetencion = cmd.Parameters.Add("@flagExcluidoRetencion", SqlDbType.VarChar);
				prmFlagExcluidoRetencion.Value = System.DBNull.Value;

                //Inicio  Ot 6891
                SqlParameter prmchvSectorEconomicoDesc = cmd.Parameters.Add("@chvSectorEconomicoDesc", SqlDbType.VarChar);
                prmchvSectorEconomicoDesc.Value = System.DBNull.Value;
                //Fin Ot 6891

				//OT11436 INI 
				SqlParameter prmchvCodigoCanalCliente = cmd.Parameters.Add("@chvCodigoCanalCliente", SqlDbType.VarChar);
				prmchvCodigoCanalCliente.Value = "WM";

				SqlParameter prmchvFlagCertificadoRetencion = cmd.Parameters.Add("@chvFlagCertificadoRetencion", SqlDbType.VarChar);
				prmchvFlagCertificadoRetencion.Value = "S";
				//OT11436 FIN

				SqlParameter prmIdentificador = cmd.Parameters.Add("@identificador", SqlDbType.Int);
				prmIdentificador.Direction = ParameterDirection.ReturnValue;

				cmd.ExecuteNonQuery();

				int idParticipe = Convert.ToInt32(cmd.Parameters["@identificador"].Value.ToString());

				DataTable dtPrmGrabarDatosParticipe = new DataTable();
				dtPrmGrabarDatosParticipe.Columns.Add("VALOR_PARAMETRO");

				DataRow drPrmGrabarDatosParticipe1 = dtPrmGrabarDatosParticipe.NewRow();
				drPrmGrabarDatosParticipe1["VALOR_PARAMETRO"]=idParticipe; //Id de tabla PARTICIPE
				dtPrmGrabarDatosParticipe.Rows.Add(drPrmGrabarDatosParticipe1);

				DataRow drPrmGrabarDatosParticipe2 = dtPrmGrabarDatosParticipe.NewRow();
				drPrmGrabarDatosParticipe2["VALOR_PARAMETRO"] = prmNumIdTablaAcceso.Value; //Id de tabla PARTICIPE_ACCESO
				dtPrmGrabarDatosParticipe.Rows.Add(drPrmGrabarDatosParticipe2);

				return dtPrmGrabarDatosParticipe;

			}
			catch(Exception e)
			{
				return null;
			}
			finally
			{
				cmd.Dispose();

			}
		}

		public void registrarCuentaParticipacionXParticipe(string idParticipe, string codigoParticipe, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
		{ 
			SqlCommand cmd = new SqlCommand("dbo.FOND_INS_CUENTA_PARTICIPACION_XPARTICIPE", cn);
			cmd.Transaction = trans;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 50000;

			try
			{
				SqlParameter prmCodigo = cmd.Parameters.Add("@codigo", SqlDbType.VarChar);
				prmCodigo.Value = codigoParticipe + "-01"; 

				SqlParameter prmEtiqueta = cmd.Parameters.Add("@etiqueta", SqlDbType.VarChar);
				prmEtiqueta.Value = "CUENTA DE PARTICIPACIÓN";

				SqlParameter prmFlagPredeterminado = cmd.Parameters.Add("@flagPredeterminado", SqlDbType.VarChar);
				prmFlagPredeterminado.Value = "S";

				SqlParameter prmIdParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.VarChar);
				prmIdParticipe.Value = idParticipe;

				SqlParameter prmUsuarioCreacion = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
				prmUsuarioCreacion.Value = codigoUsuario;

				SqlParameter prmUsuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
				prmUsuarioModificacion.Value = codigoUsuario;

				SqlParameter prmAreaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
				prmAreaModificacion.Value = "OPE";  

				SqlParameter prmPorcentajeParticipacion = cmd.Parameters.Add("@porcentajeParticipacion", SqlDbType.VarChar);
				prmPorcentajeParticipacion.Value =System.DBNull.Value;

				SqlParameter prmIdParticipeAsociado = cmd.Parameters.Add("@idParticipeAsociado", SqlDbType.VarChar);
				prmIdParticipeAsociado.Value =System.DBNull.Value;

				SqlParameter prmFlagCuotasGarantia = cmd.Parameters.Add("@flagCuotasGarantia", SqlDbType.VarChar);
				prmFlagCuotasGarantia.Value =System.DBNull.Value;

				SqlParameter prmCodigoPlan = cmd.Parameters.Add("@codigoPlan", SqlDbType.VarChar);
				prmCodigoPlan.Value =System.DBNull.Value;

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

		public DataTable ObtenerCodigoParticipe(int identificador, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_PARTICIPE_CODIGO", cn);
			cmd.Transaction = trans;
			cmd.CommandType = CommandType.StoredProcedure;

			try
			{
				SqlParameter prmIdentificador = cmd.Parameters.Add("@identificador", SqlDbType.Int);
				prmIdentificador.Value = identificador;

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable("CODIGO");
				da.Fill(dt);
				return dt;
			}
			catch(Exception e)
			{ 
				return null;
			}
			finally
			{
				cmd.Dispose();
			}
		}

		public DataTable ObtenerUsuario(string codigoDominio, SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FMAC_OBT_USUARIO", cn);
			cmd.Transaction = trans;
			cmd.CommandType = CommandType.StoredProcedure;

			try
			{
				SqlParameter prmCodigoDominio = cmd.Parameters.Add("@codigoDominio", SqlDbType.VarChar);
				prmCodigoDominio.Value = codigoDominio;

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable("FONDO");
				da.Fill(dt);
				return dt;
			}
			catch
			{ 
				return null;
			}
			finally
			{
				cmd.Dispose();
			}
		}

		public String ObtenerClaveEncriptada(SqlConnection cn, SqlTransaction trans)
		{
			SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_LLAVE_3DES", cn);
			cmd.Transaction = trans;
			cmd.CommandType = CommandType.StoredProcedure;

			try
			{
				SqlParameter prmClaveEncriptada = cmd.Parameters.Add("@VAL", SqlDbType.VarChar, 200);
				prmClaveEncriptada.Direction = ParameterDirection.Output;

				cmd.ExecuteNonQuery();

				return prmClaveEncriptada.Value.ToString();
			}
			catch(Exception e)
			{
				return null;
			}
			finally
			{
				cmd.Dispose();
			}
		}

        public void InsertarLoteGeneracion(string codigo, int primerCuc, int ultimoCuc, string usuario, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_LOTE_GENERACION", cn);
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;
            
            try
            {
                SqlParameter prmCodigo = cmd.Parameters.Add("@codigo", SqlDbType.VarChar);
                prmCodigo.Value = codigo;

                SqlParameter prmCucInicio = cmd.Parameters.Add("@cucInicio", SqlDbType.Decimal);
                prmCucInicio.Value = primerCuc;

                SqlParameter prmCucFin = cmd.Parameters.Add("@cucFin", SqlDbType.Decimal);
                prmCucFin.Value = ultimoCuc;

                SqlParameter prmUsuarioCreacion = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
                prmUsuarioCreacion.Value = usuario;
                
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
