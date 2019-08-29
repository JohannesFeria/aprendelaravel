Imports System
Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Transactions

Public Class CarteraTituloValoracionBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Function SeleccionarDuraciones(ByVal CodigoPortafolioSBS As String, ByVal CodigoMnemonico As String, ByVal fechaValoracion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oCarteraTituloValoracion As New DataSet
        Try
            oCarteraTituloValoracion = New CarteraTituloValoracionDAM().SeleccionarDuraciones(CodigoPortafolioSBS, CodigoMnemonico, fechaValoracion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return oCarteraTituloValoracion
    End Function
    Public Function SeleccionarDetalleDuraciones(ByVal CodigoPortafolioSBS As String, ByVal fechaValoracion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oCarteraTituloValoracion As New DataSet
        Try
            oCarteraTituloValoracion = New CarteraTituloValoracionDAM().SeleccionarDetalleDuraciones(CodigoPortafolioSBS, fechaValoracion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return oCarteraTituloValoracion
    End Function
    Public Function ObtenerFechaValoracion(ByVal CodigoPortafolioSBS As String, ByVal Escenario As String, ByVal Indicador As Boolean) As String
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Dim FechaValoracion As String
        Try
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            FechaValoracion = oCarteraTituloValoracionDAM.ObtenerFechaValoracion(CodigoPortafolioSBS, Escenario, Indicador)
        Catch ex As Exception
            Throw ex
        End Try
        Return FechaValoracion
    End Function
    Public Function ObtenerFechaValoracion(ByVal CodigoPortafolioSBS As String) As Decimal
        Dim FechaValoracion As Decimal
        Try
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            FechaValoracion = oCarteraTituloValoracionDAM.ObtenerFechaValoracion(CodigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
        Return FechaValoracion
    End Function
    Public Function UltimaValoracion(ByVal fechaValoracion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oPort As DataSet
        Try
            oPort = New CarteraTituloValoracionDAM().UltimaValoracion(fechaValoracion)
        Catch ex As Exception
            Throw ex
        End Try
        Return oPort
    End Function
    Public Function Validar(ByVal CodigoPortafolioSBS As String, ByVal fechaValoracion As Decimal) As Boolean
        Try
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            fechaValoracion = oCarteraTituloValoracionDAM.Validar(CodigoPortafolioSBS, fechaValoracion)
        Catch ex As Exception
            Throw ex
        End Try
        Return fechaValoracion
    End Function
    Public Function ReporteVL(ByVal fecha1 As Decimal) As DataTable
        Try
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            Return oCarteraTituloValoracionDAM.ReporteVL(fecha1)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub ReporteVLGenerar(ByVal fecha1 As Decimal, ByVal CodigoPortafolio As Integer)
        Try
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            oCarteraTituloValoracionDAM.ReporteVLGenerar(fecha1, CodigoPortafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function ReporteVLObtener(ByVal fecha1 As Decimal, ByVal Privados As Integer) As DataTable
        Try
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            Return oCarteraTituloValoracionDAM.ReporteVLObtener(fecha1, Privados)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Inicio OT10902 - 23/10/2017 - Jorge Benites
    'Reporte de diferencias en el proceso de valorización
    Public Function DiferenciaReporteVLObtener(ByVal fecha1 As Decimal, ByVal Privados As Integer) As DataTable
        Try
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            Return oCarteraTituloValoracionDAM.DiferenciaReporteVLObtener(fecha1, Privados)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'FIN OT10902
    Public Function ValidadFondos(ByVal fecha1 As Decimal, ByVal Privados As Integer) As DataTable
        Try
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            Return oCarteraTituloValoracionDAM.ValidadFondos(fecha1, Privados)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ExisteValorizacionFechasPosteriores(ByVal CodigoPortafolioSBS As String, ByVal FechaValoracion As Decimal) As Decimal
        Try
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            Return CType(oCarteraTituloValoracionDAM.ExisteValorizacionFechasPosteriores(CodigoPortafolioSBS, FechaValoracion), Decimal)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Valida_PRECIERRE_CONSOLIDADO_OPERACIONES(ByVal FechaInicio As Date, ByVal FechaFinal As Date, ByVal idFondoOperaciones As Decimal) As Decimal
        Try
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            Return oCarteraTituloValoracionDAM.Valida_PRECIERRE_CONSOLIDADO_OPERACIONES(FechaInicio, FechaFinal, idFondoOperaciones)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ReporteVLDiferencia(ByVal FechaHoy As Decimal, ByVal FechaAyer As Decimal) As DataTable
        Try
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            Return oCarteraTituloValoracionDAM.ReporteVLDiferencia(FechaHoy, FechaAyer)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Inicio OT10902 - 23/10/2017 - Jorge Benites
    'Obtener diferencias en el proceso de valorización
    Public Function ObtenerDiferenciaReporteVL(ByVal FechaHoy As Decimal, ByVal Privado As Decimal) As DataTable
        Try
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            Return oCarteraTituloValoracionDAM.ObtenerDiferenciaReporteVL(FechaHoy, Privado)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Fin OT10902
    Public Function ObtenerValorCuotaPreCierreOperaciones(ByVal Fecha As String, ByVal CodigoPortafolioSisOpe As String) As DataTable
        Try
            Dim FechaValorCuota As String
            FechaValorCuota = Fecha.ToString().Substring(6, 4) & "-" & Fecha.ToString().Substring(3, 2) & "-" & Fecha.ToString().Substring(0, 2)
            Dim oCarteraTituloValoracionDAM As New CarteraTituloValoracionDAM
            Return oCarteraTituloValoracionDAM.ObtenerValorCuotaPreCierreOperaciones(FechaValorCuota, CodigoPortafolioSisOpe)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CarteraTituloValoracion_GenerarValoracionMensual(ByVal p_CodigoPortafolio As String, ByVal p_FechaInicio As Decimal, ByVal p_FechaFin As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim ValoracionMensual As Boolean = False
        Dim objValoresDAM As ValoresDAM
        Dim objCarteraTituloValoracionDAM As CarteraTituloValoracionDAM
        Dim objCarteraDAM As CarteraDAM
        Dim objPortafolioDAM As PortafolioDAM
        Dim objPrevOrdenInversion As PrevOrdenInversionDAM
        Dim objCustodio As CustodioArchivoDAM
        Dim objValorCuotaDAM As ValorCuotaDAM
        Dim fechaAnterior As Decimal
        Try
            Using Transaction As New TransactionScope
                objValoresDAM = New ValoresDAM
                objCarteraTituloValoracionDAM = New CarteraTituloValoracionDAM
                objCarteraDAM = New CarteraDAM
                objPortafolioDAM = New PortafolioDAM
                objPrevOrdenInversion = New PrevOrdenInversionDAM
                objCustodio = New CustodioArchivoDAM
                objValorCuotaDAM = New ValorCuotaDAM
                While p_FechaInicio <= p_FechaFin
                    'Reversar Portafolio
                    'objCustodio.GeneraSaldoBanco(p_FechaInicio, 0, p_CodigoPortafolio, "10", dataRequest)
                    'objCustodio.GeneraSaldoBanco(p_FechaInicio, 0, p_CodigoPortafolio, "20", dataRequest)
                    'objPortafolioDAM.Actualiza_FechaNegocio(p_CodigoPortafolio, p_FechaInicio)

                    'Apertura de Portafolio
                    fechaAnterior = UtilDM.RetornarFechaAnterior(p_FechaInicio)
                    objCarteraDAM.GeneraSaldosCarteraTitulo(fechaAnterior, p_FechaInicio, p_CodigoPortafolio, dataRequest)
                    objPortafolioDAM.Cerrar(p_CodigoPortafolio, p_FechaInicio, dataRequest) 'Actualiza la fecha de constitucion y la fecha de termino del portafolio
                    objPrevOrdenInversion.TruncarProcesoMasivo()
                    objCustodio.GeneraSaldos(p_FechaInicio, fechaAnterior, p_CodigoPortafolio, dataRequest)

                    'Valoración del Portafolio
                    objValoresDAM.ExtornarValorizacionCartera(p_CodigoPortafolio, p_FechaInicio, dataRequest)
                    objValoresDAM.BorraMontoInversion(p_FechaInicio, p_CodigoPortafolio)
                    objValoresDAM.GenerarValorizacionCartera(p_CodigoPortafolio, p_FechaInicio, "REAL", "REAL", "R", dataRequest)
                    objValoresDAM.CalculaMontoInversion(p_FechaInicio, p_CodigoPortafolio)

                    'Generación VL
                    objCarteraTituloValoracionDAM.ReporteVLGenerar(p_FechaInicio, p_CodigoPortafolio)

                    'Generación Valor Cuota
                    objValorCuotaDAM.ValorCuota_ValoracionMensual(p_CodigoPortafolio, p_FechaInicio, fechaAnterior, dataRequest)

                    p_FechaInicio = p_FechaInicio + 1
                End While
                Transaction.Complete()
                ValoracionMensual = True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return ValoracionMensual
    End Function
End Class