Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities

''' <summary>
''' Clase para el acceso de los datos para CentroCostos tabla.
''' </summary>
Public Class CentroCostosDAM

    Private oCentroCostosRow As CentroCostosBE.CentroCostosRow

    Public Sub New()
    End Sub

#Region " /* Seleccionar Functions */ "
    
    Public Function Seleccionar(ByVal codigoCentroCostos As Decimal, ByVal dataRequest As DataSet) As CentroCostosBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CentroCostos_Seleccionar")
        Dim objeto As New CentroCostosBE

        db.AddInParameter(dbCommand, "@p_CodigoCentroCostos", DbType.Decimal, codigoCentroCostos)
        db.LoadDataSet(dbCommand, objeto, "CentroCostos")
        Return objeto

    End Function

    Public Function SeleccionarPorFiltros(ByVal codigo As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As CentroCostosBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CentroCostos_SeleccionarPorFiltro")
        Dim objeto As New CentroCostosBE

        db.AddInParameter(dbCommand, "@p_CodigoCentroCosto", DbType.String, codigo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.LoadDataSet(dbCommand, objeto, "CentroCostos")

        Return objeto

    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As CentroCostosBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CentroCostos_Listar")
        Dim objeto As New CentroCostosBE

        db.LoadDataSet(dbCommand, objeto, "CentroCostos")

        Return objeto

    End Function

#End Region

#Region " /* Insertar Functions */ "

    Public Function Insertar(ByVal ob As CentroCostosBE, ByVal dataRequest As DataSet) As String
        'Dim db As Database = DatabaseFactory.CreateDatabase()
        'Dim dbCommand As dbCommand = db.GetStoredProcCommand("CentroCostos_Insertar")
        'Dim Codigo As String = String.Empty
        'oCentroCostosRow = CType(ob.CentroCostos.Rows(0), CentroCostosBE.CentroCostosRow)
        'db.AddInParameter(dbCommand, "@p_CodigoCentroCostos", DbType.Decimal, oCentroCostosRow.CodigoCentroCostos)
        'db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        'db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oCentroCostosRow.Descripcion)
        'db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        'db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCentroCostosRow.Situacion)
        ''db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        ''db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, oCentroCostosRowDataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        ''db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, oCentroCostosRowDataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.ExecuteNonQuery(dbCommand)
        'Return Codigo = True
    End Function

#End Region

#Region " /* Modificar Functions */"

    Public Function Modificar(ByVal ob As CentroCostosBE, ByVal dataRequest As DataSet) As String
        'Dim db As Database = DatabaseFactory.CreateDatabase()
        'Dim dbCommand As dbCommand = db.GetStoredProcCommand("CentroCostos_Modificar")
        'oCentroCostosRow = CType(ob.CentroCostos.Rows(0), CentroCostosBE.CentroCostosRow)

        'db.AddInParameter(dbCommand, "@p_CodigoCentroCostos", DbType.Decimal, oCentroCostosRow.CodigoCentroCostos)
        'db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oCentroCostosRow.Descripcion)
        'db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        'db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        'db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCentroCostosRow.Situacion)
        'db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        ''db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        ''db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, oCentroCostosRowDataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        ''db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, oCentroCostosRowDataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.ExecuteNonQuery(dbCommand)
        'Return True
    End Function
#End Region

#Region " /* Eliminar Functions */"
    ''' <summary>
    ''' Elimina un expediente de CentroCostos table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoCentroCostos">Decimal</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoCentroCostos As Decimal, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CentroCostos_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoCentroCostos", DbType.Decimal, codigoCentroCostos)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region


End Class

