Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class EventosAutomaticosDAM

    Public Sub New()
    End Sub

    Public Function Insertar(ByVal evAuto As EventosAutomaticosBE, ByVal dataRequest As DataSet) As Boolean
        Insertar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Insertar_EventoAutomatico")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, evAuto.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_NumeroOperacion", DbType.String, evAuto.NumeroOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, evAuto.CodigoOperacion)
            db.AddInParameter(dbCommand, "@p_Egreso", DbType.String, evAuto.Egreso)
            db.AddInParameter(dbCommand, "@p_FlagCorte", DbType.String, evAuto.FlagCorte)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Insertar = True
        End Using
    End Function

    'OT11008 - 05/01/2018 - Ian Pastor M.
    'Descripción: Obtiene los rescates y retenciones del sistema de operaciones
    Public Function ExisteOperacion(ByVal p_EventoAutomatico As EventosAutomaticosBE) As Boolean
        ExisteOperacion = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using DbCommand As DbCommand = db.GetStoredProcCommand("EventoAutomatico_ExisteOperacion")
            db.AddInParameter(DbCommand, "@p_CodigoPortafolioSBS", DbType.String, p_EventoAutomatico.CodigoPortafolioSBS)
            db.AddInParameter(DbCommand, "@p_NumeroOperacion", DbType.String, p_EventoAutomatico.NumeroOperacion)
            db.AddInParameter(DbCommand, "@p_FlagCorte", DbType.String, p_EventoAutomatico.FlagCorte)
            Dim existe As Integer = CType(db.ExecuteScalar(DbCommand), Integer)
            ExisteOperacion = IIf(existe > 0, True, False)
        End Using
    End Function

End Class
