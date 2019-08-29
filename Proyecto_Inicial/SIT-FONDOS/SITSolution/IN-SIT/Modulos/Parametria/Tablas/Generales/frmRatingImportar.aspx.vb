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
Partial Class Modulos_Parametria_Tablas_Generales_frmRatingImportar
    Inherits BasePage

    Private Sub ActualizarRatingPorExcel(ByVal data As DataTable, ByVal strReferencia As String, ByVal intOpc As Integer, ByRef strmensaje As String)
        If data.Rows.Count = 0 Then
            strmensaje &= "La pestaña de " & strReferencia & " no tiene registros\n"
        Else
            Dim oParametrosGenerales As New ParametrosGeneralesBM
            If data.Columns.Count = 2 Then
                If oParametrosGenerales.ActualizarRatingPorExcel(data, intOpc, DatosRequest, strmensaje) Then

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
        Dim DtUnidadesTerceros As New DataTable
        Dim DtUnidadesValores As New DataTable
        Dim strMensaje As String = ""

        Try
            'oConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828
            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828 Se cambio la cadena de conexion para office 2010
            oConn.Open()

            oCmd.CommandText = "SELECT [emisor],[rating] FROM [Emisores$] where rtrim(ltrim([emisor]))<>'' and rtrim(ltrim([Rating]))<>'' "
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "Terceros")

            oCmd.CommandText = "SELECT [código sbs],[rating] FROM [Emisiones$] where rtrim(ltrim([código sbs]))<>'' and rtrim(ltrim([Rating]))<>'' "
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "Valores")

            DtUnidadesTerceros = oDs.Tables(0)
            DtUnidadesValores = oDs.Tables(1)

            ActualizarRatingPorExcel(DtUnidadesValores, "Emisiones", 2, strMensaje)
            ActualizarRatingPorExcel(DtUnidadesTerceros, "Emisores", 1, strMensaje)

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

    Private Sub LeerExcel(ByVal strRuta)
        Try
            CargarArchivo(strRuta)
        Catch ex As OleDbException
            AlertaJS("Error al leer el archivo Excel")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
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
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("frmBusquedaRating.aspx")
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Dim strRuta As String = (New ParametrosGeneralesBM().Listar("PREORDEN", Nothing)).Rows(0)("Valor")

        If Not iptRuta.Value.ToString.Trim.Equals("") Then
            CargarArchivoOrigen(strRuta)
        Else
            AlertaJS("Especifique la ruta de un archivo")
        End If
    End Sub

End Class
