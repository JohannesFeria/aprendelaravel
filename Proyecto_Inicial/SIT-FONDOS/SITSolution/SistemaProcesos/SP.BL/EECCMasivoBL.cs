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
 * Fecha de Modificación: 19/09/2013
 * Modificado por: Giovana Veliz
 * Numero de OT: 5803
 * Descripción del cambio: Se agregó las columnas respectivas para los nuevos fondos estratégicos.
 * */
/*
 * Fecha de Modificación:   17/10/2013
 * Modificado por:          Robert Castillo
 * Numero de OT:            5924
 * Descripción del cambio: Se modifica el método SaveToFile para que escriba en los archivos de texto
 *                         los nombres de los meses con la primera letra en mayúsculas.
 * */
 /*
 * Fecha de Modificación:   23/01/2014
 * Modificado por:          Giovana Veliz
 * Numero de OT:            6134
 * Descripción del cambio: Se modifica el método SaveToFile para escribir los campos correspondientes a los 
 *                          fondos extranjeros (ACCIONES_USA y ACCIONES_EME)
 * */
/*
 * Fecha de Modificación:   25/03/2014
 * Modificado por:          Giovana Veliz
 * Numero de OT:            6304
 * Descripción del cambio: Se modificará el método InsertarDetallesPie, se agregará el parámetro FlagAprobado.
 * */
/*
* Fecha de Modificación:   08/07/2014
* Modificado por:          Giovana Veliz
* Numero de OT:            6590 
* Descripción del cambio: Se modifica el método SaveToFile para escribir los campos correspondientes al
*                          fondo Acciones Europeas
* */
/*
* Fecha de Modificación:   25/08/2014
* Modificado por:          Giovana Veliz
* Numero de OT:            6590 PSC001
* Descripción del cambio: Se agrega la sumatoria del fondo ACCEUR en la seccion de Saldos Dólares.
* */
/*
* Fecha de Modificación:   19/10/2015
* Modificado por:          Robert Castillo
* Numero de OT:            7833
* Descripción del cambio:  Se modifica el método SaveToFile para escribir los campos correspondientes al fondo Estructurado Soles.
* */
/*
* Fecha de Modificación:   02/02/2016
* Modificado por:          Irene Reyes
* Numero de OT:            8365
* Descripción del cambio:  Se modifica la clase para agregar el nuevo fondo SURA DEPOSITOS I DOLARES.
* */
/*
* Fecha de Modificación:   07/10/2016
* Modificado por:          Robert Castillo
* Numero de OT:            9426
* Descripción del cambio:  Realizar los siguientes cambios:
    1. En el método SaveToFile en el bucle que escribe los datos asociados a los saldos de los fondos, se incrementa en 2 la 
    cantidad de registros a analizar (por los 2 nuevos fondos).
    2. En el método SaveToFile agregar el código para escribir los datos de los 2 nuevos fondos en el archivo de EECC masivo.
* 
*/
/*
* Fecha de Modificación:   20/10/2016
* Modificado por:          Irene Reyes 
* Numero de OT:            8954
* Descripción del cambio:  Homologación
* 
*/
/* 
* Fecha Modificación:		10/11/2016
* Modificado por:			Robert Castillo
* Nro de OT:				9488
* Descripción del cambio:	Se añade método para EECC masivo de fondos FIR.
*/
/* 
* Fecha Modificación:		25/11/2016
* Modificado por:			Robert Castillo
* Nro de OT:				9613
* Descripción del cambio:	En el método SaveToFile, se reemplazan los datos del fondo con identificador 
 *                          23 por los datos del fondo con identificador 44.
*/
/* 
* Fecha Modificación:		09/01/2017
* Modificado por:			Irene Reyes
* Nro de OT:				9772
* Descripción del cambio:	Modificar clase para agregar métodos que permitan generar los archivos planos y archivos de Excel para                                fondos privado.
*/
/* 
* Fecha Modificación:		31/01/2017
* Modificado por:			Irene Reyes
* Nro de OT:				9772 - PSC001
* Descripción del cambio:	Modificar el cálculo de los totales de los movimientos de los partícipes.
 *                          Se modifican los métodos SaveToFilePWD_PRI y ObtenerDataEECC_PRI.
*/
/* 
* Fecha Modificación    :	21/03/2017
* Modificado por        :	Anthony Joaquin
* Nro de OT             :	10082
* Descripción del cambio:	Homologación.
*/
/* 
* Fecha Modificación    :	12/05/2017
* Modificado por        :	Anthony Joaquin
* Nro de OT             :	10217 - PSC001
* Descripción del cambio:	Homologación.
*/
/* 
* Fecha Modificación    :   22/09/2017
* Modificado por        :	Irene Reyes
* Nro de OT             :	10546
* Descripción del cambio:	Modificar el método SaveToFile_PRI para agregar los fondos privados personalizados con sus respectivos saldos y movimientos por partícipe
*/
/* 
* Fecha Modificación    :   12/10/2017
* Modificado por        :	Irene Reyes
* Nro de OT             :	10546 PSC001
* Descripción del cambio:	Agregar filtros para los fondo globales
*/
/* 
* Fecha Modificación    :   13/11/2017
* Modificado por        :	Robert Castillo
* Nro de OT             :	10913
* Descripción del cambio:	Se agregan filtros en el método SaveToFile_PRI para convertir al tipo decimal valores
 *							numéricos.
*/

