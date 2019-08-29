Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities



''' <summary>
''' Clase para el acceso de los datos para OperacionContable tabla.
''' </summary>
Public Class OperacionContableDAM

    Private oOperacionContableRow As OperacionContableBE.OperacionContableRow

    Public Sub New()
    End Sub


#Region " /* Seleccionar Functions */ "
    ''' <summary>
    ''' Selecciona una lista Filtrada de los expedientes de OperacionContable tabla , segun los paramepros .
    ''' <summary>
    ''' <param name="codigoOperacionContable">String</param>
    ''' <param name="descripcion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>OperacionContableBE</returns>
    Public Function SeleccionarPorFiltros(ByVal codigoOperacionContable As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As OperacionContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionContable_SeleccionarPorFiltro")
        Dim objeto As New OperacionContableBE

        db.AddInParameter(dbCommand, "@p_CodigoOperacionContable", DbType.String, codigoOperacionContable)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.LoadDataSet(dbCommand, objeto, "OperacionContable")
        Return objeto
    End Function
    ''' <summary>
    ''' Selecciona un solo expediente de OperacionContable tabla.
    ''' <summary>
    ''' <param name="codigoOperacionContable">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>OperacionContableBE</returns>
    Public Function Seleccionar(ByVal codigoOperacionContable As String, ByVal dataRequest As DataSet) As OperacionContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionContable_Seleccionar")
        Dim objeto As New OperacionContableBE
        db.AddInParameter(dbCommand, "@p_CodigoOperacionContable", DbType.String, codigoOperacionContable)
        db.LoadDataSet(dbCommand, objeto, "OperacionContable")
        Return objeto
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de OperacionContable tabla.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>OperacionContableBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As OperacionContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionContable_Listar")
        Dim objeto As New OperacionContableBE
        db.LoadDataSet(dbCommand, objeto, "OperacionContable")
        Return objeto
    End Function

#End Region

#Region " /* Insertar Functions */ "
    ''' <summary>
    ''' Inserta un expediente en OperacionContable tabla.
    ''' <summary>
    ''' <param name="ob">OperacionContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal ob As OperacionContableBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionContable_Insertar")
        Dim Codigo As String = String.Empty
        oOperacionContableRow = CType(ob.OperacionContable.Rows(0), OperacionContableBE.OperacionContableRow)

        db.AddInParameter(dbCommand, "@p_CodigoOperacionContable", DbType.String, oOperacionContableRow.CodigoOperacionContable)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oOperacionContableRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oOperacionContableRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        'db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        'db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, oOperacionContableRowDataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, oOperacionContableRowDataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return Codigo = True
    End Function
#End Region

#Region " /* Modificar Functions */"
    ''' <summary>
    ''' Midifica un expediente en OperacionContable tabla.
    ''' <summary>
    ''' <param name="ob">OperacionContableBE</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal ob As OperacionContableBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionContable_Modificar")
        oOperacionContableRow = CType(ob.OperacionContable.Rows(0), OperacionContableBE.OperacionContableRow)
        db.AddInParameter(dbCommand, "@p_CodigoOperacionContable", DbType.String, oOperacionContableRow.CodigoOperacionContable)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oOperacionContableRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oOperacionContableRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        'db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, oOperacionContableRowDataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, oOperacionContableRowDataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region

#Region " /* Eliminar Functions */"
    ''' <summary>
    ''' Elimina un expediente de OperacionContable table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoOperacionContable">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoOperacionContable As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("OperacionContable_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoOperacionContable", DbType.String, codigoOperacionContable)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region


End Class

