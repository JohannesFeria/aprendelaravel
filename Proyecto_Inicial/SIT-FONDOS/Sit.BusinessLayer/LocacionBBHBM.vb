'Creado por: HDG OT 64765 20120312
Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class LocacionBBHBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oLocacionBBHBE As LocacionBBHBE, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oLocacionBBHBE, dataRequest}

        Try
            Dim oLocacionBBHDAM As New LocacionBBHDAM

            oLocacionBBHDAM.Insertar(oLocacionBBHBE, dataRequest)

            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarPorFiltro(ByVal location As String, ByVal situacion As String, ByVal dataRequest As DataSet) As LocacionBBHBE

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {location, situacion, dataRequest}

        Try
            RegistrarAuditora(parameters)

            Return New LocacionBBHDAM().SeleccionarPorFiltro(location, situacion)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function
    Public Function Seleccionar(ByVal CodigoLocation As String, ByVal dataRequest As DataSet) As LocacionBBHBE

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoLocation, dataRequest}

        Try
            RegistrarAuditora(parameters)

            Return New LocacionBBHDAM().Seleccionar(CodigoLocation)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function Modificar(ByVal oLocacionBBHBE As LocacionBBHBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oLocacionBBHBE, dataRequest}

        Dim oLocacionBBHDAM As New LocacionBBHDAM

        Try

            actualizado = oLocacionBBHDAM.Modificar(oLocacionBBHBE, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado

    End Function

    Public Function Eliminar(ByVal CodigoLocation As String, ByVal dataRequest As DataSet) As Boolean

        Dim eliminado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoLocation, dataRequest}
        Dim oLocacionBBHDAM As New LocacionBBHDAM

        Try

            eliminado = oLocacionBBHDAM.Eliminar(CodigoLocation, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado

    End Function

    Public Function ListarLocacionBBH() As DataSet
        Try
            Return New LocacionBBHDAM().ListarLocacionBBH()
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ActualizarTransactionPorExcel(ByVal dtData As DataTable, ByVal proceso As String, ByVal dataRequest As DataSet, ByRef strmensaje As String, ByVal fechaTrans As Decimal) As Boolean
        Dim Codigo = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtData, dataRequest}

        Try
            Dim oLocacionBBHDAM As New LocacionBBHDAM
            Codigo = oLocacionBBHDAM.ActualizarTransactionPorExcel(dtData, proceso, dataRequest, strmensaje, fechaTrans)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            Dim sms As String = ex.Message
            Dim src As String = ex.Source
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function

    Public Function ActualizarTransCustodioPorTxt(ByVal objTransCustodioBBHBE As TransCustodioBBHBE, ByVal proceso As String, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim Codigo = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {objTransCustodioBBHBE, dataRequest}

        Try
            Dim oLocacionBBHDAM As New LocacionBBHDAM
            Codigo = oLocacionBBHDAM.ActualizarTransCustodioPorTxt(objTransCustodioBBHBE, proceso, dataRequest, strmensaje)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            Dim sms As String = ex.Message
            Dim src As String = ex.Source
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function

    Public Function ComisionCustodioTrasacciones(ByVal CodigoPortafolio As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim oReporte As New DataSet
        Try
            Dim oLocacionBBHDAM As New LocacionBBHDAM
            oReporte = oLocacionBBHDAM.ComisionCustodioTrasacciones(CodigoPortafolio, FechaInicio, FechaFin)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oReporte
    End Function
End Class

