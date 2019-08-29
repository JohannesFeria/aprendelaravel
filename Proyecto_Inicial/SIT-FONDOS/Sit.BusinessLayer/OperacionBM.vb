Imports System
Imports System.Data
Imports System.Data.Common
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class OperacionBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Function Insertar(ByVal oOperacionBE As OperacionBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oOperacionBE, dataRequest}
        Try
            Dim oOperacionDAM As New OperacionDAM
            oOperacionDAM.Insertar(oOperacionBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarPorFiltro(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As OperacionBE
        Dim oOperacionDAM As New OperacionDAM
        Dim intCodigoEjecucion As Integer
        Dim parameters As Object() = {descripcion, situacion, dataRequest}
        Dim oOperacionBE As OperacionBE
        Try
            intCodigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
            oOperacionBE = oOperacionDAM.SeleccionarPorFiltro(descripcion, situacion)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oOperacionBE
    End Function
    Public Function ListaModeloCartaOperacion(ByVal codigoOperacion As String, ByVal CodigoOrden As String) As DataTable
        Dim operacion As New OperacionDAM
        Dim dt As DataTable
        Try
            dt = operacion.ListaModeloCartaOperacion(codigoOperacion, CodigoOrden) 'OT12012
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function
    Public Function Seleccionar(ByVal codigoOperacion As String, ByVal dataRequest As DataSet) As OperacionBE
        Dim oOperacionDAM As New OperacionDAM
        Dim intCodigoEjecucion As Integer
        Dim parameters As Object() = {codigoOperacion, dataRequest}
        Dim oOperacionBE As OperacionBE
        Try
            intCodigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
            oOperacionBE = oOperacionDAM.Seleccionar(codigoOperacion)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oOperacionBE
    End Function
    Public Function SeleccionarPorCodigoTipoOperacion(ByVal codigoTipoOperacion As String, Optional ByVal Egreso As String = "",
    Optional ByVal CodigoClaseCuenta As String = "") As OperacionBE
        Try
            Dim oOperacionDAM As New OperacionDAM
            Dim dsOperacion As OperacionBE = oOperacionDAM.SeleccionarPorCodigoTipoOperacion(codigoTipoOperacion, Egreso, CodigoClaseCuenta)
            Return dsOperacion
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As OperacionBE
        Dim oOperacionDAM As New OperacionDAM
        Dim intCodigoEjecucion As Integer
        Dim parameters As Object() = {dataRequest}
        Dim oOperacionBE As OperacionBE
        Try
            intCodigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
            oOperacionBE = oOperacionDAM.Listar()
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oOperacionBE
    End Function
    Public Function ListarDatatable(ByVal dataRequest As DataSet) As DataTable
        Dim oOperacionDAM As New OperacionDAM
        Dim intCodigoEjecucion As Integer
        Dim parameters As Object() = {dataRequest}
        Dim oOperacionBE As DataTable
        Try
            intCodigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
            oOperacionBE = oOperacionDAM.ListarDataTable()
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oOperacionBE
    End Function
    Public Function Listar_ClaseInstrumento(ByVal datarequest As DataSet) As DataSet
        Dim oOperacionDAM As New OperacionDAM
        Dim intCodigoEjecucion As Integer
        Dim parameters As Object() = {datarequest}
        Dim oOperacionBE As DataSet
        Try
            intCodigoEjecucion = ObtenerCodigoEjecucion(datarequest)
            oOperacionBE = oOperacionDAM.Listar_ClaseInstrumento()
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oOperacionBE
    End Function
    Public Function Modificar(ByVal oOperacionBE As OperacionBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oOperacionBE, dataRequest}
        Try
            Dim oOperacionDAM As New OperacionDAM
            oOperacionDAM.Modificar(oOperacionBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function Eliminar(ByVal codigoOperacion As String, ByVal dataRequest As DataSet)
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoOperacion, dataRequest}
        Try
            Dim oOperacionDAM As New OperacionDAM
            oOperacionDAM.Eliminar(codigoOperacion, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarporClaseinstrumento(ByVal clase As String, ByVal situacion As String) As DataSet
        Dim objOperacion As New OperacionDAM
        Dim dsOperacion As New DataSet
        Try
            dsOperacion = objOperacion.SeleccionarporClaseinstrumento(clase, situacion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsOperacion
    End Function
    Public Function SeleccionarPorTrasladoOI() As DataSet
        Dim objOperacion As New OperacionDAM
        Dim dsOperacion As New DataSet
        Try
            dsOperacion = objOperacion.SeleccionarPorTrasladoOI()
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsOperacion
    End Function
    Public Function SeleccionarOperacionesSwap(ByVal codigooperacion As String) As DataSet
        Dim objOperacion As New OperacionDAM
        Dim dsOperacion As New DataSet
        Try
            dsOperacion = objOperacion.SeleccionarOperacionesSwap(codigooperacion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsOperacion
    End Function
    Public Function SeleccionarOperacionesFx() As DataSet
        Dim objOperacion As New OperacionDAM
        Dim dsOperacion As New DataSet
        Try
            dsOperacion = objOperacion.SeleccionarOperacionesFx()
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsOperacion
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Lista categoria por descripcion
    Public Function ListarOperacion_Categoria(Categoria As String) As DataTable
        Try
            Dim objOperacion As New OperacionDAM
            Return objOperacion.ListarOperacion_Categoria(Categoria)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarOperacionOpuesta(ByVal strOperacion As String, ByVal clase As String, ByVal situacion As String) As String
        Dim objOperacion As New OperacionDAM
        Dim strOpeOpuesta As String
        Try
            strOpeOpuesta = objOperacion.SeleccionarOperacionOpuesta(strOperacion, clase, situacion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return strOpeOpuesta
    End Function
    Public Function ListaOperacionLlamado() As DataSet
        Dim objOperacion As New OperacionDAM
        Dim dsOperacion As New DataSet
        Try
            dsOperacion = objOperacion.ListaOperacionLlamado()
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsOperacion
    End Function
    Public Function OperacionCodAuto() As String
        Dim objOperacion As New OperacionDAM
        Dim dsOperacion As String
        Try
            dsOperacion = objOperacion.OperacionCodAuto()
            Return dsOperacion
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#Region " /* Funciones Personalizadas*/"
    Public Sub Ingresar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Salir(ByVal dataRequest As DataSet)
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
#End Region
End Class