Imports SIT.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Text
Partial Class Modulos_Parametria_frmGeneraVencimientosFuturos
    Inherits BasePage
#Region "/* Métodos de la Página */"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                ConstruirGrilla()
                CargarCombos()
                'OT 10238 - 07/04/2017 - Carlos Espejo
                'Descripcion: Se usa la rutina de fecha por fondo
                EstablecerFecha()
                'OT 10238  Fin
            End If
        Catch ex As Exception
            AlertaJS("Ocurrio un error al cargar la Página")
        End Try
    End Sub
    Protected Sub btnSalir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Salir")
        End Try
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            If ddlFondo.SelectedIndex <= 0 Then
                AlertaJS("Debe seleccionar un fondo.")
                Exit Sub
            End If
            If tbFechaVencimiento.Text.Trim.Equals("") Then
                AlertaJS("Debe ingresar una fecha.")
                Exit Sub
            End If
            dgVencimientos.PageIndex = 0
            ViewState("Indica") = 1
            CargarGrilla()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al realizar la Búsqueda")
        End Try
    End Sub
    Protected Sub dgVencimientos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgVencimientos.RowCommand
        Try
            Dim index As Int32 = 0
            If e.CommandName = "Seleccionar" Then
                index = Convert.ToInt32(e.CommandArgument)
                dgVencimientos.SelectedIndex = index
                Dim row As GridViewRow = dgVencimientos.Rows(index)
                Dim obj As New Hashtable
                obj.Add("CodigoPortafolioSBS", dgVencimientos.DataKeys(index)("Fondo").ToString.Trim)
                obj.Add("MontoOperacion", row.Cells(9).Text)
                obj.Add("CodigoTipoCupon", row.Cells(20).Text)
                obj.Add("CodigoMoneda", row.Cells(8).Text)
                obj.Add("CodigoISIN", row.Cells(10).Text)
                obj.Add("CodigoSBS", row.Cells(18).Text)
                obj.Add("CodigoTercero", row.Cells(21).Text)
                obj.Add("CodigoNemonico", row.Cells(16).Text)
                obj.Add("CodigoTipoTitulo", row.Cells(22).Text)
                obj.Add("Categoria", row.Cells(12).Text)
                obj.Add("TasaCupon", row.Cells(23).Text)
                obj.Add("CodigoOperacion", row.Cells(19).Text)
                obj.Add("FechaVencimiento", row.Cells(13).Text)
                obj.Add("CantidadOperacion", row.Cells(24).Text)
                obj.Add("FechaOperacion", row.Cells(25).Text)
                obj.Add("CodigoOrden", row.Cells(26).Text)
                ViewState("DatosOrdenTemporal") = obj
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error en la Operación de la Grilla")
        End Try
    End Sub
    Protected Sub dgVencimientos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgVencimientos.SelectedIndexChanged
        Try
            If ddlFondo.SelectedIndex > 0 Then
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ddlFondo.SelectedValue))
            Else
                tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaNegocio(ParametrosSIT.PORTAFOLIO_MULTIFONDOS))
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub
    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            Dim obj As Hashtable,ordenInversion As String,objBM As New OrdenInversionWorkFlowBM
            If ViewState("DatosOrdenTemporal") Is Nothing Then
                AlertaJS("Debe seleccionar un vencimiento.")
                Exit Sub
            End If
            obj = ViewState("DatosOrdenTemporal")
            If obj("Categoria") = "OR" Or obj("Categoria") = "DP" Then
                ordenInversion = objBM.VencimientoDPZ_OR(ddlFondo.SelectedValue, obj.Item("CodigoISIN"), UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), _
                ddlcalculovencimiento.SelectedValue, Usuario)
            ElseIf obj("Categoria") = "BO" Or obj("Categoria") = "CD" Or obj("Categoria") = "PC" Or obj("Categoria") = "FA" Then
                'OT 10238 - 07/04/2017 - Carlos Espejo
                'Descripcion: Se usa la rutina para generar vencimientos con cupones
                If validarFechaLibor(UIUtility.ConvertirFechaaDecimal(tbFechaVencimiento.Text)) Then
                    objBM.GeneraCuponera(obj.Item("CodigoOperacion"), ddlcalculovencimiento.SelectedValue, ddlFondo.SelectedValue, obj.Item("CodigoNemonico"), _
                    UIUtility.ConvertirFechaaDecimal(tbFechaVencimiento.Text), UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
                Else
                    Exit Sub
                End If
                'OT 10238 Fin
            ElseIf obj("Categoria") = "BS" Then
                objBM.GeneraVencimientosBono_Swap(ddlFondo.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaVencimiento.Text) - 1, UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text), obj.Item("CodigoOperacion"))
            End If
            AlertaJS("Se ha(n) generado la(s) Orden(es). Verifique en la confirmación.")
            CargarGrilla()
            ViewState("DatosOrdenTemporal") = Nothing
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Procesar")
        End Try
    End Sub
