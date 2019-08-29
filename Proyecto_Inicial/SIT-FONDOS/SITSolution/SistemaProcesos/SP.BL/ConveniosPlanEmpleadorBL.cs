	/*
 * Fecha Modificación:		16/09/2013
 * Modificado por:			Robert Castillo
 * Nro de OT:				5767
 * Descripción del cambio:	Se crea la clase BO para generar el archivo de Convenios para Plan Empleador.
 * */
/*
 * Fecha de Modificación:	24/09/2013
 * Modificado por:			Robert Castillo
 * Nro de OT:				5767
 * Descripción del cambio:	Se valida que el streamwriter que crea el archivo sea distinto de null para cerrarlo.
 * */
/*
 * Fecha de Modificación:	28/01/2014
 * Modificado por:			Giovana Veliz   
 * Nro de OT:				6134
 * Descripción del cambio:	Se mueven el orden de las columnas.
 * */
/*
 * Fecha de Modificación:	05/06/2014
 * Modificado por:			Robert Castillo   
 * Nro de OT:				6469
 * Descripción del cambio:	Se modificará el método generarArchivo, se leerá las hojas ocultas que contienen el 
 *                          listado del detalle de los convenios.
 * */
/*
 * Fecha de Modificación:	24/07/2014
 * Modificado por:			Giovana Veliz
 * Nro de OT:				6469 PSC002
 * Descripción del cambio:	Se modificará el método generarArchivo, se leerá las hojas ocultas que contienen el 
 *                          listado del detalle de los convenios y además de la visible para obtener el estado.
 * */
/*
 * Fecha de Modificación:	24/09/2014
 * Modificado por:			Leslie Valerio
 * Nro de OT:				OT 6891
 * Descripción del cambio:	Se modifico la lectura de arhivos que se encuentran en las otras ocultas deacuerdo a la OT '6469 PSC002' 
 *                          para que se pueda leer desde la hoja 1 y pueda tomar Vida Ahorro
 * */
/*
 * Fecha de Modificación:	28/04/2016
 * Modificado por:			Richard Valdez
 * Nro de OT:				OT 8372-PSC001
 * Descripción del cambio:	-Se homologa la OT8372
 *                          -Modificar el método generarArchivo(): Al recorrer cada hoja del archivo excel, almacenar el contenido 
 *                          dentro de un DataTable. Al terminar de recorrer una hoja, se debe procesar los registros agrupandolos por fondo, 
 *                          a fin de obtener un archivo plano (.txt) por fondo. Usar la descripción corta del fondo para el nombre de cada archivo plano (.txt). 
 *                          -Agregar los métodos generarArchivoXFondo(), generarArchivoUnico().
 *                          -Modificar los métodos generarArchivoXFondo(), generarArchivoUnico(): cambiar el formato del nombre de los archivos 
 *                          .txt generados.
 * */

using System;
using System.IO;
using System.Data;
using System.Text;
using Procesos.TD;
using Procesos.DA;
using INGFondos.Support.Interop;
using System.Windows.Forms;

//OT 8372 INI
using System.Linq;
//OT 8372 FIN

namespace Procesos.BL
{
    /// <summary>
    /// Descripción breve de ValorCuotaBL.
    /// </summary>
    public class ConveniosPlanEmpleadorBO
    {
        public ConveniosPlanEmpleadorBO()
        {
            //
            // TODO: agregar aquí la lógica del constructor
            //
        }

