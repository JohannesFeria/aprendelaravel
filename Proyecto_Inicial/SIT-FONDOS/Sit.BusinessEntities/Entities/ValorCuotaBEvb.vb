Public Class ValorCuotaBEvb
    Private _CodigoPortafolioSBS As String
    Public Property CodigoPortafolioSBS() As String
        Get
            Return _CodigoPortafolioSBS
        End Get
        Set(ByVal value As String)
            _CodigoPortafolioSBS = value
        End Set
    End Property

    Private _CodigoSerie As String
    Public Property CodigoSerie() As String
        Get
            Return _CodigoSerie
        End Get
        Set(ByVal value As String)
            _CodigoSerie = value
        End Set
    End Property

    Private _FechaProceso As Decimal
    Public Property FechaProceso() As Decimal
        Get
            Return _FechaProceso
        End Get
        Set(ByVal value As Decimal)
            _FechaProceso = value
        End Set
    End Property

    Private _InversionesT1 As Decimal
    Public Property InversionesT1() As Decimal
        Get
            Return _InversionesT1
        End Get
        Set(ByVal value As Decimal)
            _InversionesT1 = value
        End Set
    End Property

    Private _VentasVencimientos As Decimal
    Public Property VentasVencimientos() As Decimal
        Get
            Return _VentasVencimientos
        End Get
        Set(ByVal value As Decimal)
            _VentasVencimientos = value
        End Set
    End Property

    Private _Rentabilidad As Decimal
    Public Property Rentabilidad() As Decimal
        Get
            Return _Rentabilidad
        End Get
        Set(ByVal value As Decimal)
            _Rentabilidad = value
        End Set
    End Property

    Private _ValForwards As Decimal
    Public Property ValForwards() As Decimal
        Get
            Return _ValForwards
        End Get
        Set(ByVal value As Decimal)
            _ValForwards = value
        End Set
    End Property

    Private _CajaPreCierre As Decimal
    Public Property CajaPreCierre() As Decimal
        Get
            Return _CajaPreCierre
        End Get
        Set(ByVal value As Decimal)
            _CajaPreCierre = value
        End Set
    End Property

    Private _CXCVentaTitulo As Decimal
    Public Property CXCVentaTitulo() As Decimal
        Get
            Return _CXCVentaTitulo
        End Get
        Set(ByVal value As Decimal)
            _CXCVentaTitulo = value
        End Set
    End Property

    Private _OtrasCXC As Decimal
    Public Property OtrasCXC() As Decimal
        Get
            Return _OtrasCXC
        End Get
        Set(ByVal value As Decimal)
            _OtrasCXC = value
        End Set
    End Property

    Private _OtrasCXCExclusivos As Decimal
    Public Property OtrasCXCExclusivos() As Decimal
        Get
            Return _OtrasCXCExclusivos
        End Get
        Set(ByVal value As Decimal)
            _OtrasCXCExclusivos = value
        End Set
    End Property

    Private _CXCPreCierre As Decimal
    Public Property CXCPreCierre() As Decimal
        Get
            Return _CXCPreCierre
        End Get
        Set(ByVal value As Decimal)
            _CXCPreCierre = value
        End Set
    End Property

    Private _CXPCompraTitulo As Decimal
    Public Property CXPCompraTitulo() As Decimal
        Get
            Return _CXPCompraTitulo
        End Get
        Set(ByVal value As Decimal)
            _CXPCompraTitulo = value
        End Set
    End Property

    Private _OtrasCXP As Decimal
    Public Property OtrasCXP() As Decimal
        Get
            Return _OtrasCXP
        End Get
        Set(ByVal value As Decimal)
            _OtrasCXP = value
        End Set
    End Property

    Private _OtrasCXPExclusivos As Decimal
    Public Property OtrasCXPExclusivos() As Decimal
        Get
            Return _OtrasCXPExclusivos
        End Get
        Set(ByVal value As Decimal)
            _OtrasCXPExclusivos = value
        End Set
    End Property

    Private _CXPPreCierre As Decimal
    Public Property CXPPreCierre() As Decimal
        Get
            Return _CXPPreCierre
        End Get
        Set(ByVal value As Decimal)
            _CXPPreCierre = value
        End Set
    End Property

    Private _OtrosGastos As Decimal
    Public Property OtrosGastos() As Decimal
        Get
            Return _OtrosGastos
        End Get
        Set(ByVal value As Decimal)
            _OtrosGastos = value
        End Set
    End Property

    Private _OtrosGastosExclusivos As Decimal
    Public Property OtrosGastosExclusivos() As Decimal
        Get
            Return _OtrosGastosExclusivos
        End Get
        Set(ByVal value As Decimal)
            _OtrosGastosExclusivos = value
        End Set
    End Property

    Private _OtrosIngresos As Decimal
    Public Property OtrosIngresos() As Decimal
        Get
            Return _OtrosIngresos
        End Get
        Set(ByVal value As Decimal)
            _OtrosIngresos = value
        End Set
    End Property

    Private _OtrosIngresosExclusivos As Decimal
    Public Property OtrosIngresosExclusivos() As Decimal
        Get
            Return _OtrosIngresosExclusivos
        End Get
        Set(ByVal value As Decimal)
            _OtrosIngresosExclusivos = value
        End Set
    End Property

    Private _ValPatriPreCierre1 As Decimal
    Public Property ValPatriPreCierre1() As Decimal
        Get
            Return _ValPatriPreCierre1
        End Get
        Set(ByVal value As Decimal)
            _ValPatriPreCierre1 = value
        End Set
    End Property

    Private _ComisionSAFM As Decimal
    Public Property ComisionSAFM() As Decimal
        Get
            Return _ComisionSAFM
        End Get
        Set(ByVal value As Decimal)
            _ComisionSAFM = value
        End Set
    End Property

    Private _ValPatriPreCierre2 As Decimal
    Public Property ValPatriPreCierre2() As Decimal
        Get
            Return _ValPatriPreCierre2
        End Get
        Set(ByVal value As Decimal)
            _ValPatriPreCierre2 = value
        End Set
    End Property

    Private _ValCuotaPreCierre As Decimal
    Public Property ValCuotaPreCierre() As Decimal
        Get
            Return _ValCuotaPreCierre
        End Get
        Set(ByVal value As Decimal)
            _ValCuotaPreCierre = value
        End Set
    End Property

    Private _ValCuotaPreCierreVal As Decimal
    Public Property ValCuotaPreCierreVal() As Decimal
        Get
            Return _ValCuotaPreCierreVal
        End Get
        Set(ByVal value As Decimal)
            _ValCuotaPreCierreVal = value
        End Set
    End Property

    Private _AportesCuotas As Decimal
    Public Property AportesCuotas() As Decimal
        Get
            Return _AportesCuotas
        End Get
        Set(ByVal value As Decimal)
            _AportesCuotas = value
        End Set
    End Property

    Private _AportesValores As Decimal
    Public Property AportesValores() As Decimal
        Get
            Return _AportesValores
        End Get
        Set(ByVal value As Decimal)
            _AportesValores = value
        End Set
    End Property

    Private _RescateCuotas As Decimal
    Public Property RescateCuotas() As Decimal
        Get
            Return _RescateCuotas
        End Get
        Set(ByVal value As Decimal)
            _RescateCuotas = value
        End Set
    End Property

    Private _RescateValores As Decimal
    Public Property RescateValores() As Decimal
        Get
            Return _RescateValores
        End Get
        Set(ByVal value As Decimal)
            _RescateValores = value
        End Set
    End Property

    Private _Caja As Decimal
    Public Property Caja() As Decimal
        Get
            Return _Caja
        End Get
        Set(ByVal value As Decimal)
            _Caja = value
        End Set
    End Property

    Private _CXCVentaTituloCierre As Decimal
    Public Property CXCVentaTituloCierre() As Decimal
        Get
            Return _CXCVentaTituloCierre
        End Get
        Set(ByVal value As Decimal)
            _CXCVentaTituloCierre = value
        End Set
    End Property

    Private _OtrosCXCCierre As Decimal
    Public Property OtrosCXCCierre() As Decimal
        Get
            Return _OtrosCXCCierre
        End Get
        Set(ByVal value As Decimal)
            _OtrosCXCCierre = value
        End Set
    End Property

    Private _OtrosCXCExclusivoCierre As Decimal
    Public Property OtrosCXCExclusivoCierre() As Decimal
        Get
            Return _OtrosCXCExclusivoCierre
        End Get
        Set(ByVal value As Decimal)
            _OtrosCXCExclusivoCierre = value
        End Set
    End Property

    Private _CXCCierre As Decimal
    Public Property CXCCierre() As Decimal
        Get
            Return _CXCCierre
        End Get
        Set(ByVal value As Decimal)
            _CXCCierre = value
        End Set
    End Property

    Private _CXPCompraTituloCierre As Decimal
    Public Property CXPCompraTituloCierre() As Decimal
        Get
            Return _CXPCompraTituloCierre
        End Get
        Set(ByVal value As Decimal)
            _CXPCompraTituloCierre = value
        End Set
    End Property

    Private _OtrasCXPCierre As Decimal
    Public Property OtrasCXPCierre() As Decimal
        Get
            Return _OtrasCXPCierre
        End Get
        Set(ByVal value As Decimal)
            _OtrasCXPCierre = value
        End Set
    End Property

    Private _OtrasCXPExclusivoCierre As Decimal
    Public Property OtrasCXPExclusivoCierre() As Decimal
        Get
            Return _OtrasCXPExclusivoCierre
        End Get
        Set(ByVal value As Decimal)
            _OtrasCXPExclusivoCierre = value
        End Set
    End Property

    Private _CXPCierre As Decimal
    Public Property CXPCierre() As Decimal
        Get
            Return _CXPCierre
        End Get
        Set(ByVal value As Decimal)
            _CXPCierre = value
        End Set
    End Property

    Private _OtrosGastosCierre As Decimal
    Public Property OtrosGastosCierre() As Decimal
        Get
            Return _OtrosGastosCierre
        End Get
        Set(ByVal value As Decimal)
            _OtrosGastosCierre = value
        End Set
    End Property

    Private _OtrosGastosExclusivosCierre As Decimal
    Public Property OtrosGastosExclusivosCierre() As Decimal
        Get
            Return _OtrosGastosExclusivosCierre
        End Get
        Set(ByVal value As Decimal)
            _OtrosGastosExclusivosCierre = value
        End Set
    End Property

    Private _OtrosIngresosCierre As Decimal
    Public Property OtrosIngresosCierre() As Decimal
        Get
            Return _OtrosIngresosCierre
        End Get
        Set(ByVal value As Decimal)
            _OtrosIngresosCierre = value
        End Set
    End Property

    Private _OtrosIngresosExclusivosCierre As Decimal
    Public Property OtrosIngresosExclusivosCierre() As Decimal
        Get
            Return _OtrosIngresosExclusivosCierre
        End Get
        Set(ByVal value As Decimal)
            _OtrosIngresosExclusivosCierre = value
        End Set
    End Property

    Private _ValPatriCierreCuota As Decimal
    Public Property ValPatriCierreCuota() As Decimal
        Get
            Return _ValPatriCierreCuota
        End Get
        Set(ByVal value As Decimal)
            _ValPatriCierreCuota = value
        End Set
    End Property

    Private _ValPatriCierreValores As Decimal
    Public Property ValPatriCierreValores() As Decimal
        Get
            Return _ValPatriCierreValores
        End Get
        Set(ByVal value As Decimal)
            _ValPatriCierreValores = value
        End Set
    End Property

    Private _ValCuotaCierre As Decimal
    Public Property ValCuotaCierre() As Decimal
        Get
            Return _ValCuotaCierre
        End Get
        Set(ByVal value As Decimal)
            _ValCuotaCierre = value
        End Set
    End Property

    Private _ValCuotaValoresCierre As Decimal
    Public Property ValCuotaValoresCierre() As Decimal
        Get
            Return _ValCuotaValoresCierre
        End Get
        Set(ByVal value As Decimal)
            _ValCuotaValoresCierre = value
        End Set
    End Property

    Private _InversionesSubTotal As Decimal
    Public Property InversionesSubTotal() As Decimal
        Get
            Return _InversionesSubTotal
        End Get
        Set(ByVal value As Decimal)
            _InversionesSubTotal = value
        End Set
    End Property

    Private _InversionesSubTotalSerie As Decimal
    Public Property InversionesSubTotalSerie() As Decimal
        Get
            Return _InversionesSubTotalSerie
        End Get
        Set(ByVal value As Decimal)
            _InversionesSubTotalSerie = value
        End Set
    End Property
    'OT10916 - 26/10/2017 - Ian Pastor M. Agregar campo "CXCVentaTituloDividendos"
    Private _CXCVentaTituloDividendos As Decimal
    Public Property CXCVentaTituloDividendos() As Decimal
        Get
            Return _CXCVentaTituloDividendos
        End Get
        Set(ByVal value As Decimal)
            _CXCVentaTituloDividendos = value
        End Set
    End Property
    'OT10916 Fin
End Class
