Option Explicit On 
Option Strict On
Imports System.Data
Imports Cartas.DataAccessLayer
Imports Cartas.BusinessEntities
Public Class PersonalBM
    Public Sub New()
    End Sub
    Public Function SeleccionarPorCodigoInterno(ByVal codigoInterno As String, ByVal dataRequest As DataSet) As String
        Try
            Return New PersonalDAM().SeleccionarPorCodigoInterno(codigoInterno)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarMail(ByVal codigoUsuario As String) As String
        Try
            Return New PersonalDAM().SeleccionarMail(codigoUsuario)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class