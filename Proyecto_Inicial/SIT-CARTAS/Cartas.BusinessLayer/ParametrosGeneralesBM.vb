Imports System
Imports System.Data
Imports System.Data.Common
Imports Cartas.DataAccessLayer
Imports Cartas.BusinessEntities
Public Class ParametrosGeneralesBM
    Public Sub New()
    End Sub
    Dim oParametrosGeneralesDAM As New ParametrosGeneralesDAM
    Public Function ListarRutaGeneracionCartas(ByVal datarequest As DataSet) As String
        Try
            Return oParametrosGeneralesDAM.ListarRutaGeneracionCartas()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Listar(ByVal Clasificacion As String, ByVal dataRequest As DataSet) As TablaGeneralBEList
        Try
            Return oParametrosGeneralesDAM.Listar(Clasificacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10025 21/02/2017 - Carlos Espejo
    'Descripcion: Se agrega la fecha de operacion al filtro
    Public Function ListarClaveFirmantesCartas(FechaOperacion As Decimal) As TablaGeneralBEList
        Try
            Return oParametrosGeneralesDAM.ListarClaveFirmantesCartas(FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorFiltro(ByVal clasificacion As String, ByVal nombre As String, ByVal valor As String, ByVal comentario As String) As TablaGeneralBEList
        Try
            Return oParametrosGeneralesDAM.SeleccionarPorFiltro(clasificacion, nombre, valor, comentario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class