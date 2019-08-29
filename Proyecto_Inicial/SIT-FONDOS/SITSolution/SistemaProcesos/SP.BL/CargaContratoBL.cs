/*
 * Fecha de Modificación: 30/10/2012
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
 * Descripción del cambio: Se modificó el método Procesar
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
 * Descripción del cambio: Se crea el método obtenerContrasenaCifrada y se modifica 
 *						   el método Procesar para enviar la contraseña encriptada como
 *						   parámetro al método registrarProspectos.
 * 
 * */
/*
 * Fecha de Modificación: 10/07/2013
 * Modificado por: Robert Castillo
 * Numero de OT: 5526
 * Descripción del cambio: En el método Procesar se obtienen los valores de las variables 
 *						   porcentajeTitular y porcentajeAsociado a partir de los datos
 *						   ingresados en la plantilla Contratos.xls, para el caso de los
 *						  partícipes mancómunos de tipo distribuido.
 * 
 * */

/*
 * Fecha de Modificación: 16/09/2014
 * Modificado por: Leslie Valerio
 * Numero de OT: 6891
 * Descripción del cambio: Se agrego nuevos campos en la plantilla Contratos.xls; para los casos de VidaAHorro
 *                         - Se agrego nuevas tablas con las que trabaja Vida Ahorro (Telefono, Direccion, Boletin, Correo,Cuenta Participacion) 
 * */
using System;
using System.Data;
using Procesos.DA;
using INGFondos.Support.Interop;
using System.Data.SqlClient;
using System.Collections;
using Procesos.TD;

using INGFondos.Data;

namespace Procesos.BL
{

    public class CargaContratoBL : INGFondos.Data.DA
    {
        private string sourcePath;
        private string codigoUsuario;

        public CargaContratoBL(string sourcePath, string codigoUsuario)
            : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones)
        {

            this.sourcePath = sourcePath;
            this.codigoUsuario = codigoUsuario;

        }

        public DataTable ObtenerListaProspectosVacios()
        {
            CargaContratoDA da = new CargaContratoDA();
            DataTable prospectos = da.ObtenerListaProspectosVacios();
            return prospectos;
        }

