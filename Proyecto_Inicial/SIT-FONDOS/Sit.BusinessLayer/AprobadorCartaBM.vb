Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class AprobadorCartaBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Sub InicializarAprobadorCarta(ByRef oRow As AprobadorCartaBE.AprobadorCartaRow)
        Try
            Dim oAprobadorCartaDAM As New AprobadorCartaDAM
            oAprobadorCartaDAM.InicializarAprobadorCarta(oRow)
            'Luego de terminar la ejecución de métodos(sin errores)             
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

#Region "Funciones Seleccionar"
    Public Function SeleccionarPorFiltro(ByVal codigoInterno As String, ByVal rol As String, ByVal situacion As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oAprobadorCartaDAM As New AprobadorCartaDAM
            Dim dsAux As DataSet
            dsAux = oAprobadorCartaDAM.SeleccionarPorFiltro(codigoInterno, rol, situacion, dataRequest)
            Return dsAux
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GeneraClaves(ByVal longitud As Decimal, ByVal upper As Boolean, ByVal dataRequest As DataSet) As DataSet
        Try
            Dim oAprobadorCartaDAM As New AprobadorCartaDAM
            Dim dsAux As DataSet
            dsAux = oAprobadorCartaDAM.GeneraClaves(longitud, upper)
            Return dsAux
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerRutaReporteAprobacion(ByVal codigoUsuario As String, ByVal CodigoOperacionCaja As String, ByVal indReporte As String) As String
        Try
            Dim oAprobadorCartaDAM As New AprobadorCartaDAM
            Dim rutaReporte As String = ""
            rutaReporte = oAprobadorCartaDAM.ObtenerRutaReporteAprobacion(codigoUsuario, CodigoOperacionCaja, indReporte)
            Return rutaReporte
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Funciones CRUD"
    Public Function Insertar(ByVal oAprobadorCartaBE As AprobadorCartaBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oAprobadorCartaDAM As New AprobadorCartaDAM
            bolResult = oAprobadorCartaDAM.Insertar(oAprobadorCartaBE, dataRequest)
            Return bolResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Modificar(ByVal oAprobadorCartaBE As AprobadorCartaBE, ByVal dataRequest As DataSet) As Boolean
        Dim bolResult As Boolean = False
        Try
            Dim oAprobadorCartaDAM As New AprobadorCartaDAM
            bolResult = oAprobadorCartaDAM.Modificar(oAprobadorCartaBE, dataRequest)
            Return bolResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
End Class
