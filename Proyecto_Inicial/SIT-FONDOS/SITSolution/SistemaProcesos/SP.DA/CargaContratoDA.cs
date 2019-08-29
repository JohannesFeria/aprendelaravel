/*
 * Fecha de Modificación: 05/11/2012
 * Modificado por: Davis Rixi
 * Numero de OT: 5012
 * Descripción del cambio: Creación
 * */
/*
 * Fecha de Modificación: 16/11/2012
 * Modificado por: Davis Rixi
 * Numero de OT: 5012
 * Descripción del cambio: Se retiró obligatoriedad de los campos profesion, cargo y compañia
 * */
/*
 * Fecha de Modificación: 23/11/2012
 * Modificado por: Robert Castillo
 * Numero de OT: 5012
 * Descripción del cambio: Se modificó el método registrarFondos y registrarParticipe
 * */
/*
 * Fecha de Modificación: 28/11/2012
 * Modificado por: Robert Castillo
 * Numero de OT: 5117
 * Descripción del cambio: Se cambia namespace
 * */
/*
 * Fecha de Modificación: 01/07/2013
 * Modificado por: Robert Castillo
 * Numero de OT: 5526
 * Descripción del cambio: Se modifican los métodos registrarProspectos y registrarParticipeXparticipe. 
 *						   Además se crea el método ObtenerClaveEncriptada.
 * */
/*
 * Fecha de Modificación: 10/07/2013
 * Modificado por: Robert Castillo
 * Numero de OT: 5526
 * Descripción del cambio: En el método registrarParticipeXparticipe se actualizan los valores de los
 *						   parámetros que se envían al stored procedure FOND_INS_PERSONA_ASOCIADA_XPARTICIPE.
 * */
/*
 * Fecha de Modificación: 16/09/2014
 * Modificado por: Leslie Valerio
 * proy : Vida Ahorro
 * Descripción del cambio: Se agrego nuevos campos en la plantilla Contratos.xls; para los casos de VidaAHorro
 *                        - Se agregaron nuevos metodos  como:
 *                        - registrarBoletin
 *                        - registrarDireccion
 *                        - registrarTelefono
 *                        - registrarCorreo
 *                        para que se depositen en la base datos los nuevos registros de los campos agregados en la plantilla.
 *                        - Se crea un nuevo metodo RegistrarCuentaParticipacion_x_Participe para insertar los registros nuevos hacia cuenta Participacion.
 *                          
 * */
using System;
using System.Data;
using System.Data.SqlClient;

using INGFondos.Data;
using Procesos.TD;

namespace Procesos.DA
{
    public class CargaContratoDA : INGFondos.Data.DA
    {
        public CargaContratoDA() : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones) { }

