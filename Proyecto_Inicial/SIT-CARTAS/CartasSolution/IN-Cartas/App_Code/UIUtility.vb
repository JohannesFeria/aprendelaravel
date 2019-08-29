Imports System.IO
Imports Cartas.BusinessLayer
Imports Cartas.BusinessEntities
Imports System.Text
Imports System.Xml
Imports System.Collections.Generic
Imports System.Data
Imports ParametrosSIT
Imports System.Net.Mail
Imports System
Imports System.Web.UI.WebControls
Imports System.Collections
Imports System.Globalization
Public Class UIUtility
    Public Shared Function ConvertirFechaaDecimal(ByVal fecha As String) As Decimal

        If fecha.Length = 0 Then
            Return 0
        ElseIf fecha.Length = 8 Then
            Return fecha
        End If
        Return Convert.ToDecimal(fecha.Substring(6, 4) + fecha.Substring(3, 2) + fecha.Substring(0, 2))
    End Function
    Public Shared Function MostrarResultadoBusqueda(ByVal iCount As Integer) As String
        Dim resultado As String = String.Empty
        resultado = Constantes.M_STR_TEXTO_RESULTADOS & iCount.ToString("#0") 'MC
        Return resultado
    End Function
    Public Shared Sub InsertarElementoSeleccion(ByVal sel As DropDownList, Optional ByVal valor As String = "", Optional ByVal texto As String = "--SELECCIONE--")
        sel.Items.Insert(0, New System.Web.UI.WebControls.ListItem(texto, valor))
    End Sub
    Public Shared Function ConvertirFechaaString(ByVal fecha As Decimal) As String
        Dim strfecha As String = Convert.ToString(fecha)
        If strfecha.Length = 8 Then Return strfecha.Substring(6, 2) + "/" + strfecha.Substring(4, 2) + "/" + strfecha.Substring(0, 4)
        Return ""
    End Function
    Public Shared Function EnviarMail(ByVal toUser As String, ByVal ccUser As String, ByVal asunto As String, ByVal message As String, ByVal DatosRequest As DataSet, Optional ByVal sAttach As String = "") As Boolean
        Dim correo As MailMessage = New MailMessage()
        Dim smtp As SmtpClient = New SmtpClient()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim lTablaGeneral As TablaGeneralBEList
        Dim fromUser As String = String.Empty
        'Se recogen los datos del 
        lTablaGeneral = oParametrosGeneralesBM.SeleccionarPorFiltro(EMAIL_FORM, "", "", "")
        fromUser = lTablaGeneral.Item(0).Valor
        Dim oAttachment As Attachment
        correo.From = New MailAddress(fromUser, "Fondos Sura", System.Text.Encoding.UTF8)
        If ccUser <> "" Then
            correo.CC.Add(ccUser)
        End If
        'Para cuando ayan mas de un correo
        Dim array = toUser.Split(";")
        For Each s_Correo As String In array
            If (Not s_Correo.Equals("")) Then
                correo.To.Add(s_Correo)
            End If
        Next
        correo.Subject = asunto
        correo.Body = message
        correo.BodyEncoding = System.Text.Encoding.UTF8
        correo.IsBodyHtml = True
        If sAttach <> "" Then
            oAttachment = New Attachment(sAttach)
            correo.Attachments.Add(oAttachment)
        End If
        lTablaGeneral = oParametrosGeneralesBM.SeleccionarPorFiltro(SERV_CORREO, "", "", "")
        smtp.Host = lTablaGeneral.Item(0).Valor
        smtp.Port = 25
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network
        Try
            smtp.Send(correo)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function GenerateRandomString(ByRef len As Integer, ByRef upper As Boolean) As String
        Dim rand As New Random
        Dim allowableChars() As Char = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()
        Dim final As String = String.Empty
        For i As Integer = 0 To len - 1
            final += allowableChars(rand.Next(allowableChars.Length - 1))
        Next
        Return IIf(upper, final.ToUpper(), final)
    End Function
    Public Shared Function ConvertirDecimalAStringFormatoFecha(ByVal fecha As Decimal) As String
        Dim strFechaResultado As String = ""
        If Not fecha = 0 And fecha.ToString.Length = 8 Then
            Dim strfecha As String = fecha.ToString()
            If strfecha.Length > 0 Then strFechaResultado = strfecha.Substring(6, 2) + "/" + strfecha.Substring(4, 2) + "/" + strfecha.Substring(0, 4)
        Else
            Return ""
        End If
        Return strFechaResultado
    End Function
    Public Shared Function ObtenerFechaMaximaNegocio() As Decimal
        Dim oPortafolio As New PortafolioBM
        Return oPortafolio.FechaMaximaPortafolio()
    End Function
    Public Shared Function ObtenerFechaNegocio(ByVal codigoPortafolio As String) As Decimal
        Dim oPortafolio As New PortafolioBM
        Return oPortafolio.FechaNegocio(codigoPortafolio)
    End Function
    '#Region " /* Métodos Personalizados (Vol. 1) */ "
    '    Public Shared Sub AgregarElementoFinal(ByVal sel As DropDownList, Optional ByVal valor As String = "", Optional ByVal texto As String = "")
    '        Dim iElemento As Integer = sel.Items.Count
    '        sel.Items.Insert(iElemento, New System.Web.UI.WebControls.ListItem(texto, valor))
    '    End Sub


    '    Public Shared Sub InsertarOtroElementoSeleccion(ByVal sel As DropDownList, Optional ByVal valor As String = "")
    '        If sel.Items(0).Text.ToUpper <> "--SELECCIONE--" Then
    '            sel.Items.Insert(0, New System.Web.UI.WebControls.ListItem("--SELECCIONE--", valor))
    '        End If
    '    End Sub
    '    Public Shared Sub SeleccionarDefaultValue(ByRef sel As DropDownList, ByVal index As Integer)
    '        If sel.Items.Count > index Then sel.SelectedIndex = index
    '    End Sub
    '    Public Shared Sub ExcluirOtroElementoSeleccion(ByVal sel As DropDownList)
    '        If sel.Items(0).Text = "--SELECCIONE--" Then sel.Items.RemoveAt(0)
    '    End Sub
    '#End Region
    '#Region " /* Métodos Personalizados (Vol. 2) */ "
    '    Public Shared Function ValidarHora(ByVal hora As String) As Boolean
    '        Dim strAux As String = hora
    '        strAux = strAux.Substring(0, 2) + strAux.Substring(3, 2)
    '        If Convert.ToDecimal(strAux) >= 2400 Or Convert.ToDecimal(strAux.Substring(2, 2)) >= 60 Then
    '            Return False
    '        End If
    '        Return True
    '    End Function

    '    Public Shared Function ConvertirStringaFecha(ByVal fecha As String) As System.DateTime
    '        Dim strfecha As System.DateTime
    '        If fecha.Length > 0 Then strfecha = New Date(fecha.Split("/")(2), fecha.Split("/")(1), fecha.Split("/")(0))
    '        Return strfecha
    '    End Function
    '    Public Shared Function fnFechaNueva(ByVal fechaAnt As String) As Date
    '        Dim Fecha As Decimal
    '        Dim fechaAnterior As Date
    '        Dim fechaNueva As Date
    '        Dim oFeriadoBM As New FeriadoBM
    '        Dim EsFeriado As Boolean
    '        fechaAnterior = Convert.ToDateTime(fechaAnt)
    '        fechaNueva = fechaAnterior.AddDays(1)
    '        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
    '        If fechaNueva.DayOfWeek = DayOfWeek.Saturday Then
    '            While fechaNueva.DayOfWeek = DayOfWeek.Saturday
    '                fechaNueva = fechaNueva.AddDays(2)
    '                Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
    '                While oFeriadoBM.BuscarPorFecha(Fecha, "A")
    '                    fechaNueva = fechaNueva.AddDays(1)
    '                    Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
    '                End While
    '            End While
    '        Else
    '            EsFeriado = oFeriadoBM.BuscarPorFecha(Fecha, "A")
    '            If EsFeriado = True Then
    '                While oFeriadoBM.BuscarPorFecha(Fecha, "A")
    '                    fechaNueva = fechaNueva.AddDays(1)
    '                    Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
    '                    While fechaNueva.DayOfWeek = DayOfWeek.Saturday
    '                        fechaNueva = fechaNueva.AddDays(2)
    '                        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
    '                    End While
    '                End While
    '            End If
    '        End If
    '        Return fechaNueva
    '    End Function
    '    Public Shared Function fnFechaAnterior(ByVal FechaActual As String) As Date
    '        Dim Fecha As Decimal
    '        Dim fechaAnterior As Date
    '        Dim fechaNueva As Date
    '        Dim oFeriadoBM As New FeriadoBM
    '        fechaAnterior = Convert.ToDateTime(FechaActual)
    '        fechaNueva = fechaAnterior.AddDays(-1)
    '        If fechaNueva.DayOfWeek = DayOfWeek.Sunday Then
    '            fechaNueva = fechaNueva.AddDays(-2)
    '            While oFeriadoBM.BuscarPorFecha(CType(fechaNueva.ToString("yyyyMMdd"), Decimal), "A")
    '                fechaNueva = fechaNueva.AddDays(-1)
    '            End While
    '        ElseIf fechaNueva.DayOfWeek = DayOfWeek.Saturday Then
    '            fechaNueva = fechaNueva.AddDays(-1)
    '            While oFeriadoBM.BuscarPorFecha(CType(fechaNueva.ToString("yyyyMMdd"), Decimal), "A")
    '                fechaNueva = fechaNueva.AddDays(-1)
    '            End While
    '        Else
    '            While oFeriadoBM.BuscarPorFecha(CType(fechaNueva.ToString("yyyyMMdd"), Decimal), "A")
    '                fechaNueva = fechaNueva.AddDays(-1)
    '            End While
    '        End If
    '        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
    '        Return fechaNueva
    '    End Function
    '#Region "66056 - Modificacion: JZAVALA"
    '    ''' <summary>66056 - JZAVALA.
    '    ''' METODO NUEVO QUE CONVIERTE EL DECIMAL A STRING CON EL FORMATO DE FECHA.
    '    ''' </summary>
    '    ''' <param name="fecha"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Shared Function ConvertirDecimalAStringFormatoFecha(ByVal fecha As Decimal) As String
    '        Dim strFechaResultado As String = ""
    '        If Not fecha = 0 And fecha.ToString.Length = 8 Then
    '            Dim strfecha As String = fecha.ToString()
    '            If strfecha.Length > 0 Then strFechaResultado = strfecha.Substring(6, 2) + "/" + strfecha.Substring(4, 2) + "/" + strfecha.Substring(0, 4)
    '        Else
    '            Return ""
    '        End If
    '        Return strFechaResultado
    '    End Function
    '#End Region
    '    Public Shared Function ObtenerNombreDia(ByVal fecha As String) As String
    '        Dim strNombreDia As String = ""
    '        Dim strfecha As System.DateTime
    '        strfecha = ConvertirStringaFecha(fecha)
    '        Select Case strfecha.DayOfWeek
    '            Case DayOfWeek.Monday
    '                strNombreDia = "Lunes"
    '            Case DayOfWeek.Tuesday
    '                strNombreDia = "Martes"
    '            Case DayOfWeek.Wednesday
    '                strNombreDia = "Miércoles"
    '            Case DayOfWeek.Thursday
    '                strNombreDia = "Jueves"
    '            Case DayOfWeek.Friday
    '                strNombreDia = "Viernes"
    '            Case DayOfWeek.Saturday
    '                strNombreDia = "Sábado"
    '            Case DayOfWeek.Sunday
    '                strNombreDia = "Domingo"
    '        End Select
    '        Return strNombreDia
    '    End Function

    '    '***********************************************************
    '    ' Autor: Julio A. Rodríguez Grados
    '    ' Función de Validación de Fechas
    '    ' Recibe una fecha con formato dd/mm/yyyy o dd-mm-yyyy
    '    ' Devuelve True/False Si es o NO Correcta la Fecha recibida
    '    '***********************************************************
    '    Public Shared Function ValidaFormatoFecha(ByVal cFecha As String) As Boolean
    '        Dim cDay, cMonth, cYear, cFechaReal As String
    '        Dim MyDate As Date
    '        Dim sDate1 As String
    '        ValidaFormatoFecha = False
    '        If cFecha.Length < 10 Then
    '            Return False
    '            Exit Function
    '        End If
    '        If cFecha <> "          " Then
    '            Return False
    '            Exit Function
    '        End If
    '        cDay = cFecha.Substring(0, 2)
    '        cMonth = cFecha.Substring(3, 2)
    '        cYear = cFecha.Substring(6, 4)
    '        If Val(cYear) < 1900 Then
    '            Return False
    '            Exit Function
    '        End If
    '        If Val(cMonth) > 12 Then
    '            Return False
    '            Exit Function
    '        End If
    '        'Si la fecha es ingresada en blanco es correcta
    '        If cDay <> Space(2) And cMonth <> Space(2) And cYear <> Space(4) Then
    '            'Windows esta en formato español
    '            If IsDate("13/05/72") Then cFechaReal = cDay + "/" + cMonth + "/" + cYear
    '            If IsDate(cFechaReal) Then
    '                MyDate = DateValue(cFechaReal)
    '                sDate1 = Format$(MyDate, "dd/MM/yyyy")
    '                If cFecha = sDate1 Then ValidaFormatoFecha = True Else ValidaFormatoFecha = False
    '            End If
    '        End If
    '    End Function
    '#End Region
    '#Region " /* Métodos Personalizados (Vol. 3) */ "
    '    Public Shared Sub ValidarLista(ByVal ddl As DropDownList, ByVal lista As String)
    '        Dim i As Integer = ddl.Items.Count - 1
    '        While (i >= 0)
    '            If lista.IndexOf(ddl.Items(i).Value) = -1 Then ddl.Items.RemoveAt(i)
    '            i = i - 1
    '        End While
    '    End Sub
    '    Public Shared Sub ValidarLista(ByVal ddl As DropDownList, ByVal lista As String())
    '        Dim i As Integer = ddl.Items.Count - 1
    '        Dim list As String = ListaCadena(lista)
    '        While (i >= 0)
    '            If list.IndexOf(ddl.Items(i).Value) = -1 Then ddl.Items.RemoveAt(i)
    '            i = i - 1
    '        End While
    '    End Sub
    '#End Region
    '#Region " /* Métodos Personalizados (Vol. 4) */ "
    '    Public Shared Function ListaCadena(ByVal lista As String()) As String
    '        Dim cadena As String = ""
    '        For Each cad As String In lista
    '            cadena = cadena & cad & ","
    '        Next
    '        Return IIf(cadena.Length > 0, cadena.Substring(0, cadena.Length - 1), cadena)
    '    End Function
    '#End Region
    '#Region " /* Métodos Personalizados (Vol. 7) */ "
    '    Public Shared Function ValidarPortafolioAperturado(ByVal codigoPortafolio As String) As Boolean
    '        Dim oPortafolio As New PortafolioBM
    '        Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
    '        Dim fechaTermino As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaTermino")
    '        Dim FechaConstitucion As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaConstitucion")
    '        Return (fechaTermino <> FechaConstitucion)
    '    End Function
    '    Public Shared Function ObtenerFechaApertura(ByVal codigoPortafolio As String) As Decimal
    '        Dim oPortafolio As New PortafolioBM
    '        Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
    '        If codigoPortafolio.Trim.Equals("") Then
    '            codigoPortafolio = PORTAFOLIO_MULTIFONDOS
    '        End If
    '        Dim fechaConstitucion As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaConstitucion")
    '        Return fechaConstitucion
    '    End Function
    
    '    Public Shared Function ObtenerFechaCajaOperaciones(ByVal codigoPortafolio As String) As Decimal
    '        Dim oPortafolio As New PortafolioBM
    '        Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
    '        Dim fechaNegocio As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaCajaOperaciones")
    '        Return fechaNegocio
    '    End Function
    
    '    Public Shared Function ObtenerFechaAnterior(ByVal codigoPortafolio As String) As Decimal
    '        Dim oPortafolio As New PortafolioBM
    '        Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
    '        Dim fechaConstitucion As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaTermino")
    '        Return fechaConstitucion
    '    End Function
    '    Public Shared Function ObtenerDatosPortafolio(ByVal codigoPortafolio As String) As String
    '        Dim oPortafolio As New PortafolioBM
    '        Dim dsPortafolio As DataSet = oPortafolio.ObtenerDatosPortafolio(Nothing, ParametrosSIT.ESTADO_ACTIVO)
    '        Dim sHoraConstitucion As String = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("HoraConstitucion")
    '        Return sHoraConstitucion
    '    End Function
    '    Public Shared Function ObtenerFechaAperturaContable(ByVal codigoPortafolio As String) As Decimal
    '        Dim oPortafolio As New PortafolioBM
    '        Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
    '        If codigoPortafolio.Trim.Equals("") Then
    '            codigoPortafolio = PORTAFOLIO_MULTIFONDOS
    '        End If
    '        Dim fechaConstitucion As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaAperturaContable")
    '        Return fechaConstitucion
    '    End Function
    '    Public Shared Function ObtenerFechaAperturaContableAnterior(ByVal codigoPortafolio As String) As Decimal
    '        Dim oPortafolio As New PortafolioBM
    '        Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
    '        Dim fechaAnterior As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaAperturaContableAnterior")
    '        Return fechaAnterior
    '    End Function
    '    Public Shared Function ObtenerCodigoTipoMercado(ByVal mercado As String) As String
    '        Dim tipomercado As String = String.Empty
    '        Select Case mercado
    '            Case Constantes.M_STR_MERCADO_LOCAL : tipomercado = System.Configuration.ConfigurationSettings.AppSettings("MERCADO_LOCAL").ToString()
    '            Case Constantes.M_STR_MERCADO_EXTRANJERO : tipomercado = System.Configuration.ConfigurationSettings.AppSettings("MERCADO_EXTRANJERO").ToString()
    '        End Select
    '        Return tipomercado
    '    End Function
    '    Public Shared Function ObtenerCodigoTipoOperacion(ByVal operacion As String) As String
    '        Dim tipoOperacion As String = String.Empty
    '        Select Case operacion
    '            Case Constantes.M_COMPRA : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_COMPRA").ToString()
    '            Case Constantes.M_VENTA : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_VENTA").ToString()
    '            Case Constantes.M_CONS : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_CONS").ToString()
    '        End Select
    '        Return tipoOperacion
    '    End Function
    '    Public Shared Function ObtenerCodigoTipoOperacionPorTraspaso(ByVal operacion As String) As String
    '        Dim tipoOperacion As String = String.Empty
    '        Select Case operacion
    '            Case Constantes.M_TRASPASO_EGRESO : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_TRASPASO_EGRESO").ToString()
    '            Case Constantes.M_TRASPASO_INGRESO : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_TRASPASO_INGRESO").ToString()
    '        End Select
    '        Return tipoOperacion
    '    End Function
    '    Public Shared Function ObtenerCodigoOperacionCompra() As String
    '        Return System.Configuration.ConfigurationSettings.AppSettings("OPERACION_COMPRA").ToString()
    '    End Function
    '    Public Shared Function ObtenerCodigoOperacionVenta() As String
    '        Return System.Configuration.ConfigurationSettings.AppSettings("OPERACION_VENTA").ToString()
    '    End Function
    '    Public Shared Function ObtenerCodigoOperacionTEgreso() As String
    '        Return System.Configuration.ConfigurationSettings.AppSettings("OPERACION_TRASPASO_EGRESO").ToString()
    '    End Function
    '    Public Shared Function ObtenerCodigoOperacionTIngreso() As String
    '        Return System.Configuration.ConfigurationSettings.AppSettings("OPERACION_TRASPASO_INGRESO").ToString()
    '    End Function
    '    Public Shared Function ObtenerCodigoTipoOperacionDP(ByVal operacion As String) As String
    '        Dim tipoOperacion As String = String.Empty
    '        Select Case operacion
    '            Case Constantes.M_CONS : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_CONS").ToString()
    '            Case Constantes.M_CANC : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_CANC").ToString()
    '            Case Constantes.M_RENO : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_RENO").ToString()
    '            Case Constantes.M_PREC : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_PREC").ToString()
    '        End Select
    '        Return tipoOperacion
    '    End Function
    '    Public Shared Function ObtenerCodigoTipoRenta(ByVal tipoRenta As String) As String
    '        Dim tipoR As String = String.Empty
    '        Select Case tipoRenta
    '            Case Constantes.M_RFIJ : tipoR = System.Configuration.ConfigurationSettings.AppSettings("TR_FIJA").ToString()
    '            Case Constantes.M_RVAR : tipoR = System.Configuration.ConfigurationSettings.AppSettings("TR_VARIABLE").ToString()
    '            Case Constantes.M_RDER : tipoR = System.Configuration.ConfigurationSettings.AppSettings("TR_DERIVADOS").ToString()
    '        End Select
    '        Return tipoR
    '    End Function
    '    Public Shared Function ObtenerPlazo_CalifInst(ByVal califInst As String) As String
    '        Dim plazo As String = String.Empty
    '        Select Case califInst
    '            Case Constantes.M_CI_PLAZO_LP : plazo = System.Configuration.ConfigurationSettings.AppSettings("TR_FIJA").ToString()
    '            Case Constantes.M_CI_PLAZO_RV : plazo = System.Configuration.ConfigurationSettings.AppSettings("TR_VARIABLE").ToString()
    '            Case Constantes.M_CI_PLAZO_CP : plazo = System.Configuration.ConfigurationSettings.AppSettings("TR_FIJA").ToString()
    '            Case Constantes.M_CI_PLAZO_DV : plazo = System.Configuration.ConfigurationSettings.AppSettings("TR_DERIVADOS").ToString()
    '        End Select
    '        Return plazo
    '    End Function
    '#End Region
    '#Region " /* Métodos Personalizados (Vol. 8) */ "
    '    Public Shared Function GetStructureTablebase(ByVal campos() As String, Optional ByVal tipos() As String = Nothing, Optional ByVal tableName As String = "", Optional ByRef mensaje As String = "") As DataTable
    '        Dim dtblBase As New DataTable(IIf(tableName.Length > 0, tableName, String.Empty))
    '        Dim tiposDatos As Boolean = False
    '        Dim type As String = String.Empty
    '        If Not tipos Is Nothing Then
    '            tiposDatos = True
    '        Else
    '            type = "System.String"
    '        End If
    '        Try
    '            For i As System.Int16 = 0 To campos.Length - 1
    '                If tiposDatos Then type = tipos(i).ToString()
    '                dtblBase.Columns.Add(New DataColumn(campos(i).ToString, System.Type.GetType(type)))
    '            Next
    '        Catch ex As Exception
    '            mensaje = ex.Message : Return Nothing
    '        End Try
    '        Return dtblBase
    '    End Function
    '#End Region
    '#Region " /* Métodos Personalizados (Vol. 9) */ "
    '    Public Shared Function MostrarResultadoCarga(ByVal dtblDatos As DataTable) As String
    '        Dim iCount As Integer = dtblDatos.Rows.Count
    '        Dim resultado As String = String.Empty
    '        If iCount = 0 Then
    '            resultado = Constantes.M_STR_TEXTO_CARGADOS & "0"
    '        Else
    '            resultado = Constantes.M_STR_TEXTO_CARGADOS & IIf(dtblDatos.Rows.Count < 10, "0" & dtblDatos.Rows.Count, dtblDatos.Rows.Count)
    '        End If
    '        Return resultado
    '    End Function
    '#End Region
    '#Region "/* Métodos Personalizados (Vol. 11) */"
    '    Public Shared Function ObtenerDescripcionTercero(ByVal codigoTercero As String, ByVal datosRequest As DataSet) As String
    '        Dim objBM As New TercerosBM
    '        Dim objBE As New TercerosBE
    '        Dim strDes As String = ""
    '        objBE = objBM.Seleccionar(codigoTercero, datosRequest)
    '        If objBE.Tables(0).Rows.Count > 0 Then
    '            strDes = objBE.Tables(0).Rows(0)("Descripcion")
    '        End If
    '        Return strDes
    '    End Function
    '    Public Shared Function ObtenerDescripcionContacto(ByVal codigoContacto As String, ByVal datosRequest As DataSet) As String
    '        Dim objBM As New ContactoBM
    '        Dim objBE As New ContactoBE
    '        Dim strDes As String = ""
    '        objBE = objBM.Seleccionar(codigoContacto, datosRequest)
    '        If objBE.Tables(0).Rows.Count > 0 Then
    '            strDes = objBE.Tables(0).Rows(0)("Descripcion")
    '        End If
    '        Return strDes
    '    End Function
    '    Public Shared Function ObtenerDescripcionClaseInstrumento(ByVal codigoCI As String) As String
    '        Dim strDato As String = ""
    '        Select Case codigoCI
    '            Case "BO" : strDato = "BONOS"
    '            Case "AC" : strDato = "ACCIONES"
    '            Case "CD" : strDato = "CERTIFICADO DE DEPOSITO"
    '            Case "CS" : strDato = "CERTIFICADO DE SUSCRIPCION"
    '            Case "PC" : strDato = "PAPELES COMERCIALES"
    '            Case "PA" : strDato = "PAGARES"
    '            Case "FI" : strDato = "ORDENES DE FONDO"
    '            Case "FM" : strDato = "ORDENES DE FONDO"
    '            Case "OR" : strDato = "OPERACIONES DE REPORTE"
    '            Case "IC" : strDato = "INSTRUMENTOS COBERTURADOS"
    '            Case "LH" : strDato = "LETRAS HIPOTECARIAS"
    '            Case "IE" : strDato = "INSTRUMENTOS ESTRUCTURADOS"
    '            Case "DP" : strDato = "DEPOSITOS A PLAZO" 'RGF 20090702
    '            Case "FD" : strDato = "OPERACIONES DERIVADAS - FORWARD DIVISAS" 'CMB OT 61566 20101210
    '            Case "CV" : strDato = "COMPRA/VENTA MONEDA EXTRANJERA" 'CMB OT 61566 20101210
    '        End Select
    '        Return strDato
    '    End Function
    '    Public Shared Function ObtenerDescripcionMoneda(ByVal codigoMoneda As String, ByVal datosRequest As DataSet) As String
    '        Dim objBM As New MonedaBM
    '        Dim objBE As New MonedaBE
    '        Dim strDescripcion As String = ""
    '        objBE = objBM.SeleccionarPorFiltro(codigoMoneda, "", "A", "", "", datosRequest)
    '        If objBE.Tables(0).Rows.Count > 0 Then
    '            strDescripcion = objBE.Tables(0).Rows(0)("Descripcion")
    '        End If
    '        Return strDescripcion
    '    End Function
    '    Public Shared Function ObtenerDescripcionMotivo(ByVal codigoMotivo As String, ByVal datosRequest As DataSet) As String
    '        Dim objBM As New MotivoBM
    '        Dim objBE As New MotivoBE
    '        Dim strDescripcion As String = ""
    '        objBE = objBM.SeleccionarPorFiltro(codigoMotivo, "", "A", datosRequest)
    '        If objBE.Tables(0).Rows.Count > 0 Then
    '            strDescripcion = objBE.Tables(0).Rows(0)("Descripcion")
    '        End If
    '        Return strDescripcion
    '    End Function
    '    Public Shared Function ObtenerDatosOperacion_Acciones(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
    '        Dim objOI_BE As New OrdenPreOrdenInversionBE
    '        Dim objOI_BM As New OrdenPreOrdenInversionBM
    '        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
    '        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    '        oRow = objOI_BE.Tables(0).Rows(0)
    '        Dim drGrilla As DataRow
    '        Dim dtGrilla As New DataTable
    '        Dim blnEsImpar As Boolean = False
    '        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
    '        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
    '        drGrilla = dtGrilla.NewRow
    '        drGrilla("c1") = "Fecha Operación"
    '        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
    '        drGrilla("c2") = "Fecha Vencimiento"
    '        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
    '        drGrilla("v3") = "Hora Operación"
    '        drGrilla("c3") = oRow.HoraOperacion
    '        drGrilla("c4") = "Numero Acciones Ordenadas"
    '        drGrilla("v4") = Format(oRow.CantidadOrdenado, "##,##0.0000000")
    '        drGrilla("c5") = "Numero Acciones Operación"
    '        drGrilla("v5") = Format(oRow.CantidadOperacion, "##,##0.0000000")
    '        drGrilla("c6") = "Precio"
    '        drGrilla("v6") = Format(oRow.Precio, "##,##0.0000000")
    '        drGrilla("c7") = "Monto Operacion"
    '        drGrilla("v7") = Format(oRow.MontoOperacion, "##,##0.0000000")
    '        drGrilla("c8") = "Intermediario"
    '        drGrilla("v8") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
    '        If oRow.CodigoContacto <> "" Then
    '            drGrilla("c9") = "Contacto"
    '            drGrilla("v9") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
    '        Else
    '            drGrilla("c9") = ""
    '            drGrilla("v9") = ""
    '        End If
    '        drGrilla("c10") = "Observación"
    '        drGrilla("v10") = oRow.Observacion
    '        If oRow.NumeroPoliza <> "" Then
    '            drGrilla("c11") = "Poliza"
    '            drGrilla("v11") = oRow.NumeroPoliza
    '        Else
    '            drGrilla("c11") = ""
    '            drGrilla("v11") = ""
    '        End If
    '        drGrilla("c12") = ""
    '        drGrilla("v12") = ""
    '        drGrilla("c13") = ""
    '        drGrilla("v13") = ""
    '        drGrilla("c14") = ""
    '        drGrilla("v14") = ""
    '        drGrilla("c15") = ""
    '        drGrilla("v15") = ""
    '        drGrilla("c16") = ""
    '        drGrilla("v16") = ""
    '        drGrilla("c17") = ""
    '        drGrilla("v17") = ""
    '        drGrilla("c18") = ""
    '        drGrilla("v18") = ""
    '        drGrilla("c19") = "Total Comisiones"
    '        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
    '        drGrilla("c20") = "Precio Promedio"
    '        drGrilla("v20") = Format(oRow.PrecioPromedio, "##,##0.0000000")
    '        drGrilla("c21") = "Monto Neto Operacion"
    '        drGrilla("v21") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
    '        dtGrilla.Rows.Add(drGrilla)
    '        Return dtGrilla
    '    End Function
    '    Public Shared Function ObtenerDatosOperacion_Bonos(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
    '        Dim objOI_BE As New OrdenPreOrdenInversionBE
    '        Dim objOI_BM As New OrdenPreOrdenInversionBM
    '        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
    '        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    '        oRow = objOI_BE.Tables(0).Rows(0)
    '        Dim drGrilla As DataRow
    '        Dim dtGrilla As New DataTable
    '        Dim blnEsImpar As Boolean = False
    '        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
    '        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
    '        drGrilla = dtGrilla.NewRow
    '        drGrilla("c1") = "Fecha Operacion"
    '        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
    '        drGrilla("c2") = "Fecha Vencimiento"
    '        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
    '        drGrilla("c3") = "Hora Operacion"
    '        drGrilla("v3") = oRow.HoraOperacion
    '        drGrilla("c4") = "Monto Nominal Ordenado"
    '        drGrilla("v4") = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
    '        drGrilla("c5") = "Monto Nominal Operacion"
    '        drGrilla("v5") = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
    '        drGrilla("c6") = "Tipo Tasa"
    '        drGrilla("v6") = IIf(oRow.CodigoTipoCupon = "1", "NOMINAL", "EFECTIVA")
    '        drGrilla("c7") = "YTM%"
    '        drGrilla("v7") = Format(oRow.YTM, "##,##0.0000000")
    '        drGrilla("c8") = "Precio Negociacion %"
    '        drGrilla("v8") = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000000")
    '        drGrilla("c9") = "Precio Calculado %"
    '        drGrilla("v9") = Format(oRow.PrecioCalculado, "##,##0.0000000")
    '        drGrilla("c10") = "Precio Negociacion Sucio"
    '        drGrilla("v10") = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
    '        drGrilla("c11") = "Interes Corrido Negociado"
    '        drGrilla("v11") = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
    '        drGrilla("c12") = "Interes Corrido"
    '        drGrilla("v12") = Format(oRow.InteresCorrido, "##,##0.0000000")
    '        drGrilla("c13") = "Monto Operacion"
    '        drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.00")
    '        drGrilla("c14") = "Numero Papeles"
    '        drGrilla("v14") = Format(oRow.CantidadOperacion, "##,##0.0000000")
    '        drGrilla("c15") = "Intermediario" '***
    '        drGrilla("v15") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
    '        If oRow.CodigoContacto <> "" Then
    '            drGrilla("c16") = "Contacto"
    '            drGrilla("v16") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
    '        Else
    '            drGrilla("c16") = ""
    '            drGrilla("v16") = ""
    '        End If
    '        If oRow.NumeroPoliza <> "" Then
    '            drGrilla("c17") = "Numero Poliza"
    '            drGrilla("v17") = oRow.NumeroPoliza
    '        Else
    '            drGrilla("c17") = ""
    '            drGrilla("v17") = ""
    '        End If
    '        drGrilla("c18") = "Observacion"
    '        drGrilla("v18") = oRow.Observacion
    '        drGrilla("c19") = "Total Comisiones"
    '        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
    '        drGrilla("c20") = "Monto Neto Operacion"
    '        drGrilla("v20") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
    '        dtGrilla.Rows.Add(drGrilla)
    '        Return dtGrilla
    '    End Function
    '    Public Shared Function ObtenerDatosOperacion_CertificadoDeposito(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
    '        Dim objOI_BE As New OrdenPreOrdenInversionBE
    '        Dim objOI_BM As New OrdenPreOrdenInversionBM
    '        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
    '        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    '        oRow = objOI_BE.Tables(0).Rows(0)
    '        Dim drGrilla As DataRow
    '        Dim dtGrilla As New DataTable
    '        Dim blnEsImpar As Boolean = False
    '        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
    '        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
    '        drGrilla = dtGrilla.NewRow
    '        drGrilla("c1") = "Fecha de Operacion"
    '        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
    '        drGrilla("c2") = "Fecha de Vencimiento"
    '        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
    '        drGrilla("c3") = "Hora Operación"
    '        drGrilla("v3") = oRow.HoraOperacion
    '        drGrilla("c4") = "Monto Nominal Ordenado"
    '        drGrilla("v4") = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
    '        drGrilla("c5") = "Monto Nominal Operación"
    '        drGrilla("v5") = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
    '        drGrilla("c6") = "Tipo Tasa"
    '        drGrilla("v6") = "EFECTIVA"
    '        drGrilla("c7") = "YTM %"
    '        drGrilla("v7") = Format(oRow.YTM, "##,##0.00")
    '        drGrilla("c8") = "Precio Negociación %"
    '        drGrilla("v8") = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000")
    '        drGrilla("c9") = "Precio Calculado"
    '        drGrilla("v9") = Format(oRow.PrecioCalculado, "##,##0.0000")
    '        drGrilla("c10") = "Precio Negociación Sucio"
    '        drGrilla("v10") = Format(oRow.PrecioNegociacionSucio, "##,##0.0000")
    '        drGrilla("c11") = ""
    '        drGrilla("v11") = ""
    '        drGrilla("c12") = ""
    '        drGrilla("v12") = ""
    '        drGrilla("c13") = "Monto Operación"
    '        drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.00")
    '        drGrilla("c14") = "Número Papeles"
    '        drGrilla("v14") = Format(oRow.CantidadOperacion, "##,##0.0000000")
    '        drGrilla("c15") = "Intermediario"
    '        drGrilla("v15") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
    '        If oRow.CodigoContacto <> "" Then
    '            drGrilla("c16") = "Contacto"
    '            drGrilla("v16") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
    '        Else
    '            drGrilla("c16") = ""
    '            drGrilla("v16") = ""
    '        End If
    '        If oRow.NumeroPoliza <> "" Then
    '            drGrilla("c17") = "Número Poliza"
    '            drGrilla("v17") = oRow.NumeroPoliza
    '        Else
    '            drGrilla("c17") = ""
    '            drGrilla("v17") = ""
    '        End If
    '        drGrilla("c18") = "Observación"
    '        drGrilla("v18") = oRow.Observacion
    '        drGrilla("c19") = ""
    '        drGrilla("v19") = ""
    '        drGrilla("c20") = ""
    '        drGrilla("v20") = ""
    '        dtGrilla.Rows.Add(drGrilla)
    '        Return dtGrilla
    '    End Function
    '    Public Shared Function ObtenerDatosOperacion_CertificadoSuscripcion(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
    '        Dim objOI_BE As New OrdenPreOrdenInversionBE
    '        Dim objOI_BM As New OrdenPreOrdenInversionBM
    '        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
    '        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    '        oRow = objOI_BE.Tables(0).Rows(0)
    '        Dim drGrilla As DataRow
    '        Dim dtGrilla As New DataTable
    '        Dim blnEsImpar As Boolean = False
    '        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
    '        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
    '        drGrilla = dtGrilla.NewRow
    '        drGrilla("c1") = "Fecha Operacion"
    '        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
    '        drGrilla("c2") = "Fecha Vencimiento"
    '        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
    '        drGrilla("c3") = "Hora Operación"
    '        drGrilla("v3") = oRow.HoraOperacion
    '        drGrilla("c4") = "Número Papeles Ordenadas"
    '        drGrilla("v4") = Format(oRow.CantidadOrdenado, "##,##0.0000000")
    '        drGrilla("c5") = "Número Papeles Operación"
    '        drGrilla("v5") = Format(oRow.CantidadOperacion, "##,##0.0000000")
    '        drGrilla("c6") = "Precio"
    '        drGrilla("v6") = Format(oRow.Precio, "##,##0.0000000")
    '        drGrilla("c7") = "Monto Operación"
    '        drGrilla("v7") = Format(oRow.MontoOperacion, "##,##0.0000000")
    '        drGrilla("c8") = "Intermediario"
    '        drGrilla("v8") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
    '        If oRow.CodigoContacto <> "" Then
    '            drGrilla("c9") = "Contacto"
    '            drGrilla("v9") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
    '        Else
    '            drGrilla("c9") = ""
    '            drGrilla("v9") = ""
    '        End If
    '        drGrilla("c10") = "Observación"
    '        drGrilla("v10") = oRow.Observacion
    '        If oRow.NumeroPoliza <> "" Then
    '            drGrilla("c11") = "Número Poliza"
    '            drGrilla("v11") = oRow.NumeroPoliza
    '        Else
    '            drGrilla("c11") = ""
    '            drGrilla("v11") = ""
    '        End If
    '        drGrilla("c12") = ""
    '        drGrilla("v12") = ""
    '        drGrilla("c13") = ""
    '        drGrilla("v13") = ""
    '        drGrilla("c14") = ""
    '        drGrilla("v14") = ""
    '        drGrilla("c15") = ""
    '        drGrilla("v15") = ""
    '        drGrilla("c16") = ""
    '        drGrilla("v16") = ""
    '        drGrilla("c17") = ""
    '        drGrilla("v17") = ""
    '        drGrilla("c18") = ""
    '        drGrilla("v18") = ""
    '        drGrilla("c19") = "Total Comisiones"
    '        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
    '        drGrilla("c20") = "Monto Neto Operación"
    '        drGrilla("v20") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
    '        drGrilla("c21") = "Precio Promedio"
    '        drGrilla("v21") = Format(oRow.PrecioPromedio, "##,##0.0000000")
    '        dtGrilla.Rows.Add(drGrilla)
    '        Return dtGrilla
    '    End Function
    '    Public Shared Function ObtenerDatosOperacion_PapelesComerciales(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
    '        Dim objOI_BE As New OrdenPreOrdenInversionBE
    '        Dim objOI_BM As New OrdenPreOrdenInversionBM
    '        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
    '        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    '        oRow = objOI_BE.Tables(0).Rows(0)
    '        Dim drGrilla As DataRow
    '        Dim dtGrilla As New DataTable
    '        Dim blnEsImpar As Boolean = False
    '        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
    '        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
    '        drGrilla = dtGrilla.NewRow
    '        drGrilla("c1") = "Fecha Operación"
    '        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
    '        drGrilla("c2") = "Fecha Vencimiento"
    '        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
    '        drGrilla("c3") = "Hora Operación"
    '        drGrilla("v3") = oRow.HoraOperacion
    '        drGrilla("c4") = "Monto Nominal Ordenado"
    '        drGrilla("v4") = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
    '        drGrilla("c5") = "Monto Nominal Operación"
    '        drGrilla("v5") = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
    '        drGrilla("c6") = "Tipo Tasa"
    '        drGrilla("v6") = IIf(oRow.CodigoTipoCupon = "1", "NOMINAL", "EFECTIVA")
    '        drGrilla("c7") = "YTM%"
    '        drGrilla("v7") = Format(oRow.YTM, "##,##0.00")
    '        drGrilla("c8") = "Precio Negociación %"
    '        drGrilla("v8") = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000000")
    '        drGrilla("c9") = "Precio Calculado %"
    '        drGrilla("v9") = Format(oRow.PrecioCalculado, "##,##0.0000000")
    '        drGrilla("c10") = "Precio Negociación Sucio"
    '        drGrilla("v10") = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
    '        drGrilla("c11") = "Interés Corrido Negociado"
    '        drGrilla("v11") = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
    '        drGrilla("c12") = "Interés Corrido"
    '        drGrilla("v12") = Format(oRow.InteresCorrido, "##,##0.0000000")
    '        drGrilla("c13") = "Monto Operación"
    '        drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.0000000")
    '        drGrilla("c14") = "Número Papeles"
    '        drGrilla("v14") = Format(oRow.CantidadOperacion, "##,##0.0000000")
    '        drGrilla("c15") = "Intermediario"
    '        drGrilla("v15") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
    '        If oRow.CodigoContacto <> "" Then
    '            drGrilla("c16") = "Contacto"
    '            drGrilla("v16") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
    '        Else
    '            drGrilla("c16") = ""
    '            drGrilla("v16") = ""
    '        End If
    '        If oRow.NumeroPoliza <> "" Then
    '            drGrilla("c17") = "Número Poliza"
    '            drGrilla("v17") = oRow.NumeroPoliza
    '        Else
    '            drGrilla("c17") = ""
    '            drGrilla("v17") = ""
    '        End If
    '        drGrilla("c18") = "Observación"
    '        drGrilla("v18") = oRow.Observacion
    '        drGrilla("c19") = "Total Comisiones"
    '        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
    '        drGrilla("c20") = "Monto Neto Operación"
    '        drGrilla("v20") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
    '        dtGrilla.Rows.Add(drGrilla)
    '        Return dtGrilla
    '    End Function
    '    Public Shared Function ObtenerDatosOperacion_Pagares(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
    '        Dim objOI_BE As New OrdenPreOrdenInversionBE
    '        Dim objOI_BM As New OrdenPreOrdenInversionBM
    '        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
    '        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    '        oRow = objOI_BE.Tables(0).Rows(0)
    '        Dim drGrilla As DataRow
    '        Dim dtGrilla As New DataTable
    '        Dim blnEsImpar As Boolean = False
    '        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
    '        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
    '        drGrilla = dtGrilla.NewRow
    '        drGrilla("c1") = "Fecha Operación"
    '        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
    '        drGrilla("c2") = "Fecha Vencimiento"
    '        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
    '        drGrilla("c3") = "Hora Operación"
    '        drGrilla("v3") = oRow.HoraOperacion
    '        drGrilla("c4") = "Monto Nominal Ordenado"
    '        drGrilla("v4") = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
    '        drGrilla("c5") = "Monto Nominal Operación"
    '        drGrilla("v5") = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
    '        drGrilla("c6") = "Tipo Tasa"
    '        drGrilla("v6") = IIf(oRow.CodigoTipoCupon = "1", "NOMINAL", "EFECTIVA")
    '        drGrilla("c7") = "YTM%"
    '        drGrilla("v7") = Format(oRow.YTM, "##,##0.0000000")
    '        drGrilla("c8") = "Precio Negociación %"
    '        drGrilla("v8") = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000000")
    '        drGrilla("c9") = "Precio Calculado %"
    '        drGrilla("v9") = Format(oRow.PrecioCalculado, "##,##0.0000000")
    '        drGrilla("c10") = "Precio Negociación Sucio"
    '        drGrilla("v10") = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
    '        drGrilla("c11") = "Interés Corrido Negociado"
    '        drGrilla("v11") = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
    '        drGrilla("c12") = "Interés Corrido"
    '        drGrilla("v12") = Format(oRow.InteresCorrido, "##,##0.0000000")
    '        drGrilla("c13") = "Monto Operación"
    '        drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.00")
    '        drGrilla("c14") = "Número Papeles"
    '        drGrilla("v14") = Format(oRow.CantidadOperacion, "##,##0.0000000")
    '        drGrilla("c15") = "Intermediario"
    '        drGrilla("v15") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
    '        If oRow.CodigoContacto <> "" Then
    '            drGrilla("c16") = "Contacto"
    '            drGrilla("v16") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
    '        Else
    '            drGrilla("c16") = ""
    '            drGrilla("v16") = ""
    '        End If
    '        If oRow.NumeroPoliza <> "" Then
    '            drGrilla("c17") = "Número Poliza"
    '            drGrilla("v17") = oRow.NumeroPoliza
    '        Else
    '            drGrilla("c17") = ""
    '            drGrilla("v17") = ""
    '        End If
    '        drGrilla("c18") = "Observación"
    '        drGrilla("v18") = oRow.Observacion
    '        drGrilla("c19") = "Total Comisiones"
    '        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
    '        drGrilla("c20") = "Total Operación"
    '        drGrilla("v20") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
    '        dtGrilla.Rows.Add(drGrilla)
    '        Return dtGrilla
    '    End Function
    '    Public Shared Function ObtenerDatosOperacion_OrdenesFondo(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
    '        Dim objOI_BE As New OrdenPreOrdenInversionBE
    '        Dim objOI_BM As New OrdenPreOrdenInversionBM
    '        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
    '        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    '        oRow = objOI_BE.Tables(0).Rows(0)
    '        Dim drGrilla As DataRow
    '        Dim dtGrilla As New DataTable
    '        Dim blnEsImpar As Boolean = False
    '        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
    '        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
    '        drGrilla = dtGrilla.NewRow
    '        drGrilla("c1") = "Fecha Operación"
    '        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
    '        drGrilla("c2") = "Fecha Vencimiento"
    '        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
    '        drGrilla("c3") = "Hora Operación"
    '        drGrilla("v3") = oRow.HoraOperacion
    '        drGrilla("c4") = "Fecha Trato"
    '        drGrilla("v4") = UIUtility.ConvertirFechaaString(oRow.FechaTrato)
    '        drGrilla("c5") = "Número Cuotas Ordenado"
    '        drGrilla("v5") = Format(oRow.CantidadOrdenado, "##,##0.0000000")
    '        drGrilla("c6") = "Número Cuotas Operación"
    '        drGrilla("v6") = Format(oRow.CantidadOperacion, "##,##0.0000000")
    '        drGrilla("c7") = "Monto Operación"
    '        drGrilla("v7") = Format(oRow.MontoOperacion, "##,##0.0000000")
    '        drGrilla("c8") = "Precio"
    '        drGrilla("v8") = Format(oRow.Precio, "##,##0.0000000")
    '        drGrilla("c9") = "Intermediario"
    '        drGrilla("v9") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
    '        If oRow.CodigoContacto <> "" Then
    '            drGrilla("c10") = "Contacto"
    '            drGrilla("v10") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
    '        Else
    '            drGrilla("c10") = ""
    '            drGrilla("v10") = ""
    '        End If
    '        drGrilla("c11") = "Observación"
    '        drGrilla("v11") = oRow.Observacion
    '        If oRow.NumeroPoliza <> "" Then
    '            drGrilla("c12") = "Número Poliza"
    '            drGrilla("v12") = oRow.NumeroPoliza
    '        Else
    '            drGrilla("c12") = ""
    '            drGrilla("v12") = ""
    '        End If
    '        drGrilla("c13") = ""
    '        drGrilla("v13") = ""
    '        drGrilla("c14") = ""
    '        drGrilla("v14") = ""
    '        drGrilla("c15") = ""
    '        drGrilla("v15") = ""
    '        drGrilla("c16") = ""
    '        drGrilla("v16") = ""
    '        drGrilla("c17") = ""
    '        drGrilla("v17") = ""
    '        drGrilla("c18") = ""
    '        drGrilla("v18") = ""
    '        drGrilla("c19") = "Total Comisiones"
    '        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
    '        drGrilla("c20") = "Monto Neto Operación"
    '        drGrilla("v20") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
    '        drGrilla("c21") = "Precio Promedio"
    '        drGrilla("v21") = Format(oRow.PrecioPromedio, "##,##0.0000000")
    '        dtGrilla.Rows.Add(drGrilla)
    '        Return dtGrilla
    '    End Function
    '    Public Shared Function ObtenerDatosOperacion_OperacionesReporte(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
    '        Dim objOI_BE As New OrdenPreOrdenInversionBE
    '        Dim objOI_BM As New OrdenPreOrdenInversionBM
    '        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
    '        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    '        oRow = objOI_BE.Tables(0).Rows(0)
    '        Dim drGrilla As DataRow
    '        Dim dtGrilla As New DataTable
    '        Dim blnEsImpar As Boolean = False
    '        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
    '        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
    '        drGrilla = dtGrilla.NewRow
    '        drGrilla("c1") = "Numero Bonos"
    '        drGrilla("v1") = oRow.CantidadOperacion
    '        drGrilla("c2") = "Valor Nominal"
    '        drGrilla("v2") = oRow.MontoNominalOperacion
    '        drGrilla("c3") = "Precio de Inicio %"
    '        drGrilla("v3") = oRow.Precio
    '        drGrilla("c4") = "Fecha Inicio"
    '        drGrilla("v4") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
    '        drGrilla("c5") = "Plazo Reporte"
    '        drGrilla("v5") = oRow.Plazo
    '        drGrilla("c6") = "Tasa Anual %"
    '        drGrilla("v6") = oRow.TasaPorcentaje
    '        drGrilla("c7") = "Nemonico"
    '        drGrilla("v7") = oRow.CodigoMnemonico
    '        drGrilla("c8") = "Interes Corrido Contado"
    '        drGrilla("v8") = oRow.InteresCorrido
    '        drGrilla("c9") = "Monto sin Interes"
    '        drGrilla("v9") = oRow.MontoNominalOrdenado
    '        drGrilla("c10") = "Precio Plazo"
    '        drGrilla("v10") = oRow.PrecioCalculado
    '        drGrilla("c11") = "Dia a Plazo"
    '        drGrilla("v11") = oRow.FechaContrato
    '        drGrilla("c12") = "Tipo Cambio"
    '        drGrilla("v12") = oRow.TipoCambio
    '        drGrilla("c13") = "Interes Corrido Plazo"
    '        drGrilla("v13") = oRow.InteresCorridoNegociacion
    '        drGrilla("c14") = "Monto a Plazo sin interes"
    '        drGrilla("v14") = oRow.MontoPlazo
    '        drGrilla("c15") = "Monto a Plazo mas interes"
    '        drGrilla("v15") = oRow.MontoCancelar
    '        drGrilla("c16") = ""
    '        drGrilla("v16") = ""
    '        drGrilla("c17") = ""
    '        drGrilla("v17") = ""
    '        drGrilla("c18") = ""
    '        drGrilla("v18") = ""
    '        drGrilla("c19") = ""
    '        drGrilla("v19") = ""
    '        drGrilla("c20") = ""
    '        drGrilla("v20") = ""
    '        drGrilla("c21") = ""
    '        drGrilla("v21") = ""
    '        dtGrilla.Rows.Add(drGrilla)
    '        Return dtGrilla
    '    End Function
    '    Public Shared Function ObtenerDatosOperacion_LetrasHipotecarias(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
    '        Dim objOI_BE As New OrdenPreOrdenInversionBE
    '        Dim objOI_BM As New OrdenPreOrdenInversionBM
    '        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
    '        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    '        oRow = objOI_BE.Tables(0).Rows(0)
    '        Dim drGrilla As DataRow
    '        Dim dtGrilla As New DataTable
    '        Dim blnEsImpar As Boolean = False
    '        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
    '        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
    '        drGrilla = dtGrilla.NewRow
    '        drGrilla("c1") = "Fecha Operación"
    '        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
    '        drGrilla("c2") = "Fecha Vencimiento"
    '        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
    '        drGrilla("c3") = "Hora Operación"
    '        drGrilla("v3") = oRow.HoraOperacion
    '        drGrilla("c4") = "Monto Nominal Ordenado"
    '        drGrilla("v4") = oRow.MontoNominalOrdenado
    '        drGrilla("c5") = "Monto Nominal Operación"
    '        drGrilla("v5") = oRow.MontoNominalOperacion
    '        drGrilla("c6") = "YTM %"
    '        drGrilla("v6") = Format(oRow.YTM, "##,##0.0000000")
    '        drGrilla("c7") = "Tipo Tasa"
    '        drGrilla("v7") = IIf(oRow.CodigoTipoCupon = "1", "NOMINAL", "EFECTIVA")
    '        drGrilla("c8") = "Precio Calculado"
    '        drGrilla("v8") = Format(oRow.PrecioCalculado, "##,##0.0000000")
    '        drGrilla("c9") = "Precio Negociación Limpio"
    '        drGrilla("v9") = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000000")
    '        drGrilla("c10") = "Interés Corrido"
    '        drGrilla("v10") = Format(oRow.InteresCorrido, "##,##0.0000000")
    '        drGrilla("c11") = "Interés Corrido Negociación"
    '        drGrilla("v11") = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
    '        drGrilla("c12") = "Precio Negociación Sucio"
    '        drGrilla("v12") = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
    '        drGrilla("c13") = "Monto Operación"
    '        drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.0000000")
    '        drGrilla("c14") = "Número Papeles"
    '        drGrilla("v14") = Format(oRow.CantidadOperacion, "##,##0.0000000")
    '        drGrilla("c15") = "Intermediario"
    '        drGrilla("v15") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
    '        If oRow.CodigoContacto <> "" Then
    '            drGrilla("c16") = "Contacto"
    '            drGrilla("v16") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
    '        Else
    '            drGrilla("c16") = ""
    '            drGrilla("v16") = ""
    '        End If
    '        drGrilla("c17") = "Observación"
    '        drGrilla("v17") = oRow.Observacion
    '        If oRow.NumeroPoliza <> "" Then
    '            drGrilla("c18") = "Número Poliza"
    '            drGrilla("v18") = oRow.NumeroPoliza
    '        Else
    '            drGrilla("c18") = ""
    '            drGrilla("v18") = ""
    '        End If
    '        drGrilla("c19") = "Total Comisiones"
    '        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
    '        drGrilla("c20") = "Total Operación"
    '        drGrilla("v20") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
    '        dtGrilla.Rows.Add(drGrilla)
    '        Return dtGrilla
    '    End Function
    '    Public Shared Function ObtenerDatosOperacion_InstrumentosEstructurados(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
    '        Dim objOI_BE As New OrdenPreOrdenInversionBE
    '        Dim objOI_BM As New OrdenPreOrdenInversionBM
    '        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
    '        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    '        oRow = objOI_BE.Tables(0).Rows(0)
    '        Dim drGrilla As DataRow
    '        Dim dtGrilla As New DataTable
    '        Dim blnEsImpar As Boolean = False
    '        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
    '        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
    '        drGrilla = dtGrilla.NewRow
    '        drGrilla("c1") = "Fecha Operación"
    '        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
    '        drGrilla("c2") = "Fecha Vencimiento"
    '        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
    '        drGrilla("c3") = "Hora Operación"
    '        drGrilla("v3") = oRow.HoraOperacion
    '        drGrilla("c4") = "Unidades Ordenadas"
    '        drGrilla("v4") = Format(oRow.CantidadOrdenado, "##,##0.0000000")
    '        drGrilla("c5") = "Unidades Operación"
    '        drGrilla("v5") = Format(oRow.CantidadOperacion, "##,##0.0000000")
    '        drGrilla("c6") = "Precio"
    '        drGrilla("v6") = Format(oRow.Precio, "##,##0.0000000")
    '        drGrilla("c7") = "Monto Operación"
    '        drGrilla("v7") = Format(oRow.MontoOperacion, "##,##0.0000000")
    '        drGrilla("c8") = "Intermediario"
    '        drGrilla("v8") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
    '        If oRow.CodigoContacto <> "" Then
    '            drGrilla("c9") = "Contacto"
    '            drGrilla("v9") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
    '        Else
    '            drGrilla("c9") = ""
    '            drGrilla("v9") = ""
    '        End If
    '        drGrilla("c10") = "Observación"
    '        drGrilla("v10") = oRow.Observacion
    '        If oRow.NumeroPoliza <> "" Then
    '            drGrilla("c11") = "Número Poliza"
    '            drGrilla("v11") = oRow.NumeroPoliza
    '        Else
    '            drGrilla("c11") = ""
    '            drGrilla("v11") = ""
    '        End If
    '        drGrilla("c12") = ""
    '        drGrilla("v12") = ""
    '        drGrilla("c13") = ""
    '        drGrilla("v13") = ""
    '        drGrilla("c14") = ""
    '        drGrilla("v14") = ""
    '        drGrilla("c15") = ""
    '        drGrilla("v15") = ""
    '        drGrilla("c16") = ""
    '        drGrilla("v16") = ""
    '        drGrilla("c17") = ""
    '        drGrilla("v17") = ""
    '        drGrilla("c18") = ""
    '        drGrilla("v18") = ""
    '        drGrilla("c19") = "Total Comisiones"
    '        drGrilla("c20") = "Monto Neto Operación"
    '        drGrilla("c21") = "Precio Promedio"
    '        dtGrilla.Rows.Add(drGrilla)
    '        Return dtGrilla
    '    End Function
    '    Public Shared Function ObtenerDatosOperacion_ForwardyDivisas(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
    '        Dim objOI_BE As New OrdenPreOrdenInversionBE
    '        Dim objOI_BM As New OrdenPreOrdenInversionBM
    '        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
    '        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    '        oRow = objOI_BE.Tables(0).Rows(0)
    '        Dim drGrilla As DataRow
    '        Dim dtGrilla As New DataTable
    '        Dim blnEsImpar As Boolean = False
    '        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21", "c22", "v22"}   'HDG OT 62325 20110323
    '        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
    '        drGrilla = dtGrilla.NewRow
    '        drGrilla("c1") = "Fecha Operación"
    '        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
    '        drGrilla("c2") = "Fecha Contrato"
    '        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaContrato)
    '        drGrilla("c3") = "Hora Operación"
    '        drGrilla("v3") = oRow.HoraOperacion
    '        drGrilla("c4") = "Fecha Liquidación"
    '        drGrilla("v4") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
    '        drGrilla("c5") = "Tipo Cambio Spot"
    '        drGrilla("v5") = Format(oRow.TipoCambioSpot, "##,##0.0000000")
    '        drGrilla("c6") = "Tipo Cambio Futuro"
    '        drGrilla("v6") = Format(oRow.TipoCambioFuturo, "##,##0.0000000")
    '        drGrilla("c7") = "De"
    '        drGrilla("v7") = ObtenerDescripcionMoneda(CType(oRow.CodigoMonedaOrigen, String), datosRequest)
    '        drGrilla("c8") = "Monto Origen"
    '        drGrilla("v8") = Format(oRow.MontoOrigen, "##,##0.0000000")
    '        drGrilla("c9") = "A"
    '        drGrilla("v9") = ObtenerDescripcionMoneda(CType(oRow.CodigoMonedaDestino, String), datosRequest)
    '        drGrilla("c10") = "Monto Futuro"
    '        drGrilla("v10") = Format(oRow.MontoOperacion, "##,##0.0000000")
    '        drGrilla("c11") = "Plazo"
    '        drGrilla("v11") = oRow.Plazo
    '        drGrilla("c12") = "Diferencial"
    '        drGrilla("v12") = Format(oRow.Diferencial, "##,##0.0000000")
    '        drGrilla("c13") = "Modalidad Compra"
    '        If oRow.Delibery <> "" Then
    '            Select Case oRow.Delibery
    '                Case "N"
    '                    drGrilla("v13") = "Non - Delivery"
    '                Case "S"
    '                    drGrilla("v13") = "Delivery"
    '            End Select
    '        Else
    '            drGrilla("v13") = ""
    '        End If
    '        drGrilla("c14") = "Intermediario"
    '        drGrilla("v14") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
    '        drGrilla("c16") = "Observacion"
    '        If oRow.Observacion <> "" Then
    '            drGrilla("v16") = oRow.Observacion
    '        Else
    '            drGrilla("v16") = ""
    '        End If
    '        drGrilla("c17") = ""
    '        drGrilla("v17") = ""
    '        drGrilla("c18") = "Motivo"
    '        drGrilla("v18") = ObtenerDescripcionMotivo(oRow.CodigoMotivo, datosRequest)
    '        drGrilla("c19") = ""
    '        drGrilla("v19") = ""
    '        drGrilla("c20") = ""
    '        drGrilla("v20") = ""
    '        drGrilla("c21") = ""
    '        drGrilla("v21") = ""
    '        drGrilla("c22") = "Cobertura"
    '        drGrilla("v22") = objOI_BM.SeleccionarTipoMonedaxMotivoForw(oRow.CodigoMotivo, oRow.TipoMonedaForw).Rows(0)("Descripcion")   'HDG OT 62325 20110323
    '        dtGrilla.Rows.Add(drGrilla)
    '        Return dtGrilla
    '    End Function
    '    Public Shared Function ObtenerDatosOperacion_CompraVentaMonedaExtranjera(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
    '        Dim objOI_BE As New OrdenPreOrdenInversionBE
    '        Dim objOI_BM As New OrdenPreOrdenInversionBM
    '        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
    '        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
    '        oRow = objOI_BE.Tables(0).Rows(0)
    '        Dim drGrilla As DataRow
    '        Dim dtGrilla As New DataTable
    '        Dim blnEsImpar As Boolean = False
    '        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
    '        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
    '        drGrilla = dtGrilla.NewRow
    '        drGrilla("c1") = "Fecha Operación"
    '        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
    '        drGrilla("c2") = "Fecha Liquidación"
    '        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
    '        drGrilla("c3") = "Hora Operación"
    '        drGrilla("v3") = oRow.HoraOperacion
    '        drGrilla("c4") = "De"
    '        drGrilla("v4") = ObtenerDescripcionMoneda(CType(oRow.CodigoMoneda, String), datosRequest)
    '        drGrilla("c5") = "Monto Divisa Negociada"
    '        drGrilla("v5") = Format(oRow.MontoOperacion, "##,##0.0000000")
    '        drGrilla("c6") = "A"
    '        drGrilla("v6") = ObtenerDescripcionMoneda(CType(oRow.CodigoMonedaDestino, String), datosRequest)
    '        drGrilla("c7") = "Monto"
    '        drGrilla("v7") = Format(oRow.MontoDestino, "##,##0.0000000")
    '        drGrilla("c8") = "Tipo Cambio"
    '        drGrilla("v8") = Format(oRow.TipoCambio, "##,##0.0000000")
    '        drGrilla("c9") = "Intermediario"
    '        drGrilla("v9") = ObtenerDescripcionTercero(CType(oRow.CodigoTercero, String), datosRequest)
    '        drGrilla("c10") = "Contacto"
    '        drGrilla("v10") = ObtenerDescripcionContacto(CType(oRow.CodigoContacto, String), datosRequest)
    '        drGrilla("c11") = ""
    '        drGrilla("v11") = ""
    '        drGrilla("c12") = "Observación"
    '        drGrilla("v12") = oRow.Observacion
    '        drGrilla("c13") = ""
    '        drGrilla("v13") = ""
    '        drGrilla("c14") = ""
    '        drGrilla("v14") = ""
    '        drGrilla("c16") = ""
    '        drGrilla("v16") = ""
    '        drGrilla("c17") = ""
    '        drGrilla("v17") = ""
    '        drGrilla("c18") = ""
    '        drGrilla("v18") = ""
    '        drGrilla("c19") = ""
    '        drGrilla("v19") = ""
    '        drGrilla("c20") = ""
    '        drGrilla("v20") = ""
    '        drGrilla("c21") = ""
    '        drGrilla("v21") = ""
    '        dtGrilla.Rows.Add(drGrilla)
    '        Return dtGrilla
    '    End Function
    '#End Region
    '#Region "/* PDF (Vol. 12) */"
    '    Public Shared Sub CrearArchivoPDF(ByVal TablaText As DataTable, ByVal HtmlPath As String, ByVal PdfPath As String)
    '        Try
    '            Dim document As New Document(PageSize.A4, 55, 55, 20, 6)
    '            Dim oStreamReader As New StreamReader(HtmlPath, System.Text.Encoding.Default)
    '            Dim styles As New iTextSharp.text.html.simpleparser.StyleSheet
    '            Dim hw As New iTextSharp.text.html.simpleparser.HTMLWorker(document)
    '            Dim oIElement As IElement
    '            Dim oParagraph As Paragraph
    '            Dim oPdfPTable As PdfPTable
    '            Dim oPdfPCell As PdfPCell
    '            Dim objects As List(Of iTextSharp.text.IElement)
    '            Dim strContent As String
    '            PdfWriter.GetInstance(document, New FileStream(PdfPath, FileMode.Create))
    '            document.Open()
    '            document.NewPage()
    '            objects = hw.ParseToList(oStreamReader, styles)
    '            For k As Integer = 0 To objects.Count - 1
    '                oIElement = CType(objects(k), IElement)
    '                If objects(k).GetType().FullName = "iTextSharp.text.Paragraph" Then
    '                    oParagraph = New Paragraph
    '                    oParagraph.Alignment = CType(objects(k), Paragraph).Alignment
    '                    For z As Integer = 0 To oIElement.Chunks.Count - 1
    '                        strContent = ReplaceText(oIElement.Chunks(z).Content, TablaText)
    '                        oParagraph.Add(New Chunk(strContent, oIElement.Chunks(z).Font))
    '                        oParagraph.Leading = 11
    '                    Next
    '                    document.Add(oParagraph)
    '                ElseIf objects(k).GetType().FullName = "iTextSharp.text.pdf.PdfPTable" Then
    '                    oPdfPTable = CType(objects(k), PdfPTable)
    '                    Dim oNewPdfPTable As PdfPTable = New PdfPTable(oPdfPTable.NumberOfColumns)
    '                    Dim DimensionColumna(oPdfPTable.NumberOfColumns - 1) As Integer
    '                    Dim aux As Integer
    '                    'oNewPdfPTable.WidthPercentage = 100
    '                    Dim imgFirma1 As String = ""
    '                    Dim imgFirma2 As String = ""
    '                    Dim jpg As iTextSharp.text.Image
    '                    For row As Integer = 0 To oPdfPTable.Rows.Count - 1
    '                        For cell As Integer = 0 To oPdfPTable.Rows(row).GetCells().Length - 1
    '                            oPdfPCell = oPdfPTable.Rows(row).GetCells()(cell)
    '                            oParagraph = New Paragraph
    '                            For paragraph As Integer = 0 To oPdfPTable.Rows(row).GetCells()(cell).CompositeElements.Count - 1
    '                                For chunk As Integer = 0 To oPdfPTable.Rows(row).GetCells()(cell).CompositeElements(paragraph).Chunks.Count - 1
    '                                    If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Entidad]" Or _
    '                                        oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Direccion]" Or _
    '                                        oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Url]") Then
    '                                        strContent = ""
    '                                        If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Entidad]") Then
    '                                            strContent = "Fondos Sura"
    '                                        End If
    '                                        If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Direccion]") Then
    '                                            strContent = "Canaval y Moreyra 522 San Isidro. Teléfono: 411-9191. Fax: 411-9192"
    '                                        End If
    '                                        If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Url]") Then
    '                                            strContent = "www.sura.pe"
    '                                        End If
    '                                        oParagraph.Add(New Chunk(strContent, oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Font))
    '                                        aux = Len(strContent)
    '                                        If aux > DimensionColumna(cell) Then
    '                                            DimensionColumna(cell) = aux
    '                                        End If
    '                                        If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Url]") Then
    '                                            oParagraph.Leading = 10
    '                                        Else
    '                                            oParagraph.Leading = 6
    '                                        End If
    '                                    ElseIf oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content <> "[Firma1]" And _
    '                                    oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content <> "[Firma2]" And _
    '                                    oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content <> "[Logo]" Then
    '                                        strContent = ReplaceText(oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content, TablaText)
    '                                        oParagraph.Add(New Chunk(strContent, oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Font))
    '                                        aux = Len(strContent)
    '                                        If aux > DimensionColumna(cell) Then
    '                                            DimensionColumna(cell) = 1 'aux
    '                                        End If
    '                                        oParagraph.Leading = 11
    '                                    Else
    '                                        If oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[Firma1]" Then
    '                                            For Each dr As DataRow In TablaText.Rows
    '                                                If dr("Find") = "[Firma1]" Then
    '                                                    imgFirma1 = CType(dr("New"), String)
    '                                                End If
    '                                            Next
    '                                            If imgFirma1 <> "" Then
    '                                                jpg = iTextSharp.text.Image.GetInstance(imgFirma1)
    '                                                jpg.ScaleToFit(120.0F, 50.0F)
    '                                                jpg.SpacingBefore = 5.0F
    '                                                jpg.SpacingAfter = 1.0F
    '                                                jpg.Alignment = Element.ALIGN_LEFT
    '                                            End If
    '                                        End If
    '                                        If oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[Firma2]" Then
    '                                            For Each dr As DataRow In TablaText.Rows
    '                                                If dr("Find") = "[Firma2]" Then
    '                                                    imgFirma2 = CType(dr("New"), String)
    '                                                End If
    '                                            Next
    '                                            If imgFirma2 <> "" Then
    '                                                jpg = iTextSharp.text.Image.GetInstance(imgFirma2)
    '                                                jpg.ScaleToFit(120.0F, 50.0F)
    '                                                jpg.SpacingBefore = 5.0F
    '                                                jpg.SpacingAfter = 1.0F
    '                                                jpg.Alignment = Element.ALIGN_LEFT
    '                                            End If
    '                                        End If
    '                                        If oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[Logo]" Then
    '                                            For Each dr As DataRow In TablaText.Rows
    '                                                If dr("Find") = "[Logo]" Then
    '                                                    imgFirma2 = New BasePage().Ruta_Logo()
    '                                                End If
    '                                            Next
    '                                            If imgFirma2 <> "" Then
    '                                                jpg = iTextSharp.text.Image.GetInstance(imgFirma2)
    '                                                jpg.ScaleToFit(127.0F, 40.0F)
    '                                                jpg.SpacingBefore = 5.0F
    '                                                jpg.SpacingAfter = 1.0F
    '                                                jpg.Alignment = Element.ALIGN_RIGHT
    '                                            End If
    '                                        End If
    '                                    End If
    '                                Next
    '                                aux = 0
    '                            Next
    '                            oPdfPCell.CompositeElements.Clear()
    '                            oPdfPCell.AddElement(oParagraph)
    '                            If Not jpg Is Nothing Then
    '                                oPdfPCell.AddElement(jpg)
    '                                jpg = Nothing
    '                            End If
    '                            oNewPdfPTable.AddCell(oPdfPCell)
    '                        Next
    '                    Next
    '                    oNewPdfPTable.SetWidths(CalcularDimensiones(DimensionColumna))
    '                    document.Add(oNewPdfPTable)
    '                End If
    '            Next
    '            document.Close()
    '            oStreamReader.Close()
    '        Catch ex As Exception
    '            Throw ex
    '        End Try
    '    End Sub
    '    Public Shared Function CalcularDimensiones(ByVal Columnas() As Integer) As Integer()
    '        Dim total As Integer
    '        Dim i As Integer
    '        For i = 0 To Columnas.Length - 1
    '            If Columnas(i) = 0 Then
    '                Columnas(i) = 10
    '            End If
    '            total = total + Columnas(i)
    '        Next
    '        For i = 0 To Columnas.Length - 1
    '            Columnas(i) = (Columnas(i) / total) * 100
    '        Next
    '        Return Columnas
    '    End Function
    '    Shared Function ReplaceText(ByVal TextBase As String, ByVal TableText As DataTable) As String
    '        Try
    '            For x As Integer = 0 To TableText.Rows.Count - 1
    '                If TextBase.IndexOf("[") = -1 And TextBase.IndexOf("]") = -1 Then Exit For
    '                If TextBase.IndexOf(TableText.Rows(x)("Find")) > -1 Then TextBase = TextBase.Replace(TableText.Rows(x)("Find"), TableText.Rows(x)("New"))
    '            Next
    '            Return TextBase
    '        Catch ex As Exception
    '            Return ""
    '        End Try
    '    End Function
    '#End Region
    '#Region "Publicar Msj Accesible (CarlosRumiche)"
    '    ''' <summary>
    '    ''' Publica un mensaje en el EventLog del equipo en el cual se ejecuta las aplicación. 
    '    ''' Ademas publica el mensaje en el archivo (\\plutonvm\archivosplanos\Log\LogEventosSIT.xml), 
    '    ''' o en el archivo definido por en el Web.Config en la clave AppSettings("LogXml_Ruta")
    '    ''' </summary>
    '    Public Shared Sub PublicarEvento(ByVal msj As String)
    '        LogXml.PublicarMensaje("P " & msj)
    '    End Sub
    '    Public Class LogXml
    '        Public Shared Sub PublicarException(ByVal Excepcion As Exception)
    '            Try
    '                Dim pathArchivoXml As String = RUTA_LOG
    '                If Not ConfigurationSettings.AppSettings("LogXml_Ruta") Is Nothing Then
    '                    If ConfigurationSettings.AppSettings("LogXml_Ruta").Trim.Length > 0 Then
    '                        pathArchivoXml = ConfigurationSettings.AppSettings("LogXml_Ruta")
    '                    End If
    '                End If
    '                Dim documentoXml As New XmlDocument
    '                Dim nodoXmlPadre As System.Xml.XmlNode
    '                If File.Exists(pathArchivoXml) Then
    '                    documentoXml.Load(pathArchivoXml)
    '                    nodoXmlPadre = documentoXml.Item("ListaExcepciones")
    '                Else
    '                    documentoXml.AppendChild(documentoXml.CreateXmlDeclaration("1.0", "utf-8", String.Empty))
    '                    nodoXmlPadre = documentoXml.AppendChild(documentoXml.CreateElement("ListaExcepciones"))
    '                End If
    '                Dim nodoXmlExcepcion As System.Xml.XmlNode = nodoXmlPadre.AppendChild(documentoXml.CreateElement("Excepcion"))
    '                Dim nodoXmlFecha As System.Xml.XmlNode = nodoXmlExcepcion.AppendChild(documentoXml.CreateElement("Fecha"))
    '                Dim nodoXmlMensaje As System.Xml.XmlNode = nodoXmlExcepcion.AppendChild(documentoXml.CreateElement("MensajeError"))
    '                nodoXmlFecha.InnerText = Now.ToString("yyyy-MM-dd hh:mm:ss tt")
    '                nodoXmlMensaje.InnerText = Excepcion.Message
    '                documentoXml.Save(pathArchivoXml)
    '            Catch ex As Exception
    '            End Try
    '        End Sub
    '        Public Shared Sub PublicarMensaje(ByVal mensaje As String)
    '            Try
    '                Dim pathArchivoXml As String = RUTA_LOG
    '                If Not ConfigurationSettings.AppSettings("LogXml_Ruta") Is Nothing Then
    '                    If ConfigurationSettings.AppSettings("LogXml_Ruta").Trim.Length > 0 Then
    '                        pathArchivoXml = ConfigurationSettings.AppSettings("LogXml_Ruta")
    '                    End If
    '                End If
    '                Dim documentoXml As New XmlDocument
    '                Dim nodoXmlPadre As System.Xml.XmlNode
    '                If File.Exists(pathArchivoXml) Then
    '                    documentoXml.Load(pathArchivoXml)
    '                    nodoXmlPadre = documentoXml.Item("ListaExcepciones")
    '                Else
    '                    documentoXml.AppendChild(documentoXml.CreateXmlDeclaration("1.0", "utf-8", String.Empty))
    '                    nodoXmlPadre = documentoXml.AppendChild(documentoXml.CreateElement("ListaExcepciones"))
    '                End If
    '                Dim nodoXmlExcepcion As System.Xml.XmlNode = nodoXmlPadre.AppendChild(documentoXml.CreateElement("Excepcion"))
    '                Dim nodoXmlFecha As System.Xml.XmlNode = nodoXmlExcepcion.AppendChild(documentoXml.CreateElement("Fecha"))
    '                Dim nodoXmlMensaje As System.Xml.XmlNode = nodoXmlExcepcion.AppendChild(documentoXml.CreateElement("Mensaje"))
    '                nodoXmlFecha.InnerText = Now.ToString("yyyyMMdd hh:mm:ss tt")
    '                nodoXmlMensaje.InnerText = mensaje
    '                documentoXml.Save(pathArchivoXml)
    '            Catch ex As Exception
    '            End Try
    '        End Sub
    '    End Class
    '#End Region
    '    Public Shared Function GeneraTablaResumenTotalInstrumentos(ByVal strFechaInicio As String, ByVal strFechaFin As String, ByVal Portafolio As String, ByVal DatosRequest As DataSet) As DataTable
    '        Dim dtFinal As New DataTable
    '        dtFinal.Columns.Add("TipoInstrumento", GetType(String))
    '        dtFinal.Columns.Add("CodigoNemonico", GetType(String))
    '        dtFinal.Columns.Add("Condicion", GetType(String))
    '        dtFinal.Columns.Add("UtilidadTotal", GetType(Double))
    '        dtFinal.Columns.Add("UtilidadEncaje", GetType(Double))
    '        dtFinal.Columns.Add("UtilidadVentaTotal", GetType(Double))
    '        dtFinal.Columns.Add("UtilidadEncajeVenta", GetType(Double))
    '        dtFinal.Columns.Add("MetodoCalculoRenta", GetType(String))
    '        dtFinal.Columns.Add("ValorVenta", GetType(String))
    '        dtFinal.Columns.Add("CostoValorVendido", GetType(String))
    '        dtFinal.Columns.Add("TotalInteresDividendo", GetType(String))
    '        dtFinal.Columns.Add("TotalInteres", GetType(Double))
    '        dtFinal.Columns.Add("TotalDividendo", GetType(Double))
    '        Dim FechaInicio As Decimal = ConvertirFechaaDecimal(strFechaInicio)
    '        Dim FechaFin As Decimal = ConvertirFechaaDecimal(strFechaFin)
    '        Dim dtConsulta As DataTable = New ReporteGestionBM().ReporteResumenRentabilidadEncajeTotalInstrumentos(FechaInicio, FechaFin, Portafolio, DatosRequest).Tables(0)
    '        Dim dtDetalle As DataTable
    '        Dim drInstrumento As DataRow
    '        Dim drFinal As DataRow
    '        Dim CodigoNemonico As String
    '        Dim TotalVentas As Decimal
    '        Dim CostoVentas As Decimal
    '        For Each drInstrumento In dtConsulta.Rows
    '            CodigoNemonico = drInstrumento("CodigoNemonico")
    '            If (CodigoNemonico.Equals("BCR10S13")) Then
    '                Dim pararm = Nothing
    '            End If
    '            dtDetalle = GeneraTablaDetallePorInstrumento(CodigoNemonico, FechaInicio, FechaFin, Portafolio, DatosRequest)
    '            CostoVentas = dtDetalle.Compute("sum(ImporteKardex)", "TipoOperacion='COSTO_VENTAS'")
    '            TotalVentas = dtDetalle.Compute("sum(ImporteKardex)", "TipoOperacion='TOTAL_VENTAS'")
    '            drFinal = dtFinal.NewRow
    '            drFinal("TipoInstrumento") = drInstrumento("TipoInstrumento")
    '            drFinal("CodigoNemonico") = CodigoNemonico
    '            drFinal("Condicion") = drInstrumento("Condicion")
    '            drFinal("UtilidadTotal") = ToNullDecimal(drInstrumento("UtilidadTotal"))
    '            drFinal("UtilidadEncaje") = ToNullDecimal(drInstrumento("UtilidadEncaje"))
    '            drFinal("ValorVenta") = TotalVentas
    '            drFinal("CostoValorVendido") = CostoVentas
    '            drFinal("TotalInteresDividendo") = ToNullDecimal(drInstrumento("TotalInteresDividendo"))
    '            drFinal("TotalInteres") = ToNullDecimal(drInstrumento("TotalInteres"))
    '            drFinal("TotalDividendo") = ToNullDecimal(drInstrumento("TotalDividendo"))
    '            If drInstrumento("MetodoCalculoRenta") = "1" Then
    '                drFinal("UtilidadVentaTotal") = TotalVentas - CostoVentas
    '                If drInstrumento("UtilidadTotal") = 0 Then
    '                    drFinal("UtilidadEncajeVenta") = 0
    '                Else
    '                    drFinal("UtilidadEncajeVenta") = (TotalVentas - CostoVentas) * ToNullDecimal(drInstrumento("UtilidadEncaje")) / ToNullDecimal(drInstrumento("UtilidadTotal"))
    '                End If
    '            End If
    '            drFinal("MetodoCalculoRenta") = ToNullDecimal(drInstrumento("MetodoCalculoRenta"))
    '            dtFinal.Rows.Add(drFinal)
    '        Next
    '        Return dtFinal
    '    End Function
    '#Region "Validadciones de numeros"
    '    Public Shared Function ToNullDecimal(ByVal valor As Object) As Decimal
    '        Dim d_Numero As Decimal = 0
    '        Try
    '            d_Numero = Convert.ToDecimal(valor)
    '        Catch ex As Exception
    '            d_Numero = 0
    '        End Try
    '        Return d_Numero
    '    End Function
    '    Public Shared Function ToNullDouble(ByVal valor As Object) As Double
    '        Dim d_Numero As Double = 0
    '        Try
    '            d_Numero = Convert.ToDouble(valor)
    '        Catch ex As Exception
    '            d_Numero = 0
    '        End Try
    '        Return d_Numero
    '    End Function
    '    Public Shared Function ToNullInt32(ByVal valor As Object) As Integer
    '        Dim d_Numero As Integer = 0
    '        Try
    '            d_Numero = Convert.ToInt32(valor)
    '        Catch ex As Exception
    '            d_Numero = 0
    '        End Try
    '        Return d_Numero
    '    End Function
    '#End Region
    '    Public Shared Function GeneraTablaDetallePorInstrumento(ByVal CodigoNemonico As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal Portafolio As String, ByVal DatosRequest As DataSet) As DataTable
    '        Try
    '            Dim dtDetalleFinal As New DataTable
    '            dtDetalleFinal.Columns.Add("TipoOperacion", GetType(String))
    '            dtDetalleFinal.Columns.Add("Fecha", GetType(String))
    '            dtDetalleFinal.Columns.Add("Unidades", GetType(Decimal))
    '            dtDetalleFinal.Columns.Add("Precio", GetType(Decimal))
    '            dtDetalleFinal.Columns.Add("TipoCambio", GetType(Decimal))
    '            dtDetalleFinal.Columns.Add("UnidadesKardex", GetType(Decimal))
    '            dtDetalleFinal.Columns.Add("ImporteKardex", GetType(Decimal))
    '            dtDetalleFinal.Columns.Add("PrecioMO", GetType(Decimal))
    '            dtDetalleFinal.Columns.Add("ImporteKardexMO", GetType(Decimal))
    '            Dim UnidadesVendidas As Decimal = 0
    '            Dim UnidadesCompradas As Decimal = 0
    '            Dim ImporteCompras As Decimal = 0
    '            Dim ImporteVentas As Decimal = 0
    '            Dim ImporteVentas2 As Decimal = 0
    '            Dim ImporteInicial As Decimal = 0
    '            Dim PrecioPromedio As Decimal = 0
    '            Dim UnidadesPrimeraVenta As Decimal = 0
    '            Dim ImporteInicialMO As Decimal = 0
    '            Dim PrecioPromedioMO As Decimal = 0
    '            Dim ImporteComprasMO As Decimal = 0
    '            Dim ImporteVentasMO As Decimal = 0
    '            Dim ImporteVentas2MO As Decimal = 0
    '            Dim dsRenta As DataSet
    '            Dim oReporte As New EncajeDetalleBM
    '            dsRenta = oReporte.ReporteImpuestoRentaAnualPorNemonico(CodigoNemonico, Portafolio, FechaInicio, FechaFin, DatosRequest)
    '            Dim dtSaldoInicial As DataTable = dsRenta.Tables(0)
    '            Dim dtOperaciones As DataTable = dsRenta.Tables(1)
    '            Dim dtTotalVentas As DataTable = dsRenta.Tables(2)
    '            Dim drSaldoInicial As DataRow
    '            Dim drDetalleFinal As DataRow
    '            Dim ClaseInstrumento As String = ""
    '            Dim FechaPrimerDia As Decimal = PRIMER_DIA_OPERACION
    '            If dsRenta.Tables.Count = 4 Then
    '                ClaseInstrumento = dsRenta.Tables(3).Rows(0)("CodigoClaseInstrumento").ToString
    '            End If
    '            If (ClaseInstrumento = "10" Or ClaseInstrumento = "12") And FechaInicio > 20121231 And FechaFin > 20121231 Then
    '                FechaPrimerDia = FechaInicio
    '            End If
    '            If dtSaldoInicial.Rows.Count > 0 Then
    '                drSaldoInicial = dtSaldoInicial.Rows(0)
    '                drDetalleFinal = dtDetalleFinal.NewRow
    '                drDetalleFinal("TipoOperacion") = "SALDO_INICIAL"
    '                drDetalleFinal("Fecha") = drSaldoInicial("FechaInicial")
    '                Try
    '                    drDetalleFinal("Unidades") = ToNullDecimal(drSaldoInicial("SaldoAnt"))
    '                    If drDetalleFinal("Unidades").ToString.Trim = "" Then
    '                        drDetalleFinal("Unidades") = ToNullDecimal(drSaldoInicial("SaldoInicial"))
    '                    End If
    '                Catch ex As Exception
    '                    drDetalleFinal("Unidades") = ToNullDecimal(drSaldoInicial("SaldoInicial"))
    '                End Try
    '                drDetalleFinal("Precio") = ToNullDecimal(drSaldoInicial("ValorPrecioSIMO"))
    '                drDetalleFinal("TipoCambio") = ToNullDecimal(drSaldoInicial("TipoCambioMO"))
    '                ImporteInicial = ToNullDecimal(drSaldoInicial("SaldoInicial")) * ToNullDecimal(drSaldoInicial("ValorPrecio")) * ToNullDecimal(drSaldoInicial("TipoCambio")) 'RGF 20101228 OT 61609
    '                ImporteInicialMO = ToNullDecimal(drSaldoInicial("SaldoInicial")) * ToNullDecimal(drSaldoInicial("ValorPrecio")) 'HDG
    '                dtDetalleFinal.Rows.Add(drDetalleFinal)
    '            End If
    '            Dim StockAgotado As Boolean = False
    '            Dim drOperacion As DataRow
    '            Dim SaldosinCompras As Integer = 0
    '            For Each drOperacion In dtOperaciones.Rows
    '                drDetalleFinal = dtDetalleFinal.NewRow
    '                SaldosinCompras = SaldosinCompras + 1
    '                If drOperacion("CodigoOperacion") = "2" Then
    '                    drDetalleFinal("TipoOperacion") = "VENTA"
    '                    If FechaPrimerDia.ToString.Substring(0, 4) - 1 <> SALDO_ANIO_ANTERIOR Then StockAgotado = True
    '                    UnidadesVendidas = UnidadesVendidas + ToNullDecimal(drOperacion("Unidades"))
    '                    If UnidadesVendidas > ToNullDecimal(drSaldoInicial("SaldoInicial")) Or FechaPrimerDia.ToString.Substring(0, 4) - 1 <> SALDO_ANIO_ANTERIOR Then
    '                        If StockAgotado Then
    '                            If SaldosinCompras = 1 Then
    '                                If (ClaseInstrumento = "10" Or ClaseInstrumento = "12") And FechaInicio > 20121231 And FechaFin > 20121231 Then
    '                                    PrecioPromedio = drSaldoInicial("ValorPrecio") * drSaldoInicial("TipoCambio")
    '                                    PrecioPromedioMO = drSaldoInicial("ValorPrecio")
    '                                Else
    '                                    PrecioPromedio = drSaldoInicial("ValorPrecio")
    '                                    PrecioPromedioMO = drSaldoInicial("ValorPrecio")
    '                                End If
    '                            End If
    '                            ImporteVentas = ImporteVentas + (drOperacion("Unidades") * PrecioPromedio)
    '                            ImporteVentasMO = ImporteVentasMO + (drOperacion("Unidades") * PrecioPromedioMO)
    '                            drDetalleFinal("Fecha") = drOperacion("Fecha")
    '                            drDetalleFinal("Unidades") = drOperacion("Unidades")
    '                            drDetalleFinal("Precio") = PrecioPromedio
    '                            drDetalleFinal("TipoCambio") = 1
    '                            drDetalleFinal("PrecioMO") = PrecioPromedioMO
    '                            If Convert.ToDecimal(drOperacion("FechaOperacion")) >= FechaInicio Then
    '                                ImporteVentas2 = ImporteVentas2 + (drOperacion("Unidades") * PrecioPromedio)
    '                                ImporteVentas2MO = ImporteVentas2MO + (drOperacion("Unidades") * PrecioPromedioMO)
    '                                dtDetalleFinal.Rows.Add(drDetalleFinal)
    '                            End If
    '                        Else 'Se abre la venta en 2 operaciones (Punto de quiebre)
    '                            'Primera venta a precio vector
    '                            UnidadesPrimeraVenta = drSaldoInicial("SaldoInicial") - UnidadesVendidas + drOperacion("Unidades")
    '                            ImporteVentas = ImporteVentas + UnidadesPrimeraVenta * drSaldoInicial("ValorPrecio") * drSaldoInicial("TipoCambio")
    '                            ImporteVentasMO = ImporteVentasMO + UnidadesPrimeraVenta * drSaldoInicial("ValorPrecio")
    '                            If UnidadesPrimeraVenta > 0 Then
    '                                drDetalleFinal("Fecha") = drOperacion("Fecha")
    '                                drDetalleFinal("Unidades") = UnidadesPrimeraVenta
    '                                drDetalleFinal("Precio") = drSaldoInicial("ValorPrecio")
    '                                drDetalleFinal("TipoCambio") = drSaldoInicial("TipoCambio")
    '                                drDetalleFinal("PrecioMO") = drSaldoInicial("ValorPrecio")
    '                                If Convert.ToDecimal(drOperacion("FechaOperacion")) >= FechaInicio Then
    '                                    ImporteVentas2 = ImporteVentas2 + UnidadesPrimeraVenta * drSaldoInicial("ValorPrecio") * drSaldoInicial("TipoCambio")
    '                                    ImporteVentas2MO = ImporteVentas2MO + UnidadesPrimeraVenta * drSaldoInicial("ValorPrecio")
    '                                    dtDetalleFinal.Rows.Add(drDetalleFinal)
    '                                End If
    '                            End If
    '                            'Segunda venta a precio promedio
    '                            ImporteVentas = ImporteVentas + (UnidadesVendidas - drSaldoInicial("SaldoInicial")) * PrecioPromedio
    '                            ImporteVentasMO = ImporteVentasMO + (UnidadesVendidas - drSaldoInicial("SaldoInicial")) * PrecioPromedioMO
    '                            drDetalleFinal = dtDetalleFinal.NewRow
    '                            drDetalleFinal("TipoOperacion") = "VENTA"
    '                            drDetalleFinal("Fecha") = drOperacion("Fecha")
    '                            drDetalleFinal("Unidades") = UnidadesVendidas - drSaldoInicial("SaldoInicial")
    '                            drDetalleFinal("Precio") = PrecioPromedio
    '                            drDetalleFinal("TipoCambio") = 1
    '                            drDetalleFinal("PrecioMO") = PrecioPromedioMO
    '                            If Convert.ToDecimal(drOperacion("FechaOperacion")) >= FechaInicio Then
    '                                ImporteVentas2 = ImporteVentas2 + (UnidadesVendidas - drSaldoInicial("SaldoInicial")) * PrecioPromedio
    '                                ImporteVentas2MO = ImporteVentas2MO + (UnidadesVendidas - drSaldoInicial("SaldoInicial")) * PrecioPromedioMO
    '                                dtDetalleFinal.Rows.Add(drDetalleFinal)
    '                            End If
    '                            StockAgotado = True
    '                        End If
    '                    Else
    '                        ImporteVentas = ImporteVentas + drOperacion("Unidades") * drSaldoInicial("ValorPrecio") * drSaldoInicial("TipoCambio")
    '                        ImporteVentasMO = ImporteVentasMO + drOperacion("Unidades") * drSaldoInicial("ValorPrecioSIMO")
    '                        drDetalleFinal("Fecha") = drOperacion("Fecha")
    '                        drDetalleFinal("Unidades") = drOperacion("Unidades")
    '                        drDetalleFinal("Precio") = drSaldoInicial("ValorPrecio")
    '                        drDetalleFinal("TipoCambio") = drSaldoInicial("TipoCambio")
    '                        drDetalleFinal("PrecioMO") = drSaldoInicial("ValorPrecioSIMO")
    '                        If Convert.ToDecimal(drOperacion("FechaOperacion")) >= FechaInicio Then
    '                            ImporteVentas2 = ImporteVentas2 + drOperacion("Unidades") * drSaldoInicial("ValorPrecio") * drSaldoInicial("TipoCambio")
    '                            ImporteVentas2MO = ImporteVentas2MO + drOperacion("Unidades") * drSaldoInicial("ValorPrecioSIMO")
    '                            dtDetalleFinal.Rows.Add(drDetalleFinal)
    '                        End If
    '                    End If
    '                Else 'COMPRA
    '                    UnidadesCompradas = UnidadesCompradas + ToNullDecimal(drOperacion("Unidades"))
    '                    ImporteCompras = ImporteCompras + (ToNullDecimal(drOperacion("Unidades")) * ToNullDecimal(drOperacion("Precio")) * ToNullDecimal(drOperacion("TipoCambio")))
    '                    ImporteComprasMO = ImporteComprasMO + (ToNullDecimal(drOperacion("Unidades")) * ToNullDecimal(drOperacion("Precio"))) 'HDG
    '                    'KARDEX PRECIO PROMEDIO
    '                    drDetalleFinal("TipoOperacion") = "COMPRA"
    '                    drDetalleFinal("Fecha") = drOperacion("Fecha")
    '                    drDetalleFinal("Unidades") = ToNullDecimal(drOperacion("Unidades"))
    '                    drDetalleFinal("Precio") = ToNullDecimal(drOperacion("Precio"))
    '                    drDetalleFinal("TipoCambio") = ToNullDecimal(drOperacion("TipoCambio"))
    '                    drDetalleFinal("PrecioMO") = ToNullDecimal(drOperacion("Precio")) 'HDG
    '                    If StockAgotado Then
    '                        drDetalleFinal("UnidadesKardex") = UnidadesCompradas + drSaldoInicial("SaldoInicial") - UnidadesVendidas 'RGF 20101228 OT 61609
    '                        drDetalleFinal("ImporteKardex") = ImporteCompras + ImporteInicial - ImporteVentas 'RGF 20101228 OT 61609
    '                        drDetalleFinal("ImporteKardexMO") = ImporteComprasMO + ImporteInicialMO - ImporteVentasMO 'HDG
    '                    Else
    '                        If FechaPrimerDia.ToString.Substring(0, 4) - 1 = SALDO_ANIO_ANTERIOR Then  'HDG INC 66297 20121025
    '                            drDetalleFinal("UnidadesKardex") = UnidadesCompradas
    '                            drDetalleFinal("ImporteKardex") = ImporteCompras
    '                            drDetalleFinal("ImporteKardexMO") = ImporteComprasMO 'HDG
    '                        Else
    '                            drDetalleFinal("UnidadesKardex") = UnidadesCompradas + drSaldoInicial("SaldoInicial")
    '                            drDetalleFinal("ImporteKardex") = ImporteCompras + ImporteInicial
    '                            drDetalleFinal("ImporteKardexMO") = ImporteComprasMO + ImporteInicialMO 'HDG
    '                        End If
    '                    End If
    '                    If drDetalleFinal("UnidadesKardex") = 0 Then
    '                        PrecioPromedio = 0
    '                        PrecioPromedioMO = 0 'HDG
    '                    Else
    '                        PrecioPromedio = drDetalleFinal("ImporteKardex") / drDetalleFinal("UnidadesKardex")
    '                        PrecioPromedioMO = drDetalleFinal("ImporteKardexMO") / drDetalleFinal("UnidadesKardex")
    '                    End If
    '                    If Convert.ToDecimal(drOperacion("FechaOperacion")) >= FechaInicio Then
    '                        dtDetalleFinal.Rows.Add(drDetalleFinal)
    '                    End If
    '                End If
    '            Next
    '            drDetalleFinal = dtDetalleFinal.NewRow
    '            drDetalleFinal("TipoOperacion") = "COSTO_VENTAS"
    '            drDetalleFinal("ImporteKardex") = ImporteVentas2
    '            drDetalleFinal("ImporteKardexMO") = ImporteVentas2MO
    '            dtDetalleFinal.Rows.Add(drDetalleFinal)
    '            drDetalleFinal = dtDetalleFinal.NewRow
    '            drDetalleFinal("TipoOperacion") = "TOTAL_VENTAS"
    '            If dtTotalVentas.Rows.Count > 0 Then
    '                drDetalleFinal("ImporteKardex") = dtTotalVentas.Rows(0)("TotalVentas")
    '                drDetalleFinal("ImporteKardexMO") = dtTotalVentas.Rows(0)("TotalVentasMO") 'HDG
    '            Else
    '                drDetalleFinal("ImporteKardex") = 0
    '                drDetalleFinal("ImporteKardexMO") = 0 'HDG
    '            End If
    '            dtDetalleFinal.Rows.Add(drDetalleFinal)
    '            Return dtDetalleFinal
    '        Catch ex As Exception
    '            Throw ex
    '        End Try
    '    End Function
    '#Region "SELECT DISTINCT"
    '    Public Shared Function SelectDistinct(ByVal SourceTable As DataTable, ByVal ParamArray FieldNames() As String) As DataTable
    '        Dim lastValues() As Object
    '        Dim newTable As DataTable
    '        If FieldNames Is Nothing OrElse FieldNames.Length = 0 Then
    '            Throw New ArgumentNullException("FieldNames")
    '        End If
    '        lastValues = New Object(FieldNames.Length - 1) {}
    '        newTable = New DataTable
    '        For Each field As String In FieldNames
    '            newTable.Columns.Add(field, SourceTable.Columns(field).DataType)
    '        Next
    '        For Each Row As DataRow In SourceTable.Select("", String.Join(", ", FieldNames))
    '            If Not fieldValuesAreEqual(lastValues, Row, FieldNames) Then
    '                newTable.Rows.Add(createRowClone(Row, newTable.NewRow(), FieldNames))
    '                setLastValues(lastValues, Row, FieldNames)
    '            End If
    '        Next
    '        Return newTable
    '    End Function
    '    Private Shared Function fieldValuesAreEqual(ByVal lastValues() As Object, ByVal currentRow As DataRow, ByVal fieldNames() As String) As Boolean
    '        Dim areEqual As Boolean = True
    '        For i As Integer = 0 To fieldNames.Length - 1
    '            If lastValues(i) Is Nothing OrElse Not lastValues(i).Equals(currentRow(fieldNames(i))) Then
    '                areEqual = False
    '                Exit For
    '            End If
    '        Next
    '        Return areEqual
    '    End Function
    '    Private Shared Function createRowClone(ByVal sourceRow As DataRow, ByVal newRow As DataRow, ByVal fieldNames() As String) As DataRow
    '        For Each field As String In fieldNames
    '            newRow(field) = sourceRow(field)
    '        Next
    '        Return newRow
    '    End Function
    '    Private Shared Sub setLastValues(ByVal lastValues() As Object, ByVal sourceRow As DataRow, ByVal fieldNames() As String)
    '        For i As Integer = 0 To fieldNames.Length - 1
    '            lastValues(i) = sourceRow(fieldNames(i))
    '        Next
    '    End Sub
    '#End Region
    
    '#Region "Metodos Personalizados"
    '    Public Shared Function MostrarPopUp(ByVal strDireccion As String, ByVal strAleatorio As String, ByVal intAncho As Integer, ByVal intAlto As Integer, _
    '        ByVal intPosicionX As Integer, ByVal intPosicionY As Integer, ByVal strMenuBar As String, ByVal strResizable As String, _
    '        ByVal strStatus As String, ByVal strScrollbar As String) As String
    '        Return "<script language='javascript'>" & _
    '                   "window.open('" + strDireccion + "', " & _
    '              "'" + strAleatorio + "', " & _
    '              "'width=" + intAncho.ToString + ", " & _
    '              " height=" + intAlto.ToString + ", " & _
    '              " top=" + intPosicionX.ToString + ", " & _
    '              " left=" + intPosicionY.ToString + ", " & _
    '              " menubar=" + strMenuBar + ", " & _
    '              " resizable=" + strResizable + ", " & _
    '              " status=" + strStatus + ", " & _
    '              " scrollbars=" + strScrollbar + "' )" & _
    '             "</script>"
    '    End Function
    '#End Region

    '    Public Shared Function ObtenerLetra(ByVal numero As Integer) As String
    '        Dim abecedario As String = " ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    '        Dim resultado As String = ""
    '        Dim valor As Integer = numero \ 26
    '        If valor <> 0 Then
    '            resultado = abecedario.Substring(numero, 1)
    '            numero = numero - (26 * valor)
    '            resultado += abecedario.Substring(numero, 1)
    '        Else
    '            resultado = abecedario.Substring(numero, 1)
    '        End If
    '        Return resultado
    '    End Function
    '    Public Shared Function RemoveDiacritics(ByVal Cadena As String) As String
    '        Dim stFormD As String = Cadena.Normalize(NormalizationForm.FormD)
    '        Dim sb As New StringBuilder()
    '        For ich As Integer = 0 To stFormD.Length - 1
    '            Dim uc As UnicodeCategory = CharUnicodeInfo.GetUnicodeCategory(stFormD(ich))
    '            If uc <> UnicodeCategory.NonSpacingMark Then
    '                sb.Append(stFormD(ich))
    '            End If
    '        Next
    '        Return (sb.ToString().Normalize(NormalizationForm.FormC))
    '    End Function
    '    Public Class COMObjectAplication
    '        Public ProgramID As String
    '        Public ProcessName As String
    '        Public ObjetoAplication As Object
    '        Public ProcessId As Integer
    '        Public Sub New()
    '            Me.ProgramID = "Word.Application"
    '            Me.ProcessName = "WINWORD"
    '            Me.CrearAplicacion()
    '        End Sub
    '        Public Sub New(ByVal programID As String, ByVal processName As String)
    '            Me.ProgramID = programID
    '            Me.ProcessName = processName
    '            Me.CrearAplicacion()
    '        End Sub
    '        Private Sub CrearAplicacion()
    '            Try
    '                Me.ObjetoAplication = Nothing
    '                Me.ProcessId = 0
    '                Dim procesosAntiguos As List(Of Integer) = Me.buscarProcesos(System.Diagnostics.Process.GetProcesses())
    '                Me.ObjetoAplication = CreateObject(Me.ProgramID)
    '                Dim procesosAhora() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcesses()

    '                For Each proceso As System.Diagnostics.Process In procesosAhora
    '                    If proceso.ProcessName.ToUpper = Me.ProcessName.ToUpper Then
    '                        If Not procesosAntiguos.Contains(proceso.Id) Then
    '                            Me.ProcessId = proceso.Id 'Se encontro el nuevo proceso creado
    '                            Exit For
    '                        End If
    '                    End If
    '                Next
    '            Catch ex As Exception
    '                Throw New Exception("No se pudo crear el componente [ " & Me.ProcessName & " ] en su servidor. Por favor comunicar a Gestión de la Demanda.")
    '            End Try
    '        End Sub
    '        Private Function buscarProcesos(ByVal listaProcesos() As System.Diagnostics.Process) As List(Of Integer)
    '            Return (From proceso In listaProcesos Where proceso.ProcessName.ToUpper = ProcessName.ToUpper Select proceso.Id).ToList()
    '        End Function
    '        Public Sub terminarProceso()
    '            Try
    '                If Me.ProcessId <> 0 Then Diagnostics.Process.GetProcessById(Me.ProcessId).Kill()
    '            Catch ex As Exception
    '            End Try
    '        End Sub
    '    End Class
End Class