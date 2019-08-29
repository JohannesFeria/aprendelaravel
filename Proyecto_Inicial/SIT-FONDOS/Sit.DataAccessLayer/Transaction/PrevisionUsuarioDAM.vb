Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Sit.BusinessEntities
Imports System.Data.Common

Public Class PrevisionUsuarioDAM

    Public Function ListarUsuario(ByVal nombre As String, ByVal estado As String) As DataSet
        Dim db As Database
        Dim cmd As DbCommand = Nothing
        Dim ds As New DataSet

        Try
            db = DatabaseFactory.CreateDatabase("ConexionSIT")
            cmd = db.GetStoredProcCommand("prov.SP_PROV_sel_ListarUsuario")

            db.AddInParameter(cmd, "@p_Nombre", DbType.String, nombre)
            db.AddInParameter(cmd, "@p_Situacion", DbType.String, estado)
            ds = db.ExecuteDataSet(cmd)
        Catch ex As Exception
            ds = New DataSet
        Finally
            cmd.Dispose()
            db = Nothing
        End Try

        Return ds
    End Function

    Function EliminarUsuario(ByVal codigoUsuario As String) As Integer
        Dim db As Database
        Dim cmd As DbCommand = Nothing
        Dim result As Integer

        Try
            db = DatabaseFactory.CreateDatabase("ConexionSIT")
            cmd = db.GetStoredProcCommand("prov.SP_PROV_del_EliminarUsuario")

            db.AddInParameter(cmd, "@p_CodigoUsuario", DbType.String, codigoUsuario)
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

    Function ListarOperacionesUsuario(ByVal codigoUsuario As String) As DataSet
        Dim db As Database
        Dim cmd As DbCommand = Nothing
        Dim ds As New DataSet

        Try
            db = DatabaseFactory.CreateDatabase("ConexionSIT")
            cmd = db.GetStoredProcCommand("prov.SP_PROV_sel_ListarUsuarioporCodigo")

            db.AddInParameter(cmd, "@p_CodUsuario", DbType.String, codigoUsuario)
            ds = db.ExecuteDataSet(cmd)
        Catch ex As Exception
            ds = New DataSet()
        Finally
            cmd.Dispose()
            db = Nothing
        End Try
        Return ds
    End Function

    Sub LimpiarUsuarioDetalle(ByVal objUsuarioBE As BusinessEntities.PrevisionUsuario)
        Dim db As Database
        Dim cmd As DbCommand = Nothing
        Try
            db = DatabaseFactory.CreateDatabase("ConexionSIT")
            cmd = db.GetStoredProcCommand("prov.SP_PROV_del_DetalleUsuarioporCodigo")

            db.AddInParameter(cmd, "@p_CodUsuario", DbType.String, objUsuarioBE.CodUsuario)
            db.ExecuteNonQuery(cmd)
        Catch ex As Exception

        Finally
            cmd.Dispose()
            db = Nothing
        End Try
    End Sub

    Sub RegistrarUsuarioDetalle(ByVal objDetalleBE As PrevisionDetalleUsuario)
        Dim db As Database
        Dim cmd As DbCommand = Nothing
        Try
            db = DatabaseFactory.CreateDatabase("ConexionSIT")
            cmd = db.GetStoredProcCommand("prov.SP_PROV_ins_RegistrarDetalleUsuario")

            db.AddInParameter(cmd, "@p_CodUsuario", DbType.String, objDetalleBE.CodUsuario)
            db.AddInParameter(cmd, "@p_IdTipoOperacion", DbType.String, objDetalleBE.IdTipoOperacion)
            db.AddInParameter(cmd, "@p_Situacion", DbType.String, objDetalleBE.Situacion)
            db.AddInParameter(cmd, "@p_UsuarioCreacion", DbType.String, objDetalleBE.UsuarioCreacion)
            db.ExecuteNonQuery(cmd)
        Catch ex As Exception

        Finally
            cmd.Dispose()
            db = Nothing
        End Try
    End Sub

    Function ActualizarUsuario(ByVal objUsuarioBE As PrevisionUsuario) As Integer
        Dim db As Database
        Dim cmd As DbCommand = Nothing
        Dim result As Integer

        Try
            db = DatabaseFactory.CreateDatabase("ConexionSIT")
            cmd = db.GetStoredProcCommand("prov.SP_PROV_upd_ActualizarUsuarioporCodigo")

            db.AddInParameter(cmd, "@p_CodUsuario", DbType.String, objUsuarioBE.CodUsuario)
            db.AddInParameter(cmd, "@p_Area", DbType.String, objUsuarioBE.Area)
            db.AddInParameter(cmd, "@p_Situacion", DbType.String, objUsuarioBE.Situacion)
            db.ExecuteNonQuery(cmd)
            result = 1
        Catch ex As Exception
            result = 0
        Finally
            cmd.Dispose()
            db = Nothing
        End Try
        Return result
    End Function

    Function RegistrarUsuario(ByVal objUsuarioBE As BusinessEntities.PrevisionUsuario) As Integer
        Dim db As Database
        Dim cmd As DbCommand = Nothing
        Dim result As Integer

        Try
            db = DatabaseFactory.CreateDatabase("ConexionSIT")
            cmd = db.GetStoredProcCommand("prov.SP_PROV_ins_InsertarUsuarioporCodigo")

            db.AddInParameter(cmd, "@p_CodUsuario", DbType.String, objUsuarioBE.CodUsuario)
            db.AddInParameter(cmd, "@p_NombreUsuario", DbType.String, objUsuarioBE.NombreUsuario)
            db.AddInParameter(cmd, "@p_Area", DbType.String, objUsuarioBE.Area)
            db.AddInParameter(cmd, "@p_Situacion", DbType.String, objUsuarioBE.Situacion)
            db.AddInParameter(cmd, "@p_UsuarioCreacion", DbType.String, objUsuarioBE.UsuarioCreacion)
            db.ExecuteNonQuery(cmd)
            result = 1
        Catch ex As Exception
            result = 0
        Finally
            cmd.Dispose()
            db = Nothing
        End Try
        Return result
    End Function

End Class
