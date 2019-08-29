Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections

Imports System.Data
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System.Runtime.InteropServices.Marshal
Imports System
Imports System.IO

Partial Class Modulos_Parametria_Tablas_Valores_frmBalanceContableImportar
    Inherits BasePage

#Region "/* Métodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaBalanceContable.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try        
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            Dim strRuta As String = (New ParametrosGeneralesBM().Listar("BAL_CONTAB", Nothing)).Rows(0)("Valor")

            If Not iptRuta.Value.ToString.Trim.Equals("") Then
                CargarArchivoOrigen(strRuta)
            Else
                AlertaJS("Especifique la ruta de un archivo")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Procesar")
        End Try        
    End Sub

#End Region

#Region "/* Métodos Personalizados */"

    Private Sub CargarArchivo(ByVal strRuta As String)
        Dim strmensaje As String
        Dim oConn As New OleDbConnection
        Dim oCmd As New OleDbCommand
        Dim oDa As New OleDbDataAdapter
        Dim oDs As New DataSet
        Dim DtUnidades As New DataTable

        Try
            'oConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828
            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828 Se cambio la cadena de conexion para office 2010
            oConn.Open()

            oCmd.CommandText = "SELECT * FROM [Hoja1$]"
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "BalanceContable")
            DtUnidades = oDs.Tables(0)

            'HDG INC 58619	20100318
            'If DtUnidades.Rows.Count = 1 Then
            If DtUnidades.Rows.Count = 0 Then
                AlertaJS("El Archivo no tiene registros")
            Else
                Dim oBalanceContable As New BalanceContableBM
                If oDs.Tables(0).Columns.Count = 8 Then
                    If oBalanceContable.ActualizarPorExcel(DtUnidades, DatosRequest) Then
                        AlertaJS("El archivo se cargó correctamente")
                    Else
                        AlertaJS("Ocurrió un error no esperado. Revisar el archivo Excel")
                    End If
                Else
                    AlertaJS("El Archivo no tiene la estructura adecuada")
                End If
            End If
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

    Private Sub LeerExcel(ByVal strRuta)

        Try
            CargarArchivo(strRuta)
        Catch ex As OleDbException
            AlertaJS("Error al leer el archivo Excel")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString)
        End Try
    End Sub

    Private Sub CargarArchivoOrigen(ByVal strRuta As String)
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)

            'PRIMERO SE SUBE EL ARCHIVO
            iptRuta.PostedFile.SaveAs(strRuta & "\" & fInfo.Name)

            'SE VERIFICA Q EXISTA EL ARCHIVO
            If Dir(strRuta & "\" & fInfo.Name) <> "" Then
                If fInfo.Extension = ".xls" Then
                    Try
                        LeerExcel(strRuta & "\" & fInfo.Name)
                    Catch ex As Exception
                        AlertaJS(ex.Message.ToString)
                    Finally
                        File.Delete(strRuta & "\" & fInfo.Name)
                        AlertaJS("Error al leer el archivo")
                    End Try
                Else
                    If fInfo.Extension.Equals("") Then
                        AlertaJS("La ruta no es válida. Especificar ruta de un archivo")
                    Else
                        AlertaJS("El tipo de archivo no es válido")
                        File.Delete(strRuta & "\" & fInfo.Name)
                    End If
                End If
            Else
                File.Delete(strRuta & "\" & fInfo.Name)
                AlertaJS("El archivo no existe")
            End If
        Catch ex As Exception
            'AlertaJS(ex.Message.toString())
        End Try
    End Sub

#End Region

End Class
