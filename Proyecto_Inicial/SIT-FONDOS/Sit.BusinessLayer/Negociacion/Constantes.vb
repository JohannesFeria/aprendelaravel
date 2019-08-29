
'Constantes y Enumerados
Namespace MotorInversiones

    ''' <summary>
    ''' Constantes para el Calculo de Cupón e Interes Corrido
    ''' Los valores indicados aquí deben corresponder con los de la tabla "ParametrosGenerales" (filtrar campo Clasificacion = 'CuponBaseMensual')
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum BaseMensualCupon As Integer
        ''' <summary>
        ''' Meses de 30 Días
        ''' </summary>
        D_30 = 1

        ''' <summary>
        ''' Meses con el número de Días que le correspondan según sea el "Mes" y "Año"
        ''' </summary>
        D_ACT = 2
    End Enum

    ''' <summary>
    ''' Constantes para el Calculo de Cupón e Interes Corrido
    ''' Los valores indicados aquí deben corresponder con los de la tabla "ParametrosGenerales" (filtrar campo Clasificacion = 'CuponBaseAnual')
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum BaseAnualCupon As Integer
        ''' <summary>
        ''' Años de 360 Días
        ''' </summary>
        D_ANUAL_360 = 1

        ''' <summary>
        ''' Años de 365 Días
        ''' </summary>
        D_ANUAL_365 = 2

        ''' <summary>
        ''' Años con el número de Días que le correspondan según sea el "Año"
        ''' </summary>
        D_ANUAL_ACT = 3
    End Enum

    ''' <summary>
    ''' Constantes para el Calculo de Cupón e Interes Corrido
    ''' Los valores indicados aquí deben corresponder con los de la tabla "ParametrosGenerales" (filtrar campo Clasificacion = 'CuponBaseAnual')
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum BasePeriodoCupon As Integer
        ''' <summary>
        ''' Días que le correspondan a un Periodo del Año 360 (El Periodo puede ser Anual, Semestral, Trimestral, Mensual, etc.")
        ''' </summary>
        D_PERIODO_CUPON_360 = 4

        ''' <summary>
        ''' Días que le correspondan a un Periodo del Año ACT (El Periodo puede ser Anual, Semestral, Trimestral, Mensual, etc.")
        ''' </summary>
        D_PERIODO_CUPON_ACT = 5

    End Enum

    ''' <summary>
    ''' Tipo de Aplicación de la Tasa para calcular el FLUJO de Pagos
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TipoAplicacionTasa As Integer
        ''' <summary>
        ''' Cálculo basado en la aplicación de la Tasa de forma NOMINAL
        ''' Para calcular el Flujo, el Pago del Cupon se dividirá entre: 
        ''' (1 + YTM%) ^ (Dias-Flujo-Descuento / Dias-Periodo-Anual)
        ''' </summary>
        NOMINAL = 1

        ''' <summary>
        ''' Cálculo basado en la aplicación de la Tasa de forma EFECTIVA
        ''' Para calcular el Flujo, el Pago del Cupon se dividirá entre: 
        ''' ((1 + YTM% / Nro-Cupones-Anual) ^ (Nro-Cupones-Anual / Dias-Periodo-Anual)) ^ (Dias-Flujo-Descuento)
        ''' </summary>
        EFECTIVA = 2
    End Enum

    Public Class ReglasAdicionalesDeCalculo
        ''' <summary>
        ''' Útil para los instrumentos de Renta Fija marcados con Tipo Cupon "A DESCUENTO" (Certificados de Depósito, Papeles Comerciales, etc)
        ''' </summary>       
        Public Property NO_GENERA_INTERES_CUPON As Boolean = False

        ''' <summary>
        '''  
        ''' </summary>       
        Public Property ES_VALOR_DEL_MERCADO_EXTRANJERO As Boolean = False

        ''' <summary>
        '''Permite que el motor solo genere cálculos para los YTM Mayores a Cero
        ''' </summary>       
        Public Property VALIDAR_YTM_MAYOR_A_CERO As Boolean = False

        ''' <summary>
        '''OT12127: Permite que el motor negocie bonos TBill
        ''' </summary>       
        Public Property CALCULO_TBILL As Boolean = False

    End Class
End Namespace