        public void Procesar()
        {
            ExcelApplication excelApplication = new ExcelApplication();
            CargaContratoDA da = new CargaContratoDA();
            SqlConnection cn = GetConnection();
            cn.Open();

            SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);
            int cucanterior = 0;
            int idanterior = 0;
            string prmVidaAhorro = "";
            string prmCUC = "";
            try
            {

                // Abriendo el archivo que contiene la información
                excelApplication.OpenWorkBook(sourcePath, ExcelMode.Full);
                ExcelWorkSheet sheet = excelApplication.GetWorkBook(1).GetSheet(1);

                //Obtiene los prospectos vacios
                CargaContratoDA cargaContratoDA = new CargaContratoDA();
                DataTable dtProspectos = cargaContratoDA.ObtenerListaProspectosVacios();

                // Obteniendo la información del archivo
                int rowIndex = 6;
                object[] row = sheet.GetRow(rowIndex++, 'C', 'Z', 4);
                int contador = 0;
                //OT 6891 inicio
                DataTable dtCodVida = ObtenerCodigoEmpresas_Vida();
                string DSC_VIDA_AHORRO = "";
                if (dtCodVida.Rows.Count > 0)
                {
                    DataRow row1 = dtCodVida.Rows[0];
                    //Obtiene nombre de la empresa Vida Ahorro
                    DSC_VIDA_AHORRO = Convert.ToString(row1["DESCRIPCION_LARGA"]);
                
                }


                DataTable dtPlanVidaAhorro = ObtenerPlanVidaAhorro();
                string DSC_PLAN_VIDA_AHORRO = "";
                if (dtPlanVidaAhorro.Rows.Count > 0)
                {
                    DataRow row2 = dtPlanVidaAhorro.Rows[0];
                    //Obtiene nombre de la empresa Vida Ahorro
                    DSC_PLAN_VIDA_AHORRO = Convert.ToString(row2["VALOR"]);

                }

                
                while (row[0] != null)
                {
                    try
                    {
                       
                        int idProspecto = Convert.ToInt32(sheet.GetString(string.Format("B{0}", rowIndex - 1))) - 44670000;
                        int codigoProspecto = Convert.ToInt32(sheet.GetString(string.Format("B{0}", rowIndex - 1)));
                       
                        //Leer datos del Partícipe


                     


                        ParticipeTD.ParticipeRow drParticipe = new ParticipeTD().Participe.NewParticipeRow();
                        drParticipe.IDENTIFICADOR = Convert.ToInt32(sheet.GetString(string.Format("B{0}", rowIndex - 1))) - 44670000;
                        drParticipe.CODIGO = Convert.ToInt32(sheet.GetString(string.Format("B{0}", rowIndex - 1)));
                        // Ini - traer el CUC del Participe OT 6891
                        prmCUC = drParticipe.CODIGO.ToString();
                        // Fin  - traer el CUC del Participe OT 6891
                        drParticipe.TIPO_PERSONA = row[71].ToString();
                        drParticipe.NOMBRE1 = row[2].ToString();
                        drParticipe.NOMBRE2 = row[3] == null ? "" : row[3].ToString();
                        drParticipe.NOMBRE3 = row[4] == null ? "" : row[4].ToString();
                        drParticipe.APELLIDO_PATERNO = row[5].ToString();
                        drParticipe.APELLIDO_MATERNO = row[6].ToString();
                        drParticipe.SEXO = row[72].ToString();
                        drParticipe.FECHA_NACIMIENTO = Convert.ToDateTime(row[8]);
                      
                        drParticipe.TIPO_DOCUMENTO = row[73].ToString();
                        drParticipe.NUMERO_DOCUMENTO = row[10].ToString();
                        drParticipe.TIPO_FIRMA = row[74].ToString();
                        drParticipe.TIPO_PARTICIPE = row[75].ToString();
                        drParticipe.ESTADO_CIVIL = row[76].ToString();
                        drParticipe.CODIGO_AGENCIA_ORIGEN = row[77] == null ? "" : row[77].ToString();
                        drParticipe.TIPO_PERSONERIA = row[78] == null ? "" : row[78].ToString();
                        drParticipe.SENAS_PARTICULARES = row[16] == null ? "" : row[16].ToString();
                        drParticipe.FLAG_RELACION_SURA = "N";
                        drParticipe.NACIONALIDAD = row[80].ToString();
                        drParticipe.PROFESION = row[18] == null ? "" : row[18].ToString();
                        drParticipe.COMPANIA = row[19] == null ? "" : row[19].ToString();
                        drParticipe.CARGO = row[20] == null ? "" : row[20].ToString();
                        drParticipe.NUMERO_HIJOS = row[21] == null ? 0 : Convert.ToInt32(row[21]);
                        drParticipe.LUGAR_NACIMIENTO = row[81] == null ? null : row[81].ToString();
                        drParticipe.PAIS_RESIDENCIA = row[82].ToString();
                        drParticipe.NIVEL_CARGO = row[83] == null ? null : row[83].ToString();
                        drParticipe.GRADO_INSTRUCCION = row[84] == null ? null : row[84].ToString();
                        drParticipe.CAPACIDAD_LEGAL =row[85]== null ? null : row[85].ToString();
                        drParticipe.FLAG_CORRESPONDENCIA_EMAIL = row[27] == null ? "" : row[27].ToString();
                        drParticipe.FLAG_MEDIO_CONSULTAS = row[28] == null ? "" : row[28].ToString();
                        drParticipe.FLAG_MEDIO_OPERACIONES = row[29] == null ? "" : row[29].ToString();
                        drParticipe.TIPO_ATRIBUCION = row[100] == null ? null : row[100].ToString();

                        drParticipe.USUARIO = codigoUsuario;
                        //INI OT 6891 
                        // Tabla telefono
                        ParticipeTD.TelefonoRow drTelefono = new ParticipeTD().Telefono.NewTelefonoRow();
                        drTelefono.NUMERO = row[31] == null ? "" : row[31].ToString();


                        //Tabla correo electronico       
                        ParticipeTD.Correo_EletronicoRow drCorreo = new ParticipeTD().Correo_Eletronico.NewCorreo_EletronicoRow();
                        drCorreo.CORREO = row[30] == null ? "" : row[30].ToString();

                        ParticipeTD.BoletinesRow drboletin = new ParticipeTD().Boletines.NewBoletinesRow();
                        drboletin.BOLETIN = row[32].ToString();

                        // Tabla direccion
                        ParticipeTD.DireccionRow drDireccion = new ParticipeTD().Direccion.NewDireccionRow();
                        string pais = "";
                        pais = row[105].ToString();
                        if (row[103].Equals("") || row[103].Equals(null) || pais.Length > 5)
                        { drDireccion.DEPARTAMENTO = 0; }
                        else
                        {
                            drDireccion.DEPARTAMENTO = Convert.ToInt32(row[103]);
                        }
                        string ciud = "";
                        ciud = row[105].ToString();
                        if (row[104].Equals("") || row[104].Equals(null) || ciud.Length > 5)
                        { drDireccion.CIUDAD = 0; }
                        else
                        {
                            drDireccion.CIUDAD = Convert.ToInt32(row[104]);
                        }
                        string dist = "";
                        dist = row[105].ToString();
                        if (row[105].Equals("") || row[105].Equals(null) || dist.Length>5)
                        { drDireccion.DISTRITO = 0; }
                        else
                        {
                            drDireccion.DISTRITO = Convert.ToInt32(row[105]);
                        }
                      //  drDireccion.CIUDAD = Convert.ToInt32(row[104]);
                        //****
                       // drDireccion.DISTRITO = Convert.ToInt32(row[105]);
                        drDireccion.TIPO_VIA = row[106] == null ? "" : row[106].ToString();
                        drDireccion.NOMBRE_VIA = row[37] == null ? "" : row[37].ToString();
                        drDireccion.NU_IND_DEP = row[38] == null ? "" : row[38].ToString();
                        drDireccion.UBICACION = row[40] == null ? "" : row[40].ToString();
                        drDireccion.TIPO_UBICACION = row[107] == null ? "" : row[107].ToString();


                        //fin OT 6891 


                        //Leer datos del Fondo

                        ParticipeTD.FondoRow drFondo = new ParticipeTD().Fondo.NewFondoRow();
                        drFondo.ID_FONDO = Convert.ToInt32(row[87]);
                        //OT5012 INI
                        drFondo.FECHA = Convert.ToDateTime(row[43]);

                        //Leer datos del documento
                        ParticipeTD.DocumentoRow drDocumento = new ParticipeTD().Documento.NewDocumentoRow();
                        //OT5012 FIN

                        drDocumento.TIPO = row[86].ToString();
                        drDocumento.FECHA = Convert.ToDateTime(row[43]);
                        drDocumento.DESCRIPCION = row[44] == null ? "" : row[44].ToString();
                        drDocumento.CODIGO_IMAGEN = row[45] == null ? "" : row[45].ToString();

                        //Inicio  OT 6891 
                        // Cuenta Participacion
                        CuentaParticipacionTD.CUENTA_PARTICIPACIONRow drCuentaParticipacion = new CuentaParticipacionTD().CUENTA_PARTICIPACION.NewCUENTA_PARTICIPACIONRow();
                        drCuentaParticipacion.VIDA_AHORRO = row[109] == null ? "" : row[109].ToString();
                        drCuentaParticipacion.CODIGO = prmCUC;
                        drCuentaParticipacion.ETIQUETA = DSC_VIDA_AHORRO;
                        prmVidaAhorro = drCuentaParticipacion.VIDA_AHORRO;
                        drCuentaParticipacion.USUARIO_CREACION = codigoUsuario;
                        drCuentaParticipacion.CODIGO_PLAN = DSC_PLAN_VIDA_AHORRO;
                        // Fin  OT 6891

                        ParticipeTD.ParticipeRow drParticipeAsociado = null;


                        //Leer partícipe asociado
                        if (drParticipe.TIPO_PERSONA.Equals("MAN") && cucanterior != codigoProspecto)
                        {

                            drParticipeAsociado = new ParticipeTD().Participe.NewParticipeRow();

                            drParticipeAsociado.TIPO_PERSONA = "NAT";

                            drParticipeAsociado.NOMBRE1 = row[47].ToString();
                            drParticipeAsociado.NOMBRE2 = row[48] == null ? "" : row[48].ToString();
                            drParticipeAsociado.NOMBRE3 = row[49] == null ? "" : row[49].ToString();
                            drParticipeAsociado.APELLIDO_PATERNO = row[50].ToString();
                            drParticipeAsociado.APELLIDO_MATERNO = row[51].ToString();
                            drParticipeAsociado.SEXO = row[72].ToString();
                            drParticipeAsociado.FECHA_NACIMIENTO = Convert.ToDateTime(row[53]);
                            drParticipeAsociado.TIPO_DOCUMENTO = row[73].ToString();
                            drParticipeAsociado.NUMERO_DOCUMENTO = row[55].ToString();
                            drParticipeAsociado.TIPO_FIRMA = "IND";
                            drParticipeAsociado.TIPO_PARTICIPE = row[91].ToString();

                            drParticipeAsociado.ESTADO_CIVIL = row[92].ToString();
                            drParticipeAsociado.CODIGO_AGENCIA_ORIGEN = "";//
                            drParticipeAsociado.TIPO_PERSONERIA = "";//

                            drParticipeAsociado.SENAS_PARTICULARES = "";//
                            drParticipeAsociado.FLAG_RELACION_SURA = "N";
                            drParticipeAsociado.NACIONALIDAD = row[94].ToString();
                            drParticipeAsociado.PROFESION = row[61] == null ? "" : row[61].ToString();
                            drParticipeAsociado.COMPANIA = row[62] == null ? "" : row[62].ToString();
                            drParticipeAsociado.CARGO = row[63] == null ? "" : row[63].ToString();
                            drParticipeAsociado.NUMERO_HIJOS = row[64] == null ? 0 : Convert.ToInt32(row[64]);
                            drParticipeAsociado.LUGAR_NACIMIENTO = row[95].ToString();
                            drParticipeAsociado.PAIS_RESIDENCIA = row[96].ToString();
                            drParticipeAsociado.NIVEL_CARGO = row[96].ToString();
                            drParticipeAsociado.GRADO_INSTRUCCION = row[99].ToString();
                            drParticipeAsociado.CAPACIDAD_LEGAL = row[99].ToString();

                            drParticipeAsociado.USUARIO = codigoUsuario;
                        }
                        //Partícipes NAT
                        if (row[71].ToString().Equals("NAT"))
                        {
                            cargaContratoDA.registrarParticipe(drParticipe, drFondo, drDocumento, drCorreo, drTelefono, drboletin, drDireccion, cn, trans);
                            // OT 6891 INICIO
                            if (prmVidaAhorro.Equals("S"))
                            {
                                cargaContratoDA.RegistrarCuentaParticipacion_x_Participe(drCuentaParticipacion, prmCUC, codigoUsuario, cn, trans);
                            }
                            // FIN OT 6891 FIN
                            DataTable dtPersona = cargaContratoDA.ObtenerPersona(drParticipe, cn, trans);
                           
                            if (dtPersona.Rows.Count == 0)
                            {
                                cargaContratoDA.registrarPersona(drParticipe, cn, trans);
                            }
                            else
                            {
                                cargaContratoDA.ActualizarPersona(drParticipe, cn, trans, "N");
                                cargaContratoDA.ActualizarParticipeXDocumento(drParticipe, cn, trans, "N");
                            }
                        }
                        //Partícipes MAN
                        else
                        {

                            //Registra dos prospectos
                            if (codigoProspecto != cucanterior)
                            {
                                String contrasenaCifrada = "";
                                String claveEncriptada = cargaContratoDA.ObtenerClaveEncriptada(cn, trans);

                                String areaModificacion = "OPE";
                                String flagVerOperacion = "S";
                                String flagAccesoWeb = "S";

                                contrasenaCifrada = obtenerContrasenaCifrada(claveEncriptada);
                                cargaContratoDA.registrarProspectos(drParticipe, contrasenaCifrada, areaModificacion, flagVerOperacion, flagAccesoWeb, cn, trans); //TITULAR

                                contrasenaCifrada = obtenerContrasenaCifrada(claveEncriptada);
                                cargaContratoDA.registrarProspectos(drParticipeAsociado, contrasenaCifrada, areaModificacion, flagVerOperacion, flagAccesoWeb, cn, trans); //ASOCIADO
                                /*CARGA DE LA PERSONA DEL ASOCIADO*/
                                DataTable dtPersona = cargaContratoDA.ObtenerPersona(drParticipeAsociado, cn, trans);
                                if (dtPersona.Rows.Count == 0)
                                {
                                    cargaContratoDA.registrarPersona(drParticipeAsociado, cn, trans);
                                }
                                else
                                {
                                    cargaContratoDA.ActualizarPersona(drParticipeAsociado, cn, trans, "S");
                                    cargaContratoDA.ActualizarParticipeXDocumento(drParticipeAsociado, cn, trans, "S");
                                }
                                int idParticipe = drParticipe.CODIGO - 44670000;
                                int idProspectoTitular = drParticipe.IDENTIFICADOR;
                                int idProspectoAsociado = drParticipeAsociado.IDENTIFICADOR;
                                cargaContratoDA.actualizarParticipe(drParticipe, cn, trans); //REGISTRO DEL PARTICIPE MANCOMUNO
                                /*CARGA DE LA PERSONA DEL TITULAR*/
                                dtPersona = cargaContratoDA.ObtenerPersona(drParticipe, cn, trans);
                                if (dtPersona.Rows.Count == 0)
                                {
                                    cargaContratoDA.registrarPersona(drParticipe, cn, trans);
                                }
                                else
                                {
                                    cargaContratoDA.ActualizarPersona(drParticipe, cn, trans, "N");
                                    cargaContratoDA.ActualizarParticipeXDocumento(drParticipe, cn, trans, "N");
                                }
                                //REGISTRAMOS LA RELACION ENTRE EL TITULAR Y EL ASOCIADO
                                decimal porcentajeTitular = decimal.MinValue;
                                decimal porcentajeAsociado = decimal.MinValue;

                                if (row[86] != null && row[86].ToString().Equals("DIS"))
                                {
                                    porcentajeTitular = Convert.ToDecimal(row[101]) * 100;
                                    porcentajeAsociado = Convert.ToDecimal(row[102]) * 100;
                                    if (porcentajeTitular + porcentajeAsociado != 100)
                                    {
                                        throw new Exception("La suma de los porcentajes debe sumar 100%.");
                                    }

                                    cargaContratoDA.registrarParticipeXparticipe(idProspecto, idProspecto, codigoUsuario, porcentajeTitular, cn, trans);
                                }
                                cargaContratoDA.registrarParticipeXparticipe(idProspecto, idProspectoAsociado, codigoUsuario, porcentajeAsociado, cn, trans);
                            }
                            //OT5012 INI
                            cargaContratoDA.registrarFondos(drFondo, idProspecto, codigoUsuario, cn, trans);
                            //OT5012 FIN
                        }
                        // FIN OT 6891
                        //sheet.SetValue(string.Format("B{0}",rowIndex-1),codigoProspecto);
                        cucanterior = codigoProspecto;
                        idanterior = idProspecto;

                        row = sheet.GetRow(rowIndex++, 'C', 'Z', 4);
                        contador++;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error en la línea {0}. Detalle " + ex.Message, rowIndex - 1));
                    }
                }

                trans.Commit();
                excelApplication.GetWorkBook(1).Save();
                excelApplication.Show();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                excelApplication.GetWorkBook(1).Close();
                excelApplication.Close();
                throw ex;
            }
            finally
            {
                trans.Dispose();
                cn.Close();
                excelApplication.Close();
            }

        }
        public DataTable ObtenerCodigoEmpresas_Vida()
        {
            CargaContratoDA cargaContratoDA = new CargaContratoDA();

            return cargaContratoDA.ObtenerLista_VidaAhorro("COD_EMP_CON");
        }

         public DataTable ObtenerPlanVidaAhorro()
        {
            CargaContratoDA cargaContrato = new CargaContratoDA();
            return cargaContrato.ObtenerPlanVidaAhorro();
        }

        public string obtenerContrasenaCifrada(string claveEncriptada)
        {

            String contrasena = "";
            String contrasenaCifrada = "";

            CryptoWS.Crypto wsCrypto = new CryptoWS.Crypto();

            do
            {
                Random random = new Random(DateTime.Now.Millisecond);
                double a = random.NextDouble();

                if (a == 0 || a == 1)
                {
                    continue;
                }
                double b = a * 100000000;
                long asd = (long)b;
                contrasena = "" + asd;
            }
            while (contrasena.Length != 8);

            contrasenaCifrada = wsCrypto.encrypt(contrasena, claveEncriptada);
            return contrasenaCifrada;
        }



        public void PrepararPlantilla(string sourcePath, string savePath, string codigoUsuario)
        {

            ExcelApplication excelApplication = new ExcelApplication();
            ExcelWorkBook workBook = null;
            workBook = excelApplication.OpenWorkBook(sourcePath, ExcelMode.Full);

            ExcelWorkSheet sheet = workBook.GetSheet(2);
            ExcelWorkSheet sheetCucs = workBook.GetSheet(3);

            //Obtiene los prospectos vacios
            CargaContratoDA cargaContratoDA = new CargaContratoDA();
            DataTable dtProspectos = cargaContratoDA.ObtenerListaProspectosVacios();
            sheetCucs.EscribirDatatable("B", 2, dtProspectos);
            DataTable dtDISTRITO = cargaContratoDA.ObtenerListaDistritos();
            int indis = 2;
            foreach (DataRow dr in dtDISTRITO.Rows)
            {
                sheet.SetValue(string.Format("W{0}", indis), dr["NOMBRE"]);
                sheet.SetValue(string.Format("X{0}", indis), dr["ID"]);
                sheet.SetValue(string.Format("Y{0}", indis++), dr["CIUDAD"]);

            }

            FondoDA fondoDA = new FondoDA();
            DataTable dtTiposDocumentos = fondoDA.ObtenerLista("TPDOCS");

            int index = 2;

            foreach (DataRow dr in dtTiposDocumentos.Rows)
            {
                sheet.SetValue(string.Format("K{0}", index), dr["DESCRIPCION_LARGA"]);
                sheet.SetValue(string.Format("L{0}", index++), dr["LLAVE_TABLA"]);

            }


            index = 2;

            DataTable dtAgenciasOrigen = fondoDA.ObtenerLista("AGEORI");

            foreach (DataRow dr in dtAgenciasOrigen.Rows)
            {
                sheet.SetValue(string.Format("H{0}", index), dr["DESCRIPCION_LARGA"]);
                sheet.SetValue(string.Format("I{0}", index++), dr["LLAVE_TABLA"]);
            }


            index = 2;
            DataTable dtFondos = cargaContratoDA.ObtenerListaFondos();
            DataRow[] drFondos = dtFondos.Select("1=1", "NOMBRE");

            foreach (DataRow dr in drFondos)
            {
                sheet.SetValue(string.Format("E{0}", index), dr["NOMBRE"]);
                sheet.SetValue(string.Format("F{0}", index++), dr["ID"]);

            }

            sheet.Hide();

            excelApplication.GetWorkBook(1).SaveAs(savePath);
            workBook.Close();

            excelApplication.Close();


            workBook = excelApplication.OpenWorkBook(savePath, ExcelMode.Full);
            excelApplication.Show();



        }



    }
}
