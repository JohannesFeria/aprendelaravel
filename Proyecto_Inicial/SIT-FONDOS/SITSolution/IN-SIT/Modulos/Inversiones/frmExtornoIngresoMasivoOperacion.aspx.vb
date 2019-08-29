Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Text
Partial Class Modulos_Inversiones_frmExtornoIngresoMasivoOperacion
    Inherits BasePage
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            btnAceptar.Attributes.Add("onclick", "javascript:return ConfirmarExtorno();")
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la grilla")
        End Try
    End Sub
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: El campo observacion no es obligatorio
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim strAlerta As String = ""
            If Me.ddlMotivoEliminar.SelectedValue = "" Then
                strAlerta = "-Debe de Seleccionar un Motivo de Eliminar.\n"
            End If
            If strAlerta.Length > 0 Then
                AlertaJS(strAlerta)
                Exit Sub
            End If
            Dim oPrevOrdenInversion As New PrevOrdenInversionBM
            Dim bolResult As Boolean
            bolResult = oPrevOrdenInversion.Extornar(Val(ViewState("codigoPrevOrden")), ddlMotivoEliminar.SelectedValue, txtComentarios.Text, DatosRequest)
            If bolResult Then
                AlertaJS("El extorno se realizo satisfactoriamente!")
                Select Case ViewState("tipoRenta")
                    Case ParametrosSIT.TR_RENTA_FIJA.ToString()
                        EjecutarJS("window.opener.location.href= 'frmIngresoMasivoOperacionRF.aspx'; window.close();")
                    Case ParametrosSIT.TR_RENTA_VARIABLE.ToString()
                        EjecutarJS("window.opener.location.href= 'frmIngresoMasivoOperacionRV.aspx'; window.close();")
                    Case ParametrosSIT.TR_DERIVADOS.ToString()
                        Dim ClaseIntrumento As String = Request.QueryString("codigoClaseInstrumento")
                        'OT10709. Se agregó la clase instrumento DPZ o OR para que al momento
                        'de extornar regrese a su pantalla correspondiente
                        Select Case ClaseIntrumento
                            Case "FT"
                                EjecutarJS("window.opener.location.href= 'frmIngresoMasivoOperacionFT.aspx'; window.close();")
                            Case "FX"
                                EjecutarJS("window.opener.location.href= 'frmIngresoMasivoOperacionFX.aspx'; window.close();")
                            Case Else 'DPZ or OR
                                EjecutarJS("window.opener.location.href= 'frmIngresoMasivoOperacionDP.aspx'; window.close();")
                        End Select
                End Select
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al extonar la operación")
        End Try
    End Sub
    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        EjecutarJS("window.close();")
    End Sub
    Private Sub CargarPagina()
        Dim tipoRenta As String = IIf(Request.QueryString("tipoRenta") Is Nothing, "", Request.QueryString("tipoRenta"))
        Dim codigoPrevOrden As Decimal = Val(Request.QueryString("codigo"))
        ViewState("tipoRenta") = tipoRenta
        ViewState("codigoPrevOrden") = codigoPrevOrden
        Select Case tipoRenta
            Case ParametrosSIT.TR_RENTA_FIJA.ToString()
                lbTipoRenta.Text = Replace(ParametrosSIT.TIPO_RENTA_FIJA, "_", " ")
            Case ParametrosSIT.TR_RENTA_VARIABLE.ToString()
                lbTipoRenta.Text = Replace(ParametrosSIT.TIPO_RENTA_VARIABLE, "_", " ")
            Case ParametrosSIT.TR_DERIVADOS.ToString()
                lbTipoRenta.Text = Replace(ParametrosSIT.TIPO_RENTA_DERIVADOS, "_", " ")
            Case "FX"
                lbTipoRenta.Text = Replace("FX", "_", " ")
            Case Else
                EjecutarJS("window.close();")
                Exit Sub
        End Select
        If codigoPrevOrden = 0 Then
            EjecutarJS("window.close();")
            Exit Sub
        End If
        CargarRegistro(tipoRenta, codigoPrevOrden)
        CargarCombo()
    End Sub
    Private Sub CargarRegistro(ByVal tipoRenta As String, ByVal codigoPrevOrden As Decimal)
        Dim oPrevOrdenInversion As New PrevOrdenInversionBM
        Dim ds As DataSet = oPrevOrdenInversion.SeleccionarPreExtorno(codigoPrevOrden, DatosRequest)
        Dim dtPrevOI As DataTable = ds.Tables(0)
        Dim dtOI As DataTable = ds.Tables(1)
        Dim dtAsignacion As DataTable = Nothing
        Dim correlativo As String = ""
        Select Case tipoRenta
            Case ParametrosSIT.TR_RENTA_FIJA.ToString()
                dtAsignacion = instanciarTablaAsinacion(dtAsignacion)
                For Each fila As DataRow In dtPrevOI.Rows
                    dtAsignacion.Rows.Add(fila("CodigoPortafolioSBS").ToString.Trim, fila("Descripcion").ToString.Trim, Decimal.Parse(fila("Asignacion").ToString.Trim))
                Next
            Case ParametrosSIT.TR_RENTA_VARIABLE.ToString()
                dtAsignacion = instanciarTablaAsinacion(dtAsignacion)
                For Each fila As DataRow In dtPrevOI.Rows
                    dtAsignacion.Rows.Add(fila("CodigoPortafolioSBS").ToString.Trim, fila("Descripcion").ToString.Trim, Decimal.Parse(fila("Asignacion").ToString.Trim))
                Next
            Case ParametrosSIT.TR_DERIVADOS.ToString()
                Dim claseInstrumentoFX As String = ""
                dtAsignacion = instanciarTablaAsinacion(dtAsignacion)
                For Each fila As DataRow In dtPrevOI.Rows
                    If fila("Correlativo").ToString.Trim <> correlativo Then
                        claseInstrumentoFX = fila("ClaseInstrumentoFX").ToString()
                        correlativo = fila("Correlativo").ToString.Trim
                    End If
                    dtAsignacion.Rows.Add(fila("CodigoPortafolioSBS").ToString.Trim, fila("Descripcion").ToString.Trim, Decimal.Parse(fila("Asignacion").ToString.Trim))
                Next
            Case "FX"
                Dim claseInstrumentoFX As String = ""
                dtAsignacion = instanciarTablaAsinacion(dtAsignacion)
                For Each fila As DataRow In dtPrevOI.Rows
                    If fila("Correlativo").ToString.Trim <> correlativo Then
                        claseInstrumentoFX = fila("ClaseInstrumentoFX").ToString()
                        correlativo = fila("Correlativo").ToString.Trim
                    End If
                    dtAsignacion.Rows.Add(fila("CodigoPortafolioSBS").ToString.Trim, fila("Descripcion").ToString.Trim, Decimal.Parse(fila("Asignacion").ToString.Trim))
                Next
        End Select
        CargarGrilla(dtOI, dtAsignacion)
    End Sub
    Private Function instanciarTablaAsinacion(tabla As DataTable) As DataTable
        tabla = New DataTable
        tabla.Columns.Add("CodigoPortafolioSBS")
        tabla.Columns.Add("Descripcion")
        tabla.Columns.Add("Asignacion")
        Return tabla
    End Function
    Private Sub CargarGrilla(ByVal dt As DataTable, dtAsignacion As DataTable)
        gvAsignacion.DataSource = dtAsignacion
        gvAsignacion.DataBind()
        dgLista.DataSource = dt
        dgLista.DataBind()
    End Sub
    Private Sub CargarCombo()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dtMotivos As DataTable = oParametrosGeneralesBM.Listar("MOTCAM", DatosRequest)
        HelpCombo.LlenarComboBox(ddlMotivoEliminar, dtMotivos, "valor", "nombre", True)
    End Sub
End Class
