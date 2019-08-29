using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using System.Data;
using Microsoft.Office;
using System.Runtime.InteropServices;
using Sit.BusinessEntities;
using Sit.BusinessLayer;

/// <summary>
/// CRumiche: Formulario para la importación de Data desde Excel
/// </summary>
public partial class frmDataImporter : BasePage // System.Web.UI.Page //
{
     
    [WebMethod]
    public static int Get_CargarExcelMaestro(string p)
    {
        //string fileName = System.Web.HttpContext.Current.Server.MapPath("PlantillaImportaDatos.v3.xlsx");
        //ProcesarArchivo(fileName);
        return 0;
    }

    public static string ProcesarArchivo(string pathArchivo)
    {
        string msgResul = "Migración realizada correctamente.";
        ImportadorExcelBE infoImportar = new ImportadorExcelBE();
        UIUtility.COMObjectAplication ObjCom = null;
        Excel.Application xlApp = null;

        try
        {
            ObjCom = new UIUtility.COMObjectAplication("Excel.Application", "EXCEL");
            xlApp = (Excel.Application)ObjCom.ObjetoAplication;

            Excel.Workbooks oBooks = xlApp.Workbooks;
            oBooks.Open(pathArchivo, ReadOnly: true, IgnoreReadOnlyRecommended: true);

            Excel.Workbook xlLibro = oBooks.Item[1];
            Excel.Sheets oSheets = xlLibro.Worksheets;

            Excel.Worksheet hojaConfig = (Excel.Worksheet)oSheets.Item[1];

            DataTable dtConfig = ObtenerConfigPrincipal(infoImportar, hojaConfig);

            int indiceHojaParaMigrar;
            Excel.Worksheet hojaParaMigrar;

            foreach (DataRow infoTabla in dtConfig.Rows)
            {
                indiceHojaParaMigrar = ObtenerIndiceHojaExcel(oSheets, infoTabla["Pestania"].ToString());
                if (indiceHojaParaMigrar > 0)
                {
                    hojaParaMigrar = (Excel.Worksheet)oSheets.Item[indiceHojaParaMigrar];

                    DataTable dtDefTablas = null;
                    dtDefTablas = ObtenerConfigColumnasSegunTabla(infoImportar, hojaParaMigrar, infoTabla["NombreTablaBD"].ToString());

                    DataTable dtDatos = null;
                    dtDatos = ObtenerRegistrosTablaSegunPestania(infoImportar, hojaParaMigrar, infoTabla["NombreTablaBD"].ToString());
                }
                else
                {
                    throw new ImportadorExcelExceptionBE("No se ha encontrado la Hoja Excel: " + infoTabla["Pestania"].ToString());
                }
            }

            ImportadorExcelBM objImportadorBM = new ImportadorExcelBM();
            objImportadorBM.GuardarImportacion(infoImportar);
        }
        catch (Exception ex)
        {
            if (xlApp != null)
            {
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            msgResul = ex.Message;
        }
        finally
        {
            if (ObjCom != null)
                ObjCom.terminarProceso();
        }

        return msgResul; // Si es "" (VACIO) entonces es PROCESO OK
    }

    public static DataTable ObtenerConfigPrincipal(ImportadorExcelBE motor, Excel.Worksheet hojaConfig)
    {
        motor.ConfigExcel.TabTabla_FilaTipoDato = Convert.ToInt32(ifNull(((Excel.Range)hojaConfig.Cells[2, 3]).Value, 1));
        motor.ConfigExcel.TabTabla_FilaNombreColumnaBD = Convert.ToInt32(ifNull(((Excel.Range)hojaConfig.Cells[3, 3]).Value, 2));
        motor.ConfigExcel.TabTabla_FilaInicioData = Convert.ToInt32(ifNull(((Excel.Range)hojaConfig.Cells[4, 3]).Value, 5));
        motor.ConfigExcel.TabTabla_MaxColumnasPorTabla = Convert.ToInt32(ifNull(((Excel.Range)hojaConfig.Cells[5, 3]).Value, 100));

        DataRow fila;
        string importar;

        for (int i = 0; i < motor.ConfigExcel.DefTablas_MaxFilas; i++)
        {
            importar = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.DefTablas_FilaInicio + i, motor.ConfigExcel.DefTablas_ColImportar]).Value, "").ToString();

