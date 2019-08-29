Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities


Public Class MontoNegociadoBVLBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal FechaOperacion As Decimal, ByVal NumeroOperacion As Decimal, ByVal CodigoMnemonico As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As MontoNegociadoBVLBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaOperacion, CodigoMnemonico, Situacion, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New MontoNegociadoBVLDAM().SeleccionarPorFiltro(FechaOperacion, NumeroOperacion, CodigoMnemonico, Situacion, dataRequest)
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


    Public Function Listar(ByVal dataRequest As DataSet) As MontoNegociadoBVLBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New MontoNegociadoBVLDAM().Listar(dataRequest)
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

    Public Function Insertar(ByVal oMontoNegociadoBVL As MontoNegociadoBVLBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oMontoNegociadoBVL, dataRequest}
        Try
            Dim oMontoNegociadoBVLDAM As New MontoNegociadoBVLDAM
            oMontoNegociadoBVLDAM.Insertar(oMontoNegociadoBVL, dataRequest)
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


    Public Function Modificar(ByVal oMontoNegociadoBVL As MontoNegociadoBVLBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oMontoNegociadoBVL, dataRequest}
        Try
            Dim oMontoNegociadoBVLDAM As New MontoNegociadoBVLDAM
            actualizado = oMontoNegociadoBVLDAM.Modificar(oMontoNegociadoBVL, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado
    End Function


    Public Function Eliminar(ByVal FechaOperacion As Decimal, ByVal NumeroOperacion As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaOperacion, NumeroOperacion, dataRequest}
        Try
            Dim oMontoNegociadoBVLDAM As New MontoNegociadoBVLDAM
            eliminado = oMontoNegociadoBVLDAM.Eliminar(FechaOperacion, NumeroOperacion, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
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
