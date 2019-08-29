Option Explicit On 
Option Strict On
Public Class Constantes

#Region "/* Generales */"

    Public Shared ReadOnly M_STR_TEXTO_INICIAL As String = String.Empty
    Public Shared ReadOnly M_STR_TEXTO_SELECCIONAR_EN_COMBO As String = "--Seleccione--"
    Public Shared ReadOnly M_STR_TEXTO_TODOS As String = "Todos"
    Public Shared ReadOnly M_STR_TEXTO_RESULTADOS As String = "Registros Encontrados: "
    Public Shared ReadOnly M_STR_TEXTO_CARGADOS As String = "Registros Cargados: "

    Public Shared ReadOnly M_STR_CONDICIONAL_SI As String = "S"
    Public Shared ReadOnly M_STR_CONDICIONAL_NO As String = "N"
    Public Shared ReadOnly M_INT_NRO_DECIMALES As Integer = 7
    Public Shared ReadOnly M_STR_MERCADO_LOCAL As String = "LOCAL"
    Public Shared ReadOnly M_STR_MERCADO_EXTRANJERO As String = "EXTRANJERO"
    Public Shared ReadOnly M_COMPRA As String = "COMPRA"
    Public Shared ReadOnly M_VENTA As String = "VENTA"
    Public Shared ReadOnly M_TRASPASO_EGRESO As String = "TEGRESO"
    Public Shared ReadOnly M_TRASPASO_INGRESO As String = "TINGRESO"
    Public Shared ReadOnly M_TRASPASO_INGRESO_ As String = "(Destino) Traspaso Ingreso"
    Public Shared ReadOnly M_CONS As String = "CONS"
    Public Shared ReadOnly M_CANC As String = "CANC"
    Public Shared ReadOnly M_RENO As String = "RENO"
    Public Shared ReadOnly M_PREC As String = "PREC"
    'Public Shared ReadOnly M_RFLP As String = "RFLP"
    'Public Shared ReadOnly M_RFCP As String = "RFCP"
    Public Shared ReadOnly M_RFIJ As String = "RFIJ"
    Public Shared ReadOnly M_RVAR As String = "RVAR"
    Public Shared ReadOnly M_RDER As String = "RDER"
    Public Shared ReadOnly M_CI_PLAZO_RV As String = "RV"
    Public Shared ReadOnly M_CI_PLAZO_CP As String = "CP"
    Public Shared ReadOnly M_CI_PLAZO_LP As String = "LP"
    Public Shared ReadOnly M_CI_PLAZO_DV As String = "DV"
    Public Shared ReadOnly M_CALIF_OTROS As String = "OT"
    Public Shared ReadOnly M_CODIGO_TIPO_INSTRUMENTO_FONDO_MUTUO As String = "52"

#End Region

#Region "ParametrosGenerales"

    Public Structure ParametrosGenerales
        Public Const RUTA_TEMP As String = "RUTA_TEMP"
    End Structure

#End Region

#Region "Pantalla"

    Public Structure Pantalla

        Public Structure FormatoFecha

            Public Const ddMMyyyy_01 As String = "dd/MM/yyyy"

        End Structure

    End Structure

    Public Structure HojaExcel

        Public Const InventarioForward As String = "Inventario Forward"
        Public Const ComposicionCartera As String = "Composicion Cartera"
        Public Const TipoCambio As String = "Tipo Cambio"
        Public Const MarkToMarketFW As String = "Mark to Makert Forwards"

    End Structure

#End Region

#Region "/* Mensajes */"

    Public Shared ReadOnly M_STR_MENSAJE_ACTUALIZAR_ENTIDAD As String = "Los datos fueron modificados correctamente"
    Public Shared ReadOnly M_STR_MENSAJE_PREGUNTA_ELIMINAR_ENTIDAD As String = "¿Confirmar la eliminación del registro?"
    Public Shared ReadOnly M_STR_MENSAJE_INSERTAR_ENTIDAD As String = "Los datos fueron grabados correctamente"
    Public Shared ReadOnly M_STR_MENSAJE_ELIMINAR_ENTIDAD As String = "El registro fue eliminado correctamente"
    Public Shared ReadOnly M_STR_MENSAJE_ENTIDAD_EXISTE As String = "Este registro ya existe"
    Public Shared ReadOnly M_STR_MENSAJE_OPERACION_EXISTE As String = "Esta operación ya esta liquidada. Debe reversar primero."
    Public Shared ReadOnly M_STR_MENSAJE_FALTA_CUPONERA As String = "Nemonico no tiene cuponera."
    Public Shared ReadOnly M_STR_MENSAJE_ENTIDAD_ID_EXISTE As String = "Esta entidad ya existe"
    Public Shared ReadOnly M_STR_MENSAJE_NO_EXISTE_DATA As String = "No se encontraron Registros"
    Public Shared ReadOnly M_STR_MENSAJE_NO_SE_ENCONTRO_ARCHIVO_RUTA As String = "No se encontró el archivo en la ruta: "
    Public Shared ReadOnly M_STR_MENSAJE_FECHA_VACIA As String = "Debe ingresar la fecha"

#End Region

#Region "/* QueryString */"

    Public Shared ReadOnly M_STR_QUERY_STRING_CADENA As String = "cadena"

#End Region

#Region "/* ViewState */"

    Public Shared ReadOnly M_STR_VIEWSTATE_DETALLE_COTIZACION_VAC As String = "CotizacionVAC"

#End Region

End Class
