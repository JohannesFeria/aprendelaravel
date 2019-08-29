Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports Sit.BusinessEntities
Public Class ValorCuotaDAM
    Public Function CalcularValoresCuota(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, Optional ByVal CodigoSerie As String = "") As DataTable
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbcomand As DbCommand = db.GetStoredProcCommand("SP_VALORCUOTA_TRAN_CalcularValoresCuotas")
            db.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbcomand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbcomand, "@CodigoSerie", DbType.String, CodigoSerie)
            Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                CalcularValoresCuota = ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function CXCVentaCompra(ByVal Venta As String, ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbcomand As DbCommand = db.GetStoredProcCommand("sp_SIT_CXCVentaCompra")
            db.AddInParameter(dbcomand, "@p_Venta", DbType.String, Venta)
            db.AddInParameter(dbcomand, "@P_CodigoPortafolio", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbcomand, "@P_FechaOperacion", DbType.Decimal, FechaOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ReporteValorCuota(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ByVal FechaCadena As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbcomand As DbCommand = db.GetStoredProcCommand("sp_SIT_ReporteValorCuota")
            db.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbcomand, "@p_FechaProceso", DbType.Decimal, FechaOperacion)
            db.AddInParameter(dbcomand, "@p_FechaCadena", DbType.String, FechaCadena)
            Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function SeleccionarValorCuota(ByVal CodigoPortafolioSBS As String, ByVal CodigoSerie As String, ByVal FechaProceso As Decimal) As ValorCuotaBE
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Using oValorCuotaBE As New ValorCuotaBE
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Using dbcomand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_ValorCuota")
                db.AddInParameter(dbcomand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
                db.AddInParameter(dbcomand, "@CodigoSerie", DbType.String, CodigoSerie)
                db.AddInParameter(dbcomand, "@FechaProceso", DbType.Decimal, FechaProceso)
                db.LoadDataSet(dbcomand, oValorCuotaBE, "ValorCuota")
                db.LoadDataSet(dbcomand, oValorCuotaBE, "ValorCuotaSerie")
                Return oValorCuotaBE
            End Using
        End Using
    End Function
    Public Function ObtenerValorCuotaCierreAnterior(ByVal CodigoPortafolioSBS As String, ByVal CodigoSerie As String, ByVal FechaProceso As Decimal) As Decimal
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Using oValorCuotaBE As New ValorCuotaBE
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Using dbcomand As DbCommand = db.GetStoredProcCommand("ValorCuota_ObtenerValCuotaAnterior")
                db.AddInParameter(dbcomand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
                db.AddInParameter(dbcomand, "@CodigoSerie", DbType.String, CodigoSerie)
                db.AddInParameter(dbcomand, "@FechaProceso", DbType.Decimal, FechaProceso)
                Return CType(db.ExecuteScalar(dbcomand), Decimal)
            End Using
        End Using
    End Function
    Public Function Insertar_ValorCuota(ByVal oValorCuotaBE As ValorCuotaBE, dataRequest As DataSet) As Boolean
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código. Agregar el parámetro @p_CXCVentaTituloDividendos
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_ValorCuota")
            Dim oRow As ValorCuotaBE.ValorCuotaRow
            Dim oRowS As ValorCuotaBE.ValorCuotaSerieRow
            oRow = DirectCast(oValorCuotaBE.ValorCuota.Rows(0), ValorCuotaBE.ValorCuotaRow)
            oRowS = DirectCast(oValorCuotaBE.ValorCuotaSerie.Rows(0), ValorCuotaBE.ValorCuotaSerieRow)
            '--1
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, oRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@CodigoSerie", DbType.String, oRow.CodigoSerie)
            db.AddInParameter(dbCommand, "@FechaProceso", DbType.Decimal, oRow.FechaProceso)
            db.AddInParameter(dbCommand, "@InversionesT1", DbType.Decimal, oRow.InversionesT1)
            db.AddInParameter(dbCommand, "@VentasVencimientos", DbType.Decimal, oRow.VentasVencimientos)
            db.AddInParameter(dbCommand, "@Rentabilidad", DbType.Decimal, oRow.Rentabilidad)
            db.AddInParameter(dbCommand, "@ValForwards", DbType.Decimal, oRow.ValForwards)
            db.AddInParameter(dbCommand, "@p_ValSwaps", DbType.Decimal, IIf(IsDBNull(oRow.ValSwaps), 0, oRow.ValSwaps))
            db.AddInParameter(dbCommand, "@InversionesSubTotal", DbType.Decimal, oRow.InversionesSubTotal)
            '--2
            db.AddInParameter(dbCommand, "@CajaPreCierre", DbType.Decimal, oRowS.CajaPreCierre)
            db.AddInParameter(dbCommand, "@CXCVentaTitulo", DbType.Decimal, oRowS.CXCVentaTitulo)
            db.AddInParameter(dbCommand, "@OtrasCXC", DbType.Decimal, oRowS.OtrasCXC)
            db.AddInParameter(dbCommand, "@OtrasCXCExclusivos", DbType.Decimal, oRowS.OtrasCXCExclusivos)
            db.AddInParameter(dbCommand, "@CXCPreCierre", DbType.Decimal, oRowS.CXCPreCierre)
            db.AddInParameter(dbCommand, "@CXPCompraTitulo", DbType.Decimal, oRowS.CXPCompraTitulo)
            db.AddInParameter(dbCommand, "@OtrasCXP", DbType.Decimal, oRowS.OtrasCXP)
            db.AddInParameter(dbCommand, "@OtrasCXPExclusivos", DbType.Decimal, oRowS.OtrasCXPExclusivos)
            db.AddInParameter(dbCommand, "@CXPPreCierre", DbType.Decimal, oRowS.CXPPreCierre)
            db.AddInParameter(dbCommand, "@OtrosGastos", DbType.Decimal, oRowS.OtrosGastos)
            db.AddInParameter(dbCommand, "@OtrosGastosExclusivos", DbType.Decimal, oRowS.OtrosGastosExclusivos)
            db.AddInParameter(dbCommand, "@OtrosIngresos", DbType.Decimal, oRowS.OtrosIngresos)
            db.AddInParameter(dbCommand, "@OtrosIngresosExclusivos", DbType.Decimal, oRowS.OtrosIngresosExclusivos)
            db.AddInParameter(dbCommand, "@ValPatriPreCierre1", DbType.Decimal, oRowS.ValPatriPreCierre1)
            db.AddInParameter(dbCommand, "@ComisionSAFM", DbType.Decimal, oRowS.ComisionSAFM)
            db.AddInParameter(dbCommand, "@ValPatriPreCierre2", DbType.Decimal, oRowS.ValPatriPreCierre2)
            db.AddInParameter(dbCommand, "@ValCuotaPreCierre", DbType.Decimal, oRowS.ValCuotaPreCierre)
            db.AddInParameter(dbCommand, "@ValCuotaPreCierreVal", DbType.Decimal, oRowS.ValCuotaPreCierreVal)
            db.AddInParameter(dbCommand, "@AportesCuotas", DbType.Decimal, oRowS.AportesCuotas)
            db.AddInParameter(dbCommand, "@AportesValores", DbType.Decimal, oRowS.AportesValores)
            db.AddInParameter(dbCommand, "@RescateCuotas", DbType.Decimal, oRowS.RescateCuotas)
            db.AddInParameter(dbCommand, "@RescateValores", DbType.Decimal, oRowS.RescateValores)
            db.AddInParameter(dbCommand, "@Caja", DbType.Decimal, oRowS.Caja)
            db.AddInParameter(dbCommand, "@CXCVentaTituloCierre", DbType.Decimal, oRowS.CXCVentaTituloCierre)
            db.AddInParameter(dbCommand, "@OtrosCXCCierre", DbType.Decimal, oRowS.OtrosCXCCierre)
            db.AddInParameter(dbCommand, "@OtrosCXCExclusivoCierre", DbType.Decimal, oRowS.OtrosCXCExclusivoCierre)
            db.AddInParameter(dbCommand, "@CXCCierre", DbType.Decimal, oRowS.CXCCierre)
            db.AddInParameter(dbCommand, "@CXPCompraTituloCierre", DbType.Decimal, oRowS.CXPCompraTituloCierre)
            db.AddInParameter(dbCommand, "@OtrasCXPCierre", DbType.Decimal, oRowS.OtrasCXPCierre)
            db.AddInParameter(dbCommand, "@OtrasCXPExclusivoCierre", DbType.Decimal, oRowS.OtrasCXPExclusivoCierre)
            db.AddInParameter(dbCommand, "@CXPCierre", DbType.Decimal, oRowS.CXPCierre)
            db.AddInParameter(dbCommand, "@OtrosGastosCierre", DbType.Decimal, oRowS.OtrosGastosCierre)
            db.AddInParameter(dbCommand, "@OtrosGastosExclusivosCierre", DbType.Decimal, oRowS.OtrosGastosExclusivosCierre)
            db.AddInParameter(dbCommand, "@OtrosIngresosCierre", DbType.Decimal, oRowS.OtrosIngresosCierre)
            db.AddInParameter(dbCommand, "@OtrosIngresosExclusivosCierre", DbType.Decimal, oRowS.OtrosIngresosExclusivosCierre)
            db.AddInParameter(dbCommand, "@ValPatriCierreCuota", DbType.Decimal, oRowS.ValPatriCierreCuota)
            db.AddInParameter(dbCommand, "@ValPatriCierreValores", DbType.Decimal, oRowS.ValPatriCierreValores)
            db.AddInParameter(dbCommand, "@ValCuotaCierre", DbType.Decimal, oRowS.ValCuotaCierre)
            db.AddInParameter(dbCommand, "@ValCuotaValoresCierre", DbType.Decimal, oRowS.ValCuotaValoresCierre)
            db.AddInParameter(dbCommand, "@InversionesSubTotalSerie", DbType.Decimal, oRowS.InversionesSubTotalSerie)
            db.AddInParameter(dbCommand, "@ValCuotaPreCierreAnt", DbType.Decimal, oRowS.ValCuotaPreCierreAnt)
            'OT 9851 27/01/2017 - Carlos Espejo
            'Descripcion: Campos nuevos para el calculo de Otras CXP y auditoria
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_ChequePendiente", DbType.Decimal, oRowS.ChequePendiente)
            db.AddInParameter(dbCommand, "@p_RescatePendiente", DbType.Decimal, oRowS.RescatePendiente)
            db.AddInParameter(dbCommand, "@p_ComisionSAFMAnterior", DbType.Decimal, oRowS.ComisionSAFMAnterior)
            'OT 9981 17/02/2017 - Carlos Espejo
            'Descripcion: Campos nuevos Ajustes CXP
            db.AddInParameter(dbCommand, "@p_AjustesCXP", DbType.Decimal, oRowS.AjustesCXP)
            'OT 9981 Fin
            'OT 9851 Fin
            db.AddInParameter(dbCommand, "@p_CXCVentaTituloDividendos", DbType.Decimal, oRowS.CXCVentaTituloDividendos)
            db.AddInParameter(dbCommand, "@p_AportesLiberadas", DbType.Decimal, oRowS.AportesLiberadas)
            db.AddInParameter(dbCommand, "@p_RetencionPendiente", DbType.Decimal, oRowS.RetencionPendiente)
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se carga nuevo campo AjustesCXC | 16/07/18 
            db.AddInParameter(dbCommand, "@p_AjustesCXC", DbType.Decimal, oRowS.AjustesCXC)
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF017 - Se carga nuevo campo AjustesCXC | 16/07/18 
            db.AddInParameter(dbCommand, "@p_DevolucionComisionUnificada", DbType.Decimal, IIf(IsDBNull(oRowS.DevolucionComisionUnificada), 0, oRowS.DevolucionComisionUnificada))
            db.AddInParameter(dbCommand, "@p_OtrasCxCPrecierre", DbType.Decimal, oRowS.OtrasCxCPreCierre)
            db.AddInParameter(dbCommand, "@p_ComisionUnificadaCuota", DbType.Decimal, IIf(IsDBNull(oRowS.ComisionUnificadaCuota), 0, oRowS.ComisionUnificadaCuota))
            db.AddInParameter(dbCommand, "@p_ComisionUnificadaMandato", DbType.Decimal, IIf(IsDBNull(oRowS.ComisionUnificadaMandato), 0, oRowS.ComisionUnificadaMandato))
            db.AddInParameter(dbCommand, "@p_DevolucionComisionDiaria", DbType.Decimal, IIf(IsDBNull(oRowS.DevolucionComisionDiaria), 0, oRowS.DevolucionComisionDiaria))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    Public Function PorcentajeComisionSerie(ByVal CodigoPortafolioSBS As String, ByVal CodigoSerie As String) As Decimal
        Using oValorCuotaBE As New ValorCuotaBE
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Using dbcomand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_PorcentajeComisionSerie")
                db.AddInParameter(dbcomand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
                db.AddInParameter(dbcomand, "@Serie", DbType.String, CodigoSerie)
                Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                    Return CDec(ds.Tables(0).Rows(0)(0).ToString())
                End Using
            End Using
        End Using
    End Function
    Public Sub PrecioValorCuota(ByVal CodigoPortafolioSBS As String, ByVal FechaProceso As Decimal, dataRequest As DataSet)
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Using oValorCuotaBE As New ValorCuotaBE
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Using dbcomand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_PrecioValorCuota")
                db.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
                db.AddInParameter(dbcomand, "@p_FechaProceso", DbType.Decimal, FechaProceso)
                db.AddInParameter(dbcomand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbcomand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbcomand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbcomand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.ExecuteNonQuery(dbcomand)
            End Using
        End Using
    End Sub
    'OT 9851 27/01/2017 - Carlos Espejo
    'Descripcion: Funcion para calcular Otras CXP
    'OT 9981 17/02/2017 - Carlos Espejo
    'Descripcion: Se cambia MesAnterior a decimal
    Public Function OtrasCXP(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ChequePendiente As Decimal, RescatePendiente As Decimal, _
    MesAnterior As Decimal, ByVal DevolucionAcumulada As Decimal) As DataTable
        Using oValorCuotaBE As New ValorCuotaBE
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Using dbcomand As DbCommand = db.GetStoredProcCommand("sp_SIT_OBT_OtrasCXP")
                db.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolioSBS)
                db.AddInParameter(dbcomand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
                db.AddInParameter(dbcomand, "@p_ChequePendiente", DbType.Decimal, ChequePendiente)
                db.AddInParameter(dbcomand, "@p_RescatePendiente", DbType.Decimal, RescatePendiente)
                db.AddInParameter(dbcomand, "@p_MesAnterior", DbType.Decimal, MesAnterior)
                db.AddInParameter(dbcomand, "@p_DevolucionAcumulada", DbType.Decimal, DevolucionAcumulada)
                db.ExecuteNonQuery(dbcomand)
                Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                    Return ds.Tables(0)
                End Using
            End Using
        End Using
    End Function
    Public Sub ValorCuota_ValoracionMensual(ByVal p_CodigoPortafolioSBS As String, ByVal p_FechaNew As Decimal, ByVal p_FechaOld As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using DbCommand As DbCommand = db.GetStoredProcCommand("ValorCuota_ValoracionMensual")
            db.AddInParameter(DbCommand, "@p_CodigoPortafolioSBS", DbType.String, p_CodigoPortafolioSBS)
            db.AddInParameter(DbCommand, "@p_FechaProcesoNew", DbType.Decimal, p_FechaNew)
            db.AddInParameter(DbCommand, "@p_FechaProcesoOld", DbType.Decimal, p_FechaOld)
            db.AddInParameter(DbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(DbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(DbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(DbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(DbCommand)
        End Using
    End Sub
    'OT10965 - 24/11/2017 - Ian Pastor M.
    Public Function ValorCuota_ObtenerRescateValores(ByVal p_CodigoPortafolioSisOpe As Decimal, ByVal p_FechaOperacion As DateTime, ByVal p_TipoProceso As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using DbCommand As DbCommand = db.GetStoredProcCommand("SIT_OBT_CIERRE_PROCESOS")
            db.AddInParameter(DbCommand, "@FECHA_PROCESO", DbType.DateTime, p_FechaOperacion)
            db.AddInParameter(DbCommand, "@ID_FONDO", DbType.Decimal, p_CodigoPortafolioSisOpe)
            db.AddInParameter(DbCommand, "@TIPO_PROCESO", DbType.String, p_TipoProceso)
            Using ds As DataSet = db.ExecuteDataSet(DbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT10965 - Fin
    'OT11008 - 19/01/2018 - Ian Pastor M.
    'Descripción: Obtiene los rescates pendientes del sistema de operaciones
    Public Function ValorCuota_ObtenerRescatesPendientes(ByVal p_CodigoPortafolioSisOpe As Decimal, ByVal p_FechaOperacion As DateTime) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using DbCommand As DbCommand = db.GetStoredProcCommand("INGF_LIS_RESCATE_PENDIENTE")
            db.AddInParameter(DbCommand, "@idFondo", DbType.Decimal, p_CodigoPortafolioSisOpe)
            db.AddInParameter(DbCommand, "@fecha", DbType.DateTime, p_FechaOperacion)
            Using ds As DataSet = db.ExecuteDataSet(DbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11237 - 15/02/2018 - Ian Pastor M.
    'Descripción: Obtiene los porcentaje series del sistema de operaciones
    Public Function ValorCuota_ObtenerPorcentajeSeries(ByVal p_CPPadreSisOpe As String, ByVal p_FechaOperacion As Date) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using DbCommand As DbCommand = db.GetStoredProcCommand("INGF_LIS_PROPORCION_FONDOS_PATRIMONIO")
            db.AddInParameter(DbCommand, "@fecha", DbType.Date, p_FechaOperacion)
            db.AddInParameter(DbCommand, "@codigoFondoPadre", DbType.String, p_CPPadreSisOpe)
            Using ds As DataSet = db.ExecuteDataSet(DbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11237 - Fin

    'OT11237 - 15/02/2018 - Ian Pastor M.
    'Descripción: Obtiene el último valor cuota calculado del fondo
    Public Function ValorCuota_ObtenerUltimoValorCuota(ByVal p_CodigoPortafolioSBS As String, ByVal p_CodigoSerie As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValorCuota_ObtenerUltimoValorCuota")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, p_CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, p_CodigoSerie)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11237 - Fin

    'OT11192 - 12/03/2018 - Ian Pastor M.
    'Descripción: Obtiene el monto de aporte valores del sistema de operaciones
    Public Function ObtenerAporteValoresSisOpe(ByVal p_CodigoPortafolioSisOpe As Decimal, ByVal p_Fecha As Date) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SIT_OBT_APORTE_VALORES")
            db.AddInParameter(dbCommand, "@FECHA_PROCESO", DbType.Date, p_Fecha)
            db.AddInParameter(dbCommand, "@ID_FONDO", DbType.Decimal, p_CodigoPortafolioSisOpe)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11192 - Fin

    'OT11143 - 01/03/2018 - Carlos Rumiche L.
    'Descripción: Obtiene los conceptos contables del portafolio Firbi
    Public Function SeleccionarConceptoContableFirbi(ByVal CodigoConceptoContable As String, ByVal situacion As String) As DataTable
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbcomand As DbCommand = db.GetStoredProcCommand("ConceptoContableFirbi_Seleccionar")
            db.AddInParameter(dbcomand, "@p_CodConceptoCont", DbType.String, CodigoConceptoContable)
            db.AddInParameter(dbcomand, "@p_Situacion", DbType.String, situacion)
            Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function RetornaSaldoDetalleContableFirbi(ByVal p_CuentaContable As String, ByVal p_Year As String, ByVal p_Month As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionPremium")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SIT_OBT_SALDOS_X_CTA")
            db.AddInParameter(dbCommand, "@Cuenta", DbType.String, p_CuentaContable)
            db.AddInParameter(dbCommand, "@Ano", DbType.String, p_Year)
            db.AddInParameter(dbCommand, "@Mes", DbType.String, p_Month)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                RetornaSaldoDetalleContableFirbi = ds.Tables(0)
            End Using
            'Dim objMonto As Object = db.ExecuteScalar(dbCommand)
            'If IsDBNull(objMonto) Then
            '    RetornaSaldoDetalleContableFirbi = 0
            'Else
            '    RetornaSaldoDetalleContableFirbi = CType(objMonto, Decimal)
            'End If
        End Using
    End Function
    Public Function ObtenerUltimoValorCuotaPrecierre_Cuotas(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As Decimal, _
                                                            ByVal p_CodigoSerie As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValorCuota_ObtenerUltimoValorCuotaPreCierre")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, p_FechaOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, p_CodigoSerie)
            Return CType(db.ExecuteScalar(dbCommand), Decimal)
        End Using
    End Function
    Public Function ObtenerFechaMaxima(ByVal p_CodigoPortafolio As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValorCuota_ObtenerFechaMaxima")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, p_CodigoPortafolio)
            Return CType(db.ExecuteScalar(dbCommand), Decimal)
        End Using
    End Function
    'OT11143 - Fin

    'OT11339 - 31/05/2018 - Ian Pastor M.
    'Descripción: Obtiene los cheques pendientes del sistema de operaciones
    Public Function ValorCuota_ObtenerChequePendiente(ByVal p_CodigoPortafolioSisOpe As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("INGF_LIS_CHEQUES_PENDIENTES_FONDO")
            db.AddInParameter(dbCommand, "@idFondo", DbType.Int32, p_CodigoPortafolioSisOpe)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11339 - Fin

    '06/05/2018 - Ian Pastor M.
    'Descripción: Obtiene las comisiones SAF del sistema de operaciones
    Public Function ObtenerComisionSAFSisOpe(ByVal p_CodigoPortafolioSisOpe As Integer, ByVal p_FechaInicio As Date, ByVal p_FechaFin As Date) As Decimal
        ObtenerComisionSAFSisOpe = 0
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("FMPR_OBT_COMISION_TOTAL_PORTAFOLIO_FECHA")
            db.AddInParameter(dbCommand, "@portafolio", DbType.Int32, p_CodigoPortafolioSisOpe)
            db.AddInParameter(dbCommand, "@fechaInicio", DbType.Date, p_FechaInicio)
            db.AddInParameter(dbCommand, "@fechaFin", DbType.Date, p_FechaFin)
            If IsDBNull(db.ExecuteScalar(dbCommand)) Then ObtenerComisionSAFSisOpe = 0 Else ObtenerComisionSAFSisOpe = CType(db.ExecuteScalar(dbCommand), Decimal)
        End Using
    End Function
    'Fin

    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se obtiene listado de movimiento de caja por operaciones OP0089 - Otras CxC | 22/06/18 
    Public Function ValorCuota_ObtenerImporteCaja_CxC(ByVal fechaOperacion As Decimal, ByVal CodigoPortafolioSBS As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbcomand As DbCommand = db.GetStoredProcCommand("ValorCuota_ObtenerImporteCaja_CxC")
            db.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbcomand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF019 - Se obtiene listado de movimiento de caja por operaciones OP0089 - Otras CxC | 22/06/18 
    Public Function ValorCuota_ObtenerDevolucionComisionUnificadaSisOpe(ByVal p_FechaInicio As Date, ByVal p_FechaFin As Date, ByVal p_NombrePortafolio As String) As Decimal
        ValorCuota_ObtenerDevolucionComisionUnificadaSisOpe = 0D
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("FMPR_OBT_COMISION_TOTAL_PORTAFOLIO_FECHA")
            db.AddInParameter(dbCommand, "@fechaInicio", DbType.Date, p_FechaInicio)
            db.AddInParameter(dbCommand, "@fechaFin", DbType.Date, p_FechaFin)
            db.AddInParameter(dbCommand, "@portafolio", DbType.String, p_NombrePortafolio)
            If IsDBNull(db.ExecuteScalar(dbCommand)) Then ValorCuota_ObtenerDevolucionComisionUnificadaSisOpe = 0 Else ValorCuota_ObtenerDevolucionComisionUnificadaSisOpe = CType(db.ExecuteScalar(dbCommand), Decimal)
        End Using
    End Function

	
    Public Function Variacion_Obtener(ByVal codigoPortafolioSBS As String, ByVal codigoSerie As String, ByVal fecha As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValorCuota_ObtenerVariacion")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, codigoSerie)
            db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function


    Public Function Variacion_ValidarExistenciaVariacion(ByVal codigoPortafolioSBS As String, ByVal codigoSerie As String, ByVal fecha As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValorCuota_ValidarExistenciaVariacion")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, codigoSerie)
            db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function Variacion_GenerarCalculo(ByVal codigoPortafolioSBS As String, ByVal codigoSerie As String, ByVal fecha As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValorCuota_FormulaVariacion")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, codigoSerie)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fecha)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function Variacion_DatosGrafica(ByVal codigoPortafolio As String, ByVal codigoSerie As String, ByVal fechaProceso As String, ByVal tipoPeriodo As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbcomand As DbCommand = db.GetStoredProcCommand("ValorCuota_GraficoDatos")
            db.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbcomand, "@p_CodigoSerie", DbType.String, codigoSerie)
            db.AddInParameter(dbcomand, "@p_FechaProceso", DbType.String, fechaProceso)
            db.AddInParameter(dbcomand, "@p_TipoPeriodo", DbType.String, tipoPeriodo)
            Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function Variacion_Insertar(ByVal valorCuotaVariacionBE As ValorCuotaVariacionBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbcomand As DbCommand = db.GetStoredProcCommand("ValorCuota_InsertarVariacion")
            db.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, valorCuotaVariacionBE.CodigoPortafolioSBS)
            db.AddInParameter(dbcomand, "@p_CodigoSerie", DbType.String, valorCuotaVariacionBE.CodigoSerie)
            db.AddInParameter(dbcomand, "@p_FechaProceso", DbType.Decimal, valorCuotaVariacionBE.FechaProceso)
            db.AddInParameter(dbcomand, "@p_CarteraPrecio", DbType.Decimal, valorCuotaVariacionBE.CarteraPrecio)
            db.AddInParameter(dbcomand, "@p_CarteraTipoCambio", DbType.Decimal, valorCuotaVariacionBE.CarteraTipoCambio)
            db.AddInParameter(dbcomand, "@p_Derivados", DbType.Decimal, valorCuotaVariacionBE.Derivados)
            db.AddInParameter(dbcomand, "@p_Comision", DbType.Decimal, valorCuotaVariacionBE.Comision)
            db.AddInParameter(dbcomand, "@p_CajaTipoCambio", DbType.Decimal, valorCuotaVariacionBE.CajaTipoCambio)
            db.AddInParameter(dbcomand, "@p_CuentasPorCobrarTipoCambio", DbType.Decimal, valorCuotaVariacionBE.CuentasPorCobrarTipoCambio)
            db.AddInParameter(dbcomand, "@p_CuentasPorPagarTipoCambio", DbType.Decimal, valorCuotaVariacionBE.CuentasPorPagarTipoCambio)
            db.AddInParameter(dbcomand, "@p_CuentasPorCobrarPrecio", DbType.Decimal, valorCuotaVariacionBE.CuentasPorCobrarPrecio)
            db.AddInParameter(dbcomand, "@p_CuentasPorPagarPrecio", DbType.Decimal, valorCuotaVariacionBE.CuentasPorPagarPrecio)
            db.AddInParameter(dbcomand, "@p_PorcentageVariacionEstimado", DbType.Decimal, valorCuotaVariacionBE.PorcentageVariacionEstimado)
            db.AddInParameter(dbcomand, "@p_TotalRentabilidadInversiones", DbType.Decimal, valorCuotaVariacionBE.TotalRentabilidadInversiones)
            db.AddInParameter(dbcomand, "@p_PorcentageVariacionSIT", DbType.Decimal, valorCuotaVariacionBE.PorcentageVariacionSIT)
            db.AddInParameter(dbcomand, "@p_DiferenciaEstimadoSIT", DbType.Decimal, valorCuotaVariacionBE.DiferenciaEstimadoSIT)
            db.AddInParameter(dbcomand, "@p_EstadoSemaforo", DbType.String, valorCuotaVariacionBE.EstadoSemaforo)
            db.AddInParameter(dbcomand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbcomand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbcomand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.ExecuteNonQuery(dbcomand)
            Return True
        End Using
    End Function
    Public Function ListarDistribucionFlujo() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("FOND_LIS_DISTRIBUCION_FLUJO")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarFondosComisionUnificadaSit(ByVal fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("FMPR_LIS_COMISION_UNIFICADA_x_FECHA")
        db.AddInParameter(dbCommand, "@fecha", DbType.Decimal, fecha)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ListarOperacionDevolucionDiaria(ByVal fechaDate As DateTime) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("FMPR_LIS_FONDOS_SERIE_COMISION_UNIFICADA_RESUMEN_FECHA")
        db.AddInParameter(dbCommand, "@fecha", DbType.Date, fechaDate)
        Using ds As DataSet = db.ExecuteDataSet(dbCommand)
            Return ds.Tables(0)
        End Using
    End Function
    Public Function ListarResumenPortafolios(ByVal fechaDate As DateTime, ByVal tipoCambio As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("FMPR_LIS_COMISION_UNIFICADA_X_PORTAFOLIO")
        db.AddInParameter(dbCommand, "@fecha", DbType.Date, fechaDate)
        db.AddInParameter(dbCommand, "@tipoCambio", DbType.Decimal, tipoCambio)
        Using ds As DataSet = db.ExecuteDataSet(dbCommand)
            Return ds.Tables(0)
        End Using
    End Function

    Public Function ListarFondosComisionUnificadaOperaciones(ByVal fechaDate As DateTime) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("FMPR_LIS_FONDOS_COMISION_UNIFICADA")
        db.AddInParameter(dbCommand, "@fecha", DbType.Date, fechaDate)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function CalcularDevolucionComisionesFondo(ByVal fechaCalculo As DateTime, ByVal idFondo As Integer, ByVal comisionUnificada As Decimal, ByVal totalCuotas As Decimal, ByVal eliminar As Boolean, ByVal tipoCambioSafp As Decimal, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using dbcomand As DbCommand = db.GetStoredProcCommand("FMPR_CAL_COMISION_UNIFICADA")
            db.AddInParameter(dbcomand, "@fecha", DbType.Date, fechaCalculo)
            db.AddInParameter(dbcomand, "@idFondo", DbType.Int32, idFondo)
            db.AddInParameter(dbcomand, "@comisionDia", DbType.Decimal, comisionUnificada)
            db.AddInParameter(dbcomand, "@numeroCuotas", DbType.Decimal, totalCuotas)
            db.AddInParameter(dbcomand, "@flag", DbType.Int32, eliminar)
            db.AddInParameter(dbcomand, "@usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbcomand, "@tipoCambio", DbType.Decimal, tipoCambioSafp)
            db.ExecuteNonQuery(dbcomand)
            Return True
        End Using
    End Function

    Public Function ValidarNegociacionFondosEnOtros(ByVal codigoPortafolioSBS As String, ByVal fecha As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ValidarNegociacionFondosEnOtros")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fecha)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function ListarFondosConNombreSerie(ByVal codigoPortafolioSBS As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ListarFondosNombreConSerie")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)

            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function RegistrarDevolucionComision(ByVal codigoPortafolioSBS As String, ByVal fechaOperacion As Decimal, ByVal montoDevolucion As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DevolucionComisiones_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
            db.AddInParameter(dbCommand, "@p_MontoComision", DbType.Decimal, montoDevolucion)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, "A")
            db.ExecuteDataSet(dbCommand)
            Return True
        End Using
    End Function

    Public Function ActualizarDevolucionComisiones(ByVal id As Integer, ByVal codigoPortafolioSBS As String, ByVal fechaOperacion As Decimal, ByVal montoDevolucion As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DevolucionComisiones_Modificar")
            db.AddInParameter(dbCommand, "@p_Id", DbType.Int32, id)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
            db.AddInParameter(dbCommand, "@p_MontoComision", DbType.Decimal, montoDevolucion)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, "A")
            db.ExecuteDataSet(dbCommand)
            Return True
        End Using
    End Function

    Public Function SeleccionarDevolucionComision(ByVal codigoPortafolioSBS As String, ByVal fechaOperacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DevolucionComisiones_Seleccionar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, "A")
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function ListarCuotasFondosOperaciones(ByVal fechaOperacionCadena As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("INGF_LIS_PRECIERRE")
            db.AddInParameter(dbCommand, "@fecha", DbType.String, fechaOperacionCadena)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

End Class