Imports Sit.BusinessLayer
Imports System.Data
Imports System.IO

Imports Microsoft.VisualBasic


Public Class HelpRistra

    Public Shared Function GenerarRistraContable(ByVal portafolio As String, ByVal fechaInicio As String, ByVal fechaFin As String, ByVal path As String, ByVal request As DataSet) As String
        Dim result As String

        Dim errorGenerar As Boolean = False
        Dim i As Integer
        Dim sFileName As String
        Dim cadenaVacia As String = ""
        Dim nombreArchivo As String = ""
        Dim arrCadenas() As String
        Dim parteEnt As String
        Dim parteDec As String
        Dim cadena As String
        Dim parteDecAux As String = "0"
        Dim FechaProceso As Date = Date.Today
        Dim FechaArchivoRistra As String
        Dim decFechaProceso As Decimal
        Dim extension As String
        Dim registro As String = ""
        Dim dsConsulta As DataSet
        Dim oAsientoContableBM As New AsientoContableBM

        Try
            decFechaProceso = CType(FechaProceso.ToString("yyyyMMdd"), Decimal)

            '#ERROR#
            If portafolio.Equals("ADMINISTRA") Then
                FechaArchivoRistra = DateTime.Now.ToString("yyyyMMdd")
            Else
                FechaArchivoRistra = RetornarFechaContableUtilAnterior(DateTime.Now)
            End If

            dsConsulta = oAsientoContableBM.Seleccionar_RistraContable(portafolio, UIUtility.ConvertirFechaaDecimal(fechaInicio), UIUtility.ConvertirFechaaDecimal(fechaFin), FechaArchivoRistra, request)
            extension = "txt"

            If dsConsulta.Tables(0).Rows.Count > 0 Then
                nombreArchivo = "pefp_ha_fix_f" + FechaArchivoRistra.Substring(2) + "_intapl"
                '#ERROR#
                If portafolio.Equals("ADMINISTRA") Then
                    nombreArchivo = nombreArchivo + "1_sit_a9" + "." + extension
                Else
                    'nombreArchivo = nombreArchivo + "2_sit_f" + obtenerCodigoFondo(ddlPortafolio.SelectedValue) + "." + extension
                    nombreArchivo = nombreArchivo + "2_sit_f" + portafolio + "." + extension
                End If

                sFileName = path + nombreArchivo
                '-----------------------------------Escribir Archivo-----------------------------------
                Dim tw As TextWriter
                tw = New StreamWriter(sFileName)
                'Obtener el detalle del archivo
                i = 0
                While (i < dsConsulta.Tables(0).Rows.Count())
                    registro = ""

                    'EMPRESA - 4 espacios (124)
                    cadena = dsConsulta.Tables(0).Rows(i).Item("EMPRESA").ToString().PadRight(4, " ")
                    registro = registro + cadena

                    'CLVINT - 3 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("CLVINT").ToString().PadRight(3, " ")
                    registro = registro + cadena

                    'FECON - 8 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("FECON").ToString().PadRight(8, " ")
                    registro = registro + cadena

                    'FECOP - 8 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("FECOP").ToString().PadRight(8, " ")
                    registro = registro + cadena

                    ''FIJOS_INICIO - 37/51 espacios
                    registro = registro + cadenaVacia.PadRight(37, " ")

                    ''HACDVISA - 3/51 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("HACDVISA").ToString().PadRight(3, " ")
                    registro = registro + cadena

                    ''FIJOS_FIN - 11/51 espacios
                    registro = registro + cadenaVacia.PadRight(11, " ")

                    'RGF 20080702 FIJOS - 51 espacios
                    'registro = registro + cadenaVacia.PadRight(51, " ") 'RGF 20081127

                    'VARIOS - 18 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("VARIOS").ToString().PadRight(18, " ")
                    registro = registro + cadena

                    'CLVAUT - 6 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("CLVAUT").ToString().PadRight(6, " ")
                    registro = registro + cadena

                    'CEOPE - 4 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("CEOPE").ToString().PadRight(4, " ")
                    registro = registro + cadena

                    'CEORIGEN - 4 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("CEORIGEN").ToString().PadRight(4, " ")
                    registro = registro + cadena

                    'CEDESTIN - 4 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("CEDESTIN").ToString().PadRight(4, " ")
                    registro = registro + cadena

                    'NUMOD - 7 numerico
                    cadena = dsConsulta.Tables(0).Rows(i).Item("NUMOD").ToString().PadLeft(7, "0")
                    registro = registro + cadena

                    'NUMOH - 7 numerico (127 >)
                    cadena = dsConsulta.Tables(0).Rows(i).Item("NUMOH").ToString().PadLeft(7, "0")
                    registro = registro + cadena

                    'IMPD - 13,2 espacios. (231 <)
                    arrCadenas = Split(dsConsulta.Tables(0).Rows(i).Item("IMPD").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                    parteEnt = arrCadenas(0).Replace("-", "").PadLeft(13, "0")
                    If arrCadenas.Length = 2 Then
                        parteDec = arrCadenas(1).PadRight(2, "0")
                        If parteDec.Length > 2 Then
                            parteDec = parteDec.Substring(0, 2) 'RGF 20081118
                        End If
                    Else
                        parteDec = parteDecAux.PadRight(2, "0")
                    End If
                    registro = registro + parteEnt + parteDec

                    'IMPH - 13,2 espacios
                    arrCadenas = Split(dsConsulta.Tables(0).Rows(i).Item("IMPH").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                    parteEnt = arrCadenas(0).Replace("-", "").PadLeft(13, "0")
                    If arrCadenas.Length = 2 Then
                        parteDec = arrCadenas(1).PadRight(2, "0")
                        If parteDec.Length > 2 Then
                            parteDec = parteDec.Substring(0, 2) 'RGF 20081118
                        End If
                    Else
                        parteDec = parteDecAux.PadRight(2, "0")
                    End If
                    registro = registro + parteEnt + parteDec

                    'IMPDDIV - 13,2 espacios
                    arrCadenas = Split(dsConsulta.Tables(0).Rows(i).Item("IMPDDIV").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                    parteEnt = arrCadenas(0).Replace("-", "").PadLeft(13, "0")
                    If arrCadenas.Length = 2 Then
                        parteDec = arrCadenas(1).PadRight(2, "0")
                        If parteDec.Length > 2 Then
                            parteDec = parteDec.Substring(0, 2) 'RGF 20081229
                        End If
                    Else
                        parteDec = parteDecAux.PadRight(2, "0")
                    End If
                    registro = registro + parteEnt + parteDec

                    'IMPHDIV - 13,2 espacios
                    arrCadenas = Split(dsConsulta.Tables(0).Rows(i).Item("IMPHDIV").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                    parteEnt = arrCadenas(0).Replace("-", "").PadLeft(13, "0")
                    If arrCadenas.Length = 2 Then
                        parteDec = arrCadenas(1).PadRight(2, "0")
                        If parteDec.Length > 2 Then
                            parteDec = parteDec.Substring(0, 2) 'RGF 20081229
                        End If
                    Else
                        parteDec = parteDecAux.PadRight(2, "0")
                    End If
                    registro = registro + parteEnt + parteDec

                    'IMPDCUO - 9,8 espacios (163)
                    arrCadenas = Split(dsConsulta.Tables(0).Rows(i).Item("IMPDCUO").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                    parteEnt = arrCadenas(0).Replace("-", "").PadLeft(9, "0")
                    If arrCadenas.Length = 2 Then
                        parteDec = arrCadenas(1).PadRight(8, "0")
                        If parteDec.Length > 8 Then
                            parteDec = parteDec.Substring(1, 8)
                        End If
                    Else
                        parteDec = parteDecAux.PadRight(8, "0")
                    End If
                    registro = registro + parteEnt + parteDec

                    'IMPHCUO - 9,8 espacios
                    arrCadenas = Split(dsConsulta.Tables(0).Rows(i).Item("IMPHCUO").ToString().Replace(UIUtility.DecimalSeparator(), "."), ".")
                    parteEnt = arrCadenas(0).Replace("-", "").PadLeft(9, "0")
                    If arrCadenas.Length = 2 Then
                        parteDec = arrCadenas(1).PadRight(8, "0")
                        If parteDec.Length > 8 Then
                            parteDec = parteDec.Substring(1, 8)
                        End If
                    Else
                        parteDec = parteDecAux.PadRight(8, "0")
                    End If
                    registro = registro + parteEnt + parteDec

                    'CORRECCIO - 1 espacio (113)
                    cadena = dsConsulta.Tables(0).Rows(i).Item("CORRECCIO").ToString().PadRight(1, " ")
                    registro = registro + cadena

                    'NUMDO - 12 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("NUMDO").ToString().PadRight(12, " ")
                    registro = registro + cadena

                    'CLACON - 3 espacios 
                    cadena = dsConsulta.Tables(0).Rows(i).Item("CLACON").ToString().PadRight(3, " ")
                    registro = registro + cadena

                    'DESCON - 14 espacios 
                    cadena = dsConsulta.Tables(0).Rows(i).Item("DESCON").ToString().PadRight(14, " ")
                    registro = registro + cadena

                    'TIPOCON - 1 espacio
                    cadena = dsConsulta.Tables(0).Rows(i).Item("TIPOCON").ToString().PadRight(1, " ")
                    registro = registro + cadena

                    'OBSERV - 30 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("OBSERV").ToString().PadRight(30, " ")
                    registro = registro + cadena

                    'SANCTCCC - 18 espacios
                    'RGF 20080716
                    cadena = dsConsulta.Tables(0).Rows(i).Item("SANCTCCC").ToString().PadRight(18, " ")
                    registro = registro + cadena

                    'ARAPORIG - 3 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("ARAPORIG").ToString().PadRight(3, " ")
                    registro = registro + cadena

                    'ARAPDEST - 3 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("ARAPDEST").ToString().PadRight(3, " ")
                    registro = registro + cadena

                    'OBSERV3 - 6 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("OBSERV3").ToString().PadRight(6, " ")
                    registro = registro + cadena

                    'RESERVAT - 4 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("RESERVAT").ToString().PadRight(4, " ")
                    registro = registro + cadena

                    'HACTRGEN - 4 espacios
                    cadena = dsConsulta.Tables(0).Rows(i).Item("HACTRGEN").ToString().PadRight(4, " ")
                    registro = registro + cadena

                    'HAYCOCAI - 1 espacio
                    cadena = dsConsulta.Tables(0).Rows(i).Item("HAYCOCAI").ToString().PadRight(1, " ")
                    registro = registro + cadena

                    'HAYCTORD - 1 espacio
                    cadena = dsConsulta.Tables(0).Rows(i).Item("HAYCTORD").ToString().PadRight(1, " ")
                    registro = registro + cadena

                    'NORDEN - 5 ceros
                    cadena = dsConsulta.Tables(0).Rows(i).Item("NORDEN").ToString().PadLeft(5, "0")
                    registro = registro + cadena

                    'DATOSCONV - 16 espacios
                    registro = registro + cadenaVacia.PadRight(16, " ")

                    'Escribir Detalle
                    If (registro.Length = 340) Then 'RGF 20080716
                        tw.WriteLine(registro)
                        i = i + 1
                    Else
                        errorGenerar = True
                        Exit While
                    End If
                End While

                tw.Close()

                If Not errorGenerar Then
                    result = "Se ha generado correctamente el archivo " + nombreArchivo
                Else
                    result = "Error al generar el archivo " + nombreArchivo
                End If
            Else
                result = "No existen datos para generar el archivo " + nombreArchivo
            End If
        Catch ex As Exception
            result = ex.Message.ToString()
        End Try
    End Function

    Public Shared Function RevertirRistraContable(ByVal portafolio As String, ByVal fechaContable As String, ByVal path As String, ByVal request As DataSet) As String
        Dim result As String

        Dim oAsientoContableBM As AsientoContableBM
        Dim FechaOperacionDesde As String = ""
        Dim FechaOperacionHasta As String = ""
        Dim FechaArchivoRistra As String = ""
        Dim errorGenerar As Boolean = False
        Dim nombreArchivo As String = ""
        Dim parteDecAux As String = "0"
        Dim extension As String = "txt"
        Dim cadenaVacia As String = ""
        Dim sFileName As String = ""
        Dim parteEnt As String = ""
        Dim parteDec As String = ""
        Dim registro As String = ""
        Dim cadena As String = ""
        Dim oSW As StreamWriter
        Try
            oAsientoContableBM = New AsientoContableBM

            FechaArchivoRistra = Microsoft.VisualBasic.Right(fechaContable, 4) & Mid(fechaContable, 4, 2) & Microsoft.VisualBasic.Left(fechaContable, 2)

            nombreArchivo = "pefp_ha_fix_f" + FechaArchivoRistra.Substring(2) + "_intapl2_teso_f" + portafolio + "." + extension
            'Ubicación de archivo nuevo

            sFileName = fnBuscarRuta(request) + nombreArchivo
            '************************Leer archivo con el error y escribir en archivo nuevo***********************
            Dim strContext As String = ""
            Dim Auxiliar As String = ""
            Dim oSR As StreamReader
            Try
                oSW = New StreamWriter(sFileName, False)
                oSR = New StreamReader(path)
                While Not strContext Is Nothing
                    strContext = oSR.ReadLine
                    If Trim(strContext) <> "" Then
                        'Fecha de asiento de Ristra con error
                        Auxiliar = Mid(strContext, 8, 8)
                        'Rangos de fechas
                        If FechaOperacionDesde = "" And FechaOperacionHasta = "" Then
                            FechaOperacionDesde = Auxiliar
                            FechaOperacionHasta = Auxiliar
                        ElseIf Auxiliar <> "" Then
                            If FechaOperacionDesde > Auxiliar Then
                                FechaOperacionDesde = Auxiliar
                            End If
                            If FechaOperacionHasta < Auxiliar Then
                                FechaOperacionHasta = Auxiliar
                            End If
                        End If

                        strContext = Left(strContext, 124) + Mid(strContext, 140, 15) + Mid(strContext, 125, 15) + Mid(strContext, 155)

                        strContext = Left(strContext, 15) + FechaArchivoRistra + Mid(strContext, 24, 87) + Mid(strContext, 118, 7) + Mid(strContext, 111, 7) + Mid(strContext, 125)
                        oSW.WriteLine(strContext)
                    End If
                End While
            Catch ex As Exception
                result = ex.Message.ToString()
            Finally
                oSR.Close()
                oSR = Nothing
                GC.Collect()
            End Try

            If Not errorGenerar Then
                result = "Se ha generado correctamente el archivo nuevo " + nombreArchivo
            Else
                result = "Error al generar el archivo nuevo " + nombreArchivo
            End If
        Catch ex As Exception
            result = "Error al revertir ristras contable: " + ex.Message
        Finally
            oAsientoContableBM = Nothing
            oSW.Close()
            oSW = Nothing
            GC.Collect()
        End Try
        Return result
    End Function

    'RGF 20090203 Segun Susana Yin: en la PU, solo es feriado el 01 de Enero y el 25 de Diciembre
    Public Shared Function RetornarFechaContableUtilAnterior(ByVal fecha As DateTime) As Decimal
        While True
            fecha = fecha.AddDays(-1)
            If Not (fecha.DayOfWeek = DayOfWeek.Saturday Or fecha.DayOfWeek = DayOfWeek.Sunday Or fecha.ToString("MMdd").Equals("1225") Or fecha.ToString("MMdd").Equals("0101")) Then
                Exit While
            End If
        End While
        Return fecha.ToString("yyyyMMdd")
    End Function

    Public Shared Function fnBuscarRuta(ByVal request As DataSet) As String
        Dim dtConsulta As DataTable
        Dim strRuta As String = ""
        dtConsulta = New ParametrosGeneralesBM().SeleccionarPorFiltro("RISTRACONT", "Ruta de Ristra Contable", "", "", request)
        If dtConsulta.Rows.Count > 0 Then
            strRuta = CType(dtConsulta.Rows(0).Item("Valor"), String)
        End If
        Return strRuta
    End Function
End Class
