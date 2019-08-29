Imports System
Imports System.Data
Imports Cartas.BusinessEntities
Imports Cartas.DataAccessLayer
Public Class PortafolioBM
    Public Sub New()
    End Sub
    Public Function PortafolioCodigoListar(ByVal portafolio As String, Optional ByVal s_Parametro As String = "S", Optional ByVal porSerie As String = "", Optional ByVal estado As String = "") As PortafolioBEList
        Try
            Return New PortafolioDAM().PortafolioCodigoListar(portafolio, s_Parametro, porSerie, estado)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function FechaNegocio(ByVal CodigoPortafolio As String) As Decimal
        Try
            Return New PortafolioDAM().FechaNegocio(CodigoPortafolio)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function FechaMaximaPortafolio() As Decimal
        Try
            Return New PortafolioDAM().FechaMaximaPortafolio
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class