#End Region
#Region "/* Métodos Personalizados */"
    Public Sub CargarCombos()
        UIUtility.CargarPortafoliosOI(ddlFondo)
    End Sub
    Private Sub CargarGrilla()
        Dim fecha As Decimal = GetFecha()
        Dim oOrdenPreOrdenInversionBM As New OrdenPreOrdenInversionBM
        Dim dtblDatos As DataTable = oOrdenPreOrdenInversionBM.ListarVencimientosFuturos(DatosRequest, ddlFondo.SelectedValue, fecha, _
        UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text))
        If Not (dtblDatos Is Nothing) Then
            Me.lblCantidad.Text = String.Format("({0})", dtblDatos.Rows.Count)
        Else
            Me.lblCantidad.Text = String.Format("({0})", 0)
        End If
        dgVencimientos.DataSource = dtblDatos
        dgVencimientos.DataBind()
        dgVencimientos.SelectedIndex = -1
        'Guarda Tabla en ViewState
        ViewState("Vencimientos") = dtblDatos
    End Sub
    Private Function GetFecha() As String
        Dim arrFecha() As String
        Dim fecha As String
        Try
            Dim dato As DateTime
            dato = Convert.ToDateTime(Me.tbFechaVencimiento.Text.Trim())
            arrFecha = Me.tbFechaVencimiento.Text.Trim().Split("/")
        Catch ex As Exception
            AlertaJS("El Formato de la fecha es incorrecto.")
            Exit Function
        End Try
        fecha = arrFecha(2) + arrFecha(1) + arrFecha(0)
        Return fecha
    End Function
    Private Sub ConstruirGrilla()
        Dim campos() As String = {"Fondo", "Descripcion", "Moneda", "MontoOperacion", "ISIN", "TipoOrden"}
        Dim tipos() As String = {"System.String", "System.String", "System.String", "System.String", "System.String", "System.String"}
        Dim dtblGenerico As DataTable = UIUtility.GetStructureTablebase(campos, tipos)
        dgVencimientos.DataSource = dtblGenerico : dgVencimientos.DataBind()
    End Sub
#End Region
    'OT 10238 - 07/04/2017 - Carlos Espejo
    'Descripcion: Se usa la rutina de fecha por fondo
    Sub EstablecerFecha()
        If ddlFondo.SelectedValue <> "" Then
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlFondo.SelectedValue))
            tbFechaVencimiento.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(ddlFondo.SelectedValue))
        Else
            tbFechaOperacion.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
            tbFechaVencimiento.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaMaximaNegocio())
        End If
    End Sub
    Protected Sub ddlFondo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlFondo.SelectedIndexChanged
        EstablecerFecha()
    End Sub
    Private Function validarFechaLibor(ByVal FechaFinCupon As Decimal) As Boolean
        Dim ValidaFechaLibor() As String
        Dim oPortafolio As New PortafolioBM
        Dim Mensaje As String = String.Empty
        ValidaFechaLibor = oPortafolio.ValidarFechasLibor(FechaFinCupon).Split(",")
        If ValidaFechaLibor(0).ToString() <> String.Empty Then
            Mensaje = "Falta asignar Tasa Libor para los siguientes fechas: <br><p align=left>"
            For i As Integer = 0 To ValidaFechaLibor.Length - 1
                Mensaje = Mensaje & "-> " & ValidaFechaLibor(i) & "<br>"
            Next
            AlertaJS(Mensaje + "</p>")
            Return False
        End If
        Return True
    End Function
End Class
