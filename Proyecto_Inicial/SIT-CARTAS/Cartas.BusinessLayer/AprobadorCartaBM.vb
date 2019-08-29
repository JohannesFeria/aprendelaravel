Imports System.Data
Imports Cartas.DataAccessLayer
Imports Cartas.BusinessEntities
Public Class AprobadorCartaBM
    Public Sub New()
    End Sub
    Public Function SeleccionarPorFiltro(ByVal codigoInterno As String, ByVal rol As String, ByVal situacion As String) As AprobadorCartaBEList
        Try
            Dim oAprobadorCartaDAM As New AprobadorCartaDAM
            Return oAprobadorCartaDAM.SeleccionarPorFiltro(codigoInterno, rol, situacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ObtenerRutaReporteAprobacion(ByVal codigoUsuario As String, ByVal CodigoOperacionCaja As String, ByVal indReporte As String) As String
        Try
            Dim oAprobadorCartaDAM As New AprobadorCartaDAM
            Return oAprobadorCartaDAM.ObtenerRutaReporteAprobacion(codigoUsuario, CodigoOperacionCaja, indReporte)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10025 21/02/2017 - Carlos Espejo
    'Descripcion: Nuevo campo para generacion de claves en un solo proceso (FechaConsulta)
    Public Function GeneraClaves(ByVal longitud As Decimal, ByVal upper As Boolean, FechaConsulta As Decimal) As TablaGeneralBEList
        Try
            Dim oAprobadorCartaDAM As New AprobadorCartaDAM
            Return oAprobadorCartaDAM.GeneraClaves(longitud, upper, FechaConsulta)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class