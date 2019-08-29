Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class PrevisionParametriaDAM

    Public Shared Function ListarParametria(ByVal Parametro As Int32) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As Common.DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ListarParametria ")
        db.AddInParameter(dbCommand, "@Parametro", DbType.String, Parametro)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Shared Function InsertarCuentaCorriente(ByVal oCta As PrevisionCuentasCorrientes) As Int32
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Dim dbCommand As Common.DbCommand = db.GetStoredProcCommand("prov.SP_PROV_INS_InsertarCuentaCorriente")
            db.AddInParameter(dbCommand, "@IdEntidad", DbType.String, oCta.IdEntidad)
            db.AddInParameter(dbCommand, "@IdBanco", DbType.String, oCta.IdBanco)
            db.AddInParameter(dbCommand, "@IdCuentaCorriente", DbType.String, oCta.IdCuentaCorriente)
            db.AddInParameter(dbCommand, "@IdTipoCuenta", DbType.String, oCta.IdTipoCuenta)
            db.AddInParameter(dbCommand, "@IdMoneda", DbType.String, oCta.IdMoneda)
            db.AddInParameter(dbCommand, "@Situacion", DbType.String, oCta.Situacion)
            db.AddInParameter(dbCommand, "@CodUsuario", DbType.String, oCta.CodUsuario)

            If db.ExecuteDataSet(dbCommand).Tables(0).Rows(0)(0) = "NOOK" Then
                Return 3
            Else
                Return 1
            End If
            Return 1
        Catch ex As Exception
            Return 2
        End Try
    End Function

    Public Shared Function ActualizarCuentaCorriente(ByVal oCta As PrevisionCuentasCorrientes) As Int32
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Dim dbCommand As Common.DbCommand = db.GetStoredProcCommand("prov.SP_PROV_UPD_ActualizarCuentaCorriente")
            db.AddInParameter(dbCommand, "@Codigo", DbType.String, oCta.Codigo)
            db.AddInParameter(dbCommand, "@IdEntidad", DbType.String, oCta.IdEntidad)
            db.AddInParameter(dbCommand, "@IdBanco", DbType.String, oCta.IdBanco)
            db.AddInParameter(dbCommand, "@IdCuentaCorriente", DbType.String, oCta.IdCuentaCorriente)
            db.AddInParameter(dbCommand, "@IdTipoCuenta", DbType.String, oCta.IdTipoCuenta)
            db.AddInParameter(dbCommand, "@IdMoneda", DbType.String, oCta.IdMoneda)
            db.AddInParameter(dbCommand, "@Situacion", DbType.String, oCta.Situacion)
            db.AddInParameter(dbCommand, "@CodUsuario", DbType.String, oCta.CodUsuario)
            If db.ExecuteDataSet(dbCommand).Tables(0).Rows(0)(0) = "NOOK" Then
                Return 3
            Else
                Return 1
            End If
            Return 1
        Catch ex As Exception
            Return 2
        End Try
    End Function

    Public Shared Function EliminarCuentaCorriente(ByVal Codigo As Integer, ByVal IdUsuario As String) As Boolean
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
            Dim dbCommand As Common.DbCommand = db.GetStoredProcCommand("prov.SP_PROV_UPD_EliminarCuentaCorriente")
            db.AddInParameter(dbCommand, "@Codigo", DbType.String, Codigo)
            db.AddInParameter(dbCommand, "@CodUsuario", DbType.String, IdUsuario)
            db.ExecuteDataSet(dbCommand)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function ListarDetalleCuentasCorrientes(ByVal Banco As String, ByVal Estado As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As Common.DbCommand = db.GetStoredProcCommand("prov.SP_PROV_SEL_ListarCuentaCorriente")
        db.AddInParameter(dbCommand, "@Banco", DbType.String, Banco)
        db.AddInParameter(dbCommand, "@Estado", DbType.String, Estado)
        Return db.ExecuteDataSet(dbCommand)
    End Function

End Class
