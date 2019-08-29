Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class CargoFirmanteDAM

    Private sqlCommand As String = ""
    Dim DECIMAL_NULO As Decimal = -1
    Public oCargoFirmanteRow As CargoFirmanteBE.CargoFirmanteRow

    Public Sub New()

    End Sub

    Public Sub InicializarCargoFirmante(ByRef oRow As CargoFirmanteBE.CargoFirmanteRow)
        oRow.CodCargoFirmante = DECIMAL_NULO
        oRow.CodigoRol = String.Empty
        oRow.Nombre = String.Empty
        oRow.Etiqueta = String.Empty
        oRow.CodReporte = String.Empty
        oRow.CodCategReporte = String.Empty
        oRow.Situacion = String.Empty
    End Sub

#Region "Metodos Transaccionales"

#End Region

#Region "Metodos No Transaccionales"
    Public Function SeleccionarPorFiltro(ByVal oCargoFirmanteBE As CargoFirmanteBE, ByVal flagFirmante As Boolean) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        oCargoFirmanteRow = CType(oCargoFirmanteBE.CargoFirmante.Rows(0), CargoFirmanteBE.CargoFirmanteRow)
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_CargoFirmante_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_codCargoFirmante", DbType.Decimal, oCargoFirmanteRow.CodCargoFirmante)
        db.AddInParameter(dbCommand, "@p_codCategReporte", DbType.String, oCargoFirmanteRow.CodCategReporte)
        db.AddInParameter(dbCommand, "@p_codReporte", DbType.String, oCargoFirmanteRow.CodReporte)
        db.AddInParameter(dbCommand, "@p_codigoRol", DbType.String, oCargoFirmanteRow.CodigoRol)
        db.AddInParameter(dbCommand, "@p_etiqueta", DbType.String, oCargoFirmanteRow.Etiqueta)
        db.AddInParameter(dbCommand, "@p_situacion", DbType.String, oCargoFirmanteRow.Situacion)
        db.AddInParameter(dbCommand, "@p_flagFirmante", DbType.Decimal, flagFirmante)
        Return db.ExecuteDataSet(dbCommand)
    End Function
#End Region

End Class
