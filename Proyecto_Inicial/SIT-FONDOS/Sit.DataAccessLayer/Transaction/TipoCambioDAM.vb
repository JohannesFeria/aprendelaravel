Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities


''' <summary>
''' Clase para el acceso de los datos para TipoCambio tabla.
''' </summary>
Public Class TipoCambioDAM
    Private oTipoCambioRow As TipoCambioBE.TipoCambioRow
    Public Sub New()

    End Sub

    Public Function SeleccionarFechaSistema(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ServidorFecha")

        Dim objeto As New DataSet
        db.LoadDataSet(dbCommand, objeto, "TipoCambio")
        Return objeto

    End Function

    Public Function Seleccionar(ByVal codigoTipoCambio As String, ByVal dataRequest As DataSet) As TipoCambioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambio_Seleccionar")

        Dim objeto As New TipoCambioBE

        db.AddInParameter(dbCommand, "@p_CodigoTipoCambio", DbType.String, codigoTipoCambio)

        db.LoadDataSet(dbCommand, objeto, "TipoCambio")
        Return objeto

    End Function

    Public Function SeleccionarTCOrigen_TCDestino(ByVal codigoTCOrigen As String, ByVal codigoTCDestino As String, ByVal fecha As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambio_SeleccionarMonedaOrigenMonedaDestino")

        Dim objeto As New DataSet

        db.AddInParameter(dbCommand, "@p_CodigoTCOrigen", DbType.String, codigoTCOrigen)
        db.AddInParameter(dbCommand, "@p_CodigoTCDestino", DbType.String, codigoTCDestino)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, fecha)

        db.LoadDataSet(dbCommand, objeto, "TipoCambio")
        Return objeto

    End Function

    Public Function SeleccionarValorTCOrigen_TCDestino(ByVal codigoTCOrigen As String, ByVal codigoTCDestino As String, ByVal fecha As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambio_SeleccionarValorMonedaOrigenMonedaDestino")

        Dim objeto As New DataSet

        db.AddInParameter(dbCommand, "@p_CodigoTCOrigen", DbType.String, codigoTCOrigen)
        db.AddInParameter(dbCommand, "@p_CodigoTCDestino", DbType.String, codigoTCDestino)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, fecha)

        db.LoadDataSet(dbCommand, objeto, "TipoCambio")
        Return objeto

    End Function

    Public Function SeleccionarPorFiltros(ByVal codigoTipoCambio As String, ByVal situacion As String, ByVal descripcion As String, ByVal DataRequest As DataSet) As TipoCambioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambio_SeleccionarPorFiltro")
        Dim objeto As New TipoCambioBE

        db.AddInParameter(dbCommand, "@p_CodigoTipoCambio", DbType.String, codigoTipoCambio)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.LoadDataSet(dbCommand, objeto, "TipoCambio")
        Return objeto
    End Function

    Public Function Insertar(ByVal ob As TipoCambioBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambio_Insertar")

        Dim Codigo As String = String.Empty

        oTipoCambioRow = CType(ob.TipoCambio.Rows(0), TipoCambioBE.TipoCambioRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoCambio", DbType.String, oTipoCambioRow.CodigoTipoCambio)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTipoCambioRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oTipoCambioRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return "True"


    End Function

    Public Function Modificar(ByVal ob As TipoCambioBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambio_Modificar")

        oTipoCambioRow = CType(ob.TipoCambio.Rows(0), TipoCambioBE.TipoCambioRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoCambio", DbType.String, oTipoCambioRow.CodigoTipoCambio)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTipoCambioRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oTipoCambioRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

    Public Function Eliminar(ByVal codigoTipoCambio As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambio_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoCambio", DbType.String, codigoTipoCambio)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function

    'LETV 20090718
    Public Function ExisteTipoCambio(ByVal Entidad As String, ByVal CodigoMoneda As String, ByVal Fecha As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambio_SelccionarPorEntidad")

        Dim objeto As New DataSet
        Dim resultado As Boolean
        db.AddInParameter(dbCommand, "@p_Entidad", DbType.String, Entidad)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, Fecha)
        db.LoadDataSet(dbCommand, objeto, "TipoCambio")
        resultado = IIf(objeto.Tables(0).Rows.Count > 0, True, False)
        Return resultado
    End Function

    'LETV 20091030
    Public Function SeleccionarValorTCOrigen_TCDestinoXEntidad(ByVal Moneda As String, ByVal MonedaDestino As String, ByVal fecha As Decimal, ByVal Entidad As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_TipoCambio_SeleccionaValorMonedaOrigenMonedaDestinoFuente")

        db.AddInParameter(dbCommand, "@p_Moneda", DbType.String, Moneda)
        db.AddInParameter(dbCommand, "@p_MonedaDestino", DbType.String, MonedaDestino)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Entidad", DbType.String, Entidad)
        Return CDec(db.ExecuteScalar(dbCommand))
    End Function

End Class
