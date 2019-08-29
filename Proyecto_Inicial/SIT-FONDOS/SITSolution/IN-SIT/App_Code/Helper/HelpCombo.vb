Imports Microsoft.VisualBasic
Imports Sit.BusinessLayer
Imports System.Data
'OT 10090 - 26/07/2017 - Carlos Espejo
'Se quitaron los comentarios y se ordeno la clase
Public Class HelpCombo
    Public Shared Sub CargarMotivosCambio(ByVal p As Page)
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim dtMotivos As DataTable = oParametrosGenerales.Listar("MOTCAM", Nothing)
        LlenarComboBox(p.FindControl("ddlMotivoCambio"), dtMotivos, "Valor", "Nombre", True)
    End Sub
    Public Shared Sub PortafolioCodigoListar(ByVal ddlFondo As DropDownList, ByVal codPortafolioPadre As String)
        Dim dtPortafolios As DataTable = New PortafolioBM().PortafolioCodigoListar(codPortafolioPadre)
        LlenarComboBox(ddlFondo, dtPortafolios, "CodigoPortafolio", "Descripcion", True, "SELECCIONE")
    End Sub
    Public Shared Sub CargaPortafolioSegunUsuario(ByVal ddlFondo As DropDownList, loginUsuario As String)
        Dim dtportafolio As DataTable = New PortafolioBM().ListarPortafolioPorUsuario(loginUsuario)
        LlenarComboBox(ddlFondo, dtportafolio, "CodigoPortafolioSBS", "Descripcion", True)
    End Sub
    Public Shared Sub LlenarComboBox(ByVal DropDown As DropDownList, ByVal DataSource As DataTable, ByVal ValueField As String, ByVal TextField As String, _
    ByVal IncludeDummy As Boolean, Optional ByVal Dummy As String = "")
        DropDown.Items.Clear()
        DropDown.DataSource = DataSource
        DropDown.DataValueField = ValueField
        DropDown.DataTextField = TextField
        DropDown.SelectedValue = Nothing
        DropDown.DataBind()
        If IncludeDummy Then
            If Dummy.Trim.Equals("") Then
                DropDown.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
            Else
                DropDown.Items.Insert(0, New ListItem("--" & Dummy.Trim & "--", ""))
            End If
        End If
    End Sub
    Public Shared Sub LlenarComboBoxBusquedas(ByVal DropDown As DropDownList, ByVal DataSource As DataTable, ByVal ValueField As String, ByVal TextField As String, _
    ByVal IncludeDummy As Boolean)
        DropDown.DataSource = DataSource
        DropDown.DataValueField = ValueField
        DropDown.DataTextField = TextField
        DropDown.DataBind()
        If IncludeDummy Then
            DropDown.Items.Insert(0, New ListItem("Todos"))
        End If
    End Sub
    Public Shared Sub LlenarListBox(ByVal ListBox As ListBox, ByVal DataSource As DataTable, ByVal ValueField As String, ByVal TextField As String, _
    ByVal IncludeDummy As Boolean)
        ListBox.Items.Clear()
        ListBox.DataSource = DataSource
        ListBox.DataValueField = ValueField
        ListBox.DataTextField = TextField
        ListBox.DataBind()
        If IncludeDummy Then
            ListBox.Items.Insert(0, New ListItem("--Seleccione--", ""))
        End If
    End Sub
End Class
