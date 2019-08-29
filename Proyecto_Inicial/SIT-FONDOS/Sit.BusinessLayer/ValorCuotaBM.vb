Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports System.Transactions
Imports System.Collections.Generic

Public Class ValorCuotaBM
    Public Function CalcularValoresCuota(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, Optional ByVal CodigoSerie As String = "") As DataTable
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.CalcularValoresCuota(CodigoPortafolioSBS, FechaOperacion, CodigoSerie)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function CXCVentaCompra(ByVal Venta As String, ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal) As DataTable
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.CXCVentaCompra(Venta, CodigoPortafolioSBS, FechaOperacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ReporteValorCuota(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ByVal FechaCadena As String) As DataTable
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.ReporteValorCuota(CodigoPortafolioSBS, FechaOperacion, FechaCadena)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Insertar_ValorCuota(ByVal oValorCuotaBE As ValorCuotaBE, dataRequest As DataSet) As Boolean
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.Insertar_ValorCuota(oValorCuotaBE, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarValorCuota(ByVal CodigoPortafolioSBS As String, ByVal CodigoSerie As String, ByVal FechaProceso As Decimal) As ValorCuotaBE
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.SeleccionarValorCuota(CodigoPortafolioSBS, CodigoSerie, FechaProceso)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ObtenerValorCuotaCierreAnterior(ByVal CodigoPortafolioSBS As String, ByVal CodigoSerie As String, ByVal FechaProceso As Decimal) As Decimal
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.ObtenerValorCuotaCierreAnterior(CodigoPortafolioSBS, CodigoSerie, FechaProceso)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function PorcentajeComisionSerie(ByVal CodigoPortafolioSBS As String, ByVal CodigoSerie As String) As Decimal
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.PorcentajeComisionSerie(CodigoPortafolioSBS, CodigoSerie)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub PrecioValorCuota(ByVal CodigoPortafolioSBS As String, ByVal FechaProceso As Decimal, dataRequest As DataSet)
        Try
            Dim ValorCuota As New ValorCuotaDAM
            ValorCuota.PrecioValorCuota(CodigoPortafolioSBS, FechaProceso, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'OT 9851 27/01/2017 - Carlos Espejo
    'Descripcion: Funcion para calcular Otras CXP
    'OT 9981 17/02/2017 - Carlos Espejo
    'Descripcion: Se cambia MesAnterior a decimal
    Public Function OtrasCXP(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ChequePendiente As Decimal, RescatePendiente As Decimal, _
    MesAnterior As Decimal, ByVal DevolucionAcumulada As Decimal) As DataTable
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.OtrasCXP(CodigoPortafolioSBS, FechaOperacion, ChequePendiente, RescatePendiente, MesAnterior, DevolucionAcumulada)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT10965 - 24/11/2017 - Ian Pastor M.
    Public Function ValorCuota_ObtenerRescateValores(ByVal p_CodigoPortafolioSisOpe As Decimal, ByVal p_FechaOperacion As Decimal, ByVal p_TipoProceso As String) As DataTable
        Try
            Dim valorCuotaDAM As New ValorCuotaDAM
            Dim fechaOpe As DateTime = UtilDM.ConvertirStringaFecha(UtilDM.ConvertirFechaaString(p_FechaOperacion))
            Return valorCuotaDAM.ValorCuota_ObtenerRescateValores(p_CodigoPortafolioSisOpe, fechaOpe, p_TipoProceso)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT10965 - Fin
    'OT11008 - 19/01/2018 - Ian Pastor M.
    'Descripción: Obtiene los rescates pendientes del sistema de operaciones
    Public Function ValorCuota_ObtenerRescatesPendientes(ByVal p_CodigoPortafolio As String, ByVal p_CodigoPortafolioSisOpe As Decimal, _
                                                         ByVal p_fechaOperacion As Decimal, ByVal p_Seriado As String) As Decimal
        Try
            Dim montoRescatePendiente As Decimal = 0
            Dim objValorCuotaDAM As New ValorCuotaDAM
            Dim dtRescatesPendientes As DataTable = Nothing
            Dim fechaOperacion As DateTime = UtilDM.ConvertirStringaFecha(UtilDM.ConvertirFechaaString(p_fechaOperacion))
            If p_Seriado = "N" Then
                dtRescatesPendientes = objValorCuotaDAM.ValorCuota_ObtenerRescatesPendientes(p_CodigoPortafolioSisOpe, fechaOperacion)
                If dtRescatesPendientes.Rows.Count > 0 Then
                    montoRescatePendiente = Decimal.Parse(dtRescatesPendientes.Rows(0)("monto"))
                End If
            Else
                Dim objPortafolioDAM As New PortafolioDAM
                Dim dtPortafolioSerie As DataTable = Nothing
                dtPortafolioSerie = objPortafolioDAM.PortafolioCodigoListar_ValoresSerie(p_CodigoPortafolio)
                For Each dr As DataRow In dtPortafolioSerie.Rows
                    dtRescatesPendientes = objValorCuotaDAM.ValorCuota_ObtenerRescatesPendientes(dr("CodigoPortafolioSO"), fechaOperacion)
                    If dtRescatesPendientes.Rows.Count > 0 Then
                        montoRescatePendiente = montoRescatePendiente + Decimal.Parse(dtRescatesPendientes.Rows(0)("monto"))
                    End If
                Next
            End If
            Return montoRescatePendiente
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11008 - Fin
    'OT11169 - 15/02/2018 - Ian Pastor M.
    'Descripción: Obtiene los porcentaje series del sistema de operaciones
    Public Function ValorCuota_ObtenerPorcentajeSeries(ByVal p_CPPadreSisOpe As String, ByVal p_FechaOperacion As String, _
                                                       ByVal p_CodigoPortafolioSBS As String, ByVal p_DescripcionPortafolio As String, _
                                                       ByVal dataRequest As DataSet) As Boolean
        Dim rpta As Boolean = False
        Try
            Dim dt As DataTable
            Dim objValorCuotaDAM As New ValorCuotaDAM
            'Dim fechaOperacion As Date = UtilDM.fnDateAddDays(p_FechaOperacion, -1)
            dt = objValorCuotaDAM.ValorCuota_ObtenerPorcentajeSeries(p_CPPadreSisOpe, UtilDM.ConvertirStringaFecha(p_FechaOperacion))
            If dt.Rows.Count > 0 Then
                'Eliminamos los porcentajes antiguos antes de insertar los nuevos.
                Dim objportafolio As New PortafolioBM
                objportafolio.Eliminar_PorcentajeSeries(p_CodigoPortafolioSBS, UtilDM.ConvertirFechaaDecimal(p_FechaOperacion))
                'Insertamos los porcentajes de las series
                For Each dtRow As DataRow In dt.Rows
                    objportafolio.Insertar_PorcentajeSeries(p_CodigoPortafolioSBS, dtRow("CODIGO_SERIE").ToString(), _
                                                            UtilDM.ConvertirFechaaDecimal(p_FechaOperacion), _
                                                            Decimal.Parse(dtRow("PORCENTAJE")), dataRequest)
                Next
                rpta = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return rpta
    End Function
    'OT11169 - Fin

    'OT11192 - 12/03/2018 - Ian Pastor M.
    'Descripción: Obtiene el monto de aporte valores del sistema de operaciones
    Public Function ObtenerAporteValoresSisOpe(ByVal p_CodigoPortafolioSisOpe As Decimal, ByVal p_Fecha As String) As Decimal
        Try
            ObtenerAporteValoresSisOpe = 0
            Dim objValorCuotaDAM As New ValorCuotaDAM
            Dim fechaOperacion As Date = UtilDM.ConvertirStringaFecha(p_Fecha)
            Dim dt As DataTable = objValorCuotaDAM.ObtenerAporteValoresSisOpe(p_CodigoPortafolioSisOpe, fechaOperacion)
            If dt.Rows.Count > 0 Then
                ObtenerAporteValoresSisOpe = Decimal.Parse(dt.Rows(0)("total"))
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11192 - Fin

    'OT11143 - 01/03/2018 - Carlos Rumiche L.
    'Descripción: Obtiene los conceptos contables del portafolio Firbi
    Public Function ObtenerSaldoPorConceptoContableFirbi(ByVal ConceptosContables As DataTable, ByVal CodConceptoContableFirbi As String, _
                                                                ByVal fechaOperacion As Decimal) As Decimal
        Try
            Dim saldo As Decimal = 0
            Dim operador As Integer = 1
            Dim lista As DataRow() = ConceptosContables.Select("CodConceptoCont = '" & CodConceptoContableFirbi & "'")
            If lista.Length > 0 Then
                For Each dtRow As DataRow In lista
                    operador = 1
                    If dtRow("Signo") = "-" Then operador = -1
                    saldo = saldo + operador * RetornaSaldoDetalleContableFirbi(dtRow("ReglaCuentas"), dtRow("ExclusionRegla"), Left(fechaOperacion.ToString, 4), fechaOperacion.ToString.Substring(4, 2))
                Next
            End If
            ObtenerSaldoPorConceptoContableFirbi = Math.Abs(saldo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerDatosFirbiPrecierre(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal) As DataTable
        Try
            Dim objValorCuotaDAM As New ValorCuotaDAM
            Dim resul As DataTable
            resul = New DataTable
            'Debemos Obtener los siguientes datos
            resul.Columns.Add("CajaPrecierre", GetType(Decimal)) ' Mediante Conceptos Contables
            resul.Columns.Add("CXCVentaTitulo", GetType(Decimal)) ' Mediante Conceptos Contables
            resul.Columns.Add("CXPCompraTitulo", GetType(Decimal)) ' Mediante Conceptos Contables
            resul.Columns.Add("InversionesSubTotal", GetType(Decimal)) ' Mediante Conceptos Contables
            resul.Columns.Add("ValCuotaPreCierre", GetType(Decimal))

            ' Iniciamos obteniendo los valores CONCEPTO CONTABLE, provenientes de OPERACIONES
            Dim listaConcepContablesFirbi As DataTable = SeleccionarConceptoContableFirbi("", "A")

            ' Habilitar esta sección cuando se programe las llamadas a la BD operaciones
            Dim saldoCaja As Decimal = ObtenerSaldoPorConceptoContableFirbi(listaConcepContablesFirbi, "CAJA", FechaOperacion)
            Dim saldoCxC As Decimal = ObtenerSaldoPorConceptoContableFirbi(listaConcepContablesFirbi, "CXC", FechaOperacion)
            Dim saldoCxP As Decimal = ObtenerSaldoPorConceptoContableFirbi(listaConcepContablesFirbi, "CXP", FechaOperacion)
            Dim saldoInversiones As Decimal = ObtenerSaldoPorConceptoContableFirbi(listaConcepContablesFirbi, "INVERSIONES", FechaOperacion)
            Dim valorCuotaPrecierre As Decimal = objValorCuotaDAM.ObtenerUltimoValorCuotaPrecierre_Cuotas(CodigoPortafolioSBS, FechaOperacion, "")

            Dim row As DataRow = resul.NewRow
            row("CajaPrecierre") = saldoCaja
            row("CXCVentaTitulo") = saldoCxC
            row("CXPCompraTitulo") = saldoCxP
            row("InversionesSubTotal") = saldoInversiones
            row("ValCuotaPreCierre") = valorCuotaPrecierre
            resul.Rows.Add(row)
            Return resul
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarConceptoContableFirbi(ByVal CodigoConceptoContable As String, ByVal situacion As String) As DataTable
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.SeleccionarConceptoContableFirbi(CodigoConceptoContable, situacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function RetornaSaldoDetalleContableFirbi(ByVal p_CuentaContable As String, ByVal p_ExclusionRegla As String, ByVal p_Year As String, ByVal p_Month As String) As Decimal
        Try
            Dim monto As Decimal = 0
            Dim ValorCuota As New ValorCuotaDAM
            Dim dtRows() As DataRow = Nothing

            Dim dtDatos As DataTable = ValorCuota.RetornaSaldoDetalleContableFirbi(p_CuentaContable, p_Year, p_Month)
            Dim dtDatosAux As DataTable = Nothing
            If p_ExclusionRegla <> "" Then
                Dim ExclusionAux As String() = p_ExclusionRegla.Split(",")
                For Each cadena As String In ExclusionAux
                    cadena = "'" + cadena + "'"
                Next
                p_ExclusionRegla = String.Join(",", ExclusionAux)
                If dtDatos.Rows.Count > 0 Then
                    dtDatosAux = dtDatos.Select("ccd01cta NOT IN (" & p_ExclusionRegla & ")").CopyToDataTable()
                End If
            End If
            If dtDatosAux Is Nothing Then
                dtDatosAux = dtDatos
            End If

            Dim sumaHaberTotal As Decimal = Decimal.Parse(dtDatosAux.Compute("SUM(ccd01car)", String.Empty))
            Dim sumaDebeTotal As Decimal = Decimal.Parse(dtDatosAux.Compute("SUM(ccd01abo)", String.Empty))
            Dim sumaTotal As Decimal = 0
            sumaTotal = sumaHaberTotal - sumaDebeTotal
            'dtRows = dtDatos.Select("ccd01cta NOT IN ('" & p_ExclusionRegla & "')")
            'If (dtRows IsNot Nothing) Then
            '    If (dtRows.Length > 0) Then
            '        For Each dtRow As DataRow In dtRows
            '            monto = monto + (Decimal.Parse(dtRow("ccd01car")) - Decimal.Parse(dtRow("ccd01abo")))
            '        Next
            '    End If
            'End If
            RetornaSaldoDetalleContableFirbi = sumaTotal
            'Return ValorCuota.RetornaSaldoDetalleContableFirbi(p_CuentaContable, p_ExclusionRegla, p_Year, p_Month)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerFechaMaxima(ByVal p_CodigoPortafolio As String) As Decimal
        Try
            Dim objValorCuotaDAM As New ValorCuotaDAM
            Return objValorCuotaDAM.ObtenerFechaMaxima(p_CodigoPortafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11143 - Fin

    'OT11339 - 31/05/2018 - Ian Pastor M.
    'Descripción: Obtiene los cheques pendientes del sistema de operaciones
    Public Function ObtenerChequePendienteSisOpe(ByVal p_CodigoPortafolio As String, ByVal p_CodigoPortafolioSisOpe As Integer, _
                                                 ByVal p_Seriado As String) As Decimal
        Try
            Dim montoChequePendiente As Decimal = 0
            Dim objValorCuotaDAM As New ValorCuotaDAM
            Dim dtChequePendiente As DataTable
            If (p_Seriado = "N") Then
                dtChequePendiente = objValorCuotaDAM.ValorCuota_ObtenerChequePendiente(p_CodigoPortafolioSisOpe)
                If (dtChequePendiente IsNot Nothing) Then
                    If dtChequePendiente.Rows.Count > 0 Then
                        montoChequePendiente = Decimal.Parse(dtChequePendiente.Compute("Sum(MONTO)", String.Empty))
                    End If
                End If
            Else
                Dim objPortafolioDAM As New PortafolioDAM
                Dim dtPortafolioSerie As DataTable = Nothing
                dtPortafolioSerie = objPortafolioDAM.PortafolioCodigoListar_ValoresSerie(p_CodigoPortafolio)
                If dtPortafolioSerie IsNot Nothing Then
                    For Each dr As DataRow In dtPortafolioSerie.Rows
                        dtChequePendiente = objValorCuotaDAM.ValorCuota_ObtenerChequePendiente(Decimal.Parse(dr("CodigoPortafolioSO")))
                        If dtChequePendiente IsNot Nothing Then
                            If dtChequePendiente.Rows.Count > 0 Then
                                montoChequePendiente = montoChequePendiente + Decimal.Parse(dtChequePendiente.Compute("Sum(MONTO)", String.Empty))
                                '   montoChequePendiente = montoChequePendiente + Decimal.Parse(dtChequePendiente.Rows(0)("MONTO"))
                            End If
                        End If
                    Next
                End If
            End If
            Return montoChequePendiente
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11339 - Fin

    '06/05/2018 - Ian Pastor M.
    'Descripción: Obtiene las comisiones SAF del sistema de operaciones
    Public Function ObtenerComisionSAFSisOpe(ByVal p_CodigoPortafolio As String, ByVal p_CodigoPortafolioSisOpe As Integer, _
                                                 ByVal p_Seriado As String, ByVal p_FechaOperacion As String) As Decimal
        Try
            Dim montoComisionSAF As Decimal = 0
            Dim objValorCuotaDAM As New ValorCuotaDAM
            'Dim dtComisionSAF As DataTable
            Dim fechaInicio As String = String.Empty
            fechaInicio = "01" & Right(p_FechaOperacion, 8)
            If (p_Seriado = "N") Then
                montoComisionSAF = objValorCuotaDAM.ObtenerComisionSAFSisOpe(p_CodigoPortafolioSisOpe, _
                                                                             UtilDM.ConvertirStringaFecha(fechaInicio), _
                                                                             UtilDM.ConvertirStringaFecha(p_FechaOperacion))
                'dtComisionSAF = objValorCuotaDAM.ObtenerComisionSAFSisOpe(p_CodigoPortafolioSisOpe, UtilDM.ConvertirStringaFecha(fechaInicio), UtilDM.ConvertirStringaFecha(p_FechaOperacion))
                'If (dtComisionSAF IsNot Nothing) Then
                '    If dtComisionSAF.Rows.Count > 0 Then
                '        montoComisionSAF = Decimal.Parse(dtComisionSAF.Rows(0)("MONTO"))
                '    End If
                'End If
            Else
                Dim objPortafolioDAM As New PortafolioDAM
                Dim dtPortafolioSerie As DataTable = Nothing
                dtPortafolioSerie = objPortafolioDAM.PortafolioCodigoListar_ValoresSerie(p_CodigoPortafolio)
                If dtPortafolioSerie IsNot Nothing Then
                    For Each dr As DataRow In dtPortafolioSerie.Rows
                        montoComisionSAF = montoComisionSAF + _
                            objValorCuotaDAM.ObtenerComisionSAFSisOpe(Decimal.Parse(dr("CodigoPortafolioSO")), _
                                                                      UtilDM.ConvertirStringaFecha(fechaInicio), _
                                                                      UtilDM.ConvertirStringaFecha(p_FechaOperacion))
                        'dtComisionSAF = objValorCuotaDAM.ObtenerComisionSAFSisOpe(Decimal.Parse(dr("CodigoPortafolioSO")), UtilDM.ConvertirStringaFecha(fechaInicio), UtilDM.ConvertirStringaFecha(p_FechaOperacion))
                        'If dtComisionSAF IsNot Nothing Then
                        '    If dtComisionSAF.Rows.Count > 0 Then
                        '        montoComisionSAF = montoComisionSAF + Decimal.Parse(dtComisionSAF.Rows(0)("MONTO"))
                        '    End If
                        'End If
                    Next
                End If
            End If
            If montoComisionSAF < 0 Then montoComisionSAF = 0
            Return montoComisionSAF
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'Fin

    Public Function ValorCuota_ObtenerImporteCaja_CxC(ByVal fechaOperacion As Decimal, ByVal CodigoPortafolioSBS As String) As DataTable
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.ValorCuota_ObtenerImporteCaja_CxC(fechaOperacion, CodigoPortafolioSBS)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValorCuota_ObtenerDevolucionComisionUnificadaSisOpe(ByVal p_FechaInicio As String, ByVal p_FechaFin As String, ByVal p_NombrePortafolio As String) As Decimal
        Try
            Dim DevolucionComisionUnificadaSisOpe As Decimal = 0D
            Dim objValorCuotaDAM As New ValorCuotaDAM
            Return objValorCuotaDAM.ValorCuota_ObtenerDevolucionComisionUnificadaSisOpe(UtilDM.ConvertirStringaFecha(p_FechaInicio), _
                                                                                        UtilDM.ConvertirStringaFecha(p_FechaFin), _
                                                                                        p_NombrePortafolio)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Variacion_DatosGrafica(ByVal codigoPortafolio As String, ByVal codigoSerie As String, ByVal fechaProceso As String, ByVal tipoPeriodo As String) As DataTable
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.Variacion_DatosGrafica(codigoPortafolio, codigoSerie, fechaProceso, tipoPeriodo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Variacion_Obtener(ByVal codigoPortafolioSBS As String, ByVal codigoSerie As String, ByVal fecha As Decimal) As DataTable
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.Variacion_Obtener(codigoPortafolioSBS, codigoSerie, fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Variacion_ValidarExistenciaVariacion(ByVal codigoPortafolioSBS As String, ByVal codigoSerie As String, ByVal fecha As Decimal) As DataTable
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.Variacion_ValidarExistenciaVariacion(codigoPortafolioSBS, codigoSerie, fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Variacion_GenerarCalculo(ByVal codigoPortafolioSBS As String, ByVal codigoSerie As String, ByVal fecha As Decimal) As DataTable
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.Variacion_GenerarCalculo(codigoPortafolioSBS, codigoSerie, fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Variacion_Insertar(ByVal valorCuotaVariacionBE As ValorCuotaVariacionBE, dataRequest As DataSet) As Boolean
        Try
            Dim ValorCuota As New ValorCuotaDAM
            Return ValorCuota.Variacion_Insertar(valorCuotaVariacionBE, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function ListarDistribucionFlujo(ByVal dataRequest As DataSet) As DataSet
        Try
            Return New ValorCuotaDAM().ListarDistribucionFlujo()
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ListarFondosComisionUnificadaOperaciones(ByVal fechaDate As DateTime) As DataSet
        Try
            Return New ValorCuotaDAM().ListarFondosComisionUnificadaOperaciones(fechaDate)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ListarFondosComisionUnificadaSit(ByVal fecha As Decimal) As DataSet
        Try
            Return New ValorCuotaDAM().ListarFondosComisionUnificadaSit(fecha)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function CalcularDevolucionComisionesFondo(ByVal fechaCalculo As DateTime, ByVal idFondo As Integer, ByVal comisionUnificada As Decimal, ByVal totalCuotas As Decimal, ByVal eliminar As Boolean, ByVal tipoCambioSafp As Decimal, ByVal dataRequest As DataSet) As Boolean

        Try
            'Return New ValorCuotaDAM().CalcularDevolucionComisionesFondo(ValorCuotaVariacionBE, dataRequest)
            Return True
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function GenerarDevolucionOperacion(ByVal fechaOperacion As Decimal, ByVal fechaOperacionCadena As String, ByVal tipoCambioSafp As Decimal, ByVal dtFondosOperacionesCuotas As DataTable, ByVal dtFondosSitComisiones As DataTable, ByVal dataRequest As DataSet) As Boolean

        Try
            Dim valorCuotaDAM As New ValorCuotaDAM

            Dim dtResumenPortafolios As DataTable = valorCuotaDAM.ListarResumenPortafolios(Convert.ToDateTime(fechaOperacionCadena), tipoCambioSafp)
            Dim listaDevolucion As New List(Of DevolucionComisionBE)

            For Each drFondoOperacion As DataRow In dtResumenPortafolios.Rows
                Dim portafolio As String = drFondoOperacion("PORTAFOLIO").ToString()
                Dim comision As Decimal = Convert.ToDecimal(drFondoOperacion("COMISION"))

                Dim dtDevolusiones As DataTable = valorCuotaDAM.SeleccionarDevolucionComision(portafolio, fechaOperacion)
                Dim objComision As New DevolucionComisionBE
                objComision.CodigoPortafolioSBS = portafolio
                objComision.MontoComision = comision
                objComision.FechaOperacion = fechaOperacion

                If (dtDevolusiones.Rows.Count = 0) Then
                    objComision.TipoPersistencia = "ING"
                Else
                    objComision.TipoPersistencia = "MOD"
                    Dim identificador As Integer = Convert.ToInt32(dtDevolusiones.Rows(0)("ID"))
                    objComision.Identificador = identificador
                End If

                listaDevolucion.Add(objComision)

            Next


            Using transaction As New TransactionScope
                For Each drFondo As DataRow In dtFondosOperacionesCuotas.Rows
                    Dim idFondoSerieA As String = drFondo("ID_SERIE_A").ToString()
                    Dim serie As String = drFondo("SERIE").ToString()
                    Dim fondo As String = drFondo("NOMBRE").ToString()
                    Dim moneda As String = drFondo("CODIGO_MONEDA").ToString()
                    Dim idFondo As Integer = Convert.ToInt32(drFondo("ID_FONDO"))
                    Dim comisionUnificada As Decimal = 0
                    Dim totalCuotas As Decimal = Convert.ToDecimal(drFondo("TOTAL_CUOTAS"))

                    Dim drComisionesUnificadas As DataRow() = dtFondosSitComisiones.Select("CodigoPortafolioSisOpe = '" + idFondoSerieA + "' ")


                    comisionUnificada = Convert.ToDecimal(drComisionesUnificadas(0)("ComisionSAFM"))
                    Dim fechaCalculoDate As DateTime = Convert.ToDateTime(fechaOperacionCadena)

                    valorCuotaDAM.CalcularDevolucionComisionesFondo(Convert.ToDateTime(fechaCalculoDate.AddDays(-1).ToShortDateString()), idFondo, comisionUnificada, totalCuotas, 1, tipoCambioSafp, dataRequest)
                Next

                transaction.Complete()
            End Using
            Using transaction As New TransactionScope
                For Each obj As DevolucionComisionBE In listaDevolucion
                    If obj.TipoPersistencia = "ING" Then
                        valorCuotaDAM.RegistrarDevolucionComision(obj.CodigoPortafolioSBS, obj.FechaOperacion, obj.MontoComision)
                    Else
                        valorCuotaDAM.ActualizarDevolucionComisiones(obj.Identificador, obj.CodigoPortafolioSBS, obj.FechaOperacion, obj.MontoComision)
                    End If
                Next

                transaction.Complete()
            End Using
          
            'For Each drFondoOperacion As DataRow In dtResumenPortafolios.Rows
            '    Dim portafolio As String = drFondoOperacion("PORTAFOLIO").ToString()
            '    Dim comision As Decimal = Convert.ToDecimal(drFondoOperacion("COMISION"))

            '    Dim dtDevolusiones As DataTable = valorCuotaDAM.SeleccionarDevolucionComision(portafolio, fechaOperacion)

            '    If (dtDevolusiones.Rows.Count = 0) Then
            '        valorCuotaDAM.RegistrarDevolucionComision(fechaOperacion, portafolio, comision)
            '    Else
            '        Dim identificador As Integer = Convert.ToInt32(dtDevolusiones.Rows(0)("ID"))
            '        valorCuotaDAM.ActualizarDevolucionComisiones(identificador, portafolio, fechaOperacion, comision)
            '    End If
            'Next
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function
    Public Function ValidarNegociacionFondosEnOtros(ByVal codigoPortafolioSBS As String, ByVal fecha As Decimal) As DataTable
        Try
            Return New ValorCuotaDAM().ValidarNegociacionFondosEnOtros(codigoPortafolioSBS, fecha)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ListarFondosConNombreSerie(ByVal codigoPortafolioSBS As String) As DataTable
        Try
            Return New ValorCuotaDAM().ListarFondosConNombreSerie(codigoPortafolioSBS)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarOperacionDevolucionDiaria(ByVal fechaDate As DateTime) As DataTable
        Try
            Return New ValorCuotaDAM().ListarOperacionDevolucionDiaria(fechaDate)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function RegistrarDevolucionComision(ByVal codigoPortafolioSBS As String, ByVal fechaOperacion As Decimal, ByVal montoDevolucion As Decimal) As Boolean
        Try
            Return New ValorCuotaDAM().RegistrarDevolucionComision(codigoPortafolioSBS, fechaOperacion, montoDevolucion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ActualizarDevolucionComisiones(ByVal id As Integer, ByVal codigoPortafolioSBS As String, ByVal fechaOperacion As Decimal, ByVal montoDevolucion As Decimal) As Boolean
        Try
            Return New ValorCuotaDAM().ActualizarDevolucionComisiones(id, codigoPortafolioSBS, fechaOperacion, montoDevolucion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ListarCuotasFondosOperaciones(ByVal fechaOperacionCadena As String) As DataTable
        Try
            Return New ValorCuotaDAM().ListarCuotasFondosOperaciones(fechaOperacionCadena)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

End Class
