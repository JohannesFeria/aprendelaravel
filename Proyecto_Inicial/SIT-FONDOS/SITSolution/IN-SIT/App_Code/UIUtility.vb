Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html
Imports Sit.BusinessLayer
Imports Sit.BusinessEntities
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
Imports Sit.BusinessLayer.MotorInversiones

Public Class UIUtility
#Region " /* Métodos Personalizados (Vol. 1) */ "
    'OT11004 - 20/12/2017 - Ian Pastor M.
    'Descripción: Obtiene el valor de una columna contenida en un dataset
    Public Shared Function ObtenerValorRequest(ByVal dataRequest As DataSet, ByVal nombre As String) As String
        Dim columnName As String = dataRequest.Tables(0).Columns(0).ColumnName
        Return CType(dataRequest.Tables(0).Select(columnName & "='" & nombre & "'")(0)(1), String)
    End Function
    'OT11004 - Fin
    'OT 9856 - 24/01/2017 - Carlos Espejo
    'Descripcion: Resalta botones
    Public Shared Sub ResaltaBotones(Boton As Button, Resalta As Boolean)
        Boton.Enabled = True
        If Resalta Then
            Boton.BorderColor = Drawing.Color.Red
        Else
            Boton.BorderColor = Drawing.Color.White
        End If
    End Sub
    'OT 9856 - 24/01/2017 - Carlos Espejo
    'Descripcion: Resalta Cajas de texto
    Public Shared Sub ResaltaCajaTexto(Caja As TextBox, Resalta As Boolean)
        Caja.Enabled = True
        If Resalta Then
            Caja.BorderColor = Drawing.Color.Red
        Else
            Caja.BorderColor = Drawing.Color.Silver
        End If
    End Sub
    'OT 9856 - 24/01/2017 - Carlos Espejo
    'Descripcion: Resalta Combos
    Public Shared Sub ResaltaCombo(Combo As DropDownList, Resalta As Boolean)
        Combo.Enabled = True
        If Resalta Then
            Combo.BorderColor = Drawing.Color.Red
        Else
            Combo.BorderColor = Drawing.Color.Silver
        End If
    End Sub
    Public Shared Sub AgregarElementoFinal(ByVal sel As DropDownList, Optional ByVal valor As String = "", Optional ByVal texto As String = "")
        Dim iElemento As Integer = sel.Items.Count
        sel.Items.Insert(iElemento, New System.Web.UI.WebControls.ListItem(texto, valor))
    End Sub
    Public Shared Sub InsertarElementoSeleccion(ByVal sel As DropDownList, Optional ByVal valor As String = "", Optional ByVal texto As String = "--SELECCIONE--")
        sel.Items.Insert(0, New System.Web.UI.WebControls.ListItem(texto, valor))
    End Sub

    Public Shared Sub InsertarOtroElementoSeleccion(ByVal sel As DropDownList, Optional ByVal valor As String = "")
        If sel.Items(0).Text.ToUpper <> "--SELECCIONE--" Then
            sel.Items.Insert(0, New System.Web.UI.WebControls.ListItem("--SELECCIONE--", valor))
        End If
    End Sub
    Public Shared Sub SeleccionarDefaultValue(ByRef sel As DropDownList, ByVal index As Integer)
        If sel.Items.Count > index Then sel.SelectedIndex = index
    End Sub
    Public Shared Sub ExcluirOtroElementoSeleccion(ByVal sel As DropDownList)
        If sel.Items(0).Text = "--SELECCIONE--" Then sel.Items.RemoveAt(0)
    End Sub
#End Region
#Region " /* Métodos Personalizados (Vol. 2) */ "
    Public Shared Function ValidarHora(ByVal hora As String) As Boolean
        Dim strAux As String = hora
        strAux = strAux.Substring(0, 2) + strAux.Substring(3, 2)
        If Convert.ToDecimal(strAux) >= 2400 Or Convert.ToDecimal(strAux.Substring(2, 2)) >= 60 Then
            Return False
        End If
        Return True
    End Function
    Public Shared Function ConvertirFechaaDecimal(ByVal fecha As String) As Decimal

        If fecha.Length = 0 Then
            Return 0
        ElseIf fecha.Length = 8 Then
            Return fecha
        End If
        Return Convert.ToDecimal(fecha.Substring(6, 4) + fecha.Substring(3, 2) + fecha.Substring(0, 2))
    End Function
    Public Shared Function ConvertirStringaFecha(ByVal fecha As String) As System.DateTime
        Dim strfecha As System.DateTime
        If fecha.Length > 0 Then strfecha = New Date(fecha.Split("/")(2), fecha.Split("/")(1), fecha.Split("/")(0))
        Return strfecha
    End Function
    Public Shared Function fnFechaNueva(ByVal fechaAnt As String) As Date
        Dim Fecha As Decimal
        Dim fechaAnterior As Date
        Dim fechaNueva As Date
        Dim oFeriadoBM As New FeriadoBM
        Dim EsFeriado As Boolean
        fechaAnterior = Convert.ToDateTime(fechaAnt)
        FechaNueva = fechaAnterior.AddDays(1)
        Fecha = CType(FechaNueva.ToString("yyyyMMdd"), Decimal)
        If FechaNueva.DayOfWeek = DayOfWeek.Saturday Then
            While FechaNueva.DayOfWeek = DayOfWeek.Saturday
                FechaNueva = FechaNueva.AddDays(2)
                Fecha = CType(FechaNueva.ToString("yyyyMMdd"), Decimal)
                While oFeriadoBM.BuscarPorFecha(Fecha, "A")
                    FechaNueva = FechaNueva.AddDays(1)
                    Fecha = CType(FechaNueva.ToString("yyyyMMdd"), Decimal)
                End While
            End While
        Else
            EsFeriado = oFeriadoBM.BuscarPorFecha(Fecha, "A")
            If EsFeriado = True Then
                While oFeriadoBM.BuscarPorFecha(Fecha, "A")
                    FechaNueva = FechaNueva.AddDays(1)
                    Fecha = CType(FechaNueva.ToString("yyyyMMdd"), Decimal)
                    While FechaNueva.DayOfWeek = DayOfWeek.Saturday
                        FechaNueva = FechaNueva.AddDays(2)
                        Fecha = CType(FechaNueva.ToString("yyyyMMdd"), Decimal)
                    End While
                End While
            End If
        End If
        Return fechaNueva
    End Function
    Public Shared Function fnFechaAnterior(ByVal FechaActual As String) As Date
        Dim Fecha As Decimal
        Dim fechaAnterior As Date
        Dim fechaNueva As Date
        Dim oFeriadoBM As New FeriadoBM
        fechaAnterior = Convert.ToDateTime(FechaActual)
        fechaNueva = fechaAnterior.AddDays(-1)
        If fechaNueva.DayOfWeek = DayOfWeek.Sunday Then
            fechaNueva = fechaNueva.AddDays(-2)
            While oFeriadoBM.BuscarPorFecha(CType(fechaNueva.ToString("yyyyMMdd"), Decimal), "A")
                fechaNueva = fechaNueva.AddDays(-1)
            End While
        ElseIf fechaNueva.DayOfWeek = DayOfWeek.Saturday Then
            fechaNueva = fechaNueva.AddDays(-1)
            While oFeriadoBM.BuscarPorFecha(CType(fechaNueva.ToString("yyyyMMdd"), Decimal), "A")
                fechaNueva = fechaNueva.AddDays(-1)
            End While
        Else
            While oFeriadoBM.BuscarPorFecha(CType(fechaNueva.ToString("yyyyMMdd"), Decimal), "A")
                fechaNueva = fechaNueva.AddDays(-1)
            End While
        End If
        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
        Return fechaNueva
    End Function
    Public Shared Function fnFechaAnteriorSinFeriado(ByVal p_FechaActual As String) As Date
        Dim fechaAnterior As Date
        Dim fechaNueva As Date
        fechaAnterior = Convert.ToDateTime(p_FechaActual)
        fechaNueva = fechaAnterior.AddDays(-1)
        Return fechaNueva
    End Function

    ''' <summary>66056 - JZAVALA.
    ''' METODO NUEVO QUE CONVIERTE EL DECIMAL A STRING CON EL FORMATO DE FECHA.
    ''' </summary>
    ''' <param name="fecha"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
    Public Shared Function ObtenerDiferenciaDias(ByVal fechaInicio As String, ByVal fechaFin As String) As Integer
        Dim strFechaResultado As String = ""
        Dim diaResultado As Integer = 0
        Dim fechaInicioVariable As Date = Convert.ToDateTime(fechaInicio)
        Dim fechaFinVariable As Date = Convert.ToDateTime(fechaFin)
        diaResultado = DateDiff(DateInterval.Day, fechaInicioVariable, fechaFinVariable)
        Return diaResultado
    End Function

    Public Shared Function ObtenerNombreDia(ByVal fecha As String) As String
        Dim strNombreDia As String = ""
        Dim strfecha As System.DateTime
        strfecha = ConvertirStringaFecha(fecha)
        Select Case strfecha.DayOfWeek
            Case DayOfWeek.Monday
                strNombreDia = "Lunes"
            Case DayOfWeek.Tuesday
                strNombreDia = "Martes"
            Case DayOfWeek.Wednesday
                strNombreDia = "Miércoles"
            Case DayOfWeek.Thursday
                strNombreDia = "Jueves"
            Case DayOfWeek.Friday
                strNombreDia = "Viernes"
            Case DayOfWeek.Saturday
                strNombreDia = "Sábado"
            Case DayOfWeek.Sunday
                strNombreDia = "Domingo"
        End Select
        Return strNombreDia
    End Function
    Public Shared Function ObtenerCantidadMes(ByVal multiploMes As String) As Integer
        Dim strCantidaMes As Integer = 0
        Select Case multiploMes.ToUpper.Trim
            Case "MENSUAL"
                strCantidaMes = 1
            Case "BIMENSUAL", "BIMESTRUAL", "BIMESTRAL"
                strCantidaMes = 2
            Case "TRIMENSUAL", "TRIMESTRUAL", "TRIMESTRAL"
                strCantidaMes = 3
            Case "CUATRIMESTRAL", "CUATRIMENSUAL", "CUATRIMESTRAL"
                strCantidaMes = 4
            Case "SEMESTRAL", "SEMESTRUAL"
                strCantidaMes = 6
            Case "ANUAL"
                strCantidaMes = 12
            Case "APERIODICO"
                strCantidaMes = 999
            Case Else
                strCantidaMes = 24
        End Select
        Return strCantidaMes
    End Function
    Public Shared Function ConvertirFechaaString(ByVal fecha As Decimal) As String
        Dim strfecha As String = Convert.ToString(fecha)
        If strfecha.Length = 8 Then Return strfecha.Substring(6, 2) + "/" + strfecha.Substring(4, 2) + "/" + strfecha.Substring(0, 4)
        Return ""
    End Function
    '***********************************************************
    ' Autor: Julio A. Rodríguez Grados
    ' Función de Validación de Fechas
    ' Recibe una fecha con formato dd/mm/yyyy o dd-mm-yyyy
    ' Devuelve True/False Si es o NO Correcta la Fecha recibida
    '***********************************************************
    Public Shared Function ValidaFormatoFecha(ByVal cFecha As String) As Boolean
        Dim cDay, cMonth, cYear, cFechaReal As String
        Dim MyDate As Date
        Dim sDate1 As String
        ValidaFormatoFecha = False
        If cFecha.Length < 10 Then
            Return False
            Exit Function
        End If
        If cFecha <> "          " Then
            Return False
            Exit Function
        End If
        cDay = cFecha.Substring(0, 2)
        cMonth = cFecha.Substring(3, 2)
        cYear = cFecha.Substring(6, 4)
        If Val(cYear) < 1900 Then
            Return False
            Exit Function
        End If
        If Val(cMonth) > 12 Then
            Return False
            Exit Function
        End If
        'Si la fecha es ingresada en blanco es correcta
        If cDay <> Space(2) And cMonth <> Space(2) And cYear <> Space(4) Then
            'Windows esta en formato español
            If IsDate("13/05/72") Then cFechaReal = cDay + "/" + cMonth + "/" + cYear
            If IsDate(cFechaReal) Then
                MyDate = DateValue(cFechaReal)
                sDate1 = Format$(MyDate, "dd/MM/yyyy")
                If cFecha = sDate1 Then ValidaFormatoFecha = True Else ValidaFormatoFecha = False
            End If
        End If
    End Function
#End Region
#Region " /* Métodos Personalizados (Vol. 3) */ "
    Public Shared Sub ValidarLista(ByVal ddl As DropDownList, ByVal lista As String)
        Dim i As Integer = ddl.Items.Count - 1
        While (i >= 0)
            If lista.IndexOf(ddl.Items(i).Value) = -1 Then ddl.Items.RemoveAt(i)
            i = i - 1
        End While
    End Sub
    Public Shared Sub ValidarLista(ByVal ddl As DropDownList, ByVal lista As String())
        Dim i As Integer = ddl.Items.Count - 1
        Dim list As String = ListaCadena(lista)
        While (i >= 0)
            If list.IndexOf(ddl.Items(i).Value) = -1 Then ddl.Items.RemoveAt(i)
            i = i - 1
        End While
    End Sub
#End Region
#Region " /* Métodos Personalizados (Vol. 4) */ "
    Public Shared Function ListaCadena(ByVal lista As String()) As String
        Dim cadena As String = ""
        For Each cad As String In lista
            cadena = cadena & cad & ","
        Next
        Return IIf(cadena.Length > 0, cadena.Substring(0, cadena.Length - 1), cadena)
    End Function
#End Region
#Region " /* Métodos Personalizados (Vol. 5) */ "
    Public Shared Sub CargarPortafoliosOI(ByVal drlista As DropDownList, Optional ByVal s_Parametro As String = "S")
        Dim oPortafolio As New PortafolioBM
        Dim dt As DataTable = oPortafolio.PortafolioCodigoListar(PORTAFOLIO_MULTIFONDOS, s_Parametro)
        HelpCombo.LlenarComboBox(drlista, dt, "CodigoPortafolio", "Descripcion", True)
    End Sub
    Public Shared Sub CargarOperacionOI(ByVal drlista As DropDownList, ByVal claseinstrumento As String)
        Dim _OperacionBM As New OperacionBM
        Dim dt As DataTable = _OperacionBM.SeleccionarporClaseinstrumento(claseinstrumento, ESTADO_ACTIVO).Tables(0)
        HelpCombo.LlenarComboBox(drlista, dt, "CodigoOperacion", "Descripcion", True, "SELECCIONE")
    End Sub
    Public Shared Sub CargarOperacionOIParaTraspaso(ByVal drlista As DropDownList)
        Dim _OperacionBM As New OperacionBM
        Dim dt As DataTable = _OperacionBM.SeleccionarPorTrasladoOI().Tables(0)
        HelpCombo.LlenarComboBox(drlista, dt, "CodigoOperacion", "Descripcion", False)
    End Sub
    Public Shared Function ObtenerOperacionOpuesta(ByVal strOperacion As String, ByVal claseinstrumento As String) As String
        Dim strOperacionOpuesta As String
        strOperacionOpuesta = New OperacionBM().SeleccionarOperacionOpuesta(strOperacion, claseinstrumento, ESTADO_ACTIVO)
        Return strOperacionOpuesta
    End Function
    Public Shared Sub CargarTipoCuponOI(ByVal drlista As DropDownList)
        Dim _ParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dt As DataTable = _ParametrosGeneralesBM.Listar("TipoTasaI", Nothing)
        HelpCombo.LlenarComboBox(drlista, dt, "Valor", "Nombre", True, "SELECCIONE")
    End Sub
    Public Shared Sub CargarIntermediariosOI(ByVal drlista As DropDownList)
        drlista.Items.Clear()
        drlista.DataSource = New TercerosBM().ListarTerceroPorClasificacion(CLASIFICACIONTERCERO_INTERMEDIARIO)
        drlista.DataTextField = "Descripcion" : drlista.DataValueField = "CodigoTercero" : drlista.DataBind() : InsertarElementoSeleccion(drlista)
    End Sub
    Public Shared Sub CargarIntermediariosOISoloBancos(ByVal drlista As DropDownList)
        drlista.Items.Clear()
        drlista.DataSource = New TercerosBM().ListarTerceroPorClasificacionSoloBancos(CLASIFICACIONTERCERO_INTERMEDIARIO)
        drlista.DataTextField = "Descripcion" : drlista.DataValueField = "CodigoTercero" : drlista.DataBind() : InsertarElementoSeleccion(drlista)
    End Sub
    Public Shared Sub CargarIntermediariosValorOI(ByVal drlista As DropDownList, ByVal codigoNemonico As String, ByVal codigoPortafolioSBS As String)
        drlista.Items.Clear()
        drlista.DataSource = New TercerosBM().ListarTerceroPorClasificacionValor(CLASIFICACIONTERCERO_TERCERO, codigoNemonico, codigoPortafolioSBS)
        drlista.DataTextField = "Descripcion" : drlista.DataValueField = "CodigoTercero" : drlista.DataBind() : InsertarElementoSeleccion(drlista)
    End Sub
    Public Shared Sub CargarIntermediariosCustodioOI(ByVal drlista As DropDownList, ByVal custodio As String)
        drlista.Items.Clear()
        drlista.DataSource = New TercerosBM().ListarTerceroPorClasificacionCustodio(CLASIFICACIONTERCERO_INTERMEDIARIO, custodio)
        drlista.DataTextField = "Descripcion" : drlista.DataValueField = "CodigoTercero" : drlista.DataBind() : InsertarElementoSeleccion(drlista)
    End Sub
    Public Shared Sub CargarIntermediariosCustodioXGrupoInterOI(ByVal drlista As DropDownList, ByVal custodio As String, ByVal strFiltro As String)
        drlista.Items.Clear()
        drlista.DataSource = New TercerosBM().ListarTerceroPorClasificacionCustodioXGrupoInter(CLASIFICACIONTERCERO_INTERMEDIARIO, custodio, strFiltro)
        drlista.DataTextField = "Descripcion" : drlista.DataValueField = "CodigoTercero" : drlista.DataBind() : InsertarElementoSeleccion(drlista)
    End Sub
    Public Shared Sub CargarMonedaOI(ByVal drlista As DropDownList)
        drlista.DataSource = New MonedaBM().Listar(ESTADO_ACTIVO)
        drlista.DataTextField = "Descripcion" : drlista.DataValueField = "CodigoMoneda" : drlista.DataBind() : InsertarElementoSeleccion(drlista)
    End Sub
    Public Shared Sub CargarMonedaSituacionOI(ByVal drlista As DropDownList, ByVal situacion As String)
        drlista.DataSource = New MonedaBM().Listar(situacion)
        drlista.DataTextField = "Descripcion" : drlista.DataValueField = "CodigoMoneda" : drlista.DataBind() : InsertarElementoSeleccion(drlista)
    End Sub
    Public Shared Sub CargarGrupoIntermediarioOI(ByVal drlista As DropDownList)
        drlista.DataSource = New ParametrosGeneralesBM().ListarGrupoIntermediario()
        drlista.DataTextField = "Nombre" : drlista.DataValueField = "Valor" : drlista.DataBind() : InsertarElementoSeleccion(drlista)
    End Sub
    Public Shared Sub CargarIntermediariosXGrupoOI(ByVal drlista As DropDownList, ByVal strFiltro As String)
        drlista.Items.Clear()
        drlista.DataSource = New TercerosBM().ListarTerceroPorGrupoIntermediario(CLASIFICACIONTERCERO_INTERMEDIARIO, strFiltro)
        drlista.DataTextField = "Descripcion" : drlista.DataValueField = "CodigoTercero" : drlista.DataBind() : InsertarElementoSeleccion(drlista)
    End Sub

    Public Shared Sub CargarPortafoliosbyAumentoCapital(ByVal drlista As DropDownList)
        Dim oPortafolio As New PortafolioBM
        Dim dt As DataTable = oPortafolio.PortafolioListarbyAumentoCapital()
        HelpCombo.LlenarComboBox(drlista, dt, "CodigoPortafolioSBS", "Descripcion", True)
    End Sub