        //OT 8372-PSC001
        //public void generarArchivo(string rutaArchivo, string rutaArchivoDestino)
        public void generarArchivoXFondo(string rutaArchivo, string rutaArchivoDestino)
        {
            StringBuilder sbLinea;
            sbLinea = new StringBuilder("");

            //StreamWriter sw = null;

            string path = rutaArchivo;

            System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            ExcelApplication excelApplication = new ExcelApplication();
            ExcelWorkBook wb = excelApplication.OpenWorkBook(path, ExcelMode.Full);

            try
            {
                ConveniosPlanEmpleadorDA conveniosPlanEmpleadorDA = new ConveniosPlanEmpleadorDA();
                string codigosEmpresas = "COD_EMP_CON";
                DataTable dtCodigosEmpresas = conveniosPlanEmpleadorDA.ObtenerTablaGeneral(codigosEmpresas);
                int numeroEmpresas = dtCodigosEmpresas.Rows.Count;

                object[] row;
                int rowIndex;

                string codigoParticipacion;
                string nombreFondo;
                DataRow[] drFondo;
                DataTable dtFondosDepositos;
                DataRow[] drFondoDeposito;
                string idFondo;
                string codigoTablatablaGeneralDepositos = "FONDO_EQUIV";
                string mes = "";
                string anio = "";

                string estadoFondos = "ACT";
                DataTable dtFondos = conveniosPlanEmpleadorDA.ListarFondos(estadoFondos);

                //OT 8372 INI

                DataTable dtRegistrosHoja;//almacena el contenido de una hoja del excel

                #region Definir las columnas del DataTable dtRegistrosHoja

                dtRegistrosHoja = new DataTable();

                dtRegistrosHoja.Columns.Add("CODIGO_PARTICIPACION", typeof(string));
                dtRegistrosHoja.Columns.Add("NOMBRE_FONDO", typeof(string));
                dtRegistrosHoja.Columns.Add("MONTO_APORTE", typeof(decimal));

                #endregion

                //OT 8372 FIN

                if (numeroEmpresas == 0)
                {
                    //no hay empresas
                }
                else
                {
                    string periodo;
                    string[] periodoElementos;
                    string tipoMovimiento = "I";
                    string tipoAporte = ""; //"P"
                    double montoAporte;
                    string montoAporteCadena;
                    string nombreEmpresa;
                    bool existenRegistrosHoja = true;
                    string codigoEmpresa = "";
                    string constanteCP = "CP_";

                   // int numeroHojas = numeroEmpresas * 2 + 1;
                    int numeroHojas = 0;
                    //OT 6891 inicio
                    int cantidadHojas = wb.GetSheetCount();
                    if (cantidadHojas == 3)
                    {
                        numeroHojas = wb.GetSheetCount();
                    }
                    else
                    {
                        numeroHojas = numeroEmpresas * 2 + 1;

                    }

                    //for (int i = 1; i <= numeroEmpresas; i++)
                    for (int i = 1; i <= numeroHojas; i++)
                    {
                        //ExcelWorkSheet sheet = wb.GetSheet(i + 1);
                        //Toma a partir de la 4ta hoja porque las 3 primeras son plantillas vacías.
                        //ExcelWorkSheet sheet = wb.GetSheet(i + 1);
                        //Toma a partir de la 4ta hoja porque las 3 primeras son plantillas vacías.
                        // OT 6891
                        ExcelWorkSheet sheet = null;
                        if (cantidadHojas == 3)
                        {
                            sheet = wb.GetSheet(i);
                        }
                        else
                        {
                            sheet = wb.GetSheet(i + 1);

                        }
                        // OT 6891 fin

                        nombreEmpresa = sheet.getName();
                        

                        if (nombreEmpresa.StartsWith(constanteCP)){
                            //StreamWriter sw = new StreamWriter(rutaArchivoDestino + "\\" + "PCP_" + nombreEmpresa + "_" + anio + mes + ".txt");

                            

                            StreamWriter sw = null;
                            String estadoCabecera;

                            //if (i == 1)
                            //{
                            periodo = sheet.GetString("B8").Substring(4, 10);
                            periodoElementos = periodo.Split('/');

                            anio = periodoElementos[2].Trim();
                            mes = periodoElementos[1].Trim();
                            //}

						    codigoEmpresa = sheet.GetString("B1").Trim();
						    DataTable dtDetalleConvenio = conveniosPlanEmpleadorDA.ObtenerDetalleConvenios(codigoEmpresa, Convert.ToInt32(mes), Convert.ToInt32(anio));          

                            rowIndex = 13;//13
                            //row = sheet.GetRow(rowIndex++, 'B', 'J'); //OT 6134
                            row = sheet.GetRow(rowIndex++, 'B', 'E');
                           

                            int cont = 0;
                            
                            while (row[0] != null)
                            {
                                //if (row[8].ToString().Trim() == "EFE")
                                //if (row[3].ToString().Trim() == "EFE")
                                estadoCabecera = ObtienePestañaEmpresa(wb, nombreEmpresa.Substring(constanteCP.Length), numeroHojas, row[0].ToString().Substring(0,8));


                                if (estadoCabecera == "EFE")
                                {
                                    if (existenRegistrosHoja)
                                    {
                                        codigoEmpresa = sheet.GetString("B1").Trim();
                                        //OT 8372 INI
                                        //sw = new StreamWriter(rutaArchivoDestino + "\\" + "PCP_" + nombreEmpresa.Substring(3, nombreEmpresa.Length - 3) + "_" + codigoEmpresa + "_" + anio + mes + ".txt");
                                        //OT 8372 FIN
                                        existenRegistrosHoja = false;
                                    }

                                    //codigoParticipacion = row[1].ToString().Trim();
                                    codigoParticipacion = row[0].ToString().Trim();
                                    //nombreFondo = row[7].ToString().Trim();//OT 6134
                                    nombreFondo = row[1].ToString().Trim();
                                    //montoAporte = Convert.ToDouble(row[6].ToString().Trim()); //OT 6134
                                    montoAporte = Convert.ToDouble(row[2].ToString().Trim()); //OT 6134

                                    //OT 8372 INI

                                    #region Agregar un registro al DataTable dtRegistrosHoja

                                    DataRow newRegistroHoja = dtRegistrosHoja.NewRow();

                                    newRegistroHoja["CODIGO_PARTICIPACION"] = codigoParticipacion;
                                    newRegistroHoja["NOMBRE_FONDO"] = nombreFondo;
                                    newRegistroHoja["MONTO_APORTE"] = montoAporte;

                                    dtRegistrosHoja.Rows.Add(newRegistroHoja);

                                    #endregion
                                                                            
                                    #region Se comenta código: El procesamiento de los registros ahora se hará al terminar de leer los registros de toda la hoja

                                    /*
								    string filtro = "CODIGO = '" + codigoParticipacion + "' AND FONDO = '" + nombreFondo + "' AND MONTO_CONCEPTO = " + montoAporte.ToString();
								    DataRow[] dDetalleConvenio = dtDetalleConvenio.Select(filtro, "ID_CONVENIO");
								    //Decimal idConvenio =0;
                                    Decimal idConvenioDetalle = 0;
                                    foreach (DataRow dr in dDetalleConvenio)
								    {
									    //if (idConvenio == 0|| idConvenio > 0 && idConvenio == Convert.ToDecimal(dr["ID_CONVENIO"]))
                                        if (idConvenioDetalle == 0 || idConvenioDetalle > 0 && idConvenioDetalle == Convert.ToDecimal(dr["ID_CONVENIO_DETALLE"]))
                                        {
                                            idConvenioDetalle = Convert.ToDecimal(dr["ID_CONVENIO_DETALLE"]);

                                            sbLinea.Append(codigoParticipacion.Replace("-", "").Trim());                              
									        drFondo = dtFondos.Select("NOMBRE = '" + nombreFondo + "'");
									        dtFondosDepositos = conveniosPlanEmpleadorDA.ObtenerTablaGeneralDepositos(codigoTablatablaGeneralDepositos);
									        idFondo = drFondo[0]["ID"].ToString();
									        drFondoDeposito = dtFondosDepositos.Select("DESCRIPCION_CORTA = '" + idFondo + "'");
									        sbLinea.Append(drFondoDeposito[0]["LLAVE_TABLA"].ToString());
    									  
									        sbLinea.Append(anio + mes);
									        sbLinea.Append(tipoMovimiento);
    									  
									        tipoAporte = dr["CODIGO_CONCEPTO"].ToString().Trim();
									        sbLinea.Append(tipoAporte);
    									  
									        montoAporte = Convert.ToDouble(dr["MONTO_CONCEPTO"]);
									        montoAporteCadena = montoAporte.ToString("000000.00");
									        montoAporteCadena = montoAporteCadena.Replace(".", "");
									        sbLinea.Append(montoAporteCadena);
    									  
									        sw.WriteLine(sbLinea);
									        sbLinea.Length = 0;
									    }
									    else
										    break;
								    }
    								
							        for (int j = dtDetalleConvenio.Rows.Count - 1; j >= 0; --j)
							        {
								        DataRow drDetalleConvenio = dtDetalleConvenio.Rows[j];
								        if (idConvenioDetalle == Convert.ToDecimal(drDetalleConvenio["ID_CONVENIO_DETALLE"]))
								          dtDetalleConvenio.Rows.Remove(drDetalleConvenio);
							        }
                                    */
                                    #endregion
                                    //OT 8372 FIN
                                    
                                }

                                

                                //row = sheet.GetRow(rowIndex++, 'B', 'J');//OT 6134
                                row = sheet.GetRow(rowIndex++, 'B', 'E');
                                cont = cont + 1;
                            }

                            //OT 8372 INI

                            #region Procesar todos los registros de la hoja almacenadas en el DataTable dtRegistrosHoja

                            //Obtener los fondos que aparecen en el DataTable dtRegistrosHoja
                            var ListaFondos = (from DataRow dRow in dtRegistrosHoja.Rows
                                               select dRow["NOMBRE_FONDO"]).Distinct();

                            if (ListaFondos != null)
                            {
                                string filtroXNombreFondo;

                                foreach (var objNombreFondo in ListaFondos)
                                {
                                    filtroXNombreFondo = "NOMBRE_FONDO = '" + objNombreFondo + "'";
                                    DataRow[] dRegistrosXFondo = dtRegistrosHoja.Select(filtroXNombreFondo, "CODIGO_PARTICIPACION");

                                    //Crear un archivo plano .txt por cada fondo

                                    //OT 8372-PSC001 INICIO

                                    /*
                                    sw = new StreamWriter(rutaArchivoDestino + "\\" + "PCP_" + nombreEmpresa.Substring(3, nombreEmpresa.Length - 3)
                                        + "_" + codigoEmpresa + "_" + objNombreFondo + "_" + anio + mes + ".txt");
                                    */
                                    sw = new StreamWriter(rutaArchivoDestino + "\\" + "PCP_" + "F" + "_" + objNombreFondo + "._" 
                                        + nombreEmpresa.Substring(3, nombreEmpresa.Length - 3)
                                        + "_" + codigoEmpresa + "_" + anio + mes + ".txt");

                                    //OT 8372-PSC001 FIN

                                    foreach (var dRegistroXFondo in dRegistrosXFondo)
                                    {
                                        #region procesar un registro

                                        string filtro = "CODIGO = '" + dRegistroXFondo["CODIGO_PARTICIPACION"]
                                            + "' AND FONDO = '" + dRegistroXFondo["NOMBRE_FONDO"]
                                            + "' AND MONTO_CONCEPTO = " + dRegistroXFondo["MONTO_APORTE"].ToString();
                                        DataRow[] dDetalleConvenio = dtDetalleConvenio.Select(filtro, "ID_CONVENIO");
                                        //Decimal idConvenio =0;
                                        Decimal idConvenioDetalle = 0;
                                        foreach (DataRow dr in dDetalleConvenio)
                                        {
                                            //if (idConvenio == 0|| idConvenio > 0 && idConvenio == Convert.ToDecimal(dr["ID_CONVENIO"]))
                                            if (idConvenioDetalle == 0 || idConvenioDetalle > 0 && idConvenioDetalle == Convert.ToDecimal(dr["ID_CONVENIO_DETALLE"]))
                                            {
                                                idConvenioDetalle = Convert.ToDecimal(dr["ID_CONVENIO_DETALLE"]);

                                                sbLinea.Append(dRegistroXFondo["CODIGO_PARTICIPACION"].ToString().Replace("-", "").Trim());
                                                drFondo = dtFondos.Select("NOMBRE = '" + dRegistroXFondo["NOMBRE_FONDO"] + "'");
                                                dtFondosDepositos = conveniosPlanEmpleadorDA.ObtenerTablaGeneralDepositos(codigoTablatablaGeneralDepositos);
                                                idFondo = drFondo[0]["ID"].ToString();
                                                drFondoDeposito = dtFondosDepositos.Select("DESCRIPCION_CORTA = '" + idFondo + "'");
                                                sbLinea.Append(drFondoDeposito[0]["LLAVE_TABLA"].ToString());

                                                sbLinea.Append(anio + mes);
                                                sbLinea.Append(tipoMovimiento);

                                                tipoAporte = dr["CODIGO_CONCEPTO"].ToString().Trim();
                                                sbLinea.Append(tipoAporte);

                                                montoAporte = Convert.ToDouble(dr["MONTO_CONCEPTO"]);
                                                montoAporteCadena = montoAporte.ToString("000000.00");
                                                montoAporteCadena = montoAporteCadena.Replace(".", "");
                                                sbLinea.Append(montoAporteCadena);

                                                sw.WriteLine(sbLinea);
                                                sbLinea.Length = 0;
                                            }
                                            else
                                                break;
                                        }

                                        for (int j = dtDetalleConvenio.Rows.Count - 1; j >= 0; --j)
                                        {
                                            DataRow drDetalleConvenio = dtDetalleConvenio.Rows[j];
                                            if (idConvenioDetalle == Convert.ToDecimal(drDetalleConvenio["ID_CONVENIO_DETALLE"]))
                                                dtDetalleConvenio.Rows.Remove(drDetalleConvenio);
                                        }

                                        #endregion
                                    }

                                    if (sw != null)
                                    {
                                        sw.Close();
                                    }
                                }

                                dtRegistrosHoja.Rows.Clear();
                            }

                            #endregion

                            //OT 8372 FIN

                            if (cont>0)
                            {
                                if (sw != null)
                                {
                                    sw.Close();
                                }
                            }
                            existenRegistrosHoja = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                wb.Close();
                excelApplication.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
            }

        }

        //OT 8372-PSC001
        public void generarArchivoUnico(string rutaArchivo, string rutaArchivoDestino)
        {
            StringBuilder sbLinea;
            sbLinea = new StringBuilder("");

            //StreamWriter sw = null;

            string path = rutaArchivo;

            System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            ExcelApplication excelApplication = new ExcelApplication();
            ExcelWorkBook wb = excelApplication.OpenWorkBook(path, ExcelMode.Full);

            try
            {
                ConveniosPlanEmpleadorDA conveniosPlanEmpleadorDA = new ConveniosPlanEmpleadorDA();
                string codigosEmpresas = "COD_EMP_CON";
                DataTable dtCodigosEmpresas = conveniosPlanEmpleadorDA.ObtenerTablaGeneral(codigosEmpresas);
                int numeroEmpresas = dtCodigosEmpresas.Rows.Count;

                object[] row;
                int rowIndex;

                string codigoParticipacion;
                string nombreFondo;
                DataRow[] drFondo;
                DataTable dtFondosDepositos;
                DataRow[] drFondoDeposito;
                string idFondo;
                string codigoTablatablaGeneralDepositos = "FONDO_EQUIV";
                string mes = "";
                string anio = "";

                string estadoFondos = "ACT";
                DataTable dtFondos = conveniosPlanEmpleadorDA.ListarFondos(estadoFondos);

                if (numeroEmpresas == 0)
                {
                    //no hay empresas
                }
                else
                {
                    string periodo;
                    string[] periodoElementos;
                    string tipoMovimiento = "I";
                    string tipoAporte = ""; //"P"
                    double montoAporte;
                    string montoAporteCadena;
                    string nombreEmpresa;
                    bool existenRegistrosHoja = true;
                    string codigoEmpresa = "";
                    string constanteCP = "CP_";

                    // int numeroHojas = numeroEmpresas * 2 + 1;
                    int numeroHojas = 0;
                    //OT 6891 inicio
                    int cantidadHojas = wb.GetSheetCount();
                    if (cantidadHojas == 3)
                    {
                        numeroHojas = wb.GetSheetCount();
                    }
                    else
                    {
                        numeroHojas = numeroEmpresas * 2 + 1;

                    }

                    //for (int i = 1; i <= numeroEmpresas; i++)
                    for (int i = 1; i <= numeroHojas; i++)
                    {
                        //ExcelWorkSheet sheet = wb.GetSheet(i + 1);
                        //Toma a partir de la 4ta hoja porque las 3 primeras son plantillas vacías.
                        //ExcelWorkSheet sheet = wb.GetSheet(i + 1);
                        //Toma a partir de la 4ta hoja porque las 3 primeras son plantillas vacías.
                        // OT 6891
                        ExcelWorkSheet sheet = null;
                        if (cantidadHojas == 3)
                        {
                            sheet = wb.GetSheet(i);
                        }
                        else
                        {
                            sheet = wb.GetSheet(i + 1);

                        }
                        // OT 6891 fin

                        nombreEmpresa = sheet.getName();


                        if (nombreEmpresa.StartsWith(constanteCP))
                        {
                            //StreamWriter sw = new StreamWriter(rutaArchivoDestino + "\\" + "PCP_" + nombreEmpresa + "_" + anio + mes + ".txt");



                            StreamWriter sw = null;
                            String estadoCabecera;

                            //if (i == 1)
                            //{
                            periodo = sheet.GetString("B8").Substring(4, 10);
                            periodoElementos = periodo.Split('/');

                            anio = periodoElementos[2].Trim();
                            mes = periodoElementos[1].Trim();
                            //}

                            codigoEmpresa = sheet.GetString("B1").Trim();
                            DataTable dtDetalleConvenio = conveniosPlanEmpleadorDA.ObtenerDetalleConvenios(codigoEmpresa, Convert.ToInt32(mes), Convert.ToInt32(anio));

                            rowIndex = 13;//13
                            //row = sheet.GetRow(rowIndex++, 'B', 'J'); //OT 6134
                            row = sheet.GetRow(rowIndex++, 'B', 'E');


                            int cont = 0;

                            while (row[0] != null)
                            {
                                //if (row[8].ToString().Trim() == "EFE")
                                //if (row[3].ToString().Trim() == "EFE")
                                estadoCabecera = ObtienePestañaEmpresa(wb, nombreEmpresa.Substring(constanteCP.Length), numeroHojas, row[0].ToString().Substring(0, 8));


                                if (estadoCabecera == "EFE")
                                {
                                    if (existenRegistrosHoja)
                                    {
                                        codigoEmpresa = sheet.GetString("B1").Trim();

                                        //OT 8372-PSC001 INICIO

                                        //sw = new StreamWriter(rutaArchivoDestino + "\\" + "PCP_" + nombreEmpresa.Substring(3, nombreEmpresa.Length - 3) + "_" + codigoEmpresa + "_" + anio + mes + ".txt");
                                        sw = new StreamWriter(rutaArchivoDestino + "\\" + "PCP_" + "T" + "_" 
                                                + nombreEmpresa.Substring(3, nombreEmpresa.Length - 3) + "_" + codigoEmpresa + "_" + anio + mes + ".txt");

                                        //OT 8372-PSC001 FIN
                                        
                                        existenRegistrosHoja = false;
                                    }

                                    //codigoParticipacion = row[1].ToString().Trim();
                                    codigoParticipacion = row[0].ToString().Trim();
                                    //nombreFondo = row[7].ToString().Trim();//OT 6134
                                    nombreFondo = row[1].ToString().Trim();
                                    //montoAporte = Convert.ToDouble(row[6].ToString().Trim()); //OT 6134
                                    montoAporte = Convert.ToDouble(row[2].ToString().Trim()); //OT 6134

                                    string filtro = "CODIGO = '" + codigoParticipacion + "' AND FONDO = '" + nombreFondo + "' AND MONTO_CONCEPTO = " + montoAporte.ToString();
                                    DataRow[] dDetalleConvenio = dtDetalleConvenio.Select(filtro, "ID_CONVENIO");
                                    //Decimal idConvenio =0;
                                    Decimal idConvenioDetalle = 0;
                                    foreach (DataRow dr in dDetalleConvenio)
                                    {
                                        //if (idConvenio == 0|| idConvenio > 0 && idConvenio == Convert.ToDecimal(dr["ID_CONVENIO"]))
                                        if (idConvenioDetalle == 0 || idConvenioDetalle > 0 && idConvenioDetalle == Convert.ToDecimal(dr["ID_CONVENIO_DETALLE"]))
                                        {
                                            idConvenioDetalle = Convert.ToDecimal(dr["ID_CONVENIO_DETALLE"]);

                                            sbLinea.Append(codigoParticipacion.Replace("-", "").Trim());
                                            drFondo = dtFondos.Select("NOMBRE = '" + nombreFondo + "'");
                                            dtFondosDepositos = conveniosPlanEmpleadorDA.ObtenerTablaGeneralDepositos(codigoTablatablaGeneralDepositos);
                                            idFondo = drFondo[0]["ID"].ToString();
                                            drFondoDeposito = dtFondosDepositos.Select("DESCRIPCION_CORTA = '" + idFondo + "'");
                                            sbLinea.Append(drFondoDeposito[0]["LLAVE_TABLA"].ToString());

                                            sbLinea.Append(anio + mes);
                                            sbLinea.Append(tipoMovimiento);

                                            tipoAporte = dr["CODIGO_CONCEPTO"].ToString().Trim();
                                            sbLinea.Append(tipoAporte);

                                            montoAporte = Convert.ToDouble(dr["MONTO_CONCEPTO"]);
                                            montoAporteCadena = montoAporte.ToString("000000.00");
                                            montoAporteCadena = montoAporteCadena.Replace(".", "");
                                            sbLinea.Append(montoAporteCadena);

                                            sw.WriteLine(sbLinea);
                                            sbLinea.Length = 0;
                                        }
                                        else
                                            break;
                                    }

                                    for (int j = dtDetalleConvenio.Rows.Count - 1; j >= 0; --j)
                                    {
                                        DataRow drDetalleConvenio = dtDetalleConvenio.Rows[j];
                                        if (idConvenioDetalle == Convert.ToDecimal(drDetalleConvenio["ID_CONVENIO_DETALLE"]))
                                            dtDetalleConvenio.Rows.Remove(drDetalleConvenio);
                                    }

                                }

                                //row = sheet.GetRow(rowIndex++, 'B', 'J');//OT 6134
                                row = sheet.GetRow(rowIndex++, 'B', 'E');
                                cont = cont + 1;
                            }

                            if (cont > 0)
                            {
                                if (sw != null)
                                {
                                    sw.Close();
                                }
                            }
                            existenRegistrosHoja = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                wb.Close();
                excelApplication.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
            }

        }


        public String ObtienePestañaEmpresa(ExcelWorkBook wb, string nombreEmpresa, int numeroHojas, string cuc)
        {
            string nombreSheet;
            int rowIndex;
            object[] row;
            rowIndex = 13;//13           

            for (int i = 1; i <= numeroHojas; i++)
            {
                //ot6891 inicio
                ExcelWorkSheet sheet = wb.GetSheet(i + 1);
                //ot6891 fin
                nombreSheet = sheet.getName();
                if (nombreSheet.Equals(nombreEmpresa)){
                                        
                    row = sheet.GetRow(rowIndex++, 'B', 'O');

                    while (row[0] != null)
                    {
                        if (row[0].ToString().Trim().Equals(cuc))
                        {
                            return row[7].ToString().Trim();
                        }
                        row = sheet.GetRow(rowIndex++, 'B', 'O');
                    }


                }

            }
            return "";
        }

    }

   



}