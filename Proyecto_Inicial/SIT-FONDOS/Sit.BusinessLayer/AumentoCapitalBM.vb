Imports MotorTransaccionesProxy
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer


Public Class AumentoCapitalBM
    Inherits InvokerCOM
    Dim oAumentoCapitalDAM As New AumentoCapitalDAM
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function AumentoCapital_CalcularDistribucion(ByVal CategoriaInstrumento As String, ByVal CodigoPortafolioSBS As String, ByVal FechaAumentoCapital As Decimal) As DataTable
        Try
            Return oAumentoCapitalDAM.AumentoCapital_CalcularDistribucion(CategoriaInstrumento, CodigoPortafolioSBS, FechaAumentoCapital)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function AumentoCapital_ExisteGeneradaOI(ByVal CodigoPortafolioSBS As String, ByVal FechaAumentoCapital As Decimal) As Integer
        Dim oDS As Integer
        Try
            oDS = oAumentoCapitalDAM.AumentoCapital_ExisteGeneradaOI(CodigoPortafolioSBS, FechaAumentoCapital)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function

    Public Function AumentoCapital_ExistePendientebyFechaPortafolio(ByVal CodigoPortafolioSBS As String, ByVal FechaAumentoCapital As Decimal) As Integer
        Dim oDS As Integer
        Try
            oDS = oAumentoCapitalDAM.AumentoCapital_ExistePendientebyFechaPortafolio(CodigoPortafolioSBS, FechaAumentoCapital)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDS
    End Function

    Public Function AumentoCapital_ListarbyFechaPortafolio(ByVal CodigoPortafolioSBS As String, ByVal FechaAumentoCapital As Decimal) As DataSet
        Try
            Return oAumentoCapitalDAM.AumentoCapital_ListarbyFechaPortafolio(CodigoPortafolioSBS, FechaAumentoCapital)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region " /* Funciones Insertar */ "
    Public Function AumentoCapital_Insertar(ByVal oRowAC As AumentoCapitalBE, ByVal dataRequest As DataSet) As Integer
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oRowAC.CodigoPortafolioSBS, oRowAC.FechaAumentoCapital, dataRequest}
        Dim result As Integer
        Try
            result = oAumentoCapitalDAM.AumentoCapital_Insertar(oRowAC, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return IIf(CType(result, Integer) = 0, 0, result)
    End Function
#End Region

#Region " /* Funciones Modificar */"
    Public Function AumentoCapital_Modificar(ByVal oRowAC As AumentoCapitalBE, ByVal fechaAumentoCapitalOriginal As Decimal, ByVal dataRequest As DataSet) As Integer
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oRowAC.CodigoPortafolioSBS, oRowAC.FechaAumentoCapital, dataRequest}
        Dim result As Integer
        Try
            result = oAumentoCapitalDAM.AumentoCapital_Modificar(oRowAC, fechaAumentoCapitalOriginal, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return IIf(CType(result, Integer) = 0, 0, result)
    End Function

    Public Function AumentoCapital_ActualizarGastoComision(ByVal codigoPortafolioSBS As String, ByVal FechaProceso As Decimal, ByVal DiferenciaComision As Decimal, ByVal dataRequest As DataSet) As Integer
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPortafolioSBS, FechaProceso, dataRequest}
        Dim result As Integer
        Try
            result = oAumentoCapitalDAM.AumentoCapital_ActualizarGastoComision(codigoPortafolioSBS, FechaProceso, DiferenciaComision, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return IIf(CType(result, Integer) = 0, 0, result)
    End Function

#End Region

#Region " /* Funciones Eliminar */"
    Public Function AumentoCapital_Eliminar(ByVal oRowAC As AumentoCapitalBE, ByVal dataRequest As DataSet) As Integer
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oRowAC.CodigoPortafolioSBS, oRowAC.FechaAumentoCapital, dataRequest}
        Dim result As Integer
        Try
            result = oAumentoCapitalDAM.AumentoCapital_Eliminar(oRowAC, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return IIf(CType(result, Integer) = 0, 0, result)
    End Function
#End Region

#Region " /* Funciones Personalizadas*/"
    Public Sub InicializarAumentoCapital(ByRef oRowAC As AumentoCapitalBE)
        Try
            oAumentoCapitalDAM.InicializarAumentoCapital(oRowAC)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub
#End Region



End Class

