Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Collections.Generic

Public Class MnMenuAccesoDAM
    Inherits BaseDAM

    Private sqlCommand As String = ""
    'Private oMercadoRow As MercadoBE.MercadoRow

    Public Function Listar(ByVal TokenSesion As String) As DataTable
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSeg")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Menu.pr_Sel_ListarMenuAplicativo")

        db.AddInParameter(dbCommand, "@p_TokenSesion", DbType.String, TokenSesion)

        dt = db.ExecuteDataSet(dbCommand).Tables(0)

        Return dt
    End Function

    Public Function InsertarOpcionMenu(ByVal CodOpcionMenu As String _
                                       , ByVal CodAplicativo As String _
                                       , ByVal TituloOpcionMenu As String _
                                       , ByVal Nivel As String _
                                       , ByVal Orden As String _
                                       , ByVal Url As String _
                                       , ByVal CodOpcionMenuPadre As String _
                                       , ByVal Estado As String _
                                       , ByVal Usuario As String) As String

        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSeg")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Menu.pr_Ins_OpcionMenu")

        db.AddInParameter(dbCommand, "@p_CodOpcionMenu", DbType.String, CodOpcionMenu)
        db.AddInParameter(dbCommand, "@p_CodAplicativo", DbType.String, CodAplicativo)
        db.AddInParameter(dbCommand, "@p_TituloOpcionMenu", DbType.String, TituloOpcionMenu)
        db.AddInParameter(dbCommand, "@p_Nivel", DbType.String, Nivel)
        db.AddInParameter(dbCommand, "@p_Orden", DbType.String, Orden)
        db.AddInParameter(dbCommand, "@p_Url", DbType.String, Url)
        db.AddInParameter(dbCommand, "@p_CodOpcionMenuPadre", DbType.String, CodOpcionMenuPadre)
        db.AddInParameter(dbCommand, "@p_Estado", DbType.String, Estado)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, Usuario)

        Return db.ExecuteScalar(dbCommand)
    End Function

    Public Sub InsertarRolOpcionMenu(ByVal CodRol As String, ByVal CodOpcionMenu As String, ByVal Usuario As String)
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSeg")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Menu.pr_Ins_Rol_OpcionMenu")

        db.AddInParameter(dbCommand, "@p_CodRol", DbType.Int32, CodRol)
        db.AddInParameter(dbCommand, "@p_CodOpcionMenu", DbType.Int32, CodOpcionMenu)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, Usuario)

        db.ExecuteNonQuery(dbCommand)
    End Sub

    Public Sub InsertarRolUsuario(ByVal CodRol As String, ByVal CodUsuario As String, ByVal Usuario As String)
        Dim dt As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSeg")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Menu.pr_Ins_Rol_Usuario")

        db.AddInParameter(dbCommand, "@p_CodRol", DbType.Int32, CodRol)
        db.AddInParameter(dbCommand, "@p_CodUsuario", DbType.String, CodUsuario)
        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, Usuario)

        db.ExecuteNonQuery(dbCommand)
    End Sub











    Public Function Listar(ByVal codUser As String, ByVal codRol As Integer, ByVal codAplicativo As String, ByVal dataRequest As DataSet) As List(Of MnMenuAccesoBE)
        Dim dt As New DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("menu.pr_Sel_Menu_Acceso_Aplicativo")

        db.AddInParameter(dbCommand, "@v_CodUsuario", DbType.String, codUser)
        db.AddInParameter(dbCommand, "@v_CodRol", DbType.Int32, codRol)
        db.AddInParameter(dbCommand, "@v_CodAplicativo", DbType.String, codAplicativo)

        dt = db.ExecuteDataSet(dbCommand).Tables(0)

        Return ConvertToList(Of MnMenuAccesoBE)(dt)
    End Function

    Public Function ListarMenu(ByVal codUser As String, ByVal codRol As Integer, ByVal codAplicativo As String, ByVal dataRequest As DataSet) As DataTable

        Dim dt As New DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("menu.pr_Sel_Menu_Acceso_Aplicativo")

        db.AddInParameter(dbCommand, "@v_CodUsuario", DbType.String, codUser)
        db.AddInParameter(dbCommand, "@v_CodRol", DbType.Int32, codRol)
        db.AddInParameter(dbCommand, "@v_CodAplicativo", DbType.String, codAplicativo)

        dt = db.ExecuteDataSet(dbCommand).Tables(0)

        Return dt

    End Function

    Public Function ListarBandeja(ByVal eMenuAplicativo As Menu_AccesoBE) As DataTable

        Dim dt As New DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("menu.pr_Sel_Menu_Acceso")

        db.AddInParameter(dbCommand, "@v_ESTADO", DbType.String, eMenuAplicativo.ESTADO)
        db.AddInParameter(dbCommand, "@v_NOMBRE_MENU", DbType.String, eMenuAplicativo.NOMBRE_MENU)

        dt = db.ExecuteDataSet(dbCommand).Tables(0)

        Return dt

    End Function

    Public Function ValidarUsuario(ByVal login As String, ByVal password As String) As DataTable

        Dim dt As New DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("menu.pr_Val_Usuario_Acceso")

        db.AddInParameter(dbCommand, "@p_usuario", DbType.String, login)
        db.AddInParameter(dbCommand, "@p_clave", DbType.String, password)

        dt = db.ExecuteDataSet(dbCommand).Tables(0)
        Return dt

    End Function


#Region "Metodos Nuevos de Menu Dinámico"

    Public Function ListarMenuAplicativo(ByVal codAplicativo As String, ByVal codUser As String, ByVal codRol As Integer, ByVal dataRequest As DataSet) As List(Of MnMenuAccesoBE)

        Dim dt As New DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionSIT")
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("menu.pr_ListarMenuAplicativo")

        db.AddInParameter(dbCommand, "@v_CodAplicativo", DbType.String, codAplicativo)
        db.AddInParameter(dbCommand, "@v_CodUsuario", DbType.String, codUser)
        db.AddInParameter(dbCommand, "@v_CodRol", DbType.Int32, codRol)

        dt = db.ExecuteDataSet(dbCommand).Tables(0)

        Return ConvertToList(Of MnMenuAccesoBE)(dt)

    End Function

#End Region

End Class