            if (importar.Equals("SI"))
            {
                fila = motor.DtListaTablas.NewRow();
                fila["NombreTablaBD"] = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.DefTablas_FilaInicio + i, motor.ConfigExcel.DefTablas_ColNombreTabla]).Value, "").ToString().Trim();
                fila["Pestania"] = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.DefTablas_FilaInicio + i, motor.ConfigExcel.DefTablas_ColPestania]).Value, "").ToString().Trim();
                fila["ColumnasUnique"] = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.DefTablas_FilaInicio + i, motor.ConfigExcel.DefTablas_ColColumnasUnique]).Value, "").ToString().Trim();
                fila["RollbackPorDuplicidad"] = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.DefTablas_FilaInicio + i, motor.ConfigExcel.DefTablas_ColRollbackDuplicidad]).Value, "").ToString().Trim();

                fila["PK_Autogenerado"] = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.DefTablas_FilaInicio + i, motor.ConfigExcel.DefTablas_ColPK_Autogenerado]).Value, "").ToString().Trim();
                fila["LlavesForaneas"] = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.DefTablas_FilaInicio + i, motor.ConfigExcel.DefTablas_ColLlavesForaneas]).Value, "").ToString().Trim();
                fila["PK_NombreColumna"] = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.DefTablas_FilaInicio + i, motor.ConfigExcel.DefTablas_ColPK_NombreColumna]).Value, "").ToString().Trim();
                fila["NombreTablaForanea"] = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.DefTablas_FilaInicio + i, motor.ConfigExcel.DefTablas_ColNombreTablaForanea]).Value, "").ToString().Trim();
                fila["NombreClaveForanea"] = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.DefTablas_FilaInicio + i, motor.ConfigExcel.DefTablas_ColNombreClaveForanea]).Value, "").ToString().Trim();
                fila["CargaPorScripts"] = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.DefTablas_FilaInicio + i, motor.ConfigExcel.DefTablas_ColCargaPorScripts]).Value, "").ToString().Trim();

                motor.DtListaTablas.Rows.Add(fila);
            }
        }

        return motor.DtListaTablas;    
    }

    public static DataTable ObtenerConfigColumnasSegunTabla(ImportadorExcelBE motor, Excel.Worksheet hojaConfig, string nombreTablaBD)
    {
        DataRow fila;
        string nombreColumnaBD, tipoDato;
        bool hayMasColumnas = true;
        int colActualEnExcel = 1;

        while (hayMasColumnas)
        {
            nombreColumnaBD = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.TabTabla_FilaNombreColumnaBD, colActualEnExcel]).Value, "").ToString().Trim();
            tipoDato = ifNull(((Excel.Range)hojaConfig.Cells[motor.ConfigExcel.TabTabla_FilaTipoDato, colActualEnExcel]).Value, "").ToString().Trim();

            // CRumiche: Solo tomamos en cuenta la columna si tiene "NOMBRE DE COLUMNA" y "TIPO DE DATO"
            if (nombreColumnaBD.Length > 0 && tipoDato.Length > 0) 
            {
                fila = motor.DtEstructuraTablas.NewRow();
                fila["NombreTablaBD"] = nombreTablaBD;
                fila["NombreColumnaBD"] = nombreColumnaBD;
                fila["TipoDato"] = tipoDato;
                fila["IndiceColEnExcel"] = colActualEnExcel;

                motor.DtEstructuraTablas.Rows.Add(fila);
            }

            colActualEnExcel++;

            // Si el "NOMBRE DE COLUMNA" está VACIO o se ha alcanzado el máximo de columnas SALIMOS, ya no hacemos lectura de más columnas
            if (colActualEnExcel > motor.ConfigExcel.TabTabla_MaxColumnasPorTabla)
                break;
        }

        return motor.DtEstructuraTablas;
    }

    public static DataTable ObtenerRegistrosTablaSegunPestania(ImportadorExcelBE motor, Excel.Worksheet hojaConfig, string nombreTablaBD)
    {
        // Obtenemos las columnas de la TABLA a importar
        DataRow[] columnasDT = motor.DtEstructuraTablas.Select(" NombreTablaBD = '" + nombreTablaBD + "' ");

        // Creamos un DataTable con las estructura de columnas de la TABLA objetivo
        DataTable dtDatos = new DataTable(nombreTablaBD);
        foreach (DataRow col in columnasDT)
        {
            dtDatos.Columns.Add(col["NombreColumnaBD"].ToString());            
        }

        const int _FilaInicioDatos = 5; /* Según el formato diseñado */

        // Ahora hacemos la lectura de todas las filas con datos de la pestaña del EXCEL
        List<string> valores = new List<string>();
        string valorCelda;

        int indiceFilaExcel = 0;
        bool leerSiguienteFila = true;

        while (leerSiguienteFila)
        {
            valores.Clear(); // Limpiamos la lista de valores para la nueva fila

            foreach (DataRow col in columnasDT) // Obtenemos los valores para la nueva fila
            {
                valorCelda = ifNull(((Excel.Range)hojaConfig.Cells[_FilaInicioDatos + indiceFilaExcel, Convert.ToInt32(col["IndiceColEnExcel"])]).Value, "").ToString().Trim();

                if (valores.Count == 0 && valorCelda.Length == 0)
                    break;  // Si se está evaluando la primera CELDA y su VALOR es VACIO Finalizamos el FOR (NO LEEMOS TODA LA FILA)
                else
                {
                    if (col["TipoDato"].ToString().Equals("Fecha1")) { // Trato especial para las FECHA
                        valorCelda = UIUtility.ConvertirStringaFecha(valorCelda.Substring(0, 10)).ToString("yyyyMMdd");
                    } else if (col["TipoDato"].ToString().Equals("Lista")) { // Obtener el código de la lista seleccionada de una celda
                        valorCelda = ObtenerCodigo_TipoDatoLista(valorCelda);
                    }
                    valores.Add(valorCelda); // Agregamos el valor                
                }

            }

            if (valores.Count > 0)
            {
                dtDatos.Rows.Add(valores.ToArray());
                indiceFilaExcel++;
            }
            else
                leerSiguienteFila = false;
        }

        motor.TablasCargadas.Add(dtDatos);
        return dtDatos;        
    }
       
    public static int ObtenerIndiceHojaExcel(Excel.Sheets hojasExcel, string nombreBusqueda)
    {
        foreach (Excel.Worksheet hoja in hojasExcel)
        {
            if (hoja.Name.Equals(nombreBusqueda))
            {
                return hoja.Index;
            }
        }
        return -1;
    }

    public static object ifNull(object o, object defaultValue)
    {
        if (o == null) return defaultValue;
        return o;
    }

    protected void brnProcesar_Click(object sender, EventArgs e)
    {
        string pathArchivo = "";
        pop1_areaErrores.Value = "";
        try
        {
            if (fupArchivo.FileBytes.Length > 0)
            {
                ParametrosGeneralesBM objParamGenerales = new ParametrosGeneralesBM();
                DataTable dt = objParamGenerales.SeleccionarPorFiltro(Constantes.ParametrosGenerales.RUTA_TEMP, "", "", "", DatosRequest);
                //string nombreArchivo = System.Guid.NewGuid().ToString().Substring(0, 8) + "_" + fupArchivo.FileName;
                string nombreArchivo = dt.Rows[0]["Valor"].ToString() + "_" + fupArchivo.FileName;
                //pathArchivo = System.Web.HttpContext.Current.Server.MapPath("temp_files\\" + nombreArchivo);
                pathArchivo = nombreArchivo;
                fupArchivo.SaveAs(pathArchivo);
                string resul = ProcesarArchivo(pathArchivo);
                // resul // Mostramos los resultado en un panel de texto
                pop1_areaErrores.Value = resul;
            }
            else
            {
                AlertaJS("Seleccionar archivo.");
            }
        }
        catch (Exception ex)
        {
            AlertaJS("Seleccionar archivo.");
        }
        finally
        {
            try
            {
                if (pathArchivo.Length > 0) System.IO.File.Delete(pathArchivo);
            }
            catch { }
        }
    }

    private static string ObtenerCodigo_TipoDatoLista(string valorCelda) {
        int posGuion = 0;
        string codigoLista = string.Empty;
        if (valorCelda.Length > 0) {
            posGuion = valorCelda.IndexOf("-");
            if (posGuion > 0)
            {
                codigoLista = valorCelda.Substring(0, posGuion);
            }
            else {
                return valorCelda;
            }
        }    
        return codigoLista;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) {
            CargarLoading("brnProcesar");
        }
    }
}