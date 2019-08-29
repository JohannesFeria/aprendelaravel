Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
Imports System.Data
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office

Partial Class Modulos_ValorizacionCustodia_Valorizacion_frmInterfaseBloomberg
    Inherits BasePage


#Region "Variables"

    Dim oMonedaBM As New MonedaBM
    Dim oMonedaBE As New DataSet
    Dim oValoresBM As New ValoresBM
    Dim oValoresBE As New DataSet
    Dim oVectorTipoCambioBM As New VectorTipoCambioBM
    Dim oVectorTipoCambioBE As New VectorTipoCambio
    Dim oVectorPrecioBM As New VectorPrecioBM
    Dim oVectorPrecioBE As New VectorPrecioBE
    Dim oVectorPrecioSBSBM As New VectorPrecioSBSBM
    Dim oVectorPrecioSBSBE As New VectorPrecioSBSBE
    Dim oArchivoPlanoBM As New ArchivoPlanoBM
    Dim oArchivoPlanoBE As New DataSet
    Dim oArchivoPlanoEstructuraBM As New ArchivoPlanoEstructuraBM
    Dim oArchivoPlanoEstructuraBE As New DataSet
    Dim tipoInterface As String
    Dim fechaInterface As String
    Dim oUtil As New UtilDM
    Dim strmensaje As String
    Dim sFileName As String
    Dim errorNoExiste As Boolean = False

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                CargarPagina()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarPagina()
        Me.ibCargar.Attributes.Add("onclick", "javascript:return Confirmacion();")
        CargarCombos()
        Me.divDetalle.Visible = False
    End Sub

    Private Sub CargarCombos()

        Dim tablaHojasBloomberg As New DataTable
        Dim oParametrosGenerales As New ParametrosGeneralesBM

        tablaHojasBloomberg = oParametrosGenerales.ListarInterfaseBloomberg(DatosRequest)
        HelpCombo.LlenarComboBox(Me.ddlInterface, tablaHojasBloomberg, "Valor", "Nombre", True)

        Me.tbFechaInterface.Text = oUtil.RetornarFechaSistema
        RutaArchivo()
    End Sub

    Private Sub RutaArchivo()
        Dim oParametrosGenerales As New ParametrosGeneralesBM
        Dim Ruta As New DataTable
        Ruta = oParametrosGenerales.ListarRutaBloomberg(DatosRequest)
        Me.Myfile.Text = Ruta.Rows(0).Item("Valor").ToString()
    End Sub

    Public Function Validar() As String

        Dim msg As String = ""
        Dim strMensajeError As String = ""

        If Me.tbFechaInterface.Text.Trim = "" Then
            msg += "Fecha Interfase\n"
        End If

        If Me.ddlInterface.SelectedValue = "--Seleccione--" Then
            msg += "Interfase\n"
        End If
        If Me.Myfile.Text.Trim = "" Then
            msg += "Ruta\n"
        End If
        If (msg <> "") Then
            strMensajeError = "Los siguientes campos son obligatorios:\n" + msg + "\n"
            Return strMensajeError
        Else
            Return ""
        End If

    End Function

    Private Sub ibCargar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibCargar.Click

        Dim sFileName As String
        Dim fecha As String

        Me.divDetalle.Visible = False
        Me.msgError.Visible = False

        fecha = Me.tbFechaInterface.Text.Trim.Replace("/", "")
        If (ddlInterface.SelectedValue() = "PBN") Then
            sFileName = Me.Myfile.Text.Trim & "\INTERFASENAV" & fecha & ".xls"
        Else
            sFileName = Me.Myfile.Text.Trim & "\INTERFASEBLOOMBERG" & fecha & ".xls"
        End If
        Dim mensaje As String
        mensaje = Validar()
        If mensaje <> "" Then
            AlertaJS(mensaje)
            Exit Sub
        Else
            If Not (File.Exists(sFileName)) Then
                AlertaJS("No existe el Archivo Excel.")
            Else
                If (ddlInterface.SelectedValue() = "TC") Then
                    CargarArchivoTipoCambio(sFileName)
                Else
                    If (ddlInterface.SelectedValue() = "P") Then
                        CargarArchivoVectorPrecio(sFileName)
                    Else
                        If (ddlInterface.SelectedValue() = "INV") Then
                            CargarArchivoIndicadorInversiones(sFileName)
                        Else
                            If (ddlInterface.SelectedValue() = "DYL") Then
                                CargarArchivoIndicadorDivLib(sFileName)
                            Else
                                If (ddlInterface.SelectedValue() = "BLB") Then
                                    CargarArchivoIndicadorBloomberg(sFileName)
                                Else
                                    If (ddlInterface.SelectedValue() = "PBN") Then
                                        Dim he As New HelpExcel
                                        he.CargarArchivoVectorPrecioNAV(sFileName, DatosRequest, tbFechaInterface.Text.Trim)
                                    End If
                                End If
                            End If
                        End If
                    End If

                End If
            End If
            Me.Myfile.Text = sFileName
            Me.ddlInterface.SelectedIndex = 0
        End If


    End Sub

    Private Sub ibSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ibSalir.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Private Sub CargarArchivoIndicadorInversiones(ByVal sFileName As String)
        Dim oInterfaseBloombergBM As New InterfaseBloombergBM
        Dim fecha As Date
        Dim blnExisteItem As Boolean
        Try

            blnExisteItem = ExisteIndInv()
            Dim oConn As New OleDbConnection
            Dim oCmd As New OleDbCommand
            Dim oDa As New OleDbDataAdapter
            Dim oDs As New DataSet
            Dim DtPrecios As New DataTable

            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sFileName & "; Extended Properties= Excel 8.0;"

            oConn.Open()
            oCmd.CommandText = "SELECT * FROM [IND INVERSION$]"
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd

            'Llenar el DataSet
            oDa.Fill(oDs, "ListaPrecios")
            DtPrecios = oDs.Tables(0)
            fecha = Me.tbFechaInterface.Text

            If DtPrecios.Rows.Count = 0 Then
                AlertaJS("El Archivo no tiene registros")
            Else

                If Not blnExisteItem Then
                    oInterfaseBloombergBM.InsertarIndicadorInversiones(fecha, DtPrecios, Me.DatosRequest)
                    AlertaJS("Se cargó el Archivo correctamente")
                Else
                    oInterfaseBloombergBM.EliminarIndInversiones(fecha, Me.DatosRequest)
                    oInterfaseBloombergBM.InsertarIndicadorInversiones(fecha, DtPrecios, Me.DatosRequest)
                    AlertaJS("Se cargó el Archivo correctamente")
                End If

            End If

            oConn.Close()

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub
    Private Function ExistePrecioBloomberg() As Boolean

        Dim oInterfaseBloombergBM As New InterfaseBloombergBM
        Dim DtPrecio As New DataTable

        DtPrecio = oInterfaseBloombergBM.SeleccionarPreciosBloomberg(Me.tbFechaInterface.Text.Trim(), Me.DatosRequest)

        Return DtPrecio.Rows.Count > 0

    End Function
    Private Function ExisteTipoCambioBloomberg() As Boolean

        Dim oInterfaseBloombergBM As New InterfaseBloombergBM
        Dim DtTipoCambio As New DataTable

        DtTipoCambio = oInterfaseBloombergBM.SeleccionarTipoCambioBloomberg(Me.tbFechaInterface.Text.Trim(), Me.DatosRequest)

        Return DtTipoCambio.Rows.Count > 0

    End Function
    Private Function ExisteIndInv() As Boolean

        Dim oInterfaseBloombergBM As New InterfaseBloombergBM
        Dim DtTipoCambio As New DataTable

        DtTipoCambio = oInterfaseBloombergBM.SeleccionarIndInversiones(Me.tbFechaInterface.Text.Trim(), Me.DatosRequest)

        Return DtTipoCambio.Rows.Count > 0

    End Function
    Private Function ExisteIndBloomberg() As Boolean

        Dim oInterfaseBloombergBM As New InterfaseBloombergBM
        Dim DtTipoCambio As New DataTable

        DtTipoCambio = oInterfaseBloombergBM.SeleccionarIndBloomberg(Me.tbFechaInterface.Text.Trim(), Me.DatosRequest)

        Return DtTipoCambio.Rows.Count > 0

    End Function
    Private Function ExisteIndDivLib() As Boolean

        Dim oInterfaseBloombergBM As New InterfaseBloombergBM
        Dim DtTipoCambio As New DataTable

        DtTipoCambio = oInterfaseBloombergBM.SeleccionarIndDivLib(Me.tbFechaInterface.Text.Trim(), Me.DatosRequest)

        Return DtTipoCambio.Rows.Count > 0

    End Function

    Private Sub CargarArchivoIndicadorDivLib(ByVal sFileName As String)
        Dim oInterfaseBloombergBM As New InterfaseBloombergBM
        Dim fecha As Date
        Dim blnExisteItem As Boolean
        Dim TablaErrores As New DataTable
        Dim sFecha As String = tbFechaInterface.Text

        sFecha = sFecha.Substring(6, 4) & sFecha.Substring(3, 2) & sFecha.Substring(0, 2)

        Try
            blnExisteItem = ExisteIndDivLib()

            Dim oConn As New OleDbConnection
            Dim oCmd As New OleDbCommand
            Dim oDa As New OleDbDataAdapter
            Dim oDs As New DataSet
            Dim DtPrecios As New DataTable

            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sFileName & "; Extended Properties= Excel 8.0;"

            oConn.Open()
            oCmd.CommandText = "SELECT * FROM [DIV-LIB BLOOMBERG$]"
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd

            fecha = Me.tbFechaInterface.Text
            oDa.Fill(oDs, "ListaPrecios")
            DtPrecios = oDs.Tables(0)
            If DtPrecios.Rows.Count = 0 Then
                AlertaJS("El Archivo no tiene registros")
            Else

                If Not blnExisteItem Then
                    oInterfaseBloombergBM.InsertarIndicadorDivLib(fecha, DtPrecios, Me.DatosRequest)
                    AlertaJS("Se cargó el Archivo correctamente")
                Else
                    oInterfaseBloombergBM.EliminarIndDivLib(fecha, Me.DatosRequest)
                    oInterfaseBloombergBM.InsertarIndicadorDivLib(fecha, DtPrecios, Me.DatosRequest)
                    AlertaJS("Se cargó el Archivo correctamente")
                End If

                TablaErrores = oInterfaseBloombergBM.RecuperaDivLibBloombergNoReg_Listar(sFecha, "01", DatosRequest)
                If TablaErrores.Rows.Count > 0 Then
                    divDetalle.Visible = True
                    msgError.Visible = True
                    msgError.Text = "No existe Mnemonico de  el(los) siguiente(s) Código(s) SBS"
                    dgNoRegistrados.DataSource = TablaErrores
                    dgNoRegistrados.DataBind()
                End If

            End If

            oConn.Close()

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarArchivoIndicadorBloomberg(ByVal sFileName As String)
        Dim oInterfaseBloombergBM As New InterfaseBloombergBM
        Dim fecha As Date
        Dim blnExisteItem As Boolean

        Try

            blnExisteItem = ExisteIndBloomberg()

            Dim oConn As New OleDbConnection
            Dim oCmd As New OleDbCommand
            Dim oDa As New OleDbDataAdapter
            Dim oDs As New DataSet
            Dim DtPrecios As New DataTable

            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sFileName & "; Extended Properties= Excel 8.0;"
            oConn.Open()
            oCmd.CommandText = "SELECT * FROM [IND BLOOMBERG$]"
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd

            oDa.Fill(oDs, "ListaPrecios")
            DtPrecios = oDs.Tables(0)
            fecha = tbFechaInterface.Text

            If DtPrecios.Rows.Count = 0 Then
                AlertaJS("El Archivo no tiene registros")
            Else

                If Not blnExisteItem Then
                    oInterfaseBloombergBM.InsertarIndicadorBloomberg(fecha, DtPrecios, Me.DatosRequest)
                    AlertaJS("Se cargó el Archivo correctamente")
                Else
                    oInterfaseBloombergBM.EliminarIndBloomberg(fecha, Me.DatosRequest)
                    oInterfaseBloombergBM.InsertarIndicadorBloomberg(fecha, DtPrecios, Me.DatosRequest)
                    AlertaJS("Se cargó el Archivo correctamente")
                End If
            End If

            oConn.Close()

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarArchivoTipoCambio(ByVal sFileName As String)
        Dim oInterfaseBloombergBM As New InterfaseBloombergBM
        Dim TablaErrores As New DataTable
        Dim fecha As Date
        Dim blnExisteItem As Boolean
        Try

            blnExisteItem = ExisteTipoCambioBloomberg()

            Dim oConn As New OleDbConnection
            Dim oCmd As New OleDbCommand
            Dim oDa As New OleDbDataAdapter
            Dim oDs As New DataSet
            Dim DtPrecios As New DataTable

            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sFileName & "; Extended Properties= Excel 8.0;"

            oConn.Open()
            oCmd.CommandText = "SELECT * FROM [TIPO CAMBIO$]"
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd

            oDa.Fill(oDs, "ListaPrecios")
            DtPrecios = oDs.Tables(0)
            fecha = Me.tbFechaInterface.Text
            If DtPrecios.Rows.Count = 0 Then
                AlertaJS("El Archivo no tiene registros")
            Else
                If Not blnExisteItem Then
                    oInterfaseBloombergBM.InsertarTipoCambioBloomberg(fecha, DtPrecios, Me.DatosRequest)
                    TablaErrores = oInterfaseBloombergBM.InsertarTipoCambioReal(fecha, DtPrecios, Me.DatosRequest)
                    AlertaJS("Se cargó el Archivo correctamente")
                Else
                    oInterfaseBloombergBM.EliminarTipoCambioBloomberg(fecha, Me.DatosRequest)
                    oInterfaseBloombergBM.EliminarTipoCambio(fecha, Me.DatosRequest)

                    oInterfaseBloombergBM.InsertarTipoCambioBloomberg(fecha, DtPrecios, Me.DatosRequest)
                    TablaErrores = oInterfaseBloombergBM.InsertarTipoCambioReal(fecha, DtPrecios, Me.DatosRequest)
                    AlertaJS("Se cargó el Archivo correctamente")
                End If
            End If

            oConn.Close()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarArchivoVectorPrecio(ByVal sFileName As String)
        Dim oInterfaseBloombergBM As New InterfaseBloombergBM
        Dim TablaErrores As New DataTable
        Dim fecha As Date
        Dim blnExisteItem As Boolean
        Try

            Dim oConn As New OleDbConnection
            Dim oCmd As New OleDbCommand
            Dim oDa As New OleDbDataAdapter
            Dim oDs As New DataSet
            Dim DtPrecios As New DataTable

            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sFileName & "; Extended Properties= Excel 8.0;"

            oConn.Open()
            oCmd.CommandText = "SELECT * FROM [LISTA PRECIO$]"
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd

            oDa.Fill(oDs, "ListaPrecios")
            DtPrecios = oDs.Tables(0)
            fecha = Me.tbFechaInterface.Text.Trim
            If DtPrecios.Rows.Count = 0 Then
                AlertaJS("El Archivo no tiene registros")
            Else
                blnExisteItem = ExistePrecioBloomberg()
                If Not blnExisteItem Then
                    oInterfaseBloombergBM.InsertarPrecioBloomberg(fecha, DtPrecios, Me.DatosRequest)
                    TablaErrores = oInterfaseBloombergBM.ActualizarPrecioReal(fecha, DtPrecios, Me.DatosRequest)
                    If TablaErrores.Rows.Count > 0 Then
                        Me.divDetalle.Visible = True
                        Me.msgError.Visible = True
                        Me.msgError.Text = "No existe Mnemonico de  el(los) siguiente(s) Código(s) ISIN"
                        Me.dgNoExiste.DataSource = TablaErrores
                        Me.dgNoExiste.DataBind()
                    End If
                    AlertaJS("Se cargó el Archivo correctamente")
                Else
                    oInterfaseBloombergBM.ActualizarPrecioBloomberg(fecha, DtPrecios, Me.DatosRequest)
                    TablaErrores = oInterfaseBloombergBM.ActualizarPrecioReal(fecha, DtPrecios, Me.DatosRequest)
                    If TablaErrores.Rows.Count > 0 Then
                        Me.divDetalle.Visible = True
                        Me.msgError.Visible = True
                        Me.msgError.Text = "No existe Mnemonico de  el(los) siguiente(s) Código(s) ISIN"
                        Me.dgNoExiste.DataSource = TablaErrores
                        Me.dgNoExiste.DataBind()
                    End If
                    AlertaJS("Se cargó el Archivo correctamente")
                End If

            End If

            oConn.Close()

        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try

    End Sub

    Private Sub ddlInterface_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlInterface.SelectedIndexChanged
        RutaArchivo()
    End Sub

End Class
