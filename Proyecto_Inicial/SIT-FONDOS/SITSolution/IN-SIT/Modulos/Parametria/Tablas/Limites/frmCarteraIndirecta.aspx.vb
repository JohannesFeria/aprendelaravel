Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Limites_frmCarteraIndirecta
    Inherits BasePage
    Dim FechaCarteraIndirecta As Decimal

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then
            Me.limpiarCampos()
            CargarCombos()
            If Not (Request.QueryString("cod") = Nothing) Then
                hd.Value = Request.QueryString("cod")
                CargarRegistro(hd.Value)
            Else
                hd.Value = ""
            End If
        End If
        If Not Session("SS_DatosModal") Is Nothing Then
            txtEmisor.Text = CType(Session("SS_DatosModal"), String())(0).ToString()
            txtEmisorDesc.Text = CType(Session("SS_DatosModal"), String())(1).ToString()

            CargarDatosEmisor()
            Session.Remove("SS_DatosModal")
        End If
    End Sub

    Private Sub limpiarCampos()
        Me.ddlPortafolio.Text = ""
        Me.ddlGrupoEconomico.Text = ""
        Me.txtEmisor.Text = ""
        Me.txtEmisorDesc.Text = ""
        Me.ddlActividadEconomica.Text = ""
        Me.ddlPais.Text = ""
        Me.ddlRating.Text = ""
        Me.txtPosicion.Text = ""
        Me.txtPatrimonio.Text = ""
        Me.txtParticipacion.Text = ""
        Me.txtFecha.Text = ""
        Me.ddlSituacion.Text = ""
    End Sub

    Private Sub CargarCombos()

        LlenarPortafolio()
        LlenarGrupoEconomico()
        LlenarPais()
        LlenarRating()
        LlenarSituacion()
        LlenarActividadEconomica()
       
    End Sub

    Private Sub LlenarPortafolio()
        Dim oPortafolio As New PortafolioBM
        HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True)
    End Sub

    Private Sub LlenarGrupoEconomico()

        Dim dtGrupoEconomico As DataTable
        Dim oGrupoEconomicoBM As New GrupoEconomicoBM
        dtGrupoEconomico = oGrupoEconomicoBM.Listar(Me.DatosRequest).GrupoEconomico
        HelpCombo.LlenarComboBox(Me.ddlGrupoEconomico, dtGrupoEconomico, "CodigoGrupoEconomico", "Descripcion", True)

    End Sub

    Private Sub LlenarRating()

        Dim oRating As New ParametrosGeneralesBM
        'Obtenemos la tabla completa Rating 
        Dim dtRating As DataTable = oRating.Listar("RATING", DatosRequest)

        Dim rowRating() As DataRow = dtRating.Select("Comentario in ('DLP_EXT','DLP_LOC','DCP_LOC','DCP_EXT','TODOS')")
        HelpCombo.LlenarComboBox(Me.ddlRating, rowRating.CopyToDataTable(), "Valor", "Nombre", True)
    End Sub

    Private Sub LlenarPais()

        Dim dtPais As DataTable
        Dim oPaisBM As New PaisBM
        dtPais = oPaisBM.Listar(DatosRequest).Pais
        HelpCombo.LlenarComboBox(Me.ddlPais, dtPais, "CodigoPais", "Descripcion", True)

    End Sub

    Private Sub LlenarSituacion()

        Dim DtTablaSituacion As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM

        DtTablaSituacion = oParametrosGeneralesBM.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, DtTablaSituacion, "Valor", "Nombre", False)

    End Sub

    Private Sub LlenarActividadEconomica()

        Dim oPgBM As New ParametrosGeneralesBM
        Dim dtActivdadEconomica As DataTable = oPgBM.Listar("SECTOR_GIGS", DatosRequest)
        Dim rowRating() As DataRow = dtActivdadEconomica.Select()
        HelpCombo.LlenarComboBox(Me.ddlActividadEconomica, rowRating.CopyToDataTable(), "Valor", "Nombre", True)

    End Sub

    Public Sub CargarRegistro(ByVal codigo As String)
        Dim oCarteraIndirectaBM As New CarteraIndirectaBM
        Dim oDS As DataSet
        Dim varDecimal As Decimal
        varDecimal = 0
        oDS = oCarteraIndirectaBM.SeleccionarPorFiltro(codigo, varDecimal, "", "", "", "", "", "", DatosRequest)
        'oDS = oObligacionTecnicaBM.SeleccionarPorFiltro(codigo, "", "", 0 DatosRequest)
        ddlPortafolio.SelectedValue = oDS.Tables(0).Rows(0)("CodigoPortafolio")
        ddlGrupoEconomico.SelectedValue = oDS.Tables(0).Rows(0)("CodigoGrupoEconomico")
        txtEmisor.Text = oDS.Tables(0).Rows(0)("CodigoEntidad")
        txtEmisorDesc.Text = oDS.Tables(0).Rows(0)("DesEmisor")
        ddlActividadEconomica.Text = oDS.Tables(0).Rows(0)("CodigoActividadEconomica")
        ddlPais.SelectedValue = oDS.Tables(0).Rows(0)("CodigoPais")
        ddlRating.SelectedValue = oDS.Tables(0).Rows(0)("Rating")
        txtPosicion.Text = Format(oDS.Tables(0).Rows(0)("Posicion"), "##,##0.0000000")
        txtParticipacion.Text = Format(oDS.Tables(0).Rows(0)("Participacion"), "##,##0.0000000")
        txtPatrimonio.Text = Format(oDS.Tables(0).Rows(0)("Patrimonio"), "##,##0.0000000")
        Try
            txtFecha.Text = UIUtility.ConvertirFechaaString(oDS.Tables(0).Rows(0)("Fecha"))
        Catch ex As Exception
            txtFecha.Text = ""
        End Try
        ddlSituacion.SelectedValue = oDS.Tables(0).Rows(0)("Situacion")

    End Sub

    Protected Sub btnRetornar_Click(sender As Object, e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("frmBusquedaCarteraIndirecta.aspx")
    End Sub

    Protected Sub btnAceptar_Click(sender As Object, e As System.EventArgs) Handles btnAceptar.Click
        Dim oCarteraIndirectaBM As New CarteraIndirectaBM
        Dim oCarteraIndirectaBE As New CarteraIndirectaBE
        Try
            oCarteraIndirectaBE = obtenerInstancia()

            If Me.hd.Value = "" Then
                If VerificarExisteCarteraIndirecta("") = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If
                oCarteraIndirectaBM.Insertar(oCarteraIndirectaBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
                limpiarCampos()
            Else
                If VerificarExisteCarteraIndirecta("E") = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If
                oCarteraIndirectaBM.Modificar(oCarteraIndirectaBE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Private Function obtenerInstancia() As CarteraIndirectaBE
        Dim oCarteraIndirectaBE As New CarteraIndirectaBE
        'Dim oRow As CarteraIndirectaBE.ObligacionTecnicaRow
        'oRow = CType(oObligacionTecnicaBE.ObligacionTecnica.NewRow(), ObligacionTecnicaBE.ObligacionTecnicaRow)

        If hd.Value <> "" Then
            oCarteraIndirectaBE.CodigoCarteraIndirecta = hd.Value
        Else
            oCarteraIndirectaBE.CodigoCarteraIndirecta = ""
        End If

        oCarteraIndirectaBE.CodigoPortafolio = Me.ddlPortafolio.SelectedValue
        oCarteraIndirectaBE.CodigoGrupoEconomico = Me.ddlGrupoEconomico.SelectedValue
        oCarteraIndirectaBE.CodigoEntidad = Me.txtEmisor.Text
        oCarteraIndirectaBE.CodigoActividadEconomica = Me.ddlActividadEconomica.SelectedValue
        oCarteraIndirectaBE.CodigoPais = Me.ddlPais.SelectedValue
        oCarteraIndirectaBE.Rating = Me.ddlRating.SelectedValue
        oCarteraIndirectaBE.Posicion = Me.txtPosicion.Text
        oCarteraIndirectaBE.Patrimonio = Me.txtPatrimonio.Text
        oCarteraIndirectaBE.Participacion = Me.txtParticipacion.Text
        oCarteraIndirectaBE.Fecha = UIUtility.ConvertirFechaaDecimal(Me.txtFecha.Text)
        oCarteraIndirectaBE.Situacion = Me.ddlSituacion.SelectedValue

        Return oCarteraIndirectaBE
    End Function

    Private Function VerificarExisteCarteraIndirecta(ByVal tipo As String) As Boolean
        Dim oCarteraIndirectaBM As New CarteraIndirectaBM
        Dim oDS As DataSet

        Me.FechaCarteraIndirecta = UIUtility.ConvertirFechaaDecimal(Me.txtFecha.Text)
        'Me.PortafolioOT = Me.ddlPortafolio.SelectedValue

        oDS = oCarteraIndirectaBM.SeleccionarPorFiltro(hd.Value, Me.FechaCarteraIndirecta, "", "", "", tipo, Me.ddlPortafolio.SelectedValue, Me.txtEmisor.Text, Me.DatosRequest)
        If oDS.Tables(0).Rows.Count > 0 Then
            VerificarExisteCarteraIndirecta = True
        Else
            VerificarExisteCarteraIndirecta = False
        End If
    End Function

    Private Sub CargarDatosEmisor()

        Dim dtCambioAyer As DataTable = ListarDatosEntidad()

        ddlGrupoEconomico.SelectedValue = dtCambioAyer.Rows(0)("CodigoGrupoEconomico")
        ddlPais.SelectedValue = dtCambioAyer.Rows(0)("CodigoPais")
        ddlActividadEconomica.SelectedValue = dtCambioAyer.Rows(0)("CodigoActividadEconomica")

    End Sub

    Private Function ListarDatosEntidad() As DataTable
        Dim oCarteraIndirectaBM As New CarteraIndirectaBM
        Dim dtResult As New DataTable
        dtResult = oCarteraIndirectaBM.Listar(txtEmisor.Text, New DataSet()).Tables(0)
        Return dtResult
    End Function
End Class
