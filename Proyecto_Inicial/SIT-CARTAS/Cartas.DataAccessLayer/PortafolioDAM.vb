Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Cartas.BusinessEntities
Public Class PortafolioDAM
    Public Sub New()
    End Sub
    Public Function PortafolioCodigoListar(ByVal portafolio As String, Optional ByVal s_Parametro As String = "S", Optional ByVal porSerie As String = "", _
    Optional ByVal estado As String = "") As PortafolioBEList
        Try
            Dim bd As Database = DatabaseFactory.CreateDatabase()
            Dim lPortafolio As New PortafolioBEList
            Using dbcomand As DbCommand = bd.GetStoredProcCommand("Portafolio_listarCodigoPortafolio")
                bd.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.String, portafolio)
                bd.AddInParameter(dbcomand, "@p_Administra", DbType.String, s_Parametro)
                bd.AddInParameter(dbcomand, "@p_PorSerie", DbType.String, porSerie)
                bd.AddInParameter(dbcomand, "@p_Estado", DbType.String, estado)
                Using oReader As IDataReader = bd.ExecuteReader(dbcomand)
                    Dim oPortafolio As PortafolioBE
                    While oReader.Read()
                        oPortafolio = New PortafolioBE
                        oPortafolio.CodigoPortafolioSBS = oReader.Item(0)
                        oPortafolio.Descripcion = oReader.Item(1)
                        lPortafolio.Add(oPortafolio)
                    End While
                    oReader.Close()
                End Using
            End Using
            Return lPortafolio
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function FechaMaximaPortafolio() As Decimal
        Try
            Dim bd As Database = DatabaseFactory.CreateDatabase()
            Using dbcomand As DbCommand = bd.GetStoredProcCommand("sp_SIT_FechaMaximaPortafolio")
                FechaMaximaPortafolio = CType(bd.ExecuteDataSet(dbcomand).Tables(0).Rows(0)("FechaNegocio"), Decimal)
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function FechaNegocio(CodigoPortafolio As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_obt_Fecha_Negocio_Portafolio")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            db.AddOutParameter(dbCommand, "@p_FechaNegocio", DbType.Decimal, 8)
            FechaNegocio = CType(db.GetParameterValue(dbCommand, "@p_FechaNegocio"), Decimal)
        End Using
    End Function
End Class