using System;
using System.Data;
using Procesos.TD;
using Procesos.DA;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Procesos.BL
{
    /// <summary>
    /// Descripción breve de PrecierreBL.
    /// </summary>
    public class EECCMasivoBO
    {
        private string codigoUsuario;

        public EECCMasivoBO(string codigoUsuario)
        {
            this.codigoUsuario = codigoUsuario;
        }
        public EECCMasivoBO() { }

        public struct Constantes
        {
            public static string FormatoCuotas = "###,##0.0000000";
            public static string FormatoValorCuota = "###,##0.0000000";
            public static string FormatoMonto = "###,##0.00";
        }

        private decimal idFondo;
        private string fondo;
        private string moneda;
        private DateTime fechaInicio;
        private DateTime fechaFin;

        private StreamWriter sw;

        public decimal IdFondo
        {
            set
            {
                idFondo = value;
            }
            get
            {
                return idFondo;
            }
        }

        public string Fondo
        {
            set
            {
                fondo = value;
            }
        }
        public string Moneda
        {
            set
            {
                moneda = value;
            }
        }
        public DateTime FechaInicio
        {
            set
            {
                fechaInicio = value;
            }
        }

        public DateTime FechaFin
        {
            set
            {
                fechaFin = value;
            }
        }


        /// <summary>
        /// Escribe en el StreamWriter sw el texto ajustándolo a la longitud establecida
        /// </summary>
        /// <param name="texto">Texto que se escribirá en el archivo</param>
        /// <param name="longitud">Longitud a la que se trunca el texto</param>
        private void Escribir(string texto, int longitud)
        {
            string valor = texto.PadRight(longitud).Substring(0, longitud);
            sw.Write(valor);
        }

        /// <summary>
        /// Escribe la cantidad de espacios en blanco determinada por la longitud
        /// </summary>
        /// <param name="longitud">Indica el número de espacios en blanco que se escribirán</param>
        private void EscribirVacio(int longitud)
        {
            Escribir(string.Empty, longitud);
        }

        /// <summary>
        /// Escribe un número entero en el archivo plano ajustándolo a la longitud establecida
        /// </summary>
        /// <param name="entero">Número que se escribirá en el archivo plano</param>
        /// <param name="longitud">Indica la longitud que debe completar el entero en el archivo plano</param>
        private void Escribir(int entero, int longitud)
        {
            string valor = entero.ToString().PadRight(longitud);
            sw.Write(valor);
        }

        /// <summary>
        /// Escribe un número decimal en el archivo plano ajustándolo a la longitud establecida y en el formato establecido
        /// </summary>
        /// <param name="numero">Número que se escribirá en el archivo plano</param>
        /// <param name="longitud">Indica la longitud que debe completar el número en el archivo plano</param>
        /// <param name="formato">Indica el formato con el cual se escribirá el número como texto</param>
        private void Escribir(decimal numero, int longitud, string formato)
        {
            string valor = numero.ToString(formato).PadRight(longitud);
            sw.Write(valor);
        }

        /// <summary>
        /// Escribe una fecha en el archivo plano ajustándola a la longitud establecida y en el formato establecido
        /// </summary>
        /// <param name="fecha">Fecha que se escribirá en el archivo plano</param>
        /// <param name="longitud">Indica la longitud que debe completar la fecha en el archivo plano</param>
        /// <param name="formato">Indica el formato con el cual se escribirá la fecha como texto</param>
        private void Escribir(DateTime fecha, int longitud, string formato)
        {
            string valor = fecha.ToString(formato).PadRight(longitud);
            sw.Write(valor);
        }

        /// <summary>
        /// Inserta un retorno de carro en el archivo plano
        /// </summary>
        private void CambiarLinea()
        {
            sw.Write("\r\n");
        }

        /// <summary>
        /// Obtiene los fondos registrados en el sistema
        /// </summary>
        /// <returns>DataTable: Tabla con los datos de los diferentes fondos registrados</returns>
        public DataTable ObtenerFondos()
        {
            EECCMasivoDA ec = new EECCMasivoDA();
            DataTable dtFondos = ec.ObtenerFondos();
            return dtFondos;
        }

          /// <summary>
        /// Permite generar el archivo plano que contiene los EECC que serán impresos, así como el archivo de cargo
        /// </summary>
        /// <param name="path">Ruta en la cual se guardarán el archivo de cargo como el archivo plano</param>
        public void SaveToFile_FIR(string path, string strTipoEECC, int indicadorMasiva)
        {
            sw = new StreamWriter(path, false, Encoding.Default);
            try
            {
                EECCMasivoDA ec = new EECCMasivoDA();
                DataSet dsEECC = ec.ObtenerDataEECC_FIR(fechaInicio, fechaFin, strTipoEECC, indicadorMasiva);
                DataTable dtParticipes = dsEECC.Tables["PARTICIPES"];
                DataTable dtMancomunos = dsEECC.Tables["MANCOMUNOS"];
                DataTable dtSaldos = dsEECC.Tables["SALDOS"];
                DataTable dtMovimientos = dsEECC.Tables["MOVIMIENTOS"];
                DataTable dtSerie = dsEECC.Tables["SERIE"];
               

                //Se usa una tabla auxiliar para ordenar los fondos
				//OT10546 INI
                //DataTable saldosAux = dtSaldos.Clone();
				//OT10546 FIN

                int mes = fechaFin.Month;
                int anio = fechaInicio.Year;
                string mensaje = "";
                string titulo = "";
                string piepagina = "";
                string pie1 = "";
                string pie2 = "";
                string pie3 = "";
                string pie4 = "";
                string pie5 = "";
                string pie6 = "";

                //Se obtiene el mensaje de acuerdo al periodo seleccionado en el formulario
                DataTable dtMensaje = ec.ObtenerMensajexPeriodo(mes, anio);
                if (dtMensaje.Rows.Count > 0)
                {
                    titulo = dtMensaje.Rows[0]["TITULO"].ToString().Trim();
                    mensaje = dtMensaje.Rows[0]["MENSAJE"].ToString().Trim();
                    piepagina = dtMensaje.Rows[0]["PIE"].ToString().Trim();
                }
                 
                //Se obtienen los pies de paginas
                DataTable dtPies = ec.ObtenerPiesPaginaxPeriodo(mes, anio);

                if (dtPies.Rows.Count > 0)
                {
                    pie1 = dtPies.Rows[0].ItemArray[3].ToString().Trim();
                    switch (dtPies.Rows.Count)
                    {
                        case 2: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            break;
                        case 3: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            pie3 = dtPies.Rows[2].ItemArray[3].ToString();
                            break;
                        case 4: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            pie3 = dtPies.Rows[2].ItemArray[3].ToString();
                            pie4 = dtPies.Rows[3].ItemArray[3].ToString();
                            break;
                        case 5: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            pie3 = dtPies.Rows[2].ItemArray[3].ToString();
                            pie4 = dtPies.Rows[3].ItemArray[3].ToString();
                            pie5 = dtPies.Rows[4].ItemArray[3].ToString();
                            break;
                        case 6: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            pie3 = dtPies.Rows[2].ItemArray[3].ToString();
                            pie4 = dtPies.Rows[3].ItemArray[3].ToString();
                            pie5 = dtPies.Rows[4].ItemArray[3].ToString();
                            pie6 = dtPies.Rows[5].ItemArray[3].ToString();
                            break;
                    }
                }

                StringBuilder sb = new StringBuilder();

                foreach (DataRow drParticipe in dtParticipes.Rows)
                {
                    decimal idParticipe = Convert.ToDecimal(drParticipe["IDP"]);
                    //foreach (fondos del participe)
                    string filtroParticipeFondos = string.Format("id_participe = {0}", idParticipe);

                    //DataRow[] drParticipeFondos = dtSaldos.Select(filtroParticipeFondos);
                    DataRow[] drParticipeFondos = dtSaldos.Select(filtroParticipeFondos,"id_fondo");

                    foreach (DataRow drParticipeFondo in drParticipeFondos)
                    {
                        decimal totalSaldoInicialFondos = 0;
                        decimal totalSaldoFinalFondos = 0;
                        decimal totalSuscripciones = 0;
                        decimal totalRescates = 0;
                        decimal totalPagoFlujos = 0;
                        decimal totalTraspasosFavor = 0;
                        decimal totalTraspasosContra = 0;
                        int indiceBucle = 0;

                        //decimal idParticipe = Convert.ToDecimal(drParticipe["IDP"]);
                        string filtro = string.Format("id_participe = {0}", idParticipe);
                        DataRow[] drMancomunos = dtMancomunos.Select(filtro);

                        string mesTexto = fechaFin.ToString("MMMMMMMMMMMMMM");
                        string fechaTexto = mesTexto.Substring(0, 1).ToUpper() + mesTexto.Substring(1) + " de " + fechaFin.ToString("yyyy"); //Auxiliar para formatos dificiles de Fecha

                        string comentario = drParticipe["comentario"].ToString().Trim().Replace("\r", "").Replace("\n", "");

                        #region Datos del participe

                        Escribir(fechaTexto, 20); //campo1
                        Escribir(drParticipe["codigo"].ToString().Trim(), 8); //campo2
                        Escribir(drParticipe["titulo"].ToString().Trim(), 10); //campo3
                        Escribir(drParticipe["nombre"].ToString().Trim(), 100); //campo4

                        while (indiceBucle < 3)
                        {
                            if (drMancomunos.Length > indiceBucle)
                            {
                                Escribir(drMancomunos[indiceBucle]["macomuno"].ToString().Trim(), 100); //campo5,6,7
                            }
                            else
                            {
                                EscribirVacio(100); //campo5,6,7
                            }
                            indiceBucle++;
                        }

                        Escribir(drParticipe["direccion"].ToString().Trim().Replace("\r", "").Replace("\n", ""), 100); //campo8
                        Escribir(drParticipe["urb"].ToString().Trim().Replace("\r", "").Replace("\n", ""), 100); //campo9
                        Escribir(drParticipe["dpto"].ToString().Trim(), 30); //campo10
                        Escribir(drParticipe["ciudad"].ToString().Trim(), 30); //campo11
                        Escribir(drParticipe["distrito"].ToString().Trim(), 30); //campo12

                        #endregion

                        mesTexto = fechaInicio.ToString("MMMMMMMMMMMMMM");
                        mesTexto = mesTexto.Substring(0, 1).ToUpper() + mesTexto.Substring(1);
                        fechaTexto = fechaInicio.ToString("dd") + " de " + mesTexto + " de " + fechaInicio.ToString("yyyy");
                        Escribir(fechaTexto, 25); //campo13
                        mesTexto = fechaFin.ToString("MMMMMMMMMMMMMM");
                        mesTexto = mesTexto.Substring(0, 1).ToUpper() + mesTexto.Substring(1);
                        fechaTexto = fechaFin.ToString("dd") + " de " + mesTexto + " de " + fechaFin.ToString("yyyy");
                        Escribir(fechaTexto, 25); //campo14

                        //Serie
                        decimal idFondo = Convert.ToDecimal(drParticipeFondo["id_fondo"]);
                        string filtroSerieFondo = string.Format("ID = {0}", idFondo);
                        DataRow[] drSerieFondo = dtSerie.Select(filtroSerieFondo);
                        DataRow serieFondo = null;
                        if (drSerieFondo.Length > 0)
                        {
                            serieFondo = drSerieFondo[0];
                        }
                        if (serieFondo != null && serieFondo["SERIE"].ToString() != null)
                        {
                            Escribir(serieFondo["SERIE"].ToString(), 1); //campo15
                        }
                  

                        #region SALDOS
                        filtro = string.Format("id_participe = {0} and id_fondo = {1}", idParticipe,idFondo);
                        DataRow[] drSaldos = dtSaldos.Select(filtro);
                        if (drSaldos.Length > 0)
                        {
                            if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo16
                            }
                            else
                            {
                                EscribirVacio(20);
                            }

                            if (drSaldos[0]["valorcini"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo17
                            }
                            else
                            {
                                EscribirVacio(13);
                            }
                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo18
                            }
                            else
                            {
                                EscribirVacio(30);
                            }

                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                            }

                            if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo19
                            }
                            else
                            {
                                EscribirVacio(20);
                            }
                            if (drSaldos[0]["valorcfin"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo20
                            }
                            else
                            {
                                EscribirVacio(13);
                            }
                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo21
                            }
                            else
                            {
                                EscribirVacio(30);
                            }
                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                totalSaldoFinalFondos += Convert.ToDecimal(drSaldos[0]["montofin"]);
                            }
                        }
                        else {
                            EscribirVacio(20); //campo16
                            EscribirVacio(13); //campo17
                            EscribirVacio(30); //campo18
                            EscribirVacio(20); //campo19
                            EscribirVacio(13); //campo20
                            EscribirVacio(30); //campo21
                        }

                        if (drSaldos.Length > 0)
                        {
                            Escribir(totalSaldoInicialFondos, 30, Constantes.FormatoMonto); //campo22
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos.Length > 0)
                        {
                            Escribir(totalSaldoFinalFondos, 30, Constantes.FormatoMonto); //campo23
                        }
                        else
                        {
                            EscribirVacio(30);
                        }

                        #endregion

                        #region MOVIMIENTOS

                        DataView dvMovSoles = new DataView(dtMovimientos);
                        filtro = string.Format("id_participe = {0} and fondo = {1}", idParticipe,idFondo);
                        string sOrden = "fecha,fondo,id_operacion";
                        dvMovSoles.Sort = sOrden;
                        dvMovSoles.RowFilter = filtro;
                        DataRow[] drMovimientos = new DataRow[dvMovSoles.Count];
                        int i = 0;
                        foreach (DataRowView drv in dvMovSoles)
                        {
                            drMovimientos[i] = drv.Row;
                            i++;
                        }

                        indiceBucle = 0;
                        totalSuscripciones = 0;
                        totalRescates = 0;

                        while (indiceBucle < 15) {
                            if (drMovimientos.Length > indiceBucle)
                            {
                                Escribir(Convert.ToDateTime(drMovimientos[indiceBucle]["fecha"]), 10, "dd/MM/yyyy"); //campo24
                                Escribir(drMovimientos[indiceBucle]["nombrefondo"].ToString().Trim(), 30); //campo25
                                Escribir(drMovimientos[indiceBucle]["descripcion"].ToString().Trim(), 15); //campo26
                                if (drMovimientos[indiceBucle]["cuotas"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["cuotas"]), 20, Constantes.FormatoCuotas);  //campo27
                                }
                                else
                                {
                                    EscribirVacio(20); //campo27
                                }
                                if (drMovimientos[indiceBucle]["valor_cuota"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["valor_cuota"]), 13, Constantes.FormatoValorCuota); //campo28
                                }
                                else
                                {
                                    EscribirVacio(13); //campo28
                                }
                                if (drMovimientos[indiceBucle]["monto"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]), 30, Constantes.FormatoMonto); //campo29
                                }
                                else
                                {
                                    EscribirVacio(30); //campo29
                                }
                                if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("SUS"))
                                {
                                    totalSuscripciones += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                                }
                                if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("RES"))
                                {
                                    totalRescates += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                                }
                                if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("FLU"))
                                {
                                    totalPagoFlujos += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                                }
                                //TPS a favor
                                if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("TPS") && Convert.ToDecimal(drMovimientos[indiceBucle]["cuotas"]) > 0)
                                {
                                    totalTraspasosFavor += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                                }
                                //TPS en contra
                                if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("TPS") && Convert.ToDecimal(drMovimientos[indiceBucle]["cuotas"]) < 0)
                                {
                                    totalTraspasosContra += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                                }
                            }
                            else {
                                EscribirVacio(10); //campo24
                                EscribirVacio(30); //campo25
                                EscribirVacio(15); //campo26
                                EscribirVacio(20); //campo27
                                EscribirVacio(13); //campo28
                                EscribirVacio(30); //campo29
                            }
                            indiceBucle++;
                        }

                        if (drMovimientos.Length > 0)
                        {
                            Escribir(totalSuscripciones, 30, Constantes.FormatoMonto); //campo88
                            Escribir(totalRescates, 30, Constantes.FormatoMonto); //campo89
                            Escribir(totalPagoFlujos, 30, Constantes.FormatoMonto); //campo90
                            Escribir(totalTraspasosFavor, 30, Constantes.FormatoMonto); //campo91
                            Escribir(totalTraspasosContra, 30, Constantes.FormatoMonto); //campo92
                        }
                        else
                        {
                            EscribirVacio(30); //campo88
                            EscribirVacio(30); //campo89
                            EscribirVacio(30); //campo90
                            EscribirVacio(30); //campo91
                            EscribirVacio(30); //campo92
                        }
                        #endregion

                       

                        CambiarLinea();
                    }
                }
            }
            catch(Exception e){
                throw e;
            }
            finally
            {
                sw.Close();
            }
        }
        

        //INI OT9772
        /// <summary>
        /// Permite generar el archivo plano que contiene los EECC que serán impresos, así como el archivo de cargo
        /// </summary>
        /// <param name="path">Ruta en la cual se guardarán el archivo de cargo como el archivo plano de fondos privados</param>
        public void SaveToFile_PRI(string path, string strTipoEECC, int indicadorMasiva)
        {
            sw = new StreamWriter(path, false, Encoding.Default);
            try
            {
                EECCMasivoDA ec = new EECCMasivoDA();
                DataSet dsEECC = ec.ObtenerDataEECC_PRI(fechaInicio, fechaFin, strTipoEECC, indicadorMasiva);
                DataTable dtParticipes = dsEECC.Tables["PARTICIPES"];
                DataTable dtMancomunos = dsEECC.Tables["MANCOMUNOS"];
                DataTable dtSaldos = dsEECC.Tables["SALDOS"];
                DataTable dtMovimientos = dsEECC.Tables["MOVIMIENTOS"];
                DataTable dtSerie = dsEECC.Tables["SERIE"];
                DataTable dtFondo = dsEECC.Tables["FONDO"];

                //Se usa una tabla auxiliar para ordenar los fondos
				//OT10546 INI
               // DataTable saldosAux = dtSaldos.Clone();
			   //10546 FIN

                int mes = fechaFin.Month;
                int anio = fechaInicio.Year;
                string mensaje = "";
                string titulo = "";
                string piepagina = "";
                string pie1 = "";
                string pie2 = "";
                string pie3 = "";
                string pie4 = "";
                string pie5 = "";
                string pie6 = "";

                //Se obtiene el mensaje de acuerdo al periodo seleccionado en el formulario
                DataTable dtMensaje = ec.ObtenerMensajexPeriodo(mes, anio);
                if (dtMensaje.Rows.Count > 0)
                {
                    titulo = dtMensaje.Rows[0]["TITULO"].ToString().Trim();
                    mensaje = dtMensaje.Rows[0]["MENSAJE"].ToString().Trim();
                    piepagina = dtMensaje.Rows[0]["PIE"].ToString().Trim();
                }

                //Se obtienen los pies de paginas
                DataTable dtPies = ec.ObtenerPiesPaginaxPeriodo(mes, anio);

                if (dtPies.Rows.Count > 0)
                {
                    pie1 = dtPies.Rows[0].ItemArray[3].ToString().Trim();
                    switch (dtPies.Rows.Count)
                    {
                        case 2: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            break;
                        case 3: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            pie3 = dtPies.Rows[2].ItemArray[3].ToString();
                            break;
                        case 4: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            pie3 = dtPies.Rows[2].ItemArray[3].ToString();
                            pie4 = dtPies.Rows[3].ItemArray[3].ToString();
                            break;
                        case 5: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            pie3 = dtPies.Rows[2].ItemArray[3].ToString();
                            pie4 = dtPies.Rows[3].ItemArray[3].ToString();
                            pie5 = dtPies.Rows[4].ItemArray[3].ToString();
                            break;
                        case 6: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            pie3 = dtPies.Rows[2].ItemArray[3].ToString();
                            pie4 = dtPies.Rows[3].ItemArray[3].ToString();
                            pie5 = dtPies.Rows[4].ItemArray[3].ToString();
                            pie6 = dtPies.Rows[5].ItemArray[3].ToString();
                            break;
                    }
                }

                StringBuilder sb = new StringBuilder();

				 #region UNICO VALORES
                //OT10546 INI
                

                DataTable dtParticipesUnicos = dtParticipes.DefaultView.ToTable(true, "idp", "codigo", "tipo_persona", "nombre", "direccion", "urb", "distrito", "ciudad", "dpto", "pais", "titulo", "comentario", "numero_doc", "codigo_agencia");
                

               

                #endregion
                foreach (DataRow drParticipe in dtParticipesUnicos.Rows)
                {
                  //OT10546 FIN
                    decimal idParticipe = Convert.ToDecimal(drParticipe["IDP"]);
                    //foreach (fondos del participe)
                    string filtroParticipeFondos = string.Format("id_participe = {0}", idParticipe);

                    //OT10546 INI
					/*
                    //DataRow[] drParticipeFondos = dtSaldos.Select(filtroParticipeFondos);
                    DataRow[] drParticipeFondos = dtSaldos.Select(filtroParticipeFondos, "id_fondo");

                    foreach (DataRow drParticipeFondo in drParticipeFondos)
                    {
                    */
					//OT10546FIN
                        decimal totalSaldoInicialFondos = 0;
                        decimal totalSaldoFinalFondos = 0;
                        decimal totalSuscripciones = 0;
                        decimal totalRescates = 0;
                        decimal totalPagoFlujos = 0;
                        decimal totalTraspasosFavor = 0;
                        decimal totalTraspasosContra = 0;
                        int indiceBucle = 0;

                        //decimal idParticipe = Convert.ToDecimal(drParticipe["IDP"]);
                        string filtro = string.Format("id_participe = {0}", idParticipe);
                        DataRow[] drMancomunos = dtMancomunos.Select(filtro);

                        string mesTexto = fechaFin.ToString("MMMMMMMMMMMMMM");
                        string fechaTexto = mesTexto.Substring(0, 1).ToUpper() + mesTexto.Substring(1) + " de " + fechaFin.ToString("yyyy"); //Auxiliar para formatos dificiles de Fecha

                        string comentario = drParticipe["comentario"].ToString().Trim().Replace("\r", "").Replace("\n", "");

                        #region Datos del participe

                        Escribir(fechaTexto, 20); //campo1
                        Escribir(drParticipe["codigo"].ToString().Trim(), 8); //campo2
                        Escribir(drParticipe["titulo"].ToString().Trim(), 10); //campo3
                        Escribir(drParticipe["nombre"].ToString().Trim(), 100); //campo4

                        while (indiceBucle < 3)
                        {
                            if (drMancomunos.Length > indiceBucle)
                            {
                                Escribir(drMancomunos[indiceBucle]["macomuno"].ToString().Trim(), 100); //campo5,6,7
                            }
                            else
                            {
                                EscribirVacio(100); //campo5,6,7
                            }
                            indiceBucle++;
                        }

                        Escribir(drParticipe["direccion"].ToString().Trim().Replace("\r", "").Replace("\n", ""), 100); //campo8
                        Escribir(drParticipe["urb"].ToString().Trim().Replace("\r", "").Replace("\n", ""), 100); //campo9
                        Escribir(drParticipe["dpto"].ToString().Trim(), 30); //campo10
                        Escribir(drParticipe["ciudad"].ToString().Trim(), 30); //campo11
                        Escribir(drParticipe["distrito"].ToString().Trim(), 30); //campo12

                        #endregion

                        mesTexto = fechaInicio.ToString("MMMMMMMMMMMMMM");
                        mesTexto = mesTexto.Substring(0, 1).ToUpper() + mesTexto.Substring(1);
                        fechaTexto = fechaInicio.ToString("dd") + " de " + mesTexto + " de " + fechaInicio.ToString("yyyy");
                        Escribir(fechaTexto, 25); //campo13
                        mesTexto = fechaFin.ToString("MMMMMMMMMMMMMM");
                        mesTexto = mesTexto.Substring(0, 1).ToUpper() + mesTexto.Substring(1);
                        fechaTexto = fechaFin.ToString("dd") + " de " + mesTexto + " de " + fechaFin.ToString("yyyy");
                        Escribir(fechaTexto, 25); //campo14

                          #region Serie
                       /*
                        //OT10546 INI
                        DataRow[] drParticipeFondoSaldos = dtSaldos.Select(filtroParticipeFondos, "id_fondo");

                        foreach (DataRow drParticipeFondo in drParticipeFondoSaldos)
                         {
                            //idFondo de saldos
                             decimal idFondo = Convert.ToDecimal(drParticipeFondo["id_fondo"]);
                           
                             string filtroSerieFondo = string.Format("ID = {0}", idFondo);
                             DataRow[] drSerieFondo = dtSerie.Select(filtroSerieFondo);
                             DataRow serieFondo = null;
                             if (drSerieFondo.Length > 0)
                             {
                                 serieFondo = drSerieFondo[0];
                             }
                             if (serieFondo != null && serieFondo["SERIE"].ToString() != null)
                             {
                                 Escribir(serieFondo["SERIE"].ToString(), 1); //campo15
                             }
                             else
                             {
                                 EscribirVacio(1);
                             }
                         }
						 //OT10546 FIN
						 */
                        
                       
                       //OT10546 INI
						/* decimal idFondo = Convert.ToDecimal(drParticipeFondo["id_fondo"]);
						 string filtroSerieFondo = string.Format("ID = {0}", idFondo);
						 DataRow[] drSerieFondo = dtSerie.Select(filtroSerieFondo);
						 DataRow serieFondo = null;
						 if (drSerieFondo.Length > 0)
						 {
							 serieFondo = drSerieFondo[0];
						 }
						 if (serieFondo != null && serieFondo["SERIE"].ToString() != null)
						 {
							 Escribir(serieFondo["SERIE"].ToString(), 1); //campo15
						 }
						 else                        
						 {
							 EscribirVacio(1);
						 }
						 //OT10546 FIN
						 */
						  #endregion

						#region SALDOS
						/*
						//OT10546 INI
						#region SALDO DOLARES
						
						filtro = "";
                        filtro = string.Format("id_participe = {0} and id_fondo = {1}", idParticipe, idFondo);
                        DataRow[] drSaldos = dtSaldos.Select(filtro);
						
						
						
                        if (drSaldos.Length > 0)
                        {
                            if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo16
                            }
                            else
                            {
                                EscribirVacio(20);
                            }

                            if (drSaldos[0]["valorcini"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo17
                            }
                            else
                            {
                                EscribirVacio(13);
                            }
                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo18
                            }
                            else
                            {
                                EscribirVacio(30);
                            }

                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                            }

                            if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo19
                            }
                            else
                            {
                                EscribirVacio(20);
                            }
                            if (drSaldos[0]["valorcfin"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo20
                            }
                            else
                            {
                                EscribirVacio(13);
                            }
                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo21
                            }
                            else
                            {
                                EscribirVacio(30);
                            }
                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                totalSaldoFinalFondos += Convert.ToDecimal(drSaldos[0]["montofin"]);
                            }
                        }
                        else
                        {
                            EscribirVacio(20); //campo16
                            EscribirVacio(13); //campo17
                            EscribirVacio(30); //campo18
                            EscribirVacio(20); //campo19
                            EscribirVacio(13); //campo20
                            EscribirVacio(30); //campo21
                        }

                        if (drSaldos.Length > 0)
                        {
                            Escribir(totalSaldoInicialFondos, 30, Constantes.FormatoMonto); //campo22
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos.Length > 0)
                        {
                            Escribir(totalSaldoFinalFondos, 30, Constantes.FormatoMonto); //campo23
                        }
                        else
                        {
                            EscribirVacio(30);
                        }

                         */
						 
						 #region SALDO DOLARES
                         
                                    filtro = "";
                                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                                    DataRow[] drSaldos = dtSaldos.Select(filtro, "id_fondo");

                                    indiceBucle = 0;
                                    totalSaldoInicialFondos = 0;
                                    totalSaldoFinalFondos = 0;
                                   
                                    while (indiceBucle < 5)
                                   
                                    {
                                        switch (indiceBucle)
                                        {
                                            case 0:
                                                idFondo = 32;
                                                break;
                                            case 1:
                                                idFondo = 43;
                                                break;
                                            case 2:
                                                idFondo = 49;
                                                break;
                                            case 3:
                                                idFondo = 45;
                                                break;
                                            case 4:
                                                idFondo = 48;
                                                break;

                                        }

                                        filtro = string.Format("id_participe = {0} and moneda = 'DOL' and id_fondo= {1} ", idParticipe,idFondo);
                                        drSaldos = dtSaldos.Select(filtro, "id_fondo");
                                       
                                        if (drSaldos.Length > 0)
                                        {
                                            if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                                            {
                                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo16
                                            }
                                            else
                                            {
                                                EscribirVacio(20);
                                            }

                                            if (drSaldos[0]["valorcini"] != DBNull.Value)
                                            {
                                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo17
                                            }
                                            else
                                            {
                                                EscribirVacio(13);
                                            }
                                            if (drSaldos[0]["montoini"] != DBNull.Value)
                                            {
                                                Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo18
                                            }
                                            else
                                            {
                                                EscribirVacio(30);
                                            }

                                            if (drSaldos[0]["montoini"] != DBNull.Value)
                                            {
                                                totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                                            }

                                            if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                                            {
                                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo19
                                            }
                                            else
                                            {
                                                EscribirVacio(20);
                                            }
                                            if (drSaldos[0]["valorcfin"] != DBNull.Value)
                                            {
                                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo20
                                            }
                                            else
                                            {
                                                EscribirVacio(13);
                                            }
                                            if (drSaldos[0]["montofin"] != DBNull.Value)
                                            {
                                                Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo21
                                            }
                                            else
                                            {
                                                EscribirVacio(30);
                                            }
                                            if (drSaldos[0]["montofin"] != DBNull.Value)
                                            {
                                                totalSaldoFinalFondos += Convert.ToDecimal(drSaldos[0]["montofin"]);
                                            }
                                        }
                                        else
                                        {
                                            EscribirVacio(20); //campo22
                                            EscribirVacio(13); //campo23
                                            EscribirVacio(30); //campo24
                                            EscribirVacio(20); //campo25
                                            EscribirVacio(13); //campo26
                                            EscribirVacio(30); //campo27
                                        }

                                        indiceBucle++;
                                    }

                                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                                    if (drSaldos.Length > 0)
                                    {
                                        Escribir(totalSaldoInicialFondos, 30, Constantes.FormatoMonto); //campo28
                                    }
                                    else
                                    {
                                        EscribirVacio(30);
                                    }
                                    if (drSaldos.Length > 0)
                                    {
                                        Escribir(totalSaldoFinalFondos, 30, Constantes.FormatoMonto); //campo29
                                    }
                                    else
                                    {
                                        EscribirVacio(30);
                                    }
                                     ////verificar si es un total parcial segun las caracteristicas IMPORTANTE
                           
                         
                         #endregion SALDO DOLARES
						 
						 
						 
						 
						 
						 //OT10546 FIN
                        #endregion
						

                        #region MOVIMIENTOS
                         /*

                        DataView dvMovSoles = new DataView(dtMovimientos);
                        filtro = string.Format("id_participe = {0} and fondo = {1}", idParticipe, idFondo);
                        string sOrden = "fecha,fondo,id_operacion";
                        dvMovSoles.Sort = sOrden;
                        dvMovSoles.RowFilter = filtro;
                        DataRow[] drMovimientos = new DataRow[dvMovSoles.Count];
                        int i = 0;
                        foreach (DataRowView drv in dvMovSoles)
                        {
                            drMovimientos[i] = drv.Row;
                            i++;
                        }

                        indiceBucle = 0;
                        totalSuscripciones = 0;
                        totalRescates = 0;

                        while (indiceBucle < 15)
                        {
                            if (drMovimientos.Length > indiceBucle)
                            {
                                Escribir(Convert.ToDateTime(drMovimientos[indiceBucle]["fecha"]), 10, "dd/MM/yyyy"); //campo24
                                Escribir(drMovimientos[indiceBucle]["nombrefondo"].ToString().Trim(), 30); //campo25
                                Escribir(drMovimientos[indiceBucle]["descripcion"].ToString().Trim(), 15); //campo26
                                if (drMovimientos[indiceBucle]["cuotas"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["cuotas"]), 20, Constantes.FormatoCuotas);  //campo27
                                }
                                else
                                {
                                    EscribirVacio(20); //campo27
                                }
                                if (drMovimientos[indiceBucle]["valor_cuota"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["valor_cuota"]), 13, Constantes.FormatoValorCuota); //campo28
                                }
                                else
                                {
                                    EscribirVacio(13); //campo28
                                }
                                if (drMovimientos[indiceBucle]["monto"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]), 30, Constantes.FormatoMonto); //campo29
                                }
                                else
                                {
                                    EscribirVacio(30); //campo29
                                }
                                //if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("SUS"))
                                //{
                                //    totalSuscripciones += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                                //}
                                //if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("RES"))
                                //{
                                //    totalRescates += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                                //}
                                //if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("FLU"))
                                //{
                                //    totalPagoFlujos += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                                //}
                                ////TPS a favor
                                //if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("TPS") && Convert.ToDecimal(drMovimientos[indiceBucle]["cuotas"]) > 0)
                                //{
                                //    totalTraspasosFavor += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                                //}
                                ////TPS en contra
                                //if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("TPS") && Convert.ToDecimal(drMovimientos[indiceBucle]["cuotas"]) < 0)
                                //{
                                //    totalTraspasosContra += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                                //}
                            }
                            else
                            {
                                EscribirVacio(10); //campo24
                                EscribirVacio(30); //campo25
                                EscribirVacio(15); //campo26
                                EscribirVacio(20); //campo27
                                EscribirVacio(13); //campo28
                                EscribirVacio(30); //campo29
                            }
                            indiceBucle++;
                        }



                        //Inicio  OT9772 PSC001
                        int indiceBucleTotal = 0;

                        if (drMovimientos.Length > 0)
                        {
                            while (drMovimientos.Length > indiceBucleTotal)
                            {
                                if (drMovimientos[indiceBucleTotal]["tipoope"].ToString().Trim().Equals("SUS"))
                                {
                                    totalSuscripciones += Convert.ToDecimal(drMovimientos[indiceBucleTotal]["monto"]);
                                }
                                if (drMovimientos[indiceBucleTotal]["tipoope"].ToString().Trim().Equals("RES"))
                                {
                                    totalRescates += Convert.ToDecimal(drMovimientos[indiceBucleTotal]["monto"]);
                                }
                                if (drMovimientos[indiceBucleTotal]["tipoope"].ToString().Trim().Equals("FLU"))
                                {
                                    totalPagoFlujos += Convert.ToDecimal(drMovimientos[indiceBucleTotal]["monto"]);
                                }
                                //TPS a favor
                                if (drMovimientos[indiceBucleTotal]["tipoope"].ToString().Trim().Equals("TPS") && Convert.ToDecimal(drMovimientos[indiceBucleTotal]["cuotas"]) > 0)
                                {
                                    totalTraspasosFavor += Convert.ToDecimal(drMovimientos[indiceBucleTotal]["monto"]);
                                }
                                //TPS en contra
                                if (drMovimientos[indiceBucleTotal]["tipoope"].ToString().Trim().Equals("TPS") && Convert.ToDecimal(drMovimientos[indiceBucleTotal]["cuotas"]) < 0)
                                {
                                    totalTraspasosContra += Convert.ToDecimal(drMovimientos[indiceBucleTotal]["monto"]);
                                }

                                indiceBucleTotal = indiceBucleTotal + 1;
                            }
                            Escribir(totalSuscripciones, 30, Constantes.FormatoMonto); //campo88
                            Escribir(totalRescates, 30, Constantes.FormatoMonto); //campo89
                            Escribir(totalPagoFlujos, 30, Constantes.FormatoMonto); //campo90
                            Escribir(totalTraspasosFavor, 30, Constantes.FormatoMonto); //campo91
                            Escribir(totalTraspasosContra, 30, Constantes.FormatoMonto); //campo92


                        }
                        
                        //if (drMovimientos.Length > 0)
                        //{
                        //    Escribir(totalSuscripciones, 30, Constantes.FormatoMonto); //campo88
                        //    Escribir(totalRescates, 30, Constantes.FormatoMonto); //campo89
                        //    Escribir(totalPagoFlujos, 30, Constantes.FormatoMonto); //campo90
                        //    Escribir(totalTraspasosFavor, 30, Constantes.FormatoMonto); //campo91
                        //    Escribir(totalTraspasosContra, 30, Constantes.FormatoMonto); //campo92
                        //}
                        //Fin  OT9772 PSC001
                        else
                        {
                            EscribirVacio(30); //campo88
                            EscribirVacio(30); //campo89
                            EscribirVacio(30); //campo90
                            EscribirVacio(30); //campo91
                            EscribirVacio(30); //campo92
                        }
                         */
                        #endregion

                        
                       //OT10546 INI
                            string sOrden = "";
                            int i = 0;
                            int indiceBucleTotal = 0;
                            #region movimientos soles
                                    /*
                                    DataView dvMovSoles = new DataView(dtMovimientos);
                                    filtro = string.Format("id_participe = {0} and moneda = 'SOL'", idParticipe);
                                    string sOrden = "fecha,fondo,id_operacion";
                                    dvMovSoles.Sort = sOrden;
                                    dvMovSoles.RowFilter = filtro;
                                    DataRow[] drMovimientos = new DataRow[dvMovSoles.Count];
                                    int i = 0;
                                    foreach (DataRowView drv in dvMovSoles)
                                    {
                                        drMovimientos[i] = drv.Row;
                                        i++;
                                    }

                                    indiceBucle = 0;
                                    totalSuscripciones = 0;
                                    totalRescates = 0;

                                    while (indiceBucle < 15)
                                    {
                                        if (drMovimientos.Length > indiceBucle)
                                        {
                                            Escribir(Convert.ToDateTime(drMovimientos[indiceBucle]["fecha"]), 10, "dd/MM/yyyy"); //campo24
                                            Escribir(drMovimientos[indiceBucle]["nombrefondo"].ToString().Trim(), 30); //campo25
                                            Escribir(drMovimientos[indiceBucle]["descripcion"].ToString().Trim(), 15); //campo26
                                            if (drMovimientos[indiceBucle]["cuotas"] != DBNull.Value)
                                            {
                                                Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["cuotas"]), 20, Constantes.FormatoCuotas);  //campo27
                                            }
                                            else
                                            {
                                                EscribirVacio(20); //campo27
                                            }
                                            if (drMovimientos[indiceBucle]["valor_cuota"] != DBNull.Value)
                                            {
                                                Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["valor_cuota"]), 13, Constantes.FormatoValorCuota); //campo28
                                            }
                                            else
                                            {
                                                EscribirVacio(13); //campo28
                                            }
                                            if (drMovimientos[indiceBucle]["monto"] != DBNull.Value)
                                            {
                                                Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]), 30, Constantes.FormatoMonto); //campo29
                                            }
                                            else
                                            {
                                                EscribirVacio(30); //campo29
                                            }
                                           
                                        }
                                        else
                                        {
                                            EscribirVacio(10); //campo24
                                            EscribirVacio(30); //campo25
                                            EscribirVacio(15); //campo26
                                            EscribirVacio(20); //campo27
                                            EscribirVacio(13); //campo28
                                            EscribirVacio(30); //campo29
                                        }
                                        indiceBucle++;
                                    }



                                    
                                    int indiceBucleTotal = 0;

                                    if (drMovimientos.Length > 0)
                                    {
                                        while (drMovimientos.Length > indiceBucleTotal)
                                        {
                                            if (drMovimientos[indiceBucleTotal]["tipoope"].ToString().Trim().Equals("SUS"))
                                            {
                                                totalSuscripciones += Convert.ToDecimal(drMovimientos[indiceBucleTotal]["monto"]);
                                            }
                                            if (drMovimientos[indiceBucleTotal]["tipoope"].ToString().Trim().Equals("RES"))
                                            {
                                                totalRescates += Convert.ToDecimal(drMovimientos[indiceBucleTotal]["monto"]);
                                            }
                                            if (drMovimientos[indiceBucleTotal]["tipoope"].ToString().Trim().Equals("FLU"))
                                            {
                                                totalPagoFlujos += Convert.ToDecimal(drMovimientos[indiceBucleTotal]["monto"]);
                                            }
                                            //TPS a favor
                                            if (drMovimientos[indiceBucleTotal]["tipoope"].ToString().Trim().Equals("TPS") && Convert.ToDecimal(drMovimientos[indiceBucleTotal]["cuotas"]) > 0)
                                            {
                                                totalTraspasosFavor += Convert.ToDecimal(drMovimientos[indiceBucleTotal]["monto"]);
                                            }
                                            //TPS en contra
                                            if (drMovimientos[indiceBucleTotal]["tipoope"].ToString().Trim().Equals("TPS") && Convert.ToDecimal(drMovimientos[indiceBucleTotal]["cuotas"]) < 0)
                                            {
                                                totalTraspasosContra += Convert.ToDecimal(drMovimientos[indiceBucleTotal]["monto"]);
                                            }

                                            indiceBucleTotal = indiceBucleTotal + 1;
                                        }
                                        Escribir(totalSuscripciones, 30, Constantes.FormatoMonto); //campo88
                                        Escribir(totalRescates, 30, Constantes.FormatoMonto); //campo89
                                        Escribir(totalPagoFlujos, 30, Constantes.FormatoMonto); //campo90
                                        Escribir(totalTraspasosFavor, 30, Constantes.FormatoMonto); //campo91
                                        Escribir(totalTraspasosContra, 30, Constantes.FormatoMonto); //campo92


                                    }

                                   
                                    else
                                    {
                                        EscribirVacio(30); //campo88
                                        EscribirVacio(30); //campo89
                                        EscribirVacio(30); //campo90
                                        EscribirVacio(30); //campo91
                                        EscribirVacio(30); //campo92
                                    }
                                    
                                   */
                                  
                           #endregion

                           #region movimientos dolares
                                    DataView dvMovDolares = new DataView(dtMovimientos);
                                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                                     sOrden = "fecha,fondo,id_operacion";
                                    dvMovDolares.Sort = sOrden;
                                    dvMovDolares.RowFilter = filtro;
                                    DataRow[] drMovimientosDolares = new DataRow[dvMovDolares.Count];
                                     i = 0;
                                    foreach (DataRowView drv in dvMovDolares)
                                    {
                                        drMovimientosDolares[i] = drv.Row;
                                        i++;
                                    }

                                    int indiceBucleDolares = 0;
                                    totalSuscripciones = 0;
                                    totalRescates = 0;

                                    while (indiceBucleDolares < 15)
                                    {
                                        if (drMovimientosDolares.Length > indiceBucleDolares)
                                        {
                                            Escribir(Convert.ToDateTime(drMovimientosDolares[indiceBucleDolares]["fecha"]), 10, "dd/MM/yyyy"); //campo24
                                            Escribir(drMovimientosDolares[indiceBucleDolares]["nombrefondo"].ToString().Trim(), 30); //campo25
                                            Escribir(drMovimientosDolares[indiceBucleDolares]["descripcion"].ToString().Trim(), 15); //campo26
                                            if (drMovimientosDolares[indiceBucleDolares]["cuotas"] != DBNull.Value)
                                            {
                                                Escribir(Convert.ToDecimal(drMovimientosDolares[indiceBucleDolares]["cuotas"]), 20, Constantes.FormatoCuotas);  //campo27
                                            }
                                            else
                                            {
                                                EscribirVacio(20); //campo27
                                            }
                                            if (drMovimientosDolares[indiceBucleDolares]["valor_cuota"] != DBNull.Value)
                                            {
                                                Escribir(Convert.ToDecimal(drMovimientosDolares[indiceBucleDolares]["valor_cuota"]), 13, Constantes.FormatoValorCuota); //campo28
                                            }
                                            else
                                            {
                                                EscribirVacio(13); //campo28
                                            }
                                            if (drMovimientosDolares[indiceBucleDolares]["monto"] != DBNull.Value)
                                            {
                                                Escribir(Convert.ToDecimal(drMovimientosDolares[indiceBucleDolares]["monto"]), 30, Constantes.FormatoMonto); //campo29
                                            }
                                            else
                                            {
                                                EscribirVacio(30); //campo29
                                            }

                                        }
                                        else
                                        {
                                            EscribirVacio(10); //campo24
                                            EscribirVacio(30); //campo25
                                            EscribirVacio(15); //campo26
                                            EscribirVacio(20); //campo27
                                            EscribirVacio(13); //campo28
                                            EscribirVacio(30); //campo29
                                        }
                                        indiceBucleDolares++;
                                    }

                                    int indiceBucleTotalDol = 0;

                                    if (drMovimientosDolares.Length > 0)
                                    {
                                        while (drMovimientosDolares.Length > indiceBucleTotalDol)
                                        {
                                            if (drMovimientosDolares[indiceBucleTotalDol]["tipoope"].ToString().Trim().Equals("SUS"))
                                            {
												//OT10913 INI
												if (drMovimientosDolares[indiceBucleTotalDol]["monto"] != null && drMovimientosDolares[indiceBucleTotalDol]["monto"] != DBNull.Value)
												{
													totalSuscripciones += Convert.ToDecimal(drMovimientosDolares[indiceBucleTotalDol]["monto"]);
												}
												//OT10913 FIN
                                            }
                                            if (drMovimientosDolares[indiceBucleTotalDol]["tipoope"].ToString().Trim().Equals("RES"))
                                            {
												//OT10913 INI
												if (drMovimientosDolares[indiceBucleTotalDol]["monto"] != null && drMovimientosDolares[indiceBucleTotalDol]["monto"] != DBNull.Value)
												{
													totalRescates += Convert.ToDecimal(drMovimientosDolares[indiceBucleTotalDol]["monto"]);
												}
												//OT10913 FIN
											}
                                            if (drMovimientosDolares[indiceBucleTotalDol]["tipoope"].ToString().Trim().Equals("FLU"))
                                            {
												//OT10913 INI
												if (drMovimientosDolares[indiceBucleTotalDol]["monto"] != null && drMovimientosDolares[indiceBucleTotalDol]["monto"] != DBNull.Value)
												{
													totalPagoFlujos += Convert.ToDecimal(drMovimientosDolares[indiceBucleTotalDol]["monto"]);
												}
												//OT10913 FIN
                                            }

											//OT10913 INI
                                            //if (drMovimientosDolares[indiceBucleTotalDol]["tipoope"].ToString().Trim().Equals("TPS") && Convert.ToDecimal(drMovimientosDolares[indiceBucleTotal]["cuotas"]) > 0)
											if (drMovimientosDolares[indiceBucleTotal]["cuotas"] != null && drMovimientosDolares[indiceBucleTotal]["cuotas"] != DBNull.Value)
											{
												if (drMovimientosDolares[indiceBucleTotalDol]["tipoope"].ToString().Trim().Equals("TPS") && Convert.ToDecimal(drMovimientosDolares[indiceBucleTotal]["cuotas"]) > 0)
												{
													totalTraspasosFavor += Convert.ToDecimal(drMovimientosDolares[indiceBucleTotalDol]["monto"]);
												}

												if (drMovimientosDolares[indiceBucleTotalDol]["tipoope"].ToString().Trim().Equals("TPS") && Convert.ToDecimal(drMovimientosDolares[indiceBucleTotal]["cuotas"]) < 0)
												{
													totalTraspasosContra += Convert.ToDecimal(drMovimientosDolares[indiceBucleTotalDol]["monto"]);
												}
											}
											//OT10913 FIN
                                           


                                            indiceBucleTotalDol = indiceBucleTotalDol + 1;
                                        }
                                        Escribir(totalSuscripciones, 30, Constantes.FormatoMonto); //campo88
                                        Escribir(totalRescates, 30, Constantes.FormatoMonto); //campo89
                                        Escribir(totalPagoFlujos, 30, Constantes.FormatoMonto); //campo90
                                        Escribir(totalTraspasosFavor, 30, Constantes.FormatoMonto); //campo91
                                        Escribir(totalTraspasosContra, 30, Constantes.FormatoMonto); //campo92


                                    }


                                    else
                                    {
                                        EscribirVacio(30); //campo88
                                        EscribirVacio(30); //campo89
                                        EscribirVacio(30); //campo90
                                        EscribirVacio(30); //campo91
                                        EscribirVacio(30); //campo92
                                    }


                                    //OT10546 FIN

						#endregion //MOVIMIENTOS DOLARES




                        //FONDO
                        #region 
						//OT10546 INI
                        /*

                        string filtroFondo = string.Format("ID = {0}", idFondo);
                        DataRow[] drFondo = dtFondo.Select(filtroFondo);
                        DataRow datosFondo = null;
                        if (drFondo.Length > 0)
                        {
                            datosFondo = drFondo[0];
                        }
                        if (datosFondo != null && datosFondo["NOMBRE"].ToString() != null)
                        {
                            Escribir(datosFondo["MONEDA"].ToString(), 30); //campo119
                            Escribir(datosFondo["NOMBRE"].ToString(), 50); //campo120
                            Escribir(datosFondo["SIMBOLO"].ToString(), 3); //campo120
                        }
                        else
                        {
                            EscribirVacio(30); //campo119
                            EscribirVacio(50); //campo120
                            EscribirVacio(3); //campo121
                        }
                         */
						//OT10546 FIN
                        #endregion

                        CambiarLinea();
                    //} OT10546
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                sw.Close();
            }
        }

       
        


        public void SaveToFilePWD_PRI(string path, string strTipoEECC, int indicadorMasiva)
        {
            sw = new StreamWriter(path, false, Encoding.Default);
            try
            {
                EECCMasivoDA ec = new EECCMasivoDA();
                DataSet dsEECC = ec.ObtenerDataEECC_PRI(fechaInicio, fechaFin, strTipoEECC, indicadorMasiva);
                DataTable dtParticipes = dsEECC.Tables["PARTICIPES"];
				                 //OT10546 INI
								 /*
								//OT9772 PSC001 - INI
								DataTable dtSaldos = dsEECC.Tables["SALDOS"];
								//OT9772 PSC001 - FIN
								*/
								//OT10546 FIN

                //OT10546 INI

                DataTable participesUnicos = dtParticipes.DefaultView.ToTable(true, "IDP", "codigo", "numero_doc");

                //OT10546 FIN
                foreach (DataRow drParticipe in participesUnicos.Rows)
                {
				
				 //OT10546 INI
									//OT9772 PSC001 - INI
									/*decimal idParticipe = Convert.ToDecimal(drParticipe["IDP"]);
									string filtroParticipeFondos = string.Format("id_participe = {0}", idParticipe);
									DataRow[] drParticipeFondos = dtSaldos.Select(filtroParticipeFondos, "id_fondo");

									foreach (DataRow drParticipeFondo in drParticipeFondos)
									{*/
                    Escribir(drParticipe["codigo"].ToString().Trim(), 8); //campo1
                    EscribirVacio(1);
                    Escribir(drParticipe["numero_doc"].ToString().Trim(), 20); //campo2
                    CambiarLinea();
                                    /*}*/
									//OT9772 PSC001 - FIN
				   //OT10546 FIN
				}
            }
            finally
            {
                sw.Close();
            }

        }

        //OT9772 FIN


        public void SaveToFile(string path, string strTipoEECC, int indicadorMasiva)
        {
            sw = new StreamWriter(path, false, Encoding.Default);
            try
            {

                EECCMasivoDA ec = new EECCMasivoDA();
                DataSet dsEECC = ec.ObtenerDataEECC(fechaInicio, fechaFin, strTipoEECC, indicadorMasiva);
                DataTable dtParticipes = dsEECC.Tables["PARTICIPES"];
                DataTable dtMancomunos = dsEECC.Tables["MANCOMUNOS"];
                DataTable dtSaldos = dsEECC.Tables["SALDOS"];
                DataTable dtMovimientos = dsEECC.Tables["MOVIMIENTOS"];
                DataTable dtCuentasParticipacion = dsEECC.Tables["CUENTAS_PARTICIPACION"];

                //OT2888: Se usa una tabla auxiliar para ordenar los fondos de acuerdo a la especificación de la OT
                //DataTable saldosAux = dtSaldos.Clone(); --OT10546

                //03/09/2009: Se agrega el mensaje en la zona final del EECC
                //11/03/2011: OT-ING08032011 se obtiene el mensaje de acuerdo al periodo del EECC
                int mes = fechaFin.Month;
                int anio = fechaInicio.Year;
                string mensaje = "";
                string titulo = "";
                string piepagina = "";
                string pie1 = "";
                string pie2 = "";
                string pie3 = "";
                string pie4 = "";
                string pie5 = "";
                string pie6 = "";
                //DataTable dtMensaje = ec.ObtenerMensaje();
                //OT-ING08032011 Se obtiene el mensaje de acuerdo al periodo seleccionado en el formulario
                DataTable dtMensaje = ec.ObtenerMensajexPeriodo(mes, anio);
                if (dtMensaje.Rows.Count > 0)
                {
                    titulo = dtMensaje.Rows[0]["TITULO"].ToString().Trim();
                    mensaje = dtMensaje.Rows[0]["MENSAJE"].ToString().Trim();
                    piepagina = dtMensaje.Rows[0]["PIE"].ToString().Trim();
                }
                //OT-ING08032011 Se obtienen los pies de paginas
                DataTable dtPies = ec.ObtenerPiesPaginaxPeriodo(mes, anio);
                if (dtPies.Rows.Count > 0)
                {
                    pie1 = dtPies.Rows[0].ItemArray[3].ToString().Trim();
                    switch (dtPies.Rows.Count)
                    {
                        case 2: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            break;
                        case 3: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            pie3 = dtPies.Rows[2].ItemArray[3].ToString();
                            break;
                        case 4: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            pie3 = dtPies.Rows[2].ItemArray[3].ToString();
                            pie4 = dtPies.Rows[3].ItemArray[3].ToString();
                            break;
                        case 5: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            pie3 = dtPies.Rows[2].ItemArray[3].ToString();
                            pie4 = dtPies.Rows[3].ItemArray[3].ToString();
                            pie5 = dtPies.Rows[4].ItemArray[3].ToString();
                            break;
                        case 6: pie2 = dtPies.Rows[1].ItemArray[3].ToString();
                            pie3 = dtPies.Rows[2].ItemArray[3].ToString();
                            pie4 = dtPies.Rows[3].ItemArray[3].ToString();
                            pie5 = dtPies.Rows[4].ItemArray[3].ToString();
                            pie6 = dtPies.Rows[5].ItemArray[3].ToString();
                            break;
                    }
                }

                StringBuilder sb = new StringBuilder();

                foreach (DataRow drParticipe in dtParticipes.Rows)
                {
                    decimal totalSaldoInicialFondos = 0;
                    decimal totalSaldoFinalFondos = 0;
                    decimal totalSaldoDolares = 0;
                    decimal totalSaldoSoles = 0;
                    decimal totalSuscripciones = 0;
                    decimal totalRescates = 0;
                    int indiceBucle = 0;

                    decimal idParticipe = Convert.ToDecimal(drParticipe["IDP"]);

                    string filtro = string.Format("id_participe = {0}", idParticipe);

                    DataRow[] drMancomunos = dtMancomunos.Select(filtro);

                    string mesTexto = fechaFin.ToString("MMMMMMMMMMMMMM");
                    string fechaTexto = mesTexto.Substring(0, 1).ToUpper() + mesTexto.Substring(1) + " de " + fechaFin.ToString("yyyy"); //Auxiliar para formatos dificiles de Fecha

                    // INI SR 19102010 - OT3199
                    string comentario = drParticipe["comentario"].ToString().Trim().Replace("\r", "").Replace("\n", "");
                    // FIN SR 19102010 - OT3199

                    Escribir(fechaTexto, 20); //campo1
                    Escribir(drParticipe["codigo"].ToString().Trim(), 8); //campo2
                    Escribir(drParticipe["titulo"].ToString().Trim(), 10); //campo3
                    Escribir(drParticipe["nombre"].ToString().Trim(), 100); //campo4
                    //OT 8365 INI
                    while (indiceBucle < 3)
                    //OT 8365 FIN
                    {
                        if (drMancomunos.Length > indiceBucle)
                        {
                            Escribir(drMancomunos[indiceBucle]["macomuno"].ToString().Trim(), 100); //campo5,6,7
                        }
                        else
                        {
                            EscribirVacio(100); //campo5,6,7
                        }
                        indiceBucle++;
                    }

                    Escribir(drParticipe["direccion"].ToString().Trim().Replace("\r", "").Replace("\n", ""), 100); //campo8
                    //OT2888: Se agrega campo urb (urbanizacion)
                    Escribir(drParticipe["urb"].ToString().Trim().Replace("\r", "").Replace("\n", ""), 100); //campo9
                    Escribir(drParticipe["dpto"].ToString().Trim(), 30); //campo10
                    Escribir(drParticipe["ciudad"].ToString().Trim(), 30); //campo11
                    Escribir(drParticipe["distrito"].ToString().Trim(), 30); //campo12
                    mesTexto = fechaInicio.ToString("MMMMMMMMMMMMMM");
                    mesTexto = mesTexto.Substring(0, 1).ToUpper() + mesTexto.Substring(1);
                    fechaTexto = fechaInicio.ToString("dd") + " de " + mesTexto + " de " + fechaInicio.ToString("yyyy");
                    Escribir(fechaTexto, 25); //campo13
                    mesTexto = fechaFin.ToString("MMMMMMMMMMMMMM");
                    mesTexto = mesTexto.Substring(0, 1).ToUpper() + mesTexto.Substring(1);
                    fechaTexto = fechaFin.ToString("dd") + " de " + mesTexto + " de " + fechaFin.ToString("yyyy");
                    Escribir(fechaTexto, 25); //campo14

                    #region SALDOS
                    #region SALDOS SOLES

                    filtro = string.Format("id_participe = {0} and moneda = 'SOL'", idParticipe);
                    DataRow[] drSaldos = dtSaldos.Select(filtro);

                    indiceBucle = 0;
                    totalSaldoInicialFondos = 0;
                    totalSaldoFinalFondos = 0;
                    int idFondo = 0;
                    //OT7833 INI
                    //while (indiceBucle < 11)
                    while (indiceBucle < 13)
                    //OT7833 FIN
                    {
                        switch (indiceBucle)
                        {
                            case 0:
                                idFondo = 9;
                                break;
                            case 1:
                                idFondo = 1;
                                break;
                            case 2:
                                idFondo = 4;
                                break;
                            case 3:
                                idFondo = 3;
                                break;
                            case 4:
                                idFondo = 12;
                                break;
                            case 5:
                                idFondo = 14;
                                break;
                            case 6:
                                idFondo = 15;
                                break;
                            case 7:
                                idFondo = 16;
                                break;
                            case 8:
                                idFondo = 17;
                                break;
                            case 9:
                                idFondo = 18;
                                break;
                            case 10:
                                idFondo = 19;
                                break;
                            //OT7833 INI
                            case 11:
                                //OT9613 INI	
                                //idFondo = 23;
                                idFondo = 44;
                                //OT9613 FIN
                                break;
                            //OT7833 FIN
                            //OT 8365 INI
                            case 12:
                                idFondo = 30;
                                break;
                            //OT 8365 FIN

                        }
                        filtro = string.Format("id_participe = {0} and moneda = 'SOL' and id_fondo = {1}", idParticipe, idFondo);
                        drSaldos = dtSaldos.Select(filtro, "id_fondo");

                        if (drSaldos.Length > 0)
                        {
                            if (idFondo < 12)
                            {

                                if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo15,21,27,33
                                }
                                else
                                {
                                    EscribirVacio(20);
                                }
                                if (drSaldos[0]["valorcini"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo16,22,28,34
                                }
                                else
                                {
                                    EscribirVacio(13);
                                }
                                if (drSaldos[0]["montoini"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo17,23,29,35
                                }
                                else
                                {
                                    EscribirVacio(30);
                                }

                            }

                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                            }

                            if (idFondo < 12)
                            {

                                if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo18,24,30,36
                                }
                                else
                                {
                                    EscribirVacio(20);
                                }
                                if (drSaldos[0]["valorcfin"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo19,25,31,37
                                }
                                else
                                {
                                    EscribirVacio(13);
                                }
                                if (drSaldos[0]["montofin"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo20,26,32,38
                                }
                                else
                                {
                                    EscribirVacio(30);
                                }
                            }
                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                totalSaldoFinalFondos += Convert.ToDecimal(drSaldos[0]["montofin"]);
                            }

                            /*
                            if(drSaldos[indiceBucle]["variacion"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[indiceBucle]["variacion"]), 50, Constantes.FormatoCuotas); //campo20,27,34,41
                            }
                            else
                            {
                                EscribirVacio(50);
                            }
                            */
                        }
                        else
                        {
                            if (idFondo < 12)
                            {
                                EscribirVacio(20); //campo15,21,27,33
                                EscribirVacio(13); //campo16,22,28,34
                                EscribirVacio(30); //campo17,23,29,35
                                EscribirVacio(20); //campo18,24,30,36
                                EscribirVacio(13); //campo19,25,31,37
                                EscribirVacio(30); //campo20,26,32,38
                            }
                            //EscribirVacio(50); //campo20,27,34,41
                        }
                        indiceBucle++;
                    }
                    filtro = string.Format("id_participe = {0} and moneda = 'SOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");
                    if (drSaldos.Length > 0)
                    {
                        Escribir(totalSaldoInicialFondos, 30, Constantes.FormatoMonto); //campo39
                    }
                    else
                    {
                        EscribirVacio(30);
                    }
                    if (drSaldos.Length > 0)
                    {
                        Escribir(totalSaldoFinalFondos, 30, Constantes.FormatoMonto); //campo40
                    }
                    else
                    {
                        EscribirVacio(30);
                    }
                    #endregion

                    #region SALDOS DOLARES
                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    indiceBucle = 0;
                    totalSaldoInicialFondos = 0;
                    totalSaldoFinalFondos = 0;
                    //OT 8365 INI
                    //OT9426 INI
                    //while (indiceBucle < 8)
                    //while (indiceBucle < 10)
					while (indiceBucle < 12) //OT10546 PSC001
                    //OT9426 FIN
                    //OT 8365 FIN
                    {
                        switch (indiceBucle)
                        {
                            case 0:
                                idFondo = 8;
                                break;
                            case 1:
                                idFondo = 2;
                                break;
                            case 2:
                                idFondo = 10;
                                break;
                            case 3:
                                idFondo = 11;
                                break;
                            case 4:
                                idFondo = 20;//OT6134
                                break;
                            case 5:
                                idFondo = 21; //OT6134
                                break;
                            case 6:
                                idFondo = 22; //OT6590
                                break;
                                //OT 8365 INI
                            case 7:
                                idFondo = 31; //OT6590
                                break;
                                //OT 8365 FIN
                            //OT9426 INI	
                            case 8:
                                idFondo = 33; //SEL_GLOBAL_IA
                                break;
                            case 9:
                                idFondo = 34; //SEL_GLOBAL_IB
                                break;
                            //OT9426 FIN
                             //OT10546 PSC001 INI	
                            case 10:
                                idFondo = 50; //BON_GLO_A
                                break;
                            case 11:
                                idFondo = 51; //BON_GLO_B
                                break;
                            //OT10546 PSC001 FIN

                        }
                        filtro = string.Format("id_participe = {0} and moneda = 'DOL' and id_fondo = {1}", idParticipe, idFondo);
                        drSaldos = dtSaldos.Select(filtro, "id_fondo");

                        if (drSaldos.Length > 0)
                        {
                            if (idFondo <10)
                            {
                                if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo41,47
                                }
                                else
                                {
                                    EscribirVacio(20);
                                }
                                if (drSaldos[0]["valorcini"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo42,48
                                }
                                else
                                {
                                    EscribirVacio(13);
                                }
                                if (drSaldos[0]["montoini"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo43,49
                                }
                                else
                                {
                                    EscribirVacio(30);
                                }
                            }
                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                            }
                            if (idFondo < 10)
                            {
                                if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo44,50
                                }
                                else
                                {
                                    EscribirVacio(20);
                                }
                                if (drSaldos[0]["valorcfin"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo45,51
                                }
                                else
                                {
                                    EscribirVacio(13);
                                }
                                if (drSaldos[0]["montofin"] != DBNull.Value)
                                {
                                    Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo46,52
                                }
                                else
                                {
                                    EscribirVacio(30);
                                }
                            }
                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                totalSaldoFinalFondos += Convert.ToDecimal(drSaldos[0]["montofin"]);
                            }

                            /*
                            if(drSaldos[indiceBucle]["valorcini"] !=DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[indiceBucle]["variacion"]), 50, Constantes.FormatoCuotas); //campo50,57
                            }
                            else
                            {
                                EscribirVacio(50); //campo50,57
                            }
                            */
                        }
                        else
                        {
                            if (idFondo <10)
                            {
                                EscribirVacio(20); //campo41,47
                                EscribirVacio(13); //campo42,48
                                EscribirVacio(30); //campo43,49
                                EscribirVacio(20); //campo44,50
                                EscribirVacio(13); //campo45,51
                                EscribirVacio(30); //campo46,52
                                //EscribirVacio(50); //campo50,57
                            }
                        }
                        indiceBucle++;
                    }

                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");
                    if (drSaldos.Length > 0)
                    {
                        Escribir(totalSaldoInicialFondos, 30, Constantes.FormatoMonto); //campo53
                    }
                    else
                    {
                        EscribirVacio(30);
                    }
                    if (drSaldos.Length > 0)
                    {
                        Escribir(totalSaldoFinalFondos, 30, Constantes.FormatoMonto); //campo54
                    }
                    else
                    {
                        EscribirVacio(30);
                    }
                    #endregion
                    #endregion


                    #region CUENTAS PARTICIPACION
                    filtro = string.Format("idparticipe = {0}", idParticipe);
                    DataRow[] drCuentasParticipacion = dtCuentasParticipacion.Select(filtro, "fechaaperturafondo");

                    indiceBucle = 0;
                    totalSaldoDolares = 0;
                    totalSaldoSoles = 0;
                    while (indiceBucle < 10)
                    {
                        if (drCuentasParticipacion.Length > indiceBucle)
                        {
                            Escribir(drCuentasParticipacion[indiceBucle]["nombrecuenta"].ToString().Trim(), 50); //campo55,61,67,73,79,85,91,97,103,109
                            Escribir(drCuentasParticipacion[indiceBucle]["codigocuenta"].ToString().Trim(), 11); //campo56,62,68,74,80,86,92,98,104,110
                            Escribir(drCuentasParticipacion[indiceBucle]["nombrefondo"].ToString().Trim(), 30); //campo57,63,69,75,81,87,93,99,105,111

                            if (drCuentasParticipacion[indiceBucle]["monedafondo"].ToString().Trim() == "DOL")
                            {
                                Escribir("Dólares", 7); //campo58,64,70,76,82,88,94,100,106,112
                            }
                            else
                            {
                                Escribir("Soles", 7); //campo58,64,70,76,82,88,94,100,106,112
                            }

                            if (drCuentasParticipacion[indiceBucle]["saldodol"].ToString().Trim() != "")
                            {
                                Escribir(Convert.ToDecimal(drCuentasParticipacion[indiceBucle]["saldodol"]), 30, Constantes.FormatoMonto);  //campo59,65,71,77,83,89,95,101,107,113
                                totalSaldoDolares += Convert.ToDecimal(drCuentasParticipacion[indiceBucle]["saldodol"]);
                            }
                            else
                            {
                                EscribirVacio(30); //campo59,65,71,77,83,89,95,101,107,113
                            }
                            if (drCuentasParticipacion[indiceBucle]["saldosol"].ToString().Trim() != "")
                            {
                                Escribir(Convert.ToDecimal(drCuentasParticipacion[indiceBucle]["saldosol"]), 30, Constantes.FormatoMonto);  //campo60,66,72,78,84,90,96,102,108,114
                                totalSaldoSoles += Convert.ToDecimal(drCuentasParticipacion[indiceBucle]["saldosol"]);
                            }
                            else
                            {
                                EscribirVacio(30); //campo60,66,72,78,84,90,96,102,108,114
                            }

                        }
                        else
                        {
                            EscribirVacio(50); //campo55,61,67,73,79,85,91,97,103,109
                            EscribirVacio(11); //campo56,62,68,74,80,86,92,98,104,110
                            EscribirVacio(30); //campo57,63,69,75,81,87,93,99,105,111
                            EscribirVacio(7); //campo58,64,70,76,82,88,94,100,106,112
                            EscribirVacio(30); //campo59,65,71,77,83,89,95,101,107,113
                            EscribirVacio(30); //campo60,66,72,78,84,90,96,102,108,114
                        }
                        indiceBucle++;
                    }

                    if (drCuentasParticipacion.Length > 0)
                    {
                        Escribir(totalSaldoDolares, 30, Constantes.FormatoMonto); //campo115
                        Escribir(totalSaldoSoles, 30, Constantes.FormatoMonto); //campo116
                    }
                    else
                    {
                        EscribirVacio(30); //campo115
                        EscribirVacio(30); //campo116
                    }
                    #endregion

                    #region MOVIMIENTOS
                    #region MOVIMIENTOS SOLES

                    DataView dvMovSoles = new DataView(dtMovimientos);
                    filtro = string.Format("id_participe = {0} and moneda = 'SOL'", idParticipe);
                    string sOrden = "fecha,fondo,id_operacion";
                    dvMovSoles.Sort = sOrden;
                    dvMovSoles.RowFilter = filtro;
                    DataRow[] drMovimientos = new DataRow[dvMovSoles.Count];
                    int i = 0;
                    foreach (DataRowView drv in dvMovSoles)
                    {
                        drMovimientos[i] = drv.Row;
                        i++;
                    }

                    indiceBucle = 0;
                    totalSuscripciones = 0;
                    totalRescates = 0;
                    while (indiceBucle < 15)
                    {
                        if (drMovimientos.Length > indiceBucle)
                        {
                            Escribir(Convert.ToDateTime(drMovimientos[indiceBucle]["fecha"]), 10, "dd/MM/yyyy"); //campo117,124,131,138,145,152,159,166,173,180,187,194,201,208,215
                            Escribir(drMovimientos[indiceBucle]["nombrefondo"].ToString().Trim(), 30); //campo118,125,132,139,146,153,160,167,174,181,188,195,202,209,216
                            Escribir(drMovimientos[indiceBucle]["plan_ahorro"].ToString().Trim(), 11); //campo119,126,133,140,147,154,161,168,175,182,189,196,203,210,217
                            Escribir(drMovimientos[indiceBucle]["descripcion"].ToString().Trim(), 15); //campo120,127,134,141,148,155,162,169,176,183,190,197,204,211,218
                            if (drMovimientos[indiceBucle]["cuotas"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["cuotas"]), 20, Constantes.FormatoCuotas);  //campo121,128,135,142,149,156,163,170,177,184,191,198,205,212,219
                            }
                            else
                            {
                                EscribirVacio(20); //campo121,128,135,142,149,156,163,170,177,184,191,198,205,212,219
                            }
                            if (drMovimientos[indiceBucle]["valor_cuota"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["valor_cuota"]), 13, Constantes.FormatoValorCuota); //campo122,129,136,143,150,157,164,171,178,185,192,199,206,213,220
                            }
                            else
                            {
                                EscribirVacio(13); //campo122,129,136,143,150,157,164,171,178,185,192,199,206,213,220
                            }
                            if (drMovimientos[indiceBucle]["monto"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]), 30, Constantes.FormatoMonto); //campo123,130,137,144,151,158,165,172,179,186,193,200,207,214,221
                            }
                            else
                            {
                                EscribirVacio(30); //campo123,130,137,144,151,158,165,172,179,186,193,200,207,214,221
                            }
                            //Inicio  OT9772 PSC001
                            //if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("SUS"))
                            //{
                            //    totalSuscripciones += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                            //}
                            //if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("RES"))
                            //{
                            //    totalRescates += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                            //}
                            //Fin  OT9772 PSC001
                        }
                        else
                        {
                            EscribirVacio(10); //campo117,124,131,138,145,152,159,166,173,180,187,194,201,208,215
                            EscribirVacio(30); //campo118,125,132,139,146,153,160,167,174,181,188,195,202,209,216
                            EscribirVacio(11); //campo119,126,133,140,147,154,161,168,175,182,189,196,203,210,217
                            EscribirVacio(15); //campo120,127,134,141,148,155,162,169,176,183,190,197,204,211,218
                            EscribirVacio(20); //campo121,128,135,142,149,156,163,170,177,184,191,198,205,212,219
                            EscribirVacio(13); //campo122,129,136,143,150,157,164,171,178,185,192,199,206,213,220
                            EscribirVacio(30); //campo123,130,137,144,151,158,165,172,179,186,193,200,207,214,221
                        }
                        indiceBucle++;
                    }
                    //Inicio  OT9772 PSC001
                    int indiceBucleTotal = 0;

                    if (drMovimientos.Length > 0)
                    {
                        while (drMovimientos.Length > indiceBucleTotal)
                        {
                            if (drMovimientos[indiceBucleTotal]["tipoope"].ToString().Trim().Equals("SUS"))
                            {
                                totalSuscripciones += Convert.ToDecimal(drMovimientos[indiceBucleTotal]["monto"]);
                            }
                            if (drMovimientos[indiceBucleTotal]["tipoope"].ToString().Trim().Equals("RES"))
                            {
                                totalRescates += Convert.ToDecimal(drMovimientos[indiceBucleTotal]["monto"]);
                            }

                            indiceBucleTotal = indiceBucleTotal + 1;
                        }
                        Escribir(totalSuscripciones, 30, Constantes.FormatoMonto); //campo222
                        Escribir(totalRescates, 30, Constantes.FormatoMonto); //campo223


                    }
                    //if (drMovimientos.Length > 0)
                    //{
                    //    Escribir(totalSuscripciones, 30, Constantes.FormatoMonto); //campo222
                    //    Escribir(totalRescates, 30, Constantes.FormatoMonto); //campo223
                    //}
                    //Fin  OT9772 PSC001
                    else
                    {
                        EscribirVacio(30); //campo222
                        EscribirVacio(30); //campo223
                    }
                    #endregion

                    #region MOVIMIENTOS DOLARES

                    DataView dvMovDolares = new DataView(dtMovimientos);
                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                    drMovimientos = dtMovimientos.Select(filtro);

                    dvMovDolares.Sort = sOrden;
                    dvMovDolares.RowFilter = filtro;
                    drMovimientos = new DataRow[dvMovDolares.Count];
                    i = 0;
                    foreach (DataRowView drv in dvMovDolares)
                    {
                        drMovimientos[i] = drv.Row;
                        i++;
                    }

                    indiceBucle = 0;
                    totalSuscripciones = 0;
                    totalRescates = 0;
                    while (indiceBucle < 15)
                    {
                        if (drMovimientos.Length > indiceBucle)
                        {
                            Escribir(Convert.ToDateTime(drMovimientos[indiceBucle]["fecha"]), 10, "dd/MM/yyyy"); //campo224,231,238,245,252,259,266,273,280,287,294,301,308,315,322
                            Escribir(drMovimientos[indiceBucle]["nombrefondo"].ToString().Trim(), 30); //campo225,232,239,246,253,260,267,274,281,288,295,302,309,316,323
                            Escribir(drMovimientos[indiceBucle]["plan_ahorro"].ToString().Trim(), 11); //campo226,233,240,247,254,261,268,275,282,289,296,303,310,317,324
                            Escribir(drMovimientos[indiceBucle]["descripcion"].ToString().Trim(), 15); //campo227,234,241,248,255,262,269,276,283,290,297,304,311,318,325
                            if (drMovimientos[indiceBucle]["cuotas"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["cuotas"]), 20, Constantes.FormatoCuotas);  //campo228,235,242,249,256,263,270,277,284,291,298,305,312,319,326
                            }
                            else
                            {
                                EscribirVacio(20); //campo228,235,242,249,256,263,270,277,284,291,298,305,312,319,326
                            }
                            if (drMovimientos[indiceBucle]["valor_cuota"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["valor_cuota"]), 13, Constantes.FormatoValorCuota); //campo229,236,243,250,257,264,271,278,285,292,299,306,313,320,327
                            }
                            else
                            {
                                EscribirVacio(13); //campo229,236,243,250,257,264,271,278,285,292,299,306,313,320,327
                            }
                            if (drMovimientos[indiceBucle]["monto"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]), 30, Constantes.FormatoMonto); //campo230,237,244,251,258,265,272,279,286,293,300,307,314,321,328
                            }
                            else
                            {
                                EscribirVacio(30); //campo230,237,244,251,258,265,272,279,286,293,300,307,314,321,328
                            }
                            //Inicio OT9772 PSC001
                            //if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("SUS"))
                            //{
                            //    totalSuscripciones += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                            //}
                            //if (drMovimientos[indiceBucle]["tipoope"].ToString().Trim().Equals("RES"))
                            //{
                            //    totalRescates += Convert.ToDecimal(drMovimientos[indiceBucle]["monto"]);
                            //}
                            //Fin  OT9772 PSC001
                        }
                        else
                        {
                            EscribirVacio(10); //campo224,231,238,245,252,259,266,273,280,287,294,301,308,315,322
                            EscribirVacio(30); //campo225,232,239,246,253,260,267,274,281,288,295,302,309,316,323
                            EscribirVacio(11); //campo226,233,240,247,254,261,268,275,282,289,296,303,310,317,324
                            EscribirVacio(15); //campo227,234,241,248,255,262,269,276,283,290,297,304,311,318,325
                            EscribirVacio(20); //campo228,235,242,249,256,263,270,277,284,291,298,305,312,319,326
                            EscribirVacio(13); //campo229,236,243,250,257,264,271,278,285,292,299,306,313,320,327
                            EscribirVacio(30); //campo230,237,244,251,258,265,272,279,286,293,300,307,314,321,328
                        }
                        indiceBucle++;
                    }
                    //Inicio OT9772 PSC001
                    int indiceBucleTotalDol = 0;

                    if (drMovimientos.Length > 0)
                    {
                        while (drMovimientos.Length > indiceBucleTotalDol)
                        {
                            if (drMovimientos[indiceBucleTotalDol]["tipoope"].ToString().Trim().Equals("SUS"))
                            {
                                totalSuscripciones += Convert.ToDecimal(drMovimientos[indiceBucleTotalDol]["monto"]);
                            }
                            if (drMovimientos[indiceBucleTotalDol]["tipoope"].ToString().Trim().Equals("RES"))
                            {
                                totalRescates += Convert.ToDecimal(drMovimientos[indiceBucleTotalDol]["monto"]);
                            }

                            indiceBucleTotalDol = indiceBucleTotalDol + 1;
                        }
                        Escribir(totalSuscripciones, 30, Constantes.FormatoMonto); //campo329
                        Escribir(totalRescates, 30, Constantes.FormatoMonto); //campo330


                    }
                    //if (drMovimientos.Length > 0)
                    //{
                    //    Escribir(totalSuscripciones, 30, Constantes.FormatoMonto); //campo329
                    //    Escribir(totalRescates, 30, Constantes.FormatoMonto); //campo330
                    //} 
                    //Fin  OT9772 PSC001
                    else
                    {
                        EscribirVacio(30); //campo329
                        EscribirVacio(30); //campo330
                    }
                    #endregion
                    #endregion


                    //					Escribir(titulo,200); //OT-ING08032011
                    //					Escribir(mensaje, 1000);//campo331
                    //					Escribir(piepagina,200);//OT-ING08032011
                    //					Escribir(pie1,500);//OT-ING08032011
                    //					Escribir(pie2,500);//OT-ING08032011
                    //					Escribir(pie3,500);//OT-ING08032011
                    //					Escribir(pie4,500);//OT-ING08032011
                    //					Escribir(pie5,500);//OT-ING08032011
                    //					Escribir(pie6,500);//OT-ING08032011
                    //					// INI SR 19102010 - OT3199
                    //
                    //					Escribir(comentario, 100);//campo332
                    // FIN SR 19102010 - OT3199

                    //Escribir("imagen.jpg", 15); //campo341
                    /*
                    Escribir(drParticipe["nombre_promotor"].ToString().Trim(), 100); //campo338
                    Escribir(drParticipe["telefono_promotor"].ToString().Trim(), 50); //campo339
                    Escribir(drParticipe["correo_promotor"].ToString().Trim(), 100); //campo340
                    */


                    //INI - SALDO MILA

                    #region SALDOS DOLARES MILA

                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    indiceBucle = 0;
                    totalSaldoInicialFondos = 0;
                    totalSaldoFinalFondos = 0;


                    idFondo = 10;

                    filtro = string.Format("id_participe = {0} and moneda = 'DOL' and id_fondo = {1}", idParticipe, idFondo);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    if (drSaldos.Length > 0)
                    {
                        if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo41,47
                        }
                        else
                        {
                            EscribirVacio(20);
                        }
                        if (drSaldos[0]["valorcini"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo42,48
                        }
                        else
                        {
                            EscribirVacio(13);
                        }
                        if (drSaldos[0]["montoini"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo43,49
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos[0]["montoini"] != DBNull.Value)
                        {
                            //totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                        }

                        if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo44,50
                        }
                        else
                        {
                            EscribirVacio(20);
                        }
                        if (drSaldos[0]["valorcfin"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo45,51
                        }
                        else
                        {
                            EscribirVacio(13);
                        }
                        if (drSaldos[0]["montofin"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo46,52
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos[0]["montofin"] != DBNull.Value)
                        {
                            //totalSaldoFinalFondos+= Convert.ToDecimal(drSaldos[0]["montofin"]);
                        }

                        /*
                            if(drSaldos[indiceBucle]["valorcini"] !=DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[indiceBucle]["variacion"]), 50, Constantes.FormatoCuotas); //campo50,57
                            }
                            else
                            {
                                EscribirVacio(50); //campo50,57
                            }
                        */
                    }
                    else
                    {
                        EscribirVacio(20); //campo41,47
                        EscribirVacio(13); //campo42,48
                        EscribirVacio(30); //campo43,49
                        EscribirVacio(20); //campo44,50
                        EscribirVacio(13); //campo45,51
                        EscribirVacio(30); //campo46,52
                        //EscribirVacio(50); //campo50,57
                    }


                    //					filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                    //					drSaldos = dtSaldos.Select(filtro, "id_fondo");
                    //					if(drSaldos.Length > 0)
                    //					{
                    //						Escribir(totalSaldoInicialFondos, 30, Constantes.FormatoMonto); //campo53
                    //					}
                    //					else
                    //					{
                    //						EscribirVacio(30);
                    //					}
                    //					if(drSaldos.Length > 0)
                    //					{
                    //						Escribir(totalSaldoFinalFondos, 30, Constantes.FormatoMonto); //campo54
                    //					}
                    //					else
                    //					{
                    //						EscribirVacio(30);
                    //					}
                    #endregion


                    //FIN - SALDO MILA
                    Escribir(drParticipe["codigo_agencia"].ToString().Trim(), 3); //campo338 - Codigo Agencia origen

                    //INI - SALDO UCD
                    #region SALDOS UCD

                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    indiceBucle = 0;
                    totalSaldoInicialFondos = 0;
                    totalSaldoFinalFondos = 0;


                    idFondo = 11;

                    filtro = string.Format("id_participe = {0} and moneda = 'DOL' and id_fondo = {1}", idParticipe, idFondo);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    if (drSaldos.Length > 0)
                    {
                        if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo41,47
                        }
                        else
                        {
                            EscribirVacio(20);
                        }
                        if (drSaldos[0]["valorcini"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo42,48
                        }
                        else
                        {
                            EscribirVacio(13);
                        }
                        if (drSaldos[0]["montoini"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo43,49
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos[0]["montoini"] != DBNull.Value)
                        {
                            //totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                        }

                        if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo44,50
                        }
                        else
                        {
                            EscribirVacio(20);
                        }
                        if (drSaldos[0]["valorcfin"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo45,51
                        }
                        else
                        {
                            EscribirVacio(13);
                        }
                        if (drSaldos[0]["montofin"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo46,52
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos[0]["montofin"] != DBNull.Value)
                        {
                            //totalSaldoFinalFondos+= Convert.ToDecimal(drSaldos[0]["montofin"]);
                        }

                    }
                    else
                    {
                        EscribirVacio(20); //campo41,47
                        EscribirVacio(13); //campo42,48
                        EscribirVacio(30); //campo43,49
                        EscribirVacio(20); //campo44,50
                        EscribirVacio(13); //campo45,51
                        EscribirVacio(30); //campo46,52
                    }
                    #endregion
                    //FIN - SALDO UCD		


                    //INI - SALDO UCS
                    #region SALDOS UCS

                    filtro = string.Format("id_participe = {0} and moneda = 'SOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    indiceBucle = 0;
                    totalSaldoInicialFondos = 0;
                    totalSaldoFinalFondos = 0;


                    idFondo = 12;

                    filtro = string.Format("id_participe = {0} and moneda = 'SOL' and id_fondo = {1}", idParticipe, idFondo);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    if (drSaldos.Length > 0)
                    {
                        if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo41,47
                        }
                        else
                        {
                            EscribirVacio(20);
                        }
                        if (drSaldos[0]["valorcini"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo42,48
                        }
                        else
                        {
                            EscribirVacio(13);
                        }
                        if (drSaldos[0]["montoini"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo43,49
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos[0]["montoini"] != DBNull.Value)
                        {
                            //totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                        }

                        if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo44,50
                        }
                        else
                        {
                            EscribirVacio(20);
                        }
                        if (drSaldos[0]["valorcfin"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo45,51
                        }
                        else
                        {
                            EscribirVacio(13);
                        }
                        if (drSaldos[0]["montofin"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo46,52
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos[0]["montofin"] != DBNull.Value)
                        {
                            //totalSaldoFinalFondos+= Convert.ToDecimal(drSaldos[0]["montofin"]);
                        }

                    }
                    else
                    {
                        EscribirVacio(20); //campo41,47
                        EscribirVacio(13); //campo42,48
                        EscribirVacio(30); //campo43,49
                        EscribirVacio(20); //campo44,50
                        EscribirVacio(13); //campo45,51
                        EscribirVacio(30); //campo46,52
                    }
                    #endregion
                    //FIN - SALDO UCS	


                    //OT5803 - INICIO FONDOS ESTRATEGICOS
                    #region FONDOS ESTRATEGICOS

                    filtro = string.Format("id_participe = {0} and moneda = 'SOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    indiceBucle = 0;
                    totalSaldoInicialFondos = 0;
                    totalSaldoFinalFondos = 0;

                    idFondo = 0;

                    while (indiceBucle < 6)
                    {
                        switch (indiceBucle)
                        {
                            case 0:
                                idFondo = 14;
                                break;
                            case 1:
                                idFondo = 15;
                                break;
                            case 2:
                                idFondo = 16;
                                break;
                            case 3:
                                idFondo = 17;
                                break;
                            case 4:
                                idFondo = 18;
                                break;
                            case 5:
                                idFondo = 19;
                                break;
                        }
                        filtro = string.Format("id_participe = {0} and moneda = 'SOL' and id_fondo = {1}", idParticipe, idFondo);
                        drSaldos = dtSaldos.Select(filtro, "id_fondo");

                        if (drSaldos.Length > 0)
                        {

                            if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo53,59,65,71,77
                            }
                            else
                            {
                                EscribirVacio(20);
                            }
                            if (drSaldos[0]["valorcini"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo54,60,66,71,78
                            }
                            else
                            {
                                EscribirVacio(13);
                            }
                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo55,61,67,72,79
                            }
                            else
                            {
                                EscribirVacio(30);
                            }


                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                            }

                            if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo56,62,68,73,80
                            }
                            else
                            {
                                EscribirVacio(20);
                            }
                            if (drSaldos[0]["valorcfin"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo57,63,69,74,81
                            }
                            else
                            {
                                EscribirVacio(13);
                            }
                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo58,64,70,75,82
                            }
                            else
                            {
                                EscribirVacio(30);
                            }

                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                totalSaldoFinalFondos += Convert.ToDecimal(drSaldos[0]["montofin"]);
                            }

                        }
                        else
                        {
                            EscribirVacio(20); //campo53,59,65,71,77
                            EscribirVacio(13); //campo54,60,66,71,78
                            EscribirVacio(30); //campo55,61,67,72,79
                            EscribirVacio(20); //campo56,62,68,73,80
                            EscribirVacio(13); //campo57,63,69,74,81
                            EscribirVacio(30); //campo58,64,70,75,82                                                        
                        }
                        indiceBucle++;
                    }
                    filtro = string.Format("id_participe = {0} and moneda = 'SOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");
                    //if (drSaldos.Length > 0)
                    //{
                    //    Escribir(totalSaldoInicialFondos, 30, Constantes.FormatoMonto); //campo83
                    //}
                    //else
                    //{
                    //    EscribirVacio(30);
                    //}
                    //if (drSaldos.Length > 0)
                    //{
                    //    Escribir(totalSaldoFinalFondos, 30, Constantes.FormatoMonto); //campo84
                    //}
                    //else
                    //{
                    //    EscribirVacio(30);
                    //}
                    #endregion
                    //OT5803 - FIN FONDOS ESTRATEGICOS

                    //OT6134 - INICIO ACCIONES_USA y ACCIONES_EME
                    //OT6590 - Se agrega el fondo ACCIONES_EUR
                    #region ACCIONES_USA , ACCIONES_EME y ACCIONES_EUR

                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    indiceBucle = 0;
                    totalSaldoInicialFondos = 0;
                    totalSaldoFinalFondos = 0;

                    idFondo = 0;

                    while (indiceBucle < 3)
                    {
                        switch (indiceBucle)
                        {
                            case 0:
                                idFondo = 20;
                                break;
                            case 1:
                                idFondo = 21;
                                break;
                            case 2:
                                idFondo = 22;
                                break;   
                        }
                        filtro = string.Format("id_participe = {0} and moneda = 'DOL' and id_fondo = {1}", idParticipe, idFondo);
                        drSaldos = dtSaldos.Select(filtro, "id_fondo");

                        if (drSaldos.Length > 0)
                        {

                            if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo53,59,65,71,77
                            }
                            else
                            {
                                EscribirVacio(20);
                            }
                            if (drSaldos[0]["valorcini"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo54,60,66,71,78
                            }
                            else
                            {
                                EscribirVacio(13);
                            }
                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo55,61,67,72,79
                            }
                            else
                            {
                                EscribirVacio(30);
                            }


                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                            }

                            if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo56,62,68,73,80
                            }
                            else
                            {
                                EscribirVacio(20);
                            }
                            if (drSaldos[0]["valorcfin"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo57,63,69,74,81
                            }
                            else
                            {
                                EscribirVacio(13);
                            }
                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo58,64,70,75,82
                            }
                            else
                            {
                                EscribirVacio(30);
                            }

                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                totalSaldoFinalFondos += Convert.ToDecimal(drSaldos[0]["montofin"]);
                            }

                        }
                        else
                        {
                            EscribirVacio(20); //campo53,59,65,71,77
                            EscribirVacio(13); //campo54,60,66,71,78
                            EscribirVacio(30); //campo55,61,67,72,79
                            EscribirVacio(20); //campo56,62,68,73,80
                            EscribirVacio(13); //campo57,63,69,74,81
                            EscribirVacio(30); //campo58,64,70,75,82                                                        
                        }
                        indiceBucle++;
                    }
                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");                   
                    #endregion
                    //OT6134 - FIN ACCIONES_USA y ACCIONES_EME

                    //OT7833 INI
                    //INI - ESTSOLES
                    //OT9613 INI
                    //#region ESTSOLES
                    #region NOTESTRUCISOLES
                    //OT9613 FIN

                    filtro = string.Format("id_participe = {0} and moneda = 'SOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    indiceBucle = 0;
                    totalSaldoInicialFondos = 0;
                    totalSaldoFinalFondos = 0;

                    //OT9613 INI
                    //idFondo = 23;
                    idFondo = 44;
                    //OT9613 FIN

                    filtro = string.Format("id_participe = {0} and moneda = 'SOL' and id_fondo = {1}", idParticipe, idFondo);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    if (drSaldos.Length > 0)
                    {
                        if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo404
                        }
                        else
                        {
                            EscribirVacio(20);
                        }
                        if (drSaldos[0]["valorcini"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo405
                        }
                        else
                        {
                            EscribirVacio(13);
                        }
                        if (drSaldos[0]["montoini"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo406
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos[0]["montoini"] != DBNull.Value)
                        {
                            //totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                        }

                        if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo407
                        }
                        else
                        {
                            EscribirVacio(20);
                        }
                        if (drSaldos[0]["valorcfin"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo408
                        }
                        else
                        {
                            EscribirVacio(13);
                        }
                        if (drSaldos[0]["montofin"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo409
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos[0]["montofin"] != DBNull.Value)
                        {
                            //totalSaldoFinalFondos+= Convert.ToDecimal(drSaldos[0]["montofin"]);
                        }

                    }
                    else
                    {
                        EscribirVacio(20); //campo404
                        EscribirVacio(13); //campo405
                        EscribirVacio(30); //campo406
                        EscribirVacio(20); //campo407
                        EscribirVacio(13); //campo408
                        EscribirVacio(30); //campo409
                    }
                    #endregion
                    //FIN - ESTSOLES	
                    //OT7833 FIN
                    //OT 8365 INI
                    #region ESTSOLES2

                    filtro = string.Format("id_participe = {0} and moneda = 'SOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    indiceBucle = 0;
                    totalSaldoInicialFondos = 0;
                    totalSaldoFinalFondos = 0;


                    idFondo = 30;

                    filtro = string.Format("id_participe = {0} and moneda = 'SOL' and id_fondo = {1}", idParticipe, idFondo);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    if (drSaldos.Length > 0)
                    {
                        if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo404
                        }
                        else
                        {
                            EscribirVacio(20);
                        }
                        if (drSaldos[0]["valorcini"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo405
                        }
                        else
                        {
                            EscribirVacio(13);
                        }
                        if (drSaldos[0]["montoini"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo406
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos[0]["montoini"] != DBNull.Value)
                        {
                            //totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                        }

                        if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo407
                        }
                        else
                        {
                            EscribirVacio(20);
                        }
                        if (drSaldos[0]["valorcfin"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo408
                        }
                        else
                        {
                            EscribirVacio(13);
                        }
                        if (drSaldos[0]["montofin"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo409
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos[0]["montofin"] != DBNull.Value)
                        {
                            //totalSaldoFinalFondos+= Convert.ToDecimal(drSaldos[0]["montofin"]);
                        }

                    }
                    else
                    {
                        EscribirVacio(20); //campo404
                        EscribirVacio(13); //campo405
                        EscribirVacio(30); //campo406
                        EscribirVacio(20); //campo407
                        EscribirVacio(13); //campo408
                        EscribirVacio(30); //campo409
                    }
                    #endregion
                    //OT 8365 FIN
                    #region DEPDOL1

                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    indiceBucle = 0;
                    totalSaldoInicialFondos = 0;
                    totalSaldoFinalFondos = 0;


                    idFondo = 31;

                    filtro = string.Format("id_participe = {0} and moneda = 'DOL' and id_fondo = {1}", idParticipe, idFondo);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    if (drSaldos.Length > 0)
                    {
                        if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo404
                        }
                        else
                        {
                            EscribirVacio(20);
                        }
                        if (drSaldos[0]["valorcini"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo405
                        }
                        else
                        {
                            EscribirVacio(13);
                        }
                        if (drSaldos[0]["montoini"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo406
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos[0]["montoini"] != DBNull.Value)
                        {
                            //totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                        }

                        if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo407
                        }
                        else
                        {
                            EscribirVacio(20);
                        }
                        if (drSaldos[0]["valorcfin"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo408
                        }
                        else
                        {
                            EscribirVacio(13);
                        }
                        if (drSaldos[0]["montofin"] != DBNull.Value)
                        {
                            Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo409
                        }
                        else
                        {
                            EscribirVacio(30);
                        }
                        if (drSaldos[0]["montofin"] != DBNull.Value)
                        {
                            //totalSaldoFinalFondos+= Convert.ToDecimal(drSaldos[0]["montofin"]);
                        }

                    }
                    else
                    {
                        EscribirVacio(20); //campo404
                        EscribirVacio(13); //campo405
                        EscribirVacio(30); //campo406
                        EscribirVacio(20); //campo407
                        EscribirVacio(13); //campo408
                        EscribirVacio(30); //campo409
                    }
                    #endregion

                    //OT9426 INI
                    //INI - SEL_GLOBAL_IA y SEL_GLOBAL_IB
                    #region SEL_GLOBAL_IA y SEL_GLOBAL_IB

                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    indiceBucle = 0;
                    totalSaldoInicialFondos = 0;
                    totalSaldoFinalFondos = 0;

                    idFondo = 0;

                    while (indiceBucle < 2)
                    {
                        switch(indiceBucle)
                        {
                            case 0:
                                idFondo = 33;
                                break;
                            case 1:
                                idFondo = 34;
                                break;
                        }
                        filtro = string.Format("id_participe = {0} and moneda = 'DOL' and id_fondo = {1}", idParticipe, idFondo);
                        drSaldos = dtSaldos.Select(filtro, "id_fondo");

                        if (drSaldos.Length > 0)
                        {
                            if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo422,428
                            }
                            else
                            {
                                EscribirVacio(20);
                            }
                            if (drSaldos[0]["valorcini"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo423,429
                            }
                            else
                            {
                                EscribirVacio(13);
                            }
                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo424,430
                            }
                            else
                            {
                                EscribirVacio(30);
                            }

                            /*
                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                            }
                             * */

                            if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo425,431
                            }
                            else
                            {
                                EscribirVacio(20);
                            }
                            if (drSaldos[0]["valorcfin"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo426,432
                            }
                            else
                            {
                                EscribirVacio(13);
                            }
                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo427,433
                            }
                            else
                            {
                                EscribirVacio(30);
                            }

                            /*
                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                totalSaldoFinalFondos += Convert.ToDecimal(drSaldos[0]["montofin"]);
                            }
                            */
                        }
                        else
						{
                            EscribirVacio(20); //campo422,428
                            EscribirVacio(13); //campo423,429
                            EscribirVacio(30); //campo424,430
                            EscribirVacio(20); //campo425,431
                            EscribirVacio(13); //campo426,432
                            EscribirVacio(30); //campo427,433   
                        }
                        indiceBucle++;
                    }
                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");    
                    #endregion
                    //OT9426 FIN

                   //OT10546 INI PSC001
                    //INI - BON_GLO_A y BON_GLO_B
                    #region BON_GLO_A y BON_GLO_B

                    filtro = string.Format("id_participe = {0} and moneda = 'DOL'", idParticipe);
                    drSaldos = dtSaldos.Select(filtro, "id_fondo");

                    indiceBucle = 0;
                    totalSaldoInicialFondos = 0;
                    totalSaldoFinalFondos = 0;

                    idFondo = 0;

                    while (indiceBucle < 2)
                    {
                        switch (indiceBucle)
                        {
                            case 0:
                                idFondo = 50;
                                break;
                            case 1:
                                idFondo = 51;
                                break;
                        }
                        filtro = string.Format("id_participe = {0} and moneda = 'DOL' and id_fondo = {1}", idParticipe, idFondo);
                        drSaldos = dtSaldos.Select(filtro, "id_fondo");

                        if (drSaldos.Length > 0)
                        {
                            if (drSaldos[0]["cuotasinicial"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasinicial"]), 20, Constantes.FormatoCuotas); //campo434,440
                            }
                            else
                            {
                                EscribirVacio(20);
                            }
                            if (drSaldos[0]["valorcini"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcini"]), 13, Constantes.FormatoValorCuota); //campo435,441
                            }
                            else
                            {
                                EscribirVacio(13);
                            }
                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["montoini"]), 30, Constantes.FormatoMonto); //campo436,442
                            }
                            else
                            {
                                EscribirVacio(30);
                            }

                            /*
                            if (drSaldos[0]["montoini"] != DBNull.Value)
                            {
                                totalSaldoInicialFondos += Convert.ToDecimal(drSaldos[0]["montoini"]);
                            }
                             * */

                            if (drSaldos[0]["cuotasfinal"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["cuotasfinal"]), 20, Constantes.FormatoCuotas); //campo437,443
                            }
                            else
                            {
                                EscribirVacio(20);
                            }
                            if (drSaldos[0]["valorcfin"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["valorcfin"]), 13, Constantes.FormatoValorCuota); //campo438,444
                            }
                            else
                            {
                                EscribirVacio(13);
                            }
                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                Escribir(Convert.ToDecimal(drSaldos[0]["montofin"]), 30, Constantes.FormatoMonto); //campo439,445
                            }
                            else
                            {
                                EscribirVacio(30);
                            }

                            /*
                            if (drSaldos[0]["montofin"] != DBNull.Value)
                            {
                                totalSaldoFinalFondos += Convert.ToDecimal(drSaldos[0]["montofin"]);
                            }
                            */
                        }
                        else
                        {
                            EscribirVacio(20); //campo434,440
                            EscribirVacio(13); //campo435,441
                            EscribirVacio(30); //campo436,442
                            EscribirVacio(20); //campo437,443
                            EscribirVacio(13); //campo438,444
                            EscribirVacio(30); //campo439,445   
                        }
                        indiceBucle++;
                    }

                    #endregion
                    //OT10546 FIN PSC001
                    CambiarLinea();


                }

            }
            finally
            {
                sw.Close();
            }

        }

        public void SaveToFilePWD(string path, string strTipoEECC, int indicadorMasiva)
        {
            sw = new StreamWriter(path, false, Encoding.Default);
            try
            {
                EECCMasivoDA ec = new EECCMasivoDA();
                DataSet dsEECC = ec.ObtenerDataEECC(fechaInicio, fechaFin, strTipoEECC, indicadorMasiva);
                DataTable dtParticipes = dsEECC.Tables["PARTICIPES"];
                foreach (DataRow drParticipe in dtParticipes.Rows)
                {
                    Escribir(drParticipe["codigo"].ToString().Trim(), 8); //campo1
                    EscribirVacio(1);
                    Escribir(drParticipe["numero_doc"].ToString().Trim(), 20); //campo2
                    CambiarLinea();
                }
            }
            finally
            {
                sw.Close();
            }

        }

        public void SaveToFilePWD_FIR(string path, string strTipoEECC, int indicadorMasiva)
        {
            sw = new StreamWriter(path, false, Encoding.Default);
            try
            {
                EECCMasivoDA ec = new EECCMasivoDA();
                DataSet dsEECC = ec.ObtenerDataEECC_FIR(fechaInicio, fechaFin, strTipoEECC, indicadorMasiva);
                DataTable dtParticipes = dsEECC.Tables["PARTICIPES"];
                foreach (DataRow drParticipe in dtParticipes.Rows)
                {
                    Escribir(drParticipe["codigo"].ToString().Trim(), 8); //campo1
                    EscribirVacio(1);
                    Escribir(drParticipe["numero_doc"].ToString().Trim(), 20); //campo2
                    CambiarLinea();
                }
            }
            finally
            {
                sw.Close();
            }

        }


       

        /// <summary>
        /// Obtiene el mensaje que se insertará en la zona final del EECC
        /// </summary>
        /// <returns>DataTable: Datos del último mensaje registrado</returns>
        public DataTable ObtenerMensaje()
        {
            //OT-ING08032011 Obtener ultimo mensaje registrado
            EECCMasivoDA ec = new EECCMasivoDA();
            DataTable mensaje = ec.ObtenerMensaje();
            return mensaje;
        }

        /// <summary>
        /// Obtiene los meses del año
        /// </summary>
        /// <returns>DataTable: Datos de los meses</returns>
        public DataTable ObtenerMeses()
        {
            //OT-ING08032011 - Obtiene el listado de los meses matriculados en TABLA_GENERAL
            EECCMasivoDA ec = new EECCMasivoDA();
            DataTable meses = ec.ObtenerMeses();
            return meses;
        }


        /// <summary>
        /// Obtiene el mensaje que se insertará en la zona final del EECC
        /// </summary>
        /// <returns>DataTable: Datos del mensaje</returns>
        public DataTable ObtenerMensajexPeriodo(int mes, int anio)
        {
            //OT-ING08032011 - Obtiene mensaje correspondiente al periodo 
            EECCMasivoDA ec = new EECCMasivoDA();
            DataTable mensaje = ec.ObtenerMensajexPeriodo(mes, anio);
            return mensaje;
        }


        /// <summary>
        /// Obtiene los pies de página de acuerdo al periodo del EECC
        /// </summary>
        /// <returns>DataTable: Datos de los pies de página.</returns>
        public DataTable ObtenerPiesPaginaxPeriodo(int mes, int anio)
        {
            //OT-ING08032011 - Obtiene los pies del mensaje de acuerdo al periodo del EECC
            EECCMasivoDA ec = new EECCMasivoDA();
            DataTable piespagina = ec.ObtenerPiesPaginaxPeriodo(mes, anio);
            return piespagina;
        }


        /// <summary>
        /// Actualiza el mensaje final del EECC
        /// </summary>
        /// <param name="id">Indica el ID en Base de Datos del registro a actualizar</param>
        /// <param name="mensaje">Contiene el texto que será el nuevo mensaje del EECC</param>
        public void ActualizarMensaje(int anio, int mes, string titulo, string mensaje, string pie, string usuario)
        {
            //OT-ING08032011 Actualiza el mensaje.
            EECCMasivoDA ec = new EECCMasivoDA();
            ec.ActualizarMensaje(mes, anio, titulo, mensaje, pie, usuario);

        }

        /// <summary>
        /// Inserta nuevo mensaje para el EECC
        /// </summary>
        /// <param name="mes">Indica el mes del mensaje</param>
        /// <param name="anio">Indica el año para el cual se va a ingresar el mensaje</param>
        /// <param name="titulo">Es el titulo que tendra el mensaje dentro del EECC</param>
        /// <param name="mensaje">Es el cuerpo del mensaje dentro del EECC</param>
        /// <param name="pie">Es el pie del mensaje dentro del EECC</param>
        public void InsertarMensaje(int mes, int anio, string titulo, string mensaje, string pie, string usuario)
        {
            //OT-ING08032011 - Realiza la inserción de un nuevo mensaje para un periodo de EECC
            EECCMasivoDA ec = new EECCMasivoDA();
            ec.InsertarMensaje(mes, anio, titulo, mensaje, pie, usuario);
        }


        /// <summary>
        /// Inserta detalle de pies de mensaje de EECC
        /// </summary>
        /// <param name="mes">Indica el mes del mensaje</param>
        /// <param name="anio">Indica el año para el cual se va a ingresar el mensaje</param>
        /// <param name="pie1">Primer pie de página del EECC</param>
        /// <param name="pie2">Segundo pie de página del EECC</param>
        /// <param name="pie3">Tercer pie de página del EECC</param>
        /// <param name="pie4">Cuarto pie de página del EECC</param>
        /// <param name="pie5">Quinto pie de página del EECC</param>
        /// <param name="pie6">Sexto pie de página del EECC</param>
        /// <param name="usuario">Usuario que realiza la creación del registro</param>
        public void InsertarDetallesPie(int mes, int anio, string pie1, string pie2, string pie3, string pie4, string pie5, string pie6, string usuario, string flagSegundoGrupoDetalle, string flagAprobado)
        {
            //OT-ING08032011 - Realiza la inserción de los pies de página del mensaje
            EECCMasivoDA ec = new EECCMasivoDA();
            ec.InsertarDetallesPie(mes, anio, pie1, pie2, pie3, pie4, pie5, pie6, usuario, flagSegundoGrupoDetalle, flagAprobado); //OT 6304 
        }

        /*public void InsertarMuestraEECC(decimal cuc)
        {
            EstadoCuenta ec = new EstadoCuenta();
            ec.InsertarCucMuestra(cuc);
        }
        */
        /*		public void EliminarMuestraEECC(decimal cuc)
                {
                    EstadoCuenta ec = new EstadoCuenta();
                    ec.EliminarCucMuestra(cuc);
                }
        */
        /*		public DataTable ListarMuestraEECC()
                {
                    EstadoCuenta ec = new EstadoCuenta();
                    DataTable dtCucMuestra = ec.ObtenerListaCucMuestra();
                    return dtCucMuestra;
                }
        */
        //OT-ING08032011 - Se agrego método que realiza la carga de los cucs que se utilizarán para realizar la muestra.
        //OT4679 - Se añade parametro indicador
        public void CargarCucMuestra(DateTime finicio, DateTime ffinal, int indicador)
        {
            EECCMasivoDA ec = new EECCMasivoDA();
            ec.CargarCucsMuestra(finicio, ffinal, indicador);
        }

        //OT9772 INI
        public void CargarCucMuestra_PRI(DateTime finicio, DateTime ffinal, int indicador)
        {
            EECCMasivoDA ec = new EECCMasivoDA();
            ec.CargarCucsMuestra_PRI(finicio, ffinal, indicador);
        }
        //OT9772 FIN

        public DataTable ObtenerDataCargosEECC(DateTime fechaInicio, DateTime fechaFin, string strTipoEECC)
        {
            EECCMasivoDA ec = new EECCMasivoDA();
            return ec.ObtenerDataCargosEECC(fechaInicio, fechaFin, strTipoEECC);
        }

        public DataTable ObtenerTablaGeneralByCodigo(string codigo)
        {
            EECCMasivoDA ec = new EECCMasivoDA();
            return ec.ObtenerTablaGeneralByCodigo(codigo);

        }
        public decimal CargarDatosEECC(decimal idFondo, DateTime fechainicial, DateTime fechafinal)
        {
            EECCMasivoDA ec = new EECCMasivoDA();
            return ec.CargarDatosEECC(idFondo, fechainicial, fechafinal);
        }

        public DataTable ObtenerDataCargosEECC_FIR(DateTime fechaInicio, DateTime fechaFin, string strTipoEECC)
        {
            EECCMasivoDA ec = new EECCMasivoDA();
            return ec.ObtenerDataCargosEECC_FIR(fechaInicio, fechaFin, strTipoEECC);
        }

        //INI OTXXXX
        public DataTable ObtenerDataCargosEECC_PRI(DateTime fechaInicio, DateTime fechaFin, string strTipoEECC)
        {
            EECCMasivoDA ec = new EECCMasivoDA();
            return ec.ObtenerDataCargosEECC_PRI(fechaInicio, fechaFin, strTipoEECC);
        }
        //FIN OTXXXX

				//OT9772 PSC001 - INI
				public DataSet ObtenerDataEECC_PRI(DateTime fechaInicio, DateTime fechaFin, string strTipoEECC, int indicadorMasiva)
				{
					EECCMasivoDA ec = new EECCMasivoDA();
					DataSet dsEECC = ec.ObtenerDataEECC_PRI(fechaInicio, fechaFin, strTipoEECC, indicadorMasiva);
					return dsEECC;
				}
			//OT9772 PSC001 - FIN
    }
}
