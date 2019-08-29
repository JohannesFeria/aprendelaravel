Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Cartas.BusinessEntities
Public Class MercadoDAM
    Private sqlCommand As String = ""
    Public Sub New()
    End Sub
    Public Function Listar(ByVal dataRequest As DataSet, ByVal situacion As String) As MercadoBEList
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim LMercado As New MercadoBEList
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Mercado_Listar")
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
            Using oReader As IDataReader = db.ExecuteReader(dbCommand)
                Dim oMercado As MercadoBE
                While oReader.Read()
                    oMercado = New MercadoBE
                    oMercado.CodigoMercado = oReader.Item(0)
                    oMercado.Descripcion = oReader.Item(1)
                    LMercado.Add(oMercado)
                End While
                oReader.Close()
            End Using
        End Using
        Return LMercado
    End Function
End Class