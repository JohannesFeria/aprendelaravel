Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
'Imports System.Data.OleDb
Imports Microsoft.Office

Partial Class Modulos_Gestion_Archivos_Planos_frmInterfaseBenchmark
    Inherits BasePage


#Region "Variables"
    Dim strmensaje As String
    Dim sFileName As String
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            CargarPagina()
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarPagina()

        If Not Page.IsPostBack Then
            CargarCombos()
        End If

    End Sub

    Private Sub CargarCombos()
        Dim dtPortafolio As DataTable
        'Dim oParametrosGeneralesBM As New ParametrosGeneralesBM

        dtPortafolio = New PortafolioBM().PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, Constantes.M_STR_CONDICIONAL_NO)
        HelpCombo.LlenarComboBox(ddlFondo, dtPortafolio, "CodigoPortafolio", "Descripcion", False)
        'dtPortafolio = oParametrosGeneralesBM.Listar("Fondos", Me.DatosRequest)
        'HelpCombo.LlenarComboBox(Me.ddlFondo, dtPortafolio, "Valor", "Nombre", False)
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            If iptRuta.Value.ToString.Trim.Equals("") Then
                AlertaJS("Especifique la ruta del archivo.")
            Else
                CargarArchivoVectorPrecioForwards()
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub Imagebutton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Private Sub CargarArchivoVectorPrecioForwards()

        'Dim strRuta As String = (New ParametrosGeneralesBM().Listar("PREORDEN", Nothing)).Rows(0)("Valor")
        Dim strRuta As String = (New ParametrosGeneralesBM().Listar("RUTA_TEMP", Nothing)).Rows(0)("Valor")
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)

            'PRIMERO SE SUBE EL ARCHIVO
            'If Dir(strRuta & "\" & fInfo.Name) = "" Then strRuta = Environment.GetEnvironmentVariable("systemroot")
            strRuta = strRuta & fInfo.Name
            'iptRuta.PostedFile.SaveAs(strRuta & "\" & fInfo.Name)
            iptRuta.PostedFile.SaveAs(strRuta)

            'SE VERIFICA Q EXISTA EL ARCHIVO
            'If Dir(strRuta & "\" & fInfo.Name) <> "" Then
            If Dir(strRuta) <> "" Then
                If fInfo.Extension = ".xls" Then
                    Try
                        Dim he As New HelpExcel
                        'Call he.LeerActualizarExcel(strRuta & "\" & fInfo.Name, ddlFondo.SelectedValue.ToString, DatosRequest)
                        Call he.LeerActualizarExcel(strRuta, ddlFondo.SelectedValue.ToString, DatosRequest)
                        AlertaJS("Se realizó la importación de datos correctamente")
                    Catch ex As Exception
                    Finally
                        'File.Delete(strRuta & "\" & fInfo.Name)
                        File.Delete(strRuta)
                    End Try
                Else
                    If fInfo.Extension.Equals("") Then
                        AlertaJS("La ruta no es válida. Especificar ruta de un archivo")
                    Else
                        'File.Delete(strRuta & "\" & fInfo.Name)
                        File.Delete(strRuta)
                        AlertaJS("El tipo de archivo no es válido")
                    End If
                End If
            Else
                'File.Delete(strRuta & "\" & fInfo.Name)
                File.Delete(strRuta)
                AlertaJS("El archivo no existe")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al importar los datos")
        End Try
    End Sub

End Class
