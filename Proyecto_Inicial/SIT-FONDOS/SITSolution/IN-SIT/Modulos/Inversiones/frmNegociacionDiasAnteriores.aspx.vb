Imports SIT.BusinessLayer
Imports SIT.BusinessEntities
Imports System.Data
Imports System.Text
Partial Class Modulos_Inversiones_frmNegociacionDiasAnteriores
    Inherits BasePage

#Region "/* Eventos de la Pagina */"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack = True Then
            Try
                HelpCombo.LlenarComboBox(ddlOrdenInversion, ConsultarClaseInstrumento, "Categoria", "Descripcion", True, "SELECCIONE")
                ConsultarPaginasPorOI()
                Dim fechaActual As DateTime = Now.Date.AddDays(-1)
                tbFechaOperacion.Text = fechaActual.ToString("dd/MM/yyyy")
            Catch ex As Exception
                AlertaJS(ex.Message.ToString())
            End Try
        End If
    End Sub

    Protected Sub btnAceptar_Click(sender As Object, e As System.EventArgs) Handles btnAceptar.Click
        Dim objValorizacion As New CarteraTituloValoracionBM
        Dim FechaSeleccion As Decimal
        Dim ultimaFechaValorado As String
        Dim valorado As String
        If tbFechaOperacion.Text.Trim = "" Then
            AlertaJS("Debe ingresar una Fecha")
            Exit Sub
        End If
        If Me.ddlOrdenInversion.SelectedIndex = 0 Then
            AlertaJS("Debe ingresar un Instrumento")
            Exit Sub
        End If
        If ValidarFecha(Me.tbFechaOperacion.Text.Trim) Then
            FechaSeleccion = UIUtility.ConvertirFechaaDecimal(tbFechaOperacion.Text.Trim)
            Dim dtAux As DataTable = objValorizacion.UltimaValoracion(FechaSeleccion, DatosRequest).Tables(0)
            If dtAux.Rows.Count <> 0 Then
                ultimaFechaValorado = UIUtility.ConvertirFechaaString(Convert.ToDecimal(dtAux.Rows(0)("FechaFinalValoracion")))
                valorado = dtAux.Rows(0)("Valorado")
                If valorado = "SI" Then
                    AlertaJS("Fecha no puede ser menor o igual a valorización " & ultimaFechaValorado & ".")
                Else
                    Ir(ddlOrdenInversion.SelectedValue)
                End If
            End If
        End If
    End Sub

    Protected Sub btnsalir_Click(sender As Object, e As System.EventArgs) Handles btnsalir.Click
        Response.Redirect("../../frmDefault.aspx")
    End Sub

#End Region

#Region "/* Metodos Personalizados */"

    Private Function Pagina(ByVal clasificacion As String) As String
        Dim dvwPaginas As DataView = DirectCast(ViewState("PaginasOI"), DataTable).DefaultView
        dvwPaginas.RowFilter = "Clasificacion = '" + clasificacion + "'"
        Return IIf(dvwPaginas.Count > 0, dvwPaginas(0)(1).ToString, String.Empty)
    End Function
    Private Sub Ir(ByVal clasificacion As String)
        Response.Redirect(Pagina(clasificacion).Replace("#", clasificacion).Replace("%", "&").Replace("fecData", Me.tbFechaOperacion.Text))
    End Sub
    Private Function ValidarFecha(ByVal fecha As String) As Boolean
        Try
            Dim dtFecha As DateTime = Convert.ToDateTime(fecha)
            If fecha = String.Empty Or fecha = "" Then AlertaJS("Ingrese una fecha.") Else 
            If dtFecha >= Now Then AlertaJS("La fecha seleccionada no está permitida para operaciones mayores o igual al día de hoy.") Else Return True
            Return True
        Catch ex As Exception
            AlertaJS("Formato de Fecha incorrecto.")
            Return False
        End Try
    End Function
    Private Sub ConsultarPaginasPorOI()
        Try
            Dim dsPaginas As New DataSet
            dsPaginas.ReadXml(MapPath("") + "\Configuracion\TClasificacionOI.xml")
            ViewState("PaginasOI") = dsPaginas.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Function ConsultarClaseInstrumento() As DataTable
        Return New ClaseInstrumentoBM().Seleccionar(Me.DatosRequest).Tables(0)
    End Function

#End Region

End Class
