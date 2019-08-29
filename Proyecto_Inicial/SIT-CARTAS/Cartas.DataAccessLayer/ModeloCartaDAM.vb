Imports System.Data
Imports System.Data.Common
Imports Cartas.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ModeloCartaDAM
    Public Sub New()
    End Sub
    Public Function SeleccionarCartaEstructuraPorModelo(ByVal codigoModelo As String) As TablaGeneralBEList
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim lTablaGeneral As New TablaGeneralBEList
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ModeloCartaEstructura_SeleccionarPorModelo")
            db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)
            Using oReader As IDataReader = db.ExecuteReader(dbCommand)
                Dim oTablaGeneral As TablaGeneralBE
                While oReader.Read()
                    oTablaGeneral = New TablaGeneralBE
                    oTablaGeneral.Codigo = oReader.Item(0)
                    oTablaGeneral.Valor = oReader.Item(1)
                    lTablaGeneral.Add(oTablaGeneral)
                End While
                oReader.Close()
            End Using
        End Using
        Return lTablaGeneral
    End Function

    Public Sub GrabarObservacionCarta(ByVal CodigoAgrupacion As Integer, ByVal Observacion As String, ByVal DatosRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_GrabarObservacionCarta")
        db.AddInParameter(dbCommand, "@p_CodigoAgrupacion", DbType.Int32, CodigoAgrupacion)
        db.AddInParameter(dbCommand, "@p_Observacion", DbType.String, Observacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(DatosRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(DatosRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(DatosRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(DatosRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Sub

    Public Function ObtenerObservacionCarta(ByVal CodigoAgrupacion As Integer) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_ObtenerObservacionCarta")
        db.AddInParameter(dbCommand, "@p_CodigoAgrupacion", DbType.Int32, CodigoAgrupacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function
End Class