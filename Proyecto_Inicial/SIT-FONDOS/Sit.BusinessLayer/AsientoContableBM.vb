Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Transactions 'OT10783 - Librería de transacciones
Public Class AsientoContableBM
    Inherits InvokerCOM
    'OT10783 - Se eliminó código innecesario en todos los métodos.
    Public Sub New()
    End Sub
    Public Function InsertarRevision(ByVal objAsientoContableRow As AsientoContableBE.AsientoContableRow, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        Try
            Dim daAsientoContable As New AsientoContableDAM
            strCodigo = daAsientoContable.InsertarRevision(objAsientoContableRow, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return strCodigo
    End Function
    Public Function SeleccionarPorFiltro(ByVal fechaAsiento As Decimal, ByVal codigoPortafolioSBS As String, ByVal dataRequest As DataSet) As AsientoContableBE
        Dim oAsientoContableBE As AsientoContableBE
        Dim oAsientoContableDAM As New AsientoContableDAM
        Try
            oAsientoContableBE = oAsientoContableDAM.SeleccionarPorFiltro(fechaAsiento, codigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
        Return oAsientoContableBE
    End Function
    Public Function SeleccionarPorFiltroRevision(ByVal fechaAsiento As Decimal, ByVal codigoPortafolioSBS As String, ByVal codigoMoneda As String, ByVal tipoLote As String, ByVal CodigoMercado As String, ByVal dataRequest As DataSet) As AsientoContableBE
        Dim oAsientoContableBE As AsientoContableBE
        Dim oAsientoContableDAM As New AsientoContableDAM
        Try
            oAsientoContableBE = oAsientoContableDAM.SeleccionarPorFiltroRevision(fechaAsiento, codigoPortafolioSBS, codigoMoneda, tipoLote, CodigoMercado)
        Catch ex As Exception
            Throw ex
        End Try
        Return oAsientoContableBE
    End Function
    Public Function RetornarTipoCambio(ByVal strMoneda As String, ByVal strFecha As String, ByVal dataRequest As DataSet) As DataTable
        Dim oAsientoContableDAM As New AsientoContableDAM
        Dim oDS As DataSet
        Try
            oDS = oAsientoContableDAM.RetornarTipoCambio(strMoneda, strFecha)
        Catch ex As Exception
            Throw ex
        End Try
        Return oDS.Tables(0)
    End Function
    Public Function RetornarTipoCambio_T1(ByVal strMoneda As String, ByVal Escenario As String, ByVal Fecha As Decimal) As Decimal
        Dim oAsientoContableDAM As New AsientoContableDAM
        Dim oDS As Decimal
        Try
            oDS = oAsientoContableDAM.RetornarTipoCambio_T1(strMoneda, Escenario, Fecha)
        Catch ex As Exception
            Throw ex
        End Try
        Return oDS
    End Function
    Public Function ListarNumeroAsiento(ByVal IndiceReferencial As String, ByVal portafolio As String, ByVal fechaAsiento As Decimal, ByVal dataRequest As DataSet) As DataTable
        Dim oAsientoContableBE As DataSet
        Dim oAsientoContableDAM As New AsientoContableDAM
        Dim DtTabla As DataTable
        Try
            DtTabla = oAsientoContableDAM.ListarNumerosAsiento(IndiceReferencial, portafolio, fechaAsiento).Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
        Return DtTabla
    End Function
    Public Function Modificar(ByVal oAsientoContableBE As AsientoContableBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim oAsientonContableDAM As New AsientoContableDAM
        Try
            actualizado = oAsientonContableDAM.Modificar(oAsientoContableBE, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function
    Public Function ModificarRevision(ByVal CodigoPortafolioSBSKey As String, ByVal NumeroAsientoKey As String, ByVal SecuenciaKey As String, ByVal obj As AsientoContableBE.AsientoContableRow, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        Try
            Dim daAsientoContable As New AsientoContableDAM
            strCodigo = daAsientoContable.ModificarRevision(CodigoPortafolioSBSKey, NumeroAsientoKey, SecuenciaKey, obj, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return strCodigo
    End Function
    Public Function EliminarRevision(ByVal CodigoPortafolioSBSKey As String, ByVal NumeroAsientoKey As String, ByVal SecuenciaKey As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        Try
            Dim daAsientoContable As New AsientoContableDAM
            strCodigo = daAsientoContable.EliminarRevision(CodigoPortafolioSBSKey, NumeroAsientoKey, SecuenciaKey, fecha)
        Catch ex As Exception
            Throw ex
        End Try
        Return strCodigo
    End Function
    Public Function GenerarAsientoContable(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal tipoLote As String, ByVal dataRequest As DataSet, Optional ByVal CodigoMercado As String = "") As ArrayList
        'OT10783 - Generar asientos de ganancia y pérdida para los lotes contables CVI
        Dim oAsientoContable As New AsientoContableDAM
        Dim NumListaAsientos As ArrayList
        Try
            Using Transaction As New TransactionScope
                If tipoLote = "CCI" Then
                    NumListaAsientos = oAsientoContable.GenerarAsientoContable(codigoPortafolio, fechaOperacion, tipoLote, dataRequest, CodigoMercado)
                Else
                    NumListaAsientos = oAsientoContable.GenerarAsientoContable(codigoPortafolio, fechaOperacion, tipoLote, dataRequest)
                    If tipoLote = "CVI" Then
                        If VerificarGananciaPerdidaCVI(codigoPortafolio, fechaOperacion) Then
                            oAsientoContable.AsientoContable_GenerarGananciaYperdida(codigoPortafolio, fechaOperacion, dataRequest)
                        End If
                    End If
                End If
                Transaction.Complete()
                Return NumListaAsientos
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        'OT10783 - Fin
    End Function
    Public Function GenerarAsientoContableSAFM(ByVal codigoPortafolio As String, CodigoSerie As String, CodigoMatriz As String, ByVal fechaOperacion As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim oAsientoContable As New AsientoContableDAM
        Try
            oAsientoContable.GenerarAsientoContableSAFM(codigoPortafolio, CodigoSerie, CodigoMatriz, fechaOperacion, dataRequest)
            Return True
        Catch ex As Exception
            Throw ex
        Finally
            oAsientoContable = Nothing
            GC.Collect()
        End Try
    End Function
    Public Sub Extornar(ByVal codigoPortafolio As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet)
        Try
            Dim oAsientoContable As New AsientoContableDAM
            oAsientoContable.Extornar(codigoPortafolio, fechaProceso, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function AsientoContable_SeleccionarPorFiltroRevisionCabecera(ByVal fechaAsiento As Decimal, ByVal codigoPortafolioSBS As String, ByVal codigoMoneda As String, ByVal dataRequest As DataSet) As AsientoContableBE
        Dim oAsientoContableBE As AsientoContableBE
        Dim oAsientoContableDAM As New AsientoContableDAM
        Try
            oAsientoContableBE = oAsientoContableDAM.AsientoContable_SeleccionarPorFiltroRevisionCabecera(fechaAsiento, codigoPortafolioSBS, codigoMoneda)
        Catch ex As Exception
            Throw ex
        End Try
        Return oAsientoContableBE
    End Function
    'obtencion de datos para generar archivos de ristra contable
    Public Function Seleccionar_RistraContable(ByVal portafolio As String, ByVal fechaAperturaDesde As Decimal, ByVal fechaAperturaHasta As Decimal, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim dsConsulta As DataSet
        Dim oAsientoContableDAM As New AsientoContableDAM
        Try
            dsConsulta = oAsientoContableDAM.Seleccionar_RistraContable(portafolio, fechaAperturaDesde, fechaAperturaHasta, fechaProceso)
        Catch ex As Exception
            Throw ex
        End Try
        Return dsConsulta
    End Function
    Public Function AsientoContable_Interface(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal tipoLote As String) As DataSet
        Dim dsConsulta As DataSet
        Dim oAsientoContableDAM As New AsientoContableDAM
        Try
            dsConsulta = oAsientoContableDAM.AsientoContable_Interface(codigoPortafolio, fechaOperacion, tipoLote)
        Catch ex As Exception
            Throw ex
        End Try
        Return dsConsulta
    End Function
    'OT10783 - Método que verifica si existen diferencias de operaciones de compra y venta en los asientos contables
    Public Function VerificarGananciaPerdidaCVI(ByVal p_CodigoPortafolio As String, ByVal p_FechaOperacion As Decimal) As Boolean
        VerificarGananciaPerdidaCVI = False
        Dim operaciones_T As DataTable
        Dim operaciones_T1 As DataTable
        Dim oValorCuotaDAM As New ValorCuotaDAM
        Dim montoRow() As DataRow
        operaciones_T = oValorCuotaDAM.CXCVentaCompra("0", p_CodigoPortafolio, p_FechaOperacion)
        Dim fechaOperacionAnterior As Decimal = UtilDM.RetornarFechaAnterior(p_FechaOperacion)
        operaciones_T1 = oValorCuotaDAM.CXCVentaCompra("0", p_CodigoPortafolio, fechaOperacionAnterior)
        For Each dr As DataRow In operaciones_T.Rows
            If dr("CodigoOperacion") = "1" Or dr("CodigoOperacion") = "2" Then
                montoRow = operaciones_T1.Select("CodigoOrden='" & dr("CODIGOORDEN") & "'")
                If montoRow.Length > 0 Then
                    If dr("MontoMonedaFondo") <> montoRow(0)("MontoMonedaFondo") Then
                        VerificarGananciaPerdidaCVI = True
                        Exit For
                    End If
                End If
            End If
        Next
    End Function
End Class