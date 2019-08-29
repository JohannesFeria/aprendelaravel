Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities

''' <summary>
''' Clase para el acceso de los datos para ConceptoAsientoContable tabla.
''' </summary>
Public Class ConceptoAsientoContableDAM

    Private oConceptoAsientoContableRow As ConceptoAsientoContableBE.ConceptoAsientoContableRow

    Public Sub New()
    End Sub


#Region " /* Seleccionar Functions */ "
    ''' <summary>
    ''' Selecciona una lista Filtrada de los expedientes de ConceptoAsientoContable tabla , segun los paramepros .
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="situacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ConceptoAsientoContableBE</returns>
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As ConceptoAsientoContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoAsientoContable_SeleccionarPorFiltro")
        Dim objeto As New ConceptoAsientoContableBE

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.LoadDataSet(dbCommand, objeto, "ConceptoAsientoContable")
        Return objeto
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de ConceptoAsientoContable tabla.
    ''' <summary>
    ''' <param name="codigoAsientoContable">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ConceptoAsientoContableBE</returns>
    Public Function Seleccionar(ByVal codigoAsientoContable As String, ByVal dataRequest As DataSet) As ConceptoAsientoContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoAsientoContable_Seleccionar")

        Dim objeto As New ConceptoAsientoContableBE

        db.AddInParameter(dbCommand, "@p_CodigoAsientoContable", DbType.String, codigoAsientoContable)
        db.LoadDataSet(dbCommand, objeto, "ConceptoAsientoContable")

        Return objeto

    End Function

    ''' <summary>
    ''' Lista todos los expedientes de ConceptoAsientoContable tabla.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ConceptoAsientoContableBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As ConceptoAsientoContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoAsientoContable_Listar")

        Dim objeto As New ConceptoAsientoContableBE

        db.LoadDataSet(dbCommand, objeto, "ConceptoAsientoContable")
        Return objeto
    End Function

#End Region

#Region " /* Insertar Functions */ "


    ''' <summary>
    ''' Inserta un expediente en ConceptoAsientoContable tabla.
    ''' <summary>
    ''' <param name="ob">ConceptoAsientoContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal ob As ConceptoAsientoContableBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoAsientoContable_Insertar")

        Dim Codigo As String = String.Empty


        oConceptoAsientoContableRow = CType(ob.ConceptoAsientoContable.Rows(0), ConceptoAsientoContableBE.ConceptoAsientoContableRow)
        db.AddInParameter(dbCommand, "@p_CodigoAsientoContable", DbType.String, oConceptoAsientoContableRow.CodigoAsientoContable)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oConceptoAsientoContableRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oConceptoAsientoContableRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return "true"

    End Function

#End Region

#Region " /* Modificar Functions */"
    ''' <summary>
    ''' Midifica un expediente en ConceptoAsientoContable tabla.
    ''' <summary>
    ''' <param name="ob">ConceptoAsientoContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal ob As ConceptoAsientoContableBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoAsientoContable_Modificar")

        oConceptoAsientoContableRow = CType(ob.ConceptoAsientoContable.Rows(0), ConceptoAsientoContableBE.ConceptoAsientoContableRow)
        db.AddInParameter(dbCommand, "@p_CodigoAsientoContable", DbType.String, oConceptoAsientoContableRow.CodigoAsientoContable)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oConceptoAsientoContableRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oConceptoAsientoContableRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True


    End Function

#End Region

#Region " /* Eliminar Functions */"
    ''' <summary>
    ''' Elimina un expediente de ConceptoAsientoContable table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoMovimiento">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoAsientoContable As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoAsientoContable_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoAsientoContable", DbType.String, codigoAsientoContable)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function
#End Region



End Class

