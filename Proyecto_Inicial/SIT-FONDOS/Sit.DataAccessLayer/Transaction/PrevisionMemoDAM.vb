Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common

Public Class PrevisionMemoDAM

    Public Function InsertarParametriaMemo(ByVal objMemoBE As PrevisionMemo) As Integer
        Dim db As Database
        Dim cmd As DbCommand = Nothing
        Dim result As Integer = 0

        Try
            db = DatabaseFactory.CreateDatabase("ConexionSIT")
            cmd = db.GetStoredProcCommand("prov.sp_PROV_ins_InsertarParametriaMemo")

            db.AddInParameter(cmd, "@p_TipoReporte", DbType.String, objMemoBE.TipoReporte)
            db.AddInParameter(cmd, "@p_IdTipoOperacion", DbType.String, objMemoBE.IdTipoOperacion)
            db.AddInParameter(cmd, "@p_DescripcionA", DbType.String, objMemoBE.DescripcionA)
            db.AddInParameter(cmd, "@p_DescripcionDe", DbType.String, objMemoBE.DescripcionDe)
            db.AddInParameter(cmd, "@p_Referencia", DbType.String, objMemoBE.Referencia)
            db.AddInParameter(cmd, "@p_Contenido", DbType.String, objMemoBE.Contenido)
            db.AddInParameter(cmd, "@p_Despedida", DbType.String, objMemoBE.Despedida)
            db.AddInParameter(cmd, "@p_UsuarioFirma", DbType.String, objMemoBE.UsuarioFirma)
            db.AddInParameter(cmd, "@p_AreaUsuarioFirma", DbType.String, objMemoBE.AreaUsuarioFirma)
            db.AddInParameter(cmd, "@p_InicialesDocumentador", DbType.String, objMemoBE.InicialesDocumentador)
            db.ExecuteNonQuery(cmd)
            result = 1
        Catch ex As Exception
            result = 0
        Finally
            db = Nothing
            cmd.Dispose()
        End Try
        Return result
    End Function

    Public Function ListarDatosMemo(ByVal TipoReporte As String, ByVal idTipoOperacion As String) As PrevisionMemo
        Dim db As Database
        Dim cmd As DbCommand = Nothing
        Dim result As PrevisionMemo = New PrevisionMemo()
        Dim ds As DataSet

        Try
            db = DatabaseFactory.CreateDatabase("ConexionSIT")
            cmd = db.GetStoredProcCommand("prov.sp_PROV_sel_ListarParametriaMemo")

            db.AddInParameter(cmd, "@p_TipoReporte", DbType.String, TipoReporte)
            db.AddInParameter(cmd, "@p_IdTipoOperacion", DbType.String, idTipoOperacion)
            ds = db.ExecuteDataSet(cmd)

            If ds.Tables.Count > 0 Then
                Dim dr As DataRow
                dr = ds.Tables(0).Rows(0)
                With result
                    .DescripcionA = dr("DescripcionA")
                    .DescripcionDe = dr("DescripcionDe")
                    .Referencia = dr("Referencia")
                    .Contenido = dr("Contenido")
                    .Despedida = dr("Despedida")
                    .UsuarioFirma = dr("UsuarioFirma")
                    .AreaUsuarioFirma = dr("AreaUsuarioFirma")
                    .InicialesDocumentador = dr("InicialesDocumentador")
                End With
            Else
                result = Nothing
            End If
        Catch ex As Exception
            result = New PrevisionMemo()
        Finally
            db = Nothing
            cmd.Dispose()
        End Try
        Return result
    End Function


    Public Function SeleccionarTipoReportexUsuario(ByVal IdTabla As String, ByVal Usuario As String) As DataSet
        Dim db As Database
        Dim cmd As DbCommand = Nothing

        db = DatabaseFactory.CreateDatabase("ConexionSIT")
        cmd = db.GetStoredProcCommand("prov.sp_PROV_sel_TiposReportexUsuario")
        db.AddInParameter(cmd, "@p_IdTabla", DbType.String, IdTabla)
        db.AddInParameter(cmd, "@p_Usuario", DbType.String, Usuario)
        Return db.ExecuteDataSet(cmd)
    End Function

    Public Function SeleccionarTipoOperacionxUsuario(ByVal IdTabla As String, ByVal Usuario As String) As DataSet
        Dim db As Database
        Dim cmd As DbCommand = Nothing

        db = DatabaseFactory.CreateDatabase("ConexionSIT")
        cmd = db.GetStoredProcCommand("prov.sp_PROV_sel_TiposOperacionxUsuario")
        db.AddInParameter(cmd, "@p_IdTabla", DbType.String, IdTabla)
        db.AddInParameter(cmd, "@p_Usuario", DbType.String, Usuario)
        Return db.ExecuteDataSet(cmd)
    End Function

    Public Function SeleccionarReporteTipoOperacion(ByVal TipoOperacion As String, ByVal FechaPago As Decimal) As DataSet
        Dim db As Database
        Dim cmd As DbCommand = Nothing

        db = DatabaseFactory.CreateDatabase("ConexionSIT")
        cmd = db.GetStoredProcCommand("prov.sp_PROV_sel_ReporteTipoOperacion")
        db.AddInParameter(cmd, "@p_TipoOperacion", DbType.String, TipoOperacion)
        db.AddInParameter(cmd, "@p_FechaPago", DbType.Decimal, FechaPago)
        Return db.ExecuteDataSet(cmd)
    End Function

    Public Function SeleccionarReporteGeneral(ByVal FechaPago As Decimal) As DataSet
        Dim db As Database
        Dim cmd As DbCommand = Nothing

        db = DatabaseFactory.CreateDatabase("ConexionSIT")
        cmd = db.GetStoredProcCommand("prov.sp_PROV_sel_ReporteGeneral")
        db.AddInParameter(cmd, "@p_FechaPago", DbType.Decimal, FechaPago)
        Return db.ExecuteDataSet(cmd)
    End Function
End Class
