Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System.Runtime.InteropServices.Marshal
Imports System.IO

Partial Class Modulos_Gestion_Archivos_Planos_frmInterfaseLineaxContraparteFWD
    Inherits BasePage


    Private Sub ActualizarLineasContraparteFWDPorExcel(ByVal data As DataTable, ByVal strReferencia As String, ByVal intOpc As Integer, ByRef strmensaje As String)
        If data.Rows.Count = 0 Then
            strmensaje &= "La pestaña de " & strReferencia & " no tiene registros <br />"
        Else
            Dim oLineasContraparteFWDBM As New LineasContraparteFWDBM
            Dim nCol As Integer

            If intOpc = 1 Then
                nCol = 2
            Else
                nCol = 3
            End If
            If data.Columns.Count = nCol Then
                If oLineasContraparteFWDBM.ActualizarLineasContraparteFWDPorExcel(data, DatosRequest, strmensaje, intOpc) Then
                Else
                    strmensaje &= "Los datos de " & strReferencia & " son inconsistentes <br />"
                End If
            Else
                strmensaje &= "La pestaña " & strReferencia & " no tiene la estructura adecuada <br />"
            End If
        End If
    End Sub

    Private Sub CargarArchivo(ByVal strRuta As String)

        Dim oConn As New OleDbConnection
        Dim oCmd As New OleDbCommand
        Dim oDa As New OleDbDataAdapter
        Dim oDs As New DataSet
        Dim DtFactor As New DataTable
        Dim DtLineas As New DataTable
        Dim strMensaje As String = ""

        Try
            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;"
            oConn.Open()

            oCmd.CommandText = "SELECT [Codigo_FWD], [FRP_Diario] FROM [FactorRiesgoP$] "
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "FactorRiesgoP")

            oCmd.CommandText = "SELECT [Contraparte], [Rating], [Linea_Asignada] FROM [Rating_LinAsignada$] "
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "Rating_LinAsignada")

            DtFactor = oDs.Tables("FactorRiesgoP")
            DtLineas = oDs.Tables("Rating_LinAsignada")

            ActualizarLineasContraparteFWDPorExcel(DtFactor, "FactorRiesgoP", 1, strMensaje)
            ActualizarLineasContraparteFWDPorExcel(DtLineas, "Rating_LinAsignada", 2, strMensaje)

            AlertaJS("Los datos se cargaron correctamente")
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
            oConn.Close()

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
        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            strRuta = strRuta & fInfo.Name
            'PRIMERO SE SUBE EL ARCHIVO
            'If Dir(strRuta & "\" & fInfo.Name) = "" Then strRuta = Environment.GetEnvironmentVariable("systemroot")
            'iptRuta.PostedFile.SaveAs(strRuta & "\" & fInfo.Name)
            iptRuta.PostedFile.SaveAs(strRuta)

            'SE VERIFICA Q EXISTA EL ARCHIVO
            'If Dir(strRuta & "\" & fInfo.Name) <> "" Then
            If Dir(strRuta) <> "" Then
                If fInfo.Extension = ".xls" Then
                    Try
                        'LeerExcel(strRuta & "\" & fInfo.Name)
                        LeerExcel(strRuta)
                    Catch ex As Exception
                        'AlertaJS(ex.Message.ToString())
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
            Throw ex
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            'Dim strRuta As String = (New ParametrosGeneralesBM().Listar("PREORDEN", Nothing)).Rows(0)("Valor")
            Dim strRuta As String = (New ParametrosGeneralesBM().Listar("RUTA_TEMP", Nothing)).Rows(0)("Valor")

            If Not iptRuta.Value.ToString.Trim.Equals("") Then
                CargarArchivoOrigen(strRuta)
            Else
                AlertaJS("Especifique la ruta de un archivo")
            End If
        Catch ex As Exception
            AlertaJS("Ocurrió un error al importar los datos")
        End Try
    End Sub

End Class
