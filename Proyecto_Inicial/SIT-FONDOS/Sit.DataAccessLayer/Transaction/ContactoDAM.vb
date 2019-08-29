Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class ContactoDAM

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oContactoBE As ContactoBE, ByVal dataRequest As DataSet) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contacto_Insertar")

        Dim oRow As ContactoBE.ContactoRow

        oRow = DirectCast(oContactoBE.Contacto.Rows(0), ContactoBE.ContactoRow)

        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, oRow.CodigoContacto)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Observaciones", DbType.String, oRow.Observaciones)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_TipoContacto", DbType.String, oRow.TipoContacto)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function Seleccionar(ByVal codigoContacto As String) As ContactoBE

        Dim oContactoBE As New ContactoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contacto_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        db.LoadDataSet(dbCommand, oContactoBE, "Contacto")

        Return oContactoBE

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoContacto As String, ByVal descripcion As String, ByVal situacion As String) As ContactoBE

        Dim oContactoBE As New ContactoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contacto_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, oContactoBE, "Contacto")

        Return oContactoBE

    End Function

    Public Function Listar() As ContactoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contacto_Listar")
        Dim oContactoBE As New ContactoBE

        db.LoadDataSet(dbCommand, oContactoBE, "Contacto")

        Return oContactoBE

    End Function

    Public Function ListarActivos() As ContactoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contacto_Listar_Activos")
        Dim oContactoBE As New ContactoBE

        db.LoadDataSet(dbCommand, oContactoBE, "Contacto")

        Return oContactoBE

    End Function

    Public Function ListarPorTercero(ByVal codigoTercero As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contacto_ListarPorTercero")

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarPorTerceroTesoreria(ByVal codigoTercero As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contacto_ListarPorTerceroTesoreria")

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    'RGF 20090708
    Public Function ListarPorCodigoEntidad(ByVal CodigoEntidad As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contacto_ListarPorCodigoEntidad")

        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Modificar(ByVal oContactoBE As ContactoBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contacto_Modificar")

        Dim oRow As ContactoBE.ContactoRow

        oRow = DirectCast(oContactoBE.Contacto.Rows(0), ContactoBE.ContactoRow)

        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, oRow.CodigoContacto)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Observaciones", DbType.String, oRow.Observaciones)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_TipoContacto", DbType.String, oRow.TipoContacto)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function Eliminar(ByVal codigoContacto As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contacto_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    'LETV 20090320
    Public Function SeleccionarUltimoContactoEnUnaNegociacion(ByVal CodigoMNemonico As String, ByVal CodigoTercero As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Contacto_UltimoContactoSeleccionado")

        db.AddInParameter(dbCommand, "@p_CodigoMNemonico", DbType.String, CodigoMNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
        Return Convert.ToString(db.ExecuteScalar(dbCommand))
    End Function

End Class