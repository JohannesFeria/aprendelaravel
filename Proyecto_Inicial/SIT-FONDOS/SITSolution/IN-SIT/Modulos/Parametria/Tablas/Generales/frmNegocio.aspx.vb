Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Generales_frmNegocio
    Inherits BasePage

#Region " /* Metodos de Pagina */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("cod") = Nothing) Then
                    hd.Value = Request.QueryString("cod")
                    cargarRegistro(hd.Value)
                Else
                    hd.Value = ""
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try        
    End Sub

    Private Sub ibAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibAceptar.Click        
        Try
            Dim oNegocioBM As New NegocioBM
            Dim oNegocioBE As New NegocioBE
            oNegocioBE = crearObjeto()
            If Me.hd.Value = "" Then
                If verificarExistenciaNegocio() = True Then
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If
                oNegocioBM.Insertar(oNegocioBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
                LimpiarCampos()
            Else
                oNegocioBM.Modificar(oNegocioBE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Grabar los datos")
        End Try
    End Sub

    Private Sub ibCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibCancelar.Click
        Try
            Response.Redirect("frmBusquedaNegocios.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cancelar la operación")
        End Try        
    End Sub
#End Region

#Region " /* Funciones Personalizadas */"

    Public Sub cargarRegistro(ByVal codigo As String)
        Dim oNegocioBM As New NegocioBM
        Dim oNegocio As New NegocioBE
        oNegocio = oNegocioBM.SeleccionarPorFiltro(codigo, "", "", DatosRequest)
        Me.tbCodigo.Enabled = False

        Me.hd.Value = CType(oNegocio.Negocio.Rows(0), NegocioBE.NegocioRow).CodigoNegocio.ToString()
        Me.tbCodigo.Text = CType(oNegocio.Negocio.Rows(0), NegocioBE.NegocioRow).CodigoNegocio.ToString()
        Me.tbDescripcion.Text = CType(oNegocio.Negocio.Rows(0), NegocioBE.NegocioRow).Descripcion.ToString()
        Me.ddlSituacion.SelectedValue = CType(oNegocio.Negocio.Rows(0), NegocioBE.NegocioRow).Situacion.ToString()
        Me.ddlMoneda.SelectedValue = CType(oNegocio.Negocio.Rows(0), NegocioBE.NegocioRow).CodigoMoneda.ToString()

    End Sub

    Public Function crearObjeto() As NegocioBE
        Dim oNegocioBE As New NegocioBE
        Dim oRow As NegocioBE.NegocioRow
        oRow = CType(oNegocioBE.Negocio.NewNegocioRow(), NegocioBE.NegocioRow)

        oRow.CodigoNegocio = tbCodigo.Text.ToUpper.Trim.ToString
        oRow.Descripcion = Me.tbDescripcion.Text.ToUpper.TrimStart.TrimEnd.ToString
        oRow.Situacion = Me.ddlSituacion.SelectedValue
        oRow.CodigoMoneda = Me.ddlMoneda.SelectedValue

        IIf(hd.Value <> "", oRow.CodigoNegocio = hd.Value, oRow.CodigoNegocio = Me.tbCodigo.Text.Trim)

        oNegocioBE.Negocio.AddNegocioRow(oRow)
        oNegocioBE.Negocio.AcceptChanges()

        Return oNegocioBE
    End Function

    Public Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

        UIUtility.CargarMonedaSituacionOI(ddlMoneda, "")
    End Sub

    Sub LimpiarCampos()
        Me.tbCodigo.Text = ""
        Me.tbDescripcion.Text = ""
        Me.ddlSituacion.SelectedIndex = 0
    End Sub

    Private Function verificarExistenciaNegocio() As Boolean
        Dim oNegocioBM As New NegocioBM
        Dim oNegocioBE As New NegocioBE
        oNegocioBE = oNegocioBM.SeleccionarPorFiltro(Me.tbCodigo.Text.Trim, "", "", DatosRequest)
        If oNegocioBE.Negocio.Rows.Count > 0 Then
            verificarExistenciaNegocio = True
        Else
            verificarExistenciaNegocio = False
        End If
    End Function

#End Region

End Class
