Imports System
Imports System.Data
Imports Cartas.DataAccessLayer
Imports Cartas.BusinessEntities
Public Class MercadoBM
    Public Sub New()
    End Sub
#Region "Funciones Seleccionar"
    Public Function Listar(ByVal dataRequest As DataSet, Optional ByVal situacion As String = "") As MercadoBEList
        Try
            Return New MercadoDAM().Listar(dataRequest, situacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
End Class