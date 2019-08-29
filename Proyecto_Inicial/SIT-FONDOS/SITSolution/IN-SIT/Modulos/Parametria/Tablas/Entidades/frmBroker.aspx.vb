Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Entidades_frmBroker
    Inherits BasePage

#Region "/* Métodos de la página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                hdEstadoBusqueda.Value = 0
                CargarSituacion()
                CargarTipoTramo()
                CargarTipoCosto()
                If Not Request.QueryString("Tramo") Is Nothing Then
                    CargarBroker(Request.QueryString("Tramo"))
                    txtTramo.Enabled = False
                Else
                    lkbBuscar.Enabled = True
                End If
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Dim arraySesiones As String() = New String(2) {}
                arraySesiones = DirectCast(Session("SS_DatosModal"), String())
                hdCodigoEntidad.Value = arraySesiones(0)
                hdDescripcion.Value = arraySesiones(1)
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try

    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oBrokerBE As EntidadBE = crearObjeto()
            Dim oBrokerBM As New EntidadBM
            If Not Request.QueryString("Tramo") Is Nothing Then
                oBrokerBM.ModificarTramoBroker(oBrokerBE, Me.DatosRequest)
                AlertaJS("Datos Modificados Correctamente")
            Else
                If oBrokerBM.ExisteTramo(Me.txtTramo.Text) Then
                    AlertaJS("El Nombre del Tramo ya existe")
                Else
                    oBrokerBM.InsertarTramoBroker(oBrokerBE, Me.DatosRequest)
                    AlertaJS("Datos Registrados Correctamente")
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

    Private Sub lkbBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lkbBuscar.Click
        Try
            If hdEstadoBusqueda.Value = 0 Then
                ShowDialogPopupValores()
                hdEstadoBusqueda.Value = 1
            ElseIf hdEstadoBusqueda.Value = 1 Then
                txtCodigoEntidad.Text = Me.hdCodigoEntidad.Value
                txtDescripcion.Text = Me.hdDescripcion.Value
                hdEstadoBusqueda.Value = 0
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar Popup")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("frmBusquedaBroker.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

#End Region

#Region "/* Métodos personalizados */"

    Private Sub CargarBroker(ByVal tramo As String)
        Dim obm As New EntidadBM
        Dim dt As DataTable = obm.SeleccionarTramoBroker(tramo)

        txtCodigoEntidad.Text = dt.Rows(0)("CodigoEntidad")
        txtDescripcion.Text = dt.Rows(0)("Descripcion")
        ddlSituacion.SelectedValue = dt.Rows(0)("Situacion")
        ddltipoTramo.SelectedValue = dt.Rows(0)("TipoTramo")
        If dt.Rows(0)("TipoTramo") = "AGENCIA" And dt.Rows(0)("TipoCosto") = "V" Then
            txtBandaInferior.Text = Format(dt.Rows(0)("BandaInferior"), "##,##0")
            If (dt.Rows(0)("BandaSuperior") <> "0") Then
                txtBandaSuperior.Text = Format(dt.Rows(0)("BandaSuperior"), "##,##0")
            End If
        End If
        txtTramo.Text = dt.Rows(0)("Tramo")
        ddlTipoCosto.SelectedValue = dt.Rows(0)("TipoCosto")
        txtCosto.Text = dt.Rows(0)("Costo")
    End Sub

    Private Sub CargarSituacion()
        Dim obm As New ParametrosGeneralesBM
        ddlSituacion.DataSource = obm.Listar("Situación", Me.DatosRequest)
        ddlSituacion.DataValueField = "Valor"
        ddlSituacion.DataTextField = "Nombre"
        ddlSituacion.DataBind()
    End Sub

    Private Sub CargarTipoTramo()
        Dim obm As New ParametrosGeneralesBM
        ddltipoTramo.DataSource = obm.Listar("TIPOTRAMO", Me.DatosRequest)
        ddltipoTramo.DataValueField = "Valor"
        ddltipoTramo.DataTextField = "Nombre"
        ddltipoTramo.DataBind()
        ddltipoTramo.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub

    Private Sub CargarTipoCosto()
        Dim obm As New ParametrosGeneralesBM

        ddlTipoCosto.DataSource = obm.ListarBaseImp(DatosRequest)
        ddlTipoCosto.DataValueField = "Valor"
        ddlTipoCosto.DataTextField = "Nombre"
        ddlTipoCosto.DataBind()
        ddlTipoCosto.Items.Insert(0, New ListItem("--SELECCIONE--", ""))
    End Sub

    Private Function crearObjeto() As EntidadBE
        Dim oBrokerBE As New EntidadBE
        Dim oRow As EntidadBE.EntidadRow
        oRow = oBrokerBE.Entidad.NewEntidadRow()
        oRow.CodigoEntidad = txtCodigoEntidad.Text.ToUpper
        oRow.Situacion = ddlSituacion.SelectedValue
        oRow.TipoTramo = ddltipoTramo.SelectedValue
        oRow.Tramo = txtTramo.Text.ToUpper
        If (txtBandaInferior.Text.Length = 0) Then
            oRow.BandaInferior = 0
        Else
            oRow.BandaInferior = CDec(txtBandaInferior.Text)
        End If
        If (txtBandaSuperior.Text.Length = 0) Then
            oRow.BandaSuperior = 0
        Else
            oRow.BandaSuperior = CDec(txtBandaSuperior.Text)
        End If
        If (txtCosto.Text.Length = 0) Then
            oRow.Costo = 0
        Else
            oRow.Costo = CDec(txtCosto.Text)
        End If

        oRow.TipoCosto = ddlTipoCosto.SelectedValue
        oBrokerBE.Entidad.AddEntidadRow(oRow)
        oBrokerBE.Entidad.AcceptChanges()
        Return oBrokerBE
    End Function

    Private Sub ShowDialogPopupValores()

        Dim strurl As String = "../../frmHelpControlParametria.aspx?tlbBusqueda=Broker"
        EjecutarJS("showModalDialog('" & strurl & "', '800', '600', '" & lkbBuscar.ClientID & "');")

        'Dim script As New StringBuilder
        'With script
        '    .Append("window.showModalDialog('../../frmHelpControlParametria.aspx?tlbBusqueda=Broker', '', 'dialogHeight:530px;dialogWidth:769px;status:no;unadorned:yes;help:No;resizable:no');")
        '    .Append("document.getElementById('lkbBuscar').click();")
        'End With
        'EjecutarJS(script.ToString())
    End Sub

#End Region

End Class
