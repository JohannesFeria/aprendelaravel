/*
 * Fecha de Modificación: 18/09/2014
 * Modificado por: Leslie Valerio
 * Numero de OT: 6891
 * Descripción del cambio: Creación
 *                          Se crea para generar la conciliacion de Seguros con Fondos Sura
 * */
using System;
using System.Data;
using System.IO;
using System.Text;
using Procesos.DA;
using INGFondos.Support.Interop;
using System.Data.SqlClient;
using System.Collections;
using Procesos.TD;

using INGFondos.Data;

namespace Procesos.BL
{
    public class CargaSegurosSuraBL : INGFondos.Data.DA
    {
        private string sourcePath;
        private string codigoUsuario;

        public CargaSegurosSuraBL(string sourcePath, string codigoUsuario)
            : base(INGFondos.Constants.Conexiones.ServidorOperaciones, INGFondos.Constants.Conexiones.BaseDeDatosOperaciones)
        {
            this.sourcePath = sourcePath;
            this.codigoUsuario = codigoUsuario;

        }

        public void truncarTablaSeguros()
        {
            CargaSegurosSuraDA da = new CargaSegurosSuraDA();
            SqlConnection cn = GetConnection();
            cn.Open();

            SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {

                CargaSegurosSuraDA cargaSegurosSuraDA = new CargaSegurosSuraDA();
                cargaSegurosSuraDA.TruncarTabla_Convenio(cn, trans);
                trans.Commit();
            }

            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }

            finally
            {
                trans.Dispose();
                cn.Close();
                //excelApplication.Close();
            }
        }

        public void insertarRegistroConci_vidaAhorro_Convenio()
        {
            CargaSegurosSuraDA da = new CargaSegurosSuraDA();
            SqlConnection cn = GetConnection();
            cn.Open();

            SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {

                CargaSegurosSuraDA cargaSegurosSuraDA = new CargaSegurosSuraDA();
                cargaSegurosSuraDA.insertarRegistroConci_vidaAhorro_Convenio(cn, trans);
                trans.Commit();
            }

            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }

            finally
            {
                trans.Dispose();
                cn.Close();
                // excelApplication.Close();
            }
        }


        public DataTable obtenerVidaAhorro()
        {
            CargaSegurosSuraDA da = new CargaSegurosSuraDA();
            da.ObtenerDataDiferenciaVidaAhorro();


            return da.ObtenerDataDiferenciaVidaAhorro();
        }

        public DataSet PaginaVida(int Scroll)
        {
            CargaSegurosSuraDA da = new CargaSegurosSuraDA();
            return da.ObtenerPaginado(Scroll);
        }
        public void Procesar()
        {
            StringBuilder sbLinea;
            sbLinea = new StringBuilder("");


            CargaSegurosSuraDA da = new CargaSegurosSuraDA();
            SqlConnection cn = GetConnection();
            cn.Open();

            SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);

            string path = sourcePath;

            System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            ExcelApplication excelApplication = new ExcelApplication();
            ExcelWorkBook wb = excelApplication.OpenWorkBook(path, ExcelMode.Full);

            try
            {


                // traer la cantidad de empresas atraves del campo codigo
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

                CargaSegurosSuraDA cargaSegurosSuraDA = new CargaSegurosSuraDA();

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
                    int grabo_registro = 0;
                    int numeroHojas = 0;
                    string parametro = "";
                    string constanteCP = "CP_";
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
                    //{
                    for (int i = 1; i <= numeroHojas; i++)
                    {
                        ExcelWorkSheet sheet = wb.GetSheet(i);
                        // ExcelWorkSheet sheet = null;


                        nombreEmpresa = sheet.getName();

                        parametro = nombreEmpresa.Substring(0, 3);
                        StreamWriter sw = null;

                        //if (i == 1)
                        //{
                        //    periodo = sheet.GetString("B8").Substring(4, 10);
                        //    periodoElementos = periodo.Split('/');

                        //    anio = periodoElementos[2].Trim();
                        //    mes = periodoElementos[1].Trim();
                        //}

                        //codigoEmpresa = sheet.GetString("B1").Trim();
                        //DataTable dtDetalleConvenio = conveniosPlanEmpleadorDA.ObtenerDetalleConvenios(codigoEmpresa, Convert.ToInt32(mes), Convert.ToInt32(anio));

                        rowIndex = 13;//13
                        row = sheet.GetRow(rowIndex++, 'B', 'O');
                        int cont = 0;

                        while (row[0] != null & parametro != constanteCP)
                        {


                            try
                            {

                                ConvenioTD.ConvenioRow drConvenio = new ConvenioTD().Convenio.NewConvenioRow();
                                drConvenio.CUC = row[0].ToString();
                                drConvenio.MATRICULA = row[1] == null ? "" : row[1].ToString();
                                drConvenio.NOMBRE = row[2] == null ? "" : row[2].ToString();
                                drConvenio.NOMBRES = row[3].ToString();
                                drConvenio.APELLIDOS = row[4].ToString();
                                drConvenio.MONTO = Convert.ToDouble(row[5]);
                                drConvenio.FONDO = row[6].ToString();
                                drConvenio.ESTADO = row[7].ToString();
                                drConvenio.PARTICIPANTE = Convert.ToDouble(row[8]);
                                drConvenio.EMPLEADOR = Convert.ToDouble(row[9]);
                                drConvenio.ANTIGUEDAD = Convert.ToDouble(row[10]);
                                drConvenio.SALARIO_PROM = Convert.ToDouble(row[11]);
                                drConvenio.EDAD = Convert.ToDouble(row[12]);
                                drConvenio.BONO_ANUAL = Convert.ToDouble(row[13]);

                                cargaSegurosSuraDA.CargaRegistroConvenio(drConvenio, cn, trans);
                                row = sheet.GetRow(rowIndex++, 'B', 'O');
                                cont = cont + 1;
                                grabo_registro = grabo_registro + 1;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(string.Format("Error en la línea {0}. Detalle " + ex.Message, rowIndex - 1));
                            }
                            // trans.Commit();


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
                    if (grabo_registro > 0)
                    {
                        trans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                wb.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
                excelApplication.Close();
            }


        }

    }
}




