Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class EmisorDAM

    Private sqlCommand As String = ""
    Private oEmisorRow As EmisorBE.EmisorRow
    Public Sub New()

    End Sub
    Public Function Listar(ByVal dataRequest As DataSet) As EmisorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_Listar")

        Dim objeto As New EmisorBE
        db.LoadDataSet(dbCommand, objeto, "Emisor")
        Return objeto
    End Function
    ''' <summary>
    ''' Inserta un expediente en Emisor tabla.
    ''' <summary>
    ''' <param name="codigoEmisor"></param>
    ''' <param name="sinonimoEmisor"></param>
    ''' <param name="descripcion"></param>
    ''' <param name="numeroDocumento"></param>
    ''' <param name="direccion"></param>
    ''' <param name="codigoPais"></param>
    ''' <param name="codigoTipoDocumento"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="codigoMercado"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="codigoPostal"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="codigoSectorEmpresarial"></param>
    ''' <param name="situacion"></param>
    ''' <param name="host"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    ''' <returns></returns>
    Public Function Insertar(ByVal codigoEmisor As Decimal, ByVal sinonimoEmisor As String, ByVal descripcion As String, ByVal numeroDocumento As Decimal, ByVal direccion As String, ByVal codigoPais As Decimal, ByVal codigoTipoDocumento As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal codigoMercado As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal codigoPostal As String, ByVal horaCreacion As String, ByVal codigoSectorEmpresarial As String, ByVal situacion As String, ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.Decimal, codigoEmisor)
        db.AddInParameter(dbCommand, "@p_SinonimoEmisor", DbType.String, sinonimoEmisor)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_NumeroDocumento", DbType.Decimal, numeroDocumento)
        db.AddInParameter(dbCommand, "@p_Direccion", DbType.String, direccion)
        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.Decimal, codigoPais)
        db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, codigoTipoDocumento)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, codigoPostal)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de Emisor tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoEmisor As Decimal) As EmisorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.Decimal, codigoEmisor)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Emisor tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoSectorEmpresarial"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoSectorEmpresarial(ByVal codigoSectorEmpresarial As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_SeleccionarPorCodigoSectorEmpresarial")

        db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Emisor tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTipoDocumento"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTipoDocumento(ByVal codigoTipoDocumento As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_SeleccionarPorCodigoTipoDocumento")

        db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, codigoTipoDocumento)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Emisor tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPais"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoPais(ByVal codigoPais As Decimal) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_SeleccionarPorCodigoPais")

        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.Decimal, codigoPais)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Emisor tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMercado"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMercado(ByVal codigoMercado As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_SeleccionarPorCodigoMercado")

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Emisor tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPostal"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoPostal(ByVal codigoPostal As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_SeleccionarPorCodigoPostal")

        db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, codigoPostal)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Midifica un expediente en Emisor tabla.
    ''' <summary>
    ''' <param name="codigoEmisor"></param>
    ''' <param name="sinonimoEmisor"></param>
    ''' <param name="descripcion"></param>
    ''' <param name="numeroDocumento"></param>
    ''' <param name="direccion"></param>
    ''' <param name="codigoPais"></param>
    ''' <param name="codigoTipoDocumento"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="codigoMercado"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="codigoPostal"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="codigoSectorEmpresarial"></param>
    ''' <param name="situacion"></param>
    ''' <param name="host"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    Public Function Modificar(ByVal codigoEmisor As Decimal, ByVal sinonimoEmisor As String, ByVal descripcion As String, ByVal numeroDocumento As Decimal, ByVal direccion As String, ByVal codigoPais As Decimal, ByVal codigoTipoDocumento As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal codigoMercado As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal codigoPostal As String, ByVal horaCreacion As String, ByVal codigoSectorEmpresarial As String, ByVal situacion As String, ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.Decimal, codigoEmisor)
        db.AddInParameter(dbCommand, "@p_SinonimoEmisor", DbType.String, sinonimoEmisor)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_NumeroDocumento", DbType.Decimal, numeroDocumento)
        db.AddInParameter(dbCommand, "@p_Direccion", DbType.String, direccion)
        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.Decimal, codigoPais)
        db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, codigoTipoDocumento)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, codigoPostal)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Emisor table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoEmisor As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.Decimal, codigoEmisor)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Emisor table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoSectorEmpresarial(ByVal codigoSectorEmpresarial As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_EliminarPorCodigoSectorEmpresarial")

        db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Emisor table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTipoDocumento(ByVal codigoTipoDocumento As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_EliminarPorCodigoTipoDocumento")

        db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, codigoTipoDocumento)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Emisor table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPais(ByVal codigoPais As Decimal) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_EliminarPorCodigoPais")

        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.Decimal, codigoPais)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Emisor table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMercado(ByVal codigoMercado As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_EliminarPorCodigoMercado")

        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Emisor table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPostal(ByVal codigoPostal As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Emisor_EliminarPorCodigoPostal")

        db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, codigoPostal)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
End Class

