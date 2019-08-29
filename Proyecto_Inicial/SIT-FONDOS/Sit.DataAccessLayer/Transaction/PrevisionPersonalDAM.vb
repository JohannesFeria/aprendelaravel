Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class PrevisionPersonalDAM

    Public Shared Function ListarPersonal(ByVal Nombre As String, ByVal Apellido As String, ByVal CodigoInterno As String) As DataSet
        Dim db As Database
        Dim cmd As DbCommand = Nothing
        Dim ds As New DataSet()

        Try
            db = DatabaseFactory.CreateDatabase("ConexionSIT")
            cmd = db.GetStoredProcCommand("prov.SP_PROV_sel_ListarPersonal")
            db.AddInParameter(cmd, "@p_Nombre", DbType.String, Nombre)
            db.AddInParameter(cmd, "@p_Apellido", DbType.String, Apellido)
            db.AddInParameter(cmd, "@p_CodigoInterno", DbType.String, CodigoInterno)
            ds = db.ExecuteDataSet(cmd)
        Catch ex As Exception
            ds = New DataSet()
        Finally
            db = Nothing
            cmd.Dispose()
        End Try

        Return ds
    End Function

End Class
