Imports System.Collections.Generic

Namespace MotorInversiones


    ''' <summary>
    ''' Entidad que contiene los campos requeridos para una Negociación de Valor de Renta Fija
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NegociacionRentaFija
        Implements ICloneable

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Return Me.MemberwiseClone()
        End Function
        'Public Function Clonar() As Object
        '    Return Me.MemberwiseClone()
        'End Function

#Region "Principalmente Atributos/Propiedades de Configuracion"

        ''' <summary>
        ''' Valor (Instrumento Financiero) de Renta Fija Negociado
        ''' </summary>    
        Public Property ValorRentaFijaNegociado As ValorRentaFija

        Public Property Cupones As List(Of CuponRentaFija)

        ''' <summary>
        ''' Cantidad Unidades Negociadas
        ''' </summary>
        ''' <value></value>    
        Public Property CantidadUnidadesNegociadas As Decimal = 0

        ''' <summary>
        ''' Valor Nominal Negociado
        ''' </summary> 
        Public ReadOnly Property ValorNominal As Decimal
            Get
                If Me.ValorRentaFijaNegociado IsNot Nothing Then
                    Return Me.CantidadUnidadesNegociadas * Me.ValorRentaFijaNegociado.PrecioUnitario
                End If

                Return 0
            End Get
        End Property

        ''' <summary>
        ''' Porcentaje (%) YTM Anual ingresado en la Negociación (Tambien denominado TIR)
        ''' </summary>    
        Public Property YTM As Decimal = 0

        ''' <summary>
        ''' Tipo de Aplicación de la Tasa para calcular el FLUJO de Pagos, puede ser NOMINAL o EFECTIVA
        ''' </summary>
        Public Property AplicacionTasa As TipoAplicacionTasa = TipoAplicacionTasa.NOMINAL

        ''' <summary>
        ''' Es la fecha para la cual se hace la Evaluación del Instrumento (Usualmente se refiere a la fecha efetiva de la Liquidación de la Negociación).
        ''' Principalmente se desea saber el "Valor Actual (Valor Presente)" a esta fecha así como el "Interes Corrido", entre otros datos
        ''' </summary>
        Public Property FechaEvaluacion As Date = DateTime.MinValue

        Private _Reglas As ReglasAdicionalesDeCalculo

        ''' <summary>
        ''' Reglas Adicionales que permiten modificar el comportamiento de los cálculos
        ''' </summary>    
        Public ReadOnly Property ReglasAdicionales As ReglasAdicionalesDeCalculo
            Get
                Return Me._Reglas
            End Get
        End Property

#End Region

#Region "Principalmente Atributos/Propiedades de valores Resultantes"

        Public Property ValorActual As Decimal = 0
        Public Property InteresCorrido As Decimal = 0
        Public Property ValorPrincipal As Decimal = 0

        Public Property InteresCorridoAjustado As Decimal = 0

        ''' <summary>
        ''' Es la relación entre el "Valor Actual" y el "Saldo Nominal Inicial del Bono Vigente" (PrecioSucio = Valor Actual / SaldoNominal Inicial del Cupon Vigente)
        ''' </summary>      
        Public Property PrecioLimpio As Decimal = 0

        ''' <summary>
        ''' Es la relación entre el "Valor Actual" y el "Saldo Nominal Inicial del Bono Vigente" (PrecioSucio = Valor Actual / Saldo Nominal Inicial del Cupon Vigente)
        ''' </summary>      
        Public Property PrecioSucio As Decimal = 0

        Public Property VAC_Emision As Decimal = 1

        Public Property VAC_Evaluacion As Decimal = 1

        Private _CuponVigente As CuponRentaFija

        ''' <summary>
        ''' Cupón Vigente a la Fecha de Evaluación Indicada (Se genera como resultante del proceso)
        ''' </summary>    
        Public ReadOnly Property CuponVigente As CuponRentaFija
            Get
                Return Me._CuponVigente
            End Get
        End Property



        ''' <summary>
        ''' Auxiliar de Cálculo: Dias del Interes Corrido
        ''' </summary>
        Public Property DiasIC As Integer = 0

        ''' <summary>
        ''' Auxiliar de Cálculo: Dias del Periodo del Interes Corrido
        ''' </summary>
        Public Property DiasPeriodoIC As Integer = 0
#End Region


