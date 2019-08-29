Namespace MotorInversiones

    ''' <summary>
    ''' Entidad que contiene los datos de un Valor de Renta Fija (requeridos para una Negociación)
    ''' </summary>
    Public Class ValorRentaFija

        Public Property CodigoNemonico As String = ""

        ''' <summary>
        ''' Precio o Valor de Cada Unidad del Instrumento Financiero
        ''' </summary>  
        Public Property PrecioUnitario As Decimal = 0

        Private _NroCuponesAnual As Decimal = 1

        Public Property NroCuponesAnual() As Decimal
            Get
                Return _NroCuponesAnual
            End Get
            Set(ByVal value As Decimal)
                If value <= 0 Then
                    Throw New Exception("El número de Cupones al año debe ser mayor a CERO")
                End If

                _NroCuponesAnual = value
            End Set
        End Property

        ''' <summary>
        ''' Tasa de Interes generada por los Cupones durate un Año
        ''' </summary>  
        Public Property TasaCuponAnual As Decimal = 0

        ''' <summary>
        ''' Tasa de Interes para un cupón regular. Se considera cupón regular si 1 Año se puede
        ''' contener un número concreto de estos cupones ejemp.: Anual, Semestral, Cuatrimestral, Trimestral, Bimestral, Mensual.
        ''' Su fórmula es: TasaCuponPeriodo = TasaCuponAnual / NroCuponesAnual
        ''' </summary>  
        Public ReadOnly Property TasaCuponPeriodoRegular As Decimal
            Get
                Return TasaCuponAnual / NroCuponesAnual
            End Get
        End Property


        ''' <summary>
        ''' Determina el número de días al Mes utilizado como base para los cálculos
        ''' </summary>
        Public Property BaseMensualCupon As BaseMensualCupon = BaseMensualCupon.D_ACT

        ''' <summary>
        ''' Determina el número de días al Año utilizado como base para los cálculos
        ''' </summary>
        Public Property BaseAnualCupon As BaseAnualCupon = BaseAnualCupon.D_ANUAL_360



        'Public Property BaseMensualIC As BaseMensualCupon = BaseMensualCupon.D_ACT

        ''' <summary>
        ''' Determina el número de días al Año utilizado como base para un Cupon
        ''' </summary>
        Public Property BasePeriodoCupon As BasePeriodoCupon = BasePeriodoCupon.D_PERIODO_CUPON_360

    End Class

End Namespace