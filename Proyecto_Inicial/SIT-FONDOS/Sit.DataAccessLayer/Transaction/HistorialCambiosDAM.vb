'Creado por: HDG OT 64016 20111017
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class HistorialCambiosDAM
    Public oHistorialCambiosRow As HistorialCambiosBE.HistorialCambiosRow
    Public Sub New()

    End Sub

    Public Sub InicializarHistorialCambios(ByRef oRow As HistorialCambiosBE.HistorialCambiosRow)
        oRow.Aplicativo = String.Empty
        oRow.Funcionalidad = String.Empty
        oRow.UsuarioCreacion = String.Empty
        oRow.FechaCreacion = Decimal.Zero
        oRow.HoraCreacion = String.Empty
        oRow.Host = String.Empty
        oRow.idRegistro = String.Empty
        oRow.SituacionAnterior = String.Empty
        oRow.SituacionActual = String.Empty
        oRow.Rol_RutaFirma = String.Empty
    End Sub

    Public Function Insertar(ByVal ob As HistorialCambiosBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_HistorialCambios")
        oHistorialCambiosRow = CType(ob.HistorialCambios.Rows(0), HistorialCambiosBE.HistorialCambiosRow)

        db.AddInParameter(dbCommand, "@p_Aplicativo", DbType.String, oHistorialCambiosRow.Aplicativo)
        db.AddInParameter(dbCommand, "@p_Funcionalidad", DbType.String, oHistorialCambiosRow.Funcionalidad)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_idRegistro", DbType.String, oHistorialCambiosRow.idRegistro)
        db.AddInParameter(dbCommand, "@p_SituacionAnterior", DbType.String, oHistorialCambiosRow.SituacionAnterior)
        db.AddInParameter(dbCommand, "@p_SituacionActual", DbType.String, oHistorialCambiosRow.SituacionActual)
        db.AddInParameter(dbCommand, "@p_Rol_RutaFirma", DbType.String, oHistorialCambiosRow.Rol_RutaFirma)

        db.ExecuteNonQuery(dbCommand)
    End Function

End Class