#Region "Métodos"

        Public Sub New()
            Me._Reglas = New ReglasAdicionalesDeCalculo
        End Sub

        Public Sub CalcularMapaDePagosEnCuponera()
            If Me.ValorNominal <= 0 Then
                Throw New Exception("El Valor Nominal negociado debe ser mayor a CERO (Función CalcularMapaDePagosEnCuponera)")
            End If
            If Me.ValorRentaFijaNegociado Is Nothing Then
                Throw New Exception("Valor de Renta Fija no encontrado (Función CalcularMapaDePagosEnCuponera)")
            End If

            If Me.ValorRentaFijaNegociado.TasaCuponAnual <= 0 Then
                If Me._Reglas.NO_GENERA_INTERES_CUPON Then
                    If Me.ValorRentaFijaNegociado.TasaCuponAnual < 0 Then Throw New Exception("La Tasa del Cupon (Anual) debe ser Mayor O Igual a CERO (Función CalcularMapaDePagosEnCuponera)")
                Else 'Si GENERA INTERESES POR CADA CUPON
                    Throw New Exception("La Tasa del Cupon (Anual) debe ser mayor a CERO (Función CalcularMapaDePagosEnCuponera)")
                End If
            End If
            If Me.Cupones Is Nothing Then
                Throw New Exception("Configuración de Cupones no encontrada (Función CalcularMapaDePagosEnCuponera)")
            End If
            If Me.Cupones.Count = 0 Then
                Throw New Exception("Se requiere por lo menos tener un (1) cupón para realizar el proceso (Función CalcularMapaDePagosEnCuponera)")
            End If

            Dim saldoNominalFinalAnterior As Decimal = Me.ValorNominal 'Ayudará a inicializar al primer Cupón

            For Each c As CuponRentaFija In Cupones
                ' Realizamos los cálculos para el Cupón Actual
                c.Amortizacion = c.PorcAmortizacion * Me.ValorNominal
                c.SaldoNominalInicial = saldoNominalFinalAnterior 'El Saldo Inicial siempre será igual al Saldo Final Anterior
                c.SaldoNominalFinal = c.SaldoNominalInicial - c.Amortizacion

                ' Calculamos los días del periodo ANUAL
                c.DiasPeriodoAnual = Utiles.DiasSegunBaseAnualCupon(Me.ValorRentaFijaNegociado.BaseAnualCupon,
                                                              Me.FechaEvaluacion)
                ' Calculamos los días del periodo del CUPON
                c.DiasPeriodoCupon = Utiles.DiasSegunBasePeriodo(Me.ValorRentaFijaNegociado.BasePeriodoCupon,
                                                                                     c.FechaInicio,
                                                                                     c.FechaFin)
                If Me._Reglas.NO_GENERA_INTERES_CUPON Then
                    c.InteresCupon = 0
                Else
                    If Me.AplicacionTasa = TipoAplicacionTasa.NOMINAL Then
                        c.InteresCupon = c.SaldoNominalInicial * Me.ValorRentaFijaNegociado.TasaCuponAnual * (c.DiasPeriodoCupon / c.DiasPeriodoAnual)
                    Else 'Me.AplicacionTasa = TipoAplicacionTasa.EFECTIVA
                        c.InteresCupon = c.SaldoNominalInicial * Me.ValorRentaFijaNegociado.TasaCuponPeriodoRegular
                    End If
                End If

                c.PagoCupon = c.InteresCupon + c.Amortizacion

                'Necesario para la siguiente Iteración
                saldoNominalFinalAnterior = c.SaldoNominalFinal
            Next

            'Inicializamos el Cupón Vigente
            Me._CuponVigente = Nothing
        End Sub

        Public Sub CalcularDatosDelCuponVigente()
            If Me.Cupones Is Nothing Then
                Throw New Exception("Configuración de Cupones no encontrada (Función CalcularDatosDelCuponVigente)")
            End If
            If Me.Cupones.Count = 0 Then
                Throw New Exception("Se requiere por lo menos tener un (1) cupón para realizar el proceso (Función CalcularDatosDelCuponVigente)")
            End If
            If Me.VAC_Evaluacion <= 0 Or Me.VAC_Emision <= 0 Then
                Throw New Exception("Los valores VAC Actual y VAC Emisión deben ser mayores a CERO (Función CalcularDatosDelCuponVigente)")
            End If

            'Inicializamos y luego buscamos el Cupón Vigente
            Me._CuponVigente = Nothing

            For Each c As CuponRentaFija In Me.Cupones
                If c.FechaInicio <= Me.FechaEvaluacion And Me.FechaEvaluacion < c.FechaFin Then
                    Me._CuponVigente = c
                    Exit For
                End If
            Next

            If Me._CuponVigente Is Nothing Then
                Throw New Exception("Debe ingresar una [Fecha de Evaluación (Liquidación)] que corresponde a algún cupon configurado (Función CalcularDatosDelCuponVigente)")
            End If

            Me.DiasIC = Utiles.DiasSegunBaseMensualCupon(Me.ValorRentaFijaNegociado.BaseMensualCupon,
                                                                    Me._CuponVigente.FechaInicio,
                                                                    Me.FechaEvaluacion)
            Me.DiasPeriodoIC = Me._CuponVigente.DiasPeriodoCupon

            'Se ha considerado el factor VAC (Actual/Emisión) para los instrumentos que lo requieran
            Me.InteresCorrido = Me._CuponVigente.InteresCupon * (Me.DiasIC / Me.DiasPeriodoIC)

            Me.InteresCorridoAjustado = (Me.VAC_Evaluacion / Me.VAC_Emision) * Me.InteresCorrido
        End Sub

        Public Sub CalcularDatosDelFlujoDeCuponesBasadoEnTIR()
            If Me._Reglas.VALIDAR_YTM_MAYOR_A_CERO And (Me.YTM <= 0 Or Me.YTM > 100) Then
                Throw New Exception("El parámetro [(%) YTM] debe ser mayor a CERO y menor que 100 (Función CalcularDatosDelFlujoDeCuponesBasadoEnTIR)")
            End If
            If Me._CuponVigente Is Nothing Then 'Utilice la funcion CalcularDatosDelCuponVigente() previamente
                Throw New Exception("Debe primeramente realizar los Calculos del [Cupon Vigente] (Función CalcularDatosDelFlujoDeCuponesBasadoEnTIR)")
            End If

            'Ahora calculamos los valores Resultantes de Flujo
            Me.ValorActual = Me.FxCalculoValorActual(Me.YTM) ' Paso 01
            Me.ValorPrincipal = (Me.ValorActual - Me.InteresCorrido) ' Paso 02
            Me.PrecioLimpio = Me.ValorPrincipal / Me._CuponVigente.SaldoNominalInicial ' Paso 03
            Me.PrecioSucio = Me.ValorActual / Me._CuponVigente.SaldoNominalInicial ' Paso 04
        End Sub

        Public Sub CalcularDatosDelFlujoDeCuponesBasadoEnPrecioSucio()
            If Me.PrecioSucio <= 0 Then
                Throw New Exception("El parámetro [(%) YTM] debe ser mayor a CERO y menor que 100 (Función CalcularDatosDelFlujoDeCuponesBasadoEnPrecioSucio)")
            End If
            If Me._CuponVigente Is Nothing Then 'Utilice la funcion CalcularDatosDelCuponVigente() previamente
                Throw New Exception("Debe primeramente realizar los Calculos del [Cupon Vigente] (Función CalcularDatosDelFlujoDeCuponesBasadoEnPrecioSucio)")
            End If

            Me.ValorActual = Me.PrecioSucio * Me._CuponVigente.SaldoNominalInicial ' Paso 01
            Me.ValorPrincipal = (Me.ValorActual - Me.InteresCorrido) ' Paso 02
            Me.PrecioLimpio = Me.ValorPrincipal / Me._CuponVigente.SaldoNominalInicial ' Paso 03

            ' Paso 04: El Paso más complejo. Se debe calcular el TIR en base a los demás parametros generados por el MOTOR. 
            '   Como no se puede despejar las ecuación del ValorActualBono a la Inversa (Por ser sumatoria de Ecuaciónes de Grado Variable y Decimal)
            '   Entonces pasamos utilizar la GRAFICA DE LA FUNCION (Es decir, utilizar la función a la inversa)
            '   Se implementa un algoritmo de BSP (Binary Space Partition) para alcanzar la TIR aproximada
            '   Esto hará que varie la TIR en la función hasta alcanzar el ValorActual (Resultante) deseado
            '   Nota: Este proceso es posible ya que se tiene la función exponencial con solo el TIR por resolver
            Me.YTM = DeterminarTirMedianteAlgoritmoBSP() ' Asignamos el TIR utilizado para el proceso en señal de que funcionó correctamente
        End Sub

        Public Sub CalcularDatosDelFlujoDeCuponesBasadoEnPrecioLimpio()
            If Me.PrecioSucio <= 0 Then
                Throw New Exception("El parámetro [(%) YTM] debe ser mayor a CERO y menor que 100 (Función CalcularDatosDelFlujoDeCuponesBasadoEnPrecioSucio)")
            End If
            If Me._CuponVigente Is Nothing Then 'Utilice la funcion CalcularDatosDelCuponVigente() previamente
                Throw New Exception("Debe primeramente realizar los Calculos del [Cupon Vigente] (Función CalcularDatosDelFlujoDeCuponesBasadoEnPrecioSucio)")
            End If

            Me.ValorActual = Me.PrecioLimpio * Me._CuponVigente.SaldoNominalInicial + Me.InteresCorrido ' Paso 01
            Me.ValorPrincipal = (Me.ValorActual - Me.InteresCorrido) ' Paso 02

            ' Paso 04: El Paso más complejo. Se debe calcular el TIR en base a los demás parametros generados por el MOTOR. 
            '   Como no se puede despejar las ecuación del ValorActualBono a la Inversa (Por ser sumatoria de Ecuaciónes de Grado Variable y Decimal)
            '   Entonces pasamos utilizar la GRAFICA DE LA FUNCION (Es decir, utilizar la función a la inversa)
            '   Se implementa un algoritmo de BSP (Binary Space Partition) para alcanzar la TIR aproximada
            '   Esto hará que varie la TIR en la función hasta alcanzar el ValorActual (Resultante) deseado
            '   Nota: Este proceso es posible ya que se tiene la función exponencial con solo el TIR por resolver
            Me.YTM = DeterminarTirMedianteAlgoritmoBSP() ' Asignamos el TIR utilizado para el proceso en señal de que funcionó correctamente
        End Sub


        ''' <summary>
        ''' Calcula la Sumatoria de los Fujos de Cupones para componer el ValorActual, su formula general es: 
        ''' ValorActual = F(x) = Sum ( PagoCupon[i] / ProporcionPago[i] )
        ''' , Donde x es el TIR
        ''' , Donde 1 <= i <= NroCuponesPendientes
        ''' , además ProporcionPago[i] = D(x) => Funcion del Divisor Basada en TIR según AplicacionTasa y/o ReglasAdicionales
        ''' </summary>
        ''' <param name="xTir">TIR utilizada (Variable X en la función) </param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function FxCalculoValorActual(ByVal xTir As Decimal) As Decimal ' El método debe ser Private pues no tiene sentido llamarlo desde afuera de la clase

            Dim sumaFlujoDescuento As Decimal = 0

            'Calculamos el FLUJO DE DESCUENTOS para obtener el Valor Actual (Valor Principal)
            For Each c As CuponRentaFija In Me.Cupones
                'Regla: Todo Cupón cuya fecha de término sea mayor a la "Fecha de Evaluación" tiene "Flujo de Descuento"
                If Me.FechaEvaluacion < c.FechaFin Then
                    c.DiasFlujoDescuento = Utiles.DiasSegunBaseMensualCupon(Me.ValorRentaFijaNegociado.BaseMensualCupon,
                                                                            Me.FechaEvaluacion,
                                                                            c.FechaFin)

                    Dim proporcionAnualFlujo As Decimal = c.DiasFlujoDescuento / c.DiasPeriodoAnual
                    Dim proporcionPago, tasaDeterminante As Decimal
                    'OT12127 | 2019-07-16 | rcolonia | Zoluxiones | Agregar Negociación TBILL
                    If Not Me._Reglas.CALCULO_TBILL Then
                        'Calculamos la PROPORCION del Pago Cupon
                        If Me.AplicacionTasa = TipoAplicacionTasa.EFECTIVA Then 'EL MÉTODO DE CÁLCULO MÁS UTILIZADO
                            If ReglasAdicionales.ES_VALOR_DEL_MERCADO_EXTRANJERO Then
                                'Tasa Diaria (Doble Exponente)
                                tasaDeterminante = Math.Pow(1 + xTir / ValorRentaFijaNegociado.NroCuponesAnual, ValorRentaFijaNegociado.NroCuponesAnual / c.DiasPeriodoAnual)
                                proporcionPago = Math.Pow(tasaDeterminante, c.DiasFlujoDescuento)
                            Else 'CÁLCULO PARA EFECTIVA MERCADO-LOCAL (El más utilizado)
                                tasaDeterminante = 1 + xTir
                                proporcionPago = Math.Pow(tasaDeterminante, proporcionAnualFlujo)
                            End If
                        Else 'Me.AplicacionTasa = TipoAplicacionTasa.NOMINAL
                            proporcionPago = 1 + xTir * proporcionAnualFlujo
                        End If

                        'Calculamos el "Flujo de Descuento" del Cupon
                        c.FlujoDescuento = c.PagoCupon / proporcionPago
                    Else
                        c.FlujoDescuento = c.PagoCupon * (1 - xTir * proporcionAnualFlujo)
                    End If

                    sumaFlujoDescuento += c.FlujoDescuento
                Else
                    c.DiasPeriodoAnual = 0
                    c.DiasFlujoDescuento = 0
                    c.FlujoDescuento = 0
                End If
            Next

            Return sumaFlujoDescuento 'Retornamos el VALOR ACTUAL obtenido para este cálculo
        End Function

        ''' <summary>
        ''' Determina el TIR utilizando la funcion FxCalculoFlujoCupon y el Algoritmo BSP (Binary Space Partition) para lograr eficiencia
        ''' NOTA: Se calcula que se encuentra el "Valor Adecuado" en menos de 60 Iteraciones logrando una precisión de 10 decimales
        ''' </summary>
        Private Function DeterminarTirMedianteAlgoritmoBSP() As Decimal
            Dim tirMaximo As Decimal = 100 / 100, tirMinimo As Decimal = 0 '100 / 100 = 100%
            'El TIR es inversamente proporcional en la Función
            Dim valorActualMinimo As Decimal = Me.FxCalculoValorActual(xTir:=tirMaximo) ' Obtenemos el Límite Inferior (Y en la gráfica)
            Dim valorActualMaximo As Decimal = Me.FxCalculoValorActual(xTir:=tirMinimo) ' Obtenemos el Límite Superior (Y en la gráfica) 

            If Not (valorActualMinimo <= Me.ValorActual And Me.ValorActual <= valorActualMaximo) Then
                Throw New Exception("El Valor Actual proporcionado no corresponde a ningún valor de TIR entre 0% y 100% (Función DeterminarTirMedianteAlgoritmoBSP)")
            End If

            Dim tirMedio As Decimal, valorActualMedio As Decimal, margenError As Decimal
            Dim iteraciones As Integer = 0, continuarBusquedaTir As Boolean = True

            Const MAX_ITERACIONES As Integer = 500 'CONFIGURACION DE MAXIMAS ITERACIONES DEL BUCLE
            Const MAX_MARGEN_ERROR_ACPTABLE As Decimal = 0.00000001 'CONFIGURACION DE MAXIMO MARGEN DE ERROR ACEPTABLE (7 DECIMALES)

            While continuarBusquedaTir
                'Determinamos el Valor Actual del Tir Medio
                tirMedio = (tirMaximo + tirMinimo) / 2 'Obtenemos el TIR medio (Algoritmo BSP)
                valorActualMedio = Me.FxCalculoValorActual(xTir:=tirMedio)

                'Paso A) Evaluamos si el ValorActual buscado está en el "Sector Medio <-> Inferior" (A valorActualMaximo pues inversamente proporcional la funcion)
                If valorActualMedio <= Me.ValorActual And Me.ValorActual <= valorActualMaximo Then
                    'Si cumple esta condición el "Sector Medio Superior" del TIR sería el adecuado
                    tirMaximo = tirMedio 'El Tir Máximo varia
                    valorActualMinimo = valorActualMedio
                Else
                    'B) Deducimos que el ValorActual buscado está en el "Sector Medio <-> Superior"                              
                    tirMinimo = tirMedio 'El Tir Mínimo varia
                    valorActualMaximo = valorActualMedio
                End If

                'Hasta aqui el Rango elegido está limitado por tirMinimo y tirMaximo
                iteraciones += 1

                margenError = Math.Abs(valorActualMaximo - Me.ValorActual)
                If iteraciones > MAX_ITERACIONES Or margenError <= MAX_MARGEN_ERROR_ACPTABLE Then
                    continuarBusquedaTir = False
                End If

            End While

            ' Podriamos asignar el valor directamente a Me.YTM pero lo mejor 
            ' será retornarlo como indicador de que el proceso terminó correctamente
            Return tirMinimo
        End Function

#End Region

        Sub CalcularDatosDeNegociacion()
            Throw New NotImplementedException
        End Sub

    End Class


End Namespace