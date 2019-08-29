Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class BalanceContableBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal CodigoEmisor As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As BalanceContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoEmisor, Situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New BalanceContableDAM().SeleccionarPorFiltro(CodigoEmisor, Situacion, dataRequest)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function


    Public Function Listar(ByVal dataRequest As DataSet) As BalanceContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New BalanceContableDAM().Listar(dataRequest)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function


#End Region

    Public Function Insertar(ByVal oBalanceContable As BalanceContableBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oBalanceContable, dataRequest}
        Try
            Dim oBalanceContableDAM As New BalanceContableDAM
            oBalanceContableDAM.Insertar(oBalanceContable, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function

    'OAB 20091023
    'Se implementó para que se pueda modificar el balance contable a traves de un archivo excel
    Public Function ActualizarPorExcel(ByVal dtBalanceContable As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim strCodigo = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtBalanceContable, dataRequest}

        Try
            Dim daBalanceContable As New BalanceContableDAM
            strCodigo = daBalanceContable.ActualizarPorExcel(dtBalanceContable, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function

    Public Function Modificar(ByVal oBalanceContable As BalanceContableBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oBalanceContable, dataRequest}
        Try
            Dim oBalanceContableDAM As New BalanceContableDAM
            actualizado = oBalanceContableDAM.Modificar(oBalanceContable, dataRequest)
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

    Public Function Eliminar(ByVal CodigoEmisor As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {CodigoEmisor, dataRequest}
        Try
            Dim oBalanceContableDAM As New BalanceContableDAM
            eliminado = oBalanceContableDAM.Eliminar(CodigoEmisor, dataRequest)
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

    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub

End Class
