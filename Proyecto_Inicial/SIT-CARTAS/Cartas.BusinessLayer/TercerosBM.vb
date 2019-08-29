Imports System.Data
Imports Cartas.DataAccessLayer
Imports Cartas.BusinessEntities
Public Class TercerosBM
    Public Sub New()
    End Sub
    Public Function SeleccionarPorFiltro(ByVal clasificacionTercero As String, ByVal tipoTercero As String) As TerceroBEList
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.SeleccionarPorFiltro(clasificacionTercero, tipoTercero)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarBancoPorCodigoMercadoYPortafolio(ByVal codigoMercado As String, ByVal codigoPortafolioSBS As String) As TerceroBEList
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.SeleccionarBancoPorCodigoMercadoYPortafolio(codigoMercado, codigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class