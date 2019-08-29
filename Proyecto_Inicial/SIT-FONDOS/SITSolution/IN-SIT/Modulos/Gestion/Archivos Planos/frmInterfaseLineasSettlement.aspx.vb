Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System.Runtime.InteropServices.Marshal
Imports System.IO

'''
'''***********************************************************************************************
'''********************* Developer by Carlos Hernández Ledesma ***********************************
'''***********************************************************************************************
''' 
''' <summary>
''' Pagina web que hereda de BasePage
''' </summary>
''' <remarks></remarks>
Partial Class Modulos_Gestion_Archivos_Planos_frmInterfaseLineasSettlement
    Inherits BasePage

    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Dim strRuta As String = (New ParametrosGeneralesBM().Listar("PREORDEN", Nothing)).Rows(0)("Valor")

        If (Not iptRuta.Value.ToString().Trim().Equals("")) Then
            CargarArchivoOrigen(strRuta)
        Else
            AlertaJS("Especifique la ruta de un archivo")
        End If
    End Sub

    ''' <summary>
    ''' Método que regresa al formulario web principal
    ''' </summary>
    ''' <param name="sender">evento del servidor</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnRetornar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    ''' <summary>
    ''' Metodo que actualiza en la DB todos los datos del excel
    ''' </summary>
    ''' <param name="data">Ruta del archivo excel</param>
    ''' <param name="strReferencia"></param>
    ''' <param name="strmensaje"></param>
    ''' <remarks></remarks>
    Private Sub ActualizarLineasSettlementPorExcel(ByVal data As DataTable, ByVal strReferencia As String, ByRef strmensaje As String)
        If (data.Rows.Count = 0) Then
            strmensaje &= "La pestaña de " & strReferencia & " no tiene registros\n"
        Else
            Dim oLineasSettlementBM As New LineasSettlementBM
            If (data.Columns.Count = 3) Then
                If oLineasSettlementBM.ActualizarLineasSettlementPorExcel(data, DatosRequest, strmensaje) Then
                    'strmensaje &= "Los datos de " & strReferencia & " cargaron correctamente\n"
                Else
                    strmensaje &= "Los datos de " & strReferencia & " son inconsistentes \n"
                End If
            Else
                strmensaje &= "La pestaña " & strReferencia & " no tiene la estructura adecuada\n"
            End If
        End If
    End Sub

    ''' <summary>
    ''' Metodo que crea el archvo excel selecciona del cliente a la ruta del servidor
    ''' </summary>
    ''' <param name="strRuta">Ruta Excel cliente</param>
    ''' <remarks></remarks>
    Private Sub CargarArchivoOrigen(ByVal strRuta As String)
        Dim strmensaje As String

        Try
            Dim fInfo As New FileInfo(iptRuta.Value)

            strRuta = strRuta & "\" & fInfo.Name
            'PRIMERO SE SUBE EL ARCHIVO
            'If Dir(strRuta & "\" & fInfo.Name) = "" Then strRuta = Environment.GetEnvironmentVariable("systemroot")
            iptRuta.PostedFile.SaveAs(strRuta)

            'SE VERIFICA Q EXISTA EL ARCHIVO
            If (File.Exists(strRuta)) Then
                If fInfo.Extension = ".xls" Then
                    Try
                        LeerExcel(strRuta)
                    Catch ex As Exception
                        'ManejarError(ex)
                    Finally
                        File.Delete(strRuta)
                        'strmensaje = "<script language='JavaScript'> alert('Error al leer el archivo')</script>"
                        'Page.RegisterStartupScript("Mensaje", strmensaje)
                        strmensaje = "Error al leer el archivo"
                        AlertaJS(strmensaje)
                    End Try
                Else
                    If (fInfo.Extension.Equals("")) Then
                        'strmensaje = "<script language='JavaScript'> alert('La ruta no es válida. Especificar ruta de un archivo')</script>"
                        'Page.RegisterStartupScript("Mensaje", strmensaje)
                        strmensaje = "La ruta no es válida. Especificar ruta de un archivo"
                        AlertaJS(strmensaje)
                    Else
                        File.Delete(strRuta)
                        'strmensaje = "<script language='JavaScript'> alert('El tipo de archivo no es válido')</script>"
                        'Page.RegisterStartupScript("Mensaje", strmensaje)
                        strmensaje = "El tipo de archivo no es válido"
                        AlertaJS(strmensaje)
                    End If
                End If
            Else
                File.Delete(strRuta)
                'strmensaje = "<script language='JavaScript'> alert('El archivo no existe')</script>"
                'Page.RegisterStartupScript("Mensaje", strmensaje)
                strmensaje = "El archivo no existe"
                AlertaJS(strmensaje)
            End If
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    ''' <summary>
    ''' Metodo que ejecuta el carga del archivo
    ''' </summary>
    ''' <param name="strRuta">Ruta excel del cliente</param>
    ''' <remarks></remarks>
    Private Sub LeerExcel(ByVal strRuta As String)
        
        Try
            CargarArchivo(strRuta)
        Catch ex As OleDbException
            AlertaJS("Error al leer el archivo Excel")
        Catch ex As Exception
            AlertaJS(ex.Message.ToString())
        End Try
    End Sub

    ''' <summary>
    ''' Metodo que recoge la información del excel y lo inserta en la DB del servidor
    ''' </summary>
    ''' <param name="strRuta">Ruta del archivo excel del cliente</param>
    ''' <remarks></remarks>
    Private Sub CargarArchivo(ByVal strRuta As String)
        'Dim oConn As New OleDbConnection
        Dim oCmd As New OleDbCommand
        Dim oDa As New OleDbDataAdapter
        Dim oDs As New DataSet
        Dim strMensaje As String = ""
        Try

            'Metodo que ejecuta la connexión y al momento de terminar cierra y limpia los recursos
            Using oConn As New OleDbConnection With { _
                .ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;"}

                oConn.Open()
                oCmd.CommandText = "SELECT [Contraparte], [Nombre], [Linea] FROM [LineasSettlement$] "
                oCmd.Connection = oConn
                oDa.SelectCommand = oCmd
                oDa.Fill(oDs, "LineasSettlement")
                ActualizarLineasSettlementPorExcel(oDs.Tables(0), "LineasSettlement", strMensaje)
                AlertaJS(strMensaje)
                oConn.Close()

            End Using


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
        End Try

    End Sub
End Class