#End Region
#Region " /* Métodos Personalizados (Vol. 6) */ "
    Public Shared Sub GeneraComisionesGrilla(ByVal dgLista As System.Web.UI.WebControls.DataGrid, ByVal Mercado As String, ByVal TipoRenta As String)
        Dim objcomisiones As New ImpuestosComisionesBM
        Dim i As Integer
        dgLista.DataSource = objcomisiones.SeleccionarFiltroDinamico(TipoRenta, Mercado).Tables(0)
        dgLista.DataBind()
        For i = 0 To dgLista.Items.Count - 1
            dgLista.Items(i).Cells(2).Text = dgLista.Items(i).Cells(2).Text.Replace(UIUtility.DecimalSeparator, ".")
        Next
    End Sub
    Public Shared Function ActualizaMontosFinales(ByVal dgLista As System.Web.UI.WebControls.GridView) As Decimal
        Dim i As Integer
        Dim dblComisiones As Decimal = 0
        For i = 0 To dgLista.Rows.Count - 1
            Dim valorComision As Decimal = 0
            If CType(dgLista.Rows(i).FindControl("txtValorComision1"), TextBox).Text = "" Or _
                CType(dgLista.Rows(i).FindControl("txtValorComision1"), TextBox).Text = String.Empty Then
                valorComision = 0
            Else
                valorComision = Convert.ToDecimal(CType(dgLista.Rows(i).FindControl("txtValorComision1"), TextBox).Text.Replace(".", UIUtility.DecimalSeparator))
            End If
            dblComisiones = dblComisiones + valorComision
            If i = dgLista.Rows.Count - 1 Then
                If CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox).Text = "" Or _
                    CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox).Text = String.Empty Then
                    Exit For
                End If
            End If
            Dim valorComision2 As Decimal = 0
            If CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox).Text = "" Or _
                CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox).Text = String.Empty Then
                valorComision2 = 0
            Else
                valorComision2 = Convert.ToDecimal(CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox).Text.Replace(".", UIUtility.DecimalSeparator))
            End If
            dblComisiones = dblComisiones + valorComision2
        Next
        FormatoImpuestosComisiones(dgLista)
        Return dblComisiones
    End Function
    Public Shared Function ObtieneValorComision(ByVal strValorDuro As String) As Decimal
        Dim strComision As String = strValorDuro
        Dim decComision As Decimal = 0.0
        If strComision.Length > 2 Then
            If strComision.Substring(strComision.Length - 2, 2) = "%)" Then 'es Porcentaje
                strComision = strComision.Substring(1)
                strComision = strComision.Substring(0, strComision.Length - 2)
                decComision = Convert.ToDecimal(strComision.Replace(".", UIUtility.DecimalSeparator)) / 100
            Else 'es Factor
                strComision = strComision.Substring(1)
                strComision = strComision.Substring(0, strComision.Length - 1)  'HDG 20120404
                decComision = Convert.ToDecimal(strComision.Replace(".", UIUtility.DecimalSeparator))
            End If
        End If
        Return decComision
    End Function
    Public Shared Function CalculaImpuestosComisiones(ByVal codigoPortafolio As String, ByVal codigoOrden As String, ByVal codigoPreOrden As String, _
           ByVal montoOperacion As Decimal, ByVal datosRequest As DataSet) As DataTable
        Dim impuestos As New ImpuestosComisionesBM
        Dim valor As Decimal
        Dim indicadorCalculo As String
        Dim baseCalculo As String
        Dim impuestoCalculado As Decimal
        Dim dtResult As DataTable
        Dim indexIGV As Integer
        Dim i As Integer
        Dim totalComisiones As Decimal
        Try
            Dim dtImpuestos As DataTable = impuestos.GetImpuestosComisiones(codigoPreOrden)
            If Not dtImpuestos.Rows.Count > 0 Then
                Return dtImpuestos
                Exit Function
            End If
            dtResult = dtImpuestos.Clone()
            dtResult.Columns.Add("Impuesto", GetType(System.Decimal))
            i = 0
            For Each dr As DataRow In dtImpuestos.Rows
                valor = Convert.ToDecimal(dr("ValorComision"))
                indicadorCalculo = Convert.ToString(dr("IndicadorCalculo"))
                baseCalculo = Convert.ToString(dr("BaseCalculo"))
                If (indicadorCalculo = "P") And baseCalculo = "M" Then
                    impuestoCalculado = (valor / 100) * montoOperacion
                ElseIf (indicadorCalculo = "V") And baseCalculo = "M" Then
                    impuestoCalculado = (valor) * montoOperacion
                End If
                totalComisiones = totalComisiones + impuestoCalculado
                If (baseCalculo = "C") Then
                    indexIGV = i
                End If
                Dim drResult As DataRow = dtResult.NewRow()
                drResult("CodigoTipoRenta") = dr("CodigoTipoRenta")
                drResult("CodigoMercado") = dr("CodigoMercado")
                drResult("CodigoComision") = dr("CodigoComision")
                drResult("Descripcion") = dr("Descripcion")
                drResult("IndicadorCalculo") = dr("IndicadorCalculo")
                drResult("BaseCalculo") = dr("BaseCalculo")
                drResult("ValorComision") = dr("ValorComision")
                drResult("CodigoPlaza") = dr("CodigoPlaza")
                If (baseCalculo = "C") Then
                    If (drResult("Impuesto") Is DBNull.Value) Then
                        drResult("Impuesto") = impuestoCalculado
                    Else
                        drResult("Impuesto") = Convert.ToDecimal(drResult("Impuesto")) + impuestoCalculado
                    End If
                Else
                    drResult("Impuesto") = impuestoCalculado
                End If
                i = i + 1
                dtResult.Rows.Add(drResult)
            Next
            dtResult.Rows(indexIGV)("Impuesto") = (Convert.ToDecimal(dtResult.Rows(indexIGV)("ValorComision")) / 100) * totalComisiones
            For Each dr As DataRow In dtResult.Rows
                InsertarModificarImpuestosComisionesLinea(String.Empty, codigoOrden, codigoPortafolio, _
                Convert.ToString(dr("CodigoComision")), Convert.ToString(dr("CodigoTipoRenta")), _
                Convert.ToString(dr("CodigoMercado")), Convert.ToDecimal(dr("Impuesto")), _
                Convert.ToDecimal(dr("Impuesto")), datosRequest, Convert.ToString(dr("CodigoPlaza")))
            Next
        Catch ex As Exception
            Throw ex
        End Try
        Return dtResult
    End Function
    'OT11008 - 22/01/2018 - Carlos Rumiche.
    'Descripción: Cálculo de comisiones
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se añade tres Input: fechaOperacion, codigoMnemonico y codigoTercero opcionales a la función para calcular Comisiones en una Nueva OI| 13/06/18 
    Public Shared Function CalculoComisionesSimplificado(ByVal codigoPreOrden As String, ByVal montoOperacion As Decimal, Optional ByVal fechaOperacion As Decimal = 0, Optional ByVal codigoMnemonico As String = "", Optional ByVal codigoTercero As String = "") As DataTable
        '/*CRumiche 2018-01-18: Forma simpleificada del Cálculo de Impuestos para la negociación y consideración de VALOR FIJO con TipoCambio y Comisiones que generan impuestos*/
        Dim dtResul As DataTable
        Try
            Dim impuestos As New ImpuestosComisionesBM
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se incluye los tres nuevos Input - Bonos| 13/06/18 
            dtResul = impuestos.GetImpuestosComisiones(codigoPreOrden, fechaOperacion, codigoMnemonico, codigoTercero)
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se incluye los tres nuevos Input - Bonos| 13/06/18 
            dtResul.Columns.Add("MontoComision", GetType(System.Decimal))
            Dim factorComision As Decimal
            Dim factorMontoBase As Decimal
            Dim totalComisionesQueGeneranImpuestos As Decimal = 0
            For Each dr As DataRow In dtResul.Rows
                factorComision = 0
                factorMontoBase = 0

                Select Case dr("IndicadorCalculo").ToString()
                    Case "V" '/*Es un VALOR*/
                        factorComision = Convert.ToDecimal(dr("ValorComision"))
                    Case "P" '/*Es un PORCENTAJE*/
                        factorComision = Convert.ToDecimal(dr("ValorComision")) / 100
                End Select

                Select Case dr("BaseCalculo").ToString()
                    Case "M" '/*en base a MONTO DE LA OPERACION*/
                        factorMontoBase = montoOperacion
                    Case "F" '/*en base a VALOR FIJO*/
                        '/*Cuando se trata de un VALOR FIJO, la Comisión tendrá el mismo valor del factorComisión*/
                        '/*Adicionalmente debe considerarse tambien el tipo de cambio pues el VALOR FIJO puede estar en una moneda específica*/
                        If dr("CodigoMoneda").ToString.ToUpper.Contains("SOL") Then
                            factorMontoBase = 1 / Convert.ToDecimal(dr("TipoCambioOrden"))
                        Else
                            factorMontoBase = 1 * Convert.ToDecimal(dr("TipoCambioOrden"))
                        End If
                    Case "C" '/*en base a COMISIONES*/
                        factorMontoBase = 1 '/*El Importe se obtendrá al terminar de calcularse todas las comisiones*/
                End Select

                dr("MontoComision") = factorComision * factorMontoBase

                ' /*Solo si la base es diferente a COMISIONES y la comisión GENERA IMPUESTOS*/
                If Not dr("BaseCalculo").ToString().Equals("C") And dr("GeneraImpuestos") = True Then
                    totalComisionesQueGeneranImpuestos += dr("MontoComision")
                End If
            Next

            For Each dr As DataRow In dtResul.Rows
                If dr("BaseCalculo").ToString().Equals("C") Then
                    '/*Calculamos la Comisión que depende de las demás Comisiones, generalmente es el IGV*/
                    dr("MontoComision") = dr("MontoComision") * totalComisionesQueGeneranImpuestos
                End If
            Next

        Catch ex As Exception
            Throw ex
        End Try

        Return dtResul
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se añade tres Input: fechaOperacion, codigoMnemonico y codigoTercero opcionales a la función para calcular Comisiones en una Nueva OI| 13/06/18 

    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se añade tres Input: fechaOperacion, codigoMnemonico y codigoTercero opcionales a la función para calcular Comisiones en una Nueva OI| 13/06/18 
    Public Shared Function CalcularComisionesYLlenarGridView(ByVal gridView As System.Web.UI.WebControls.GridView, ByVal codigoPreOrden As String, ByVal montoOperacion As Decimal, Optional ByVal fechaOperacion As Decimal = 0, Optional ByVal codigoMnemonico As String = "", Optional ByVal codigoTercero As String = "") As Decimal
        Dim codigoComision As String
        Dim totalComision As Decimal = 0
        Dim dtResul As DataTable
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se incluye los tres nuevos Input - Bonos| 13/06/18 
        dtResul = CalculoComisionesSimplificado(codigoPreOrden, montoOperacion, fechaOperacion, codigoMnemonico, codigoTercero)
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se incluye los tres nuevos Input - Bonos| 13/06/18 
        Dim rows As DataRow()
        Dim txtValorComision As TextBox

        For i = 0 To gridView.Rows.Count - 1
            codigoComision = gridView.Rows(i).Cells(0).Text
            rows = dtResul.Select("CodigoComision = '" & codigoComision & "'")
            If rows.Count > 0 Then
                txtValorComision = CType(gridView.Rows(i).FindControl("txtValorComision1"), TextBox)
                txtValorComision.Text = rows(0)("MontoComision")
                totalComision += rows(0)("MontoComision")
            End If

            codigoComision = gridView.Rows(i).Cells(6).Text
            rows = dtResul.Select("CodigoComision = '" & codigoComision & "'")
            If rows.Count > 0 Then
                txtValorComision = CType(gridView.Rows(i).FindControl("txtValorComision2"), TextBox)
                txtValorComision.Text = rows(0)("MontoComision")
                totalComision += rows(0)("MontoComision")
            End If
        Next

        FormatoImpuestosComisiones(gridView)
        Return totalComision
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se añade tres Input: fechaOperacion, codigoMnemonico y codigoTercero opcionales a la función para calcular Comisiones en una Nueva OI| 13/06/18 
    'OT11008 - Fin
    Public Shared Function CalculaImpuestosComisiones(ByVal dgLista As System.Web.UI.WebControls.GridView, ByVal Mercado As String, ByVal montoL As String, ByVal montoE As String, Optional ByVal grupoInt As String = "", Optional ByVal operacion As String = "", Optional ByVal claseInst As String = "", Optional ByVal tipoTramo As String = "", Optional ByVal tipoFondo As String = "") As Decimal 'HDG 20120224
        Dim dblsubtotal As Decimal = 0.0
        Dim dbltotalcomisiones As Decimal = 0.0
        Dim dblMontoL As Decimal = 0.0
        Dim dblMontoE As Decimal = 0.0
        Dim blnEsPar As Boolean = False
        Try
            'OT 9968 - 14/02/2017 - Carlos Espejo
            'Descripcion: Se agrega HttpUtility.HtmlDecode
            If ObtieneValorComision(HttpUtility.HtmlDecode(dgLista.Rows(dgLista.Rows.Count - 1).Cells(8).Text)) >= 0 Then
                'OT 9968 Fin
                blnEsPar = True
            End If
        Catch ex As Exception
            blnEsPar = False
        End Try

        If montoL <> "" Then
            dblMontoL = Convert.ToDecimal(montoL.Replace(".", UIUtility.DecimalSeparator))
        End If
        If montoE <> "" Then
            dblMontoE = Convert.ToDecimal(montoE.Replace(".", UIUtility.DecimalSeparator))
        End If
        Dim i As Integer
        Dim indiceAux As Integer
        Dim cellAux As Integer
        Dim cellIGV As Integer
        Dim comiAux As String
        Dim blnIGV As Boolean = False
        Dim dblComisionIGV As Decimal = 0.0
        'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea variable SMV para capturar el valor "CONTRIBUCIÓN CONASEV" de la grilla de Comisiones| 19/06/18 
        Dim SMV As Decimal = 0.0
        'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se crea variable SMV para capturar el valor "CONTRIBUCIÓN CONASEV" de la grilla de Comisiones| 19/06/18 
        For i = 0 To dgLista.Rows.Count - 1
            If dgLista.Rows(i).Cells(0).Text <> "P I.G.V. TOT" Then
                Dim txtValorComision As TextBox = CType(dgLista.Rows(i).FindControl("txtValorComision1"), TextBox)
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Cálculo de comisión cuando es mercado extranjero| 19/06/18 
                If Mercado <> "7" And dgLista.Rows(i).Cells(0).Text = "COM. SAB." Then
                    txtValorComision.Text = CType(dblMontoE * ObtieneValorComision(dgLista.Rows(i).Cells(2).Text), String).Replace(UIUtility.DecimalSeparator, ".")
                Else
                    txtValorComision.Text = CType(dblMontoL * ObtieneValorComision(dgLista.Rows(i).Cells(2).Text), String).Replace(UIUtility.DecimalSeparator, ".")
                End If
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Cálculo de comisión cuando es mercado extranjero| 19/06/18 
                dgLista.Rows(i).Cells(3).Text = txtValorComision.Text
                dblsubtotal = dblsubtotal + Convert.ToDecimal(txtValorComision.Text.Replace(".", UIUtility.DecimalSeparator))
                'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se implementa condicional para capturar el valor de CONTRIBUCIÓN CONASEV y luego utilizarlo en el cálculo de IGV|19/06/18 
                If dgLista.Rows(i).Cells(0).Text = "PCONT CONASE" Then SMV = Convert.ToDecimal(txtValorComision.Text.Replace(".", UIUtility.DecimalSeparator))
                'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se implementa condicional para capturar el valor de CONTRIBUCIÓN CONASEV y luego utilizarlo en el cálculo de IGV| 19/06/18 
            Else
                indiceAux = i
                cellAux = 2
                cellIGV = 3
                comiAux = "txtValorComision1"
                blnIGV = True
            End If
            If i = dgLista.Rows.Count - 1 Then
                If blnEsPar = False Then
                    Exit For
                End If
            End If
            If dgLista.Rows(i).Cells(6).Text <> "P I.G.V. TOT" Then 'ca
                'OT 9968 - 14/02/2017 - Carlos Espejo
                'Descripcion: Se agrega HttpUtility.HtmlDecode
                Dim txtValorComision As TextBox = CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox)
                Dim Valor As String = ObtieneValorComision(HttpUtility.HtmlDecode(dgLista.Rows(i).Cells(8).Text))
                'OT 9968 Fin
                Valor = Replace(IIf(Valor.Trim = "", "0", Valor.Trim), ",", "")
                txtValorComision.Text = CType(dblMontoL * CDec(Valor), String).Replace(UIUtility.DecimalSeparator, ".")
                dgLista.Rows(i).Cells(9).Text = txtValorComision.Text
                dblsubtotal = dblsubtotal + Convert.ToDecimal(txtValorComision.Text.Replace(".", UIUtility.DecimalSeparator))
            Else
                indiceAux = i
                cellAux = 8
                cellIGV = 9
                comiAux = "txtValorComision2"
                blnIGV = True
            End If
        Next
        If blnIGV = True Then
            Dim txtValorComisionIGV As TextBox = CType(dgLista.Rows(indiceAux).FindControl(comiAux), TextBox)
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Para el cálculo del IGV se descuenta el valor capturado de la variable SMV (CONTRIBUCIÓN CONASEV) | 19/06/18 
            dblComisionIGV = Math.Round(ObtieneValorComision(dgLista.Rows(indiceAux).Cells(cellAux).Text) * (dblsubtotal - SMV), 7)
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Para el cálculo del IGV se descuenta el valor capturado de la variable SMV (CONTRIBUCIÓN CONASEV) | 19/06/18 
            txtValorComisionIGV.Text = CType(dblComisionIGV, String).Replace(UIUtility.DecimalSeparator, ".")
            dgLista.Rows(indiceAux).Cells(cellIGV).Text = txtValorComisionIGV.Text
        End If
        dbltotalcomisiones = Math.Round(dblsubtotal + dblComisionIGV, 2)
        FormatoImpuestosComisiones(dgLista)
        Return dbltotalcomisiones
    End Function
    'Recalcula a partir de los datos ingresado en las Comisiones y Gastos
    Public Shared Function CalculaImpuestosComisionesNoRecalculo(ByVal dgLista As System.Web.UI.WebControls.GridView, ByVal Mercado As String, ByVal montoL As String, ByVal montoE As String, Optional ByVal grupoInt As String = "", Optional ByVal operacion As String = "", Optional ByVal claseInst As String = "", Optional ByVal tipoTramo As String = "", Optional ByVal tipoFondo As String = "") As Decimal 'HDG 20120224
        Dim dblsubtotal As Decimal = 0.0
        Dim dbltotalcomisiones As Decimal = 0.0
        Dim dblMontoL As Decimal = 0.0
        Dim dblMontoE As Decimal = 0.0
        Dim blnEsPar As Boolean = False
        Try
            Dim Comision = HttpUtility.HtmlDecode(dgLista.Rows(dgLista.Rows.Count - 1).Cells(8).Text).Trim
            Comision = IIf(Comision = "", "0", Comision)
            If ObtieneValorComision(Comision) >= 0 Then
                blnEsPar = True
            End If
        Catch ex As Exception
            blnEsPar = False
        End Try

        If montoL <> "" Then
            dblMontoL = Convert.ToDecimal(montoL.Replace(".", UIUtility.DecimalSeparator))
        End If
        If montoE <> "" Then
            dblMontoE = Convert.ToDecimal(montoE.Replace(".", UIUtility.DecimalSeparator))
        End If
        Dim i As Integer
        Dim indiceAux As Integer
        Dim cellAux As Integer
        Dim cellIGV As Integer
        Dim comiAux As String = ""
        Dim blnIGV As Boolean = False
        Dim dblComisionIGV As Decimal = 0.0
        For i = 0 To dgLista.Rows.Count - 1
            If dgLista.Rows(i).Cells(0).Text <> "P I.G.V. TOT" Then
                Dim txtValorComision As TextBox = CType(dgLista.Rows(i).FindControl("txtValorComision1"), TextBox)
                txtValorComision.Text = IIf(txtValorComision.Text.Trim = "", "0", txtValorComision.Text.Trim)
                dgLista.Rows(i).Cells(3).Text = txtValorComision.Text
                txtValorComision.Text = CType(txtValorComision.Text, String).Replace(UIUtility.DecimalSeparator, ".")
                dblsubtotal = dblsubtotal + Convert.ToDecimal(txtValorComision.Text.Replace(".", UIUtility.DecimalSeparator))
            Else
                indiceAux = i
                cellAux = 2
                cellIGV = 3
                comiAux = "txtValorComision1"
                blnIGV = True
            End If
            If i = dgLista.Rows.Count - 1 Then
                If blnEsPar = False Then
                    Exit For
                End If
            End If
            If dgLista.Rows(i).Cells(6).Text <> "P I.G.V. TOT" Then
                Dim txtValorComision As TextBox = CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox)
                txtValorComision.Text = IIf(txtValorComision.Text.Trim = "", "0", txtValorComision.Text.Trim)
                dgLista.Rows(i).Cells(9).Text = txtValorComision.Text
                txtValorComision.Text = CType(txtValorComision.Text, String).Replace(UIUtility.DecimalSeparator, ".")
                dblsubtotal = dblsubtotal + Convert.ToDecimal(txtValorComision.Text.Replace(".", UIUtility.DecimalSeparator))
            Else
                indiceAux = i
                cellAux = 8
                cellIGV = 9
                comiAux = "txtValorComision2"
                blnIGV = True
            End If
        Next
        If blnIGV = True Then
            Dim txtValorComisionIGV As TextBox = CType(dgLista.Rows(indiceAux).FindControl(comiAux), TextBox)
            dblComisionIGV = IIf(IsNumeric(txtValorComisionIGV.Text), txtValorComisionIGV.Text, 0)
            txtValorComisionIGV.Text = CType(dblComisionIGV, String).Replace(UIUtility.DecimalSeparator, ".")
        End If
        dbltotalcomisiones = Math.Round(dblsubtotal + dblComisionIGV, 2)
        FormatoImpuestosComisiones(dgLista)
        Return dbltotalcomisiones
    End Function
    Public Shared Function CalculaImpuestosComisionesOF(ByVal dgLista As System.Web.UI.WebControls.GridView, ByVal Mercado As String, ByVal montoL As String, ByVal montoE As String, ByVal OrdenFondo As String, ByVal TipoFondo As String) As Decimal
        Dim dblsubtotal As Decimal = 0.0
        Dim dbltotalcomisiones As Decimal = 0.0
        Dim dblMontoL As Decimal = 0.0
        Dim dblMontoE As Decimal = 0.0
        Dim blnEsPar As Boolean = False
        Try
            If ObtieneValorComision(dgLista.Rows(dgLista.Rows.Count - 1).Cells(8).Text) >= 0 Then
                blnEsPar = True
            End If
        Catch ex As Exception
            blnEsPar = False
        End Try
        If montoL <> "" Then
            dblMontoL = Convert.ToDecimal(montoL.Replace(".", UIUtility.DecimalSeparator))
        End If
        If montoE <> "" Then
            dblMontoE = Convert.ToDecimal(montoE.Replace(".", UIUtility.DecimalSeparator))
        End If
        Dim i As Integer
        Dim indiceAux As Integer
        Dim cellAux As Integer
        Dim cellIGV As Integer
        Dim comiAux As String
        Dim blnIGV As Boolean = False
        Dim dblComisionIGV As Decimal = 0.0
        For i = 0 To dgLista.Rows.Count - 1
            If dgLista.Rows(i).Cells(0).Text <> "P I.G.V. TOT" Then
                Dim txtValorComision As TextBox = CType(dgLista.Rows(i).FindControl("txtValorComision1"), TextBox)
                txtValorComision.Text = CType(dblMontoL * ObtieneValorComision(dgLista.Rows(i).Cells(2).Text), String).Replace(UIUtility.DecimalSeparator, ".")
                dgLista.Rows(i).Cells(3).Text = txtValorComision.Text
                dblsubtotal = dblsubtotal + Convert.ToDecimal(txtValorComision.Text.Replace(".", UIUtility.DecimalSeparator))
            Else
                indiceAux = i
                cellAux = 2
                cellIGV = 3
                comiAux = "txtValorComision1"
                blnIGV = True
            End If
            If i = dgLista.Rows.Count - 1 Then
                If blnEsPar = False Then
                    Exit For
                End If
            End If
            If dgLista.Rows(i).Cells(6).Text <> "P I.G.V. TOT" Then
                Dim txtValorComision As TextBox = CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox)
                txtValorComision.Text = CType(dblMontoL * ObtieneValorComision(dgLista.Rows(i).Cells(8).Text), String).Replace(UIUtility.DecimalSeparator, ".")
                dgLista.Rows(i).Cells(9).Text = txtValorComision.Text
                dblsubtotal = dblsubtotal + Convert.ToDecimal(txtValorComision.Text.Replace(".", UIUtility.DecimalSeparator))
            Else
                indiceAux = i
                cellAux = 8
                cellIGV = 9
                comiAux = "txtValorComision2"
                blnIGV = True
            End If
        Next
        If blnIGV = True Then
            Dim txtValorComisionIGV As TextBox = CType(dgLista.Rows(indiceAux).FindControl(comiAux), TextBox)
            dblComisionIGV = Math.Round(ObtieneValorComision(dgLista.Rows(indiceAux).Cells(cellAux).Text) * dblsubtotal, 7)
            txtValorComisionIGV.Text = CType(dblComisionIGV, String).Replace(UIUtility.DecimalSeparator, ".")
            dgLista.Rows(indiceAux).Cells(cellIGV).Text = txtValorComisionIGV.Text
        End If
        dbltotalcomisiones = Math.Round(dblsubtotal + dblComisionIGV, 2)
        FormatoImpuestosComisiones(dgLista)
        Return dbltotalcomisiones
    End Function
    Public Shared Sub ObtieneImpuestosComisionesGuardado(ByVal dgLista As System.Web.UI.WebControls.GridView, ByVal strCodigoOrden As String, ByVal strCodigoPortafolio As String)
        Dim objcomisiones As New ImpuestosComisionesOrdenPreOrdenBM
        Dim i As Integer
        Dim dtComisiones As DataTable = objcomisiones.ListarPorCodigoOrden(strCodigoOrden, strCodigoPortafolio).Tables(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        dtGrilla.Columns.Add("codigoComision1")
        dtGrilla.Columns.Add("Descripcion1")
        dtGrilla.Columns.Add("porcentajeComision1")
        dtGrilla.Columns.Add("strValorCalculadoComision1")
        dtGrilla.Columns.Add("ValorOcultoComision1")
        dtGrilla.Columns.Add("codigoComision2")
        dtGrilla.Columns.Add("Descripcion2")
        dtGrilla.Columns.Add("porcentajeComision2")
        dtGrilla.Columns.Add("strValorCalculadoComision2")
        dtGrilla.Columns.Add("ValorOcultoComision2")
        For i = 0 To dtComisiones.Rows.Count - 1
            If i Mod 2 = 0 Then
                drGrilla = dtGrilla.NewRow
                drGrilla("codigoComision1") = dtComisiones.Rows(i)("codigoComision")
                drGrilla("Descripcion1") = dtComisiones.Rows(i)("Descripcion")
                drGrilla("porcentajeComision1") = CType(dtComisiones.Rows(i)("porcentajeComision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drGrilla("strValorCalculadoComision1") = dtComisiones.Rows(i)("strValorCalculadoComision")
                drGrilla("ValorOcultoComision1") = dtComisiones.Rows(i)("txtValorComision")
                If i = dtComisiones.Rows.Count - 1 Then
                    dtGrilla.Rows.Add(drGrilla)
                    blnEsImpar = True
                End If
            Else
                drGrilla("codigoComision2") = dtComisiones.Rows(i)("codigoComision")
                drGrilla("Descripcion2") = dtComisiones.Rows(i)("Descripcion")
                drGrilla("porcentajeComision2") = CType(dtComisiones.Rows(i)("porcentajeComision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drGrilla("strValorCalculadoComision2") = dtComisiones.Rows(i)("strValorCalculadoComision")
                drGrilla("ValorOcultoComision2") = dtComisiones.Rows(i)("txtValorComision")
                dtGrilla.Rows.Add(drGrilla)
            End If
        Next
        dgLista.DataSource = dtGrilla
        dgLista.DataBind()
        For i = 0 To dgLista.Rows.Count - 1
            CType(dgLista.Rows(i).FindControl("txtValorComision1"), TextBox).Text = dgLista.Rows(i).Cells(5).Text.Replace(UIUtility.DecimalSeparator, ".")
            If i = dgLista.Rows.Count - 1 Then
                If blnEsImpar = True Then
                    CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox).Visible = False
                    Exit For
                End If
            End If
            CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox).Text = dgLista.Rows(i).Cells(11).Text.Replace(UIUtility.DecimalSeparator, ".")
        Next
        FormatoImpuestosComisiones(dgLista)
    End Sub
    Public Shared Function ObtenerTablaimpuestosComisionesGuardado(ByVal strCodigoOrden As String, ByVal strCodigoPortafolio As String) As DataSet
        Dim dsgrilla As New DataSet
        Dim objcomisiones As New ImpuestosComisionesOrdenPreOrdenBM
        Dim i As Integer
        Dim dtComisiones As DataTable = objcomisiones.ListarPorCodigoOrden(strCodigoOrden, strCodigoPortafolio).Tables(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        dtGrilla.Columns.Add("codigoComision1")
        dtGrilla.Columns.Add("Descripcion1")
        dtGrilla.Columns.Add("porcentajeComision1")
        dtGrilla.Columns.Add("strValorCalculadoComision1")
        dtGrilla.Columns.Add("ValorOcultoComision1")
        dtGrilla.Columns.Add("codigoComision2")
        dtGrilla.Columns.Add("Descripcion2")
        dtGrilla.Columns.Add("porcentajeComision2")
        dtGrilla.Columns.Add("strValorCalculadoComision2")
        dtGrilla.Columns.Add("ValorOcultoComision2")
        For i = 0 To dtComisiones.Rows.Count - 1
            If i Mod 2 = 0 Then
                'PAR, izq
                drGrilla = dtGrilla.NewRow
                drGrilla("codigoComision1") = dtComisiones.Rows(i)("codigoComision")
                drGrilla("Descripcion1") = dtComisiones.Rows(i)("Descripcion")
                drGrilla("porcentajeComision1") = CType(dtComisiones.Rows(i)("porcentajeComision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drGrilla("strValorCalculadoComision1") = dtComisiones.Rows(i)("strValorCalculadoComision")
                drGrilla("ValorOcultoComision1") = dtComisiones.Rows(i)("txtValorComision")
                If i = dtComisiones.Rows.Count - 1 Then
                    dtGrilla.Rows.Add(drGrilla)
                    blnEsImpar = True
                End If
            Else
                'IMPAR, der
                drGrilla("codigoComision2") = dtComisiones.Rows(i)("codigoComision")
                drGrilla("Descripcion2") = dtComisiones.Rows(i)("Descripcion")
                drGrilla("porcentajeComision2") = CType(dtComisiones.Rows(i)("porcentajeComision"), String).Replace(UIUtility.DecimalSeparator, ".")
                drGrilla("strValorCalculadoComision2") = dtComisiones.Rows(i)("strValorCalculadoComision")
                drGrilla("ValorOcultoComision2") = dtComisiones.Rows(i)("txtValorComision")
                dtGrilla.Rows.Add(drGrilla)
            End If
        Next
        dsgrilla.Tables.Add(dtGrilla)
        Return dsgrilla
    End Function
    Public Shared Sub InsertarModificarImpuestosComisiones(ByVal tipoAccion As String, ByVal dgLista As GridView, ByVal CodigoOrden As String, ByVal CodigoMercado As String, ByVal CodigoRenta As String, ByVal CodigoPortafolio As String, ByVal datosRequest As DataSet, ByVal CodigoPlaza As String)
        Dim i As Integer
        Dim blnEsPar As Boolean = False
        Try
            Dim dato As String = dgLista.Rows(dgLista.Rows.Count - 1).Cells(8).Text.Replace(".", UIUtility.DecimalSeparator)
            dato = dato.Replace("(", " ").Replace(")", " ").Replace("%", " ")
            dato = dato.Trim()
            If Convert.ToDecimal(dato.Replace(".", UIUtility.DecimalSeparator)) >= 0 Then
                blnEsPar = True
            End If
        Catch ex As Exception
            blnEsPar = False
        End Try
        Dim objimpuestoscomisionesOPBM As New ImpuestosComisionesOrdenPreOrdenBM
        objimpuestoscomisionesOPBM.Eliminar(CodigoOrden, CodigoPortafolio, datosRequest, CodigoMercado) 'HDG INC 63038	20110427
        For i = 0 To dgLista.Rows.Count - 1
            Dim valorCalculado As Decimal
            Dim valorReal As Decimal
            Try
                valorCalculado = Convert.ToDecimal(dgLista.Rows(i).Cells(3).Text.Replace(".", UIUtility.DecimalSeparator))
            Catch ex As Exception
                valorCalculado = 0
            End Try
            Try
                valorReal = Convert.ToDecimal(CType(dgLista.Rows(i).FindControl("txtValorComision1"), TextBox).Text.Replace(".", UIUtility.DecimalSeparator))
            Catch ex As Exception
                valorReal = 0
            End Try
            UIUtility.InsertarModificarImpuestosComisionesLinea(tipoAccion, CodigoOrden, CodigoPortafolio, _
            dgLista.Rows(i).Cells(0).Text, CodigoRenta, CodigoMercado, valorCalculado, valorReal, datosRequest, CodigoPlaza)
            If i = dgLista.Rows.Count - 1 Then
                If blnEsPar = False Then
                    Exit For
                End If
            End If
            Dim valorCalculado_ As Decimal = 0
            Dim valorReal_ As Decimal = 0
            Try
                valorCalculado_ = Convert.ToDecimal(dgLista.Rows(i).Cells(9).Text.Replace(".", UIUtility.DecimalSeparator))
            Catch ex As Exception
                valorCalculado_ = 0
            End Try
            Try
                valorReal_ = Convert.ToDecimal(CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox).Text.Replace(".", UIUtility.DecimalSeparator))
            Catch ex As Exception
                valorReal_ = 0
            End Try
            UIUtility.InsertarModificarImpuestosComisionesLinea(tipoAccion, CodigoOrden, CodigoPortafolio, _
            dgLista.Rows(i).Cells(6).Text, CodigoRenta, CodigoMercado, valorCalculado_, valorReal_, datosRequest, CodigoPlaza)
        Next
    End Sub
    Private Shared Sub InsertarModificarImpuestosComisionesLinea(ByVal tipoAccion As String, ByVal CodigoOrden As String, ByVal CodigoPortafolio As String, ByVal CodigoComision As String, ByVal CodigoRenta As String, ByVal CodigoMercado As String, ByVal dblValorCalculado As Decimal, ByVal dblValorReal As Decimal, ByVal datosRequest As DataSet, ByVal CodigoPlaza As String)
        Dim objimpuestoscomisionesOPBM As New ImpuestosComisionesOrdenPreOrdenBM
        Dim oImpuestosComisionesBE As New ImpuestosComisionesOrdenPreOrdenBE
        Dim oRow As ImpuestosComisionesOrdenPreOrdenBE.ImpuestosComisionesOrdenPreOrdenRow
        oRow = CType(oImpuestosComisionesBE.ImpuestosComisionesOrdenPreOrden.NewRow(), ImpuestosComisionesOrdenPreOrdenBE.ImpuestosComisionesOrdenPreOrdenRow)
        oRow.CodigoOrdenPreorden = CodigoOrden
        oRow.CodigoPortafolioSBS = CodigoPortafolio
        oRow.CodigoTarifa = CodigoComision
        oRow.CodigoRenta = CodigoRenta
        oRow.CodigoMercado = CodigoMercado
        oRow.ValorCalculado = dblValorCalculado
        oRow.ValorReal = dblValorReal
        oRow.CodigoPlaza = CodigoPlaza
        oImpuestosComisionesBE.ImpuestosComisionesOrdenPreOrden.AddImpuestosComisionesOrdenPreOrdenRow(oRow)
        oImpuestosComisionesBE.ImpuestosComisionesOrdenPreOrden.AcceptChanges()
        Dim resul As Boolean = False
        resul = objimpuestoscomisionesOPBM.VerificarExistencia(CodigoOrden, CodigoPortafolio, CodigoComision, CodigoRenta, CodigoMercado)
        If resul = False Then
            objimpuestoscomisionesOPBM.Insertar(oImpuestosComisionesBE, datosRequest)
        ElseIf resul = True Then
            objimpuestoscomisionesOPBM.Modificar(oImpuestosComisionesBE, datosRequest)
        End If
    End Sub
    Public Shared Sub FormatoImpuestosComisiones(ByRef dgLista As System.Web.UI.WebControls.GridView)
        Dim i As Integer

        For i = 0 To dgLista.Rows.Count - 1

            Dim txtValorComision1 As TextBox = CType(dgLista.Rows(i).FindControl("txtValorComision1"), TextBox)
            If txtValorComision1.Text.Trim <> "" Then
                txtValorComision1.Text = Format(Convert.ToDecimal(txtValorComision1.Text), "##,##0.00")
            End If

            Dim txtValorComision2 As TextBox = CType(dgLista.Rows(i).FindControl("txtValorComision2"), TextBox)
            If txtValorComision2.Text.Trim <> "" Then
                txtValorComision2.Text = Format(Convert.ToDecimal(txtValorComision2.Text), "##,##0.00")
            End If

        Next

    End Sub
    Public Shared Function ObtieneUnCustodio(ByVal custodios As String) As String
        Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
        Dim custodioArreglo() As String = custodios.Split(strSeparador)
        Dim i As Integer
        Dim decCantidad As Decimal = 0
        If custodioArreglo.Length > 2 Then
            For i = 0 To custodioArreglo.Length - 1
                i = i + 1
                custodioArreglo(i) = custodioArreglo(i).Replace(",", "")
                decCantidad = decCantidad + custodioArreglo(i).Replace(".", UIUtility.DecimalSeparator)
            Next
        Else
            custodioArreglo(1) = custodioArreglo(1).Replace(",", "")
            decCantidad = custodioArreglo(1).Replace(".", UIUtility.DecimalSeparator)
        End If
        Return custodioArreglo(0) + strSeparador + decCantidad.ToString.Replace(UIUtility.DecimalSeparator, ".")
    End Function
    Public Shared Function ObtieneCustodiosOI(ByVal codigoOrden As String, ByVal fondo As String, ByVal datarequest As DataSet, ByVal custodio As String, ByRef saldo As String) As String
        Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
        Dim oOIbm As New OrdenPreOrdenInversionBM
        Dim dtAux As DataTable
        Dim strAux As String = ""
        Dim i As Integer
        dtAux = oOIbm.OtroSeleccionarPorFiltro_CustodioOI(codigoOrden, fondo, datarequest).Tables(0)
        If Not dtAux Is Nothing Then
            If dtAux.Rows.Count > 0 Then
                For i = 0 To dtAux.Rows.Count - 1
                    If strAux = "" Then
                        strAux = dtAux.Rows(i)("codigoCustodio") + strSeparador + Format(dtAux.Rows(i)("valorAsignacion"), "##0.0000000")
                    Else
                        strAux = strAux + strSeparador + dtAux.Rows(i)("codigoCustodio") + strSeparador + Format(dtAux.Rows(i)("valorAsignacion"), "##0.0000000")
                    End If
                    If custodio = dtAux.Rows(i)("codigoCustodio") Then
                        saldo = Format(dtAux.Rows(i)("valorAsignacion"), "##0.0000000")
                    End If
                Next
            End If
        End If
        Return strAux
    End Function
    Public Shared Function ObtenerSumatoriaSaldosSeleccionados(ByVal cadenaCustodio As String, ByRef cantCustodios As Integer) As Decimal
        If cadenaCustodio Is Nothing Then
            Return 0
        End If
        If cadenaCustodio = "" Then
            Return 0
        End If
        Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
        Dim strAux() As String = cadenaCustodio.Split(strSeparador)
        Dim i As Integer
        Dim decTotal As Decimal = 0.0
        Dim strAuxiliar As String
        cantCustodios = 0
        For i = 0 To strAux.Length - 1
            i = i + 1
            cantCustodios = cantCustodios + 1
            strAuxiliar = strAux(i).Replace(",", "")
            decTotal = decTotal + Convert.ToDecimal(strAuxiliar.Replace(".", UIUtility.DecimalSeparator))
        Next
        Return decTotal
    End Function
    Public Shared Function AjustarMontosCustodios(ByVal cadenaCustodio As String, ByVal montoNuevo As String) As String
        Dim strSeparador As String = ParametrosSIT.SEPARADOR_OI
        Dim strAux() As String = cadenaCustodio.Split(strSeparador)
        Dim i As Integer
        Dim decTotal As Decimal = 0.0
        Dim strAuxiliar As String
        Dim decMontoNuevo As Decimal = Convert.ToDecimal(montoNuevo.Replace(",", ""))
        cadenaCustodio = ""
        For i = 0 To strAux.Length - 1
            i = i + 1
            strAuxiliar = strAux(i).Replace(",", "")
            decTotal = decTotal + Convert.ToDecimal(strAuxiliar.Replace(".", UIUtility.DecimalSeparator))
            If decTotal > decMontoNuevo Then
                Dim decAux1 As Decimal = Convert.ToDecimal(strAuxiliar.Replace(".", UIUtility.DecimalSeparator))
                decAux1 = decAux1 - (decTotal - decMontoNuevo)
                cadenaCustodio = cadenaCustodio + strSeparador + strAux(i - 1) + strSeparador + decAux1.ToString.Replace(UIUtility.DecimalSeparator, ".")
                Exit For
            ElseIf decTotal = decMontoNuevo Then
                cadenaCustodio = cadenaCustodio + strSeparador + strAux(i - 1) + strSeparador + strAux(i)
            Else
                If cadenaCustodio = "" Then
                    cadenaCustodio = strAux(i - 1) + strSeparador + strAux(i)
                Else
                    cadenaCustodio = cadenaCustodio + strSeparador + strAux(i - 1) + strSeparador + strAux(i)
                End If
            End If
        Next
        Return cadenaCustodio
    End Function
#End Region
#Region " /* Métodos Personalizados (Vol. 7) */ "
    Public Shared Function DecimalSeparator() As String
        Return System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator
    End Function
    Public Shared Sub CrearConfirmDialogBox(ByVal btn As ImageButton, ByVal CodigoMensaje As String, Optional ByVal MensajeAdicional As String = "")
        Dim _objUtilitario As New UtilDM
        Dim Mensaje As String = _objUtilitario.RetornarMensajeConfirmacion(CodigoMensaje)
        Mensaje = Mensaje + " " + MensajeAdicional
        btn.Attributes.Add("onclick", "return confirm('" & Mensaje & "');")
    End Sub
    Public Shared Sub CrearConfirmDialogBox(ByVal btn As Button, ByVal CodigoMensaje As String, Optional ByVal MensajeAdicional As String = "")
        Dim _objUtilitario As New UtilDM
        Dim Mensaje As String = _objUtilitario.RetornarMensajeConfirmacion(CodigoMensaje)
        Mensaje = Mensaje + " " + MensajeAdicional
        btn.Attributes.Add("onclick", "return confirm('" & Mensaje & "');")
    End Sub
    Public Shared Function ValidarPortafolioAperturado(ByVal codigoPortafolio As String) As Boolean
        Dim oPortafolio As New PortafolioBM
        Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
        Dim fechaTermino As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaTermino")
        Dim FechaConstitucion As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaConstitucion")
        Return (fechaTermino <> FechaConstitucion)
    End Function
    Public Shared Function ObtenerFechaApertura(ByVal codigoPortafolio As String) As Decimal
        Dim oPortafolio As New PortafolioBM
        Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
        If codigoPortafolio.Trim.Equals("") Then
            codigoPortafolio = PORTAFOLIO_MULTIFONDOS
        End If
        Dim fechaConstitucion As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaConstitucion")
        Return fechaConstitucion
    End Function
    Public Shared Function ObtenerFechaNegocio(ByVal codigoPortafolio As String) As Decimal
        Dim oPortafolio As New PortafolioBM
        Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
        If codigoPortafolio.Trim.Equals("") Then
            codigoPortafolio = PORTAFOLIO_MULTIFONDOS
        End If
        Dim fechaNegocio As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaNegocio")
        Return fechaNegocio
    End Function
    Public Shared Function ObtenerFechaCajaOperaciones(ByVal codigoPortafolio As String, ByVal codigoClaseCuenta As String, Optional ByVal DatosRequest As DataSet = Nothing) As Decimal
        Dim fechaNegocio As Decimal
        Dim oPortafolioCajaBM As New PortafolioCajaBM
        Dim dtPortafolioCaja As DataTable = oPortafolioCajaBM.ObtenerFechaCajaOperaciones(codigoPortafolio, codigoClaseCuenta)

        If dtPortafolioCaja.Rows.Count > 0 Then
            Dim dr As DataRow = dtPortafolioCaja.Rows(0)
            fechaNegocio = dr("FechaCaja")
        Else
            Dim oPortafolio As New PortafolioBM
            Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
            fechaNegocio = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaCajaOperaciones")
            'oPortafolioCajaBM.Insertar(codigoPortafolio, codigoClaseCuenta, fechaNegocio, DatosRequest)
        End If

        Return (fechaNegocio)


    End Function
    Public Shared Function ValidarCajas(ByVal codigoPortafolio As String, ByVal codigoClaseCuenta As String, Optional ByVal fechaOperacion As Decimal = 0) As String
        Dim resultado As String = ""
        Dim oPortafolioCajaBM As New PortafolioCajaBM
        Dim oPortafolio As New PortafolioBM
        Dim dtCajas As DataTable = oPortafolioCajaBM.ObtenerFechaCajaOperaciones(codigoPortafolio, "")


        If dtCajas.Rows.Count = 0 Then
            Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
            Dim fechaCaja As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaCajaOperaciones")

            'Para ValorCuota
            If fechaOperacion <> 0 Then
                If fechaCaja <= fechaOperacion Then
                    resultado = "Falta realizar el cierre de caja"
                End If
            End If


            'resultado = "No existe cierre de caja para el fondo " + dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("Descripcion")
            Return resultado
        End If

        If codigoClaseCuenta <> "" Then
            Dim hayDiferencia As Boolean = False
            Dim fila As DataRow = (From dr In dtCajas
                     Where dr("CodigoPortafolioSBS") = codigoPortafolio And dr("CodigoClaseCuenta") = codigoClaseCuenta Select dr
                     ).FirstOrDefault()

            If fila IsNot Nothing Then

                For Each dr As DataRow In dtCajas.Rows
                    If fila("FechaCaja") <> dr("FechaCaja") Then
                        hayDiferencia = True
                    End If
                    resultado = resultado + "<br>-Cierre de caja  " + dr("ClaseCuenta") + ": " + dr("FechaCajaCadena")

                Next

                If hayDiferencia = True Then
                    If resultado <> "" Then
                        resultado = "Las fechas de cierre de las cajas no coinciden. " + resultado
                    End If
                Else
                    Return ""
                End If
               
             

                'If fila("FechaCaja") <> fila("FechaCajaPortafolio") Then
                '    resultado = "Las fechas de cierre de cajas no coinciden para el fondo " + fila("Portafolio") + ":"

                '    For Each dr As DataRow In dtCajas.Rows
                '        resultado = resultado + "<br>-Cierre de caja " + dr("ClaseCuenta") + ": " + dr("FechaCajaCadena")
                '    Next

                'End If
            Else
                Dim oClaseCuentaBM As New ClaseCuentaBM
                Dim oClaseCuentaBE As ClaseCuentaBE = oClaseCuentaBM.SeleccionarPorFiltro(codigoClaseCuenta, "", "", Nothing)
                Dim descripcionClaseCuenta As String = oClaseCuentaBE.Tables(0).Rows(0)("Descripcion")

                Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)

                resultado = "No existe cierre de caja para el fondo " + dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("Descripcion") + " con Caja " + descripcionClaseCuenta


            End If
        Else

            'Validacion para Valor Cuota
            Dim hayDiferencia As Boolean = False
            If fechaOperacion <> 0 Then
                Dim listaGeneral = (From dr In dtCajas
                    Where dr("CodigoPortafolioSBS") = codigoPortafolio And Convert.ToDecimal(dr("FechaCaja")) <= fechaOperacion Select dr
                    )
                If listaGeneral IsNot Nothing Then
                    If listaGeneral.Count() > 0 Then
                        For Each dr As DataRow In listaGeneral
                            resultado = resultado + "<br>-" + dr("ClaseCuenta")
                        Next
                        resultado = "Falta realizar el cierre en las siguientes cajas:" + resultado
                        Return resultado
                    Else
                        Return ""
                    End If
                Else
                    Return ""
                End If
            End If

            'Recorrer todas las cajas de ese fondo
            'For Each dr As DataRow In dtCajas.Rows
            '    If dr("FechaCaja") <> dr("FechaCajaPortafolio") Then
            '        hayDiferencia = True
            '    End If
            '    resultado = resultado + "<br>-Cierre de caja  " + dr("ClaseCuenta") + ": " + dr("FechaCajaCadena")

            'Next
            'If resultado <> "" Then
            '    resultado = "Las fechas de cierre de cajas no coinciden" + resultado
            'End If

        End If



        Return resultado
    End Function

    Public Shared Function ObtenerFechaCaja(ByVal codigoPortafolio As String, ByVal codigoClaseCuenta As String) As DataTable
        Dim fechaNegocio As Decimal
        Dim oPortafolioCajaBM As New PortafolioCajaBM

        Dim dr As DataRow

        Dim dtPortafolioCaja As DataTable = oPortafolioCajaBM.ObtenerFechaCajaOperaciones(codigoPortafolio, codigoClaseCuenta)

        'If dtPortafolioCaja.Rows.Count = 0 Then
        '    Dim oPortafolio As New PortafolioBM
        '    Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
        '    fechaNegocio = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaCajaOperaciones")
        '    oPortafolioCajaBM.Insertar(codigoPortafolio, codigoClaseCuenta, fechaNegocio, DatosRequest)
        '    dtPortafolioCaja = oPortafolioCajaBM.ObtenerFechaCajaOperaciones(codigoPortafolio, codigoClaseCuenta)
        'End If
        'dr = dtPortafolioCaja.Rows(0)
        Return dtPortafolioCaja

        'If dtPortafolioCaja.Rows.Count = 0 Then
        '    Dim oPortafolio As New PortafolioBM
        '    Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
        '    fechaNegocio = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaCajaOperaciones")
        '    oPortafolioCajaBM.Insertar(codigoPortafolio, codigoClaseCuenta, fechaNegocio, DatosRequest)
        '    dtPortafolioCaja = oPortafolioCajaBM.ObtenerFechaCajaOperaciones(codigoPortafolio, codigoClaseCuenta)
        'End If
        'dr = dtPortafolioCaja.Rows(0)
        'Return dr
    End Function

    Public Shared Function ObtenerFechaMaximaNegocio() As Decimal
        Dim oPortafolio As New PortafolioBM
        Dim dsPortafolio As DataTable = oPortafolio.FechaMaximaPortafolio()
        Dim fechaNegocio As Decimal = dsPortafolio(0)("FechaNegocio")
        Return fechaNegocio
    End Function
    Public Shared Function ObtenerFechaAnterior(ByVal codigoPortafolio As String) As Decimal
        Dim oPortafolio As New PortafolioBM
        Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
        Dim fechaConstitucion As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaTermino")
        Return fechaConstitucion
    End Function
    Public Shared Function ObtenerDatosPortafolio(ByVal codigoPortafolio As String) As String
        Dim oPortafolio As New PortafolioBM
        Dim dsPortafolio As DataSet = oPortafolio.ObtenerDatosPortafolio(Nothing, ParametrosSIT.ESTADO_ACTIVO)
        Dim sHoraConstitucion As String = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("HoraConstitucion")
        Return sHoraConstitucion
    End Function
    Public Shared Function ObtenerFechaAperturaContable(ByVal codigoPortafolio As String) As Decimal
        Dim oPortafolio As New PortafolioBM
        Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
        If codigoPortafolio.Trim.Equals("") Then
            codigoPortafolio = PORTAFOLIO_MULTIFONDOS
        End If
        Dim fechaConstitucion As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaAperturaContable")
        Return fechaConstitucion
    End Function
    Public Shared Function ObtenerFechaAperturaContableAnterior(ByVal codigoPortafolio As String) As Decimal
        Dim oPortafolio As New PortafolioBM
        Dim dsPortafolio As DataSet = oPortafolio.Listar(Nothing, ParametrosSIT.ESTADO_ACTIVO)
        Dim fechaAnterior As Decimal = dsPortafolio.Tables(0).Select("CodigoPortafolioSBS='" & codigoPortafolio & "'")(0)("FechaAperturaContableAnterior")
        Return fechaAnterior
    End Function
    Public Shared Function ObtenerCodigoTipoMercado(ByVal mercado As String) As String
        Dim tipomercado As String = String.Empty
        Select Case mercado
            Case Constantes.M_STR_MERCADO_LOCAL : tipomercado = System.Configuration.ConfigurationSettings.AppSettings("MERCADO_LOCAL").ToString()
            Case Constantes.M_STR_MERCADO_EXTRANJERO : tipomercado = System.Configuration.ConfigurationSettings.AppSettings("MERCADO_EXTRANJERO").ToString()
        End Select
        Return tipomercado
    End Function
    Public Shared Function ObtenerCodigoTipoOperacion(ByVal operacion As String) As String
        Dim tipoOperacion As String = String.Empty
        Select Case operacion
            Case Constantes.M_COMPRA : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_COMPRA").ToString()
            Case Constantes.M_VENTA : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_VENTA").ToString()
            Case Constantes.M_CONS : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_CONS").ToString()
        End Select
        Return tipoOperacion
    End Function
    Public Shared Function ObtenerCodigoTipoOperacionPorTraspaso(ByVal operacion As String) As String
        Dim tipoOperacion As String = String.Empty
        Select Case operacion
            Case Constantes.M_TRASPASO_EGRESO : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_TRASPASO_EGRESO").ToString()
            Case Constantes.M_TRASPASO_INGRESO : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_TRASPASO_INGRESO").ToString()
        End Select
        Return tipoOperacion
    End Function
    Public Shared Function ObtenerCodigoOperacionCompra() As String
        Return System.Configuration.ConfigurationSettings.AppSettings("OPERACION_COMPRA").ToString()
    End Function
    Public Shared Function ObtenerCodigoOperacionVenta() As String
        Return System.Configuration.ConfigurationSettings.AppSettings("OPERACION_VENTA").ToString()
    End Function
    Public Shared Function ObtenerCodigoOperacionTEgreso() As String
        Return System.Configuration.ConfigurationSettings.AppSettings("OPERACION_TRASPASO_EGRESO").ToString()
    End Function
    Public Shared Function ObtenerCodigoOperacionTIngreso() As String
        Return System.Configuration.ConfigurationSettings.AppSettings("OPERACION_TRASPASO_INGRESO").ToString()
    End Function
    Public Shared Function ObtenerCodigoTipoOperacionDP(ByVal operacion As String) As String
        Dim tipoOperacion As String = String.Empty
        Select Case operacion
            Case Constantes.M_CONS : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_CONS").ToString()
            Case Constantes.M_CANC : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_CANC").ToString()
            Case Constantes.M_RENO : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_RENO").ToString()
            Case Constantes.M_PREC : tipoOperacion = System.Configuration.ConfigurationSettings.AppSettings("OPERACION_PREC").ToString()
        End Select
        Return tipoOperacion
    End Function
    Public Shared Function ObtenerCodigoTipoRenta(ByVal tipoRenta As String) As String
        Dim tipoR As String = String.Empty
        Select Case tipoRenta
            Case Constantes.M_RFIJ : tipoR = System.Configuration.ConfigurationSettings.AppSettings("TR_FIJA").ToString()
            Case Constantes.M_RVAR : tipoR = System.Configuration.ConfigurationSettings.AppSettings("TR_VARIABLE").ToString()
            Case Constantes.M_RDER : tipoR = System.Configuration.ConfigurationSettings.AppSettings("TR_DERIVADOS").ToString()
        End Select
        Return tipoR
    End Function
    Public Shared Function ObtenerPlazo_CalifInst(ByVal califInst As String) As String
        Dim plazo As String = String.Empty
        Select Case califInst
            Case Constantes.M_CI_PLAZO_LP : plazo = System.Configuration.ConfigurationSettings.AppSettings("TR_FIJA").ToString()
            Case Constantes.M_CI_PLAZO_RV : plazo = System.Configuration.ConfigurationSettings.AppSettings("TR_VARIABLE").ToString()
            Case Constantes.M_CI_PLAZO_CP : plazo = System.Configuration.ConfigurationSettings.AppSettings("TR_FIJA").ToString()
            Case Constantes.M_CI_PLAZO_DV : plazo = System.Configuration.ConfigurationSettings.AppSettings("TR_DERIVADOS").ToString()
        End Select
        Return plazo
    End Function
#End Region
#Region " /* Métodos Personalizados (Vol. 8) */ "
    Public Shared Function GetStructureTablebase(ByVal campos() As String, Optional ByVal tipos() As String = Nothing, Optional ByVal tableName As String = "", Optional ByRef mensaje As String = "") As DataTable
        Dim dtblBase As New DataTable(IIf(tableName.Length > 0, tableName, String.Empty))
        Dim tiposDatos As Boolean = False
        Dim type As String = String.Empty
        If Not tipos Is Nothing Then
            tiposDatos = True
        Else
            type = "System.String"
        End If
        Try
            For i As System.Int16 = 0 To campos.Length - 1
                If tiposDatos Then type = tipos(i).ToString()
                dtblBase.Columns.Add(New DataColumn(campos(i).ToString, System.Type.GetType(type)))
            Next
        Catch ex As Exception
            mensaje = ex.Message : Return Nothing
        End Try
        Return dtblBase
    End Function
#End Region
#Region " /* Métodos Personalizados (Vol. 9) */ "
    Public Shared Function MostrarResultadoBusqueda(ByVal dtblDatos As DataSet) As String
        Dim iCount As Integer = dtblDatos.Tables(0).Rows.Count
        Dim resultado As String = String.Empty
        resultado = Constantes.M_STR_TEXTO_RESULTADOS & iCount.ToString("#0") 'MC
        Return resultado
    End Function
    Public Shared Function MostrarResultadoBusqueda(ByVal dtblDatos As DataTable) As String
        Dim iCount As Integer = dtblDatos.Rows.Count
        Dim resultado As String = String.Empty
        resultado = Constantes.M_STR_TEXTO_RESULTADOS & iCount.ToString("#0") 'MC
        Return resultado
    End Function
    Public Shared Function MostrarResultadoBusqueda(ByVal iCount As Integer) As String
        Dim resultado As String = String.Empty
        resultado = Constantes.M_STR_TEXTO_RESULTADOS & iCount.ToString("#0") 'MC
        Return resultado
    End Function
    Public Shared Function MostrarResultadoBusqueda(ByVal dvwVista As DataView) As String
        Dim iCount As Integer = dvwVista.Count
        Dim resultado As String = String.Empty
        If iCount = 0 Then
            resultado = Constantes.M_STR_TEXTO_RESULTADOS & "0"
        Else
            resultado = Constantes.M_STR_TEXTO_RESULTADOS & IIf(dvwVista.Count < 10, "0" & dvwVista.Count, dvwVista.Count)
        End If
        Return resultado
    End Function
    Public Shared Function MostrarResultadoCarga(ByVal dtblDatos As DataTable) As String
        Dim iCount As Integer = dtblDatos.Rows.Count
        Dim resultado As String = String.Empty
        If iCount = 0 Then
            resultado = Constantes.M_STR_TEXTO_CARGADOS & "0"
        Else
            resultado = Constantes.M_STR_TEXTO_CARGADOS & IIf(dtblDatos.Rows.Count < 10, "0" & dtblDatos.Rows.Count, dtblDatos.Rows.Count)
        End If
        Return resultado
    End Function
#End Region
#Region " /* Métodos Personalizados (Vol. 10) */ "
    Public Shared Function CargarSaldoXTercero(ByVal saldoActual As String, ByVal strOperacion As String, ByVal strIntermediario As String, ByVal strFondo As String, ByVal strMnemonico As String, ByVal strFecha As String, ByVal datosRequest As DataSet) As String
        Dim resultado As String = ""
        If strOperacion = UIUtility.ObtenerCodigoTipoOperacion("VENTA") Then
            Dim objTercerosAux As New TercerosBM
            Dim objDS_Aux As DataSet = objTercerosAux.Tercero_ObtenerSaldoCustodio(strIntermediario, strFondo, strMnemonico, UIUtility.ConvertirFechaaDecimal(strFecha), datosRequest)
            If objDS_Aux.Tables(0).Rows.Count > 0 Then
                Dim resulValor As Decimal = 0
                resulValor = Convert.ToDecimal(objDS_Aux.Tables(0).Rows(0)("SaldoDisponible"))
                If resulValor = -1 Then
                    resultado = saldoActual
                Else
                    resultado = objDS_Aux.Tables(0).Rows(0)("SaldoDisponible")
                End If
            Else
                resultado = "0"
            End If
        Else
            resultado = saldoActual
        End If
        Return resultado
    End Function
#End Region
#Region "/* Métodos Personalizados (Vol. 11) */"
    Public Shared Function ObtenerDescripcionTercero(ByVal codigoTercero As String, ByVal datosRequest As DataSet) As String
        Dim objBM As New TercerosBM
        Dim objBE As New TercerosBE
        Dim strDes As String = ""
        objBE = objBM.Seleccionar(codigoTercero, datosRequest)
        If objBE.Tables(0).Rows.Count > 0 Then
            strDes = objBE.Tables(0).Rows(0)("Descripcion")
        End If
        Return strDes
    End Function
    Public Shared Function ObtenerDescripcionContacto(ByVal codigoContacto As String, ByVal datosRequest As DataSet) As String
        Dim objBM As New ContactoBM
        Dim objBE As New ContactoBE
        Dim strDes As String = ""
        objBE = objBM.Seleccionar(codigoContacto, datosRequest)
        If objBE.Tables(0).Rows.Count > 0 Then
            strDes = objBE.Tables(0).Rows(0)("Descripcion")
        End If
        Return strDes
    End Function
    Public Shared Function ObtenerDescripcionClaseInstrumento(ByVal codigoCI As String) As String
        Dim strDato As String = ""
        Select Case codigoCI
            Case "BO" : strDato = "BONOS"
            Case "AC" : strDato = "ACCIONES"
            Case "CD" : strDato = "CERTIFICADO DE DEPOSITO"
            Case "CS" : strDato = "CERTIFICADO DE SUSCRIPCION"
            Case "PC" : strDato = "PAPELES COMERCIALES"
            Case "PA" : strDato = "PAGARES"
            Case "FI" : strDato = "ORDENES DE FONDO"
            Case "FM" : strDato = "ORDENES DE FONDO"
            Case "OR" : strDato = "OPERACIONES DE REPORTE"
            Case "IC" : strDato = "INSTRUMENTOS COBERTURADOS"
            Case "LH" : strDato = "LETRAS HIPOTECARIAS"
            Case "IE" : strDato = "INSTRUMENTOS ESTRUCTURADOS"
            Case "DP" : strDato = "DEPOSITOS A PLAZO" 'RGF 20090702
            Case "FD" : strDato = "OPERACIONES DERIVADAS - FORWARD DIVISAS" 'CMB OT 61566 20101210
            Case "CV" : strDato = "COMPRA/VENTA MONEDA EXTRANJERA" 'CMB OT 61566 20101210
        End Select
        Return strDato
    End Function
    Public Shared Function ObtenerDescripcionMoneda(ByVal codigoMoneda As String, ByVal datosRequest As DataSet) As String
        Dim objBM As New MonedaBM
        Dim objBE As New MonedaBE
        Dim strDescripcion As String = ""
        objBE = objBM.SeleccionarPorFiltro(codigoMoneda, "", "A", "", "", datosRequest)
        If objBE.Tables(0).Rows.Count > 0 Then
            strDescripcion = objBE.Tables(0).Rows(0)("Descripcion")
        End If
        Return strDescripcion
    End Function
    Public Shared Function ObtenerDescripcionMotivo(ByVal codigoMotivo As String, ByVal datosRequest As DataSet) As String
        Dim objBM As New MotivoBM
        Dim objBE As New MotivoBE
        Dim strDescripcion As String = ""
        objBE = objBM.SeleccionarPorFiltro(codigoMotivo, "", "A", datosRequest)
        If objBE.Tables(0).Rows.Count > 0 Then
            strDescripcion = objBE.Tables(0).Rows(0)("Descripcion")
        End If
        Return strDescripcion
    End Function
    Public Shared Function ObtenerDatosOperacion_Acciones(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
        Dim objOI_BE As New OrdenPreOrdenInversionBE
        Dim objOI_BM As New OrdenPreOrdenInversionBM
        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = objOI_BE.Tables(0).Rows(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        drGrilla("v3") = "Hora Operación"
        drGrilla("c3") = oRow.HoraOperacion
        drGrilla("c4") = "Numero Acciones Ordenadas"
        drGrilla("v4") = Format(oRow.CantidadOrdenado, "##,##0.0000000")
        drGrilla("c5") = "Numero Acciones Operación"
        drGrilla("v5") = Format(oRow.CantidadOperacion, "##,##0.0000000")
        drGrilla("c6") = "Precio"
        drGrilla("v6") = Format(oRow.Precio, "##,##0.0000000")
        drGrilla("c7") = "Monto Operacion"
        drGrilla("v7") = Format(oRow.MontoOperacion, "##,##0.0000000")
        drGrilla("c8") = "Intermediario"
        drGrilla("v8") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
        If oRow.CodigoContacto <> "" Then
            drGrilla("c9") = "Contacto"
            drGrilla("v9") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
        Else
            drGrilla("c9") = ""
            drGrilla("v9") = ""
        End If
        drGrilla("c10") = "Observación"
        drGrilla("v10") = oRow.Observacion
        If oRow.NumeroPoliza <> "" Then
            drGrilla("c11") = "Poliza"
            drGrilla("v11") = oRow.NumeroPoliza
        Else
            drGrilla("c11") = ""
            drGrilla("v11") = ""
        End If
        drGrilla("c12") = ""
        drGrilla("v12") = ""
        drGrilla("c13") = ""
        drGrilla("v13") = ""
        drGrilla("c14") = ""
        drGrilla("v14") = ""
        drGrilla("c15") = ""
        drGrilla("v15") = ""
        drGrilla("c16") = ""
        drGrilla("v16") = ""
        drGrilla("c17") = ""
        drGrilla("v17") = ""
        drGrilla("c18") = ""
        drGrilla("v18") = ""
        drGrilla("c19") = "Total Comisiones"
        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
        drGrilla("c20") = "Precio Promedio"
        drGrilla("v20") = Format(oRow.PrecioPromedio, "##,##0.0000000")
        drGrilla("c21") = "Monto Neto Operacion"
        drGrilla("v21") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Shared Function ObtenerDatosOperacion_Bonos(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
        Dim objOI_BE As New OrdenPreOrdenInversionBE
        Dim objOI_BM As New OrdenPreOrdenInversionBM
        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = objOI_BE.Tables(0).Rows(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operacion"
        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        drGrilla("c3") = "Hora Operacion"
        drGrilla("v3") = oRow.HoraOperacion
        drGrilla("c4") = "Monto Nominal Ordenado"
        drGrilla("v4") = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
        drGrilla("c5") = "Monto Nominal Operacion"
        drGrilla("v5") = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
        drGrilla("c6") = "Tipo Tasa"
        drGrilla("v6") = IIf(oRow.CodigoTipoCupon = "1", "NOMINAL", "EFECTIVA")
        drGrilla("c7") = "YTM%"
        drGrilla("v7") = Format(oRow.YTM, "##,##0.0000000")
        drGrilla("c8") = "Precio Negociacion %"
        drGrilla("v8") = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000000")
        drGrilla("c9") = "Precio Calculado %"
        drGrilla("v9") = Format(oRow.PrecioCalculado, "##,##0.0000000")
        drGrilla("c10") = "Precio Negociacion Sucio"
        drGrilla("v10") = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
        drGrilla("c11") = "Interes Corrido Negociado"
        drGrilla("v11") = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
        drGrilla("c12") = "Interes Corrido"
        drGrilla("v12") = Format(oRow.InteresCorrido, "##,##0.0000000")
        drGrilla("c13") = "Monto Operacion"
        drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.00")
        drGrilla("c14") = "Numero Papeles"
        drGrilla("v14") = Format(oRow.CantidadOperacion, "##,##0.0000000")
        drGrilla("c15") = "Intermediario" '***
        drGrilla("v15") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
        If oRow.CodigoContacto <> "" Then
            drGrilla("c16") = "Contacto"
            drGrilla("v16") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
        Else
            drGrilla("c16") = ""
            drGrilla("v16") = ""
        End If
        If oRow.NumeroPoliza <> "" Then
            drGrilla("c17") = "Numero Poliza"
            drGrilla("v17") = oRow.NumeroPoliza
        Else
            drGrilla("c17") = ""
            drGrilla("v17") = ""
        End If
        drGrilla("c18") = "Observacion"
        drGrilla("v18") = oRow.Observacion
        drGrilla("c19") = "Total Comisiones"
        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
        drGrilla("c20") = "Monto Neto Operacion"
        drGrilla("v20") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Shared Function ObtenerDatosOperacion_CertificadoDeposito(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
        Dim objOI_BE As New OrdenPreOrdenInversionBE
        Dim objOI_BM As New OrdenPreOrdenInversionBM
        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = objOI_BE.Tables(0).Rows(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha de Operacion"
        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        drGrilla("c2") = "Fecha de Vencimiento"
        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = oRow.HoraOperacion
        drGrilla("c4") = "Monto Nominal Ordenado"
        drGrilla("v4") = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
        drGrilla("c5") = "Monto Nominal Operación"
        drGrilla("v5") = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
        drGrilla("c6") = "Tipo Tasa"
        drGrilla("v6") = "EFECTIVA"
        drGrilla("c7") = "YTM %"
        drGrilla("v7") = Format(oRow.YTM, "##,##0.00")
        drGrilla("c8") = "Precio Negociación %"
        drGrilla("v8") = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000")
        drGrilla("c9") = "Precio Calculado"
        drGrilla("v9") = Format(oRow.PrecioCalculado, "##,##0.0000")
        drGrilla("c10") = "Precio Negociación Sucio"
        drGrilla("v10") = Format(oRow.PrecioNegociacionSucio, "##,##0.0000")
        drGrilla("c11") = ""
        drGrilla("v11") = ""
        drGrilla("c12") = ""
        drGrilla("v12") = ""
        drGrilla("c13") = "Monto Operación"
        drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.00")
        drGrilla("c14") = "Número Papeles"
        drGrilla("v14") = Format(oRow.CantidadOperacion, "##,##0.0000000")
        drGrilla("c15") = "Intermediario"
        drGrilla("v15") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
        If oRow.CodigoContacto <> "" Then
            drGrilla("c16") = "Contacto"
            drGrilla("v16") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
        Else
            drGrilla("c16") = ""
            drGrilla("v16") = ""
        End If
        If oRow.NumeroPoliza <> "" Then
            drGrilla("c17") = "Número Poliza"
            drGrilla("v17") = oRow.NumeroPoliza
        Else
            drGrilla("c17") = ""
            drGrilla("v17") = ""
        End If
        drGrilla("c18") = "Observación"
        drGrilla("v18") = oRow.Observacion
        drGrilla("c19") = ""
        drGrilla("v19") = ""
        drGrilla("c20") = ""
        drGrilla("v20") = ""
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Shared Function ObtenerDatosOperacion_CertificadoSuscripcion(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
        Dim objOI_BE As New OrdenPreOrdenInversionBE
        Dim objOI_BM As New OrdenPreOrdenInversionBM
        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = objOI_BE.Tables(0).Rows(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operacion"
        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = oRow.HoraOperacion
        drGrilla("c4") = "Número Papeles Ordenadas"
        drGrilla("v4") = Format(oRow.CantidadOrdenado, "##,##0.0000000")
        drGrilla("c5") = "Número Papeles Operación"
        drGrilla("v5") = Format(oRow.CantidadOperacion, "##,##0.0000000")
        drGrilla("c6") = "Precio"
        drGrilla("v6") = Format(oRow.Precio, "##,##0.0000000")
        drGrilla("c7") = "Monto Operación"
        drGrilla("v7") = Format(oRow.MontoOperacion, "##,##0.0000000")
        drGrilla("c8") = "Intermediario"
        drGrilla("v8") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
        If oRow.CodigoContacto <> "" Then
            drGrilla("c9") = "Contacto"
            drGrilla("v9") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
        Else
            drGrilla("c9") = ""
            drGrilla("v9") = ""
        End If
        drGrilla("c10") = "Observación"
        drGrilla("v10") = oRow.Observacion
        If oRow.NumeroPoliza <> "" Then
            drGrilla("c11") = "Número Poliza"
            drGrilla("v11") = oRow.NumeroPoliza
        Else
            drGrilla("c11") = ""
            drGrilla("v11") = ""
        End If
        drGrilla("c12") = ""
        drGrilla("v12") = ""
        drGrilla("c13") = ""
        drGrilla("v13") = ""
        drGrilla("c14") = ""
        drGrilla("v14") = ""
        drGrilla("c15") = ""
        drGrilla("v15") = ""
        drGrilla("c16") = ""
        drGrilla("v16") = ""
        drGrilla("c17") = ""
        drGrilla("v17") = ""
        drGrilla("c18") = ""
        drGrilla("v18") = ""
        drGrilla("c19") = "Total Comisiones"
        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
        drGrilla("c20") = "Monto Neto Operación"
        drGrilla("v20") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
        drGrilla("c21") = "Precio Promedio"
        drGrilla("v21") = Format(oRow.PrecioPromedio, "##,##0.0000000")
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Shared Function ObtenerDatosOperacion_PapelesComerciales(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
        Dim objOI_BE As New OrdenPreOrdenInversionBE
        Dim objOI_BM As New OrdenPreOrdenInversionBM
        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = objOI_BE.Tables(0).Rows(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = oRow.HoraOperacion
        drGrilla("c4") = "Monto Nominal Ordenado"
        drGrilla("v4") = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
        drGrilla("c5") = "Monto Nominal Operación"
        drGrilla("v5") = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
        drGrilla("c6") = "Tipo Tasa"
        drGrilla("v6") = IIf(oRow.CodigoTipoCupon = "1", "NOMINAL", "EFECTIVA")
        drGrilla("c7") = "YTM%"
        drGrilla("v7") = Format(oRow.YTM, "##,##0.00")
        drGrilla("c8") = "Precio Negociación %"
        drGrilla("v8") = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000000")
        drGrilla("c9") = "Precio Calculado %"
        drGrilla("v9") = Format(oRow.PrecioCalculado, "##,##0.0000000")
        drGrilla("c10") = "Precio Negociación Sucio"
        drGrilla("v10") = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
        drGrilla("c11") = "Interés Corrido Negociado"
        drGrilla("v11") = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
        drGrilla("c12") = "Interés Corrido"
        drGrilla("v12") = Format(oRow.InteresCorrido, "##,##0.0000000")
        drGrilla("c13") = "Monto Operación"
        drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.0000000")
        drGrilla("c14") = "Número Papeles"
        drGrilla("v14") = Format(oRow.CantidadOperacion, "##,##0.0000000")
        drGrilla("c15") = "Intermediario"
        drGrilla("v15") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
        If oRow.CodigoContacto <> "" Then
            drGrilla("c16") = "Contacto"
            drGrilla("v16") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
        Else
            drGrilla("c16") = ""
            drGrilla("v16") = ""
        End If
        If oRow.NumeroPoliza <> "" Then
            drGrilla("c17") = "Número Poliza"
            drGrilla("v17") = oRow.NumeroPoliza
        Else
            drGrilla("c17") = ""
            drGrilla("v17") = ""
        End If
        drGrilla("c18") = "Observación"
        drGrilla("v18") = oRow.Observacion
        drGrilla("c19") = "Total Comisiones"
        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
        drGrilla("c20") = "Monto Neto Operación"
        drGrilla("v20") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Shared Function ObtenerDatosOperacion_Pagares(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
        Dim objOI_BE As New OrdenPreOrdenInversionBE
        Dim objOI_BM As New OrdenPreOrdenInversionBM
        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = objOI_BE.Tables(0).Rows(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = oRow.HoraOperacion
        drGrilla("c4") = "Monto Nominal Ordenado"
        drGrilla("v4") = Format(oRow.MontoNominalOrdenado, "##,##0.0000000")
        drGrilla("c5") = "Monto Nominal Operación"
        drGrilla("v5") = Format(oRow.MontoNominalOperacion, "##,##0.0000000")
        drGrilla("c6") = "Tipo Tasa"
        drGrilla("v6") = IIf(oRow.CodigoTipoCupon = "1", "NOMINAL", "EFECTIVA")
        drGrilla("c7") = "YTM%"
        drGrilla("v7") = Format(oRow.YTM, "##,##0.0000000")
        drGrilla("c8") = "Precio Negociación %"
        drGrilla("v8") = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000000")
        drGrilla("c9") = "Precio Calculado %"
        drGrilla("v9") = Format(oRow.PrecioCalculado, "##,##0.0000000")
        drGrilla("c10") = "Precio Negociación Sucio"
        drGrilla("v10") = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
        drGrilla("c11") = "Interés Corrido Negociado"
        drGrilla("v11") = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
        drGrilla("c12") = "Interés Corrido"
        drGrilla("v12") = Format(oRow.InteresCorrido, "##,##0.0000000")
        drGrilla("c13") = "Monto Operación"
        drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.00")
        drGrilla("c14") = "Número Papeles"
        drGrilla("v14") = Format(oRow.CantidadOperacion, "##,##0.0000000")
        drGrilla("c15") = "Intermediario"
        drGrilla("v15") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
        If oRow.CodigoContacto <> "" Then
            drGrilla("c16") = "Contacto"
            drGrilla("v16") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
        Else
            drGrilla("c16") = ""
            drGrilla("v16") = ""
        End If
        If oRow.NumeroPoliza <> "" Then
            drGrilla("c17") = "Número Poliza"
            drGrilla("v17") = oRow.NumeroPoliza
        Else
            drGrilla("c17") = ""
            drGrilla("v17") = ""
        End If
        drGrilla("c18") = "Observación"
        drGrilla("v18") = oRow.Observacion
        drGrilla("c19") = "Total Comisiones"
        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
        drGrilla("c20") = "Total Operación"
        drGrilla("v20") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Shared Function ObtenerDatosOperacion_OrdenesFondo(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
        Dim objOI_BE As New OrdenPreOrdenInversionBE
        Dim objOI_BM As New OrdenPreOrdenInversionBM
        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = objOI_BE.Tables(0).Rows(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = oRow.HoraOperacion
        drGrilla("c4") = "Fecha Trato"
        drGrilla("v4") = UIUtility.ConvertirFechaaString(oRow.FechaTrato)
        drGrilla("c5") = "Número Cuotas Ordenado"
        drGrilla("v5") = Format(oRow.CantidadOrdenado, "##,##0.0000000")
        drGrilla("c6") = "Número Cuotas Operación"
        drGrilla("v6") = Format(oRow.CantidadOperacion, "##,##0.0000000")
        drGrilla("c7") = "Monto Operación"
        drGrilla("v7") = Format(oRow.MontoOperacion, "##,##0.0000000")
        drGrilla("c8") = "Precio"
        drGrilla("v8") = Format(oRow.Precio, "##,##0.0000000")
        drGrilla("c9") = "Intermediario"
        drGrilla("v9") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
        If oRow.CodigoContacto <> "" Then
            drGrilla("c10") = "Contacto"
            drGrilla("v10") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
        Else
            drGrilla("c10") = ""
            drGrilla("v10") = ""
        End If
        drGrilla("c11") = "Observación"
        drGrilla("v11") = oRow.Observacion
        If oRow.NumeroPoliza <> "" Then
            drGrilla("c12") = "Número Poliza"
            drGrilla("v12") = oRow.NumeroPoliza
        Else
            drGrilla("c12") = ""
            drGrilla("v12") = ""
        End If
        drGrilla("c13") = ""
        drGrilla("v13") = ""
        drGrilla("c14") = ""
        drGrilla("v14") = ""
        drGrilla("c15") = ""
        drGrilla("v15") = ""
        drGrilla("c16") = ""
        drGrilla("v16") = ""
        drGrilla("c17") = ""
        drGrilla("v17") = ""
        drGrilla("c18") = ""
        drGrilla("v18") = ""
        drGrilla("c19") = "Total Comisiones"
        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
        drGrilla("c20") = "Monto Neto Operación"
        drGrilla("v20") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
        drGrilla("c21") = "Precio Promedio"
        drGrilla("v21") = Format(oRow.PrecioPromedio, "##,##0.0000000")
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Shared Function ObtenerDatosOperacion_OperacionesReporte(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
        Dim objOI_BE As New OrdenPreOrdenInversionBE
        Dim objOI_BM As New OrdenPreOrdenInversionBM
        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = objOI_BE.Tables(0).Rows(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Numero Bonos"
        drGrilla("v1") = oRow.CantidadOperacion
        drGrilla("c2") = "Valor Nominal"
        drGrilla("v2") = oRow.MontoNominalOperacion
        drGrilla("c3") = "Precio de Inicio %"
        drGrilla("v3") = oRow.Precio
        drGrilla("c4") = "Fecha Inicio"
        drGrilla("v4") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        drGrilla("c5") = "Plazo Reporte"
        drGrilla("v5") = oRow.Plazo
        drGrilla("c6") = "Tasa Anual %"
        drGrilla("v6") = oRow.TasaPorcentaje
        drGrilla("c7") = "Nemonico"
        drGrilla("v7") = oRow.CodigoMnemonico
        drGrilla("c8") = "Interes Corrido Contado"
        drGrilla("v8") = oRow.InteresCorrido
        drGrilla("c9") = "Monto sin Interes"
        drGrilla("v9") = oRow.MontoNominalOrdenado
        drGrilla("c10") = "Precio Plazo"
        drGrilla("v10") = oRow.PrecioCalculado
        drGrilla("c11") = "Dia a Plazo"
        drGrilla("v11") = oRow.FechaContrato
        drGrilla("c12") = "Tipo Cambio"
        drGrilla("v12") = oRow.TipoCambio
        drGrilla("c13") = "Interes Corrido Plazo"
        drGrilla("v13") = oRow.InteresCorridoNegociacion
        drGrilla("c14") = "Monto a Plazo sin interes"
        drGrilla("v14") = oRow.MontoPlazo
        drGrilla("c15") = "Monto a Plazo mas interes"
        drGrilla("v15") = oRow.MontoCancelar
        drGrilla("c16") = ""
        drGrilla("v16") = ""
        drGrilla("c17") = ""
        drGrilla("v17") = ""
        drGrilla("c18") = ""
        drGrilla("v18") = ""
        drGrilla("c19") = ""
        drGrilla("v19") = ""
        drGrilla("c20") = ""
        drGrilla("v20") = ""
        drGrilla("c21") = ""
        drGrilla("v21") = ""
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Shared Function ObtenerDatosOperacion_LetrasHipotecarias(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
        Dim objOI_BE As New OrdenPreOrdenInversionBE
        Dim objOI_BM As New OrdenPreOrdenInversionBM
        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = objOI_BE.Tables(0).Rows(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = oRow.HoraOperacion
        drGrilla("c4") = "Monto Nominal Ordenado"
        drGrilla("v4") = oRow.MontoNominalOrdenado
        drGrilla("c5") = "Monto Nominal Operación"
        drGrilla("v5") = oRow.MontoNominalOperacion
        drGrilla("c6") = "YTM %"
        drGrilla("v6") = Format(oRow.YTM, "##,##0.0000000")
        drGrilla("c7") = "Tipo Tasa"
        drGrilla("v7") = IIf(oRow.CodigoTipoCupon = "1", "NOMINAL", "EFECTIVA")
        drGrilla("c8") = "Precio Calculado"
        drGrilla("v8") = Format(oRow.PrecioCalculado, "##,##0.0000000")
        drGrilla("c9") = "Precio Negociación Limpio"
        drGrilla("v9") = Format(oRow.PrecioNegociacionLimpio, "##,##0.0000000")
        drGrilla("c10") = "Interés Corrido"
        drGrilla("v10") = Format(oRow.InteresCorrido, "##,##0.0000000")
        drGrilla("c11") = "Interés Corrido Negociación"
        drGrilla("v11") = Format(oRow.InteresCorridoNegociacion, "##,##0.0000000")
        drGrilla("c12") = "Precio Negociación Sucio"
        drGrilla("v12") = Format(oRow.PrecioNegociacionSucio, "##,##0.0000000")
        drGrilla("c13") = "Monto Operación"
        drGrilla("v13") = Format(oRow.MontoOperacion, "##,##0.0000000")
        drGrilla("c14") = "Número Papeles"
        drGrilla("v14") = Format(oRow.CantidadOperacion, "##,##0.0000000")
        drGrilla("c15") = "Intermediario"
        drGrilla("v15") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
        If oRow.CodigoContacto <> "" Then
            drGrilla("c16") = "Contacto"
            drGrilla("v16") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
        Else
            drGrilla("c16") = ""
            drGrilla("v16") = ""
        End If
        drGrilla("c17") = "Observación"
        drGrilla("v17") = oRow.Observacion
        If oRow.NumeroPoliza <> "" Then
            drGrilla("c18") = "Número Poliza"
            drGrilla("v18") = oRow.NumeroPoliza
        Else
            drGrilla("c18") = ""
            drGrilla("v18") = ""
        End If
        drGrilla("c19") = "Total Comisiones"
        drGrilla("v19") = Format(oRow.TotalComisiones, "##,##0.0000000")
        drGrilla("c20") = "Total Operación"
        drGrilla("v20") = Format(oRow.MontoNetoOperacion, "##,##0.0000000")
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Shared Function ObtenerDatosOperacion_InstrumentosEstructurados(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
        Dim objOI_BE As New OrdenPreOrdenInversionBE
        Dim objOI_BM As New OrdenPreOrdenInversionBM
        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = objOI_BE.Tables(0).Rows(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        drGrilla("c2") = "Fecha Vencimiento"
        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = oRow.HoraOperacion
        drGrilla("c4") = "Unidades Ordenadas"
        drGrilla("v4") = Format(oRow.CantidadOrdenado, "##,##0.0000000")
        drGrilla("c5") = "Unidades Operación"
        drGrilla("v5") = Format(oRow.CantidadOperacion, "##,##0.0000000")
        drGrilla("c6") = "Precio"
        drGrilla("v6") = Format(oRow.Precio, "##,##0.0000000")
        drGrilla("c7") = "Monto Operación"
        drGrilla("v7") = Format(oRow.MontoOperacion, "##,##0.0000000")
        drGrilla("c8") = "Intermediario"
        drGrilla("v8") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
        If oRow.CodigoContacto <> "" Then
            drGrilla("c9") = "Contacto"
            drGrilla("v9") = ObtenerDescripcionContacto(oRow.CodigoContacto, datosRequest)
        Else
            drGrilla("c9") = ""
            drGrilla("v9") = ""
        End If
        drGrilla("c10") = "Observación"
        drGrilla("v10") = oRow.Observacion
        If oRow.NumeroPoliza <> "" Then
            drGrilla("c11") = "Número Poliza"
            drGrilla("v11") = oRow.NumeroPoliza
        Else
            drGrilla("c11") = ""
            drGrilla("v11") = ""
        End If
        drGrilla("c12") = ""
        drGrilla("v12") = ""
        drGrilla("c13") = ""
        drGrilla("v13") = ""
        drGrilla("c14") = ""
        drGrilla("v14") = ""
        drGrilla("c15") = ""
        drGrilla("v15") = ""
        drGrilla("c16") = ""
        drGrilla("v16") = ""
        drGrilla("c17") = ""
        drGrilla("v17") = ""
        drGrilla("c18") = ""
        drGrilla("v18") = ""
        drGrilla("c19") = "Total Comisiones"
        drGrilla("c20") = "Monto Neto Operación"
        drGrilla("c21") = "Precio Promedio"
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Shared Function ObtenerDatosOperacion_ForwardyDivisas(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
        Dim objOI_BE As New OrdenPreOrdenInversionBE
        Dim objOI_BM As New OrdenPreOrdenInversionBM
        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = objOI_BE.Tables(0).Rows(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21", "c22", "v22"}   'HDG OT 62325 20110323
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        drGrilla("c2") = "Fecha Contrato"
        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaContrato)
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = oRow.HoraOperacion
        drGrilla("c4") = "Fecha Liquidación"
        drGrilla("v4") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        drGrilla("c5") = "Tipo Cambio Spot"
        drGrilla("v5") = Format(oRow.TipoCambioSpot, "##,##0.0000000")
        drGrilla("c6") = "Tipo Cambio Futuro"
        drGrilla("v6") = Format(oRow.TipoCambioFuturo, "##,##0.0000000")
        drGrilla("c7") = "De"
        drGrilla("v7") = ObtenerDescripcionMoneda(CType(oRow.CodigoMonedaOrigen, String), datosRequest)
        drGrilla("c8") = "Monto Origen"
        drGrilla("v8") = Format(oRow.MontoOrigen, "##,##0.0000000")
        drGrilla("c9") = "A"
        drGrilla("v9") = ObtenerDescripcionMoneda(CType(oRow.CodigoMonedaDestino, String), datosRequest)
        drGrilla("c10") = "Monto Futuro"
        drGrilla("v10") = Format(oRow.MontoOperacion, "##,##0.0000000")
        drGrilla("c11") = "Plazo"
        drGrilla("v11") = oRow.Plazo
        drGrilla("c12") = "Diferencial"
        drGrilla("v12") = Format(oRow.Diferencial, "##,##0.0000000")
        drGrilla("c13") = "Modalidad Compra"
        If oRow.Delibery <> "" Then
            Select Case oRow.Delibery
                Case "N"
                    drGrilla("v13") = "Non - Delivery"
                Case "S"
                    drGrilla("v13") = "Delivery"
            End Select
        Else
            drGrilla("v13") = ""
        End If
        drGrilla("c14") = "Intermediario"
        drGrilla("v14") = ObtenerDescripcionTercero(oRow.CodigoTercero, datosRequest)
        drGrilla("c16") = "Observacion"
        If oRow.Observacion <> "" Then
            drGrilla("v16") = oRow.Observacion
        Else
            drGrilla("v16") = ""
        End If
        drGrilla("c17") = ""
        drGrilla("v17") = ""
        drGrilla("c18") = "Motivo"
        drGrilla("v18") = ObtenerDescripcionMotivo(oRow.CodigoMotivo, datosRequest)
        drGrilla("c19") = ""
        drGrilla("v19") = ""
        drGrilla("c20") = ""
        drGrilla("v20") = ""
        drGrilla("c21") = ""
        drGrilla("v21") = ""
        drGrilla("c22") = "Cobertura"
        drGrilla("v22") = objOI_BM.SeleccionarTipoMonedaxMotivoForw(oRow.CodigoMotivo, oRow.TipoMonedaForw).Rows(0)("Descripcion")   'HDG OT 62325 20110323
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
    Public Shared Function ObtenerDatosOperacion_CompraVentaMonedaExtranjera(ByVal codigoOrden As String, ByVal fondo As String, ByVal datosRequest As DataSet) As DataTable
        Dim objOI_BE As New OrdenPreOrdenInversionBE
        Dim objOI_BM As New OrdenPreOrdenInversionBM
        objOI_BE = objOI_BM.ListarOrdenesInversionPorCodigoOrden(codigoOrden, fondo, datosRequest, PORTAFOLIO_MULTIFONDOS)
        Dim oRow As OrdenPreOrdenInversionBE.OrdenPreOrdenInversionRow
        oRow = objOI_BE.Tables(0).Rows(0)
        Dim drGrilla As DataRow
        Dim dtGrilla As New DataTable
        Dim blnEsImpar As Boolean = False
        Dim strTabla As String() = {"c1", "v1", "c2", "v2", "c3", "v3", "c4", "v4", "c5", "v5", "c6", "v6", "c7", "v7", "c8", "v8", "c9", "v9", "c10", "v10", "c11", "v11", "c12", "v12", "c13", "v13", "c14", "v14", "c15", "v15", "c16", "v16", "c17", "v17", "c18", "v18", "c19", "v19", "c20", "v20", "c21", "v21"}
        dtGrilla = UIUtility.GetStructureTablebase(strTabla)
        drGrilla = dtGrilla.NewRow
        drGrilla("c1") = "Fecha Operación"
        drGrilla("v1") = UIUtility.ConvertirFechaaString(oRow.FechaOperacion)
        drGrilla("c2") = "Fecha Liquidación"
        drGrilla("v2") = UIUtility.ConvertirFechaaString(oRow.FechaLiquidacion)
        drGrilla("c3") = "Hora Operación"
        drGrilla("v3") = oRow.HoraOperacion
        drGrilla("c4") = "De"
        drGrilla("v4") = ObtenerDescripcionMoneda(CType(oRow.CodigoMoneda, String), datosRequest)
        drGrilla("c5") = "Monto Divisa Negociada"
        drGrilla("v5") = Format(oRow.MontoOperacion, "##,##0.0000000")
        drGrilla("c6") = "A"
        drGrilla("v6") = ObtenerDescripcionMoneda(CType(oRow.CodigoMonedaDestino, String), datosRequest)
        drGrilla("c7") = "Monto"
        drGrilla("v7") = Format(oRow.MontoDestino, "##,##0.0000000")
        drGrilla("c8") = "Tipo Cambio"
        drGrilla("v8") = Format(oRow.TipoCambio, "##,##0.0000000")
        drGrilla("c9") = "Intermediario"
        drGrilla("v9") = ObtenerDescripcionTercero(CType(oRow.CodigoTercero, String), datosRequest)
        drGrilla("c10") = "Contacto"
        drGrilla("v10") = ObtenerDescripcionContacto(CType(oRow.CodigoContacto, String), datosRequest)
        drGrilla("c11") = ""
        drGrilla("v11") = ""
        drGrilla("c12") = "Observación"
        drGrilla("v12") = oRow.Observacion
        drGrilla("c13") = ""
        drGrilla("v13") = ""
        drGrilla("c14") = ""
        drGrilla("v14") = ""
        drGrilla("c16") = ""
        drGrilla("v16") = ""
        drGrilla("c17") = ""
        drGrilla("v17") = ""
        drGrilla("c18") = ""
        drGrilla("v18") = ""
        drGrilla("c19") = ""
        drGrilla("v19") = ""
        drGrilla("c20") = ""
        drGrilla("v20") = ""
        drGrilla("c21") = ""
        drGrilla("v21") = ""
        dtGrilla.Rows.Add(drGrilla)
        Return dtGrilla
    End Function
#End Region
#Region "/* PDF (Vol. 12) */"
    Public Shared Sub CrearArchivoPDF(ByVal TablaText As DataTable, ByVal HtmlPath As String, ByVal PdfPath As String)
        Try
            Dim document As New Document(PageSize.A4, 55, 55, 20, 6)
            Dim oStreamReader As New StreamReader(HtmlPath, System.Text.Encoding.Default)
            Dim styles As New iTextSharp.text.html.simpleparser.StyleSheet
            Dim hw As New iTextSharp.text.html.simpleparser.HTMLWorker(document)
            Dim oIElement As IElement
            Dim oParagraph As Paragraph
            Dim oPdfPTable As PdfPTable
            Dim oPdfPCell As PdfPCell
            Dim objects As List(Of iTextSharp.text.IElement)
            Dim strContent As String
            PdfWriter.GetInstance(document, New FileStream(PdfPath, FileMode.Create))
            document.Open()
            document.NewPage()
            objects = hw.ParseToList(oStreamReader, styles)
            For k As Integer = 0 To objects.Count - 1
                oIElement = CType(objects(k), IElement)
                If objects(k).GetType().FullName = "iTextSharp.text.Paragraph" Then
                    oParagraph = New Paragraph
                    oParagraph.Alignment = CType(objects(k), Paragraph).Alignment
                    For z As Integer = 0 To oIElement.Chunks.Count - 1
                        strContent = ReplaceText(oIElement.Chunks(z).Content, TablaText)
                        oParagraph.Add(New Chunk(strContent, oIElement.Chunks(z).Font))
                        oParagraph.Leading = 11
                    Next
                    document.Add(oParagraph)
                ElseIf objects(k).GetType().FullName = "iTextSharp.text.pdf.PdfPTable" Then
                    oPdfPTable = CType(objects(k), PdfPTable)
                    Dim oNewPdfPTable As PdfPTable = New PdfPTable(oPdfPTable.NumberOfColumns)
                    Dim DimensionColumna(oPdfPTable.NumberOfColumns - 1) As Integer
                    Dim aux As Integer
                    'oNewPdfPTable.WidthPercentage = 100
                    Dim imgFirma1 As String = ""
                    Dim imgFirma2 As String = ""
                    Dim jpg As iTextSharp.text.Image
                    For row As Integer = 0 To oPdfPTable.Rows.Count - 1
                        For cell As Integer = 0 To oPdfPTable.Rows(row).GetCells().Length - 1
                            oPdfPCell = oPdfPTable.Rows(row).GetCells()(cell)
                            oParagraph = New Paragraph
                            For paragraph As Integer = 0 To oPdfPTable.Rows(row).GetCells()(cell).CompositeElements.Count - 1
                                For chunk As Integer = 0 To oPdfPTable.Rows(row).GetCells()(cell).CompositeElements(paragraph).Chunks.Count - 1
                                    If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Entidad]" Or _
                                        oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Direccion]" Or _
                                        oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Url]") Then
                                        strContent = ""
                                        If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Entidad]") Then
                                            strContent = "Fondos Sura"
                                        End If
                                        If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Direccion]") Then
                                            strContent = "Canaval y Moreyra 522 San Isidro. Teléfono: 411-9191. Fax: 411-9192"
                                        End If
                                        If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Url]") Then
                                            strContent = "www.sura.pe"
                                        End If
                                        oParagraph.Add(New Chunk(strContent, oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Font))
                                        aux = Len(strContent)
                                        If aux > DimensionColumna(cell) Then
                                            DimensionColumna(cell) = aux
                                        End If
                                        If (oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[s_Url]") Then
                                            oParagraph.Leading = 10
                                        Else
                                            oParagraph.Leading = 6
                                        End If
                                    ElseIf oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content <> "[Firma1]" And _
                                    oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content <> "[Firma2]" And _
                                    oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content <> "[Logo]" Then
                                        strContent = ReplaceText(oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content, TablaText)
                                        oParagraph.Add(New Chunk(strContent, oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Font))
                                        aux = Len(strContent)
                                        If aux > DimensionColumna(cell) Then
                                            DimensionColumna(cell) = 1 'aux
                                        End If
                                        oParagraph.Leading = 11
                                    Else
                                        If oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[Firma1]" Then
                                            For Each dr As DataRow In TablaText.Rows
                                                If dr("Find") = "[Firma1]" Then
                                                    imgFirma1 = CType(dr("New"), String)
                                                End If
                                            Next
                                            If imgFirma1 <> "" Then
                                                jpg = iTextSharp.text.Image.GetInstance(imgFirma1)
                                                jpg.ScaleToFit(120.0F, 50.0F)
                                                jpg.SpacingBefore = 5.0F
                                                jpg.SpacingAfter = 1.0F
                                                jpg.Alignment = Element.ALIGN_LEFT
                                            End If
                                        End If
                                        If oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[Firma2]" Then
                                            For Each dr As DataRow In TablaText.Rows
                                                If dr("Find") = "[Firma2]" Then
                                                    imgFirma2 = CType(dr("New"), String)
                                                End If
                                            Next
                                            If imgFirma2 <> "" Then
                                                jpg = iTextSharp.text.Image.GetInstance(imgFirma2)
                                                jpg.ScaleToFit(120.0F, 50.0F)
                                                jpg.SpacingBefore = 5.0F
                                                jpg.SpacingAfter = 1.0F
                                                jpg.Alignment = Element.ALIGN_LEFT
                                            End If
                                        End If
                                        If oPdfPCell.CompositeElements(paragraph).Chunks(chunk).Content = "[Logo]" Then
                                            For Each dr As DataRow In TablaText.Rows
                                                If dr("Find") = "[Logo]" Then
                                                    imgFirma2 = New BasePage().Ruta_Logo()
                                                End If
                                            Next
                                            If imgFirma2 <> "" Then
                                                jpg = iTextSharp.text.Image.GetInstance(imgFirma2)
                                                jpg.ScaleToFit(127.0F, 40.0F)
                                                jpg.SpacingBefore = 5.0F
                                                jpg.SpacingAfter = 1.0F
                                                jpg.Alignment = Element.ALIGN_RIGHT
                                            End If
                                        End If
                                    End If
                                Next
                                aux = 0
                            Next
                            oPdfPCell.CompositeElements.Clear()
                            oPdfPCell.AddElement(oParagraph)
                            If Not jpg Is Nothing Then
                                oPdfPCell.AddElement(jpg)
                                jpg = Nothing
                            End If
                            oNewPdfPTable.AddCell(oPdfPCell)
                        Next
                    Next
                    oNewPdfPTable.SetWidths(CalcularDimensiones(DimensionColumna))
                    document.Add(oNewPdfPTable)
                End If
            Next
            document.Close()
            oStreamReader.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Shared Function CalcularDimensiones(ByVal Columnas() As Integer) As Integer()
        Dim total As Integer
        Dim i As Integer
        For i = 0 To Columnas.Length - 1
            If Columnas(i) = 0 Then
                Columnas(i) = 10
            End If
            total = total + Columnas(i)
        Next
        For i = 0 To Columnas.Length - 1
            Columnas(i) = (Columnas(i) / total) * 100
        Next
        Return Columnas
    End Function
    Shared Function ReplaceText(ByVal TextBase As String, ByVal TableText As DataTable) As String
        Try
            For x As Integer = 0 To TableText.Rows.Count - 1
                If TextBase.IndexOf("[") = -1 And TextBase.IndexOf("]") = -1 Then Exit For
                If TextBase.IndexOf(TableText.Rows(x)("Find")) > -1 Then TextBase = TextBase.Replace(TableText.Rows(x)("Find"), TableText.Rows(x)("New"))
            Next
            Return TextBase
        Catch ex As Exception
            Return ""
        End Try
    End Function
#End Region
#Region "Publicar Msj Accesible (CarlosRumiche)"
    ''' <summary>
    ''' Publica un mensaje en el EventLog del equipo en el cual se ejecuta las aplicación. 
    ''' Ademas publica el mensaje en el archivo (\\plutonvm\archivosplanos\Log\LogEventosSIT.xml), 
    ''' o en el archivo definido por en el Web.Config en la clave AppSettings("LogXml_Ruta")
    ''' </summary>
    Public Shared Sub PublicarEvento(ByVal msj As String)
        LogXml.PublicarMensaje("P " & msj)
    End Sub
    Public Class LogXml
        Public Shared Sub PublicarException(ByVal Excepcion As Exception)
            Try
                Dim pathArchivoXml As String = RUTA_LOG
                If Not ConfigurationSettings.AppSettings("LogXml_Ruta") Is Nothing Then
                    If ConfigurationSettings.AppSettings("LogXml_Ruta").Trim.Length > 0 Then
                        pathArchivoXml = ConfigurationSettings.AppSettings("LogXml_Ruta")
                    End If
                End If
                Dim documentoXml As New XmlDocument
                Dim nodoXmlPadre As System.Xml.XmlNode
                If File.Exists(pathArchivoXml) Then
                    documentoXml.Load(pathArchivoXml)
                    nodoXmlPadre = documentoXml.Item("ListaExcepciones")
                Else
                    documentoXml.AppendChild(documentoXml.CreateXmlDeclaration("1.0", "utf-8", String.Empty))
                    nodoXmlPadre = documentoXml.AppendChild(documentoXml.CreateElement("ListaExcepciones"))
                End If
                Dim nodoXmlExcepcion As System.Xml.XmlNode = nodoXmlPadre.AppendChild(documentoXml.CreateElement("Excepcion"))
                Dim nodoXmlFecha As System.Xml.XmlNode = nodoXmlExcepcion.AppendChild(documentoXml.CreateElement("Fecha"))
                Dim nodoXmlMensaje As System.Xml.XmlNode = nodoXmlExcepcion.AppendChild(documentoXml.CreateElement("MensajeError"))
                nodoXmlFecha.InnerText = Now.ToString("yyyy-MM-dd hh:mm:ss tt")
                nodoXmlMensaje.InnerText = Excepcion.Message
                documentoXml.Save(pathArchivoXml)
            Catch ex As Exception
            End Try
        End Sub
        Public Shared Sub PublicarMensaje(ByVal mensaje As String)
            Try
                Dim pathArchivoXml As String = RUTA_LOG
                If Not ConfigurationSettings.AppSettings("LogXml_Ruta") Is Nothing Then
                    If ConfigurationSettings.AppSettings("LogXml_Ruta").Trim.Length > 0 Then
                        pathArchivoXml = ConfigurationSettings.AppSettings("LogXml_Ruta")
                    End If
                End If
                Dim documentoXml As New XmlDocument
                Dim nodoXmlPadre As System.Xml.XmlNode
                If File.Exists(pathArchivoXml) Then
                    documentoXml.Load(pathArchivoXml)
                    nodoXmlPadre = documentoXml.Item("ListaExcepciones")
                Else
                    documentoXml.AppendChild(documentoXml.CreateXmlDeclaration("1.0", "utf-8", String.Empty))
                    nodoXmlPadre = documentoXml.AppendChild(documentoXml.CreateElement("ListaExcepciones"))
                End If
                Dim nodoXmlExcepcion As System.Xml.XmlNode = nodoXmlPadre.AppendChild(documentoXml.CreateElement("Excepcion"))
                Dim nodoXmlFecha As System.Xml.XmlNode = nodoXmlExcepcion.AppendChild(documentoXml.CreateElement("Fecha"))
                Dim nodoXmlMensaje As System.Xml.XmlNode = nodoXmlExcepcion.AppendChild(documentoXml.CreateElement("Mensaje"))
                nodoXmlFecha.InnerText = Now.ToString("yyyyMMdd hh:mm:ss tt")
                nodoXmlMensaje.InnerText = mensaje
                documentoXml.Save(pathArchivoXml)
            Catch ex As Exception
            End Try
        End Sub
    End Class
#End Region
    Public Shared Function GeneraTablaResumenTotalInstrumentos(ByVal strFechaInicio As String, ByVal strFechaFin As String, ByVal Portafolio As String, ByVal DatosRequest As DataSet) As DataTable
        Dim dtFinal As New DataTable
        dtFinal.Columns.Add("TipoInstrumento", GetType(String))
        dtFinal.Columns.Add("CodigoNemonico", GetType(String))
        dtFinal.Columns.Add("Condicion", GetType(String))
        dtFinal.Columns.Add("UtilidadTotal", GetType(Double))
        dtFinal.Columns.Add("UtilidadEncaje", GetType(Double))
        dtFinal.Columns.Add("UtilidadVentaTotal", GetType(Double))
        dtFinal.Columns.Add("UtilidadEncajeVenta", GetType(Double))
        dtFinal.Columns.Add("MetodoCalculoRenta", GetType(String))
        dtFinal.Columns.Add("ValorVenta", GetType(String))
        dtFinal.Columns.Add("CostoValorVendido", GetType(String))
        dtFinal.Columns.Add("TotalInteresDividendo", GetType(String))
        dtFinal.Columns.Add("TotalInteres", GetType(Double))
        dtFinal.Columns.Add("TotalDividendo", GetType(Double))
        Dim FechaInicio As Decimal = ConvertirFechaaDecimal(strFechaInicio)
        Dim FechaFin As Decimal = ConvertirFechaaDecimal(strFechaFin)
        Dim dtConsulta As DataTable = New ReporteGestionBM().ReporteResumenRentabilidadEncajeTotalInstrumentos(FechaInicio, FechaFin, Portafolio, DatosRequest).Tables(0)
        Dim dtDetalle As DataTable
        Dim drInstrumento As DataRow
        Dim drFinal As DataRow
        Dim CodigoNemonico As String
        Dim TotalVentas As Decimal
        Dim CostoVentas As Decimal
        For Each drInstrumento In dtConsulta.Rows
            CodigoNemonico = drInstrumento("CodigoNemonico")
            If (CodigoNemonico.Equals("BCR10S13")) Then
                Dim pararm = Nothing
            End If
            dtDetalle = GeneraTablaDetallePorInstrumento(CodigoNemonico, FechaInicio, FechaFin, Portafolio, DatosRequest)
            CostoVentas = dtDetalle.Compute("sum(ImporteKardex)", "TipoOperacion='COSTO_VENTAS'")
            TotalVentas = dtDetalle.Compute("sum(ImporteKardex)", "TipoOperacion='TOTAL_VENTAS'")
            drFinal = dtFinal.NewRow
            drFinal("TipoInstrumento") = drInstrumento("TipoInstrumento")
            drFinal("CodigoNemonico") = CodigoNemonico
            drFinal("Condicion") = drInstrumento("Condicion")
            drFinal("UtilidadTotal") = ToNullDecimal(drInstrumento("UtilidadTotal"))
            drFinal("UtilidadEncaje") = ToNullDecimal(drInstrumento("UtilidadEncaje"))
            drFinal("ValorVenta") = TotalVentas
            drFinal("CostoValorVendido") = CostoVentas
            drFinal("TotalInteresDividendo") = ToNullDecimal(drInstrumento("TotalInteresDividendo"))
            drFinal("TotalInteres") = ToNullDecimal(drInstrumento("TotalInteres"))
            drFinal("TotalDividendo") = ToNullDecimal(drInstrumento("TotalDividendo"))
            If drInstrumento("MetodoCalculoRenta") = "1" Then
                drFinal("UtilidadVentaTotal") = TotalVentas - CostoVentas
                If drInstrumento("UtilidadTotal") = 0 Then
                    drFinal("UtilidadEncajeVenta") = 0
                Else
                    drFinal("UtilidadEncajeVenta") = (TotalVentas - CostoVentas) * ToNullDecimal(drInstrumento("UtilidadEncaje")) / ToNullDecimal(drInstrumento("UtilidadTotal"))
                End If
            End If
            drFinal("MetodoCalculoRenta") = ToNullDecimal(drInstrumento("MetodoCalculoRenta"))
            dtFinal.Rows.Add(drFinal)
        Next
        Return dtFinal
    End Function
#Region "Validadciones de numeros"
    Public Shared Function ToNullDecimal(ByVal valor As Object) As Decimal
        Dim d_Numero As Decimal = 0
        Try
            d_Numero = Convert.ToDecimal(valor)
        Catch ex As Exception
            d_Numero = 0
        End Try
        Return d_Numero
    End Function
    Public Shared Function ToNullDouble(ByVal valor As Object) As Double
        Dim d_Numero As Double = 0
        Try
            d_Numero = Convert.ToDouble(valor)
        Catch ex As Exception
            d_Numero = 0
        End Try
        Return d_Numero
    End Function
    Public Shared Function ToNullInt32(ByVal valor As Object) As Integer
        Dim d_Numero As Integer = 0
        Try
            d_Numero = Convert.ToInt32(valor)
        Catch ex As Exception
            d_Numero = 0
        End Try
        Return d_Numero
    End Function
#End Region
    Public Shared Function GeneraTablaDetallePorInstrumento(ByVal CodigoNemonico As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal Portafolio As String, ByVal DatosRequest As DataSet) As DataTable
        Try
            Dim dtDetalleFinal As New DataTable
            dtDetalleFinal.Columns.Add("TipoOperacion", GetType(String))
            dtDetalleFinal.Columns.Add("Fecha", GetType(String))
            dtDetalleFinal.Columns.Add("Unidades", GetType(Decimal))
            dtDetalleFinal.Columns.Add("Precio", GetType(Decimal))
            dtDetalleFinal.Columns.Add("TipoCambio", GetType(Decimal))
            dtDetalleFinal.Columns.Add("UnidadesKardex", GetType(Decimal))
            dtDetalleFinal.Columns.Add("ImporteKardex", GetType(Decimal))
            dtDetalleFinal.Columns.Add("PrecioMO", GetType(Decimal))
            dtDetalleFinal.Columns.Add("ImporteKardexMO", GetType(Decimal))
            Dim UnidadesVendidas As Decimal = 0
            Dim UnidadesCompradas As Decimal = 0
            Dim ImporteCompras As Decimal = 0
            Dim ImporteVentas As Decimal = 0
            Dim ImporteVentas2 As Decimal = 0
            Dim ImporteInicial As Decimal = 0
            Dim PrecioPromedio As Decimal = 0
            Dim UnidadesPrimeraVenta As Decimal = 0
            Dim ImporteInicialMO As Decimal = 0
            Dim PrecioPromedioMO As Decimal = 0
            Dim ImporteComprasMO As Decimal = 0
            Dim ImporteVentasMO As Decimal = 0
            Dim ImporteVentas2MO As Decimal = 0
            Dim dsRenta As DataSet
            Dim oReporte As New EncajeDetalleBM
            dsRenta = oReporte.ReporteImpuestoRentaAnualPorNemonico(CodigoNemonico, Portafolio, FechaInicio, FechaFin, DatosRequest)
            Dim dtSaldoInicial As DataTable = dsRenta.Tables(0)
            Dim dtOperaciones As DataTable = dsRenta.Tables(1)
            Dim dtTotalVentas As DataTable = dsRenta.Tables(2)
            Dim drSaldoInicial As DataRow
            Dim drDetalleFinal As DataRow
            Dim ClaseInstrumento As String = ""
            Dim FechaPrimerDia As Decimal = PRIMER_DIA_OPERACION
            If dsRenta.Tables.Count = 4 Then
                ClaseInstrumento = dsRenta.Tables(3).Rows(0)("CodigoClaseInstrumento").ToString
            End If
            If (ClaseInstrumento = "10" Or ClaseInstrumento = "12") And FechaInicio > 20121231 And FechaFin > 20121231 Then
                FechaPrimerDia = FechaInicio
            End If
            If dtSaldoInicial.Rows.Count > 0 Then
                drSaldoInicial = dtSaldoInicial.Rows(0)
                drDetalleFinal = dtDetalleFinal.NewRow
                drDetalleFinal("TipoOperacion") = "SALDO_INICIAL"
                drDetalleFinal("Fecha") = drSaldoInicial("FechaInicial")
                Try
                    drDetalleFinal("Unidades") = ToNullDecimal(drSaldoInicial("SaldoAnt"))
                    If drDetalleFinal("Unidades").ToString.Trim = "" Then
                        drDetalleFinal("Unidades") = ToNullDecimal(drSaldoInicial("SaldoInicial"))
                    End If
                Catch ex As Exception
                    drDetalleFinal("Unidades") = ToNullDecimal(drSaldoInicial("SaldoInicial"))
                End Try
                drDetalleFinal("Precio") = ToNullDecimal(drSaldoInicial("ValorPrecioSIMO"))
                drDetalleFinal("TipoCambio") = ToNullDecimal(drSaldoInicial("TipoCambioMO"))
                ImporteInicial = ToNullDecimal(drSaldoInicial("SaldoInicial")) * ToNullDecimal(drSaldoInicial("ValorPrecio")) * ToNullDecimal(drSaldoInicial("TipoCambio")) 'RGF 20101228 OT 61609
                ImporteInicialMO = ToNullDecimal(drSaldoInicial("SaldoInicial")) * ToNullDecimal(drSaldoInicial("ValorPrecio")) 'HDG
                dtDetalleFinal.Rows.Add(drDetalleFinal)
            End If
            Dim StockAgotado As Boolean = False
            Dim drOperacion As DataRow
            Dim SaldosinCompras As Integer = 0
            For Each drOperacion In dtOperaciones.Rows
                drDetalleFinal = dtDetalleFinal.NewRow
                SaldosinCompras = SaldosinCompras + 1
                If drOperacion("CodigoOperacion") = "2" Then
                    drDetalleFinal("TipoOperacion") = "VENTA"
                    If FechaPrimerDia.ToString.Substring(0, 4) - 1 <> SALDO_ANIO_ANTERIOR Then StockAgotado = True
                    UnidadesVendidas = UnidadesVendidas + ToNullDecimal(drOperacion("Unidades"))
                    If UnidadesVendidas > ToNullDecimal(drSaldoInicial("SaldoInicial")) Or FechaPrimerDia.ToString.Substring(0, 4) - 1 <> SALDO_ANIO_ANTERIOR Then
                        If StockAgotado Then
                            If SaldosinCompras = 1 Then
                                If (ClaseInstrumento = "10" Or ClaseInstrumento = "12") And FechaInicio > 20121231 And FechaFin > 20121231 Then
                                    PrecioPromedio = drSaldoInicial("ValorPrecio") * drSaldoInicial("TipoCambio")
                                    PrecioPromedioMO = drSaldoInicial("ValorPrecio")
                                Else
                                    PrecioPromedio = drSaldoInicial("ValorPrecio")
                                    PrecioPromedioMO = drSaldoInicial("ValorPrecio")
                                End If
                            End If
                            ImporteVentas = ImporteVentas + (drOperacion("Unidades") * PrecioPromedio)
                            ImporteVentasMO = ImporteVentasMO + (drOperacion("Unidades") * PrecioPromedioMO)
                            drDetalleFinal("Fecha") = drOperacion("Fecha")
                            drDetalleFinal("Unidades") = drOperacion("Unidades")
                            drDetalleFinal("Precio") = PrecioPromedio
                            drDetalleFinal("TipoCambio") = 1
                            drDetalleFinal("PrecioMO") = PrecioPromedioMO
                            If Convert.ToDecimal(drOperacion("FechaOperacion")) >= FechaInicio Then
                                ImporteVentas2 = ImporteVentas2 + (drOperacion("Unidades") * PrecioPromedio)
                                ImporteVentas2MO = ImporteVentas2MO + (drOperacion("Unidades") * PrecioPromedioMO)
                                dtDetalleFinal.Rows.Add(drDetalleFinal)
                            End If
                        Else 'Se abre la venta en 2 operaciones (Punto de quiebre)
                            'Primera venta a precio vector
                            UnidadesPrimeraVenta = drSaldoInicial("SaldoInicial") - UnidadesVendidas + drOperacion("Unidades")
                            ImporteVentas = ImporteVentas + UnidadesPrimeraVenta * drSaldoInicial("ValorPrecio") * drSaldoInicial("TipoCambio")
                            ImporteVentasMO = ImporteVentasMO + UnidadesPrimeraVenta * drSaldoInicial("ValorPrecio")
                            If UnidadesPrimeraVenta > 0 Then
                                drDetalleFinal("Fecha") = drOperacion("Fecha")
                                drDetalleFinal("Unidades") = UnidadesPrimeraVenta
                                drDetalleFinal("Precio") = drSaldoInicial("ValorPrecio")
                                drDetalleFinal("TipoCambio") = drSaldoInicial("TipoCambio")
                                drDetalleFinal("PrecioMO") = drSaldoInicial("ValorPrecio")
                                If Convert.ToDecimal(drOperacion("FechaOperacion")) >= FechaInicio Then
                                    ImporteVentas2 = ImporteVentas2 + UnidadesPrimeraVenta * drSaldoInicial("ValorPrecio") * drSaldoInicial("TipoCambio")
                                    ImporteVentas2MO = ImporteVentas2MO + UnidadesPrimeraVenta * drSaldoInicial("ValorPrecio")
                                    dtDetalleFinal.Rows.Add(drDetalleFinal)
                                End If
                            End If
                            'Segunda venta a precio promedio
                            ImporteVentas = ImporteVentas + (UnidadesVendidas - drSaldoInicial("SaldoInicial")) * PrecioPromedio
                            ImporteVentasMO = ImporteVentasMO + (UnidadesVendidas - drSaldoInicial("SaldoInicial")) * PrecioPromedioMO
                            drDetalleFinal = dtDetalleFinal.NewRow
                            drDetalleFinal("TipoOperacion") = "VENTA"
                            drDetalleFinal("Fecha") = drOperacion("Fecha")
                            drDetalleFinal("Unidades") = UnidadesVendidas - drSaldoInicial("SaldoInicial")
                            drDetalleFinal("Precio") = PrecioPromedio
                            drDetalleFinal("TipoCambio") = 1
                            drDetalleFinal("PrecioMO") = PrecioPromedioMO
                            If Convert.ToDecimal(drOperacion("FechaOperacion")) >= FechaInicio Then
                                ImporteVentas2 = ImporteVentas2 + (UnidadesVendidas - drSaldoInicial("SaldoInicial")) * PrecioPromedio
                                ImporteVentas2MO = ImporteVentas2MO + (UnidadesVendidas - drSaldoInicial("SaldoInicial")) * PrecioPromedioMO
                                dtDetalleFinal.Rows.Add(drDetalleFinal)
                            End If
                            StockAgotado = True
                        End If
                    Else
                        ImporteVentas = ImporteVentas + drOperacion("Unidades") * drSaldoInicial("ValorPrecio") * drSaldoInicial("TipoCambio")
                        ImporteVentasMO = ImporteVentasMO + drOperacion("Unidades") * drSaldoInicial("ValorPrecioSIMO")
                        drDetalleFinal("Fecha") = drOperacion("Fecha")
                        drDetalleFinal("Unidades") = drOperacion("Unidades")
                        drDetalleFinal("Precio") = drSaldoInicial("ValorPrecio")
                        drDetalleFinal("TipoCambio") = drSaldoInicial("TipoCambio")
                        drDetalleFinal("PrecioMO") = drSaldoInicial("ValorPrecioSIMO")
                        If Convert.ToDecimal(drOperacion("FechaOperacion")) >= FechaInicio Then
                            ImporteVentas2 = ImporteVentas2 + drOperacion("Unidades") * drSaldoInicial("ValorPrecio") * drSaldoInicial("TipoCambio")
                            ImporteVentas2MO = ImporteVentas2MO + drOperacion("Unidades") * drSaldoInicial("ValorPrecioSIMO")
                            dtDetalleFinal.Rows.Add(drDetalleFinal)
                        End If
                    End If
                Else 'COMPRA
                    UnidadesCompradas = UnidadesCompradas + ToNullDecimal(drOperacion("Unidades"))
                    ImporteCompras = ImporteCompras + (ToNullDecimal(drOperacion("Unidades")) * ToNullDecimal(drOperacion("Precio")) * ToNullDecimal(drOperacion("TipoCambio")))
                    ImporteComprasMO = ImporteComprasMO + (ToNullDecimal(drOperacion("Unidades")) * ToNullDecimal(drOperacion("Precio"))) 'HDG
                    'KARDEX PRECIO PROMEDIO
                    drDetalleFinal("TipoOperacion") = "COMPRA"
                    drDetalleFinal("Fecha") = drOperacion("Fecha")
                    drDetalleFinal("Unidades") = ToNullDecimal(drOperacion("Unidades"))
                    drDetalleFinal("Precio") = ToNullDecimal(drOperacion("Precio"))
                    drDetalleFinal("TipoCambio") = ToNullDecimal(drOperacion("TipoCambio"))
                    drDetalleFinal("PrecioMO") = ToNullDecimal(drOperacion("Precio")) 'HDG
                    If StockAgotado Then
                        drDetalleFinal("UnidadesKardex") = UnidadesCompradas + drSaldoInicial("SaldoInicial") - UnidadesVendidas 'RGF 20101228 OT 61609
                        drDetalleFinal("ImporteKardex") = ImporteCompras + ImporteInicial - ImporteVentas 'RGF 20101228 OT 61609
                        drDetalleFinal("ImporteKardexMO") = ImporteComprasMO + ImporteInicialMO - ImporteVentasMO 'HDG
                    Else
                        If FechaPrimerDia.ToString.Substring(0, 4) - 1 = SALDO_ANIO_ANTERIOR Then  'HDG INC 66297 20121025
                            drDetalleFinal("UnidadesKardex") = UnidadesCompradas
                            drDetalleFinal("ImporteKardex") = ImporteCompras
                            drDetalleFinal("ImporteKardexMO") = ImporteComprasMO 'HDG
                        Else
                            drDetalleFinal("UnidadesKardex") = UnidadesCompradas + drSaldoInicial("SaldoInicial")
                            drDetalleFinal("ImporteKardex") = ImporteCompras + ImporteInicial
                            drDetalleFinal("ImporteKardexMO") = ImporteComprasMO + ImporteInicialMO 'HDG
                        End If
                    End If
                    If drDetalleFinal("UnidadesKardex") = 0 Then
                        PrecioPromedio = 0
                        PrecioPromedioMO = 0 'HDG
                    Else
                        PrecioPromedio = drDetalleFinal("ImporteKardex") / drDetalleFinal("UnidadesKardex")
                        PrecioPromedioMO = drDetalleFinal("ImporteKardexMO") / drDetalleFinal("UnidadesKardex")
                    End If
                    If Convert.ToDecimal(drOperacion("FechaOperacion")) >= FechaInicio Then
                        dtDetalleFinal.Rows.Add(drDetalleFinal)
                    End If
                End If
            Next
            drDetalleFinal = dtDetalleFinal.NewRow
            drDetalleFinal("TipoOperacion") = "COSTO_VENTAS"
            drDetalleFinal("ImporteKardex") = ImporteVentas2
            drDetalleFinal("ImporteKardexMO") = ImporteVentas2MO
            dtDetalleFinal.Rows.Add(drDetalleFinal)
            drDetalleFinal = dtDetalleFinal.NewRow
            drDetalleFinal("TipoOperacion") = "TOTAL_VENTAS"
            If dtTotalVentas.Rows.Count > 0 Then
                drDetalleFinal("ImporteKardex") = dtTotalVentas.Rows(0)("TotalVentas")
                drDetalleFinal("ImporteKardexMO") = dtTotalVentas.Rows(0)("TotalVentasMO") 'HDG
            Else
                drDetalleFinal("ImporteKardex") = 0
                drDetalleFinal("ImporteKardexMO") = 0 'HDG
            End If
            dtDetalleFinal.Rows.Add(drDetalleFinal)
            Return dtDetalleFinal
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#Region "SELECT DISTINCT"
    Public Shared Function SelectDistinct(ByVal SourceTable As DataTable, ByVal ParamArray FieldNames() As String) As DataTable
        Dim lastValues() As Object
        Dim newTable As DataTable
        If FieldNames Is Nothing OrElse FieldNames.Length = 0 Then
            Throw New ArgumentNullException("FieldNames")
        End If
        lastValues = New Object(FieldNames.Length - 1) {}
        newTable = New DataTable
        For Each field As String In FieldNames
            newTable.Columns.Add(field, SourceTable.Columns(field).DataType)
        Next
        For Each Row As DataRow In SourceTable.Select("", String.Join(", ", FieldNames))
            If Not fieldValuesAreEqual(lastValues, Row, FieldNames) Then
                newTable.Rows.Add(createRowClone(Row, newTable.NewRow(), FieldNames))
                setLastValues(lastValues, Row, FieldNames)
            End If
        Next
        Return newTable
    End Function
    Private Shared Function fieldValuesAreEqual(ByVal lastValues() As Object, ByVal currentRow As DataRow, ByVal fieldNames() As String) As Boolean
        Dim areEqual As Boolean = True
        For i As Integer = 0 To fieldNames.Length - 1
            If lastValues(i) Is Nothing OrElse Not lastValues(i).Equals(currentRow(fieldNames(i))) Then
                areEqual = False
                Exit For
            End If
        Next
        Return areEqual
    End Function
    Private Shared Function createRowClone(ByVal sourceRow As DataRow, ByVal newRow As DataRow, ByVal fieldNames() As String) As DataRow
        For Each field As String In fieldNames
            newRow(field) = sourceRow(field)
        Next
        Return newRow
    End Function
    Private Shared Sub setLastValues(ByVal lastValues() As Object, ByVal sourceRow As DataRow, ByVal fieldNames() As String)
        For i As Integer = 0 To fieldNames.Length - 1
            lastValues(i) = sourceRow(fieldNames(i))
        Next
    End Sub
#End Region
    Public Shared Function EnviarMail(ByVal toUser As String, ByVal ccUser As String, ByVal asunto As String, ByVal message As String, ByVal DatosRequest As DataSet, Optional ByVal sAttach As String = "") As Boolean
        Dim correo As MailMessage = New MailMessage()
        Dim smtp As SmtpClient = New SmtpClient()
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim dt As DataTable
        Dim fromUser As String = String.Empty
        'Se recogen los datos del 
        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(EMAIL_FORM, "", "", "", DatosRequest)
        fromUser = dt.Rows(0)(2).ToString()
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
        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(SERV_CORREO, "", "", "", DatosRequest)
        smtp.Host = dt.Rows(0)(2).ToString()
        smtp.Port = 25
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network
        Try
            smtp.Send(correo)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function EnviarMailPlantilla(ByVal toUser As String, ByVal ccUser As String, ByVal asunto As String, ByVal plantilla As String, ByVal parametros As Dictionary(Of String, String), ByVal DatosRequest As DataSet, Optional ByVal sAttach As String = "") As Boolean
        Dim oArchivoPlanoBM As New ArchivoPlanoBM
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim oArchivoPlanoBE As New DataSet
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("028", DatosRequest)

        Dim oFile As String = oArchivoPlanoBE.Tables(0).Rows(0).Item(4) & plantilla
        Dim cuerpoCorreo As String = System.IO.File.ReadAllText(oFile)
        For i = 0 To parametros.Count - 1
            Dim item = parametros.ElementAt(i)
            cuerpoCorreo = cuerpoCorreo.Replace(item.Key, item.Value)
        Next
        Dim correo As MailMessage = New MailMessage()
        Dim smtp As SmtpClient = New SmtpClient()
        Dim dt As DataTable
        Dim fromUser As String = String.Empty
        'Se recogen los datos del 
        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(EMAIL_FORM, "", "", "", DatosRequest)
        fromUser = dt.Rows(0)(2).ToString()
        Dim oAttachment As Attachment
        correo.From = New MailAddress(fromUser, "Fondos Sura", System.Text.Encoding.UTF8)
        'If ccUser <> "" Then
        '    correo.CC.Add(ccUser)
        'End If

        Dim arrayCc = ccUser.Split(";")
        For Each cc_Correo As String In arrayCc
            If (Not cc_Correo.Equals("")) Then
                correo.CC.Add(cc_Correo)
            End If
        Next

        'Para cuando ayan mas de un correo
        Dim array = toUser.Split(";")
        For Each s_Correo As String In array
            If (Not s_Correo.Equals("")) Then
                correo.To.Add(s_Correo)
            End If
        Next
        correo.Subject = asunto
        correo.Body = cuerpoCorreo
        correo.BodyEncoding = System.Text.Encoding.UTF8
        correo.IsBodyHtml = True
        If sAttach <> "" Then
            oAttachment = New Attachment(sAttach)
            correo.Attachments.Add(oAttachment)
        End If
        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(SERV_CORREO, "", "", "", DatosRequest)
        smtp.Host = dt.Rows(0)(2).ToString()
        smtp.Port = 25
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network
        Try
            smtp.Send(correo)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function EnviarMailPlantilla_X_ArchivoCodigo(ByVal toUser As String, ByVal ccUser As String, ByVal asunto As String, ByVal plantilla As String, ByVal parametros As Dictionary(Of String, String), ByVal archivoCodigo As String, ByVal DatosRequest As DataSet, Optional ByVal sAttach As String = "") As Boolean
        Dim oArchivoPlanoBM As New ArchivoPlanoBM
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim oArchivoPlanoBE As New DataSet
        'oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("028", DatosRequest)
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar(archivoCodigo, DatosRequest)

        Dim oFile As String = oArchivoPlanoBE.Tables(0).Rows(0).Item(4) & plantilla
        Dim cuerpoCorreo As String = System.IO.File.ReadAllText(oFile)
        For i = 0 To parametros.Count - 1
            Dim item = parametros.ElementAt(i)
            cuerpoCorreo = cuerpoCorreo.Replace(item.Key, item.Value)
        Next
        Dim correo As MailMessage = New MailMessage()
        Dim smtp As SmtpClient = New SmtpClient()
        Dim dt As DataTable
        Dim fromUser As String = String.Empty
        'Se recogen los datos del 
        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(EMAIL_FORM, "", "", "", DatosRequest)
        fromUser = dt.Rows(0)(2).ToString()
        Dim oAttachment As Attachment
        correo.From = New MailAddress(fromUser, "Fondos Sura", System.Text.Encoding.UTF8)
        Dim arrayCc = ccUser.Split(";")
        For Each cc_Correo As String In arrayCc
            If (Not cc_Correo.Equals("")) Then
                correo.CC.Add(cc_Correo)
            End If
        Next
        'Para cuando ayan mas de un correo
        Dim array = toUser.Split(";")
        For Each s_Correo As String In array
            If (Not s_Correo.Equals("")) Then
                correo.To.Add(s_Correo)
            End If
        Next
        correo.Subject = asunto
        correo.Body = cuerpoCorreo
        correo.BodyEncoding = System.Text.Encoding.UTF8
        correo.IsBodyHtml = True
        If sAttach <> "" Then
            oAttachment = New Attachment(sAttach)
            correo.Attachments.Add(oAttachment)
        End If
        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(SERV_CORREO, "", "", "", DatosRequest)
        smtp.Host = dt.Rows(0)(2).ToString()
        smtp.Port = 25
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network
        Try
            smtp.Send(correo)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ' INICIO | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-17 | Enviar correo
    Public Shared Function EnviarMail_v2(ByVal toUsers() As String, ByVal ccUsers() As String, ByVal asunto As String, ByVal message As String, ByVal DatosRequest As DataSet, ByVal ParamArray adjuntos() As Attachment) As Boolean
        Dim dt As DataTable
        Dim oParametrosGeneralesBM As New ParametrosGeneralesBM
        Dim fromUser As String = String.Empty

        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(EMAIL_FORM, "", "", "", DatosRequest)
        fromUser = dt.Rows(0)(2).ToString()

        Dim correo As MailMessage = New MailMessage()
        Dim smtp As SmtpClient = New SmtpClient()

        correo.From = New MailAddress(fromUser, "Fondos Sura", System.Text.Encoding.UTF8)

        For Each strCorreo As String In toUsers
            If Not strCorreo.Equals("") Then
                Try
                    correo.To.Add(strCorreo)
                Catch ex As Exception
                    'Omitimos los correos que no puedan ser configurados
                End Try
            End If
        Next

        'CRumiche: Mínimo debería haber un destinatario
        If correo.To.Count = 0 Then Throw New Exception("No se encontró Correos Destinatarios en la configuración")

        For Each strCorreo As String In ccUsers
            If Not strCorreo.Equals("") Then
                Try
                    correo.CC.Add(strCorreo)
                Catch ex As Exception
                    'Omitimos los correos que no puedan ser configurados
                End Try
            End If
        Next

        correo.Subject = asunto
        correo.Body = message
        correo.BodyEncoding = System.Text.Encoding.UTF8
        correo.IsBodyHtml = True

        'CRumiche: Agregamos los adjuntos al correo
        If adjuntos IsNot Nothing Then
            For Each unAttach As Attachment In adjuntos
                correo.Attachments.Add(unAttach)
            Next
        End If

        'Dim oAttachment As Attachment
        'If sAttach <> "" Then
        '    oAttachment = New Attachment(sAttach)
        '    correo.Attachments.Add(oAttachment)
        'End If

        dt = oParametrosGeneralesBM.SeleccionarPorFiltro(SERV_CORREO, "", "", "", DatosRequest)
        smtp.Host = dt.Rows(0)(2).ToString()

        smtp.Port = 25
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network
        Try
            smtp.Send(correo)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ' FIN | Proyecto SIT Fondos - Limites | Sprint I | CRumiche | 2018-10-17 | Enviar correo

    Public Shared Function GenerateRandomString(ByRef len As Integer, ByRef upper As Boolean) As String
        Dim rand As New Random
        Dim allowableChars() As Char = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()
        Dim final As String = String.Empty
        For i As Integer = 0 To len - 1
            final += allowableChars(rand.Next(allowableChars.Length - 1))
        Next
        Return IIf(upper, final.ToUpper(), final)
    End Function

    Public Shared Function MostrarPopUp(ByVal strDireccion As String, ByVal strAleatorio As String, ByVal intAncho As Integer, ByVal intAlto As Integer, _
        ByVal intPosicionX As Integer, ByVal intPosicionY As Integer, ByVal strMenuBar As String, ByVal strResizable As String, _
        ByVal strStatus As String, ByVal strScrollbar As String) As String
        Return "<script language='javascript'>" & _
                   "window.open('" + strDireccion + "', " & _
              "'" + strAleatorio + "', " & _
              "'width=" + intAncho.ToString + ", " & _
              " height=" + intAlto.ToString + ", " & _
              " top=" + intPosicionX.ToString + ", " & _
              " left=" + intPosicionY.ToString + ", " & _
              " menubar=" + strMenuBar + ", " & _
              " resizable=" + strResizable + ", " & _
              " status=" + strStatus + ", " & _
              " scrollbars=" + strScrollbar + "' )" & _
             "</script>"
    End Function
    Public Shared Sub AsignarMensajeBoton(ByVal boton As Button, ByVal CodigoMensaje As String, Optional ByVal MensajeAdicional As String = "")
        Dim _objUtilitario As New UtilDM
        Dim Mensaje As String = _objUtilitario.RetornarMensajeConfirmacion(CodigoMensaje)
        Mensaje = Mensaje + " " + MensajeAdicional
        'OT 9968 - 14/02/2017 - Carlos Espejo
        'Descripcion: Se agrega el comenado para que el boton solo de un click
        boton.Attributes.Add("onClick", "if (MostrarMensaje('" + Mensaje + "')){this.disabled = true; this.value = 'en proceso...'; __doPostBack('" + boton.ClientID + "','');}")
        'OT 9968 Fin
    End Sub
    Public Shared Sub AsignarMensajeBotonProceso(ByVal boton As Button, ByVal CodigoMensaje As String, Optional ByVal MensajeAdicional As String = "")
        Dim _objUtilitario As New UtilDM
        Dim Mensaje As String = _objUtilitario.RetornarMensajeConfirmacion(CodigoMensaje)
        Mensaje = Mensaje + " " + MensajeAdicional
        boton.Attributes.Add("onClick", "if (MostrarMensaje('" + Mensaje + "')){this.disabled = true; this.value = 'en proceso...'; __doPostBack('" + boton.ClientID + "','');}")
    End Sub

    Public Shared Function ManejaMensajeConfirmacionNumAsientos(ByVal listaNumAsientos As ArrayList, ByVal tipoLote As String) As String
        Dim _objUtilitario As New UtilDM
        Dim Mensaje As String
        If listaNumAsientos.Count = 3 Then
            Mensaje = "Se generaron : " & CType(listaNumAsientos(0), String) & " Asientos en Inversiones, " & CType(listaNumAsientos(1), String) & " Asientos en Tesoreria, " & CType(listaNumAsientos(2), String) & " Asientos en Valorizacion. "
        Else
            Mensaje = "Se generaron : " & listaNumAsientos(0) & " Asientos en "
            Select Case tipoLote
                Case "CVI"
                    Mensaje = Mensaje & "Inversiones."
                Case "VC"
                    Mensaje = Mensaje & "Valorizacion."
                    If listaNumAsientos(0) = -2 Then
                        Mensaje = "Debe cargar la rentabilidad de Encaje y la rentabilidad Total para generar los Asientos de valorizacion."
                    End If
                Case "CCI"
                    Mensaje = Mensaje & "Tesoreria."
                Case "PP"
                    Mensaje = Mensaje & "Provisiones."
                Case "PI"
                    Mensaje = Mensaje & "Provision de Interes."
            End Select
        End If
        Return Mensaje
    End Function
    Public Shared Function ObtenerLetra(ByVal numero As Integer) As String
        Dim abecedario As String = " ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim resultado As String = ""
        Dim valor As Integer = numero \ 26
        If valor <> 0 Then
            resultado = abecedario.Substring(numero, 1)
            numero = numero - (26 * valor)
            resultado += abecedario.Substring(numero, 1)
        Else
            resultado = abecedario.Substring(numero, 1)
        End If
        Return resultado
    End Function
    Public Shared Function RemoveDiacritics(ByVal Cadena As String) As String
        Dim stFormD As String = Cadena.Normalize(NormalizationForm.FormD)
        Dim sb As New StringBuilder()
        For ich As Integer = 0 To stFormD.Length - 1
            Dim uc As UnicodeCategory = CharUnicodeInfo.GetUnicodeCategory(stFormD(ich))
            If uc <> UnicodeCategory.NonSpacingMark Then
                sb.Append(stFormD(ich))
            End If
        Next
        Return (sb.ToString().Normalize(NormalizationForm.FormC))
    End Function
    Public Shared Function GetStringDateFromObject(ByVal value As Object, ByVal stringFormat As String, ByVal defaultValue As String) As String
        If TypeOf value Is Date Then
            Return DirectCast(value, Date).ToString(stringFormat)
        Else
            Return defaultValue
        End If
    End Function

    'OT10689 - Inicio.Implements Class Kill process excel
    Public Class COMObjectAplication
        Public ProgramID As String
        Public ProcessName As String
        Public ObjetoAplication As Object
        Public ProcessId As Integer
        Public Sub New()
            Me.ProgramID = "Word.Application"
            Me.ProcessName = "WINWORD"
            Me.CrearAplicacion()
        End Sub
        Public Sub New(ByVal programID As String, ByVal processName As String)
            Me.ProgramID = programID
            Me.ProcessName = processName
            Me.CrearAplicacion()
        End Sub
        Private Sub CrearAplicacion()
            Try
                Me.ObjetoAplication = Nothing
                Me.ProcessId = 0
                Dim procesosAntiguos As List(Of Integer) = Me.buscarProcesos(System.Diagnostics.Process.GetProcesses())
                Me.ObjetoAplication = CreateObject(Me.ProgramID)
                Dim procesosAhora() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcesses()
                For Each proceso As System.Diagnostics.Process In procesosAhora
                    If proceso.ProcessName.ToUpper = Me.ProcessName.ToUpper Then
                        If Not procesosAntiguos.Contains(proceso.Id) Then
                            Me.ProcessId = proceso.Id 'Se encontro el nuevo proceso creado
                            Exit For
                        End If
                    End If
                Next
            Catch ex As Exception
                Throw New Exception("No se pudo crear el componente [ " & Me.ProcessName & " ] en su servidor. Por favor comunicar a Gestión de la Demanda.")
            End Try
        End Sub
        Private Function buscarProcesos(ByVal listaProcesos() As System.Diagnostics.Process) As List(Of Integer)
            Return (From proceso In listaProcesos Where proceso.ProcessName.ToUpper = ProcessName.ToUpper Select proceso.Id).ToList()
        End Function
        Public Sub terminarProceso()
            Try
                If Me.ProcessId <> 0 Then Diagnostics.Process.GetProcessById(Me.ProcessId).Kill()
            Catch ex As Exception
            End Try
        End Sub
    End Class
    'OT10689 - Fin.



    '==== INICIO | PROYECTO FONDOS-II: RF007 | ZOLUXIONES | CRumiche | 29/05/2018

    ' ''' <summary>
    ' ''' Invocamos al Motor de Inversiones quien realizará los calculos centralizadamente (CRumiche: 2018-05-29)
    ' ''' Negociacines Soportadas:
    ' '''     -> Certificados de 
    ' '''     -> Papeles Comerciales
    ' ''' </summary>   
    'Public Shared Function CalcularNegociacion01Cupon(ByVal fechaInicio As DateTime,
    '                             ByVal fechaFin As DateTime,
    '                             ByVal tasaCupon As Decimal,
    '                             ByVal valorNominal As Decimal,
    '                             ByVal fechaLiquidacion As DateTime,
    '                             ByVal ValorYTM As Decimal,
    '                             ByVal baseMensual As BaseMensualCupon,
    '                             ByVal baseAnual As BasePeriodoCupon,
    '                             ByVal esCuponADescuento As Boolean) As NegociacionRentaFija

    '    '01: Parametrizamos (CRumiche: 2018-05-29)
    '    Dim porcAmortizacion As Decimal = 100 / 100 ' 100% en el único CUPON

    '    Dim cupones As New List(Of CuponRentaFija)

    '    ' Unico cupon para Certificado de Depósitos
    '    cupones.Add(New CuponRentaFija With {.FechaInicio = fechaInicio,
    '                                         .FechaFin = fechaFin,
    '                                         .PorcAmortizacion = porcAmortizacion})

    '    Dim valor As New ValorRentaFija
    '    valor.TasaCuponAnual = tasaCupon / 100 '%
    '    valor.NroCuponesAnual = 1 'Es Anual

    '    Dim neg As New NegociacionRentaFija
    '    neg.ValorNominal = valorNominal

    '    neg.ValorRentaFijaNegociado = valor
    '    neg.Cupones = cupones
    '    If esCuponADescuento Then neg.ReglasAdicionales.NO_GENERA_INTERES_CUPON = True

    '    '02: Generamos los cálculos de cada cupón respectivamente (CRumiche: 2018-05-29)
    '    neg.CalcularMapaDePagosEnCuponera()

    '    neg.FechaEvaluacion = fechaLiquidacion
    '    neg.YTM = ValorYTM / 100 '%

    '    neg.ValorRentaFijaNegociado.BaseMensualCupon = baseMensual
    '    neg.ValorRentaFijaNegociado.BaseAnualCupon = baseAnual

    '    neg.ValorRentaFijaNegociado.BaseMensualIC = baseMensual
    '    neg.ValorRentaFijaNegociado.BasePeriodoIC = BasePeriodoCupon.D_PERIODO_CUPON_ACT

    '    '03: Calculamos los resultantes de la negociación (CRumiche: 2018-05-29)
    '    neg.CalcularDatosDeNegociacion()

    '    Return neg
    'End Function

    ' ''' <summary>
    ' ''' Invocamos al Motor de Inversiones quien realizará los calculos centralizadamente (CRumiche: 2018-05-29)
    ' ''' Negociacines Soportadas:
    ' '''     -> Bono Estandar
    ' '''     -> Bono Soberanos
    ' '''     -> Bono Bullet
    ' '''     -> Bono Amortizable
    ' '''     -> Bono Internacionales
    ' '''     -> Bono Globales
    ' ''' 
    ' '''     -> Certificados de Deposito ('A Descuento', y tambien los Diferentes 'A Descuento')
    ' '''     -> Papeles Comerciales ('A Descuento', y tambien los Diferentes 'A Descuento')
    ' ''' </summary>
    ' ''' <param name="diasPeriodicidadCupon">Según los días tendriamos Periodos: 360(Anual), 180(Semestral), 90(Trimestral), etc </param>
    'Public Shared Function CalcularNegociacionRentaFija(ByVal dtDetalleCupones As DataTable,
    '                             ByVal tasaCupon As Decimal,
    '                             ByVal diasPeriodicidadCupon As Integer,
    '                             ByVal valorNominal As Decimal,
    '                             ByVal fechaLiquidacion As DateTime,
    '                             ByVal ValorYTM As Decimal,
    '                             ByVal baseMensual As BaseMensualCupon,
    '                             ByVal baseAnual As BaseAnualCupon,
    '                             ByVal esCuponADescuento As Boolean,
    '                             Optional ByVal esValorExtranjero As Boolean = False) As NegociacionRentaFija

    '    '01: Parametrizamos (CRumiche: 2018-05-29)

    '    ' Múltiples Cupones del Instrumento de renta fija
    '    Dim cupones As List(Of CuponRentaFija) = LlenarListaCuponesRentaFija(dtDetalleCupones)

    '    Dim valor As New ValorRentaFija
    '    valor.PrecioUnitario = valorNominal 'Solo para lograr compatibilidad con version antigua del Motor de Inversiones
    '    valor.TasaCuponAnual = tasaCupon / 100 '%

    '    valor.NroCuponesAnual = 1 'Es Anual por defecto
    '    If diasPeriodicidadCupon > 0 Then valor.NroCuponesAnual = 360 / diasPeriodicidadCupon 'Como mínimo un cupon al año

    '    Dim neg As New NegociacionRentaFija
    '    neg.CantidadUnidadesNegociadas = 1 'Solo para lograr compatibilidad con version antigua del Motor de Inversiones

    '    neg.ValorRentaFijaNegociado = valor
    '    neg.Cupones = cupones
    '    If esCuponADescuento Then neg.ReglasAdicionales.NO_GENERA_INTERES_CUPON = True

    '    neg.FechaEvaluacion = fechaLiquidacion
    '    neg.YTM = ValorYTM / 100 '%

    '    neg.ValorRentaFijaNegociado.BaseMensualCupon = baseMensual
    '    neg.ValorRentaFijaNegociado.BaseAnualCupon = baseAnual

    '    'neg.ValorRentaFijaNegociado.BaseMensualIC = baseMensual
    '    neg.ValorRentaFijaNegociado.BasePeriodoCupon = IIf(baseMensual = BaseMensualCupon.D_30, BasePeriodoCupon.D_PERIODO_CUPON_360, BasePeriodoCupon.D_PERIODO_CUPON_ACT)

    '    neg.AplicacionTasa = TipoAplicacionTasa.EFECTIVA

    '    neg.ReglasAdicionales.ES_VALOR_DEL_MERCADO_EXTRANJERO = esValorExtranjero
    '    'If esValorExtranjero Then neg.AplicacionTasa = TipoAplicacionTasa.EFECTIVA

    '    '02: Generamos los cálculos de cada cupón respectivamente (CRumiche: 2018-05-29)
    '    neg.CalcularMapaDePagosEnCuponera()

    '    '03: Calculamos los resultantes de la negociación (CRumiche: 2018-05-29)
    '    neg.CalcularDatosDelFlujoDeCuponesBasadoEnTIR()

    '    Return neg
    'End Function

    ''' <summary>
    ''' Invocamos al Motor de Inversiones quien realizará los calculos centralizadamente (CRumiche: 2018-05-29)
    ''' La Negociación se realiza en Base al TIR
    ''' Negociacines Soportadas:
    '''     -> Bono Estandar
    '''     -> Bono Soberanos
    '''     -> Bono Bullet
    '''     -> Bono Amortizable
    '''     -> Bono Internacionales
    '''     -> Bono Globales
    ''' 
    '''     -> Certificados de Deposito (A Descuento, Diferente A Descuento)
    '''     -> Papeles Comerciales (A Descuento, Diferente A Descuento)
    '''     -> Bonos T-Bills y Treasury -  OT12127
    ''' </summary>
    ''' <param name="precioUnitarioOriginal">Precio Unitario Original del Valor (Valor Nominar por cada unidad del intrumento) </param>
    ''' <param name="diasPeriodicidadCupon">Según los días tendriamos Periodos: 360(Anual), 180(Semestral), 90(Trimestral), etc </param>
    Public Shared Function CalcularNegociacionRentaFija(ByVal dtDetalleCupones As DataTable,
                                 ByVal tasaCupon As Decimal,
                                 ByVal diasPeriodicidadCupon As Integer,
                                 ByVal precioUnitarioOriginal As Decimal,
                                 ByVal cantidadUnidNegociadas As Decimal,
                                 ByVal fechaLiquidacion As DateTime,
                                 ByVal ValorYTM As Decimal,
                                 ByVal baseMensual As BaseMensualCupon,
                                 ByVal baseAnual As BaseAnualCupon,
                                 ByVal esCuponADescuento As Boolean,
                                 ByVal esValorExtranjero As Boolean,
                                 Optional ByVal aplicacionTasa As TipoAplicacionTasa = TipoAplicacionTasa.EFECTIVA,
                                 Optional ByVal vacEmision As Decimal = 1,
                                 Optional ByVal vacEvaluacion As Decimal = 1,
                                 Optional ByVal esTBill As Boolean = False) As NegociacionRentaFija

        '01: Parametrizamos (CRumiche: 2018-05-29)

        ' Múltiples Cupones del Instrumento de renta fija
        Dim cupones As List(Of CuponRentaFija) = LlenarListaCuponesRentaFija(dtDetalleCupones)

        Dim valor As New ValorRentaFija
        valor.PrecioUnitario = precioUnitarioOriginal
        valor.TasaCuponAnual = tasaCupon / 100 '%

        valor.NroCuponesAnual = 1 'Es Anual por defecto
        If diasPeriodicidadCupon > 0 Then valor.NroCuponesAnual = 360 / diasPeriodicidadCupon 'Como mínimo un cupon al año

        Dim neg As New NegociacionRentaFija
        neg.CantidadUnidadesNegociadas = cantidadUnidNegociadas

        neg.ValorRentaFijaNegociado = valor
        neg.Cupones = cupones
        If esCuponADescuento Then neg.ReglasAdicionales.NO_GENERA_INTERES_CUPON = True

        'OT12127 | 2019-07-16 | rcolonia | Zoluxiones | Agregar Negociación TBILL
        If esTBill Then neg.ReglasAdicionales.CALCULO_TBILL = True

        neg.ReglasAdicionales.ES_VALOR_DEL_MERCADO_EXTRANJERO = esValorExtranjero

        neg.FechaEvaluacion = fechaLiquidacion

        neg.ValorRentaFijaNegociado.BaseMensualCupon = baseMensual
        neg.ValorRentaFijaNegociado.BaseAnualCupon = baseAnual

        'neg.ValorRentaFijaNegociado.BaseMensualIC = baseMensual
        neg.ValorRentaFijaNegociado.BasePeriodoCupon = IIf(baseMensual = BaseMensualCupon.D_30, BasePeriodoCupon.D_PERIODO_CUPON_360, BasePeriodoCupon.D_PERIODO_CUPON_ACT)

        neg.YTM = ValorYTM / 100 '%
        neg.AplicacionTasa = aplicacionTasa

        neg.VAC_Emision = vacEmision
        neg.VAC_Evaluacion = vacEvaluacion

        '02: Generamos los cálculos de cada cupón respectivamente (CRumiche: 2018-05-29)
        neg.CalcularMapaDePagosEnCuponera()

        '03: Calculamos los Datos del Cupon Vigente (CRumiche: 2018-05-29)
        neg.CalcularDatosDelCuponVigente()

        '04: Calculamos los Flujos Basados en TIR (NOTA: Para Calculo en Base de a Precio Sucio se debe 
        '   llamar al método CalcularDatosDelFlujoDeCuponesBasadoEnPrecioSucio()) (CRumiche: 2018-05-29)
        neg.CalcularDatosDelFlujoDeCuponesBasadoEnTIR()

        Return neg
    End Function

    Public Shared Function CalcularVariacionEstimada(ByVal variacionSIT As Decimal, ByVal valorSemaforo As Decimal, ByVal dtVariacion As DataTable) As ValorCuotaVariacionBE

        Dim oResultado As New ValorCuotaVariacionBE
        Dim filaFormula As DataRow
        If dtVariacion.Rows.Count > 0 Then
            filaFormula = dtVariacion.Rows(0)
            oResultado.CarteraPrecio = filaFormula("CarteraPrecio")
            oResultado.CarteraTipoCambio = filaFormula("CarteraTipoCambio")
            oResultado.Derivados = filaFormula("Derivados")
            oResultado.CajaTipoCambio = filaFormula("CajaTipoCambio")
            oResultado.CuentasPorPagarTipoCambio = filaFormula("CuentasPorPagarTipoCambio")
            oResultado.CuentasPorCobrarTipoCambio = filaFormula("CuentasPorCobrarTipoCambio")
            oResultado.CuentasPorCobrarPrecio = filaFormula("CuentasPorCobrarPrecio")
            oResultado.CuentasPorPagarPrecio = filaFormula("CuentasPorPagarPrecio")
            oResultado.Comision = filaFormula("Comision") / 100 'pasando a porcentage
            oResultado.TotalRentabilidadInversiones = filaFormula("Total")
            oResultado.PorcentageVariacionEstimado = oResultado.TotalRentabilidadInversiones - oResultado.Comision
            oResultado.PorcentageVariacionSIT = variacionSIT
            oResultado.DiferenciaEstimadoSIT = oResultado.PorcentageVariacionEstimado - oResultado.PorcentageVariacionSIT
        End If




        'Semaforo Verde :'V'
        'Semaforo Rojo :'R'
        If valorSemaforo > Math.Abs(oResultado.DiferenciaEstimadoSIT) Then
            oResultado.EstadoSemaforo = "V"
        Else
            oResultado.EstadoSemaforo = "R"
        End If

        Return oResultado


    End Function

    Public Shared Sub EstablecerSemaforoAnteriores(ByRef imgSemaforo As System.Web.UI.WebControls.Image)

        imgSemaforo.Visible = True
        Dim strImagenSemaforo As String = ""
        imgSemaforo.Style.Add("cursor", "pointer")
        imgSemaforo.Attributes.Remove("title")
        strImagenSemaforo = "~\App_Themes\img\icons\circulo_verde.png"
        imgSemaforo.Attributes.Add("onclick", "javascript:return AbrirModalGrafica();")
        imgSemaforo.ImageUrl = strImagenSemaforo
    End Sub

    Public Shared Sub EstablecerSemaforo(ByVal ExisteValorCuota As Boolean, ByVal variacionEstimada As ValorCuotaVariacionBE, ByRef imgSemaforo As System.Web.UI.WebControls.Image)

        imgSemaforo.Visible = True
        Dim strImagenSemaforo As String = ""
        imgSemaforo.Style.Add("cursor", "pointer")
        If ExisteValorCuota = False Then
            'No existe el registro en valor cuota
            strImagenSemaforo = "~\App_Themes\img\icons\circulo_plomo.png"
            imgSemaforo.Attributes.Add("title", "Pendiente a realizar el calculo de valor cuota")
            imgSemaforo.Attributes.Add("onclick", "javascript:return alertify.alert('Pendiente a realizar el calculo de valor cuota');")

        Else
            'si existe
            imgSemaforo.Attributes.Remove("title")
            'Obtener el valor para establecer el icono a mostrar
            If variacionEstimada.EstadoSemaforo = "R" Then
                strImagenSemaforo = "~\App_Themes\img\icons\circulo_rojo.png"
            ElseIf variacionEstimada.EstadoSemaforo = "V" Then
                strImagenSemaforo = "~\App_Themes\img\icons\circulo_verde.png"
            End If


            imgSemaforo.Attributes.Add("onclick", "javascript:return AbrirModalGrafica();")


        End If

        imgSemaforo.ImageUrl = strImagenSemaforo
    End Sub

    'Public Shared Sub EstablecerSemaforo(ByVal ExisteValorCuota As Boolean, ByVal diferenciaValorCuota As Decimal, ByVal valorSemaforo As Decimal, ByRef imgSemaforo As System.Web.UI.WebControls.Image)

    '    imgSemaforo.Visible = True
    '    Dim strImagenSemaforo As String = ""
    '    imgSemaforo.Style.Add("cursor", "pointer")
    '    If ExisteValorCuota = False Then
    '        'No existe el registro en valor cuota
    '        strImagenSemaforo = "~\App_Themes\img\icons\circulo_plomo.png"
    '        imgSemaforo.Attributes.Add("title", "Pendiente a realizar el calculo de valor cuota")
    '        imgSemaforo.Attributes.Add("onclick", "javascript:return alertify.alert('Pendiente a realizar el calculo de valor cuota');")

    '    Else
    '        'si existe
    '        imgSemaforo.Attributes.Remove("title")
    '        'Obtener el valor para establecer el icono a mostrar
    '        If valorSemaforo > diferenciaValorCuota Then
    '            strImagenSemaforo = "~\App_Themes\img\icons\circulo_rojo.png"
    '        Else
    '            strImagenSemaforo = "~\App_Themes\img\icons\circulo_verde.png"
    '        End If


    '        imgSemaforo.Attributes.Add("onclick", "javascript:return AbrirModalGrafica();")


    '    End If

    '    imgSemaforo.ImageUrl = strImagenSemaforo
    'End Sub

    Public Shared Function ObtenerSeriesFirbi(ByVal oValorCuotaBM As ValorCuotaBM, ByVal FechaOperacionCadena As String, ByVal CodigoPortafolio As String, ByRef TotalCuotas As Decimal) As DataTable
        Dim oPortafolioBM As New PortafolioBM
        Dim dtSeriesFondos As DataTable = oPortafolioBM.Portafolio_Series_CuotasFirbi(CodigoPortafolio, UIUtility.ConvertirFechaaDecimal(FechaOperacionCadena))
        Dim dtFondosCuotas As DataTable = oValorCuotaBM.ListarCuotasFondosOperaciones(FechaOperacionCadena)


        Dim listaCodigos = String.Join(",", (From dr In dtSeriesFondos Select dr("CodigoPortafolioSO")).ToArray())
        Dim dtFiltrado As DataTable = dtFondosCuotas.Select("ID IN (" + listaCodigos + ")").CopyToDataTable()
        Dim CantidadTotalCuotas As Decimal = Decimal.Parse(dtFiltrado.Compute("SUM(TOTAL_CUOTAS_CONTABLE)", String.Empty))
        TotalCuotas = 0
        For Each fila As DataRow In dtFondosCuotas.Select("ID IN (" + listaCodigos + ")")
            Dim idFondoFirbiOperacion As Integer = Convert.ToInt32(fila("ID"))
            Dim filaSeleccionada As DataRow = (From dr In dtSeriesFondos
                            Where (dr("CodigoPortafolioSO") = idFondoFirbiOperacion) Select dr
                   ).FirstOrDefault()

            filaSeleccionada("PorcentajeOnline") = (fila("TOTAL_CUOTAS_CONTABLE") * 100) / CantidadTotalCuotas
            If Decimal.Parse(filaSeleccionada("Porcentaje")) = 0 Then
                filaSeleccionada("Porcentaje") = (fila("TOTAL_CUOTAS_CONTABLE") * 100) / CantidadTotalCuotas
            End If

            If Decimal.Parse(filaSeleccionada("ValoresCierre")) = 0 Then
                filaSeleccionada("ValoresCierre") = fila("VALOR_CUOTA")
                filaSeleccionada("ValoresPrecierre") = fila("VALOR_CUOTA")
                filaSeleccionada("ValoresCieree") = fila("VALOR_CUOTA")
            End If

            If Decimal.Parse(filaSeleccionada("CuotasCierre")) = 0 Then
                filaSeleccionada("CuotasCierre") = fila("TOTAL_CUOTAS_CONTABLE")
                filaSeleccionada("CuotasPrecierre") = fila("TOTAL_CUOTAS_CONTABLE")
            End If

            filaSeleccionada("ValoresCierreOnline") = fila("VALOR_CUOTA")
            filaSeleccionada("CuotasCierreOnline") = fila("TOTAL_CUOTAS_CONTABLE")

            TotalCuotas = TotalCuotas + Decimal.Parse(filaSeleccionada("CuotasCierre"))
        Next
        'txtCuotasCierre.Text = totalCuotas
        'TotalCuotas = CantidadTotalCuotas
        Return dtSeriesFondos
    End Function



    Public Shared Function LlenarListaCuponesRentaFija(ByVal dtDetalleCupones As DataTable) As List(Of CuponRentaFija)
        Dim cupones As New List(Of CuponRentaFija)
        Dim provider As CultureInfo = CultureInfo.InvariantCulture

        For Each row As DataRow In dtDetalleCupones.Rows
            cupones.Add(New CuponRentaFija With {.FechaInicio = DateTime.ParseExact(row("FechaInicio").ToString, "yyyyMMdd", provider),
                                             .FechaFin = DateTime.ParseExact(row("FechaTermino").ToString, "yyyyMMdd", provider),
                                             .PorcAmortizacion = CDec(row("Amortizacion")) / 100})
        Next

        Return cupones
    End Function

    Public Shared Function ObtenerBaseMensualDesdeTexto(ByVal textoBaseMensual As String) As BaseMensualCupon
        If textoBaseMensual.Equals("30") Then Return BaseMensualCupon.D_30

        'De otro modo retornamos el ACTUAL
        Return BaseMensualCupon.D_ACT
    End Function

    Public Shared Function ObtenerBaseAnualDesdeTexto(ByVal textoBaseAnual As String) As BaseAnualCupon
        If textoBaseAnual.Equals("360") Then Return BaseAnualCupon.D_ANUAL_360
        If textoBaseAnual.Equals("365") Then Return BaseAnualCupon.D_ANUAL_365

        'De otro modo retornamos el ACTUAL
        Return BaseAnualCupon.D_ANUAL_ACT
    End Function

    Public Shared Function ObtenerTipoAplicacionTasaDesdeCodTipoTasa(ByVal codTipoTasa As String) As TipoAplicacionTasa
        If codTipoTasa.Equals("2") Then Return TipoAplicacionTasa.EFECTIVA

        'Retornamos el valor por Defecto
        Return TipoAplicacionTasa.NOMINAL ' codTipoTasa = "1" u otro valor
    End Function

    Public Shared Sub ObtenerValoresVAC(ByVal fechaEmision As Decimal, ByVal fechaEvaluada As Decimal,
                                  ByRef vacEmision As Decimal, ByRef vacEvaluacion As Decimal, ByVal DatosRequest As DataSet)

        Dim bmCotizacion As New CotizacionVACBM()
        Dim beCot As CotizacionVACBE = bmCotizacion.Seleccionar(fechaEmision, "FACVAC", DatosRequest)

        If beCot.Tables.Count = 0 OrElse beCot.Tables(0).Rows.Count = 0 OrElse CDec(beCot.Tables(0).Rows(0)("Valor")) <= 0 Then
            Throw New UserInfoException(HttpUtility.HtmlEncode("No se encontró una valor VAC para la fecha de Emisión: " & DateTime.ParseExact(fechaEmision, "yyyyMMdd", CultureInfo.InvariantCulture)))
        End If

        vacEmision = CDec(beCot.Tables(0).Rows(0)("Valor"))

        beCot = bmCotizacion.Seleccionar(fechaEvaluada, "FACVAC", DatosRequest)
        If beCot.Tables.Count = 0 OrElse beCot.Tables(0).Rows.Count = 0 OrElse CDec(beCot.Tables(0).Rows(0)("Valor")) <= 0 Then
            Throw New UserInfoException(HttpUtility.HtmlEncode("No se encontró una valor VAC para la fecha Evaluada: " & DateTime.ParseExact(fechaEvaluada, "yyyyMMdd", CultureInfo.InvariantCulture)))
        End If

        vacEvaluacion = CDec(beCot.Tables(0).Rows(0)("Valor"))
    End Sub


    '==== FIN | PROYECTO FONDOS-II: RF007 | ZOLUXIONES | CRumiche | 29/05/2018
    '==== INICIO | PROYECTO FONDOS-II: RF010 | ZOLUXIONES | RCE | Deshablita todos los campos de texto de un formulario | 10/08/2018
    Public Shared Sub deshabilitarCampos(ByVal p As Object)
        For Each c In p.controls
            If TypeOf (c) Is TextBox Then
                CType(c, TextBox).Enabled = False
            End If
        Next
    End Sub
    '==== FIN | PROYECTO FONDOS-II: RF010 | ZOLUXIONES | RCE | Deshablita todos los campos de texto de un formulario | 10/08/2018

    '==== INICIO | PROYECTO FONDOS-II: RF012 | ZOLUXIONES | RColonia | 08/06/2018
    ''' <summary>
    ''' Procedimiento General para enviar mensaje de confirmación en alertify a formularios de Negociación Individual
    ''' Negociacines Soportadas:
    '''     -> Bonos
    ''' </summary>
    ''' <param name="boton">Objeto botón para realizar evento click </param>
    ''' <param name="CodigoMensaje">Mensaje para mostrar en el popup de confirmación de alertify</param>
    ''' <param name="MensajeAdicional">Opcional para adicionar al mensaje del popup de confirmación de alertify</param>
    ''' <param name="Rpta">Opcional para indicar si al responder confirmación provocará doble postback </param>
    Public Shared Sub AsignarMensajeBotonAceptar(ByVal boton As Button, ByVal CodigoMensaje As String, Optional ByVal MensajeAdicional As String = "", Optional ByVal Rpta As String = "")
        Dim _objUtilitario As New UtilDM
        Dim Mensaje As String = _objUtilitario.RetornarMensajeConfirmacion(CodigoMensaje)
        Dim RetornarConfirmacion As String
        If Rpta = String.Empty Then
            RetornarConfirmacion = "if (e) { this.disabled = true; this.value = 'en proceso...'; __doPostBack('" + boton.ClientID + "',''); } else { return false }"
        Else
            RetornarConfirmacion = "if (e) { this.disabled = true; this.value = 'en proceso...'; document.getElementById('hdRptaConfirmar').value = 'SI'; __doPostBack('" + boton.ClientID + "',''); } else { document.getElementById('hdRptaConfirmar').value = 'NO'; return false; }"
        End If
        Mensaje = Mensaje + " " + MensajeAdicional

        boton.Attributes.Add("onClick", "if (Validar()){alertify.confirm('<b>" + HttpUtility.HtmlEncode(Mensaje) + "</b>',function (e) {" + RetornarConfirmacion + " });} else {return false}")
    End Sub
    '==== FIN | PROYECTO FONDOS-II: RF012 | ZOLUXIONES | RColonia | 08/06/2018
End Class


Public Class UserInfoException
    Inherits Exception

    Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub
End Class