        //Obtener prospectos vacios para registro de contratos
        public DataTable ObtenerListaProspectosVacios()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_PROSPECTO_CONTRATO", cn);
            cmd.CommandTimeout = 4000000;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("LISTA_PROSPECTOS");
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Obtiene el registro de persona posiblemente asociado al documento del nuevo partícipe
        /// </summary>
        /// <param name="participe">Contiene la información del partícipe, para saber su tipo y número de documento</param>
        /// <returns></returns>
        public DataTable ObtenerPersona(ParticipeTD.ParticipeRow participe, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_OBT_PERSONA", cn);
            cmd.Transaction = trans;
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmTipoDocumento = cmd.Parameters.Add("@tipoDocumento", SqlDbType.VarChar);
            prmTipoDocumento.Value = participe.TIPO_DOCUMENTO;

            SqlParameter prmNumeroDocumento = cmd.Parameters.Add("@numeroDocumento", SqlDbType.VarChar);
            prmNumeroDocumento.Value = participe.NUMERO_DOCUMENTO;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("PERSONA");
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Registra la información de la persona
        /// </summary>
        /// <param name="participe">Datos del partícipe</param>
        /// <param name="cn">Conexión a la base de datos</param>
        /// <param name="trans">Transacción de Base de datos para controlar posibles errores</param>
        public void registrarPersona(ParticipeTD.ParticipeRow participe, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_INS_PERSONA", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 4000000;

            string dateString = "1/1/1753 8:30:52 AM";
            DateTime date1 = DateTime.Parse(dateString,
                                      System.Globalization.CultureInfo.InvariantCulture); 


            SqlParameter prmRazonSocial = cmd.Parameters.Add("@razonSocial", SqlDbType.VarChar);
            prmRazonSocial.Value = DBNull.Value;

            SqlParameter prmSecEco = cmd.Parameters.Add("@codigoSectorEconomico", SqlDbType.VarChar);
            prmSecEco.Value = DBNull.Value;

            SqlParameter prmNombre1 = cmd.Parameters.Add("@nombre1", SqlDbType.VarChar);
            prmNombre1.Value = participe.NOMBRE1;

            SqlParameter prmNombre2 = cmd.Parameters.Add("@nombre2", SqlDbType.VarChar);
            prmNombre2.Value = participe.NOMBRE2;

            SqlParameter prmNombre3 = cmd.Parameters.Add("@nombre3", SqlDbType.VarChar);
            prmNombre3.Value = participe.NOMBRE3;

            SqlParameter prmApellidoPaterno = cmd.Parameters.Add("@apePaterno", SqlDbType.VarChar);
            prmApellidoPaterno.Value = participe.APELLIDO_PATERNO;

            SqlParameter prmApellidoMaterno = cmd.Parameters.Add("@apeMaterno", SqlDbType.VarChar);
            prmApellidoMaterno.Value = participe.APELLIDO_MATERNO;

            SqlParameter prmSexo = cmd.Parameters.Add("@sexo", SqlDbType.VarChar);
            prmSexo.Value = participe.SEXO;

            SqlParameter prmFechaNacimiento = cmd.Parameters.Add("@fechaNacimiento", SqlDbType.DateTime);
           // prmFechaNacimiento.Value = participe.FECHA_NACIMIENTO;
            //proy vida Ahorro Ini
            if (participe.FECHA_NACIMIENTO < date1) { prmFechaNacimiento.Value = DBNull.Value; }
            else prmFechaNacimiento.Value = participe.FECHA_NACIMIENTO;
            //Proy Vida Ahorro Final

          


            SqlParameter prmTipoDocumento = cmd.Parameters.Add("@tipoDocumento", SqlDbType.VarChar);
            prmTipoDocumento.Value = participe.TIPO_DOCUMENTO;

            SqlParameter prmNumeroDocumento = cmd.Parameters.Add("@numeroDocumento", SqlDbType.VarChar);
            prmNumeroDocumento.Value = participe.NUMERO_DOCUMENTO;

            SqlParameter prmTipoParticipe = cmd.Parameters.Add("@tipoParticipe", SqlDbType.VarChar);
            prmTipoParticipe.Value = participe.TIPO_PARTICIPE;

            SqlParameter prmIdPais = cmd.Parameters.Add("@idPais", SqlDbType.Int);
            prmIdPais.Value = participe.NACIONALIDAD;

            SqlParameter prmEstadoCivil = cmd.Parameters.Add("@codEstadoCivil", SqlDbType.VarChar);
            prmEstadoCivil.Value = participe.ESTADO_CIVIL;

            SqlParameter prmCodCatCompania = cmd.Parameters.Add("@codCategoriaCompañia", SqlDbType.VarChar);
            prmCodCatCompania.Value = DBNull.Value;

            SqlParameter prmCargoCompania = cmd.Parameters.Add("@cargoCompania", SqlDbType.VarChar);
            prmCargoCompania.Value = participe.CARGO;

            SqlParameter prmProfesionDesc = cmd.Parameters.Add("@profesionDesc", SqlDbType.VarChar);
            prmProfesionDesc.Value = participe.PROFESION;

            SqlParameter prmCompaniaDesc = cmd.Parameters.Add("@companiaDesc", SqlDbType.VarChar);
            prmCompaniaDesc.Value = participe.COMPANIA;

            SqlParameter prmTipoPersoneria = cmd.Parameters.Add("@tipoPersoneria", SqlDbType.VarChar);
            if (participe.TIPO_PERSONERIA.Equals(""))
                prmTipoPersoneria.Value = DBNull.Value;
            else
                prmTipoPersoneria.Value = participe.TIPO_PERSONERIA;

            SqlParameter prmNivelCargo = cmd.Parameters.Add("@nivelCargo", SqlDbType.VarChar);
            prmNivelCargo.Value = participe.NIVEL_CARGO;

            SqlParameter prmGradoInstruccion = cmd.Parameters.Add("@gradoInstruccion", SqlDbType.VarChar);
            prmGradoInstruccion.Value = participe.GRADO_INSTRUCCION;

            SqlParameter prmRangoSueldo = cmd.Parameters.Add("@rangoSueldo", SqlDbType.VarChar);
            prmRangoSueldo.Value = DBNull.Value;

            SqlParameter prmNumeroHijos = cmd.Parameters.Add("@numeroHijos", SqlDbType.Int);
            prmNumeroHijos.Value = participe.NUMERO_HIJOS;

            SqlParameter prmFlagSujetoRenta = cmd.Parameters.Add("@flagSujetoRenta", SqlDbType.VarChar);
            prmFlagSujetoRenta.Value = "S";
            // MODIF POR MODIFICACIONES
            SqlParameter prmSectorEconomicoDesc = cmd.Parameters.Add("@chvSectorEconomicoDesc", SqlDbType.VarChar);
            prmSectorEconomicoDesc.Value = "";

            SqlParameter prmchvCodProfesion = cmd.Parameters.Add("@chvCodProfesion ", SqlDbType.VarChar);
            prmchvCodProfesion.Value = "";
            ///
            //Auditoria
            SqlParameter prmUsuarioModificacion = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
            prmUsuarioModificacion.Value = participe.USUARIO;

            SqlParameter prmAreaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            prmAreaModificacion.Value = "";

            cmd.Transaction = trans;
            try
            {
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

        /// <summary>
        /// Actualiza la información de la persona
        /// </summary>
        /// <param name="participe">Datos del partícipe</param>
        /// <param name="cn">Conexión a la base de datos</param>
        /// <param name="trans">Transacción de Base de datos para controlar posibles errores</param>
        public void ActualizarPersona(ParticipeTD.ParticipeRow participe, SqlConnection cn, SqlTransaction trans, string isProspecto)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_ACT_PERSONA", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 100000;
            string dateString = "1/1/1753 8:30:52 AM";
            DateTime date1 = DateTime.Parse(dateString,
                                      System.Globalization.CultureInfo.InvariantCulture); 


            SqlParameter prmRazonSocial = cmd.Parameters.Add("@razonSocial", SqlDbType.VarChar);
            prmRazonSocial.Value = DBNull.Value;

            SqlParameter prmSecEco = cmd.Parameters.Add("@codigoSectorEconomico", SqlDbType.VarChar);
            prmSecEco.Value = DBNull.Value;

            SqlParameter prmNombre1 = cmd.Parameters.Add("@nombre1", SqlDbType.VarChar);
            prmNombre1.Value = participe.NOMBRE1;

            SqlParameter prmNombre2 = cmd.Parameters.Add("@nombre2", SqlDbType.VarChar);
            prmNombre2.Value = participe.NOMBRE2;

            SqlParameter prmNombre3 = cmd.Parameters.Add("@nombre3", SqlDbType.VarChar);
            prmNombre3.Value = participe.NOMBRE3;

            SqlParameter prmApellidoPaterno = cmd.Parameters.Add("@apePaterno", SqlDbType.VarChar);
            prmApellidoPaterno.Value = participe.APELLIDO_PATERNO;

            SqlParameter prmApellidoMaterno = cmd.Parameters.Add("@apeMaterno", SqlDbType.VarChar);
            prmApellidoMaterno.Value = participe.APELLIDO_MATERNO;

            SqlParameter prmSexo = cmd.Parameters.Add("@sexo", SqlDbType.VarChar);
            prmSexo.Value = participe.SEXO;

            SqlParameter prmFechaNacimiento = cmd.Parameters.Add("@fechaNacimiento", SqlDbType.DateTime);
           // prmFechaNacimiento.Value = participe.FECHA_NACIMIENTO;
            //proy vida Ahorro Ini
            if (participe.FECHA_NACIMIENTO < date1) { prmFechaNacimiento.Value = DBNull.Value; }
            else prmFechaNacimiento.Value = participe.FECHA_NACIMIENTO;
            //Proy Vida Ahorro Final

            SqlParameter prmTipoDocumento = cmd.Parameters.Add("@tipoDocumento", SqlDbType.VarChar);
            prmTipoDocumento.Value = participe.TIPO_DOCUMENTO;

            SqlParameter prmNumeroDocumento = cmd.Parameters.Add("@numeroDocumento", SqlDbType.VarChar);
            prmNumeroDocumento.Value = participe.NUMERO_DOCUMENTO;

            SqlParameter prmTipoParticipe = cmd.Parameters.Add("@tipoParticipe", SqlDbType.VarChar);
            prmTipoParticipe.Value = participe.TIPO_PARTICIPE;

            SqlParameter prmIdPais = cmd.Parameters.Add("@idPais", SqlDbType.Int);
            prmIdPais.Value = participe.NACIONALIDAD;

            SqlParameter prmEstadoCivil = cmd.Parameters.Add("@codEstadoCivil", SqlDbType.VarChar);
            prmEstadoCivil.Value = participe.ESTADO_CIVIL;

            SqlParameter prmCodCatCompania = cmd.Parameters.Add("@codCategoriaCompañia", SqlDbType.VarChar);
            prmCodCatCompania.Value = DBNull.Value;

            SqlParameter prmCargoCompania = cmd.Parameters.Add("@cargoCompania", SqlDbType.VarChar);
            prmCargoCompania.Value = participe.CARGO;

            SqlParameter prmProfesionDesc = cmd.Parameters.Add("@profesionDesc", SqlDbType.VarChar);
            prmProfesionDesc.Value = participe.PROFESION;

            SqlParameter prmCompaniaDesc = cmd.Parameters.Add("@companiaDesc", SqlDbType.VarChar);
            prmCompaniaDesc.Value = participe.COMPANIA;

            SqlParameter prmTipoPersoneria = cmd.Parameters.Add("@tipoPersoneria", SqlDbType.VarChar);
            if (participe.TIPO_PERSONERIA.Equals(""))
                prmTipoPersoneria.Value = DBNull.Value;
            else
                prmTipoPersoneria.Value = participe.TIPO_PERSONERIA;

            SqlParameter prmNivelCargo = cmd.Parameters.Add("@nivelCargo", SqlDbType.VarChar);
            prmNivelCargo.Value = participe.NIVEL_CARGO;

            SqlParameter prmGradoInstruccion = cmd.Parameters.Add("@gradoInstruccion", SqlDbType.VarChar);
            prmGradoInstruccion.Value = participe.GRADO_INSTRUCCION;

            SqlParameter prmRangoSueldo = cmd.Parameters.Add("@rangoSueldo", SqlDbType.VarChar);
            prmRangoSueldo.Value = DBNull.Value;

            SqlParameter prmNumeroHijos = cmd.Parameters.Add("@numeroHijos", SqlDbType.Int);
            prmNumeroHijos.Value = participe.NUMERO_HIJOS;

            SqlParameter prmFlagSujetoRenta = cmd.Parameters.Add("@flagSujetoRenta", SqlDbType.VarChar);
            prmFlagSujetoRenta.Value = "S";

            //Auditoria
            SqlParameter prmUsuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            prmUsuarioModificacion.Value = participe.USUARIO;

            SqlParameter prmAreaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            prmAreaModificacion.Value = "";

            SqlParameter prmTipoPersona = cmd.Parameters.Add("@tipoParticipeActualizacion", SqlDbType.VarChar);
            prmTipoPersona.Value = participe.TIPO_PERSONA;

            SqlParameter prmIsProspecto = cmd.Parameters.Add("@prospecto", SqlDbType.VarChar);
            prmIsProspecto.Value = isProspecto;
            // modificaion
            SqlParameter prmchvSectorEconomicoDesc = cmd.Parameters.Add("@chvSectorEconomicoDesc", SqlDbType.VarChar);
            prmchvSectorEconomicoDesc.Value = "";


            SqlParameter prmchvCodigoProfesion = cmd.Parameters.Add("@chvCodigoProfesion", SqlDbType.VarChar);
            prmchvCodigoProfesion.Value = "";

            cmd.Transaction = trans;
            try
            {
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

        public void ActualizarParticipeXDocumento(ParticipeTD.ParticipeRow participe, SqlConnection cn, SqlTransaction trans, string isProspecto)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_ACT_PARTICIPE_X_DOCUMENTO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 400000;

            string dateString = "1/1/1753 8:30:52 AM";
            DateTime date1 = DateTime.Parse(dateString,
                                      System.Globalization.CultureInfo.InvariantCulture); 

            SqlParameter prmRazonSocial = cmd.Parameters.Add("@razonSocial", SqlDbType.VarChar);
            prmRazonSocial.Value = DBNull.Value;

            SqlParameter prmSecEco = cmd.Parameters.Add("@codigoSectorEconomico", SqlDbType.VarChar);
            prmSecEco.Value = DBNull.Value;

            SqlParameter prmNombre1 = cmd.Parameters.Add("@nombre1", SqlDbType.VarChar);
            prmNombre1.Value = participe.NOMBRE1;

            SqlParameter prmNombre2 = cmd.Parameters.Add("@nombre2", SqlDbType.VarChar);
            prmNombre2.Value = participe.NOMBRE2;

            SqlParameter prmNombre3 = cmd.Parameters.Add("@nombre3", SqlDbType.VarChar);
            prmNombre3.Value = participe.NOMBRE3;

            SqlParameter prmApellidoPaterno = cmd.Parameters.Add("@apePaterno", SqlDbType.VarChar);
            prmApellidoPaterno.Value = participe.APELLIDO_PATERNO;

            SqlParameter prmApellidoMaterno = cmd.Parameters.Add("@apeMaterno", SqlDbType.VarChar);
            prmApellidoMaterno.Value = participe.APELLIDO_MATERNO;

            SqlParameter prmSexo = cmd.Parameters.Add("@sexo", SqlDbType.VarChar);
            prmSexo.Value = participe.SEXO;

            SqlParameter prmFechaNacimiento = cmd.Parameters.Add("@fechaNacimiento", SqlDbType.DateTime);
            //prmFechaNacimiento.Value = participe.FECHA_NACIMIENTO;
            //proy vida Ahorro Ini
            if (participe.FECHA_NACIMIENTO < date1) { prmFechaNacimiento.Value = DBNull.Value; }
            else prmFechaNacimiento.Value = participe.FECHA_NACIMIENTO;
            //Proy Vida Ahorro Final
            SqlParameter prmTipoDocumento = cmd.Parameters.Add("@tipoDocumento", SqlDbType.VarChar);
            prmTipoDocumento.Value = participe.TIPO_DOCUMENTO;

            SqlParameter prmNumeroDocumento = cmd.Parameters.Add("@numeroDocumento", SqlDbType.VarChar);
            prmNumeroDocumento.Value = participe.NUMERO_DOCUMENTO;

            SqlParameter prmTipoParticipe = cmd.Parameters.Add("@tipoParticipe", SqlDbType.VarChar);
            prmTipoParticipe.Value = participe.TIPO_PARTICIPE;

            SqlParameter prmIdPais = cmd.Parameters.Add("@idPais", SqlDbType.Int);
            prmIdPais.Value = participe.NACIONALIDAD;

            SqlParameter prmEstadoCivil = cmd.Parameters.Add("@codEstadoCivil", SqlDbType.VarChar);
            prmEstadoCivil.Value = participe.ESTADO_CIVIL;

            SqlParameter prmCodCatCompania = cmd.Parameters.Add("@codCategoriaCompañia", SqlDbType.VarChar);
            prmCodCatCompania.Value = DBNull.Value;

            SqlParameter prmCargoCompania = cmd.Parameters.Add("@cargoCompania", SqlDbType.VarChar);
            prmCargoCompania.Value = participe.CARGO;

            SqlParameter prmProfesionDesc = cmd.Parameters.Add("@profesionDesc", SqlDbType.VarChar);
            prmProfesionDesc.Value = participe.PROFESION;

            SqlParameter prmCompaniaDesc = cmd.Parameters.Add("@companiaDesc", SqlDbType.VarChar);
            prmCompaniaDesc.Value = participe.COMPANIA;

            SqlParameter prmTipoPersoneria = cmd.Parameters.Add("@tipoPersoneria", SqlDbType.VarChar);
            if (participe.TIPO_PERSONERIA.Equals(""))
                prmTipoPersoneria.Value = DBNull.Value;
            else
                prmTipoPersoneria.Value = participe.TIPO_PERSONERIA;

            SqlParameter prmNivelCargo = cmd.Parameters.Add("@nivelCargo", SqlDbType.VarChar);
            prmNivelCargo.Value = participe.NIVEL_CARGO;

            SqlParameter prmGradoInstruccion = cmd.Parameters.Add("@gradoInstruccion", SqlDbType.VarChar);
            prmGradoInstruccion.Value = participe.GRADO_INSTRUCCION;

            SqlParameter prmRangoSueldo = cmd.Parameters.Add("@rangoSueldo", SqlDbType.VarChar);
            prmRangoSueldo.Value = DBNull.Value;

            SqlParameter prmNumeroHijos = cmd.Parameters.Add("@numeroHijos", SqlDbType.Int);
            prmNumeroHijos.Value = participe.NUMERO_HIJOS;

            SqlParameter prmFlagSujetoRenta = cmd.Parameters.Add("@flagSujetoRenta", SqlDbType.VarChar);
            prmFlagSujetoRenta.Value = "S";

            //Auditoria
            SqlParameter prmUsuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            prmUsuarioModificacion.Value = participe.USUARIO;

            SqlParameter prmAreaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            prmAreaModificacion.Value = "";

            SqlParameter prmTipoPersona = cmd.Parameters.Add("@tipoParticipeActualizacion", SqlDbType.VarChar);
            prmTipoPersona.Value = participe.TIPO_PERSONA;

            SqlParameter prmIsProspecto = cmd.Parameters.Add("@prospecto", SqlDbType.VarChar);
            prmIsProspecto.Value = isProspecto;
            //modificaion
            //@SECTOR_ECONOMICO_DESC

            SqlParameter prmSECTOR_ECONOMICO_DESC = cmd.Parameters.Add("@SECTOR_ECONOMICO_DESC", SqlDbType.VarChar);
            prmSECTOR_ECONOMICO_DESC.Value = isProspecto;

            SqlParameter prmCODIGO_PROFESION = cmd.Parameters.Add("@CODIGO_PROFESION", SqlDbType.VarChar);
            prmCODIGO_PROFESION.Value = isProspecto;
            //

            cmd.Transaction = trans;
            try
            {
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

        //Obtener lista de fondos
        public DataTable ObtenerListaFondos()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.INGF_LIS_FONDO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("LISTA_FONDO");
            da.Fill(dt);
            return dt;
        }

        public DataTable ObtenerListaPaises()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_PAIS", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("LISTA_PAIS");
            da.Fill(dt);
            return dt;
        }
        //PROY  VIDA AHORRO
        public DataTable ObtenerListaDistritos()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FOND_LIS_DISTRITO_TOTAL", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("LISTA_DISTRITOS");
            da.Fill(dt);
            return dt;
        }

        //Registrar los datos del partícipe Natural
        public void RegistrarCuentaParticipacion_x_Participe(CuentaParticipacionTD.CUENTA_PARTICIPACIONRow cuentaParticipacion, string prmcuc, string usuario, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_EJE_CUENTA_PARTICIPACION_X_PARTICIPE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 100000;

            SqlParameter prmCodigo = cmd.Parameters.Add("@codigo", SqlDbType.VarChar);
            prmCodigo.Value = cuentaParticipacion.CODIGO;

            SqlParameter prmetiqueta = cmd.Parameters.Add("@etiqueta", SqlDbType.VarChar);
            prmetiqueta.Value = cuentaParticipacion.ETIQUETA;//"VIDA AHORRO";

            SqlParameter prmFlagPredeterminador = cmd.Parameters.Add("@flagPredeterminado", SqlDbType.VarChar);
            prmFlagPredeterminador.Value = "N";

            SqlParameter prmusuarioCreacion = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
            prmusuarioCreacion.Value = usuario;

            SqlParameter prmusuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            prmusuarioModificacion.Value = usuario;

            SqlParameter prmareaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            prmareaModificacion.Value = "OPE";

            SqlParameter prmporcentajeParticipacion = cmd.Parameters.Add("@porcentajeParticipacion", SqlDbType.VarChar);
            prmporcentajeParticipacion.Value = DBNull.Value;

            SqlParameter prmidParticipeAsociado = cmd.Parameters.Add("@idParticipeAsociado", SqlDbType.VarChar);
            prmidParticipeAsociado.Value = DBNull.Value;

            SqlParameter prmflagCuotasGarantia = cmd.Parameters.Add("@flagCuotasGarantia", SqlDbType.VarChar);
            prmflagCuotasGarantia.Value = DBNull.Value;

            SqlParameter prmcodigoPlan = cmd.Parameters.Add("@codigoPlan", SqlDbType.VarChar);
            prmcodigoPlan.Value = cuentaParticipacion.CODIGO_PLAN;

            cmd.Transaction = trans;

            try
            {

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
       
        public void registrarParticipe(ParticipeTD.ParticipeRow participe, ParticipeTD.FondoRow fondo, ParticipeTD.DocumentoRow documento, ParticipeTD.Correo_EletronicoRow correo_tab, ParticipeTD.TelefonoRow telefono, ParticipeTD.BoletinesRow boletin, ParticipeTD.DireccionRow direccion, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_ACT_PARTICIPE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 400000;

            string dateString = "1/1/1753 8:30:52 AM";
            DateTime date1 = DateTime.Parse(dateString,
                                      System.Globalization.CultureInfo.InvariantCulture); 

            SqlParameter prmIdentificador = cmd.Parameters.Add("@identificador", SqlDbType.Int);
            prmIdentificador.Value = participe.CODIGO;

            SqlParameter prmTipoPersona = cmd.Parameters.Add("@chvTipoPersona", SqlDbType.VarChar);
            prmTipoPersona.Value = participe.TIPO_PERSONA;

            SqlParameter prmRazonSocial = cmd.Parameters.Add("@chvRazonSocial", SqlDbType.VarChar);
            prmRazonSocial.Value = DBNull.Value;

            SqlParameter prmCodigoSectorEconomico = cmd.Parameters.Add("@chvCodigoSectorEconomico", SqlDbType.VarChar);
            prmCodigoSectorEconomico.Value = DBNull.Value;

            SqlParameter prmNombre1 = cmd.Parameters.Add("@chvNombre1", SqlDbType.VarChar);
            prmNombre1.Value = participe.NOMBRE1;

            SqlParameter prmNombre2 = cmd.Parameters.Add("@chvNombre2", SqlDbType.VarChar);
            prmNombre2.Value = participe.NOMBRE2;

            SqlParameter prmNombre3 = cmd.Parameters.Add("@chvNombre3", SqlDbType.VarChar);
            prmNombre3.Value = participe.NOMBRE3;

            SqlParameter prmApellidoPaterno = cmd.Parameters.Add("@chvApePaterno", SqlDbType.VarChar);
            prmApellidoPaterno.Value = participe.APELLIDO_PATERNO;

            SqlParameter prmApellidoMaterno = cmd.Parameters.Add("@chvApeMaterno", SqlDbType.VarChar);
            prmApellidoMaterno.Value = participe.APELLIDO_MATERNO;

            SqlParameter prmFechaNacimiento = cmd.Parameters.Add("@dateFechaNacimiento", SqlDbType.DateTime);
            //prmFechaNacimiento.Value = participe.FECHA_NACIMIENTO;
            if (participe.FECHA_NACIMIENTO < date1)
            { prmFechaNacimiento.Value = DBNull.Value; }
            else prmFechaNacimiento.Value = participe.FECHA_NACIMIENTO;


            SqlParameter prmTipoDocumento = cmd.Parameters.Add("@chvTipoDocumento", SqlDbType.VarChar);
            prmTipoDocumento.Value = participe.TIPO_DOCUMENTO;

            SqlParameter prmNumeroDocumento = cmd.Parameters.Add("@chvNumeroDocumento", SqlDbType.VarChar);
            prmNumeroDocumento.Value = participe.NUMERO_DOCUMENTO;

            SqlParameter prmTipoFirma = cmd.Parameters.Add("@chvTipoFirma", SqlDbType.VarChar);
            prmTipoFirma.Value = participe.TIPO_FIRMA;

            SqlParameter prmTipoParticipe = cmd.Parameters.Add("@chvTipoParticipe", SqlDbType.VarChar);
            prmTipoParticipe.Value = participe.TIPO_PARTICIPE;

            SqlParameter prmFlagProspecto = cmd.Parameters.Add("@chvFlagProspecto", SqlDbType.VarChar);
            prmFlagProspecto.Value = "N";

            SqlParameter prmIdPais = cmd.Parameters.Add("@chvIdPais", SqlDbType.Int);
            //prmIdPais.Value = Convert.ToInt32(participe.NACIONALIDAD);
            if (participe.NACIONALIDAD.Equals(""))
            { prmIdPais.Value = DBNull.Value; }
            else prmIdPais.Value = participe.NACIONALIDAD;

            SqlParameter prmEstadoCivil = cmd.Parameters.Add("@chvCodEstadoCivil", SqlDbType.VarChar);
            prmEstadoCivil.Value = participe.ESTADO_CIVIL;

            SqlParameter prmCodProfesion = cmd.Parameters.Add("@chvCodProfesion", SqlDbType.VarChar);
            prmCodProfesion.Value = DBNull.Value;

            SqlParameter prmCodCompania = cmd.Parameters.Add("@chvCodCompania", SqlDbType.VarChar);
            prmCodCompania.Value = DBNull.Value;

            SqlParameter prmCodCategoriaCompania = cmd.Parameters.Add("@chvCodCategoriaCompañia", SqlDbType.VarChar);
            prmCodCategoriaCompania.Value = DBNull.Value;

            SqlParameter prmEstado = cmd.Parameters.Add("@chvEstado", SqlDbType.VarChar);
            prmEstado.Value = "ACT";

            SqlParameter prmFlagNoPublicidad = cmd.Parameters.Add("@chvFlagNoPublicidad", SqlDbType.VarChar);
            prmFlagNoPublicidad.Value = "N";

            SqlParameter prmFlagCorrespondenciaEMail = cmd.Parameters.Add("@chvFlagCorrespondenciaEMail", SqlDbType.VarChar);
            //ot 6891 Inicio
            //prmFlagCorrespondenciaEMail.Value = "N";	
            prmFlagCorrespondenciaEMail.Value = participe.FLAG_CORRESPONDENCIA_EMAIL;
            //ot 6891 Fin
            SqlParameter prmFlagCorrespondenciaCorreo = cmd.Parameters.Add("@chvFlagCorrespondenciaCorreo", SqlDbType.VarChar);
            prmFlagCorrespondenciaCorreo.Value = "N";

            //Auditoria
            SqlParameter prmUsuarioModificacion = cmd.Parameters.Add("@chvUsuarioModificacion", SqlDbType.VarChar);
            prmUsuarioModificacion.Value = participe.USUARIO;

            SqlParameter prmAreaModificacion = cmd.Parameters.Add("@chvAreaModificacion", SqlDbType.VarChar);
            prmAreaModificacion.Value = "";

            SqlParameter prmFlagBloqueo = cmd.Parameters.Add("@chvFlagBloqueo", SqlDbType.VarChar);
            prmFlagBloqueo.Value = "N";

            SqlParameter prmFlagNivel1 = cmd.Parameters.Add("@chvFlagNivel1", SqlDbType.VarChar);
            prmFlagNivel1.Value = "N";

            SqlParameter prmFlagNivel2 = cmd.Parameters.Add("@chvFlagNivel2", SqlDbType.VarChar);
            prmFlagNivel2.Value = "N";

            SqlParameter prmFlagEliminado = cmd.Parameters.Add("@chvFlagEliminado", SqlDbType.VarChar);
            prmFlagEliminado.Value = "N";

            SqlParameter prmFlagAutogenerado = cmd.Parameters.Add("@chvFlagAutogenerado", SqlDbType.VarChar);
            prmFlagAutogenerado.Value = "N";

            SqlParameter prmEstadoAnterior = cmd.Parameters.Add("@chvEstadoAnterior", SqlDbType.VarChar);
            prmEstadoAnterior.Value = "PEN";

            SqlParameter prmFlagRelacionadoING = cmd.Parameters.Add("@chvFlagRelacionadoING", SqlDbType.VarChar);
            prmFlagRelacionadoING.Value = participe.FLAG_RELACION_SURA;

            SqlParameter prmIdTitular = cmd.Parameters.Add("@idTitular", SqlDbType.Int);
            prmIdTitular.Value = DBNull.Value;

            SqlParameter prmFlagAsociado = cmd.Parameters.Add("@flagAsociado", SqlDbType.VarChar);
            prmFlagAsociado.Value = "N";

            SqlParameter prmCodigosMancomuno = cmd.Parameters.Add("@codigosMancomuno", SqlDbType.VarChar);
            prmCodigosMancomuno.Value = DBNull.Value;

            SqlParameter prmSexo = cmd.Parameters.Add("@sexo", SqlDbType.VarChar);
            prmSexo.Value = participe.SEXO;

            SqlParameter prmCargoCompania = cmd.Parameters.Add("@chvCargoCompania", SqlDbType.VarChar);
            prmCargoCompania.Value = participe.CARGO;

            SqlParameter prmEsPromocionParticipe = cmd.Parameters.Add("@esPromocionParticipe", SqlDbType.Int);
            prmEsPromocionParticipe.Value = 0;

            SqlParameter prmFlagNoMedioTelefono = cmd.Parameters.Add("@chvFlagNoMedioTelefono", SqlDbType.VarChar);
            prmFlagNoMedioTelefono.Value = "S";

            SqlParameter prmFlagNoMedioAccesoWeb = cmd.Parameters.Add("@chvFlagNoMedioAccesoWeb", SqlDbType.VarChar);
            prmFlagNoMedioAccesoWeb.Value = "S";

            SqlParameter prmProfesionDesc = cmd.Parameters.Add("@chvProfesionDesc", SqlDbType.VarChar);
            prmProfesionDesc.Value = participe.PROFESION;

            SqlParameter prmCompaniaDesc = cmd.Parameters.Add("@chvCompaniaDesc", SqlDbType.VarChar);
            prmCompaniaDesc.Value = participe.COMPANIA;


            SqlParameter prmCodigoAgenciaOrigen = cmd.Parameters.Add("@chvCodigoAgenciaOrigen", SqlDbType.VarChar);
            if (participe.CODIGO_AGENCIA_ORIGEN.Equals(""))
                prmCodigoAgenciaOrigen.Value = DBNull.Value;
            else
                prmCodigoAgenciaOrigen.Value = participe.CODIGO_AGENCIA_ORIGEN;


            SqlParameter prmTipoPersoneria = cmd.Parameters.Add("@chvTipoPersoneria", SqlDbType.VarChar);
            if (participe.TIPO_PERSONERIA.Equals(""))
                prmTipoPersoneria.Value = DBNull.Value;
            else
                prmTipoPersoneria.Value = participe.TIPO_PERSONERIA;


            SqlParameter prmNivelCargo = cmd.Parameters.Add("@chvNivelCargo", SqlDbType.VarChar);
            prmNivelCargo.Value = participe.NIVEL_CARGO;

            SqlParameter prmGradoInstruccion = cmd.Parameters.Add("@chvGradoInstruccion", SqlDbType.VarChar);
            prmGradoInstruccion.Value = participe.GRADO_INSTRUCCION;

            SqlParameter prmRangoSueldo = cmd.Parameters.Add("@chvRangoSueldo", SqlDbType.VarChar);
            prmRangoSueldo.Value = DBNull.Value;

            SqlParameter prmNumeroHijos = cmd.Parameters.Add("@numeroHijos", SqlDbType.Int);
            prmNumeroHijos.Value = participe.NUMERO_HIJOS;

            SqlParameter prmFlagCourier = cmd.Parameters.Add("@flagCourier", SqlDbType.VarChar);
            prmFlagCourier.Value = "N";

            SqlParameter prmFlagOficinaING = cmd.Parameters.Add("@flagOficinaING", SqlDbType.VarChar);
            prmFlagOficinaING.Value = "N";


            SqlParameter prmFlagCapacidadLegal = cmd.Parameters.Add("@flagCapacidadLegal", SqlDbType.VarChar);
            prmFlagCapacidadLegal.Value = participe.CAPACIDAD_LEGAL;


            SqlParameter prmSenasParticulares = cmd.Parameters.Add("@senasParticulares", SqlDbType.VarChar);
            prmSenasParticulares.Value = participe.SENAS_PARTICULARES;

            SqlParameter prmFlagSujetoRenta = cmd.Parameters.Add("@flagSujetoRenta", SqlDbType.VarChar);
            prmFlagSujetoRenta.Value = "S";

            SqlParameter prmFlagMedioConsultas = cmd.Parameters.Add("@flagMedioConsultas", SqlDbType.VarChar);
            //OT 6891 Inicio
            //prmFlagMedioConsultas.Value = "S";					
            prmFlagMedioConsultas.Value = participe.FLAG_MEDIO_CONSULTAS;
            //OT 6891 Fin

            SqlParameter prmFlagMedioOperaciones = cmd.Parameters.Add("@flagMedioOperaciones", SqlDbType.VarChar);
            //OT 6891 Inicio
            //prmFlagMedioOperaciones.Value = "S";
            prmFlagMedioOperaciones.Value = participe.FLAG_MEDIO_OPERACIONES;
            //OT 6891 Fin
            SqlParameter prmAdicional3 = cmd.Parameters.Add("@adicional3", SqlDbType.VarChar);
            prmAdicional3.Value = DBNull.Value;

            SqlParameter prmIdPaisResidencia = cmd.Parameters.Add("@chvIdPaisResidencia", SqlDbType.Int);
            //prmIdPaisResidencia.Value = participe.PAIS_RESIDENCIA;
            if (participe.PAIS_RESIDENCIA.Equals(""))
                prmIdPaisResidencia.Value = DBNull.Value;
            else
                prmIdPaisResidencia.Value = participe.PAIS_RESIDENCIA;


            SqlParameter prmIdCiudadLugarNacimiento = cmd.Parameters.Add("@chvIdCiudadLugarNacimiento", SqlDbType.Int);
            prmIdCiudadLugarNacimiento.Value = participe.LUGAR_NACIMIENTO;
            if (participe.LUGAR_NACIMIENTO.Equals(""))
                prmIdCiudadLugarNacimiento.Value = DBNull.Value;
            else
                prmIdCiudadLugarNacimiento.Value = participe.LUGAR_NACIMIENTO;


            SqlParameter prmLugarNacimientoOtro = cmd.Parameters.Add("@chvLugarNacimientoOtro", SqlDbType.VarChar);
            prmLugarNacimientoOtro.Value = DBNull.Value;

            SqlParameter prmFechaCreacionEmpresa = cmd.Parameters.Add("@dateFechaCreacionEmpresa", SqlDbType.DateTime);
            prmFechaCreacionEmpresa.Value = DBNull.Value;

            SqlParameter prmFechaInicioOperaciones = cmd.Parameters.Add("@dateFechaInicioOperaciones", SqlDbType.DateTime);
            prmFechaInicioOperaciones.Value = DBNull.Value;

            SqlParameter prmFlagCuotasGarantia = cmd.Parameters.Add("@flagCuotasGarantia", SqlDbType.VarChar);
            prmFlagCuotasGarantia.Value = "N";

            SqlParameter prmFlagExcluidoRetencion = cmd.Parameters.Add("@flagExcluidoRetencion", SqlDbType.VarChar);
            prmFlagExcluidoRetencion.Value = "N";

            SqlParameter prmSectorEconomicoDesc = cmd.Parameters.Add("@chvSectorEconomicoDesc", SqlDbType.VarChar);
            prmSectorEconomicoDesc.Value = "";

            cmd.Transaction = trans;
            try
            {
                //OT5012 INI
                //Se quita la llamada a método RegistrarDocumentos
                registrarFondos(fondo, participe.IDENTIFICADOR, participe.USUARIO, cn, trans);
                // //OT 6891 Inicio
                registrarCorreo(correo_tab, participe.IDENTIFICADOR, participe.USUARIO, cn, trans);
                registrarTelefono(telefono, participe.IDENTIFICADOR, participe.USUARIO, cn, trans);

                registrarBoletin(boletin, participe.IDENTIFICADOR, participe.USUARIO, cn, trans);
                registrarDireccion(direccion, participe.IDENTIFICADOR, participe.USUARIO, cn, trans);
                // //OT 6891 Fin

                //OT5012 FIN

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


        //=============Inicio OT 6891 ==============================
        private void registrarBoletin(ParticipeTD.BoletinesRow boletin, int idParticipe, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_EJE_BOLETINES", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 10000;
            string parti;
            parti = boletin.BOLETIN;
            SqlParameter prmIdParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Char);
            prmIdParticipe.Value = idParticipe;

            SqlParameter prmflagIPG = cmd.Parameters.Add("flagIPG", SqlDbType.VarChar);
            prmflagIPG.Value = "";

            SqlParameter prmflagSemanal = cmd.Parameters.Add("@flagSemanal", SqlDbType.VarChar);
            prmflagSemanal.Value = boletin.BOLETIN;

            SqlParameter prmflagMensual = cmd.Parameters.Add("@flagMensual", SqlDbType.VarChar);
            prmflagMensual.Value = boletin.BOLETIN;

            SqlParameter prmflagEliminado = cmd.Parameters.Add("@flagEliminado", SqlDbType.VarChar);
            prmflagEliminado.Value ="N";

            SqlParameter prmusuario = cmd.Parameters.Add("@usuario", SqlDbType.VarChar);
            prmusuario.Value = codigoUsuario;

            SqlParameter prmfecha = cmd.Parameters.Add("@fecha", SqlDbType.VarChar);
            prmfecha.Value = DateTime.Now;

            SqlParameter prmarea = cmd.Parameters.Add("@area", SqlDbType.VarChar);
            prmarea.Value = "";


            cmd.Transaction = trans;

            try
            {

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

        public DataTable ObtenerLista_VidaAhorro(string codigoTabla)
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.REPO_LIS_TGENERAL_VIDA", cn);
            cmd.CommandType = CommandType.StoredProcedure;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("LISTA_AHORRO");
            da.Fill(dt);
            return dt;
        }


        public DataTable ObtenerPlanVidaAhorro()
        {
            SqlConnection cn = GetConnection();
            SqlCommand cmd = new SqlCommand("dbo.FMPR_LIS_PLAN_VIDA_AHORRO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("LISTA_PLAN_AHORRO");
            da.Fill(dt);
            return dt;
        }


        private void registrarDireccion(ParticipeTD.DireccionRow direccion, int idParticipe, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FOND_EJE_DIRECCION_X_PARTICIPE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 10000;

            int id_pais = 173;

            SqlParameter prmtipoDireccion = cmd.Parameters.Add("@tipoDireccion", SqlDbType.VarChar);
            prmtipoDireccion.Value = "CA1";

            SqlParameter prmtipoDireccionOtro = cmd.Parameters.Add("@tipoDireccionOtro", SqlDbType.VarChar);
            prmtipoDireccionOtro.Value = DBNull.Value;

            SqlParameter prmtipoVia = cmd.Parameters.Add("@tipoVia", SqlDbType.VarChar);
            prmtipoVia.Value = direccion.TIPO_VIA;

            SqlParameter prmnombreVia = cmd.Parameters.Add("@nombreVia", SqlDbType.VarChar);
            prmnombreVia.Value = direccion.NOMBRE_VIA;

            SqlParameter prmnumero = cmd.Parameters.Add("@numero", SqlDbType.VarChar);
            prmnumero.Value = direccion.NU_IND_DEP;

            SqlParameter prmtipoUbicacion = cmd.Parameters.Add("@tipoUbicacion", SqlDbType.VarChar);
            prmtipoUbicacion.Value = direccion.TIPO_UBICACION;

            SqlParameter prmnombreUbicacion = cmd.Parameters.Add("@nombreUbicacion", SqlDbType.VarChar);
            prmnombreUbicacion.Value = direccion.UBICACION;

            SqlParameter prmreferencia = cmd.Parameters.Add("@referencia", SqlDbType.VarChar);
            prmreferencia.Value = DBNull.Value;

            SqlParameter prmcodigoPostal = cmd.Parameters.Add("@codigoPostal", SqlDbType.VarChar);
            prmcodigoPostal.Value = "";

            SqlParameter prmidDistrito = cmd.Parameters.Add("@idDistrito", SqlDbType.VarChar);
           // prmidDistrito.Value = direccion.DISTRITO;
            if (direccion.DISTRITO.Equals(0))
                prmidDistrito.Value = DBNull.Value;
            else
                prmidDistrito.Value = direccion.DISTRITO;


            SqlParameter prmcomentario = cmd.Parameters.Add("@comentario", SqlDbType.VarChar);
            prmcomentario.Value = DBNull.Value;

            SqlParameter prmflagPredeterminado = cmd.Parameters.Add("@flagPredeterminado", SqlDbType.VarChar);
            prmflagPredeterminado.Value = "S";

            SqlParameter prmIdParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Int);
            prmIdParticipe.Value = idParticipe;

            SqlParameter prmusuarioCreacion = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
            prmusuarioCreacion.Value = codigoUsuario;

            SqlParameter prmUsuarioModificacioncorreo = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            prmUsuarioModificacioncorreo.Value = codigoUsuario;

            SqlParameter prmAreaModificacionDocumento = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            prmAreaModificacionDocumento.Value = "";

            SqlParameter prmid_pais = cmd.Parameters.Add("@id_pais", SqlDbType.Int);
            prmid_pais.Value = id_pais;

            SqlParameter prmid_departamento = cmd.Parameters.Add("@id_departamento", SqlDbType.VarChar);
           // prmid_departamento.Value =  direccion.DEPARTAMENTO;
            if (direccion.DEPARTAMENTO.Equals(0))
                prmid_departamento.Value = DBNull.Value;
            else
                prmid_departamento.Value = direccion.DEPARTAMENTO;



            SqlParameter prmid_ciudad = cmd.Parameters.Add("@id_ciudad", SqlDbType.VarChar);
            //prmid_ciudad.Value = direccion.DEPARTAMENTO;
            if (direccion.CIUDAD.Equals(0))
                prmid_ciudad.Value = DBNull.Value;
            else
                prmid_ciudad.Value = direccion.CIUDAD;

            SqlParameter prmciudadOtro = cmd.Parameters.Add("@ciudadOtro", SqlDbType.VarChar);
            prmciudadOtro.Value = DBNull.Value;

            cmd.Transaction = trans;

            try
            {

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

        private void registrarTelefono(ParticipeTD.TelefonoRow telefono, int idParticipe, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand("dbo.FOND_EJE_TELEFONO_X_PARTICIPE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmtipoTelefono = cmd.Parameters.Add("@tipoTelefono", SqlDbType.VarChar);
            prmtipoTelefono.Value = "CA1";

            SqlParameter prmtipoTelefonoOtro = cmd.Parameters.Add("@tipoTelefonoOtro", SqlDbType.VarChar);
            prmtipoTelefonoOtro.Value = DBNull.Value;

            SqlParameter prmnumero = cmd.Parameters.Add("@numero", SqlDbType.VarChar);
            prmnumero.Value = telefono.NUMERO;


            SqlParameter prmcomentario = cmd.Parameters.Add("@comentario", SqlDbType.VarChar);
            prmcomentario.Value = DBNull.Value;

            SqlParameter prmflagPredeterminado = cmd.Parameters.Add("@flagPredeterminado", SqlDbType.VarChar);
            prmflagPredeterminado.Value = "S";


            SqlParameter prmIdParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Int);
            prmIdParticipe.Value = idParticipe;

            SqlParameter prmUsuarioModificacioncorreo = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            prmUsuarioModificacioncorreo.Value = codigoUsuario;

            SqlParameter prmAreaModificacionDocumento = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            prmAreaModificacionDocumento.Value = "";

            cmd.Transaction = trans;

            try
            {

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

        private void registrarCorreo(ParticipeTD.Correo_EletronicoRow correo_tab, int idParticipe, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FOND_EJE_CORREO_ELECTRONICO_XPARTICIPE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 10000;
            SqlParameter prmtipoCorreoElectronico = cmd.Parameters.Add("@tipoCorreoElectronico", SqlDbType.VarChar);
            prmtipoCorreoElectronico.Value = "CA1";

            SqlParameter prmtipoCorreoElectronicoOtro = cmd.Parameters.Add("@tipoCorreoElectronicoOtro", SqlDbType.VarChar);
            prmtipoCorreoElectronicoOtro.Value = DBNull.Value;

            SqlParameter prmcorreo = cmd.Parameters.Add("@correo", SqlDbType.VarChar);
            prmcorreo.Value = correo_tab.CORREO;


            SqlParameter prmcomentario = cmd.Parameters.Add("@comentario", SqlDbType.VarChar);
            prmcomentario.Value = DBNull.Value;

            SqlParameter prmflagPredeterminado = cmd.Parameters.Add("@flagPredeterminado", SqlDbType.VarChar);
            prmflagPredeterminado.Value = "S";


            SqlParameter prmIdParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Int);
            prmIdParticipe.Value = idParticipe;

            SqlParameter prmusuarioCreacioncorreo = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
            prmusuarioCreacioncorreo.Value = codigoUsuario;

            SqlParameter prmUsuarioModificacioncorreo = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            prmUsuarioModificacioncorreo.Value = codigoUsuario;

            SqlParameter prmAreaModificacionDocumento = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            prmAreaModificacionDocumento.Value = "";

            cmd.Transaction = trans;

            try
            {

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
        //=============Fin OT 6891 Fin==============================
        //Registrar los datos de los documentos del partícipe
        public void registrarDocumentos(ParticipeTD.DocumentoRow documento, int idParticipe, string codigoUsuario, SqlConnection cn, SqlTransaction trans)
        {

            //Registrar Documento

            SqlCommand cmd = new SqlCommand("dbo.FOND_INS_DOCUMENTO_XPARTICIPE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmNumero = cmd.Parameters.Add("@numero", SqlDbType.VarChar);
            prmNumero.Value = documento.NUMERO;

            SqlParameter prmTipo = cmd.Parameters.Add("@tipo", SqlDbType.VarChar);
            prmTipo.Value = documento.TIPO;

            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = documento.FECHA;

            SqlParameter prmDescripcion = cmd.Parameters.Add("@descripcion", SqlDbType.VarChar);
            prmDescripcion.Value = documento.DESCRIPCION;

            SqlParameter prmCodigoImagen = cmd.Parameters.Add("@codigoImagen", SqlDbType.VarChar);
            prmCodigoImagen.Value = documento.CODIGO_IMAGEN;

            SqlParameter prmIdParticipeDocumento = cmd.Parameters.Add("@idParticipe", SqlDbType.Int);
            prmIdParticipeDocumento.Value = idParticipe;


            SqlParameter prmUsuarioModificacionDocumento = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            prmUsuarioModificacionDocumento.Value = codigoUsuario;

            SqlParameter prmAreaModificacionDocumento = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            prmAreaModificacionDocumento.Value = "";


            cmd.Transaction = trans;

            try
            {

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
        //Registrar los fondos del partícipe
        public void registrarFondos(ParticipeTD.FondoRow fondo, int idParticipe, string CodigoUsuario, SqlConnection cn, SqlTransaction trans)
        {
            //Registrar Fondo
            //OT5012 INI
            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_CONTRATO_PARTICIPE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 100000;
            //OT5012 FIN

            SqlParameter prmIdParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Int);
            prmIdParticipe.Value = idParticipe;

            SqlParameter prmId = cmd.Parameters.Add("@idFondo", SqlDbType.Int);
            prmId.Value = fondo.ID_FONDO;

            SqlParameter prmEstadoFondo = cmd.Parameters.Add("@estado", SqlDbType.VarChar);
            prmEstadoFondo.Value = "ACT";

            SqlParameter prmDiaRescate = cmd.Parameters.Add("@diaRescate", SqlDbType.Int);
            prmDiaRescate.Value = 0;

            SqlParameter prmPeriodoRescate = cmd.Parameters.Add("@periodoRescate", SqlDbType.Int);
            prmPeriodoRescate.Value = 0;

            SqlParameter prmMesInicioRescate = cmd.Parameters.Add("@mesInicioRescate", SqlDbType.Int);
            prmMesInicioRescate.Value = 0;

            SqlParameter prmAnioInicioRescate = cmd.Parameters.Add("@anioInicioRescate", SqlDbType.Int);
            prmAnioInicioRescate.Value = 0;

            //OT5012 INI
            SqlParameter prmFlagUsaMonto = cmd.Parameters.Add("@flagUsaMonto", SqlDbType.VarChar);
            prmFlagUsaMonto.Value = DBNull.Value;
            //OT5012 FIN

            SqlParameter prmMonto = cmd.Parameters.Add("@monto", SqlDbType.Decimal);
            prmMonto.Value = 0;

            SqlParameter prmIdCuentaBancaria = cmd.Parameters.Add("@idCuentaBancaria", SqlDbType.Int);
            prmIdCuentaBancaria.Value = DBNull.Value;

            SqlParameter prmIdCuentaParticipacion = cmd.Parameters.Add("@idCuentaParticipacion", SqlDbType.Int);
            prmIdCuentaParticipacion.Value = DBNull.Value;

            SqlParameter prmUsuarioCreacion = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
            prmUsuarioCreacion.Value = CodigoUsuario;

            //OT5012 INI
            SqlParameter prmFecha = cmd.Parameters.Add("@fecha", SqlDbType.DateTime);
            prmFecha.Value = fondo.FECHA;

            SqlParameter prmDescripcion = cmd.Parameters.Add("@descripcion", SqlDbType.VarChar);
            prmDescripcion.Value = "";

            SqlParameter prmCodigoImagen = cmd.Parameters.Add("@codigoImagen", SqlDbType.VarChar);
            prmCodigoImagen.Value = "";
            //OT5012 FIN

            SqlParameter prmUsuarioModificacionFondo = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            prmUsuarioModificacionFondo.Value = CodigoUsuario;

            SqlParameter prmAreaModificacionFondo = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            prmAreaModificacionFondo.Value = "";

            cmd.Transaction = trans;

            try
            {

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

        //registrar los prospectos
        public void registrarProspectos(ParticipeTD.ParticipeRow participe, string contrasena, string areaModificacion, string flagVerOperacion, string flagAccesoWeb, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMPR_INS_PARTICIPE_CONTRATO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 10000;

            int idProspecto;

            SqlParameter prmTipoPersona = cmd.Parameters.Add("@chvTipoPersona", SqlDbType.VarChar);
            prmTipoPersona.Value = "NAT";

            SqlParameter prmNombre1 = cmd.Parameters.Add("@chvNombre1", SqlDbType.VarChar);
            prmNombre1.Value = participe.NOMBRE1;

            SqlParameter prmNombre2 = cmd.Parameters.Add("@chvNombre2", SqlDbType.VarChar);
            prmNombre2.Value = participe.NOMBRE2;

            SqlParameter prmNombre3 = cmd.Parameters.Add("@chvNombre3", SqlDbType.VarChar);
            prmNombre3.Value = participe.NOMBRE3;

            SqlParameter prmApellidoPaterno = cmd.Parameters.Add("@chvApePaterno", SqlDbType.VarChar);
            prmApellidoPaterno.Value = participe.APELLIDO_PATERNO;

            SqlParameter prmApellidoMaterno = cmd.Parameters.Add("@chvApeMaterno", SqlDbType.VarChar);
            prmApellidoMaterno.Value = participe.APELLIDO_MATERNO;

            SqlParameter prmSexo = cmd.Parameters.Add("@sexo", SqlDbType.VarChar);
            prmSexo.Value = participe.SEXO;

            SqlParameter prmFechaNacimiento = cmd.Parameters.Add("@dateFechaNacimiento", SqlDbType.DateTime);
            prmFechaNacimiento.Value = participe.FECHA_NACIMIENTO;

            SqlParameter prmTipoDocumento = cmd.Parameters.Add("@chvTipoDocumento", SqlDbType.VarChar);
            prmTipoDocumento.Value = participe.TIPO_DOCUMENTO;

            SqlParameter prmNumeroDocumento = cmd.Parameters.Add("@chvNumeroDocumento", SqlDbType.VarChar);
            prmNumeroDocumento.Value = participe.NUMERO_DOCUMENTO;

            SqlParameter prmTipoFirma = cmd.Parameters.Add("@chvTipoFirma", SqlDbType.VarChar);
            prmTipoFirma.Value = participe.TIPO_FIRMA;

            SqlParameter prmTipoParticipe = cmd.Parameters.Add("@chvTipoParticipe", SqlDbType.VarChar);
            prmTipoParticipe.Value = participe.TIPO_PARTICIPE;

            SqlParameter prmEstadoCivil = cmd.Parameters.Add("@chvCodEstadoCivil", SqlDbType.VarChar);
            prmEstadoCivil.Value = participe.ESTADO_CIVIL;

            SqlParameter prmSenasParticulares = cmd.Parameters.Add("@senasParticulares", SqlDbType.VarChar);
            prmSenasParticulares.Value = participe.SENAS_PARTICULARES;

            SqlParameter prmFlagRelacionadoING = cmd.Parameters.Add("@chvFlagRelacionadoING", SqlDbType.VarChar);
            prmFlagRelacionadoING.Value = participe.FLAG_RELACION_SURA;

            SqlParameter prmIdPais = cmd.Parameters.Add("@chvIdPais", SqlDbType.Int);
            prmIdPais.Value = participe.NACIONALIDAD;

            SqlParameter prmProfesionDesc = cmd.Parameters.Add("@chvProfesionDesc", SqlDbType.VarChar);
            prmProfesionDesc.Value = participe.PROFESION;

            SqlParameter prmCompaniaDesc = cmd.Parameters.Add("@chvCompaniaDesc", SqlDbType.VarChar);
            prmCompaniaDesc.Value = participe.COMPANIA;

            SqlParameter prmCargoCompania = cmd.Parameters.Add("@chvCargoCompania", SqlDbType.VarChar);
            prmCargoCompania.Value = participe.CARGO;

            SqlParameter prmNumeroHijos = cmd.Parameters.Add("@numeroHijos", SqlDbType.Int);
            prmNumeroHijos.Value = participe.NUMERO_HIJOS;

            SqlParameter prmIdCiudadLugarNacimiento = cmd.Parameters.Add("@chvIdCiudadLugarNacimiento", SqlDbType.Int);
            prmIdCiudadLugarNacimiento.Value = participe.LUGAR_NACIMIENTO;

            SqlParameter prmLugarNacimientoOtro = cmd.Parameters.Add("@chvLugarNacimientoOtro", SqlDbType.VarChar);
            prmLugarNacimientoOtro.Value = DBNull.Value;

            SqlParameter prmIdPaisResidencia = cmd.Parameters.Add("@chvIdPaisResidencia", SqlDbType.Int);
            prmIdPaisResidencia.Value = participe.PAIS_RESIDENCIA;

            SqlParameter prmNivelCargo = cmd.Parameters.Add("@chvNivelCargo", SqlDbType.VarChar);
            prmNivelCargo.Value = participe.NIVEL_CARGO;

            SqlParameter prmGradoInstruccion = cmd.Parameters.Add("@chvGradoInstruccion", SqlDbType.VarChar);
            prmGradoInstruccion.Value = participe.GRADO_INSTRUCCION;

            SqlParameter prmFlagCapacidadLegal = cmd.Parameters.Add("@flagCapacidadLegal", SqlDbType.VarChar);
            prmFlagCapacidadLegal.Value = participe.CAPACIDAD_LEGAL;

            //Auditoria
            SqlParameter prmUsuarioModificacion = cmd.Parameters.Add("@chvUsuarioModificacion", SqlDbType.VarChar);
            prmUsuarioModificacion.Value = participe.USUARIO;

            SqlParameter prmChvContrasena = cmd.Parameters.Add("@chvContrasena", SqlDbType.VarChar);
            prmChvContrasena.Value = contrasena;

            SqlParameter prmChvAreaModificacion = cmd.Parameters.Add("@chvAreaModificacion", SqlDbType.VarChar);
            prmChvAreaModificacion.Value = areaModificacion;

            SqlParameter prmFlagVerOperacion = cmd.Parameters.Add("@flagVerOperacion", SqlDbType.VarChar);
            prmFlagVerOperacion.Value = flagVerOperacion;

            SqlParameter prmFlagAccesoWeb = cmd.Parameters.Add("@flagAccesoWeb", SqlDbType.VarChar);
            prmFlagAccesoWeb.Value = flagAccesoWeb;

            cmd.Transaction = trans;

            try
            {
                idProspecto = Convert.ToInt32(cmd.ExecuteScalar());
                participe.IDENTIFICADOR = idProspecto;
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

        //Actualizar los datos del partícipe mancómuno con los datos del partícipe titular
        public void actualizarParticipe(ParticipeTD.ParticipeRow participe, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FMPR_ACT_PARTICIPE_CONTRATO", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 100000;
            SqlParameter prmCodigoParticipe = cmd.Parameters.Add("@codigoParticipe", SqlDbType.Int);
            prmCodigoParticipe.Value = participe.CODIGO;

            SqlParameter prmIdProspecto = cmd.Parameters.Add("@idProspecto", SqlDbType.Int);
            prmIdProspecto.Value = participe.IDENTIFICADOR;

            SqlParameter prmCodigoAgenciaOrigen = cmd.Parameters.Add("@codigoAgenciaOrigen", SqlDbType.VarChar);
            prmCodigoAgenciaOrigen.Value = participe.CODIGO_AGENCIA_ORIGEN;

            SqlParameter prmTipoPersoneria = cmd.Parameters.Add("@tipoPersoneria", SqlDbType.VarChar);
            prmTipoPersoneria.Value = participe.TIPO_PERSONERIA;

            SqlParameter prmTipoAtribucion = cmd.Parameters.Add("@tipoAtribucion", SqlDbType.VarChar);
            prmTipoAtribucion.Value = participe.TIPO_ATRIBUCION;

            cmd.Transaction = trans;

            try
            {

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

        //Registrar la asociación del partícipe mancómuno con el asociado
        public void registrarParticipeXparticipe(int idParticipe, int idParticipeAsociado, string codigoUsuario, decimal porcentaje, SqlConnection cn, SqlTransaction trans)
        {

            SqlCommand cmd = new SqlCommand("dbo.FOND_INS_PERSONA_ASOCIADA_XPARTICIPE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prmIdParticipe = cmd.Parameters.Add("@idParticipe", SqlDbType.Int);
            prmIdParticipe.Value = idParticipe;

            SqlParameter prmIdParticipeAsociado = cmd.Parameters.Add("@idParticipeAsociado", SqlDbType.Int);
            prmIdParticipeAsociado.Value = idParticipeAsociado;

            SqlParameter prmTipoRelacion = cmd.Parameters.Add("@tipoRelacion", SqlDbType.VarChar);
            if (idParticipe == idParticipeAsociado)
            {
                prmTipoRelacion.Value = "TIT";
            }
            else
            {
                prmTipoRelacion.Value = "MAN";
            }

            SqlParameter prmTipoRelacionOtro = cmd.Parameters.Add("@tipoRelacionOtro", SqlDbType.VarChar);
            prmTipoRelacionOtro.Value = DBNull.Value;

            SqlParameter prmComentario = cmd.Parameters.Add("@comentario", SqlDbType.VarChar);
            prmComentario.Value = DBNull.Value;

            SqlParameter prmFlagContacto1 = cmd.Parameters.Add("@flagContacto1", SqlDbType.VarChar);
            prmFlagContacto1.Value = "N";

            SqlParameter prmFlagContacto2 = cmd.Parameters.Add("@flagContacto2", SqlDbType.VarChar);
            prmFlagContacto2.Value = "N";

            SqlParameter prmUsuarioCreacion = cmd.Parameters.Add("@usuarioCreacion", SqlDbType.VarChar);
            prmUsuarioCreacion.Value = codigoUsuario;

            SqlParameter prmUsuarioModificacion = cmd.Parameters.Add("@usuarioModificacion", SqlDbType.VarChar);
            prmUsuarioModificacion.Value = codigoUsuario;

            SqlParameter prmAreaModificacion = cmd.Parameters.Add("@areaModificacion", SqlDbType.VarChar);
            prmAreaModificacion.Value = "";

            SqlParameter prmPorcentajeParticipacion = cmd.Parameters.Add("@PORCENTAJE_PARTICIPACION", SqlDbType.Decimal);
            if (porcentaje == decimal.MinValue)
            {
                prmPorcentajeParticipacion.Value = DBNull.Value;
            }
            else
            {
                prmPorcentajeParticipacion.Value = porcentaje;
            }

            cmd.Transaction = trans;

            try
            {

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
            catch (Exception e)
            {
                //return null;
                throw e;
            }
            finally
            {
                cmd.Dispose();
            }
        }

    }
}
