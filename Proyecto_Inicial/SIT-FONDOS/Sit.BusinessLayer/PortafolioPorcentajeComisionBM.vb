Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports System.Collections.Generic

Public Class PortafolioPorcentajeComisionBM

#Region "Constructor"
    Public Sub New()

    End Sub
#End Region

#Region "Metodos Publicos"

    Public Function Insertar(ByVal objPortafolioPorcentajeComisionBE As PortafolioPorcentajeComisionBE) As Boolean
        Dim objPortafolioPC As PortafolioPorcentajeComisionDAM
        Dim resultado As Boolean = False
        Try
            objPortafolioPC = New PortafolioPorcentajeComisionDAM()
            resultado = objPortafolioPC.Insertar(objPortafolioPorcentajeComisionBE)
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function
    Public Function Eliminar(ByVal objPortafolioPorcentajeComisionBE As PortafolioPorcentajeComisionBE) As Boolean
        Dim objPortafolioPC As PortafolioPorcentajeComisionDAM
        Dim resultado As Boolean = False
        Try
            objPortafolioPC = New PortafolioPorcentajeComisionDAM()
            resultado = objPortafolioPC.Eliminar(objPortafolioPorcentajeComisionBE)
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function
    Public Function Listar(ByVal codigoPortafolio As String) As List(Of PortafolioPorcentajeComisionBE)
        Dim objPortafolioPC As PortafolioPorcentajeComisionDAM
        Try
            objPortafolioPC = New PortafolioPorcentajeComisionDAM()
            Return objPortafolioPC.Listar(codigoPortafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

End Class
