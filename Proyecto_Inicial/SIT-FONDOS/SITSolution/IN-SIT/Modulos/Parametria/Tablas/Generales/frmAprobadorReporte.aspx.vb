Imports System.IO
Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmAprobadorReporte
    Inherits BasePage

#Region "Eventos de la Pagina"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                tbCodigoUsuario.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
                tbNombreUsuario.Text = CType(Session("SS_DatosModal"), String())(1).ToString()
                Session.Remove("SS_DatosModal")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim rutaFirmas As String
            Dim rutaImagenFirma As String
            Dim dt As New DataTable
            dt = New ParametrosGeneralesBM().Listar(ParametrosSIT.RUTA_FIRMA_CARTA, DatosRequest)
            rutaFirmas = CType(dt.Rows(0)("Comentario"), String)

            Dim oAprobadorDocumento As New AprobadorDocumentoBM
            Dim oAprobadorDocumentoBE As New AprobadorDocumentoBE
            Dim oAprobadorDocumentoRow As AprobadorDocumentoBE.AprobadorDocumentoRow

            If Not Request.QueryString("codInterno") Is Nothing Then
                'MODIFICAR
                If iptRuta.Value <> "" Then
                    If File.Exists(rutaFirmas & Path.GetFileName(iptRuta.Value)) Then
                        AlertaJS("La firma ya existe, renombre o ingrese el archivo correcto!")
                        Exit Sub
                    Else
                        rutaImagenFirma = rutaFirmas & Path.GetFileName(iptRuta.Value)
                    End If
                Else
                    rutaImagenFirma = hdFirma.Value
                End If

                oAprobadorDocumentoRow = CType(oAprobadorDocumentoBE.AprobadorDocumento.NewRow(), AprobadorDocumentoBE.AprobadorDocumentoRow)
                oAprobadorDocumento.InicializarAprobadorDocumento(oAprobadorDocumentoRow)
                oAprobadorDocumentoRow.CodigoInterno = CType(Request.QueryString("codInterno"), String)
                oAprobadorDocumentoRow.Situacion = CType(ddlSituacion.SelectedValue, String)
                oAprobadorDocumentoRow.Administrador = CType(IIf(chkAdmin.Checked, 1, 0), Decimal)
                oAprobadorDocumentoRow.Firmante = CType(IIf(chkFirma.Checked, 1, 0), Decimal)
                oAprobadorDocumentoRow.Operador = CType(IIf(chkOperador.Checked, 1, 0), Decimal)
                oAprobadorDocumentoRow.Firma = rutaImagenFirma
                oAprobadorDocumentoRow.Clave = tbClave.Text.ToString().Trim()
                oAprobadorDocumentoBE.AprobadorDocumento.AddAprobadorDocumentoRow(oAprobadorDocumentoRow)
                oAprobadorDocumentoBE.AcceptChanges()
                oAprobadorDocumento.Modificar(oAprobadorDocumentoBE, DatosRequest)
                If iptRuta.Value <> "" Then
                    Dim fInfo As New FileInfo(iptRuta.Value)
                    iptRuta.PostedFile.SaveAs(rutaFirmas & fInfo.Name)
                    'File.Copy(iptRuta.Value, rutaFirmas & Path.GetFileName(iptRuta.Value))
                End If
                AlertaJS("Los cambios se han realizado satisfactoriamente!")
            Else
                'INSERTAR
                If iptRuta.Value <> "" Then
                    If File.Exists(rutaFirmas & Path.GetFileName(iptRuta.Value)) Then
                        AlertaJS("La firma ya existe, renombre o ingrese el archivo correcto!")
                        Exit Sub
                    Else
                        oAprobadorDocumentoRow = CType(oAprobadorDocumentoBE.AprobadorDocumento.NewRow(), AprobadorDocumentoBE.AprobadorDocumentoRow)
                        oAprobadorDocumento.InicializarAprobadorDocumento(oAprobadorDocumentoRow)
                        oAprobadorDocumentoRow.CodigoInterno = tbCodigoUsuario.Text
                        oAprobadorDocumentoRow.Administrador = IIf(chkAdmin.Checked, 1, 0)
                        oAprobadorDocumentoRow.Firmante = IIf(chkFirma.Checked, 1, 0)
                        oAprobadorDocumentoRow.Operador = IIf(chkOperador.Checked, 1, 0)
                        oAprobadorDocumentoRow.Situacion = ddlSituacion.SelectedValue
                        oAprobadorDocumentoRow.Firma = rutaFirmas & Path.GetFileName(iptRuta.Value)
                        oAprobadorDocumentoRow.Clave = tbClave.Text.ToString().Trim()
                        oAprobadorDocumentoBE.AprobadorDocumento.Rows.Add(oAprobadorDocumentoRow)
                        oAprobadorDocumentoBE.AprobadorDocumento.AcceptChanges()
                        oAprobadorDocumento.Insertar(oAprobadorDocumentoBE, DatosRequest)

                        Dim fInfo As New FileInfo(iptRuta.Value)
                        iptRuta.PostedFile.SaveAs(rutaFirmas & fInfo.Name)

                        AlertaJS("Se ha registrado satisfactoriamente!")
                        LimpiarCampos()
                    End If
                Else
                    AlertaJS("Debe seleccionar el archivo! ")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaAprobadorReporte.aspx?p_codInterno=" & Request.QueryString("p_codInterno") & "&p_situacion=" & Request.QueryString("p_situacion"))
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub
#End Region

