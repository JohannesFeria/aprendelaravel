/*
 * Fecha de Modificación : 27/04/2017
 * Modificado por        : Anthony Joaquin
 * Nro. Orden de Trabajo : 10217
 * Descripción del cambio: Creación.
 * */
using System;
using System.Data;
using Procesos.TD;
using Procesos.DA;
using System.Data.SqlClient;
using INGFondos.Constants;
using INGFondos.Data;
using System.Configuration;

namespace Procesos.BL
{
	/// <summary>
    /// Descripción breve de DocumentoIdentidadBL.
	/// </summary>
	public class DocumentoIdentidadBO
	{
        public DocumentoIdentidadBO()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}	



        public DataTable ListarTipoDocumento()
        {
            DocumentoIdentidadDA da = new DocumentoIdentidadDA();
            return da.ListarTipoDocumento(Constants.ConstantesING.TABLA_TIPO_DOCUMENTO);
        }


        public DataTable ListarNombreParticipe(string tipoDocumento, string numeroDocumento)
        {
            DocumentoIdentidadDA da = new DocumentoIdentidadDA();
            return da.ListarNombreParticipe(tipoDocumento, numeroDocumento);      
        }

				public void grabarDocumento(DocumentoIdentidad.DOCUMENTO_IDENTIDADRow drDocumentoIdentidad)
				{
							ConnectionManager cm = new ConnectionManager();

							string server = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorOperaciones];
							string database = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosOperaciones];

							SqlConnection cnOperaciones = cm.GetTrustedConnection(server, database);
							cnOperaciones.Open();
							SqlTransaction transOperaciones = cnOperaciones.BeginTransaction();

							server = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorComercial];
							database = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosComercial];

							SqlConnection cnComercial = cm.GetTrustedConnection(server, database);
							cnComercial.Open();
							SqlTransaction transComercial = cnComercial.BeginTransaction();

							server = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorCONASEV];
							database = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosCONASEV];

							SqlConnection cnConasev = cm.GetTrustedConnection(server, database);
							cnConasev.Open();
							SqlTransaction transConasev = cnConasev.BeginTransaction();

							server = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorLavadoActivos];
							database = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosLavadoActivos];

							SqlConnection cnLavadoActivos = cm.GetTrustedConnection(server, database);
							cnLavadoActivos.Open();
							SqlTransaction transLavadoActivos = cnLavadoActivos.BeginTransaction();

							server = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.ServidorTributacion];
							database = ConfigurationSettings.AppSettings[INGFondos.Constants.Conexiones.BaseDeDatosTributacion];

							SqlConnection cnTributacion = cm.GetTrustedConnection(server, database);
							cnTributacion.Open();
							SqlTransaction transTributacion = cnTributacion.BeginTransaction();

							DocumentoIdentidadDA documentoIdentidadDA = new DocumentoIdentidadDA();
							
							try {
								documentoIdentidadDA.ActualizarDocumentoParticipeAdmCuentas(drDocumentoIdentidad, cnOperaciones, transOperaciones);
								documentoIdentidadDA.ActualizarDocumentoParticipeComercial(drDocumentoIdentidad, cnComercial, transComercial);
								documentoIdentidadDA.ActualizarDocumentoParticipeConasev(drDocumentoIdentidad, cnConasev, transConasev);
								documentoIdentidadDA.ActualizarDocumentoParticipeDeteccionLavadoActivos(drDocumentoIdentidad, cnLavadoActivos, transLavadoActivos);
								documentoIdentidadDA.ActualizarDocumentoParticipeTributacion(drDocumentoIdentidad, cnTributacion, transTributacion);

								transOperaciones.Commit();
								transComercial.Commit();
								transConasev.Commit();
								transLavadoActivos.Commit();
								transTributacion.Commit();
							}
							catch(Exception e){
								transOperaciones.Rollback();
								transComercial.Rollback();
								transConasev.Rollback();
								transLavadoActivos.Rollback();
								transTributacion.Rollback();

								throw e;
							}
							finally
							{
								transOperaciones.Dispose();
								cnOperaciones.Close();
								transComercial.Dispose();
								cnComercial.Close();
								transConasev.Dispose();
								cnConasev.Close();
								transLavadoActivos.Dispose();
								cnLavadoActivos.Close();
								transTributacion.Dispose();
								cnTributacion.Close();
							}
				}
	}
}
