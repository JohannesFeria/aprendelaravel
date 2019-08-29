/*
 * Fecha de Modificación: 25/08/2016
 * Modificado por: Robert Castillo
 * Numero de OT: 8895
 * Descripción del cambio: Creación de clase que se encarga de la lógica de negocio de las retenciones
 *                         asociadas a pagos de flujos.
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
    public class CargarRetencionPagoFlujosBO : INGFondos.Data.DA
    {
        private string sourcePath;
        private string codigoUsuario;

        public CargarRetencionPagoFlujosBO(string sourcePath, string codigoUsuario)
            : base(INGFondos.Constants.Conexiones.ServidorTributacion, INGFondos.Constants.Conexiones.BaseDeDatosTributacion)
        {
            this.sourcePath = sourcePath;
            this.codigoUsuario = codigoUsuario;
        }

        public void Procesar()
        {
            ExcelApplication excelApplication = new ExcelApplication();
            CargarRetencionPagoFlujosDA da = new CargarRetencionPagoFlujosDA();
            SqlConnection cn = GetConnection();
            cn.Open();

            SqlTransaction trans = cn.BeginTransaction(IsolationLevel.ReadUncommitted);
            DateTime fechaActual = DateTime.Now;


            try
            {
                // Abriendo el archivo que contiene la información
                excelApplication.OpenWorkBook(sourcePath, ExcelMode.Full);
                ExcelWorkSheet sheet = excelApplication.GetWorkBook(1).GetSheet(1);

                // Obteniendo la información del archivo
                int rowIndex = 2;
                //object[] row = sheet.GetRow(rowIndex++, 'A', 'C', 4);
                object[] row = sheet.GetRow(rowIndex++, 'A', 'C');

                while (row[0] != null)
                {
                    try
                    {
                        int idOperacion = Convert.ToInt32(sheet.GetString(string.Format("A{0}", rowIndex - 1)));
                        int idParticipe = Convert.ToInt32(sheet.GetString(string.Format("B{0}", rowIndex - 1)));
                        //decimal montoRetencion = Convert.ToDecimal(sheet.GetString(string.Format("C{0}", rowIndex - 1)));
                        String montoRetencionCadena = Convert.ToDecimal(sheet.GetString(string.Format("C{0}", rowIndex - 1))).ToString("0.00000000");
                        decimal montoRetencion = Convert.ToDecimal(montoRetencionCadena);

                        PagoFlujosTD.PagoFlujosRow drPagoFlujos = new PagoFlujosTD().PagoFlujos.NewPagoFlujosRow();
                        drPagoFlujos.ID_OPERACION = idOperacion;
                        drPagoFlujos.ID_PARTICIPE = idParticipe;
                        drPagoFlujos.RETENCION = montoRetencion;
                        drPagoFlujos.FECHA = fechaActual;
                        drPagoFlujos.USUARIO_CREACION = codigoUsuario;

                        da.registrarPagoFlujos(drPagoFlujos,cn,trans);
                        row = sheet.GetRow(rowIndex++, 'A', 'C');
                    
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error en la línea {0}. Detalle " + ex.Message, rowIndex - 1));
                    }
                    //excelApplication.GetWorkBook(1).Save();
                    //excelApplication.Show();
                }
                trans.Commit();

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
    }
}
