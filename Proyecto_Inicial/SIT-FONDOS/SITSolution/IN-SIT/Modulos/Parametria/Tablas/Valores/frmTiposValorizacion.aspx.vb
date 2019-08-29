Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Valores_frmTiposValorizacion
    Inherits BasePage

#Region " /* Metodos de Pagina */ "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Select Case Request.QueryString("TipoBCR")
                    Case "Unico"
                        lblTitulo.Text = "Mantenimiento BCR Único"
                        Me.TablaBCRSeriado.Visible = False
                        Me.TablaBCRAceptar.Visible = False

                        If Not (Request.QueryString("codUnico") = Nothing) Then
                            Me.tbBCRUnico_CodigoSBS.Enabled = False
                            Me.hdCodigoUnico.Value = Request.QueryString("codUnico")
                            CargarRegistroBCRUnico(hdCodigoUnico.Value)
                        Else
                            Me.tbBCRUnico_CodigoSBS.Enabled = True
                            Me.hdCodigoUnico.Value = ""
                        End If
                    Case "Seriado"
                        lblTitulo.Text = "Mantenimiento BCR Seriado"
                        TablaBCRUnico.Visible = False
                        TablaBCRUnicoAceptar.Visible = False

                        If Not (Request.QueryString("cod") = Nothing) Then
                            Me.tbCodigo.Enabled = False
                            Me.hd.Value = Request.QueryString("cod")
                            CargarRegistro(hd.Value)
                        Else
                            Me.tbCodigo.Enabled = True
                            Me.hd.Value = ""
                        End If
                End Select
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oTipoValorizacionBM As New TipoValorizacionBM
            Dim oBCRSeriadoBE As New BCRSeriadoBE
            If (Me.hd.Value = "") Then
                If verificarExistencia() = False Then
                    oBCRSeriadoBE = crearObjeto()
                    oTipoValorizacionBM.InsertarBCRSeriado(oBCRSeriadoBE, DatosRequest)
                    AlertaJS("Los datos fueron grabados correctamente")
                    LimpiarCampos()
                Else
                    AlertaJS("Este registro ya existe")
                End If
            Else
                oBCRSeriadoBE = crearObjeto()
                oTipoValorizacionBM.ModificarBCRSeriado(oBCRSeriadoBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al grabar los datos")
        End Try
    End Sub

    Private Sub btnAceptar2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar2.Click
        Try
            Dim oBM As New TipoValorizacionBM
            Dim oBCRUnicoBE As New BCRUnicoBE
            If (hdCodigoUnico.Value = "") Then
                If verificarExistenciaBCRUnico() = False Then
                    oBCRUnicoBE = crearObjetoBCRUnico()
                    oBM.InsertarBCRUnico(oBCRUnicoBE, DatosRequest)
                    LimpiarCamporBCRUnicos()
                    AlertaJS("Los datos fueron grabados correctamente")
                Else
                    AlertaJS("Este registro ya existe")
                End If

            Else
                oBCRUnicoBE = crearObjetoBCRUnico()
                oBM.ModificarBCRUnico(oBCRUnicoBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al grabar los datos")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Salir()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub

    Private Sub btnRetornar2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar2.Click
        Try
            Salir()
        Catch ex As Exception
            AlertaJS("Ocurrió un error al retornar la página")
        End Try
    End Sub

#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub Salir()
        Response.Redirect("frmBusquedaTiposValorizacion.aspx")
    End Sub

    Public Sub CargarRegistro(ByVal Codigo As String)
        Dim oBM As New TipoValorizacionBM
        Dim oData As DataTable = oBM.SeleccionarBcrSeriado(Codigo, Me.DatosRequest)
        Me.tbCodigo.Text = oData.Rows(0)("CodigoMnemonico")
        Me.tbDescripcion.Text = oData.Rows(0)("CuentaContable")
        Me.tbNombreCuenta.Text = oData.Rows(0)("NombreCuenta")
    End Sub

    Public Sub CargarRegistroBCRUnico(ByVal Codigo As String)
        Dim oBM As New TipoValorizacionBM
        Dim oData As DataTable = oBM.SeleccionarBcrUnico(Codigo, Me.DatosRequest)
        Me.tbBCRUnico_CodigoTitulo.Text = Convert.ToString(oData.Rows(0)("CodigoTipoTitulo"))
        Me.tbBCRUnico_CodigoEntidad.Text = Convert.ToString(oData.Rows(0)("CodigoEntidad"))
        Me.tbBCRUnico_CodigoMoneda.Text = Convert.ToString(oData.Rows(0)("CodigoMoneda"))
        Me.tbBCRUnico_CodigoSBS.Text = Convert.ToString(oData.Rows(0)("CodigoSBS"))
        Me.tbBCRUnico_CuentaContable.Text = Convert.ToString(oData.Rows(0)("CuentaContable"))
        Me.tbBCRUnico_NombreCuenta.Text = Convert.ToString(oData.Rows(0)("NombreCuenta"))
    End Sub

    Public Function CargarSecuencial() As Boolean
        Try
            tbCodigo.Text = New ParametrosGeneralesBM().ObtenerProximoSecuencial(ParametrosSIT.TABLAS_TBL_T01, DatosRequest).ToString()
        Catch ex As Exception
            tbCodigo.Text = ""
        End Try
    End Function

    Public Function crearObjeto() As BCRSeriadoBE
        Dim oBCRSeriadoBE As New BCRSeriadoBE
        Dim oRow1 As BCRSeriadoBE.BCRSeriadoRow
        oRow1 = CType(oBCRSeriadoBE.BCRSeriado.NewRow(), BCRSeriadoBE.BCRSeriadoRow)
        oRow1.CodigoMnemonico = tbCodigo.Text
        oRow1.CuentaContable = tbDescripcion.Text
        oRow1.NombreCuenta = tbNombreCuenta.Text
        oBCRSeriadoBE.BCRSeriado.AddBCRSeriadoRow(oRow1)
        oBCRSeriadoBE.BCRSeriado.AcceptChanges()
        Return oBCRSeriadoBE
    End Function

    Public Function crearObjetoBCRUnico() As BCRUnicoBE
        Dim oBCRUnicoBE As New BCRUnicoBE
        Dim oRow1 As BCRUnicoBE.BCRUnicoRow
        oRow1 = CType(oBCRUnicoBE.BCRUnico.NewRow(), BCRUnicoBE.BCRUnicoRow)
        oRow1.CodigoTipoTitulo = tbBCRUnico_CodigoTitulo.Text
        oRow1.CodigoSBS = tbBCRUnico_CodigoSBS.Text
        oRow1.CodigoEntidad = tbBCRUnico_CodigoEntidad.Text
        oRow1.CodigoMoneda = tbBCRUnico_CodigoMoneda.Text
        oRow1.CuentaContable = tbBCRUnico_CuentaContable.Text
        oRow1.NombreCuenta = tbBCRUnico_NombreCuenta.Text
        oBCRUnicoBE.BCRUnico.AddBCRUnicoRow(oRow1)
        oBCRUnicoBE.BCRUnico.AcceptChanges()
        Return oBCRUnicoBE
    End Function

    Public Sub LimpiarCampos()
        Me.tbCodigo.Text = ""
        Me.tbDescripcion.Text = ""
        tbNombreCuenta.Text = ""
        'Me.ddlSituacion.SelectedValue = "A"
    End Sub

    Public Sub LimpiarCamporBCRUnicos()
        Me.tbBCRUnico_CodigoEntidad.Text = ""
        Me.tbBCRUnico_CodigoMoneda.Text = ""
        Me.tbBCRUnico_CodigoSBS.Text = ""
        Me.tbBCRUnico_CodigoTitulo.Text = ""
        Me.tbBCRUnico_NombreCuenta.Text = ""
        Me.tbBCRUnico_CuentaContable.Text = ""
    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oTipoValorizacionBM As New TipoValorizacionBM
        Dim oData As New DataTable
        oData = oTipoValorizacionBM.SeleccionarBcrSeriado(Me.tbCodigo.Text, DatosRequest)
        If oData.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Private Function verificarExistenciaBCRUnico() As Boolean
        Dim oTipoValorizacionBM As New TipoValorizacionBM
        Dim oData As New DataTable
        oData = oTipoValorizacionBM.SeleccionarBcrUnico(Me.tbBCRUnico_CodigoSBS.Text, DatosRequest)
        If oData.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

#End Region

End Class
