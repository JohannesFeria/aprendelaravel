Namespace MotorInversiones

    ''' <summary>
    ''' Contiene datos referentes a la Evaluacion de un Cupon de Renta Fija. 
    ''' </summary>
    Public Class CuponRentaFija

        ''' <summary>
        ''' Fecha de Inicio del Cupón de Renta Fija. 
        ''' En Bonos, esta fecha es equivalente a la Fecha Fin del Cupón Anterior. 
        ''' En un sentido más estricto el Cupón inicia a correr un día despues de esta fecha.
        ''' </summary>
        Public Property FechaInicio As Date = Now

        ''' <summary>
        ''' Fecha de Fin del Cupón. Es el día hasta el cual el Cupón corre.
        ''' </summary>
        Public Property FechaFin As Date = Now

        ''' <summary>
        ''' (%) de Amortización (del Valor Nominal) a efectuar durante el Cupon en Evaluación
        ''' </summary>
        Public Property PorcAmortizacion As Decimal = 0

        ''' <summary>
        ''' Monto a Amoritizar durante el Cupon en Evaluación
        ''' </summary>
        Public Property Amortizacion As Decimal = 0

        ''' <summary>
        ''' Saldo Nominal al iniciar el Cupón. 
        ''' Como concepto sería el Monto que queda pendiente que nos reembolsen (por haber adquirido el Instrumento(Bono, CDP, etc.)) al inicio del Cupon
        ''' </summary>    
        Public Property SaldoNominalInicial As Decimal = 0

        ''' <summary>
        ''' Saldo Nominal al término del Cupón. Se obtiene de sumar (SaldoNominalInicial + Amortizacion)
        ''' Como concepto sería el Monto que queda pendiente que nos reembolsen (por haber adquirido el Instrumento(Bono, CDP, etc.)) al término del Cupon
        ''' </summary>   
        Public Property SaldoNominalFinal As Decimal = 0

        ''' <summary>
        ''' Es el Interes obtenido por el Cupón en Evaluación.
        ''' Se obtienen de multiplicar (SaldoNominalInicial * TasaPeriodoCupon), donde TasaPeriodoCupon = TasaCupon / NroCuponesAlAño
        ''' </summary>   
        Public Property InteresCupon As Decimal = 0

        ''' <summary>
        ''' Es el Pago que se realiza al Vencimiento o Término del Cupon
        ''' </summary>    
        Public Property PagoCupon As Decimal = 0

        ''' <summary>
        ''' Es el Flujo de Descuento de los cupones pendientes
        ''' </summary>    
        Public Property FlujoDescuento As Decimal = 0





        ''' <summary>
        ''' Auxiliar de Cálculo: Cantidad de Días entre la fecha de Evaluación 
        ''' de la Negociación y la Fecha de Término de un Cupon pendiente de Vencimiento
        ''' </summary>
        Public Property DiasFlujoDescuento As Integer = 0

        ''' <summary>
        ''' Auxiliar de Cálculo: 
        ''' </summary>
        Public Property DiasPeriodoAnual As Integer = 0

        ''' <summary>
        ''' Auxiliar de Cálculo: 
        ''' </summary>
        Public Property DiasPeriodoCupon As Integer = 0

    End Class


End Namespace