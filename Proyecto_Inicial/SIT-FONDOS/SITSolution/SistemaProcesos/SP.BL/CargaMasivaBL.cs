/*
 * Fecha de Creación: 06/10/2016
 * Creado por: Irene Reyes
 * Numero de OT: 8954
 * Descripción del cambio: Creación
 * 
 */
/*
 * Fecha de Modificación: 20/10/2016
 * Modificado por: Irene Reyes
 * Numero de OT: 8954 PSC-001
 * Descripción del cambio: Agregar método CargarArchivoTraspasoMasivo() y agregar la nueva opción en el método Reversion().
 * 
 */
/*
 * Fecha de Modificación: 21/03/2017
 * Modificado por: Anthony Joaquin
 * Numero de OT: 10082 PSC001
 * Descripción del cambio: Agregar el método CargarArchivoFlujoMasivo()  para leer el archivo de excel,
 *                         se registra en la tabla operaciones y en la tabla alerta. 
 */
/*
 * Fecha de Modificación: 24/04/2017
 * Modificado por: Anthony Joaquin
 * Numero de OT: 10217
 * Descripción del cambio: Agregar método CargaArchivoConversionCuotasMasivo() y agregar la nueva opción en el método Reversion().
 * 
 */
/*
 * Fecha de Modificación: 07/06/2017
 * Modificado por: Robert Castillo
 * Numero de OT: 10433
 * Descripción del cambio: Realizar los siguientes cambios:
		1. Crear método CargaArchivoCuentasParticipacionMasivo, que se encargue de procesar el archivo de cuentas de participación.
		2. En el método Reversion agregar la llamada al método ActualizarCuentaParticipacion de la clase CargaMasivaDA.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Procesos.DA;
using INGFondos.Support.Interop;
using System.Globalization;
using System.Threading;
using System.Data;
using System.Collections;
using Procesos.TD;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Net;
using Procesos.Constants;
using System.Configuration;
using INGFondos.Constants;

namespace Procesos.BL
{
    public class CargaMasivaBL
    {
        CargaMasivaDA cargaMasivaDA = new CargaMasivaDA();

       

        
        #region
        public DataTable ListaTipoOperacion()
        {


            cargaMasivaDA.CrearConexion();
            cargaMasivaDA.AbrirConexion();
            cargaMasivaDA.AbrirTransaccion();

            DataTable listado;
            listado=cargaMasivaDA.ListarTablaGeneral(ConstantesING.CODIGO_CARGA_MASIVA);
            return listado;

        }
        #endregion

        #region
        public DataTable ListaLotes()
        {

            cargaMasivaDA.CrearConexion();
            cargaMasivaDA.AbrirConexion();
            cargaMasivaDA.AbrirTransaccion();

            DataTable listado;
            listado = cargaMasivaDA.ListarLotes();
            return listado;

        }
        #endregion

        #region
    
        public void CargarArchivoTraspasoFondoMasivo(string archivo, DateTime fechaSolicitud, string usuario,int numeroColumna, string dscTipoOperacion)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-PE");
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            #region Leer archivo Excel
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            ExcelApplication app = new ExcelApplication();
            ExcelWorkBook wb = app.OpenWorkBook(archivo, ExcelMode.Full);
            ExcelWorkSheet sheet = wb.GetSheet(1);

            //string prueba = sheet.GetString("B2");
            DataTable dt = sheet.LeerTabla("B", "I", 2, sheet.ObtenerUltimaFilaTabla("B", 2), true);
            wb.Close();
            app.Close();
            #endregion
           // dt.Columns.Count;
            int numeroColumnasArchivoSeleccionado = Convert.ToInt32(dt.Columns.Count.ToString());
            int numeroColumnasOperacion = numeroColumna;//btenerNumeroColumnasTipoOperacion(llave);
            if(numeroColumnasOperacion==numeroColumnasArchivoSeleccionado)
            {


                CargaMasivaDA da = new CargaMasivaDA();

                    da.CrearConexion();
                    da.AbrirConexion();
                    da.AbrirTransaccion();

                    DataTable dtFondos = da.ListarFondos();
                    DataTable dtCuentaRecaudo = da.ListarTablaGeneral(ConstantesING.CODIGO_TABLA_CUENTA_RECAUDO);

                    int numeroFila;
                    numeroFila = 0;
                    int numeroLote = da.ObtenerNumeroLote();
                    try
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            
                            int cuc = Convert.ToInt32(dr["CUC"]);
                            int idParticipe = cuc - 44670000;
                            string cuentaParticipacion = dr["CUENTA_PARTICIPACION"].ToString().Trim();
                            DataTable dtCuentasParticipacion = da.ListarCuentasParticipacion(idParticipe);
                            int idCuentaParticipacion = Convert.ToInt32(dtCuentasParticipacion.Select("CODIGO = '" + cuentaParticipacion + "'")[0]["ID"]);
                            string fondoOrigen = dr["FONDO_ORIGEN"].ToString().Trim();
                            string fondoDestino = dr["FONDO_DESTINO"].ToString().Trim();
                            int idFondo = Convert.ToInt32(dtFondos.Select("NOMBRE = '" + fondoOrigen + "'")[0]["ID"]);
                            DataTable dtFondo = da.ObtenerFondo(idFondo);
                            int plazo = Convert.ToInt32(dtFondo.Rows[0]["PLAZO_LIQUIDACION_DIAS_RESCATE"]);
                            DateTime horaHasta = Convert.ToDateTime(dtFondo.Rows[0]["HORARIO_OPERACION_HASTA"]);
                            DateTime fechaProceso = fechaSolicitud;
                            DateTime fechaPago = fechaSolicitud;

                            fechaProceso = fechaProceso.AddYears(1900 - fechaProceso.Year);
                            fechaProceso = fechaProceso.AddMonths(1 - fechaProceso.Month);
                            fechaProceso = fechaProceso.AddDays(1 - fechaProceso.Day);

                            if (fechaProceso > horaHasta)
                            {
                                fechaProceso = fechaSolicitud.AddDays(1);
                                while (fechaProceso.DayOfWeek == DayOfWeek.Saturday || fechaProceso.DayOfWeek == DayOfWeek.Sunday || 
                                    isFeriado(fechaProceso)==true  )
                                {
                                    fechaProceso.AddDays(1);
                                }
                                fechaProceso = fechaProceso.Date.AddHours(8);
                            }
                            else
                            {
                                fechaProceso = fechaSolicitud;
                            }

                            while (plazo > 0)
                            {
                                fechaPago = fechaPago.AddDays(1);
                                if (fechaPago.DayOfWeek == DayOfWeek.Saturday || fechaPago.DayOfWeek == DayOfWeek.Sunday ||
                                    isFeriado(fechaPago)==true)
                                {
                                    continue;
                                }
                                plazo--;
                            }
                            fechaPago = fechaPago.Date;

                            string monedaOrigen = dtFondos.Select("NOMBRE = '" + fondoOrigen + "'")[0]["CODIGO_MONEDA"].ToString();
                            string tipoRescate = dr["TIPO_RESCATE"].ToString().Trim();
                            decimal monto = Convert.ToDecimal(dr["MONTO"]);
                            decimal cuotas = Convert.ToDecimal(dr["CUOTAS"]);
                            int idFondoDestino = Convert.ToInt32(dtFondos.Select("NOMBRE = '" + fondoDestino + "'")[0]["ID"]);
                            string monedaDestino = dtFondos.Select("NOMBRE = '" + fondoDestino + "'")[0]["CODIGO_MONEDA"].ToString();
                            string cuentaRecaudo = dtCuentaRecaudo.Select("DESCRIPCION_CORTA = '" + idFondoDestino.ToString() + "'")[0]["LLAVE_TABLA"].ToString();

                            OperacionMasivo traspaso = new OperacionMasivo();
                            traspaso.IdParticipeOrigen = idParticipe;
                            traspaso.IdParticipeDestino = idParticipe;
                            traspaso.IdCuentaOrigen = idCuentaParticipacion;
                            traspaso.IdCuentaDestino = 0;
                            traspaso.IdFondoOrigen = idFondo;
                            traspaso.IdFondoDestino = idFondoDestino;
                            traspaso.FlagMonedaDistinto = monedaOrigen.Equals(monedaDestino) ? ConstantesING.NO : ConstantesING.SI;
                            traspaso.Usuario = usuario;
                            traspaso.Area = ConstantesING.AREA_TRASPASO_FONDO;
            
                            //OPERACIONES DESDE LA BASE DE DATOS
                           /* operacionRescateTraspasoFondos(dscTipoOperacion,numeroFila , da, traspaso, idParticipe,idFondo,idCuentaParticipacion,tipoRescate, monto,
                                cuotas,usuario,cuentaRecaudo,fechaSolicitud,fechaProceso,fechaPago);*/

                            int idTraspasoFondo = da.InsertarTraspasoFondo(traspaso);

                            string promotor = da.ObtenerPromotor(idParticipe);

                            decimal vc = da.ObtenerUltimoValorCuotaFondo(idFondo);

                            if (tipoRescate == ConstantesING.TIPO_RESCATE_TOTAL)
                            {
                                cuotas = da.ObtenerSaldoCuotasxCuentaParticipacion(idFondo, idCuentaParticipacion);
                            }

                            string numeroOperacionTraspasoFondo = da.InsertarRescateTraspaso(idParticipe, idFondo, idCuentaParticipacion, idTraspasoFondo, tipoRescate, monto, cuotas, vc, promotor, usuario, cuentaRecaudo, fechaSolicitud, fechaProceso, fechaPago);

                            //INSERTANDO EN ALERTA
                           

                            string dscTipoOpe = ConstantesING.TITULO_TRASPASO_FONDO;
                            int numOpeTraspasoFondo = Convert.ToInt32(numeroOperacionTraspasoFondo);
                            //int numOpeTraspasoFondo = 1;

														//OT10433 INI
														//da.InsertarAlerta(dscTipoOpe, numOpeTraspasoFondo, usuario, numeroLote, string.Empty);
														da.InsertarAlerta(dscTipoOpe, dscTipoOpe, numOpeTraspasoFondo, usuario, numeroLote, string.Empty);
														//OT10433 FIN
                            numeroFila = numeroFila + 1;
                        }

                        da.CommitTransaccion();
                        
                        MessageBox.Show("Se cargaron " + (numeroFila).ToString()+ " registros de " + dscTipoOperacion+" con el siguiente número de lote "+numeroLote.ToString(), "Mensaje");

                    }
                    catch (Exception ex)
                    {

                        da.RollbackTransaccion();
        
                         MessageBox.Show("Error en la carga del archivo  en el número de fila "+(numeroFila+2).ToString()+" "+ex,"Atención");

                    }
                    finally
                    {
                        da.LiberarConexion();
                        Thread.CurrentThread.CurrentCulture = originalCulture;
                    }
                     
            }
            else
            {
                MessageBox.Show("La plantilla del archivo no le pertenece a este tipo de operación","Atención",MessageBoxButtons.OKCancel);
            }
        }
        #endregion

  

        #region
        public void CargarArchivoRescateMasivo(string archivo, DateTime fechaSolicitud, string usuario, int numeroColumna, string dscTipoOperacion)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-PE");
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            #region Leer archivo Excel
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            ExcelApplication app = new ExcelApplication();
            ExcelWorkBook wb = app.OpenWorkBook(archivo, ExcelMode.Full);
            ExcelWorkSheet sheet = wb.GetSheet(1);

            DataTable dt = sheet.LeerTabla("B", "M", 2, sheet.ObtenerUltimaFilaTabla("B", 2), true);
            wb.Close();
            app.Close();
            #endregion

            int numeroColumnasArchivoSeleccionado = Convert.ToInt32(dt.Columns.Count.ToString());
            int numeroColumnasOperacion = numeroColumna;//btenerNumeroColumnasTipoOperacion(llave);
            if (numeroColumnasOperacion == numeroColumnasArchivoSeleccionado)
            {

                CargaMasivaDA da = new CargaMasivaDA();

                da.CrearConexion();
                da.AbrirConexion();
                da.AbrirTransaccion();

                DataTable dtFondos = da.ListarFondos();
                DataTable dtCuentaRecaudo = da.ListarTablaGeneral(ConstantesING.CODIGO_TABLA_CUENTA_RECAUDO);
                int numeroFila = 0;
                int numeroLote = da.ObtenerNumeroLote();
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int cuc = Convert.ToInt32(dr["CUC"]);
                        int idParticipe = cuc - 44670000;
                        string cuentaParticipacion = dr["CUENTA_PARTICIPACION"].ToString().Trim();
                        DataTable dtCuentasParticipacion = da.ListarCuentasParticipacion(idParticipe);
                        int idCuentaParticipacion = Convert.ToInt32(dtCuentasParticipacion.Select("CODIGO = '" + cuentaParticipacion + "'")[0]["ID"]);
                        string fondoOrigen = dr["FONDO_ORIGEN"].ToString().Trim();

                        int idFondo = Convert.ToInt32(dtFondos.Select("NOMBRE = '" + fondoOrigen + "'")[0]["ID"]);
                        DataTable dtFondo = da.ObtenerFondo(idFondo);
                        int plazo = Convert.ToInt32(dtFondo.Rows[0]["PLAZO_LIQUIDACION_DIAS_RESCATE"]);
                        DateTime horaHasta = Convert.ToDateTime(dtFondo.Rows[0]["HORARIO_OPERACION_HASTA"]);
                        DateTime fechaProceso = fechaSolicitud;
                        DateTime fechaPago = fechaSolicitud;

                        string viaPago = dr["CODIGO_BANCO"].ToString().Trim();
                        string formaPago = dr["FORMA_PAGO"].ToString().Trim();
                        string cuentaBancaria = dr["CUENTA_BANCARIA"].ToString().Trim();
                        string comentario = dr["COMENTARIO"].ToString().Trim();

                        int idCuentaBancaria = 0;

                        if (formaPago == ConstantesING.CODIGO_FORMA_PAGO_TRASPASO_FONDOS)
                        {
                            DataTable dtCuentaBancaria = da.ListarCuentasBancarias(idParticipe);
                            if (cuentaBancaria.Equals(string.Empty))
                            {
                                if (dtCuentaBancaria.Select("COMENTARIO = '" + comentario + "'").Length == 0)
                                {
                                    throw new Exception("El CUC " + cuc.ToString() + "no tiene una cuenta bancaria que cumpla con el comentario.");
                                }
                                viaPago = dtCuentaBancaria.Select("COMENTARIO = '" + comentario + "'")[0]["CODIGO_BANCO"].ToString();
                                idCuentaBancaria = Convert.ToInt32(dtCuentaBancaria.Select("COMENTARIO = '" + comentario + "'")[0]["ID"]);
                            }
                            else
                            {
                                if (dtCuentaBancaria.Select("NUMERO_CUENTA = '" + cuentaBancaria + "'").Length == 0)
                                {
                                    throw new Exception("El número de la cuenta bancaria ingresada no está registrada para el CUC " + cuc.ToString());
                                }
                                idCuentaBancaria = Convert.ToInt32(dtCuentaBancaria.Select("NUMERO_CUENTA = '" + cuentaBancaria + "'")[0]["ID"]);
                                //Prueba idCuentaBancaria = 6879;
                            }
                        }


                        fechaProceso = fechaProceso.AddYears(1900 - fechaProceso.Year);
                        fechaProceso = fechaProceso.AddMonths(1 - fechaProceso.Month);
                        fechaProceso = fechaProceso.AddDays(1 - fechaProceso.Day);

                        if (fechaProceso > horaHasta)
                        {
                            fechaProceso = fechaSolicitud.AddDays(1);
                            while (fechaProceso.DayOfWeek == DayOfWeek.Saturday || fechaProceso.DayOfWeek == DayOfWeek.Sunday || isFeriado(fechaProceso) == true)
                            {
                                fechaProceso.AddDays(1);
                            }
                            fechaProceso = fechaProceso.Date.AddHours(8);
                        }
                        else
                        {
                            fechaProceso = fechaSolicitud;
                        }

                        while (plazo > 0)
                        {
                            fechaPago = fechaPago.AddDays(1);
                            if (fechaPago.DayOfWeek == DayOfWeek.Saturday || fechaPago.DayOfWeek == DayOfWeek.Sunday || isFeriado(fechaPago) == true)
                            {
                                continue;
                            }
                            plazo--;
                        }
                        fechaPago = fechaPago.Date;

                        string monedaOrigen = dtFondos.Select("NOMBRE = '" + fondoOrigen + "'")[0]["CODIGO_MONEDA"].ToString();
                        string tipoRescate = dr["TIPO_RESCATE"].ToString().Trim();
                        decimal monto = Convert.ToDecimal(dr["MONTO"]);
                        decimal cuotas = Convert.ToDecimal(dr["CUOTAS"]);

                        string promotor = da.ObtenerPromotor(idParticipe);

                        decimal vc = da.ObtenerUltimoValorCuotaFondo(idFondo);

                        if (tipoRescate == "TOTAL")
                        {
                            cuotas = da.ObtenerSaldoCuotasxCuentaParticipacion(idFondo, idCuentaParticipacion);
                        }


                        //operacionRescate(dscTipoOperacion, da, numeroColumna, idParticipe, idFondo, idCuentaParticipacion, 0, tipoRescate, monto, cuotas, vc, promotor, usuario, string.Empty, fechaSolicitud, fechaProceso, fechaPago, viaPago, formaPago, idCuentaBancaria);

                        //INSERTANDO EN OPERACIONES
                        string numeroOperacionRescate =
                        da.InsertarRescate(idParticipe, idFondo, idCuentaParticipacion, 0, tipoRescate, monto, cuotas, vc, promotor, usuario, string.Empty, fechaSolicitud, fechaProceso, fechaPago, viaPago, formaPago, idCuentaBancaria);

                        //Ingresando en alerta
                        
                        string dscTipoOpe = ConstantesING.TITULO_RESCATE;
                        int numOpeRescate = Convert.ToInt32(numeroOperacionRescate);
												//OT10433 INI
												//da.InsertarAlerta(dscTipoOpe, numOpeRescate, usuario, numeroLote, string.Empty);
												da.InsertarAlerta(dscTipoOpe, dscTipoOpe, numOpeRescate, usuario, numeroLote, string.Empty);
												//OT10433 FIN

                        numeroFila = numeroFila + 1;
                    }

                    da.CommitTransaccion();

                    


                    MessageBox.Show("Se cargaron " + numeroFila + " registros de " + dscTipoOperacion +" "+ "con el siguiente número de lote " +numeroLote.ToString(), "Mensaje");

                }
                catch (Exception ex)
                {
                    da.RollbackTransaccion();
                    MessageBox.Show("Error en la carga del archivo  en el número de fila " + (numeroFila + 2).ToString() + " " + ex.ToString(), "Atención");
                }
                finally
                {
                    da.LiberarConexion();
                    Thread.CurrentThread.CurrentCulture = originalCulture;
                }
            }
            else
            {
                MessageBox.Show("La plantilla del archivo no le pertenece a este tipo de operación", "Atención", MessageBoxButtons.OKCancel);
            }

        }
            
        #endregion


				//OT10082 PSC001 INI
				public void CargarArchivoFlujoMasivo(string archivo, DateTime fechaSolicitud, string usuario, int numeroColumna, string dscTipoOperacion)
				{
					Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-PE");
					CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

					#region Leer archivo Excel
					Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
					ExcelApplication app = new ExcelApplication();
					ExcelWorkBook wb = app.OpenWorkBook(archivo, ExcelMode.Full);
					ExcelWorkSheet sheet = wb.GetSheet(1);

					DataTable dt = sheet.LeerTabla("B", "D", 2, sheet.ObtenerUltimaFilaTabla("B", 2), true);
					wb.Close();
					app.Close();
					#endregion

					int numeroColumnasArchivoSeleccionado = Convert.ToInt32(dt.Columns.Count.ToString());
					int numeroColumnasOperacion = numeroColumna;//btenerNumeroColumnasTipoOperacion(llave);
					if (numeroColumnasOperacion == numeroColumnasArchivoSeleccionado)
					{

						CargaMasivaDA da = new CargaMasivaDA();

						da.CrearConexion();
						da.AbrirConexion();
						da.AbrirTransaccion();

						DataTable dtFondos = da.ListarFondos();
						DataTable dtCuentaRecaudo = da.ListarTablaGeneral(ConstantesING.CODIGO_TABLA_CUENTA_RECAUDO);
						int numeroFila = 0;
						int numeroLote = da.ObtenerNumeroLote();
						try
						{
							foreach (DataRow dr in dt.Rows)
							{
								int cuc = Convert.ToInt32(dr["CUC"]);
								int idParticipe = cuc - 44670000;
								////if (idParticipe == 44715637)
								////{
								////    Console.WriteLine("AQUI");
								////}
								//string cuentaParticipacion = dr["CUENTA_PARTICIPACION"].ToString().Trim();
								//DataTable dtCuentasParticipacion = da.ListarCuentasParticipacion(idParticipe);
								//int idCuentaParticipacion = Convert.ToInt32(dtCuentasParticipacion.Select("CODIGO = '" + cuentaParticipacion + "'")[0]["ID"]);
								string fondoOrigen = dr["FONDO_ORIGEN"].ToString().Trim();

								int idFondo = Convert.ToInt32(dtFondos.Select("NOMBRE = '" + fondoOrigen + "'")[0]["ID"]);

								//DataRow[] drFon = dtFondos.Select("NOMBRE = '" + fondoOrigen + "'");
								//int idFondo = 0;

								//if (drFon.Length > 0)
								//{
								//    idFondo = Convert.ToInt32(drFon[0]["ID"]);
								//}


								DataTable dtFondo = da.ObtenerFondo(idFondo);
								int plazo = Convert.ToInt32(dtFondo.Rows[0]["PLAZO_LIQUIDACION_DIAS_RESCATE"]);
								DateTime horaHasta = Convert.ToDateTime(dtFondo.Rows[0]["HORARIO_OPERACION_HASTA"]);
								DateTime fechaProceso = fechaSolicitud;
								DateTime fechaPago = fechaSolicitud;

								string viaPago = string.Empty;// dr["CODIGO_BANCO"].ToString().Trim();
								string formaPago = "ABO";
								string cuentaBancaria = string.Empty;// dr["CUENTA_BANCARIA"].ToString().Trim();
								string comentario = string.Empty;// dr["COMENTARIO"].ToString().Trim();

								int idCuentaBancaria = 0;

								//BUSQUEDA DE LA CUENTA BANCARIA
								DataTable dtCuentaBancaria = da.ListarCuentasBancarias(idParticipe);
								if (cuentaBancaria.Equals(string.Empty))
								{
									string filtroCuenta = "ID_FONDO = " + idFondo.ToString() + " AND ESTADO = 'ACT' AND FLAG_ELIMINADO = 'N'";
									if (dtCuentaBancaria.Select(filtroCuenta).Length == 0)
									{
										throw new Exception("El CUC " + cuc.ToString() + "no tiene una cuenta bancaria que coincida con el fondo.");
									}
									viaPago = dtCuentaBancaria.Select(filtroCuenta)[0]["CODIGO_BANCO"].ToString();
									idCuentaBancaria = Convert.ToInt32(dtCuentaBancaria.Select(filtroCuenta)[0]["ID"]);
								}

								fechaProceso = fechaProceso.AddYears(1900 - fechaProceso.Year);
								fechaProceso = fechaProceso.AddMonths(1 - fechaProceso.Month);
								fechaProceso = fechaProceso.AddDays(1 - fechaProceso.Day);

								if (fechaProceso > horaHasta)
								{
									fechaProceso = fechaSolicitud.AddDays(1);
									while (fechaProceso.DayOfWeek == DayOfWeek.Saturday || fechaProceso.DayOfWeek == DayOfWeek.Sunday || isFeriado(fechaProceso) == true)
									{
										fechaProceso.AddDays(1);
									}
									fechaProceso = fechaProceso.Date.AddHours(8);
								}
								else
								{
									fechaProceso = fechaSolicitud;
								}

								while (plazo > 0)
								{
									fechaPago = fechaPago.AddDays(1);
									if (fechaPago.DayOfWeek == DayOfWeek.Saturday || fechaPago.DayOfWeek == DayOfWeek.Sunday || isFeriado(fechaPago) == true)
									{
										continue;
									}
									plazo--;
								}
								fechaPago = fechaPago.Date;

								string monedaOrigen = dtFondos.Select("NOMBRE = '" + fondoOrigen + "'")[0]["CODIGO_MONEDA"].ToString();
								//string tipoRescate = "";// dr["TIPO_RESCATE"].ToString().Trim();
								decimal monto = Convert.ToDecimal(dr["MONTO"]);
								//decimal cuotas = 0;// Convert.ToDecimal(dr["CUOTAS"]);

								string promotor = da.ObtenerPromotor(idParticipe);

								decimal vc = da.ObtenerUltimoValorCuotaFondo(idFondo);

								//if (tipoRescate == "TOTAL")
								//{
								//    cuotas = da.ObtenerSaldoCuotasxCuentaParticipacion(idFondo, idCuentaParticipacion);
								//}


								//operacionRescate(dscTipoOperacion, da, numeroColumna, idParticipe, idFondo, idCuentaParticipacion, 0, tipoRescate, monto, cuotas, vc, promotor, usuario, string.Empty, fechaSolicitud, fechaProceso, fechaPago, viaPago, formaPago, idCuentaBancaria);

								//INSERTANDO EN OPERACIONES
								string numeroOperacionRescate =
								da.InsertarFlujo(idParticipe, idFondo, monto, vc, promotor, usuario, fechaSolicitud, fechaProceso, fechaPago, viaPago, formaPago, idCuentaBancaria);

								//Ingresando en alerta

								string dscTipoOpe = ConstantesING.TITULO_RESCATE;
								int numOpeRescate = Convert.ToInt32(numeroOperacionRescate);
								//OT10433 INI
								//da.InsertarAlerta(dscTipoOpe, numOpeRescate, usuario, numeroLote, string.Empty);
								da.InsertarAlerta(dscTipoOpe, dscTipoOpe, numOpeRescate, usuario, numeroLote, string.Empty);
								//OT10433 FIN
								numeroFila = numeroFila + 1;
							}

							da.CommitTransaccion();

							MessageBox.Show("Se cargaron " + numeroFila + " registros de " + dscTipoOperacion + " " + "con el siguiente número de lote " + numeroLote.ToString(), "Mensaje");

						}
						catch (Exception ex)
						{
							da.RollbackTransaccion();
							MessageBox.Show("Error en la carga del archivo  en el número de fila " + (numeroFila + 2).ToString() + " " + ex.ToString(), "Atención");
						}
						finally
						{
							da.LiberarConexion();
							Thread.CurrentThread.CurrentCulture = originalCulture;
						}
					}
					else
					{
						MessageBox.Show("La plantilla del archivo no le pertenece a este tipo de operación", "Atención", MessageBoxButtons.OKCancel);
					}

				}
				//OT10082 PSC001 FIN

        #region
        private bool isFeriado(DateTime fecha)
        {
            int cantidad =cargaMasivaDA.ListarFechasFeriados(fecha).Rows.Count;
            if (cantidad == 0)
            {
                return false;
            }
            else
            {
                return true;
            }


        }
        #endregion

        
        #region

        public void CargarArchivoAbonosMasivo(string patch, DateTime fechaSolicitud, string usuario, int numeroColumna, string dscTipoOperacion)
        {

            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-PE");
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            #region Leer archivo Excel
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            ExcelApplication app = new ExcelApplication();
            ExcelWorkBook wb = app.OpenWorkBook(patch, ExcelMode.Full);
            ExcelWorkSheet sheet = wb.GetSheet(1);

            
            DataTable dtAbonos = sheet.LeerTabla("B", "G", 2, sheet.ObtenerUltimaFilaTabla("B", 2), true);
            
            wb.Close();
            app.Close();
            #endregion
            

            int numeroColumnasArchivoSeleccionado = Convert.ToInt32(dtAbonos.Columns.Count.ToString());
            int numeroColumnasOperacion = numeroColumna;//btenerNumeroColumnasTipoOperacion(llave);
            if (numeroColumnasOperacion == numeroColumnasArchivoSeleccionado)
            {

                CargaMasivaDA da = new CargaMasivaDA();

                da.CrearConexion();
                da.AbrirConexion();
                da.AbrirTransaccion();

                da.CrearConexionDeposito();
                da.AbrirConexionDeposito();
                da.AbrirTransaccionDeposito();
                DataTable dtFondos = da.ObtenerFondosOperaciones();

                int numeroFila;
                numeroFila = 0;
                int numeroLote = da.ObtenerNumeroLote();

                try
                {
                    

                    foreach (DataRow dr in dtAbonos.Rows)
                    {
                        AbonoMasivo abo = new AbonoMasivo();

                  
                            abo.NumOperacionBancaria = dr["NUM_OPERACION_BAN"].ToString();
                            abo.Cuc =Convert.ToInt32(dr["CUC"].ToString());
														//OT10433 INI		
														string monto = dr["MONTO"].ToString().Trim();
                            abo.Monto = Convert.ToDecimal(monto);
														//abo.Monto = Convert.ToDecimal(Convert.ToString(dr["MONTO"]));
														//OT10433 FIN
														abo.Fondo =dr["FONDO"].ToString();
                            abo.Referencia = dr["REFERENCIA"].ToString();
                            abo.FechaContable = fechaSolicitud;
                            abo.FechaDisponible = fechaSolicitud;


                            abo.IdFondo = Convert.ToInt32(dr["ID_FONDO"].ToString());
                                
                             //Convert.ToInt32(dtFondos.Select("NOMBRE = '"+abo.Fondo+"'")[0]["ID"]);

                            //operacionAbonoMasivo(da, abo, usuario, dscTipoOperacion, numeroFila);

                            //OPERACIONES 

                        string  numeroOperacionAbonoMasivo = da.CargarAbonosMasivo(abo, usuario);
												//OT10433 INI
												int idOperacion = 0;

                        //Ingresando en alerta
                        //int numOpeAbonoMasivo = Convert.ToInt32(numeroOperacionAbonoMasivo);
                        
                        string dscTipoOpe = ConstantesING.TITULO_ABONO;
												//da.InsertarAlerta(dscTipoOpe, numOpeAbonoMasivo, usuario, numeroLote,string.Empty);
												da.InsertarAlerta(dscTipoOpe, numeroOperacionAbonoMasivo, idOperacion, usuario, numeroLote, string.Empty);
												//OT10433 FIN

                        numeroFila = numeroFila + 1;
                    }
                    da.CommitTransaccion();
                    da.CommitTransaccionDeposito();
                  
                    MessageBox.Show("Se cargaron " + numeroFila + " registros de " + dscTipoOperacion + " con el siguiente número de lote " + numeroLote.ToString(), "Mensaje");

                }
                catch (Exception ex)
                {
                    da.RollbackTransaccion();
                    da.RollbackTransaccionDeposito();
                    MessageBox.Show("Error en la carga del archivo  en el número de fila " + (numeroFila + 2).ToString() + " " + ex.ToString(), "Atención");

            
                }
                finally
                {
                    da.LiberarConexion();
                    da.LiberarConexionDeposito();
                    Thread.CurrentThread.CurrentCulture = originalCulture;
                }
            }
            else
            {
                MessageBox.Show("La plantilla del archivo no le pertenece a este tipo de operación", "Atención", MessageBoxButtons.OKCancel);
            }

        }
        #endregion

       

        #region

        public void Reversion(int lote, string codigoUsuario)
        {
            CargaMasivaDA da = new CargaMasivaDA();

            da.CrearConexion();
            da.AbrirConexion();
            da.AbrirTransaccion();

            da.CrearConexionDeposito();
            da.AbrirConexionDeposito();
            da.AbrirTransaccionDeposito();

            DataTable dtRegistrosAlerta;
            dtRegistrosAlerta = ListaLotes();
            DataRow[] dtRegistroAlertaLotes = dtRegistrosAlerta.Select("SUBTIPO_OTRO = '" + lote.ToString() + "'");
            int numero = 0;
            string tipoOperacion="";
           
            try
            {
                foreach (DataRow dr in dtRegistroAlertaLotes)
                {
                    string titulo = dr["TITULO"].ToString();
										//OT10433 INI 
										//int idOperacion = Convert.ToInt32(dr["ID_OPERACION"]);
										int idOperacion = 0;
										if (dr["ID_OPERACION"] != null && dr["ID_OPERACION"] != DBNull.Value)
										{
												idOperacion = Convert.ToInt32(dr["ID_OPERACION"]);
										}
										string idRegistroCadena = Convert.ToString(dr["DESCRIPCION"]); 
										int idRegistro = 0;
										//OT10433 FIN

                    switch (titulo)
                    {
                        case "Carga Masiva de Abonos Masivos":
														 //OT10433 INI 		
														 idRegistro = Convert.ToInt32(idRegistroCadena);  
                             //da.ActualizarDeposito(codigoUsuario, idOperacion);
														 da.ActualizarDeposito(codigoUsuario, idRegistro);
														 //OT10433 FIN	
														 tipoOperacion="Abonos Masivos";
                            break;
                        case "Carga Masiva de Rescates Masivos":
                            da.ActualizarOperacion(codigoUsuario, idOperacion);
                            tipoOperacion="Rescates Masivos";
                            break;
                        case "Carga Masiva de Traspasos Fondos Masivos":
                            da.ActualizarTraspasoFondo(codigoUsuario, idOperacion);
                            da.ActualizarOperacion(codigoUsuario, idOperacion);
                           tipoOperacion="Traspasos Fondos Masivos";
                            break;
                        //OT8954 PSC-001 INI
                        case "Carga Masiva de Traspaso Masivo":
                            da.ActualizarOperacion(codigoUsuario, idOperacion);
                            tipoOperacion="Traspaso Masivo";
                            break;
                        //OT8954 PSC-001 FIN
                        //OT10217 INI
												case "Carga Masiva de Conversion de Cuotas Masivo":
                            da.ActualizarOperacion(codigoUsuario, idOperacion);
                            tipoOperacion = "Conversión de Cuotas";
                            break; 
                        //OT10217 FIN

												//OT10433 INI
                        case "Carga Masiva de Cuentas de Participación":

														idRegistro = Convert.ToInt32(idRegistroCadena);
														da.ActualizarCuentaParticipacion(codigoUsuario, idRegistro);
                            tipoOperacion = "Cuentas de Participación";
                            break;
												//OT10433 FIN
                    }
                    //Insertando en alerta para una evidencia de la reversión
                    numero = numero + 1;
                }
								//OT10433 INI 
								int identificadorOperacion = 0;
								//da.InsertarAlerta(ConstantesING.TITULO_REVERSION, 0, codigoUsuario, 0, "REV");
								da.InsertarAlerta(ConstantesING.TITULO_REVERSION, ConstantesING.TITULO_REVERSION, identificadorOperacion, codigoUsuario, 0, "REV");
								//OT10433 FIN

                da.CommitTransaccion();
                da.CommitTransaccionDeposito();
                MessageBox.Show("Se reversaron "+ numero+" "+tipoOperacion, "Mensaje");
            }
            catch (Exception ex)
            {
                da.RollbackTransaccion();
                da.RollbackTransaccionDeposito();
                MessageBox.Show("Error en el proceso de reversión "+ex.ToString(), "Atención");
            }
            finally
            {
                da.LiberarConexion();
                da.LiberarConexionDeposito(); 
            }
        }
        #endregion

        //OT8954 PSC-001 INI
        #region
        public void CargarArchivoTraspasoMasivo(string rutaArchivo, DateTime fechaSeleccionada, string codigoUsuario, int numero, string descTipoOperacion)
        {
             Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-PE");
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            #region Leer archivo Excel
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            ExcelApplication app = new ExcelApplication();
            ExcelWorkBook wb = app.OpenWorkBook(rutaArchivo, ExcelMode.Full);
            ExcelWorkSheet sheet = wb.GetSheet(1);

            
            DataTable dtTraspaso = sheet.LeerTabla("B", "I", 2, sheet.ObtenerUltimaFilaTabla("B", 2), true);
            
            wb.Close();
            app.Close();
            #endregion
            

            int numeroColumnasArchivoSeleccionado = Convert.ToInt32(dtTraspaso.Columns.Count.ToString());
            int numeroColumnasOperacion = numero;//btenerNumeroColumnasTipoOperacion(llave);
            if (numeroColumnasOperacion == numeroColumnasArchivoSeleccionado)
            {

                CargaMasivaDA da = new CargaMasivaDA();

                da.CrearConexion();
                da.AbrirConexion();
                da.AbrirTransaccion();

                 int numeroFila;
                numeroFila = 0;
                int numeroLote = da.ObtenerNumeroLote();

                try
                {


                    foreach (DataRow dr in dtTraspaso.Rows)
                    {
                        OperacionMasivo tpf = new OperacionMasivo();
                            
                            tpf.CucOrigen = Convert.ToInt32(dr["CUC_ORIGEN"]);
                            tpf.IdParticipeOrigenTraspaso = tpf.CucOrigen - 44670000;
                            tpf.CucDestino = Convert.ToInt32(dr["CUC_DESTINO"]);
                            tpf.IdParticipeDestinoTraspaso = tpf.CucDestino - 44670000;
                            tpf.CuentaParticipacionOrigenTraspaso = dr["CUENTA_PARTICIPACION_ORIGEN"].ToString().Trim();
                            DataTable dtCuentasParticipacionOrigenTraspaso = da.ListarCuentasParticipacion(tpf.IdParticipeOrigenTraspaso);
                            tpf.IdCuentaParticipacionOrigenTraspaso = Convert.ToInt32(dtCuentasParticipacionOrigenTraspaso.Select("CODIGO = '" + tpf.CuentaParticipacionOrigenTraspaso + "'")[0]["ID"]);
                            tpf.CuentaParticipacionDestinoTraspaso = dr["CUENTA_PARTICIPACION_DESTINO"].ToString().Trim();
                            DataTable dtCuentasParticipacionDestinoTraspaso = da.ListarCuentasParticipacion(tpf.IdParticipeDestinoTraspaso);
                            tpf.IdCuentaParticipacionDestinoTraspaso = Convert.ToInt32(dtCuentasParticipacionDestinoTraspaso.Select("CODIGO = '" + tpf.CuentaParticipacionDestinoTraspaso + "'")[0]["ID"]);
                            tpf.NombreFondoOrigenTraspaso = dr["FONDO_ORIGEN"].ToString().Trim();
                            tpf.IdNombreFondoOrigenTraspaso = Convert.ToInt32(dr["ID_FONDO_ORIGEN"]);
                            DataTable dtFondo = da.ObtenerFondo(tpf.IdNombreFondoOrigenTraspaso);
                            

                            tpf.HoraHasta = Convert.ToDateTime(dtFondo.Rows[0]["HORARIO_OPERACION_HASTA"]);
                            tpf.FechaProceso = fechaSeleccionada;
                            
                            tpf.FechaProceso = tpf.FechaProceso.AddYears(1900 - tpf.FechaProceso.Year);
                            tpf.FechaProceso = tpf.FechaProceso.AddMonths(1 - tpf.FechaProceso.Month);
                            tpf.FechaProceso = tpf.FechaProceso.AddDays(1 - tpf.FechaProceso.Day);

                            if (tpf.FechaProceso > tpf.HoraHasta)
                            {
                                tpf.FechaProceso = fechaSeleccionada.AddDays(1);
                                while (tpf.FechaProceso.DayOfWeek == DayOfWeek.Saturday || tpf.FechaProceso.DayOfWeek == DayOfWeek.Sunday ||
                                    isFeriado(tpf.FechaProceso) == true)
                                {
                                    tpf.FechaProceso.AddDays(1);
                                }
                                tpf.FechaProceso = tpf.FechaProceso.Date.AddHours(8);
                            }
                            else
                            {
                                tpf.FechaProceso = fechaSeleccionada;
                            }

                            
                            tpf.TipoTraspaso = dr["TIPO_TRASPASO"].ToString().Trim();
                            tpf.Cuotas = Convert.ToDecimal(dr["CUOTAS"]);
                            
                           
                          

                            tpf.CodigoPromotorOrigen = da.ObtenerPromotor(tpf.IdParticipeOrigenTraspaso);

                            tpf.ValorCuota = da.ObtenerUltimoValorCuotaFondo(tpf.IdNombreFondoOrigenTraspaso);

                            if (tpf.TipoTraspaso == ConstantesING.TIPO_RESCATE_TOTAL)
                            {
                                tpf.Cuotas = da.ObtenerSaldoCuotasxCuentaParticipacion(tpf.IdNombreFondoOrigenTraspaso, tpf.IdCuentaParticipacionOrigenTraspaso);
                            }
                        


                            string numeroOperacionTraspasoMasivo = da.InsertarTraspasoMasivo(tpf, codigoUsuario);




                        //Ingresando en alerta
                        int numOpeTraspasoMasivo = Convert.ToInt32(numeroOperacionTraspasoMasivo);

                        string dscTipoOpe = ConstantesING.TITULO_TRASPASO;
												//OT10433 INI   
												//da.InsertarAlerta(dscTipoOpe, numOpeTraspasoMasivo, codigoUsuario, numeroLote, string.Empty);	
												da.InsertarAlerta(dscTipoOpe, dscTipoOpe, numOpeTraspasoMasivo, codigoUsuario, numeroLote, string.Empty);
												//OT10433 FIN

                        numeroFila = numeroFila + 1;
                    }
                    da.CommitTransaccion();

                    MessageBox.Show("Se cargaron " + numeroFila + " registros de " + descTipoOperacion + " con el siguiente número de lote " + numeroLote.ToString(), "Mensaje");

                }
                catch (Exception ex)
                {
                    da.RollbackTransaccion();
                    
                    MessageBox.Show("Error en la carga del archivo  en el número de fila " + (numeroFila + 2).ToString() + " " + ex.ToString(), "Atención");


                }
                finally
                {
                    da.LiberarConexion();
                   
                    Thread.CurrentThread.CurrentCulture = originalCulture;
                }

            }
            else
            {
                MessageBox.Show("La plantilla del archivo no le pertenece a este tipo de operación", "Atención", MessageBoxButtons.OKCancel);
            }

        }
        #endregion
        //OT8954 PSC-001 FIN
        
        //OT10217 INI
		public void CargaArchivoConversionCuotasMasivo(string rutaArchivo, DateTime fechaSeleccionada, string codigoUsuario, int numero, string descTipoOperacion)
		{
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-PE");
			CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

			#region Leer archivo Excel
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
			ExcelApplication app = new ExcelApplication();
			ExcelWorkBook wb = app.OpenWorkBook(rutaArchivo, ExcelMode.Full);
			ExcelWorkSheet sheet = wb.GetSheet(1);

			DataTable dtConversionCuotas = sheet.LeerTabla("B", "H", 2, sheet.ObtenerUltimaFilaTabla("B", 2), true);

			wb.Close();
			app.Close();
			#endregion

			int numeroColumnasArchivoSeleccionado = Convert.ToInt32(dtConversionCuotas.Columns.Count.ToString());
			int numeroColumnasOperacion = numero;
			if (numeroColumnasOperacion == numeroColumnasArchivoSeleccionado)
			{
				CargaMasivaDA da = new CargaMasivaDA();

				da.CrearConexion();
				da.AbrirConexion();
				da.AbrirTransaccion();

				int numeroFila = 0;
				int numeroLote = da.ObtenerNumeroLote();
				int numeroOperacionCov = 1;//
				int idOpeReferencia = 0;
                

				try
				{
					foreach (DataRow dr in dtConversionCuotas.Rows)
					{	

							OperacionMasivo cov = new OperacionMasivo();
              cov.CucOrigen = Convert.ToInt32(dr["CUC_ORIGEN"]);
							cov.CucDestino = Convert.ToInt32(dr["CUC_DESTINO"]);          

							//cov.TipoTraspaso = se setea en el DA
							//estado en DA
							cov.IdParticipeOrigen = cov.CucOrigen - 44670000;
							cov.IdParticipeDestino = cov.CucDestino - 44670000;
                            
							DataTable dtCuentasParticipacionOrigen = da.ListarCuentasParticipacion(cov.IdParticipeOrigen);
							DataRow[] dtCuenPart = dtCuentasParticipacionOrigen.Select("FLAG_PREDETERMINADO = 'S'");
							if (dtCuenPart.Length > 0)
							{
								cov.IdCuentaParticipacionOrigenTraspaso = Convert.ToInt32(dtCuenPart[0]["ID"]);
							}

                            DataTable dtCuentasParticipacionDestinoTraspaso = da.ListarCuentasParticipacion(cov.IdParticipeDestino);
							DataRow[] dtCuenPartDestino = dtCuentasParticipacionDestinoTraspaso.Select("FLAG_PREDETERMINADO = 'S'");
							if (dtCuenPartDestino.Length > 0)
							{
								cov.IdCuentaParticipacionDestinoTraspaso = Convert.ToInt32(dtCuenPartDestino[0]["ID"]);
							}

	
							cov.IdFondo = Convert.ToInt32(dr["ID_FONDO_ORIGEN"]);
                            cov.IdOperacionReferencia = 0;

							cov.IdFondoOrigen = Convert.ToInt32(dr["ID_FONDO_ORIGEN"]);
							cov.IdFondoDestino = Convert.ToInt32(dr["ID_FONDO_DESTINO"]);
							cov.Monto = Convert.ToDecimal(dr["MONTO"]);

							
							FondoDA fondoDA = new FondoDA();
							string valorCadenaIGV = fondoDA.obtenerParametroXCodigo(ConstantesING.IGV);
							decimal igv = Convert.ToDecimal(valorCadenaIGV);


                            DataTable dtFondo = da.ObtenerFondo(cov.IdFondoOrigen);
							cov.HoraHasta = Convert.ToDateTime(dtFondo.Rows[0]["HORARIO_OPERACION_HASTA"]);
							cov.FechaProceso = fechaSeleccionada;
                            DateTime FechaSolicitudRescate = fechaSeleccionada;

							cov.FechaProceso = cov.FechaProceso.AddYears(1900 - cov.FechaProceso.Year);
							cov.FechaProceso = cov.FechaProceso.AddMonths(1 - cov.FechaProceso.Month);
							cov.FechaProceso = cov.FechaProceso.AddDays(1 - cov.FechaProceso.Day);

							if (cov.FechaProceso > cov.HoraHasta)
							{
								cov.FechaProceso = fechaSeleccionada.AddDays(1);
								while (cov.FechaProceso.DayOfWeek == DayOfWeek.Saturday || cov.FechaProceso.DayOfWeek == DayOfWeek.Sunday ||
										isFeriado(cov.FechaProceso) == true)
								{
									cov.FechaProceso = cov.FechaProceso.AddDays(1);
								}
								cov.FechaProceso = cov.FechaProceso.Date.AddHours(8);
							}
							else
							{
								cov.FechaProceso = fechaSeleccionada;

                                while (cov.FechaProceso.DayOfWeek == DayOfWeek.Saturday || cov.FechaProceso.DayOfWeek == DayOfWeek.Sunday ||
                                        isFeriado(cov.FechaProceso) == true)
                                {
                                    cov.FechaProceso = cov.FechaProceso.AddDays(1);
                                }
                               
							}

                            int i = 0;
                            string numeroOperacionConversionCuotas = "";
                            idOpeReferencia = 0;
                            while(i<2)
                            {
                                //Insertando operación de conversión de cuotas masivo
                                int idFondoOrigen = cov.IdFondoOrigen;
                                int idFondoDestino = cov.IdFondoDestino;

                                numeroOperacionConversionCuotas = da.InsertarConversionCuotasMasivo(cov, codigoUsuario, igv, FechaSolicitudRescate, idOpeReferencia);
                                idOpeReferencia = Convert.ToInt32(numeroOperacionConversionCuotas);

                                cov.IdFondo = idFondoDestino;
                                 
                                //Insertando alerta
                                string dscTipoOpe = ConstantesING.TITULO_CONVERSION;
																//OT10433 INI 
																//da.InsertarAlerta(dscTipoOpe, idOpeReferencia, codigoUsuario, numeroLote, string.Empty);
																da.InsertarAlerta(dscTipoOpe, dscTipoOpe, idOpeReferencia, codigoUsuario, numeroLote, string.Empty);
																//OT10433 FIN	
																i++;
                            }


                            if (numeroOperacionCov == 1)
                            {
                                numeroOperacionCov++;
                            }
                            else
                            {
                                numeroOperacionCov = 1;
                            }


                            numeroFila = numeroFila + 2;
					}
                    da.CommitTransaccion();

                    MessageBox.Show("Se cargaron " + numeroFila + " registros de " + descTipoOperacion + " con el siguiente número de lote " + numeroLote.ToString(), "Mensaje");
				}
                catch(Exception e)
                {
                    //throw e;
                    da.RollbackTransaccion();

                    MessageBox.Show("Error en la carga del archivo  en el número de fila " + (numeroFila + 2).ToString() + " " + e.ToString(), "Atención");
				}
                finally
                {
                    da.LiberarConexion();

                    Thread.CurrentThread.CurrentCulture = originalCulture;
                }

			}
			else
			{
				MessageBox.Show("La plantilla del archivo no le pertenece a este tipo de operación", "Atención", MessageBoxButtons.OKCancel);
			}
		}
        //OT10217 FIN


				//OT10433 INI
        public void CargaArchivoCuentasParticipacionMasivo(string rutaArchivo, DateTime fechaSeleccionada, string codigoUsuario, int numero, string descTipoOperacion)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-PE");
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            #region Leer archivo Excel
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            ExcelApplication app = new ExcelApplication();
            ExcelWorkBook wb = app.OpenWorkBook(rutaArchivo, ExcelMode.Full);
            ExcelWorkSheet sheet = wb.GetSheet(1);

            DataTable dtCuentasParticipacion = sheet.LeerTabla("B", "E", 2, sheet.ObtenerUltimaFilaTabla("B", 2), true);

            wb.Close();
            app.Close();
            #endregion

            int numeroColumnasArchivoSeleccionado = Convert.ToInt32(dtCuentasParticipacion.Columns.Count.ToString());
            int numeroColumnasOperacion = numero;
            if (numeroColumnasOperacion == numeroColumnasArchivoSeleccionado)
            {
                CargaMasivaDA da = new CargaMasivaDA();

                da.CrearConexion();
                da.AbrirConexion();
                da.AbrirTransaccion();

                int numeroFila = 0;
                int numeroLote = da.ObtenerNumeroLote();

                try
                {
                    foreach (DataRow dr in dtCuentasParticipacion.Rows)
                    {
                        CuentaParticipacionMasivo cpm = new CuentaParticipacionMasivo();
                        
                        cpm.Cuc = Convert.ToInt32(dr["CUC"]);
                        cpm.Etiqueta = dr["ETIQUETA"].ToString().Trim();
												cpm.FlagBloqueada = dr["FLAG_CUENTA_BLOQUEADA"].ToString().Trim();
                        cpm.IdParticipe = cpm.Cuc - 44670000;
                        cpm.CodigoPlan = dr["CODIGO_PLAN"].ToString().Trim();
												
                        string correlativoCodigoIdParticipe = "";
                        string CorrelativoIdParticipe = da.CorrelativoCuentaParticipacion(Convert.ToString(cpm.IdParticipe));

                        if (CorrelativoIdParticipe == "")
                        {
                            CorrelativoIdParticipe = "1";
                        }

                        if (Convert.ToInt32(CorrelativoIdParticipe) < 10)
                        {
                            correlativoCodigoIdParticipe = Convert.ToString(cpm.Cuc) + '-' + '0' + CorrelativoIdParticipe;
                        }
                        else
                        {
                            correlativoCodigoIdParticipe = Convert.ToString(cpm.Cuc) + '-' + CorrelativoIdParticipe;
                        }
                        
                        //Insertando cuentas de participación masivo
                        int idcuentaParticipacion = 0;
												idcuentaParticipacion = da.InsertarCuentasParticipacionMasivo(cpm, correlativoCodigoIdParticipe, codigoUsuario);

												string titulo = ConstantesING.TITULO_CUENTA_PARTICIPACION;
												string descripcion = Convert.ToString(idcuentaParticipacion);
                        //Insertando alerta

												int idOperacion = 0;
												da.InsertarAlerta(titulo, descripcion, idOperacion, codigoUsuario, numeroLote, string.Empty);
                        numeroFila = numeroFila + 1;
                    }
                    da.CommitTransaccion();

                    MessageBox.Show("Se cargaron " + numeroFila + " registros de " + descTipoOperacion + " con el siguiente número de lote " + numeroLote.ToString(), "Mensaje");
                }
                catch (Exception e)
                {
                    //throw e;
                    da.RollbackTransaccion();
                    MessageBox.Show("Error en la carga del archivo  en el número de fila " + (numeroFila + 2).ToString() + " " + e.ToString(), "Atención");
                }
                finally
                {
                    da.LiberarConexion();
                    Thread.CurrentThread.CurrentCulture = originalCulture;
                }
            }
            else
            {
                MessageBox.Show("La plantilla del archivo no le pertenece a este tipo de operación", "Atención", MessageBoxButtons.OKCancel);
            }
        }
			//OT10433 FIN
    }

}
