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
''' Página heredada de BasePage
''' </summary>
''' <remarks></remarks>
Partial Class Modulos_Gestion_Archivos_Planos_frmInterfaseLineasContraparte
    Inherits BasePage

    ''' <summary>
    ''' Archivo que permite insertar en las tablas de la DB
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="strReferencia"></param>
    ''' <param name="strmensaje"></param>
    ''' <remarks>No retorna valores</remarks>
    Private Sub ActualizarLineasContrapartePorExcel(ByVal data As DataTable, ByVal strReferencia As String, ByRef strmensaje As String)
        If data.Rows.Count = 0 Then
            strmensaje &= "La pestaña de " & strReferencia & " no tiene registros\n"
        Else
            Dim oLineasContraparteBM As New LineasContraparteBM
            If data.Columns.Count = 3 Then
                If oLineasContraparteBM.ActualizarLineasContrapartePorExcel(data, DatosRequest, strmensaje) Then
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
    ''' Metodo del servidor que retorna a la página principal
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnRetornar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Response.Redirect("../../../frmDefault.aspx")
    End Sub

    ''' <summary>
    ''' Esta opción permite realizar la lectura del archio excel
    ''' </summary>
    ''' <param name="strRuta">Especifique la ruta del archivo a improtar</param>
    ''' <remarks></remarks>
    Private Sub CargarArchivo(ByVal strRuta As String)
        Dim oConn As New OleDbConnection
        Dim oCmd As New OleDbCommand
        Dim oDa As New OleDbDataAdapter
        Dim oDs As New DataSet
        Dim strMensaje As String = ""
        Try
            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828 Se cambio la cadena de conexion para office 2010
            oConn.Open()
            oCmd.CommandText = "SELECT [Contraparte], [Nombre], [Linea] FROM [LineasContraparte$] "
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "LineasContraparte")
            ActualizarLineasContrapartePorExcel(oDs.Tables(0), "LineasContraparte", strMensaje)
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
            oConn.Close()

        Catch ex As Exception
            AlertaJS("Error al leer el archivo Excel")
            oConn.Close()
        End Try

    End Sub

    ''' <summary>
    ''' Metodo para acceder a la opción de carga de archivo excel
    ''' </summary>
    ''' <param name="strRuta">Especificar la ruta del archivo</param>
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
    ''' Metodo de creación del archivo excel en la ruta especificada del servidor, si da error se elimina del servidor
    ''' </summary>
    ''' <param name="strRuta">Ruta del archivo cliente</param>
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
            If (System.IO.File.Exists(strRuta)) Then
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
                    If fInfo.Extension.Equals("") Then
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
    ''' Evento que se ejecuta en el servidor 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            Dim strRuta As String = (New ParametrosGeneralesBM().Listar("PREORDEN", Nothing)).Rows(0)("Valor")

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
