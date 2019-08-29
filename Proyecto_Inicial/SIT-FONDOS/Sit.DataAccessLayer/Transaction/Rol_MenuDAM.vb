Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Collections.Generic

Public Class Rol_MenuDAM
    Inherits BaseDAM

    Private sqlCommand As String = ""
    'Private oMercadoRow As MercadoBE.MercadoRow

    Public Function InsertarRolMenu(ByVal eRolMenu As E_Rol_Menu) As Integer

        Dim dt As New DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("menu.pr_Ins_Rol_Menu")

        db.AddInParameter(dbCommand, "@CODIGO_ROL", DbType.Int32, eRolMenu.CODIGO_ROL)
        db.AddInParameter(dbCommand, "@CODIGO_MENU", DbType.Int32, eRolMenu.CODIGO_MENU)

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function Listar(ByVal eRolMenu As E_Rol_Menu) As DataTable

        Dim dt As New DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("menu.pr_Sel_Rol_Menu")

        db.AddInParameter(dbCommand, "@CODIGO_ROL", DbType.String, eRolMenu.CODIGO_ROL)

        dt = db.ExecuteDataSet(dbCommand).Tables(0)

        Return dt

    End Function

End Class
