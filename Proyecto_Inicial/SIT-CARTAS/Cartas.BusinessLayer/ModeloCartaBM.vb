Imports System.Data
Imports Cartas.DataAccessLayer
Imports Cartas.BusinessEntities
Public Class ModeloCartaBM
    Public Sub New()
    End Sub
    Public Function SeleccionarCartaEstructuraPorModelo(ByVal codigoModelo As String) As TablaGeneralBEList
        Try
            Return New ModeloCartaDAM().SeleccionarCartaEstructuraPorModelo(codigoModelo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub GrabarObservacionCarta(ByVal CodigoAgrupacion As Integer, ByVal Observacion As String, ByVal DatosRequest As DataSet)
        Dim objBL As New ModeloCartaDAM
        Try
            objBL.GrabarObservacionCarta(CodigoAgrupacion, Observacion, DatosRequest)
        Catch ex As Exception

        End Try
    End Sub

    Public Function ObtenerObservacionCarta(ByVal CodigoAgrupacion As Integer) As DataSet
        Dim objBL As New ModeloCartaDAM
        Dim ds As DataSet
        Try
            ds = objBL.ObtenerObservacionCarta(CodigoAgrupacion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return ds
    End Function
End Class