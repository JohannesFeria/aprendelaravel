Imports System.Web
Imports System.Diagnostics
Imports System.Configuration
Imports System.Data
Imports ParametrosSIT
Imports Cartas.BusinessLayer
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
        If DatosRequest Is Nothing Then
            CargarDatos()
        End If
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-PE")
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
            Return ""
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
            Response.Redirect(Server.MapPath("~/frmError.aspx"))
        End Try
    End Sub
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
End Class