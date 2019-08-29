Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaFactor
    Inherits BasePage

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            Buscar()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub

    Private Sub btnIngresar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngresar.Click
        Try
            Response.Redirect("frmFactor.aspx?ope=reg")
        Catch ex As Exception
            AlertaJS("Ocirrió un error al Ingresar")
        End Try        
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            GenerarFactorEmisor()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Exportar")
        End Try        
    End Sub

    Private Sub btnImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            Response.Redirect("frmFactorEmisorImportar.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Importar")
        End Try        
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(1).FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
                If (e.Row.Cells(5).Text = "A") Then
                    e.Row.Cells(5).Text = "Activo"
                Else
                    e.Row.Cells(5).Text = "Inactivo"
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim tipo As String
            Dim codEntidad As String
            Dim codMnemonico As String

            If (e.CommandName = "Eliminar" Or e.CommandName = "Modificar") Then
                Dim Row As GridViewRow = DirectCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                Dim Index As Integer = Row.RowIndex
                tipo = CType(Row.FindControl("hdTipoFactor"), HiddenField).Value.ToString()
                codEntidad = CType(Row.FindControl("hdCodigoEntidad"), HiddenField).Value.ToString()
            End If

            Select Case e.CommandName
                Case "Eliminar"
                    Try
                        Dim oFactorBM As New FactorBM
                        Dim codigo As String = e.CommandArgument.ToString()
                        oFactorBM.Eliminar(tipo, codEntidad, "", ParametrosSIT.GRUPO_FACTOR_EMISOR, Me.DatosRequest) 'CMB OT 61566
                        CargarGrilla()
                    Catch ex As Exception
                        AlertaJS(ex.ToString)
                    End Try
                Case "Modificar"
                    Response.Redirect("frmFactor.aspx?ope=mod&tipo=" + tipo + "&codEnt=" + codEntidad + "&codMne=" + codMnemonico)
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            Me.dgLista.DataSource = CType(Session("dtFactor"), DataTable)
            Me.dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocirrió un error en la Paginación")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub CargarPagina()
        'Me.imbEntidad.Attributes.Add("onclick", "javascript:showPopupEntidad();")
        'Me.imbMnemonico.Attributes.Add("onclick", "javascript:showPopupMnemonico();")
        If Not Page.IsPostBack Then
            Me.CargarCombos()
            Me.CargarGrilla()           
        End If
        If Not Session("SS_DatosModal") Is Nothing Then
            Dim arraySesiones As String() = New String(2) {}
            arraySesiones = DirectCast(Session("SS_DatosModal"), String())
            If hdTipoBusqueda.Value.Equals("E") Then
                Me.tbCodigoEntidad.Text = arraySesiones(0)
            ElseIf hdTipoBusqueda.Value.Equals("M") Then
                Me.tbCodigoMnemonico.Text = arraySesiones(0)
            End If
            hdTipoBusqueda.Value = ""
            Session.Remove("SS_DatosModal")
        End If
    End Sub

    Private Sub Buscar()
        Me.dgLista.PageIndex = 0
        CargarGrilla()
        'Me.lbContador.Text = Me.dgLista.Items.Count.ToString
        If Me.dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
        'Me.LimpiarConsulta()
    End Sub

    Private Sub CargarGrilla()
        Dim oGrupoEconomicoBM As New GrupoEconomicoBM
        Dim dt As DataTable
        Dim Tipo As String
        Dim situacion As String
        situacion = IIf(Me.ddlSituacion.SelectedIndex = 0, "", Me.ddlSituacion.SelectedValue)
        Tipo = IIf(Me.ddlTipoFactor.SelectedValue = "Todos", "", Me.ddlTipoFactor.SelectedValue)
        dt = New FactorBM().SeleccionarPorFiltro(Tipo, Me.tbCodigoMnemonico.Text.Trim, tbCodigoEntidad.Text.Trim, situacion, ParametrosSIT.GRUPO_FACTOR_EMISOR, DatosRequest).Tables(0)  'CMB OT 61566
        Session("dtFactor") = dt
        Me.dgLista.DataSource = dt
        Me.dgLista.DataBind()
        EjecutarJS("$('#" + lbContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
    End Sub

    Private Sub CargarCombos()
        Dim tablaTipoFactor As New DataTable
        Dim tablaSituacion As New DataTable

        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        tablaTipoFactor = oParametrosGenerales.ListarTipoFactor(DatosRequest)

        'RGF 20080807
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlTipoFactor, tablaTipoFactor, "Valor", "Nombre", True)
        'HelpCombo.LlenarComboBoxBusquedas(Me.ddlTipoFactor, tablaTipoFactor, "Nombre", "Nombre", True)
        HelpCombo.LlenarComboBoxBusquedas(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Private Sub LimpiarConsulta()
        Me.tbCodigoEntidad.Text = ""
        Me.tbCodigoMnemonico.Text = ""
        Me.ddlSituacion.SelectedIndex = 0
        Me.ddlTipoFactor.SelectedIndex = 0
    End Sub
    'CMB OT 61566
    Private Sub GenerarFactorEmisor()
        Try
            Dim oFactorBM As New FactorBM
            Dim dt As DataTable, dr As DataRow, ary() As Object
            Dim sb As New System.Text.StringBuilder, sf As String
            Dim iRow As Integer, iCol As Integer
            Dim strNombreReporte As String
            Dim situacion As String
            Dim tipo As String

            strNombreReporte = "FactorEmisor_" & Date.Now.ToShortDateString().Replace("/", "_") & ".txt"
            Response.AddHeader("Content-Disposition", "attachment; filename= " & strNombreReporte)
            Response.ContentType = "application/vnd.ms-text"
            Response.Charset = ""
            situacion = IIf(Me.ddlSituacion.SelectedIndex = 0, "", Me.ddlSituacion.SelectedValue)
            tipo = IIf(Me.ddlTipoFactor.SelectedValue = "Todos", "", Me.ddlTipoFactor.SelectedValue)
            dt = oFactorBM.SeleccionarPorFiltro(tipo, tbCodigoMnemonico.Text.Trim, tbCodigoEntidad.Text.Trim, situacion, ParametrosSIT.GRUPO_FACTOR_EMISOR, DatosRequest).Tables(0)

            sf = ""
            'TipoFactor
            sf += Chr(124).ToString + dt.Columns("TipoFactor").ToString
            'DescripcionTipoFactor
            sf += Chr(124).ToString + dt.Columns("DescripcionTipoFactor").ToString
            'Codigoentidad
            sf += Chr(124).ToString + dt.Columns("Codigoentidad").ToString
            'DescripcionEntidad
            sf += Chr(124).ToString + dt.Columns("DescripcionEntidad").ToString
            'Factor
            sf += Chr(124).ToString + dt.Columns("Factor").ToString
            'Situacion
            sf += Chr(124).ToString + dt.Columns("Situacion").ToString
            'FechaCreacion
            sf += Chr(124).ToString + dt.Columns("FechaCreacion").ToString
            'FechaVigencia
            sf += Chr(124).ToString + dt.Columns("FechaVigencia").ToString

            sb.Append(sf.Substring(1) + vbCrLf)

            For iRow = 0 To dt.Rows.Count - 1
                dr = dt.Rows.Item(iRow)
                ary = dr.ItemArray
                sf = ""
                'TipoFactor
                sf += Chr(124).ToString + CType(dr("TipoFactor"), String)
                'DescripcionTipoFactor
                sf += Chr(124).ToString + CType(dr("DescripcionTipoFactor"), String)
                'Codigoentidad
                sf += Chr(124).ToString + CType(dr("Codigoentidad"), String)
                'DescripcionEntidad
                sf += Chr(124).ToString + CType(dr("DescripcionEntidad"), String)
                'Factor
                Try
                    sf += Chr(124).ToString + CType(dr("Factor"), String)
                Catch ex As Exception
                    sf += Chr(124).ToString + ""
                End Try

                'Situacion
                sf += Chr(124).ToString + CType(dr("Situacion"), String)
                'FechaCreacion
                Try
                    sf += Chr(124).ToString + UIUtility.ConvertirFechaaString(CType(dr("FechaCreacion"), Decimal))
                Catch ex As Exception
                    sf += Chr(124).ToString + ""
                End Try

                'FechaVigencia
                Try
                    sf += Chr(124).ToString + UIUtility.ConvertirFechaaString(CType(dr("FechaVigencia"), Decimal))
                Catch ex As Exception
                    sf += Chr(124).ToString + ""
                End Try

                sb.Append(sf.Substring(1) + vbCrLf)
            Next
            Response.Write(sb.ToString)
            Response.End()
        Catch ex As Exception
            AlertaJS("Error al generar el archivo")
        End Try

    End Sub
#End Region

End Class
