Imports Sit.BusinessEntities
Imports Sit.BusinessLayer
Imports System.Data
Imports System
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports System.Data.OleDb
Imports Microsoft.Office

Partial Class Modulos_Gestion_Archivos_Planos_frmInterfaseReporteVLMidas
    Inherits BasePage

    Private RutaDestino As String = String.Empty
    Dim strmensaje As String
    Dim sFileName As String
    Dim errorNoExiste As Boolean = False

    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        CargarReporteVLMidas()
    End Sub

    Private Sub CargarReporteVLMidas()
        errorNoExiste = False
        msgError.Visible = False

        Dim dtReporteVLMidas As New DataTable
        sFileName = Myfile.Text
        hfRutaDestino.Value = "D:\"

        Try
            Dim fInfo As New FileInfo(iptRuta.Value)
            RutaDestino = hfRutaDestino.Value
            If fInfo.Extension = ".xls" Then
                iptRuta.PostedFile.SaveAs(RutaDestino & "\" & fInfo.Name)
                If Dir(RutaDestino & "\" & fInfo.Name) <> "" Then
                    Try
                        loadRange(RutaDestino & "\" & fInfo.Name, "Hoja1", "A7:Z5000", dtReporteVLMidas)
                    Catch ex As Exception
                        AlertaJS(ex.ToString)
                    Finally
                        File.Delete(RutaDestino & "\" & fInfo.Name)
                    End Try
                Else
                    File.Delete(RutaDestino & "\" & fInfo.Name)
                End If
            Else
                If fInfo.Extension.Equals("") Then
                    AlertaJS("La ruta no es válida. Especificar ruta de un archivo")
                Else
                    File.Delete(RutaDestino & "\" & fInfo.Name)
                    AlertaJS("El tipo de archivo no es válido")
                End If
            End If

            Dim ReporteVLMidasBM As New ReporteVLMidasBM
            Dim ReporteVLMidasBE As New ReporteVLMidasBE
            Dim Resultado As String = String.Empty

            For Each dr As DataRow In dtReporteVLMidas.Rows
                ReporteVLMidasBE.Fecha = dr(3).ToString.Substring(6, 4) + dr(3).ToString.Substring(3, 2) + dr(3).ToString.Substring(0, 2)
                ReporteVLMidasBM.Eliminar(ReporteVLMidasBE, DatosRequest)
                Exit For
            Next

            For Each dr As DataRow In dtReporteVLMidas.Rows
                ReporteVLMidasBE.TipoRegistro = dr(0)
                ReporteVLMidasBE.Administradora = dr(1)
                ReporteVLMidasBE.Fondo = dr(2)
                ReporteVLMidasBE.Fecha = dr(3).ToString.Substring(6, 4) + dr(3).ToString.Substring(3, 2) + dr(3).ToString.Substring(0, 2)
                ReporteVLMidasBE.TipoCodigoValor = dr(4)
                ReporteVLMidasBE.CodigoValor = dr(5)
                ReporteVLMidasBE.IdentificadorOperacion = dr(6)
                ReporteVLMidasBE.FormaValorizacion = dr(7)
                ReporteVLMidasBE.MontoNominal = dr(8)
                ReporteVLMidasBE.PrecioTasa = dr(9)
                ReporteVLMidasBE.TipoCambio = dr(10)
                ReporteVLMidasBE.MontoFinal = dr(11)
                ReporteVLMidasBE.MontoInversion = dr(12)
                If dr(13).ToString.Trim <> "" Then
                    ReporteVLMidasBE.FechaOperacion = dr(13).ToString.Substring(6, 4) + dr(13).ToString.Substring(3, 2) + dr(13).ToString.Substring(0, 2)
                Else
                    ReporteVLMidasBE.FechaOperacion = 0
                End If

                If dr(14).ToString.Trim <> "" Then
                    ReporteVLMidasBE.FechaInicioPagaIntereses = dr(14).ToString.Substring(6, 4) + dr(14).ToString.Substring(3, 2) + dr(14).ToString.Substring(0, 2)
                Else
                    ReporteVLMidasBE.FechaInicioPagaIntereses = 0
                End If

                If dr(15).ToString.Trim <> "" Then
                    ReporteVLMidasBE.FechaVencimiento = dr(15).ToString.Substring(6, 4) + dr(15).ToString.Substring(3, 2) + dr(15).ToString.Substring(0, 2)
                Else
                    ReporteVLMidasBE.FechaVencimiento = 0
                End If
                ReporteVLMidasBE.InteresesCorrido = dr(16)
                ReporteVLMidasBE.InteresesGanado = dr(17)
                ReporteVLMidasBE.Ganancia_Perdida = dr(18)
                ReporteVLMidasBE.Valorizacion = dr(19)
                ReporteVLMidasBE.TipoInstrumento = dr(20)
                ReporteVLMidasBE.Clasificacion = dr(21)
                ReporteVLMidasBE.ComisionContado = dr(22)
                ReporteVLMidasBE.ComisionPlazo = dr(23)
                ReporteVLMidasBE.TIR = dr(24)
                ReporteVLMidasBE.Duracion = dr(25)
                Resultado = ReporteVLMidasBM.Insertar(ReporteVLMidasBE, DatosRequest)
            Next

            If Resultado = "True" Then
                AlertaJS("Se realizó la carga del VL Midas Correctamente")
            End If

        Catch ex As Exception
            AlertaJS(ex.ToString)
        End Try

    End Sub

    Private Sub loadRange(ByVal sFileName As String, ByVal sSheetName As String, ByVal sRange As String, ByRef dt As DataTable)
        Try
            If System.IO.File.Exists(sFileName) Then
                Dim objDataSet As System.Data.DataSet
                Dim objDataAdapter As System.Data.OleDb.OleDbDataAdapter
                Dim sCs As String = ""
                If System.Configuration.ConfigurationManager.AppSettings("Ambiente").ToString() = "Desarrollo" Then
                    sCs = "provider=Microsoft.Jet.OLEDB.4.0; " & "data source=" & sFileName & "; Extended Properties=Excel 8.0;"
                Else
                    sCs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & sFileName & "; Extended Properties= Excel 8.0;"
                End If
                Dim objOleConnection As System.Data.OleDb.OleDbConnection
                objOleConnection = New System.Data.OleDb.OleDbConnection(sCs)
                Dim sSql As String = "select * from " & "[" & sSheetName & "$" & sRange & "]"
                objDataAdapter = New System.Data.OleDb.OleDbDataAdapter(sSql, objOleConnection)
                objDataSet = New System.Data.DataSet
                objDataAdapter.Fill(objDataSet)
                dt = objDataSet.Tables(0)
                objOleConnection.Close()
            Else
                AlertaJS("No se ha encontrado el archivo: " & sFileName)
            End If
        Catch ex As Exception
            AlertaJS(Replace(ex.Message, "'", ""))
        End Try
    End Sub

End Class
