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

Partial Class Modulos_Parametria_Tablas_Valores_frmPatrimonioFideicomisoImportar
    Inherits BasePage

#Region "/* Metodos de la Página */"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

        Catch ex As Exception
            AlertaJS("Ocurrió un error al cargar la Página")
        End Try
    End Sub

    Private Sub btnRetornar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetornar.Click
        Try
            Response.Redirect("frmBusquedaPatrimonioFideicomiso.aspx")
        Catch ex As Exception
            AlertaJS("Ocurrió un error al Retornar")
        End Try        
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Try
            Dim strRuta As String = (New ParametrosGeneralesBM().Listar("PREORDEN", Nothing)).Rows(0)("Valor")
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

#Region "/* Metodos de la Página */"

    Private Sub ActualizarPatrimonioFideicomisoPorExcel(ByVal data As DataTable, ByVal strReferencia As String, ByRef strmensaje As String)
        If data.Rows.Count = 0 Then
            strmensaje &= "La pestaña de " & strReferencia & " no tiene registros\n"
        Else
            Dim oPatrimonioFideicomisoBM As New PatrimonioFideicomisoBM
            If data.Columns.Count = 8 Then
                If oPatrimonioFideicomisoBM.ActualizarPatrimonioFideicomisoPorExcel(data, DatosRequest, strmensaje) Then

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
            'oConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828
            oConn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strRuta & "; Extended Properties= Excel 8.0;" 'CMB Migracion 20120828 Se cambio la cadena de conexion para office 2010
            oConn.Open()

            oCmd.CommandText = "SELECT DISTINCT [Cod], [Total Activo], [Total Pasivo], [Total Patrimonio], [Fecha de vigencia], [Factor Riesgo], [Factor Liquidez], [Situación] FROM [PatrimonioFideicomiso$] "
            oCmd.Connection = oConn
            oDa.SelectCommand = oCmd
            oDa.Fill(oDs, "PatrimonioFideicomiso")

            Dim dt1 As DataTable, dt2 As DataTable
            Dim sCod1 As String, sCod2 As String, nCon As Long
            dt1 = oDs.Tables(0)
            dt2 = oDs.Tables(0)

            For Each filaLinea1 As DataRow In dt1.Rows
                sCod1 = filaLinea1(0).ToString().Trim()
                nCon = 0
                For Each filaLinea2 As DataRow In dt2.Rows
                    sCod2 = filaLinea2(0).ToString().Trim()
                    If sCod1 = sCod2 Then
                        nCon += 1
                    End If
                Next
                If nCon > 1 Then
                    AlertaJS("No se ha podido cargar el archivo, si se han modificado los montos activo, pasivo y patrimonio o fecha de vigencia de algún emisor de un mismo patrimonio fideicomiso estos deben ser iguales.")
                    oConn.Close()
                    Exit Sub
                End If
            Next

            ActualizarPatrimonioFideicomisoPorExcel(oDs.Tables(0), "PatrimonioFideicomiso", strMensaje)
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

    Private Sub LeerExcel(ByVal strRuta As String)
        Try
            CargarArchivo(strRuta)
        Catch ex As OleDbException
            AlertaJS("Error al leer el archivo Excel")
        Catch ex As Exception
            AlertaJS(ex.Message)
        End Try
    End Sub

    Private Sub CargarArchivoOrigen(ByVal strRuta As String)
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
                        AlertaJS(ex.Message)
                    Finally
                        File.Delete(strRuta & "\" & fInfo.Name)
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
            AlertaJS(ex.ToString())
        End Try
    End Sub

#End Region

End Class
