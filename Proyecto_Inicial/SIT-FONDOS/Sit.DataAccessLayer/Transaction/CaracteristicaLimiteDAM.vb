Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para CaracteristicaLimite tabla.
	''' </summary>
	Public class CaracteristicaLimiteDAM
    Private sqlCommand As String = ""
    Private oCaracteristicaLimiteRow As CaracteristicaLimiteBE.CaracteristicaLimiteRow
		Public Sub New()

		End Sub
#Region "/* Funciones Seleccionar */"
    Public Function Seleccionar(ByVal codigoCustodio As String) As CaracteristicaLimiteBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimite_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, codigoCustodio)

        Dim objeto As New CaracteristicaLimiteBE
        db.LoadDataSet(dbCommand, objeto, "CaracteristicaLimite")
        Return objeto
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As CaracteristicaLimiteBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimite_Listar")

        Dim objeto As New CaracteristicaLimiteBE
        db.LoadDataSet(dbCommand, objeto, "CaracteristicaLimite")
        Return objeto
    End Function
    Public Function SeleccionarPorFiltro(ByVal StrCodigoNegocio As String, ByVal StrTipoLim As String, ByVal StrCodigoPortafolio As String, ByVal StrClaseLim As String, ByVal dataRequest As DataSet) As CaracteristicaLimiteBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimite_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, StrCodigoNegocio)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, StrCodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_TipoLim", DbType.String, StrTipoLim)
        db.AddInParameter(dbCommand, "@p_ClaseLim", DbType.String, StrClaseLim)

        Dim objeto As New CaracteristicaLimiteBE
        db.LoadDataSet(dbCommand, objeto, "CaracteristicaLimite")
        Return objeto
    End Function
#End Region
    ''' <summary>
    ''' Inserta un expediente en CaracteristicaLimite tabla.
    ''' <summary>
    ''' <param name="codigoLimite"></param>
    ''' <param name="tipoCalculo"></param>
    ''' <param name="tope"></param>
    ''' <param name="unidadPosicion"></param>
    ''' <param name="valorBase"></param>
    ''' <param name="claseLimite"></param>
    ''' <param name="tipoLimite"></param>
    ''' <param name="factor"></param>
    ''' <param name="aplicarCastigo"></param>
    ''' <param name="saldoBanco"></param>
    ''' <param name="codigoPortafolio"></param>
    ''' <param name="situacion"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="host"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="codigoNegocio"></param>
    ''' <param name="horaEliminacion"></param>
    ''' <returns></returns>
    Public Function Insertar(ByVal codigoLimite As String, ByVal tipoCalculo As String, ByVal tope As String, ByVal unidadPosicion As String, ByVal valorBase As String, ByVal claseLimite As String, ByVal tipoLimite As String, ByVal factor As String, ByVal aplicarCastigo As String, ByVal saldoBanco As String, ByVal codigoPortafolio As String, ByVal situacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal codigoNegocio As String, ByVal horaEliminacion As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimite_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)
        db.AddInParameter(dbCommand, "@p_TipoCalculo", DbType.String, tipoCalculo)
        db.AddInParameter(dbCommand, "@p_Tope", DbType.String, tope)
        db.AddInParameter(dbCommand, "@p_UnidadPosicion", DbType.String, unidadPosicion)
        db.AddInParameter(dbCommand, "@p_ValorBase", DbType.String, valorBase)
        db.AddInParameter(dbCommand, "@p_ClaseLimite", DbType.String, claseLimite)
        db.AddInParameter(dbCommand, "@p_TipoLimite", DbType.String, tipoLimite)
        db.AddInParameter(dbCommand, "@p_Factor", DbType.String, factor)
        db.AddInParameter(dbCommand, "@p_AplicarCastigo", DbType.String, aplicarCastigo)
        db.AddInParameter(dbCommand, "@p_SaldoBanco", DbType.String, saldoBanco)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
    End Function





    ''' <summary>
    ''' Midifica un expediente en CaracteristicaLimite tabla.
    ''' <summary>
    ''' <param name="codigoLimite"></param>
    ''' <param name="tipoCalculo"></param>
    ''' <param name="tope"></param>
    ''' <param name="unidadPosicion"></param>
    ''' <param name="valorBase"></param>
    ''' <param name="claseLimite"></param>
    ''' <param name="tipoLimite"></param>
    ''' <param name="factor"></param>
    ''' <param name="aplicarCastigo"></param>
    ''' <param name="saldoBanco"></param>
    ''' <param name="codigoPortafolio"></param>
    ''' <param name="situacion"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="host"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="codigoNegocio"></param>
    ''' <param name="horaEliminacion"></param>
    Public Function Modificar(ByVal codigoLimite As String, ByVal tipoCalculo As String, ByVal tope As String, ByVal unidadPosicion As String, ByVal valorBase As String, ByVal claseLimite As String, ByVal tipoLimite As String, ByVal factor As String, ByVal aplicarCastigo As String, ByVal saldoBanco As String, ByVal codigoPortafolio As String, ByVal situacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal codigoNegocio As String, ByVal horaEliminacion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimite_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)
        db.AddInParameter(dbCommand, "@p_TipoCalculo", DbType.String, tipoCalculo)
        db.AddInParameter(dbCommand, "@p_Tope", DbType.String, tope)
        db.AddInParameter(dbCommand, "@p_UnidadPosicion", DbType.String, unidadPosicion)
        db.AddInParameter(dbCommand, "@p_ValorBase", DbType.String, valorBase)
        db.AddInParameter(dbCommand, "@p_ClaseLimite", DbType.String, claseLimite)
        db.AddInParameter(dbCommand, "@p_TipoLimite", DbType.String, tipoLimite)
        db.AddInParameter(dbCommand, "@p_Factor", DbType.String, factor)
        db.AddInParameter(dbCommand, "@p_AplicarCastigo", DbType.String, aplicarCastigo)
        db.AddInParameter(dbCommand, "@p_SaldoBanco", DbType.String, saldoBanco)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CaracteristicaLimite table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoLimite As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimite_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CaracteristicaLimite table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoNegocio(ByVal codigoNegocio As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimite_EliminarPorCodigoNegocio")

        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CaracteristicaLimite table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimite_EliminarPorCodigoPortafolio")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de CaracteristicaLimite table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoLimite(ByVal codigoLimite As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CaracteristicaLimite_EliminarPorCodigoLimite")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
End Class

