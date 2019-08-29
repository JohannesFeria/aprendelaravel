Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Contabilidad_frmMatriz
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("cod") = Nothing) Then
                    tbCodigo.Enabled = False
                    hd.Value = Request.QueryString("cod")
                    CargarRegistro(hd.Value)
                Else
                    tbCodigo.Enabled = True
                    hd.Value = ""
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try
    End Sub

#Region " /* Metodos de Pagina */ "

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Dim oMatrizContableBM As New MatrizContableBM
        Dim oMatrizContableBE As New MatrizContableBE
        If (hd.Value = "") Then
            'Si no tiene nada es porque es un nuevo registro
            Try
                If verificarExistencia() = False Then
                    oMatrizContableBE = crearObjeto()
                    oMatrizContableBM.Insertar(oMatrizContableBE, DatosRequest)
                    AlertaJS("Los datos fueron grabados correctamente")
                    LimpiarCampos()
                Else
                    AlertaJS("Este registro ya existe")
                End If
            Catch ex As Exception
                AlertaJS("Ocurrió un error al grabar los datos")
            End Try
        Else
            Try
                oMatrizContableBE = crearObjeto()
                oMatrizContableBM.Modificar(oMatrizContableBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
            Catch ex As Exception
                AlertaJS("Ocurrió un error al modificar los datos")
            End Try
        End If
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Try
            Response.Redirect("frmBusquedaMatriz.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al salir del sistema")
        End Try
    End Sub
#End Region

#Region " /* Funciones Personalizadas*/"

    Private Sub LimpiarCampos()
        tbCodigo.Text = ""
        tbDescripcion.Text = ""
        tbClaveInterfaz.Text = "" 'RGF 20080716
        ddlSituacion.SelectedValue = "A"
    End Sub

    Public Sub CargarRegistro(ByVal Codigo As String)
        Dim oMatrizContableBM As New MatrizContableBM
        Dim oMatrizContableBE As New MatrizContableBE

        oMatrizContableBE = oMatrizContableBM.Seleccionar(Codigo, DatosRequest)

        hd.Value = CType(oMatrizContableBE.MatrizContable.Rows(0), MatrizContableBE.MatrizContableRow).CodigoMatrizContable.ToString()
        tbCodigo.Text = CType(oMatrizContableBE.MatrizContable.Rows(0), MatrizContableBE.MatrizContableRow).CodigoMatrizContable.ToString()
        tbDescripcion.Text() = CType(oMatrizContableBE.MatrizContable.Rows(0), MatrizContableBE.MatrizContableRow).Descripcion.ToString().Trim.ToUpper
        'RGF 20080716
        tbClaveInterfaz.Text() = CType(oMatrizContableBE.MatrizContable.Rows(0), MatrizContableBE.MatrizContableRow).ClaveInterfaz.ToString().Trim.ToUpper
        ddlSituacion.SelectedValue = CType(oMatrizContableBE.MatrizContable.Rows(0), MatrizContableBE.MatrizContableRow).Situacion.ToString()
        ddlTabla.SelectedValue = CType(oMatrizContableBE.MatrizContable.Rows(0), MatrizContableBE.MatrizContableRow).TablaMatriz.ToString()
    End Sub


    Public Function crearObjeto() As MatrizContableBE
        Dim oMatrizContableBE As New MatrizContableBE
        Dim oRow As MatrizContableBE.MatrizContableRow

        oRow = CType(oMatrizContableBE.MatrizContable.NewRow(), MatrizContableBE.MatrizContableRow)

        oRow.Descripcion = tbDescripcion.Text.Trim.ToUpper
        oRow.ClaveInterfaz = tbClaveInterfaz.Text.Trim.ToUpper 'RGF 20080716
        oRow.Situacion = ddlSituacion.SelectedValue.ToString()
        oRow.TablaMatriz = ddlTabla.SelectedValue
        If hd.Value <> "" Then
            oRow.CodigoMatrizContable = hd.Value
        Else
            oRow.CodigoMatrizContable = tbCodigo.Text.Trim.ToUpper
        End If

        oMatrizContableBE.MatrizContable.AddMatrizContableRow(oRow)
        oMatrizContableBE.MatrizContable.AcceptChanges()

        Return oMatrizContableBE
    End Function

    Private Sub CargarCombos()
        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(ddlSituacion, tablaSituacion, "Valor", "Nombre", False)
        Dim dtTablaMatriz As DataTable = oParametrosGenerales.Listar("Matriz", DatosRequest)
        ddlTabla.DataSource = dtTablaMatriz
        ddlTabla.DataTextField = "Nombre"
        ddlTabla.DataValueField = "Valor"
        ddlTabla.DataBind()
    End Sub

    Private Function verificarExistencia() As Boolean
        Dim oMatrizContableBM As New MatrizContableBM
        Dim oMatrizContableBE As New MatrizContableBE
        oMatrizContableBE = oMatrizContableBM.Seleccionar(tbCodigo.Text, DatosRequest)
        If oMatrizContableBE.MatrizContable.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

#End Region

End Class
