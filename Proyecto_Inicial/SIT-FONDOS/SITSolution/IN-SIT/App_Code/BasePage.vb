Imports System.Web
Imports System.Diagnostics
Imports System.Configuration
Imports System.Data
Imports ParametrosSIT
Imports Sit.BusinessLayer
Imports System.Globalization
Imports System.Threading

Public Class BasePage
    Inherits System.Web.UI.Page

    Private Shared _Msj_ErrorSesion As String = "La sesión no ha cumplido con el proceso de autenticación. Se recomienda autenticarse nuevamente mediante el 'Sistema de Seguridad'"

    Private _dsDatosTransaccion As DataSet
    Private _dtDatosTransaccion As DataTable
    Private _Usuario As String = ""

#Region "Eventos Pagina"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UInfo_CodUsuario") Is Nothing Then
            Session("AppError_Msj") = "Su sesi&oacute;n a expirado. Inicie sesi&oacute;n nuevamente."
            Response.Redirect("~/frmDefault.aspx")
        ElseIf DatosRequest Is Nothing Then
            CargarDatos()
        End If
        'If DatosRequest Is Nothing Then
        '    CargarDatos()
        'End If
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-PE")
    End Sub
    Public Sub CargarLoading(ByVal nameControl As String)
        'Dim script As String = "$(document).ready(function () { $('[id*=btnSubmit]').click(); });"
        Dim script As String = String.Empty
        'script = "$(document).ready(function () { $('[id*=btnBuscar]').click(); });"
        script = "$(document).ready(function () { $('[id*=" & nameControl & "]').click(); });"
        ClientScript.RegisterStartupScript(Me.GetType, "load", script, True)
    End Sub
#End Region

#Region "Propiedades"
    ''' <summary>
    ''' Propiedad que contiene el nombre del usuario quien esta logueado en el sistema.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected ReadOnly Property Usuario() As String
        Get
            Return _Usuario
        End Get
    End Property

    ''' <summary>
    ''' Propiedad que contiene la fecha Actual del sistema
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected ReadOnly Property FechaActual As String
        Get
            Return Date.Today.ToString("dd/MM/yyyy")
        End Get
    End Property

    ''' <summary>
    ''' Propiedad que contiene datos relevantes 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected ReadOnly Property DatosRequest() As DataSet
        Get
            If _dsDatosTransaccion Is Nothing Then
                CargarDatos()
            End If
            Return _dsDatosTransaccion
        End Get
    End Property

#End Region

#Region "Propiedades Parametrias Válidas"

    Public ReadOnly Property PORTAFOLIO_MULTIFONDOS() As String
        Get
            Return ""
        End Get
    End Property
#End Region
#Region "Funciones"
    Protected Sub CargarDatos()
        Try
            'Generamos una excepcion si no se encuentran los datos generados por la autenticación
            If Session("UInfo_CodUsuario") Is Nothing Then Throw New Exception(_Msj_ErrorSesion)

            _dsDatosTransaccion = New DataSet
            _dtDatosTransaccion = New DataTable("DatosRequest")
            _dtDatosTransaccion.Columns.Add("Campos")
            _dtDatosTransaccion.Columns.Add("Valores")

            'Permitirá el acceso al CodUsuario como propiedad
            Me._Usuario = Session("UInfo_CodUsuario")

            _dtDatosTransaccion.Rows.Add({"Usuario", Me._Usuario})
            _dtDatosTransaccion.Rows.Add({"Fecha", Date.Now})
            _dtDatosTransaccion.Rows.Add({"Host", Request.UserHostAddress})
            _dtDatosTransaccion.Rows.Add({"Pagina", Request.Url.AbsolutePath})
            _dtDatosTransaccion.Rows.Add({"CodigoEjecucion", 0})

            _dsDatosTransaccion.Tables.Add(_dtDatosTransaccion)
        Catch ex As Exception
            'Manejar el Error
            HttpContext.Current.Session("AppError_Msj") = ex.Message
            HttpContext.Current.Session("AppError_Obj") = ex
            Server.Transfer("~/frmError.aspx?handler=CargarDatos%20-%20BasePage.vb", True)
        End Try
    End Sub

    Protected Function RutaPlantillas() As String
        Dim oArchivoPlanoBM As New ArchivoPlanoBM
        Dim oArchivoPlanoBE As New DataSet
        oArchivoPlanoBE = oArchivoPlanoBM.Seleccionar("018", DatosRequest())
        Return oArchivoPlanoBE.Tables(0).Rows(0).Item(4)
    End Function

    Protected Function VerificarFecha(ByVal FechaSeleccionada As String, Optional ByVal sInd As String = "") As String  'HDG 20130730
        Dim Fecha As Decimal
        Dim FechaValida As String

        Dim fechaNueva As Date
        Dim oFeriadoBM As New FeriadoBM
        FechaValida = String.Empty

        fechaNueva = Convert.ToDateTime(FechaSeleccionada)
        Fecha = CType(fechaNueva.ToString("yyyyMMdd"), Decimal)
        If (fechaNueva.DayOfWeek = DayOfWeek.Saturday) Or (fechaNueva.DayOfWeek = DayOfWeek.Sunday) Then
            FechaValida = fechaNueva.DayOfWeek.ToString

        Else
            If oFeriadoBM.BuscarPorFecha(Fecha, sInd) = True Then   'HDG 20130730
                FechaValida = "Feriado"
            End If
        End If
        Return FechaValida
    End Function
