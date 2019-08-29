
Namespace MotorInversiones

    Public Class Utiles

        ''' <summary>
        ''' Devuelve la cantidad de días entre dos fechas basándose en un año de 360 días (12 meses de 30 días)
        ''' </summary>
        ''' <param name="StartDate">Fecha Inicial</param>
        ''' <param name="EndDate">Fecha Final</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Dias360(ByVal StartDate As DateTime, ByVal EndDate As DateTime) As Double
            If EndDate < StartDate Then
                Throw New Exception("Función Dias360: La Fecha Final debe ser mayor o igual a la Fecha de Inicio")
            End If

            Dim StartDay As Integer = StartDate.Day
            Dim StartMonth As Integer = StartDate.Month
            Dim StartYear As Integer = StartDate.Year
            Dim EndDay As Integer = EndDate.Day
            Dim EndMonth As Integer = EndDate.Month
            Dim EndYear As Integer = EndDate.Year

            If StartDay = 31 Or IsLastDayOfFebruary(StartDate) Then
                StartDay = 30
            End If

            If StartDay = 30 And EndDay = 31 Then
                EndDay = 30
            End If

            Return (((EndYear - StartYear) * 360) + ((EndMonth - StartMonth) * 30) + (EndDay - StartDay))
        End Function

        Public Shared Function IsLastDayOfFebruary(ByVal fecha As DateTime) As Boolean
            Return (fecha.Month = 2 And fecha.Day = DateTime.DaysInMonth(fecha.Year, fecha.Month))
        End Function

        ''' <summary>
        ''' Devuelve la cantidad de días entre dos fechas basándose en un año de 365 días.
        ''' Si entre las fechas se encuentra la fecha 29/02 (Bisiesto), no será tomado en cuenta
        ''' </summary>
        ''' <param name="fechaInicio"></param>
        ''' <param name="fechaFin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Dias365(ByVal fechaInicio As DateTime, ByVal fechaFin As DateTime) As Double
            If fechaFin < fechaInicio Then
                Throw New Exception("Función Dias365: La Fecha Final debe ser mayor o igual a la Fecha de Inicio")
            End If

            Dim tieneDiaBisiesto As Boolean = False

            If DateTime.IsLeapYear(fechaInicio.Year) Or DateTime.IsLeapYear(fechaFin.Year) Then
                Dim fechaBisiesto As DateTime

                If DateTime.IsLeapYear(fechaInicio.Year) Then
                    fechaBisiesto = New DateTime(fechaInicio.Year, 2, 29) ' 29 de Febrero de ese Año
                Else
                    fechaBisiesto = New DateTime(fechaFin.Year, 2, 29) ' 29 de Febrero de ese Año
                End If

                tieneDiaBisiesto = (fechaInicio < fechaBisiesto And fechaBisiesto <= fechaFin)
            End If

            Return (fechaFin - fechaInicio).Days - IIf(tieneDiaBisiesto, 1, 0)
        End Function

        ''' <summary>
        ''' Devuelve la cantidad de días entre dos fechas restando ambas fechas directamente
        ''' </summary>
        ''' <param name="fechaInicio"></param>
        ''' <param name="fechaFin"></param>
        Public Shared Function DiasACT(ByVal fechaInicio As DateTime, ByVal fechaFin As DateTime) As Double
            If fechaFin < fechaInicio Then
                Throw New Exception("Función DiasACT (Base Anual ACTUAL): La Fecha Final debe ser mayor o igual a la Fecha de Inicio")
            End If

            Return (fechaFin - fechaInicio).Days
        End Function



        ''' <summary>
        ''' Devuelve la cantidad de días entre dos fechas según la Base Mensual del Cupon
        ''' </summary>    
        Public Shared Function DiasSegunBaseMensualCupon(ByVal baseMensual As BaseMensualCupon,
                                                     ByVal fechaInicio As DateTime,
                                                     ByVal fechaFin As DateTime) As Integer
            Dim cantDias As Integer = 0
            '1. Primero calculamos los Dias del Corrido
            If baseMensual = BaseMensualCupon.D_30 Then
                cantDias = Utiles.Dias360(fechaInicio, fechaFin)
            End If
            If baseMensual = BaseMensualCupon.D_ACT Then
                cantDias = Utiles.DiasACT(fechaInicio, fechaFin)
            End If

            Return cantDias
        End Function

        ''' <summary>
        ''' Devuelve la cantidad de Días de un Año (Base Anual) según la Base Anual indicada.
        ''' Para los casos 360 y 365 solo se requiere el parámetro baseAnual.
        ''' Si baseAnual = D_ACT, se requerirá el parámetro fechaEvaluacion.
        ''' Si baseAnual = D_X_PERIODO_CUPON, se requerirá los parámetros fechaInicioCupon, fechaFinCupon y nroCuponesAnual 
        ''' (Aplica principalmente para los Bonos Soberanos.
        ''' <param name="fechaEvaluacion">Aplica para la Base Anual Actual (D_ACT), esta fecha 
        ''' ayudará a determinar si la cantidad de días corresponde a un año Normal (365) o a un año Bisiesto (366)</param>
        ''' </summary>  
        Public Shared Function DiasSegunBaseAnualCupon(ByVal basePeriodo As BaseAnualCupon,
                                                     ByVal fechaEvaluacion As DateTime) As Integer

            Dim diasAnio As Integer = 365 'Valor por defecto = Nro de días de  un Año Normal

            If basePeriodo = BaseAnualCupon.D_ANUAL_360 Then
                diasAnio = 360
            End If
            If basePeriodo = BaseAnualCupon.D_ANUAL_365 Then
                diasAnio = 365
            End If
            If basePeriodo = BaseAnualCupon.D_ANUAL_ACT Then
                'Basado en el Año de la fecha de Cálculo o fecha de Evaluación (usualmente fecha de LIQUIDACION en una Negociación)
                diasAnio = 365 + IIf(DateTime.IsLeapYear(fechaEvaluacion.Year), 1, 0)
            End If

            Return diasAnio
        End Function

        ''' <summary>
        ''' Devuelve la cantidad de Días de un Año (Base Anual) según la Base Anual indicada.
        ''' Para los casos 360 y 365 solo se requiere el parámetro baseAnual.
        ''' Si baseAnual = D_ACT, se requerirá el parámetro fechaEvaluacion.
        ''' Si baseAnual = D_X_PERIODO_CUPON, se requerirá los parámetros fechaInicioCupon, fechaFinCupon y nroCuponesAnual 
        ''' (Aplica principalmente para los Bonos Soberanos.
        ''' <param name="fechaEvaluacion">Aplica para la Base Anual Actual (D_ACT), esta fecha 
        ''' ayudará a determinar si la cantidad de días corresponde a un año Normal (365) o a un año Bisiesto (366)</param>
        ''' </summary>  
        Public Shared Function DiasSegunBasePeriodo(ByVal basePeriodo As BasePeriodoCupon,
                                                     ByVal fechaInicioCupon As DateTime,
                                                     ByVal fechaFinCupon As DateTime) As Integer

            Dim diasAnio As Integer = 365 'Valor por defecto = Nro de días de  un Año Normal

            If basePeriodo = BasePeriodoCupon.D_PERIODO_CUPON_360 Then
                'Basado en los días del periodo de un CUPON con Año 360
                diasAnio = Utiles.Dias360(fechaInicioCupon, fechaFinCupon)
            Else ' If basePeriodo = BasePeriodoCupon.D_PERIODO_CUPON_ACT Then
                'Basado en los días REALES del periodo de un CUPON
                diasAnio = Utiles.DiasACT(fechaInicioCupon, fechaFinCupon)
            End If

            Return diasAnio
        End Function

    End Class
End Namespace

