Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmBusquedaFactorEmision
    Inherits BasePage

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                CargarGrilla()
            End If
            If Not Session("SS_DatosModal") Is Nothing Then
                Select Case _TipoModal.Value
                    Case "M"
                        tbCodigoMnemonico.Text = CType(Session("SS_DatosModal")(0), String)
                    Case "E"
                        tbCodigoEmisor.Text = CType(Session("SS_DatosModal")(0), String)
                End Select
                _TipoModal.Value = ""
                Session.Remove("SS_DatosModal")
            End If
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
            Response.Redirect("frmFactorEmision.aspx?ope=reg")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ingresar")
        End Try        
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Try
            Response.Redirect("~/frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try        
    End Sub

    Private Sub btnExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportar.Click
        Try
            GenerarFactorEmision()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Exportar")
        End Try        
    End Sub

    Private Sub btnImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportar.Click
        Try
            Response.Redirect("frmFactorEmisionImportar.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error de Importar")
        End Try        
    End Sub

    Protected Sub dgLista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgLista.PageIndexChanging
        Try
            dgLista.PageIndex = e.NewPageIndex
            dgLista.DataSource = CType(Session("dtFactor"), DataTable)
            dgLista.DataBind()
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Paginación")
        End Try        
    End Sub

    Protected Sub dgLista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLista.RowCommand
        Try
            Dim tipo As String = String.Empty
            Dim codEntidad As String = String.Empty
            Dim codMnemonico As String = String.Empty
            Dim index As Integer = 0

            If (e.CommandName = "Eliminar" Or e.CommandName = "Modificar") Then
                index = CType(e.CommandArgument.ToString(), Integer)

                tipo = CType(dgLista.Rows(index).FindControl("_TipoFactor"), HiddenField).Value.ToString()
                codMnemonico = dgLista.Rows(index).Cells(3).Text.ToString()
            End If

            Select Case e.CommandName
                Case "Eliminar"
                    Try
                        Dim oFactorBM As New FactorBM
                        Dim codigo As String = e.CommandArgument.ToString()
                        oFactorBM.Eliminar(tipo, "", codMnemonico, ParametrosSIT.GRUPO_FACTOR_EMISION, DatosRequest)
                        CargarGrilla()
                    Catch ex As Exception
                        AlertaJS(ex.ToString())
                    End Try
                Case "Modificar"
                    Response.Redirect("frmFactorEmision.aspx?ope=mod&tipo=" + tipo + "&codEnt=" + codEntidad + "&codMne=" + codMnemonico)
            End Select
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try       
    End Sub

    Protected Sub dgLista_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgLista.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ibEliminar"), ImageButton).Attributes.Add("onClick", "return confirm('¿Confirmar la eliminación del registro?')")
                If (e.Row.Cells(7).Text = "A") Then
                    e.Row.Cells(7).Text = "Activo"
                Else
                    e.Row.Cells(7).Text = "Inactivo"
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Grilla")
        End Try        
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub GenerarFactorEmision()
        Try
            Dim oFactorBM As New FactorBM
            Dim dt As DataTable, dr As DataRow, ary() As Object
            Dim sb As New System.Text.StringBuilder, sf As String
            Dim iRow As Integer, iCol As Integer
            Dim strNombreReporte As String
            Dim situacion As String
            Dim tipo As String

            strNombreReporte = "FactorEmision_" & Date.Now.ToShortDateString().Replace("/", "_") & ".txt"
            Response.AddHeader("Content-Disposition", "attachment; filename= " & strNombreReporte)
            Response.ContentType = "application/vnd.ms-text"
            Response.Charset = ""
            situacion = IIf(ddlSituacion.SelectedIndex = 0, "", ddlSituacion.SelectedValue)
            tipo = IIf(ddlTipoFactor.SelectedValue = "Todos", "", ddlTipoFactor.SelectedValue)
            'dt = oPatrimonioFideicomisoBM.SeleccionarPorFiltroExportar("", tbDescripcion.Text, situacion, DatosRequest)
            dt = oFactorBM.SeleccionarPorFiltro(tipo, tbCodigoMnemonico.Text.Trim, tbCodigoEmisor.Text.Trim, situacion, ParametrosSIT.GRUPO_FACTOR_EMISION, DatosRequest).Tables(0)

            sf = ""
            'For iCol = 0 To dt.Columns.Count - 1
            '    sf += Chr(124).ToString + dt.Columns(iCol).ToString
            'Next

            'TipoFactor
            sf += Chr(124).ToString + dt.Columns("TipoFactor").ToString
            'DescripcionTipoFactor
            sf += Chr(124).ToString + dt.Columns("DescripcionTipoFactor").ToString
            'CodigoSBS
            sf += Chr(124).ToString + dt.Columns("CodigoSBS").ToString
            'CodigoISIN
            sf += Chr(124).ToString + dt.Columns("CodigoISIN").ToString
            'CodigoMnemonico
            sf += Chr(124).ToString + dt.Columns("CodigoMnemonico").ToString
            'DescripcionMnemonico
            sf += Chr(124).ToString + dt.Columns("DescripcionMnemonico").ToString
            'Codigoentidad
            sf += Chr(124).ToString + dt.Columns("Codigoentidad").ToString
            'DescripcionEntidad
            sf += Chr(124).ToString + dt.Columns("DescripcionEntidad").ToString
            'Factor
            sf += Chr(124).ToString + "Factor"
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
                'For iCol = 0 To UBound(ary)
                '    sf += Chr(124).ToString + ary(iCol).ToString
                'Next
                'TipoFactor
                sf += Chr(124).ToString + CType(dr("TipoFactor"), String)
                'DescripcionTipoFactor
                sf += Chr(124).ToString + CType(dr("DescripcionTipoFactor"), String)
                'CodigoSBS
                Try
                    sf += Chr(124).ToString + CType(dr("CodigoSBS"), String)
                Catch ex As Exception
                    sf += Chr(124).ToString + ""
                End Try
                'CodigoISIN
                Try
                    sf += Chr(124).ToString + CType(dr("CodigoISIN"), String)
                Catch ex As Exception
                    sf += Chr(124).ToString + ""
                End Try

                'CodigoMnemonico
                Try
                    sf += Chr(124).ToString + CType(dr("CodigoMnemonico"), String)
                Catch ex As Exception
                    sf += Chr(124).ToString + ""
                End Try

                'DescripcionMnemonico
                Try
                    sf += Chr(124).ToString + CType(dr("DescripcionMnemonico"), String)
                Catch ex As Exception
                    sf += Chr(124).ToString + ""
                End Try

                'Codigoentidad
                Try
                    sf += Chr(124).ToString + CType(dr("Codigoentidad"), String)
                Catch ex As Exception
                    sf += Chr(124).ToString + ""
                End Try

                'DescripcionEntidad
                Try
                    sf += Chr(124).ToString + CType(dr("DescripcionEntidad"), String)
                Catch ex As Exception
                    sf += Chr(124).ToString + ""
                End Try

                'FloatOficioMultiple
                Try
                    sf += Chr(124).ToString + CType(dr("FloatOficioMultiple"), String)
                Catch ex As Exception
                    sf += Chr(124).ToString + ""
                End Try

                'Situacion
                Try
                    sf += Chr(124).ToString + CType(dr("Situacion"), String)
                Catch ex As Exception
                    sf += Chr(124).ToString + ""
                End Try

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

    Private Sub CargarCombos()
        Dim tablaTipoFactor As New DataTable
        Dim tablaSituacion As New DataTable

        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        tablaTipoFactor = oParametrosGenerales.ListarTipoFactor(DatosRequest)

        'RGF 20080807
        HelpCombo.LlenarComboBoxBusquedas(ddlTipoFactor, tablaTipoFactor, "Valor", "Nombre", True)
        HelpCombo.LlenarComboBoxBusquedas(ddlSituacion, tablaSituacion, "Valor", "Nombre", True)
    End Sub

    Private Sub CargarGrilla()
        Dim dt As DataTable
        Dim Tipo As String
        Dim situacion As String
        situacion = IIf(ddlSituacion.SelectedIndex = 0, "", ddlSituacion.SelectedValue)
        Tipo = IIf(ddlTipoFactor.SelectedValue = "Todos", "", ddlTipoFactor.SelectedValue)
        dt = New FactorBM().SeleccionarPorFiltro(Tipo, tbCodigoMnemonico.Text.Trim, tbCodigoEmisor.Text.Trim, situacion, ParametrosSIT.GRUPO_FACTOR_EMISION, DatosRequest).Tables(0)
        Session("dtFactor") = dt
        dgLista.DataSource = dt
        dgLista.DataBind()
        EjecutarJS("$('#" + lblContador.ClientID + "').text('" + UIUtility.MostrarResultadoBusqueda(dt) + "')")
    End Sub

    Private Sub Buscar()
        CargarGrilla()
        dgLista.PageIndex = 0
        If dgLista.Rows.Count = 0 Then
            AlertaJS(Constantes.M_STR_MENSAJE_NO_EXISTE_DATA)
        End If
    End Sub

    Private Sub LimpiarConsulta()
        tbCodigoEmisor.Text = ""
        tbCodigoMnemonico.Text = ""
        ddlSituacion.SelectedIndex = 0
        ddlTipoFactor.SelectedIndex = 0
    End Sub

#End Region

End Class
