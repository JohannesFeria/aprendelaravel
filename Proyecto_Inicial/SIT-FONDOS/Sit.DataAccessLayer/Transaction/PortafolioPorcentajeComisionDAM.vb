Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities
Imports System.Collections.Generic

''' <summary>
''' Clase para el acceso de los datos de la tabla PortafolioPorcentajeComision.
''' </summary>
Public Class PortafolioPorcentajeComisionDAM

#Region "Constructor"
    Public Sub New()

    End Sub
#End Region

#Region "Metodos Publicos"
    Public Function Insertar(ByVal objPortafolioPorcentajeComisionBE As PortafolioPorcentajeComisionBE) As Boolean
        Dim filaAfectada As Int32 = 0
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("PortafolioPorcentajeComision_Insertar")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, objPortafolioPorcentajeComisionBE.CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_ValorMargenMinimo", DbType.Decimal, objPortafolioPorcentajeComisionBE.ValorMargenMinimo)
        db.AddInParameter(dbCommand, "@p_ValorMargenMaximo", DbType.Decimal, objPortafolioPorcentajeComisionBE.ValorMargenMaximo)
        db.AddInParameter(dbCommand, "@p_ValorPorcentajeComision", DbType.Decimal, objPortafolioPorcentajeComisionBE.ValorPorcentajeComision)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, objPortafolioPorcentajeComisionBE.UsuarioCreacion)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, objPortafolioPorcentajeComisionBE.FechaCreacion)
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, objPortafolioPorcentajeComisionBE.HoraCreacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, objPortafolioPorcentajeComisionBE.Host)
        filaAfectada = db.ExecuteScalar(dbCommand)
        If filaAfectada > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function Eliminar(ByVal objPortafolioPorcentajeComisionBE As PortafolioPorcentajeComisionBE) As Boolean
        Dim filaAfectada As Int32 = 0
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("PortafolioPorcentajeComision_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, objPortafolioPorcentajeComisionBE.CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_Secuencia", DbType.Int32, objPortafolioPorcentajeComisionBE.Secuencia)
        filaAfectada = db.ExecuteScalar(dbCommand)
        If filaAfectada > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function Listar(ByVal codigoPortafolio As String) As List(Of PortafolioPorcentajeComisionBE)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("PortafolioPorcentajeComision_Listar")
        Dim dtReader As IDataReader
        Dim listPortafolioPC As New List(Of PortafolioPorcentajeComisionBE)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        dtReader = db.ExecuteReader(dbCommand)

        While (dtReader.Read())
            Dim objPortafolioPC = New PortafolioPorcentajeComisionBE With {.CodigoPortafolio = dtReader("CodigoPortafolio"), .Secuencia = dtReader("Secuencia"), _
                                                                           .ValorMargenMaximo = dtReader("ValorMargenMaximo"), .ValorMargenMinimo = dtReader("ValorMargenMinimo"), _
                                                                           .ValorPorcentajeComision = dtReader("ValorPorcentaje") _
                                                                          }
            listPortafolioPC.Add(objPortafolioPC)
        End While

        Return listPortafolioPC
    End Function

#End Region

End Class
