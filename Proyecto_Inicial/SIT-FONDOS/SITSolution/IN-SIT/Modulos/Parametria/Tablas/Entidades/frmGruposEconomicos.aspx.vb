Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data

Partial Class Modulos_Parametria_Tablas_Entidades_frmGruposEconomicos
    Inherits BasePage

#Region "/* Métodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.btnAceptar.Attributes.Add("onclick", "javascript:return Validar();")
            If Not Page.IsPostBack Then
                CargarCombos()
                If Not (Request.QueryString("cod") = Nothing) Then
                    hd.Value = Request.QueryString("cod")
                    cargarRegistro(hd.Value)
                Else
                    hd.Value = String.Empty
                End If
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la página")
        End Try        
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaGruposEconomicos.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try        
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        Try
            Dim oGrupoEconomicoBM As New GrupoEconomicoBM
            Dim oGrupoEconomicoBE As New GrupoEconomicoBE

            oGrupoEconomicoBE = crearObjeto()
            If Me.hd.Value = "" Then
                If verificarExistenciaGrupoEconomico() = True Then                    
                    AlertaJS("Este registro ya existe")
                    Exit Sub
                End If
                oGrupoEconomicoBM.Insertar(oGrupoEconomicoBE, DatosRequest)
                AlertaJS("Los datos fueron grabados correctamente")
                LimpiarCampos()
            Else
                oGrupoEconomicoBM.Modificar(oGrupoEconomicoBE, DatosRequest)
                AlertaJS("Los datos fueron modificados correctamente")
            End If            
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Aceptar")
        End Try
    End Sub

#End Region

#Region " */ Funciones Personalizadas"

    Public Sub cargarRegistro(ByVal codigo As String)
        Dim oGrupoEconomicoBM As New GrupoEconomicoBM
        Dim oGrupoEconomico As New GrupoEconomicoBE
        oGrupoEconomico = oGrupoEconomicoBM.SeleccionarPorFiltro(codigo, String.Empty, String.Empty, DatosRequest)
        tbCodigo.Enabled = False

        Me.hd.Value = DirectCast(oGrupoEconomico.GrupoEconomico.Rows(0), GrupoEconomicoBE.GrupoEconomicoRow).CodigoGrupoEconomico.ToString()
        Me.tbCodigo.Text = DirectCast(oGrupoEconomico.GrupoEconomico.Rows(0), GrupoEconomicoBE.GrupoEconomicoRow).CodigoGrupoEconomico.ToString()
        Me.tbDescripcion.Text = DirectCast(oGrupoEconomico.GrupoEconomico.Rows(0), GrupoEconomicoBE.GrupoEconomicoRow).Descripcion.ToString()
        Me.ddlSituacion.SelectedValue = DirectCast(oGrupoEconomico.GrupoEconomico.Rows(0), GrupoEconomicoBE.GrupoEconomicoRow).Situacion.ToString()

        'INI CMB OT 61566 20101026 - Muestra el valor del campo "Entidad Vinculada" (S/N)
        Try
            If Constantes.M_STR_CONDICIONAL_SI = DirectCast(oGrupoEconomico.GrupoEconomico.Rows(0), GrupoEconomicoBE.GrupoEconomicoRow).EntidadVinculada.ToString() Then
                Me.chkEntidadVinculada.Checked = True
            Else
                Me.chkEntidadVinculada.Checked = False
            End If
        Catch ex As Exception
            Me.chkEntidadVinculada.Checked = False
        End Try
        'FIN CMB OT 61566 20101026 
    End Sub
    Public Function crearObjeto() As GrupoEconomicoBE
        Dim oGrupoEconomicoBE As New GrupoEconomicoBE
        Dim oRow As GrupoEconomicoBE.GrupoEconomicoRow
        oRow = DirectCast(oGrupoEconomicoBE.GrupoEconomico.NewGrupoEconomicoRow(), GrupoEconomicoBE.GrupoEconomicoRow)

        oRow.CodigoGrupoEconomico = tbCodigo.Text.ToString.ToUpper.TrimStart.TrimEnd
        oRow.Descripcion = Me.tbDescripcion.Text.ToString.ToUpper.TrimStart.TrimEnd
        oRow.Situacion = Me.ddlSituacion.SelectedValue

        IIf(hd.Value <> "", oRow.CodigoGrupoEconomico = hd.Value, oRow.CodigoGrupoEconomico = Me.tbCodigo.Text.Trim)

        'CMB 20101026 Se agrego un nuevo parametro EntidadVinculada para el GrupoEcomonico
        oRow.EntidadVinculada = IIf(chkEntidadVinculada.Checked, Constantes.M_STR_CONDICIONAL_SI, Constantes.M_STR_CONDICIONAL_NO)

        oGrupoEconomicoBE.GrupoEconomico.AddGrupoEconomicoRow(oRow)
        oGrupoEconomicoBE.GrupoEconomico.AcceptChanges()

        Return oGrupoEconomicoBE

    End Function
    Public Sub CargarCombos()

        Dim tablaSituacion As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaSituacion = oParametrosGenerales.ListarSituacion(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlSituacion, tablaSituacion, "Valor", "Nombre", False)

    End Sub
    Public Sub LimpiarCampos()

        Me.tbCodigo.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.tbDescripcion.Text = Constantes.M_STR_TEXTO_INICIAL
        Me.ddlSituacion.SelectedIndex = 0
    End Sub
    Public Function verificarExistenciaGrupoEconomico()
        Dim oGrupoEconomicoBM As New GrupoEconomicoBM
        Dim oGrupoEconomicoBE As New GrupoEconomicoBE
        oGrupoEconomicoBE = oGrupoEconomicoBM.SeleccionarPorFiltro(Me.tbCodigo.Text.Trim, String.Empty, String.Empty, DatosRequest)

        Return oGrupoEconomicoBE.GrupoEconomico.Rows.Count > 0
    End Function
#End Region

End Class
