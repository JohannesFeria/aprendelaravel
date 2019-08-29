Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data
Imports System.Data.OleDb
Imports System.IO


Partial Class Modulos_Parametria_Tablas_Valores_frmFactorEmisorImportar
    Inherits BasePage

#Region "/* Métodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            Try

            Catch ex As Exception
                AlertaJS("Ocurrió un error al cargar la Página")
            End Try
        End If
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaFactor.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try        
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            Dim strRuta As String = "E:\Archivos Planos"
            If Not iptRuta.Value.ToString.Trim.Equals("") Then
                CargarArchivoOrigen(strRuta)
            Else
                AlertaJS("Especifique la ruta del archivo a importar")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Procesar")
        End Try        
    End Sub

#End Region

#Region "/* Métodos Personalizados */"

    Private Sub CargarArchivoOrigen(ByVal strRuta As String)
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            If Dir(strRuta & "\" & fInfo.Name) = "" Then strRuta = Environment.GetEnvironmentVariable("systemroot")
            iptRuta.PostedFile.SaveAs(strRuta & "\" & fInfo.Name)

            If Dir(strRuta & "\" & fInfo.Name) <> "" Then
                If fInfo.Extension = ".xls" Then
                    Try
                        LeerExcel(strRuta & "\" & fInfo.Name)
                    Catch ex As Exception
                        AlertaJS(ex.Message.ToString())
                    Finally
                        File.Delete(strRuta & "\" & fInfo.Name)
                        AlertaJS("Error al leer el archivo")
                    End Try
                Else
                    If fInfo.Extension.Equals("") Then
                        AlertaJS("La ruta no es válida. Especificar ruta de un archivo")
                    Else
                        File.Delete(strRuta & "\" & fInfo.Name)
                        AlertaJS("El tipo de archivo no es válido")
                    End If
                End If
            Else
                File.Delete(strRuta & "\" & fInfo.Name)
                AlertaJS("El archivo no existe")
            End If
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Private Sub LeerExcel(ByVal strRuta As String)
        Try
            CargarArchivo(strRuta)
        Catch ex As OleDbException
            AlertaJS("Error al leer el archivo Excel")
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Private Sub CargarArchivo(ByVal strRuta As String)
        Dim oConn As New OleDbConnection
        Dim oCmd As New OleDbCommand
        Dim oDa As New OleDbDataAdapter
        Dim oDs As New DataSet
        Dim strMensaje As String = ""

        Try
            'oConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828
            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & "; Extended Properties= Excel 12.0;" 'CMB Migracion 20120828 Se cambio la cadena de conexion para office 2010
            oConn.Open()

            'oCmd.CommandText = "SELECT [Emisor], [Código Isin], [Código SBS], [Descripción], [código Mnemónico], [Total Activo], [Total Pasivo], [Total Patrimonio], [Factor Riesgo], [Factor Liquidez], [Fecha de vigencia], [Situación] FROM [PatrimonioFideicomiso$] "
            oCmd.CommandText = "SELECT '" & ParametrosSIT.GRUPO_FACTOR_EMISOR & "' AS [GrupoFactor], [TipoFactor], [DescripcionTipoFactor], [CodigoEntidad], [DescripcionEntidad], [Factor], [Situacion], [FechaCreacion], [FechaVigencia] FROM [FactorEmisor$] "
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "FactorEmisor")

            ActualizarFactorPorExcel(oDs.Tables(0), "FactorEmisor", strMensaje)
            AlertaJS(strMensaje)
            oConn.Close()

        Catch ex As OleDbException
            Select Case ex.ErrorCode
                Case -2147467259
                    AlertaJS("El excel en este momento se encuentra abierto")
                Case -2147217865
                    AlertaJS("Error al leer el archivo Excel")
                Case Else
                    AlertaJS("Ocurrió un error no esperado. Revisar el archivo Excel")
            End Select

        Catch ex As Exception
            AlertaJS("Error al leer el archivo Excel")
            oConn.Close()
        End Try
    End Sub

    Private Sub ActualizarFactorPorExcel(ByVal data As DataTable, ByVal strReferencia As String, ByRef strmensaje As String)
        If data.Rows.Count = 0 Then
            strmensaje &= "La pestaña de " & strReferencia & " no tiene registros\n"
        Else
            Dim oFactorBM As New FactorBM
            If data.Columns.Count = 9 Then
                If oFactorBM.ActualizarFactorPorExcel(data, DatosRequest, strmensaje) Then
                    strmensaje &= "Los datos de " & strReferencia & " cargaron correctamente\n"
                Else
                    strmensaje &= "Los datos de " & strReferencia & " son inconsistentes \n"
                End If
            Else
                strmensaje &= "La pestaña " & strReferencia & " no tiene la estructura adecuada\n"
            End If
        End If
    End Sub

#End Region

End Class