#End Region

#Region "Funciones JS"
    ''' <summary>
    ''' Metodo que permite la ejecución de javascript.
    ''' </summary>
    ''' <param name="strJS"></param>
    ''' <param name="addScriptTags"></param>
    ''' <remarks></remarks>
    Public Sub EjecutarJS(ByVal strJS As String, Optional ByVal addScriptTags As Boolean = True)
        Dim sGUID As String = System.Guid.NewGuid.ToString()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), sGUID, strJS, addScriptTags)
    End Sub

    ''' <summary>
    ''' Función para lanzar alert personalizado con la libreria js alertify.
    ''' </summary>
    ''' <param name="strJS">Mensaje que se desea mostrar</param>
    ''' <remarks></remarks>
    Public Sub AlertaJS(ByVal strJS As String)
        Dim js As New StringBuilder

        'strJS = Replace(strJS, Chr(13), "\n")
        'strJS = Replace(strJS, Chr(10), "")

        js.AppendLine("alertify.alert('<b>" + strJS + "</b>');")
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertify", js.ToString(), True)

    End Sub
    ''' <summary>
    ''' Función para lanzar alert personalizado con instrucciones js como callback con la libreria js alertify
    ''' </summary>
    ''' <param name="strJS">Mensaje que se desea mostrar</param>
    ''' <param name="jsCallBack">Sentencias js a ejecutar despues de mostrar el alert</param>
    ''' <remarks></remarks>
    Public Sub AlertaJS(ByVal strJS As String, ByVal jsCallBack As String)
        Dim js As New StringBuilder

        strJS = Replace(strJS, Chr(13), "\n")
        strJS = Replace(strJS, Chr(10), "")

        js.AppendLine("alertify.alert('<b>" + HttpUtility.HtmlEncode(strJS) + "</b>',function (e) {" + jsCallBack + " });")
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertify", js.ToString(), True)
    End Sub
    ''' <summary>
    ''' Función para lanzar alert personalizado con instrucciones js como callback con la libreria js alertify
    ''' </summary>
    ''' OT10927 - 21/11/2017 - Hanz Cocchi.
    ''' <param name="strJS">Mensaje que se desea mostrar</param>
    ''' <param name="jsCallBack">Sentencias js a ejecutar despues de mostrar el alert</param>
    ''' <remarks></remarks>
    Public Sub AlertaJS2(ByVal strJS As String, ByVal jsCallBack As String)
        Dim js As New StringBuilder

        strJS = Replace(strJS, Chr(13), "\n")
        strJS = Replace(strJS, Chr(10), "")

        js.AppendLine("alertify.alert('<b>" + strJS + "</b>',function (e) {" + jsCallBack + " });")
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertify", js.ToString(), True)
    End Sub

    Public Sub AlertaJS3(ByVal strJS As String, ByVal jsCallBack As String, ByVal wait As String)
        Dim js As New StringBuilder

        strJS = Replace(strJS, Chr(13), "\n")
        strJS = Replace(strJS, Chr(10), "")

        js.AppendLine("alertify.success('<b>" + HttpUtility.HtmlEncode(strJS) + "</b>',function (e) {" + jsCallBack + " }," + wait + ");")
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertify", js.ToString(), True)
    End Sub

    ''' <summary>
    ''' Función que permite obtener un mensaje desde parametria en base al código
    ''' </summary>
    ''' <param name="CodigoMensaje">Código del mensaje a obtener</param>
    ''' <param name="param">Texto adicional que se puede agregar al mensaje original</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenerMensaje(ByVal CodigoMensaje As String, Optional ByVal param As String = "") As String
        Dim _objutilitario As New UtilDM
        Dim mensaje As String = _objutilitario.RetornarMensajeConfirmacion(CodigoMensaje) & param
        Return mensaje
    End Function

    ''' <summary>
    ''' Función que devuelve el código javascript para asignar un confirm personalizado
    ''' </summary>
    ''' <param name="str">Texto del mensaje</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConfirmJS(ByVal str As String) As String
        Dim confirm As String
        confirm = "return confirm('" + str + "');"
        Return confirm
    End Function
#End Region

#Region "Validadciones de numeros"
    Public Function ToNullDecimal(ByVal valor As Object) As Decimal
        Dim d_Numero As Decimal = 0
        Try
            d_Numero = Convert.ToDecimal(valor)
        Catch ex As Exception
            d_Numero = 0
        End Try
        Return d_Numero
    End Function

    Public Function ToNullDouble(ByVal valor As Object) As Double
        Dim d_Numero As Double = 0
        Try
            d_Numero = Convert.ToDouble(valor)
        Catch ex As Exception
            d_Numero = 0
        End Try
        Return d_Numero
    End Function

    Public Function ToNullInt32(ByVal valor As Object) As Integer
        Dim d_Numero As Integer = 0
        Try
            d_Numero = Convert.ToInt32(valor)
        Catch ex As Exception
            d_Numero = 0
        End Try
        Return d_Numero
    End Function
#End Region

    Public Function Ruta_Logo() As String
        Return Server.MapPath(ConfigurationManager.AppSettings("RUTA_LOGO"))
    End Function
    Public Function ExportToMemoryStream(ByVal fileStream As IO.FileStream) As IO.MemoryStream
        Dim result As IO.MemoryStream
        Try
            Dim oReader As IO.BinaryReader
            oReader = New IO.BinaryReader(fileStream)
            result = New IO.MemoryStream(oReader.ReadBytes(fileStream.Length))
            fileStream.Close()
            oReader.Close()
        Catch ex As Exception
            result = Nothing
        End Try
        Return result
    End Function

    ' INICIO | ZOLUXIONES | RCE | RF002 - Función para lanzar confirm personalizado con instrucciones js como callback con la libreria js alertify | 24/05/18

    Public Sub ConfirmarJS(ByVal strJS As String, ByVal jsCallBack As String)
        Dim js As New StringBuilder
        Dim RetornarConfirmacion As String

        strJS = Replace(strJS, Chr(13), "\n")
        strJS = Replace(strJS, Chr(10), "")
        RetornarConfirmacion = "if (e) { " & jsCallBack & " } else { return false }"

        js.AppendLine("alertify.confirm('<b>" + strJS + "</b>',function (e) {" + RetornarConfirmacion + " });")
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alertify", js.ToString(), True)
    End Sub
    ' FIN | ZOLUXIONES | RCE | RF002 - Función para lanzar confirm personalizado con instrucciones js como callback con la libreria js alertify | 24/05/18

    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se implementa retorno de mensaje de confirmación de acción | 07/06/18 
    Public Sub retornarMensajeAccion(ByVal strAccion As String)
        AlertaJS("Se " + strAccion + " correctamente la Orden de Inversi\u00f3n", "window.location.href = window.location.href")
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se implementa retorno de mensaje de confirmación de acción | 20/08/18 

    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se implementa retorno de mensaje de confirmación de acción | 07/06/18 
    Public Sub retornarMensajeAccionPreOrden(ByVal strAccion As String)
        AlertaJS("Se " + strAccion + " correctamente la Pre-Orden de Inversi\u00f3n", "window.location.href = window.location.href")
    End Sub
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se implementa retorno de mensaje de confirmación de acción | 20/08/18 
End Class
