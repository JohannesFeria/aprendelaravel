Option Explicit On 
Option Strict Off
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Cartas.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class PersonalDAM
    Public Sub New()
    End Sub
    Public Function SeleccionarPorCodigoInterno(ByVal codigoInterno As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Personal_SeleccionarPorCodigoInterno")
            db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
            db.AddOutParameter(dbCommand, "@p_Nombre", DbType.String, 200)
            db.ExecuteNonQuery(dbCommand)
            SeleccionarPorCodigoInterno = CType(db.GetParameterValue(dbCommand, "@p_Nombre"), String)
        End Using
    End Function
    Public Function SeleccionarMail(ByVal codigoUsuario As String) As String
        SeleccionarMail = ""
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Personal_Selecionar_Mail")
            db.AddInParameter(dbCommand, "@CodigoUsuario", DbType.String, codigoUsuario)
            Dim dt As DataTable = db.ExecuteDataSet(dbCommand).Tables(0)
            For Each dr As DataRow In dt.Rows
                SeleccionarMail = Convert.ToString(dr("email_trabajo"))
                Exit For
            Next
        End Using
    End Function
End Class