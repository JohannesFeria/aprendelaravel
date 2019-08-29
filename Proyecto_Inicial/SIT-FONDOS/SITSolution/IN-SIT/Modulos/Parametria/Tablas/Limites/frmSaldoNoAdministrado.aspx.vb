Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Partial Class Modulos_Parametria_Tablas_Limites_frmSaldoNoAdministrado
    Inherits BasePage
    Dim FechaSaldo As Decimal
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
    End Sub

    Private Sub limpiarCampos()
        Me.ddlMandato.Text = ""
        Me.ddlBanco.Text = ""
        Me.txtFecha.Text = ""
        Me.ddlTipoCuenta.Text = ""
        Me.ddlMoneda.Text = ""
        Me.txtSaldo.Text = ""
        Me.ddlEstado.Text = ""
    End Sub

    Private Sub CargarCombos()

        LlenarMandato()
        LlenarBanco()
        LlenarTipoCuenta()
        LlenarMoneda()
        LlenarSituacion()

    End Sub

    Private Sub LlenarMandato()
        Dim dtMandatos As DataTable
        Dim oTercerosBM As New TercerosBM

        dtMandatos = oTercerosBM.ListarMandatos().Terceros
        HelpCombo.LlenarComboBox(Me.ddlMandato, dtMandatos, "CodigoTercero", "Descripcion", True)
    End Sub

    Private Sub LlenarBanco()
        Dim dtBancos As DataTable
        Dim oTercerosBM As New TercerosBM

        dtBancos = oTercerosBM.ListarBancosMandatos().Terceros
        HelpCombo.LlenarComboBox(Me.ddlBanco, dtBancos, "CodigoTercero", "Descripcion", True)
    End Sub

    Private Sub LlenarTipoCuenta()
        Dim dtTipoCuenta As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM

        dtTipoCuenta = oParametrosGeneralesBM.Listar("TipoCuenta", Me.DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlTipoCuenta, dtTipoCuenta, "Valor", "Comentario", True)
    End Sub

    Private Sub LlenarMoneda()
        Dim dtMoneda As DataTable
        Dim oMonedaBM As New MonedaBM

        dtMoneda = oMonedaBM.Listar(Me.DatosRequest).Tables(0)
        HelpCombo.LlenarComboBox(Me.ddlMoneda, dtMoneda, "CodigoMoneda", "Descripcion", True)
    End Sub

    Private Sub LlenarSituacion()
        Dim DtTablaSituacion As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM

        DtTablaSituacion = oParametrosGeneralesBM.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlEstado, DtTablaSituacion, "Valor", "Nombre", False)
    End Sub

    Public Sub CargarRegistro(ByVal codigo As String)
        Dim oSaldoNoAdministradoBM As New SaldoNoAdministradoBM
        Dim oDS As DataSet
        Dim varDecimal As Decimal
        varDecimal = 0
        oDS = oSaldoNoAdministradoBM.SeleccionarPorFiltro(codigo, "", varDecimal, "", "", "", "")

        ddlMandato.SelectedValue = oDS.Tables(0).Rows(0)("CodigoMandato")
        ddlBanco.SelectedValue = oDS.Tables(0).Rows(0)("CodigoBanco")
        ddlTipoCuenta.SelectedValue = oDS.Tables(0).Rows(0)("CodigoTipoCuenta")
        ddlMoneda.SelectedValue = oDS.Tables(0).Rows(0)("CodigoMoneda")
        txtSaldo.Text = Format(oDS.Tables(0).Rows(0)("Saldo"), "##,##0.0000000")
        Try
            txtFecha.Text = UIUtility.ConvertirFechaaString(oDS.Tables(0).Rows(0)("Fecha"))
        Catch ex As Exception
            txtFecha.Text = ""
        End Try
        ddlEstado.SelectedValue = oDS.Tables(0).Rows(0)("Situacion")

    End Sub

    Protected Sub btnRetornar_Click(sender As Object, e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("frmBusquedaSaldoNoAdministrado.aspx")
    End Sub

    Protected Sub btnAceptar_Click(sender As Object, e As System.EventArgs) Handles btnAceptar.Click
        Dim oSaldoNoAdministradoBM As New SaldoNoAdministradoBM
        Dim oSaldoNoAdministradoBE As New SaldoNoAdministradoBE
        Try
            oSaldoNoAdministradoBE = obtenerInstancia()

            If Me.hd.Value = "" Then
                If VerificarExisteSaldoNoAdministrado("") = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If
                oSaldoNoAdministradoBM.Insertar(oSaldoNoAdministradoBE, Me.DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
                limpiarCampos()
            Else
                If VerificarExisteSaldoNoAdministrado("E") = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If
                oSaldoNoAdministradoBM.Modificar(oSaldoNoAdministradoBE, Me.DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Private Function obtenerInstancia() As SaldoNoAdministradoBE
        Dim oSaldoNoAdministradoBE As New SaldoNoAdministradoBE


        If hd.Value <> "" Then
            oSaldoNoAdministradoBE.CodigoSaldoNoAdministrado = hd.Value
        Else
            oSaldoNoAdministradoBE.CodigoSaldoNoAdministrado = ""
        End If

        oSaldoNoAdministradoBE.CodigoTercero = Me.ddlMandato.SelectedValue
        oSaldoNoAdministradoBE.CodigoTerceroFinanciero = Me.ddlBanco.SelectedValue
        oSaldoNoAdministradoBE.TipoCuenta = Me.ddlTipoCuenta.SelectedValue
        oSaldoNoAdministradoBE.CodigoMoneda = Me.ddlMoneda.SelectedValue
        oSaldoNoAdministradoBE.Situacion = Me.ddlEstado.SelectedValue
        oSaldoNoAdministradoBE.Saldo = Me.txtSaldo.Text
        oSaldoNoAdministradoBE.Fecha = UIUtility.ConvertirFechaaDecimal(Me.txtFecha.Text)

        Return oSaldoNoAdministradoBE
    End Function

    Private Function VerificarExisteSaldoNoAdministrado(ByVal tipo As String) As Boolean
        Dim oSaldoNoAdministradoBM As New SaldoNoAdministradoBM
        Dim oDS As DataSet

        Me.FechaSaldo = UIUtility.ConvertirFechaaDecimal(Me.txtFecha.Text)
        'Me.PortafolioOT = Me.ddlPortafolio.SelectedValue

        oDS = oSaldoNoAdministradoBM.SeleccionarPorFiltro(hd.Value, Me.ddlMandato.SelectedValue, Me.FechaSaldo, Me.ddlBanco.SelectedValue, Me.ddlTipoCuenta.SelectedValue, Me.ddlMoneda.SelectedValue, tipo)
        If oDS.Tables(0).Rows.Count > 0 Then
            VerificarExisteSaldoNoAdministrado = True
        Else
            VerificarExisteSaldoNoAdministrado = False
        End If
    End Function

End Class
