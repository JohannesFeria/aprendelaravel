
Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
Imports System.Collections.Generic

Public Class Rol_MenuBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub


    Public Function InsertarRolMenu(ByVal eRolMenu As E_Rol_Menu) As String

        Dim codigo As String = String.Empty
        Try
            Dim oRolMenu As New Rol_MenuDAM
            codigo = oRolMenu.InsertarRolMenu(eRolMenu)
        Catch ex As Exception

        End Try

        Return codigo

    End Function

    Public Function Listar(ByVal eRolMenu As E_Rol_Menu) As DataTable
        Dim dt As New DataTable
        Try
            Dim oRolMenu As New Rol_MenuDAM
            dt = oRolMenu.Listar(eRolMenu)
        Catch ex As Exception

        End Try

        Return dt
    End Function

End Class
