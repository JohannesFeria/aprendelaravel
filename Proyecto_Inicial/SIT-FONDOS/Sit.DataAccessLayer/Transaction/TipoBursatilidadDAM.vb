Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class TipoBursatilidadDAM
    Private sqlCommand As String = ""
    Private oRow As TipoBursatilidadBE.TipoBursatilidadRow
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltros(ByVal situacion As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As TipoBursatilidadBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoBursatilidad_SeleccionarPorFiltro")
        Dim oTipoBursatilidadBE As New TipoBursatilidadBE

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)

        db.LoadDataSet(dbCommand, oTipoBursatilidadBE, "TipoBursatilidad")

        Return oTipoBursatilidadBE

    End Function

    Public Function Seleccionar(ByVal codigoBursatilidad As String, ByVal dataRequest As DataSet) As TipoBursatilidadBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoBursatilidad_Seleccionar")
        Dim oTipoBursatilidadBE As New TipoBursatilidadBE

        db.AddInParameter(dbCommand, "@p_CodigoBursatilidad", DbType.String, codigoBursatilidad)

        db.LoadDataSet(dbCommand, oTipoBursatilidadBE, "TipoBursatilidad")

        Return oTipoBursatilidadBE

    End Function
    ''' <summary>
    ''' Lista todos los expedientes de TipoBursatilidadBE tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As TipoBursatilidadBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoBursatilidad_Listar")

        Dim oTipoBursatilidadBE As New TipoBursatilidadBE
        db.LoadDataSet(dbCommand, oTipoBursatilidadBE, "TipoBursatilidad")
        Return oTipoBursatilidadBE

    End Function
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oTipoBursatilidadBE As TipoBursatilidadBE, ByVal dataRequest As DataSet) As String
        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoBursatilidad_Insertar")

        oRow = CType(oTipoBursatilidadBE.TipoBursatilidad.Rows(0), TipoBursatilidadBE.TipoBursatilidadRow)

        db.AddInParameter(dbCommand, "@p_CodigoBursatilidad", DbType.String, oRow.CodigoBursatilidad)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return Codigo

    End Function

#End Region

#Region " /* Funciones Modificar */"

    ''' <summary>
    ''' Midifica un expediente en TipoBursatilidadBE tabla.
    ''' <summary>
    ''' <param name="codigoTipoBursatilidad"></param>
    ''' <param name="descripcion"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="onservaciones"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="situacion"></param>
    ''' <param name="host"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    Public Function Modificar(ByVal oTipoBursatilidadBE As TipoBursatilidadBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoBursatilidad_Modificar")
        oRow = CType(oTipoBursatilidadBE.TipoBursatilidad.Rows(0), TipoBursatilidadBE.TipoBursatilidadRow)

        db.AddInParameter(dbCommand, "@p_CodigoBursatilidad", DbType.String, oRow.CodigoBursatilidad)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True


    End Function


#End Region

#Region " /* Funciones Eliminar */"

    ''' <summary>
    ''' Elimina un expediente de TipoBursatilidadBE table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoBursatilidad As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoBursatilidad_Eliminar")
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_CodigoBursatilidad", DbType.String, codigoBursatilidad)

        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function
#End Region

#Region " /* Funciones Personalizadas*/"

#End Region


End Class