#Region "Metodos Personalizados"
    Private Sub CargarPagina()
        'trPrevFirma.Visible = False
        CargarSituacion()
        If Not Request.QueryString("codInterno") Is Nothing Then
            CargarRegistro(Request.QueryString("codInterno"))
        Else
            lkbBuscarUsuario.Enabled = True
            lkbBuscarUsuario.Attributes.Add("onclick", "javascript:return showModal();")
        End If
        If Not Session("SS_DatosModal") Is Nothing Then
            Dim arraySesiones As String() = New String(5) {}
            arraySesiones = DirectCast(Session("SS_DatosModal"), String())

            Session.Remove("SS_DatosModal")
        End If

    End Sub

    Private Sub CargarSituacion()
        Dim dt As New DataTable
        dt = New ParametrosGeneralesBM().Listar(ParametrosSIT.SITUACION, DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, dt, "Valor", "Nombre", False)
        ddlSituacion.SelectedValue = ParametrosSIT.ESTADO_ACTIVO
    End Sub

    Private Sub CargarRegistro(ByVal codigoInterno As String)
        Dim dt As New DataTable
        Dim oAprobadorDocumento As New AprobadorDocumentoBM
        Dim oAprobadorDocumentoBE As New AprobadorDocumentoBE
        Dim oAprobadorDocumentoRow As AprobadorDocumentoBE.AprobadorDocumentoRow
        oAprobadorDocumentoRow = CType(oAprobadorDocumentoBE.AprobadorDocumento.NewRow(), AprobadorDocumentoBE.AprobadorDocumentoRow)
        oAprobadorDocumento.InicializarAprobadorDocumento(oAprobadorDocumentoRow)
        oAprobadorDocumentoRow.CodigoInterno = codigoInterno
        oAprobadorDocumentoBE.AprobadorDocumento.AddAprobadorDocumentoRow(oAprobadorDocumentoRow)
        oAprobadorDocumentoBE.AcceptChanges()
        dt = oAprobadorDocumento.SeleccionarPorFiltro(oAprobadorDocumentoBE, DatosRequest).Tables(0)
        'trPrevFirma.Visible = True
        For Each fila As DataRow In dt.Rows
            tbCodigoUsuario.Text = CType(fila("CodigoInterno"), String)
            tbNombreUsuario.Text = CType(fila("Nombre"), String)
            ddlSituacion.SelectedValue = CType(fila("Situacion"), String)
            chkAdmin.Checked = CType(fila("Administrador"), Boolean)
            chkFirma.Checked = CType(fila("Firmante"), Boolean)
            chkOperador.Checked = CType(fila("Operador"), Boolean)
            Dim rutimg As String() = CType(fila("Firma"), String).Split("\")
            Dim img As String = rutimg(rutimg.Length - 1)
            imgFirma.ImageUrl = "~/Imagenes/" + img
            hdFirma.Value = CType(fila("Firma"), String)
            tbClave.Text = CType(fila("Clave"), String)
        Next
    End Sub

    Private Sub HabilitarControles(ByVal habilita As Boolean)
        tbCodigoUsuario.Enabled = habilita
        lkbBuscarUsuario.Enabled = habilita
    End Sub

    Private Sub LimpiarCampos()
        tbCodigoUsuario.Text = ""
        tbNombreUsuario.Text = ""
        ddlSituacion.SelectedIndex = 0
        chkAdmin.Checked = False
        chkFirma.Checked = False
        chkOperador.Checked = False
    End Sub

#End Region
End Class
