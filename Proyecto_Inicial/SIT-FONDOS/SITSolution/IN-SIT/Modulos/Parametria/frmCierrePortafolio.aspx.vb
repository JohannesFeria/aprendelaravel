Imports Sit.BusinessLayer
Imports ParametrosSIT
Imports System.Data
Partial Class Modulos_Parametria_frmCierrePortafolio
    Inherits BasePage
#Region "/* Métodos de la página */"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPortafolio()
                divBtnEjecutar.Style("Display") = "None"
                lblFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(dlPortafolio.SelectedValue))
                ObtenerFechaActual(lblFecha.Text)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("../../frmDefault.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir de la página")
        End Try
    End Sub
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            EjecutarJS("MostrarConfirmacion('" & VerificarFecha(Me.tbFechaInicio.Text.Trim(), VALIDAFERIADO) & "');")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al grabar los datos")
        End Try
    End Sub
    Private Sub btnEjecutar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEjecutar.Click
        Try
            Dim oPortafolio As New PortafolioBM
            If chkReproceso.Checked Then
                oPortafolio.CerrarReproceso(dlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), DatosRequest)
                AlertaJS("Se realizó la operación satisfactoriamente")
            Else
                'Valida que la fecha de termino sea menor a la fecha de constitucion.
                'If Not UIUtility.ValidarPortafolioAperturado(dlPortafolio.SelectedValue) Then
                '    AlertaJS(ObtenerMensaje("ALERT14"))
                '    Exit Sub
                'End If
                'Valida que no existan ordenes son confirmar
                If Not New PortafolioBM().ValidarCierre(dlPortafolio.SelectedValue) Then
                    AlertaJS(ObtenerMensaje("ALERT67"), "MostrarPopup();")
                    Exit Sub
                End If
                'Crea y actualiza SaldosCarteraTitulo, que es un consolidado de saldos custodio
                If Not GeneraSaldosCarteraTitulo() Then
                    AlertaJS(ObtenerMensaje("ALERT156"))
                    Exit Sub
                End If
                'Actualiza la fecha de constitucion y la fecha de termino del portafolio
                oPortafolio.Cerrar(dlPortafolio.SelectedValue, UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text), DatosRequest)
                Dim dFechaApertura As Decimal = UIUtility.ObtenerFechaApertura(dlPortafolio.SelectedValue)
                Dim sHoraApertura As String = UIUtility.ObtenerDatosPortafolio(dlPortafolio.SelectedValue)
                Dim oPrevOrdenInversionBM As New PrevOrdenInversionBM
                oPrevOrdenInversionBM.TruncarProcesoMasivo()
                AlertaJS("Se ejecuto correctamente el proceso.")
                lblFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(dlPortafolio.SelectedValue))
                ObtenerFechaActual(lblFecha.Text)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Ejecutar")
        End Try
    End Sub
    Private Sub dlPortafolio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dlPortafolio.SelectedIndexChanged
        Try
            lblFecha.Text = UIUtility.ConvertirFechaaString(UIUtility.ObtenerFechaApertura(dlPortafolio.SelectedValue))
            ObtenerFechaActual(lblFecha.Text)
            chkReproceso.Checked = False
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub
    Private Sub chkReproceso_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkReproceso.CheckedChanged
        Try
            If (chkReproceso.Checked And Me.tbFechaInicio.Text <> "") Then
                ObtenerFechaAAnterior(lblFecha.Text)
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Seleccionar")
        End Try
    End Sub
