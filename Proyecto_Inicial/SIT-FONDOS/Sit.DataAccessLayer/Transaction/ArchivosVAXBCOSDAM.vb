Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ArchivosVAXBCOSDAM
    Private oArchivosVAXBCOS As ArchivosVAXBCOSBE.ArchivosVAXBCOSRow
    Public Function Insertar(ByVal objArchivosVAXBCOS As ArchivosVAXBCOSBE.ArchivosVAXBCOSRow, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ArchivosVAXBCOS_Insertar")
        oArchivosVAXBCOS = CType(objArchivosVAXBCOS, ArchivosVAXBCOSBE.ArchivosVAXBCOSRow)

        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, oArchivosVAXBCOS.CodigoIndicador.ToString)
        db.AddInParameter(dbCommand, "@p_PortafolioSBS", DbType.String, oArchivosVAXBCOS.PortafolioSBS.ToString)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Convert.ToDecimal(oArchivosVAXBCOS.Fecha))
        db.AddInParameter(dbCommand, "@p_Valor", DbType.Decimal, Convert.ToDecimal(oArchivosVAXBCOS.Valor))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, "A")
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteScalar(dbCommand)
    End Function
    'Public Function SeleccionarPorPortafolioFecha(ByVal portafolioSBS As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("ArchivosVAXBCOS_SeleccionarPorPortafolioFecha")

    '    db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolioSBS)
    '    db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)

    '    Dim oArchivosVAXBCOS As New DataSet
    '    db.LoadDataSet(dbCommand, oArchivosVAXBCOS, "ArchivosVAXBCOS")
    '    Return oArchivosVAXBCOS

    'End Function
    Public Function SeleccionarPorFiltro(ByVal codigoIndicador As String, ByVal portafolioSBS As String, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ArchivosVaxBCOS_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, codigoIndicador)
        db.AddInParameter(dbCommand, "@p_PortafolioSBS", DbType.String, portafolioSBS)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)

        Dim oArchivosVAXBCOS As New DataSet
        db.LoadDataSet(dbCommand, oArchivosVAXBCOS, "ArchivosVAXBCOS")
        Return oArchivosVAXBCOS

    End Function


End Class
