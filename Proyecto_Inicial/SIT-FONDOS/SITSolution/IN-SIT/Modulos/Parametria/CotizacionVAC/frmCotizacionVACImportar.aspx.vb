Imports Sit.BusinessEntities
Imports SIT.BusinessLayer
Imports System.Data.SqlTypes
Imports System.Collections
Imports System.Data
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System.Runtime.InteropServices.Marshal
Imports System
Imports System.IO
Partial Class Modulos_Parametria_CotizacionVAC_frmCotizacionVACImportar
    Inherits BasePage

#Region "/* Métodos de la página*/"

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaCotizacionVAC.aspx")
        Catch ex As Exception
            AlertaJS(ex.Message.toString())
        End Try        
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            Dim strRuta As String = (New ParametrosGeneralesBM().Listar("PREORDEN", Nothing)).Rows(0)("Valor")
            If Not iptRuta.Value.ToString.Trim.Equals("") Then
                CargarArchivoOrigen(strRuta)
            Else
                Dim strmensaje As String
                strmensaje = "Especifique la ruta de un archivo"
                AlertaJS(strmensaje)
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.toString())
        End Try        
    End Sub

#End Region

#Region "/* Métodos personalizados*/"

    Private Sub ActualizarCotizacionVACPorExcel(ByVal data As DataTable, ByVal strReferencia As String, ByRef strmensaje As String)
        If data.Rows.Count = 0 Then
            strmensaje &= "La pestaña de " & strReferencia & " no tiene registros\n"
        Else
            Dim oCotizacionVACBM As New CotizacionVACBM
            If data.Columns.Count = 3 Then
                If oCotizacionVACBM.ActualizarCotizacionVACPorExcel(data, DatosRequest, strmensaje) Then
                    'strmensaje &= "Los datos de " & strReferencia & " cargaron correctamente\n"
                Else
                    strmensaje &= "Los datos de " & strReferencia & " son inconsistentes \n"
                End If
            Else
                strmensaje &= "La pestaña " & strReferencia & " no tiene la estructura adecuada\n"
            End If
        End If
    End Sub

    Private Sub CargarArchivo(ByVal strRuta As String)
        Dim oConn As New OleDbConnection
        Dim oCmd As New OleDbCommand
        Dim oDa As New OleDbDataAdapter
        Dim oDs As New DataSet
        Dim strMensaje As String = ""

        Try
            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828 Se cambio la cadena de conexion para office 2010
            oConn.Open()

            oCmd.CommandText = "SELECT [Código de indicador], [Fecha], [Valor] FROM [Indicadores$] where rtrim(ltrim([Código de indicador]))<>'' and rtrim(ltrim([Fecha]))<>'' and rtrim(ltrim([Valor]))<>'' "
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "Indicadores")

            ActualizarCotizacionVACPorExcel(oDs.Tables(0), "Indicadores", strMensaje)

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

    Private Sub LeerExcel(ByVal strRuta As String)
        Try
            CargarArchivo(strRuta)
        Catch ex As OleDbException
            AlertaJS("Error al leer el archivo Excel")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub CargarArchivoOrigen(ByVal strRuta As String)
        Dim strmensaje As String

        Try
            Dim fInfo As New FileInfo(iptRuta.Value)

            'PRIMERO SE SUBE EL ARCHIVO
            If Dir(strRuta & "\" & fInfo.Name) = "" Then strRuta = Environment.GetEnvironmentVariable("systemroot")
            iptRuta.PostedFile.SaveAs(strRuta & "\" & fInfo.Name)

            'SE VERIFICA Q EXISTA EL ARCHIVO
            If Dir(strRuta & "\" & fInfo.Name) <> "" Then
                If fInfo.Extension = ".xls" Then
                    Try
                        LeerExcel(strRuta & "\" & fInfo.Name)
                    Catch ex As Exception
                        AlertaJS(ex.Message.toString())
                    Finally
                        File.Delete(strRuta & "\" & fInfo.Name)
                    End Try
                Else
                    If fInfo.Extension.Equals("") Then
                        strmensaje = "La ruta no es válida. Especificar ruta de un archivo"
                        AlertaJS(strmensaje)
                    Else
                        File.Delete(strRuta & "\" & fInfo.Name)
                        strmensaje = "El tipo de archivo no es válido"
                        AlertaJS(strmensaje)
                    End If
                End If
            Else
                File.Delete(strRuta & "\" & fInfo.Name)
                strmensaje = "El archivo no existe"
                AlertaJS(strmensaje)
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.toString())
        End Try
    End Sub

#End Region

End Class