#End Region
#Region "/* Métodos personalizados */"
    Private Function GeneraSaldosCarteraTitulo() As Boolean
        Dim oInfCarteraTitulo As New CarteraBM
        Dim strNewFechaOperacion As String
        Dim strOldFechaOperacion As String
        strNewFechaOperacion = UIUtility.ConvertirFechaaDecimal(tbFechaInicio.Text)
        strOldFechaOperacion = UIUtility.ConvertirFechaaDecimal(lblFecha.Text)
        If oInfCarteraTitulo.GeneraSaldosCarteraTitulo(strOldFechaOperacion, strNewFechaOperacion, dlPortafolio.SelectedValue.Trim, DatosRequest) Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub MostrarMensajeConfirmacion(ByVal DsTablas As DataSet)
        Dim StrCantidad As String = String.Empty
        Dim StrTabla As String = String.Empty
        Dim i As Integer
        Dim msg As String = ""
        msg = "El Portafolio se ha cerrado Satisfactoriamente \n"
        msg += "Se Generó el siguiente Backup :\n"
        For i = 0 To DsTablas.Tables.Count - 1
            For Each drv As DataRow In DsTablas.Tables(i).Rows
                StrCantidad = drv("cantidad")
                StrTabla = drv("Tabla")
                msg += "\t- " & StrTabla & ":" & StrCantidad & " Registros \n"
            Next
        Next
        AlertaJS(msg)
    End Sub
    Private Sub CargarPortafolio(Optional ByVal enabled As Boolean = True)
        If enabled Then
            Dim oPortafolio As New PortafolioBM
            dlPortafolio.DataSource = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS)
            dlPortafolio.DataValueField = "CodigoPortafolio"
            dlPortafolio.DataTextField = "Descripcion"
            dlPortafolio.DataBind()
        Else
            dlPortafolio.Items.Clear()
            UIUtility.InsertarElementoSeleccion(dlPortafolio)
        End If
        dlPortafolio.Enabled = enabled
    End Sub
    Private Sub ObtenerFechaActual(ByVal fechaAnt As String)
        Dim Fecha As Decimal
        Dim fechaAnterior As Date
        Dim fechaNueva As Date
        Dim oFeriadoBM As New FeriadoBM
        Dim EsFeriado As Boolean
        fechaAnterior = Convert.ToDateTime(fechaAnt)
        fechaNueva = fechaAnterior.AddDays(1)
        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
        If fechaNueva.DayOfWeek = DayOfWeek.Saturday Then
            While fechaNueva.DayOfWeek = DayOfWeek.Saturday
                fechaNueva = fechaNueva.AddDays(2)
                Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
                While oFeriadoBM.BuscarPorFecha(Fecha, VALIDAFERIADO)
                    fechaNueva = fechaNueva.AddDays(1)
                    Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
                End While
            End While
        Else
            EsFeriado = oFeriadoBM.BuscarPorFecha(Fecha, VALIDAFERIADO)
            If EsFeriado = True Then
                While oFeriadoBM.BuscarPorFecha(Fecha, VALIDAFERIADO)
                    fechaNueva = fechaNueva.AddDays(1)
                    Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
                    While fechaNueva.DayOfWeek = DayOfWeek.Saturday
                        fechaNueva = fechaNueva.AddDays(2)
                        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
                    End While
                End While
            End If
        End If
        Me.tbFechaInicio.Text = fechaNueva.ToShortDateString
    End Sub
    Private Sub ObtenerFechaAAnterior(ByVal fechaAct As String)
        Dim Fecha As Decimal
        Dim fechaAnterior As Date
        Dim fechaNueva As Date
        Dim oFeriadoBM As New FeriadoBM
        Dim EsFeriado As Boolean
        fechaAnterior = Convert.ToDateTime(fechaAct)
        fechaNueva = fechaAnterior.AddDays(-1)
        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
        If fechaNueva.DayOfWeek = DayOfWeek.Sunday Then
            While fechaNueva.DayOfWeek = DayOfWeek.Sunday
                fechaNueva = fechaNueva.AddDays(-2)
                Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
                While oFeriadoBM.BuscarPorFecha(Fecha, VALIDAFERIADO)
                    fechaNueva = fechaNueva.AddDays(-1)
                    Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
                End While
            End While
        Else
            EsFeriado = oFeriadoBM.BuscarPorFecha(Fecha, VALIDAFERIADO)
            If EsFeriado = True Then
                While oFeriadoBM.BuscarPorFecha(Fecha, VALIDAFERIADO)
                    fechaNueva = fechaNueva.AddDays(-1)
                    Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
                    While fechaNueva.DayOfWeek = DayOfWeek.Sunday
                        fechaNueva = fechaNueva.AddDays(-2)
                        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
                    End While
                End While
            End If
        End If
        Me.tbFechaInicio.Text = fechaNueva.ToShortDateString
    End Sub
#End Region
End Class