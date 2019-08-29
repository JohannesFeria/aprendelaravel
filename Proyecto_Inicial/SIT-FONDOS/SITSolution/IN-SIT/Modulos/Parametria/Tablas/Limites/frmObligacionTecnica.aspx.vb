Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
'Imports System.Data.SqlTypes
'Imports System.Collections
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Limites_frmObligacionTecnica
    Inherits BasePage

    Private operacion As String
    Private oObligacionTecnicaBE As ObligacionTecnicaBE
    Private FechaObligacionTecnica As Decimal
    Private PortafolioOT As String

#Region "/* Métodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then
            Me.limpiarCampos()
            Me.CargarPortafolio()
            If Not (Request.QueryString("cod") = Nothing) Then
                hd.Value = Request.QueryString("cod")
                CargarRegistro(hd.Value)
            Else
                hd.Value = ""
            End If
        End If
    End Sub


    Protected Sub btnAceptar_Click(sender As Object, e As System.EventArgs) Handles btnAceptar.Click
        Dim oObligacionTecnicaBM As New ObligacionTecnicaBM
        Dim oObligacionTecnicaBE As New ObligacionTecnicaBE
        Try
            oObligacionTecnicaBE = obtenerInstancia()

            If Me.hd.Value = "" Then
                If VerificarExisteObligacionTecnica("") = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If
                oObligacionTecnicaBM.Insertar(oObligacionTecnicaBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
                limpiarCampos()
            Else
                If VerificarExisteObligacionTecnica("E") = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If
                oObligacionTecnicaBM.Modificar(oObligacionTecnicaBE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Protected Sub btnRetornar_Click(sender As Object, e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("frmBusquedaObligacionTecnica.aspx")
    End Sub

#End Region

#Region "/* Métodos de la Página */"

    Private Sub CargarPortafolio()
        Dim oPortafolio As New PortafolioBM
        HelpCombo.LlenarComboBox(ddlPortafolio, oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS), "CodigoPortafolio", "Descripcion", True)

    End Sub

    Private Sub limpiarCampos()
        Me.ddlPortafolio.Text = ""
        Me.txtMonto.Text = ""
        Me.txtFecha.Text = ""
        'Me.hd.Value = ""
        'Me.habilitarControles(True)
    End Sub

    Private Function obtenerInstancia() As ObligacionTecnicaBE
        Dim oObligacionTecnicaBE As New ObligacionTecnicaBE
        Dim oRow As ObligacionTecnicaBE.ObligacionTecnicaRow
        oRow = CType(oObligacionTecnicaBE.ObligacionTecnica.NewRow(), ObligacionTecnicaBE.ObligacionTecnicaRow)

        oRow.CodigoPortafolioSBS = Me.ddlPortafolio.SelectedValue
        oRow.Fecha = UIUtility.ConvertirFechaaDecimal(Me.txtFecha.Text)
        oRow.Monto = CType(Me.txtMonto.Text, Decimal)

        If hd.Value <> "" Then
            oRow.CodigoObligacionTecnica = hd.Value
        Else
            oRow.CodigoObligacionTecnica = ""
        End If

        oObligacionTecnicaBE.ObligacionTecnica.AddObligacionTecnicaRow(oRow)
        oObligacionTecnicaBE.ObligacionTecnica.AcceptChanges()
        Return oObligacionTecnicaBE
    End Function

    Private Function VerificarExisteObligacionTecnica(ByVal tipo As String) As Boolean
        Dim oObligacionTecnicaBM As New ObligacionTecnicaBM
        Dim oDS As DataSet

        Me.FechaObligacionTecnica = UIUtility.ConvertirFechaaDecimal(Me.txtFecha.Text)
        Me.PortafolioOT = Me.ddlPortafolio.SelectedValue

        oDS = oObligacionTecnicaBM.SeleccionarPorFiltro(hd.Value, Me.PortafolioOT, "", Me.FechaObligacionTecnica, tipo, Me.DatosRequest)
        If oDS.Tables(0).Rows.Count > 0 Then
            VerificarExisteObligacionTecnica = True
        Else
            VerificarExisteObligacionTecnica = False
        End If
    End Function


    Public Sub CargarRegistro(ByVal codigo As String)
        Dim oObligacionTecnicaBM As New ObligacionTecnicaBM
        Dim oDS As DataSet
        Dim varDecimal As Decimal
        varDecimal = 0
        oDS = oObligacionTecnicaBM.SeleccionarPorFiltro(codigo, "", "", varDecimal, "", DatosRequest)
        'oDS = oObligacionTecnicaBM.SeleccionarPorFiltro(codigo, "", "", 0 DatosRequest)
        ddlPortafolio.SelectedValue = oDS.Tables(0).Rows(0)("CodigoPortafolioSBS")
        Try
            txtFecha.Text = UIUtility.ConvertirFechaaString(oDS.Tables(0).Rows(0)("Fecha"))
        Catch ex As Exception
            txtFecha.Text = ""
        End Try
        txtMonto.Text = Format(oDS.Tables(0).Rows(0)("Monto"), "##,##0.0000000")
    End Sub
#End Region



End Class
