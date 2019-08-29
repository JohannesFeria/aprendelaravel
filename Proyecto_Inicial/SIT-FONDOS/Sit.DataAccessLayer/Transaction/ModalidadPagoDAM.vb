Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities


''' <summary>
''' Clase para el acceso de los datos para ModalidadPago tabla.
''' </summary>
Public Class ModalidadPagoDAM

    Private oModalidadPagoRow As ModalidadPagoBE.ModalidadPagoRow

    Public Sub New()
    End Sub

#Region " /* Seleccionar Functions */ "
    ''' <summary>
    ''' Selecciona una lista Filtrada de los expedientes de ModalidadPago tabla , segun los paramepros .
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="situacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ModalidadPagoBE</returns>
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As ModalidadPagoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModalidadPago_SeleccionarPorFiltro")
        Dim objeto As New ModalidadPagoBE

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.LoadDataSet(dbCommand, objeto, "ModalidadPago")
        Return objeto
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de ModalidadPago tabla.
    ''' <summary>
    ''' <param name="codigoModalidadPago">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ModalidadPagoBE</returns>
    Public Function Seleccionar(ByVal codigoModalidadPago As String, ByVal dataRequest As DataSet) As ModalidadPagoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModalidadPago_Seleccionar")

        Dim objeto As New ModalidadPagoBE

        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, codigoModalidadPago)

        db.LoadDataSet(dbCommand, objeto, "ModalidadPago")
        Return objeto

    End Function

    ''' <summary>
    ''' Lista todos los expedientes de ModalidadPago tabla.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ModalidadPagoBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet, Optional ByVal situacion As String = "") As ModalidadPagoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModalidadPago_Listar")
        db.AddInParameter(dbCommand, "@p_situacion", DbType.String, situacion)
        Dim objeto As New ModalidadPagoBE
        db.LoadDataSet(dbCommand, objeto, "ModalidadPago")
        Return objeto
    End Function

#End Region

#Region " /* Insertar Functions */ "
    ''' <summary>
    ''' Inserta un expediente en ModalidadPago tabla.
    ''' <summary>
    ''' <param name="ob">ModalidadPagoBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal ob As ModalidadPagoBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModalidadPago_Insertar")

        Dim Codigo As String = String.Empty

        oModalidadPagoRow = CType(ob.ModalidadPago.Rows(0), ModalidadPagoBE.ModalidadPagoRow)

        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, oModalidadPagoRow.CodigoModalidadPago)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oModalidadPagoRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oModalidadPagoRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Naturaleza", DbType.String, oModalidadPagoRow.Naturaleza)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return "True"


    End Function



#End Region

#Region " /* Modificar Functions */"
    ''' <summary>
    ''' Midifica un expediente en ModalidadPago tabla.
    ''' <summary>
    ''' <param name="ob">ModalidadPagoBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal ob As ModalidadPagoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModalidadPago_Modificar")

        oModalidadPagoRow = CType(ob.ModalidadPago.Rows(0), ModalidadPagoBE.ModalidadPagoRow)

        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, oModalidadPagoRow.CodigoModalidadPago)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oModalidadPagoRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oModalidadPagoRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Naturaleza", DbType.String, oModalidadPagoRow.Naturaleza)
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
    ''' Elimina un expediente de ModalidadPago table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoModalidadPago">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoModalidadPago As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModalidadPago_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoModalidadPago", DbType.String, codigoModalidadPago)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function
#End Region

End Class

