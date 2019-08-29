Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer

Public Class TipoInstrumentoCuentaBCRDAM

    Private oRow As TipoInstrumentoCuentaBCRBE.TipoInstrumentoCuentaBCRBERow
    Public Function SeleccionarPorFiltro(ByVal CodigoInstrumento As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumentoCuentaBCR_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, CodigoInstrumento)
        Dim objeto As New DataSet
        db.LoadDataSet(dbCommand, objeto, "Terceros")
        Return objeto.Tables(0)
    End Function

    Public Function Insertar(ByVal oTipoInstrumentoCuentaBCRBE As TipoInstrumentoCuentaBCRBE) As Boolean
        For Each oRow In oTipoInstrumentoCuentaBCRBE.TipoInstrumentoCuentaBCRBE.Rows
            Insertar(oRow)
        Next
        Return True
    End Function

    Private Function Insertar(ByVal CuentaBCR As TipoInstrumentoCuentaBCRBE.TipoInstrumentoCuentaBCRBERow) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumentoCuentaBCR_Insertar")
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, CuentaBCR.CodigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, CuentaBCR.CodigoTipoInstrumentoSBS)
        db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, CuentaBCR.CuentaContable)
        'db.AddInParameter(dbCommand, "@p_NombreCuenta", DbType.String, CuentaBCR.NombreCuenta) 'RGF 20090911
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CuentaBCR.CodigoEntidad)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Modificar(ByVal oCuentaBCRBENueva As TipoInstrumentoCuentaBCRBE) As Boolean
        'Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oCuentaBCROriginal As DataTable = SeleccionarPorFiltro(oCuentaBCRBENueva.TipoInstrumentoCuentaBCRBE.Rows(0)("CodigoTipoInstrumento"))
        Dim oRowOriginal As DataRow
        Dim existe As Boolean = False
        oRow = CType(oCuentaBCRBENueva.TipoInstrumentoCuentaBCRBE.Rows(0), TipoInstrumentoCuentaBCRBE.TipoInstrumentoCuentaBCRBERow)
        For Each oRowOriginal In oCuentaBCROriginal.Rows
            existe = False
            For Each oRow In oCuentaBCRBENueva.TipoInstrumentoCuentaBCRBE.Rows
                If oRow.Item("CodigoTipoInstrumentoCuentaBCR") = oRowOriginal.Item("CodigoTipoInstrumentoCuentaBCR") Then
                    Modificar(oRow)
                    existe = True
                    Exit For
                End If
            Next
            If Not existe Then
                Eliminar(CInt(oRowOriginal.Item("CodigoTipoInstrumentoCuentaBCR")))
            End If
        Next
        For Each oRow In oCuentaBCRBENueva.TipoInstrumentoCuentaBCRBE.Rows
            If CInt(oRow.Item("CodigoTipoInstrumentoCuentaBCR")) < 0 Then
                Insertar(oRow)
            End If
        Next
        Return True
    End Function

    Private Function Modificar(ByVal CuentaBCR As TipoInstrumentoCuentaBCRBE.TipoInstrumentoCuentaBCRBERow) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumentoCuentaBCR_Modificar")
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoCuentaBCR", DbType.Int32, CuentaBCR.CodigoTipoInstrumentoCuentaBCR)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, CuentaBCR.CodigoTipoInstrumento)
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, CuentaBCR.CodigoTipoInstrumentoSBS)
        db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, CuentaBCR.CuentaContable)
        'db.AddInParameter(dbCommand, "@p_NombreCuenta", DbType.String, CuentaBCR.NombreCuenta) 'RGF 20090911
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CuentaBCR.CodigoEntidad)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Private Function Eliminar(ByVal CodigoTipoInstrumentoCuentaBCR As Integer) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoInstrumentoCuentaBCR_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoCuentaBCR", DbType.Int32, CodigoTipoInstrumentoCuentaBCR)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

End Class
