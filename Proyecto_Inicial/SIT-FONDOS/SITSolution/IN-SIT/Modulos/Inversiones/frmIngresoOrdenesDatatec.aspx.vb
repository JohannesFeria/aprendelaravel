Imports Sit.BusinessLayer
Imports System.Data
Imports System.IO
Partial Class Modulos_Inversiones_frmIngresoOrdenesDatatec
    Inherits BasePage
    Dim objArchivoPlanoBM As New ArchivoPlanoBM
    Dim strmensaje As String
#Region "Rutinas"
    Private Function ObtenerArchivosCarga() As DataSet
        Dim dsArchivos As DataSet = New ArchivoPlanoBM().Listar(DatosRequest, ParametrosSIT.ARCHIVOSINTERFAZ_DATATEC)
        Return dsArchivos
    End Function
    Private Function ObtenerColumnasArchivo(ByVal dsArchivos As DataSet) As DataTable
        Dim dtColumnas As DataTable = New ArchivoPlanoBM().ListarEstructura(dsArchivos.Tables(0).Rows(0)(1), DatosRequest)
        Return dtColumnas
    End Function
    Private Function ValidarRuta(ByRef mensaje As String) As Boolean
        If txtRuta.Text.Trim.Length = 0 Then
            mensaje = "Ingrese un Ruta"
            Return False
        End If
        If Not File.Exists(txtRuta.Text) Then
            mensaje = "El Archivo para procesar no existe"
            Return False
        End If
        Return True
    End Function
    Private Sub RegistrarDatos(ByVal dtsDatos As DataSet, ByVal dtsRequest As DataSet)
        objArchivoPlanoBM.Insertar_OrdenInversionDATATEC(dtsDatos, dtsRequest)
    End Sub
#End Region
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.ibProcesar.Attributes.Add("onclick", "javascript:return ValidaRuta();")
            Dim oParametroGeneralesBM As New ParametrosGeneralesBM
            Me.txtRuta.Text = oParametroGeneralesBM.Listar("DATATEC", Me.DatosRequest).Rows(0)("Valor")
        End If
    End Sub
    Protected Sub ibProcesar_Click(sender As Object, e As EventArgs) Handles ibProcesar.Click
        Try
            Dim mensaje As String = String.Empty
            If ValidarRuta(mensaje) = False Then
                AlertaJS(mensaje)
                Exit Sub
            End If
            Dim objReader As New StreamReader(txtRuta.Text)
            Dim strLine As String = ""
            Dim arrText As New ArrayList
            Do
                strLine = objReader.ReadLine()
                If Not strLine Is Nothing Then
                    arrText.Add(strLine)
                End If
            Loop Until strLine Is Nothing
            objReader.Close()

            Dim dsArchivos As DataSet = ObtenerArchivosCarga()
            Dim dtEstructura As DataTable = ObtenerColumnasArchivo(dsArchivos)
            Dim dtDatos As New DataTable

            Dim filaEstructura As Integer
            For filaEstructura = 0 To dtEstructura.Rows.Count - 1
                dtDatos.Columns.Add(dtEstructura.Rows(filaEstructura).Item("ColumnaDescripcion"))
            Next

            Dim filaDatos As Integer
            For filaDatos = 0 To arrText.Count - 1
                Dim strDato As String = ""
                Dim objFila As DataRow
                objFila = dtDatos.NewRow()
                Dim intCantidad As Integer = arrText(filaDatos).Length

                filaEstructura = 0
                For filaEstructura = 0 To dtEstructura.Rows.Count - 1
                    Dim intPosiciInicial As Integer
                    Dim intLongitud As Integer
                    intPosiciInicial = dtEstructura.Rows(filaEstructura).Item("ColumnaPosicionInicial")
                    intLongitud = dtEstructura.Rows(filaEstructura).Item("ColumnaLongitud")

                    If (intCantidad > (intPosiciInicial + intLongitud)) Then
                        strDato = arrText(filaDatos).ToString.Substring(intPosiciInicial - 1, intLongitud)
                        objFila(filaEstructura) = strDato
                    Else
                        Exit For
                    End If
                Next
                dtDatos.Rows.Add(objFila)
            Next
            Dim dtsDatos As New DataSet
            Dim dtsResultado As New DataSet
            dtsDatos.Tables.Add(dtDatos)
            RegistrarDatos(dtsDatos, dtsResultado)
            lbContador.Text = dtsDatos.Tables(0).Rows.Count
            Me.dgLista.DataSource = objArchivoPlanoBM.ListarResultado(DatosRequest)
            Me.dgLista.DataBind()
            AlertaJS("Se cargaron las preordenes satisfactoriamente")
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub
    Protected Sub dgLista_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgLista.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow OrElse Not e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            e.Row.Cells(6).Text = UIUtility.ConvertirFechaaString(e.Row.Cells(6).Text)
            e.Row.Cells(7).Text = UIUtility.ConvertirFechaaString(e.Row.Cells(7).Text)
        End If
    End Sub
End Class
