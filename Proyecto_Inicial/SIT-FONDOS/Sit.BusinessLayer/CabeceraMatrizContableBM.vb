Imports System
Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class CabeceraMatrizContableBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Function Seleccionar(ByVal strCodigoMatrizContable As String, ByVal dataRequest As DataSet) As CabeceraMatrizContableBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoMatrizContable, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New CabeceraMatrizContableDAM().Seleccionar(strCodigoMatrizContable, dataRequest)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarPorFiltro(ByVal ob As CabeceraMatrizContableBE, ByVal MatrizFondo As String) As DataSet
        Dim oCabeceraMatrizContableDAM As New CabeceraMatrizContableDAM
        Dim oCabeceraMatrizContableBE As New DataSet
        Try
            oCabeceraMatrizContableBE = oCabeceraMatrizContableDAM.SeleccionarPorFiltro(ob, MatrizFondo)
        Catch ex As Exception
            Throw ex
        End Try
        Return oCabeceraMatrizContableBE
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim oCabeceraMatrizContableBE As New CabeceraMatrizContableBE
        Dim oCabeceraMatrizContableDAM As New CabeceraMatrizContableDAM
        Try
            RegistrarAuditora(parameters)
            oCabeceraMatrizContableBE = oCabeceraMatrizContableDAM.Listar(dataRequest)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oCabeceraMatrizContableBE
    End Function
    Public Function Insertar_1(ByVal oCabeceraMatrizContable As CabeceraMatrizContableBE, ByVal dataRequest As DataSet) As String
        Dim strCodigoMatrizContable As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCabeceraMatrizContable, dataRequest}
        Try
            Dim oCabeceraMatrizContableDAM As New CabeceraMatrizContableDAM
            strCodigoMatrizContable = oCabeceraMatrizContableDAM.Insertar_1(oCabeceraMatrizContable, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return strCodigoMatrizContable
    End Function
    Public Function Eliminar(ByVal codigoMatrizContable As String, ByVal dataRequest As DataSet) As Boolean
        Dim strCodigoMatrizContable As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoMatrizContable, dataRequest}
        Try
            Dim oCabeceraMatrizContableDAM As New CabeceraMatrizContableDAM
            strCodigoMatrizContable = oCabeceraMatrizContableDAM.Eliminar(codigoMatrizContable)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return strCodigoMatrizContable
    End Function
    Public Function Modificar(ByVal oCabeceraMatrizContable As CabeceraMatrizContableBE, ByVal dataRequest As DataSet) As Boolean
        Dim strCodigoMatrizContable As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCabeceraMatrizContable, dataRequest}
        Try
            Dim oCabeceraMatrizContableDAM As New CabeceraMatrizContableDAM
            strCodigoMatrizContable = oCabeceraMatrizContableDAM.Modificar(oCabeceraMatrizContable, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function
    Public Function ValidaExistenciaMatrizFondo(CodigoMatrizContable As Decimal, ByVal CodigoNegocio As String, CodigoSerie As String) As Integer
        Try
            Dim oCabeceraMatrizContableDAM As New CabeceraMatrizContableDAM
            Return oCabeceraMatrizContableDAM.ValidaExistenciaMatrizFondo(CodigoMatrizContable, CodigoNegocio, CodigoSerie)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class