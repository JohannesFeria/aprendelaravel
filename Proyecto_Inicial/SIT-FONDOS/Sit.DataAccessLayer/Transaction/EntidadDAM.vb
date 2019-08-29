Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class EntidadDAM

    Private sqlCommand As String = ""
    Private oEntidadRow As EntidadBE.EntidadRow
    Private oEntidadExcesosRow As EntidadExcesosBE.EntidadExcesosRow    'HDG OT 60022 20100709
    Private strVacio As String = ""
    Private decVacio As String = 0
    Public Sub New()

    End Sub

    Public Function Seleccionar(ByVal codigoEntidad As String) As EntidadBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_Seleccionar")
        Dim oEntidadBE As New EntidadBE

        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, codigoEntidad)

        db.LoadDataSet(dbCommand, oEntidadBE, "Entidad")

        Dim oDS As New DataSet
        oDS = db.ExecuteDataSet(dbCommand)

        Dim oRow As EntidadBE.EntidadRow
        For Each dr As DataRow In oDS.Tables(0).Rows
            oRow = CType(oEntidadBE.Entidad.NewRow(), EntidadBE.EntidadRow)

            oRow.CodigoBeneficiario = IIf(dr("CodigoBeneficiario") Is DBNull.Value, strVacio, dr("CodigoBeneficiario"))
            oRow.CodigoCustodio = IIf(dr("CodigoCustodio") Is DBNull.Value, strVacio, dr("CodigoCustodio"))
            oRow.CodigoEntidad = IIf(dr("CodigoEntidad") Is DBNull.Value, strVacio, dr("CodigoEntidad"))
            oRow.CodigoEntidadFinanciera = IIf(dr("CodigoEntidadFinanciera") Is DBNull.Value, strVacio, dr("CodigoEntidadFinanciera"))
            oRow.CodigoGrupoEconomico = IIf(dr("CodigoGrupoEconomico") Is DBNull.Value, strVacio, dr("CodigoGrupoEconomico"))
            oRow.CodigoPais = IIf(dr("CodigoPais") Is DBNull.Value, strVacio, dr("CodigoPais"))
            oRow.CodigoMercado = IIf(dr("CodigoMercado") Is DBNull.Value, strVacio, dr("CodigoMercado"))
            oRow.CodigoPostal = IIf(dr("CodigoPostal") Is DBNull.Value, strVacio, dr("CodigoPostal"))
            oRow.CodigoSBS = IIf(dr("CodigoSBS") Is DBNull.Value, strVacio, dr("CodigoSBS"))
            oRow.CodigoTercero = IIf(dr("CodigoTercero") Is DBNull.Value, strVacio, dr("CodigoTercero"))
            oRow.CodigoTipoEntidad = IIf(dr("CodigoTipoEntidad") Is DBNull.Value, strVacio, dr("CodigoTipoEntidad"))
            oRow.Direccion = IIf(dr("Direccion") Is DBNull.Value, strVacio, dr("Direccion"))
            oRow.EntidadAval = IIf(dr("EntidadAval") Is DBNull.Value, decVacio, dr("EntidadAval"))
            oRow.EntidadBroker = IIf(dr("EntidadBroker") Is DBNull.Value, strVacio, dr("EntidadBroker"))
            oRow.EntidadComisionista = IIf(dr("EntidadComisionista") Is DBNull.Value, strVacio, dr("EntidadComisionista"))
            oRow.EntidadCustodio = IIf(dr("EntidadCustodio") Is DBNull.Value, strVacio, dr("EntidadCustodio"))
            oRow.EntidadEmisora = IIf(dr("EntidadEmisora") Is DBNull.Value, strVacio, dr("EntidadEmisora"))
            oRow.EntidadFinanciera = IIf(dr("EntidadFinanciera") Is DBNull.Value, strVacio, dr("EntidadFinanciera"))
            oRow.EntidadReguladora = IIf(dr("EntidadReguladora") Is DBNull.Value, strVacio, dr("EntidadReguladora"))
            oRow.EntidadRiesgocero = IIf(dr("EntidadRiesgocero") Is DBNull.Value, strVacio, dr("EntidadRiesgocero"))
            oRow.EntidadVigilada = IIf(dr("EntidadVigilada") Is DBNull.Value, strVacio, dr("EntidadVigilada"))
            oRow.EntidadVinculada = IIf(dr("EntidadVinculada") Is DBNull.Value, strVacio, dr("EntidadVinculada"))
            oRow.NombreBeneficiario = IIf(dr("NombreBeneficiario") Is DBNull.Value, strVacio, dr("NombreBeneficiario"))
            oRow.NombreCompleto = IIf(dr("NombreCompleto") Is DBNull.Value, strVacio, dr("NombreCompleto"))
            oRow.NombreSituacion = IIf(dr("NombreSituacion") Is DBNull.Value, strVacio, dr("NombreSituacion"))
            oRow.Observaciones = IIf(dr("Observaciones") Is DBNull.Value, strVacio, dr("Observaciones"))
            oRow.PorcentajeComision = IIf(dr("PorcentajeComision") Is DBNull.Value, decVacio, dr("PorcentajeComision"))
            oRow.Situacion = IIf(dr("Situacion") Is DBNull.Value, strVacio, dr("Situacion"))
            oRow.TipoTercero = IIf(dr("TipoTercero") Is DBNull.Value, strVacio, dr("TipoTercero"))
            oRow.Sinonimo = IIf(dr("Sinonimo") Is DBNull.Value, strVacio, dr("Sinonimo"))
            oRow.CodigoBroker = IIf(dr("CodigoBroker") Is DBNull.Value, strVacio, dr("CodigoBroker"))
            'RGF 20080822
            oRow.UnidadesEmitidas = IIf(dr("UnidadesEmitidas") Is DBNull.Value, Nothing, dr("UnidadesEmitidas"))

            'LETV 20090324
            'oRow.TipoTramo = IIf(dr("TipoTramo") Is DBNull.Value, "", dr("TipoTramo"))
            'oRow.Tramo = IIf(dr("Tramo") Is DBNull.Value, "", dr("Tramo"))
            'oRow.BandaInferior = IIf(dr("BandaInferior") Is DBNull.Value, 0, dr("BandaInferior"))
            'oRow.BandaSuperior = IIf(dr("BandaSuperior") Is DBNull.Value, 0, dr("BandaSuperior"))
            'oRow.Costo = IIf(dr("Costo") Is DBNull.Value, 0, dr("Costo"))
            'oRow.TipoCosto = IIf(dr("TipoCosto") Is DBNull.Value, "", dr("TipoCosto"))



            oEntidadBE.Entidad.AddEntidadRow(oRow)
            oEntidadBE.Entidad.AcceptChanges()
        Next

        Return oEntidadBE


    End Function
    Public Function ExisteTercero(ByVal CodigoEntidad As String, ByVal CodigoTercero As String, ByVal FlagIngreso As String, ByVal datarequest As DataSet) As EntidadBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_ExisteTercero")
        db.AddInParameter(dbCommand, "@p_codigoEntidad", DbType.String, CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_codigoTercero", DbType.String, CodigoTercero)
        db.AddInParameter(dbCommand, "@p_codigoSBS", DbType.String, "") 'RGF 20080917
        db.AddInParameter(dbCommand, "@p_FlagIngreso", DbType.String, FlagIngreso)

        Dim oEntidadBE As New EntidadBE

        db.LoadDataSet(dbCommand, oEntidadBE, "Entidad")

        Return oEntidadBE

    End Function
    Public Function ExisteCodigoSBS(ByVal CodigoEntidad As String, ByVal CodigoSBS As String, ByVal FlagIngreso As String, ByVal datarequest As DataSet) As EntidadBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        'RGF 20081209 Estaba llamando al procedure incorrecto
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_ExisteCodigoSBS")
        'Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_ExisteTercero")
        db.AddInParameter(dbCommand, "@p_codigoEntidad", DbType.String, CodigoEntidad)
        'db.AddInParameter(dbCommand, "@p_codigoTercero", DbType.String, "")
        db.AddInParameter(dbCommand, "@p_codigoSBS", DbType.String, CodigoSBS) 'RGF 20080917
        db.AddInParameter(dbCommand, "@p_FlagIngreso", DbType.String, FlagIngreso)

        Dim oEntidadBE As New EntidadBE

        db.LoadDataSet(dbCommand, oEntidadBE, "Entidad")

        Return oEntidadBE

    End Function

    Public Function Listar() As EntidadBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_Listar")
        Dim oEntidadBE As New EntidadBE

        db.LoadDataSet(dbCommand, oEntidadBE, "Entidad")

        Return oEntidadBE

    End Function

    Public Function ListarCustodio() As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_ListarCustodio")
        Dim oDS As New DataSet

        db.LoadDataSet(dbCommand, oDS, "Entidad")

        Return oDS

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoEntidad As String, ByVal codigoIdentificacion As String, ByVal codigoPostal As String, ByVal codigoMercado As String, ByVal codigoSectorEmpresarial As String, ByVal tipoTercero As String, ByVal situacion As String, ByVal descripcion As String) As EntidadBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_SeleccionarPorFiltro")
        Dim oEntidadBE As New EntidadBE

        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, codigoEntidad)
        db.AddInParameter(dbCommand, "@p_CodigoIdentificacion", DbType.String, codigoIdentificacion)
        db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, codigoPostal)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)
        db.AddInParameter(dbCommand, "@p_CodigoTipoTercero", DbType.String, tipoTercero)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)

        db.LoadDataSet(dbCommand, oEntidadBE, "Entidad")

        Return oEntidadBE

    End Function
    '==============================================================
    ' CREADO POR  : Zoluxiones Consulting S.A.C (JVC)
    ' NÚMERO DE OT: 
    ' DESCRIPCIÓN : Obtiene datos de intermediarios
    ' FECHA DE CREACIÓN : 09/03/2009
    ' PARÁMETROS ENTRADA: CodigoIntermediario: Código del intermediario
    '  	                  Situacion	        : Situación
    '	                  EntidadBroker      : Entidad de broker
    '================================================================*/
    Public Function Entidad_Listar(ByVal CodigoIntermediario As String, _
                                   ByVal Situacion As String, _
                                   ByVal EntidadBroker As String) As EntidadSitBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_Listar_Sit")
        Dim oEntidadBE As New EntidadSitBE
        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, CodigoIntermediario)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
        db.AddInParameter(dbCommand, "@p_EntidadBroker", DbType.String, EntidadBroker)
        db.LoadDataSet(dbCommand, oEntidadBE, "Entidad")
        Return oEntidadBE
    End Function

    Public Function Insertar(ByVal oEntidadBE As EntidadBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_Insertar")

        Dim oEntidadRow As EntidadBE.EntidadRow

        oEntidadRow = DirectCast(oEntidadBE.Entidad.Rows(0), EntidadBE.EntidadRow)

        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, oEntidadRow.CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_CodigoTipoEntidad", DbType.String, oEntidadRow.CodigoTipoEntidad)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oEntidadRow.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, oEntidadRow.CodigoSBS)
        db.AddInParameter(dbCommand, "@p_CodigoGrupoEconomico", DbType.String, oEntidadRow.CodigoGrupoEconomico)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oEntidadRow.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_EntidadEmisora", DbType.String, oEntidadRow.EntidadEmisora)
        db.AddInParameter(dbCommand, "@p_EntidadComisionista", DbType.String, oEntidadRow.EntidadComisionista)
        db.AddInParameter(dbCommand, "@p_EntidadCustodio", DbType.String, oEntidadRow.EntidadCustodio)
        db.AddInParameter(dbCommand, "@p_EntidadFinanciera", DbType.String, oEntidadRow.EntidadFinanciera)
        db.AddInParameter(dbCommand, "@p_EntidadAval", DbType.String, oEntidadRow.EntidadAval)
        db.AddInParameter(dbCommand, "@p_EntidadRiesgocero", DbType.String, oEntidadRow.EntidadRiesgocero)
        db.AddInParameter(dbCommand, "@p_EntidadVinculada", DbType.String, oEntidadRow.EntidadVinculada)
        db.AddInParameter(dbCommand, "@p_EntidadVigilada", DbType.String, oEntidadRow.EntidadVinculada)
        db.AddInParameter(dbCommand, "@p_EntidadReguladora", DbType.String, oEntidadRow.EntidadReguladora)
        db.AddInParameter(dbCommand, "@p_EntidadBroker", DbType.String, oEntidadRow.EntidadBroker)
        db.AddInParameter(dbCommand, "@p_Observaciones", DbType.String, oEntidadRow.Observaciones)
        db.AddInParameter(dbCommand, "@p_PorcentajeComision", DbType.Decimal, oEntidadRow.PorcentajeComision)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oEntidadRow.Situacion)
        db.AddInParameter(dbCommand, "@p_CodigoEntidadFinanciera", DbType.String, oEntidadRow.CodigoEntidadFinanciera)
        db.AddInParameter(dbCommand, "@p_CodigoBeneficiario", DbType.String, oEntidadRow.CodigoBeneficiario)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oEntidadRow.CodigoPortafolio)

        If oEntidadRow.FactorCastigo = -1 Then
            db.AddInParameter(dbCommand, "@p_FactorCastigo", DbType.Decimal, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_FactorCastigo", DbType.Decimal, oEntidadRow.FactorCastigo)
        End If

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_Sinonimo", DbType.String, IIf(oEntidadRow.Sinonimo = "", DBNull.Value, oEntidadRow.Sinonimo))
        db.AddInParameter(dbCommand, "@p_CodigoBroker", DbType.String, IIf(oEntidadRow.CodigoBroker = "", DBNull.Value, oEntidadRow.CodigoBroker))

        'RGF 20080822
        db.AddInParameter(dbCommand, "@p_UnidadesEmitidas", DbType.Decimal, IIf(oEntidadRow.UnidadesEmitidas = Nothing, DBNull.Value, oEntidadRow.UnidadesEmitidas))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function Modificar(ByVal oEntidadBE As EntidadBE, ByVal sinonimo As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_Modificar")

        Dim oEntidadRow As EntidadBE.EntidadRow

        oEntidadRow = DirectCast(oEntidadBE.Entidad.Rows(0), EntidadBE.EntidadRow)

        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, oEntidadRow.CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_CodigoTipoEntidad", DbType.String, oEntidadRow.CodigoTipoEntidad)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oEntidadRow.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, oEntidadRow.CodigoSBS)
        db.AddInParameter(dbCommand, "@p_CodigoGrupoEconomico", DbType.String, oEntidadRow.CodigoGrupoEconomico)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oEntidadRow.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_EntidadEmisora", DbType.String, oEntidadRow.EntidadEmisora)
        db.AddInParameter(dbCommand, "@p_EntidadComisionista", DbType.String, oEntidadRow.EntidadComisionista)
        db.AddInParameter(dbCommand, "@p_EntidadCustodio", DbType.String, oEntidadRow.EntidadCustodio)
        db.AddInParameter(dbCommand, "@p_EntidadFinanciera", DbType.String, oEntidadRow.EntidadFinanciera)
        db.AddInParameter(dbCommand, "@p_EntidadAval", DbType.String, oEntidadRow.EntidadAval)
        db.AddInParameter(dbCommand, "@p_EntidadRiesgocero", DbType.String, oEntidadRow.EntidadRiesgocero)
        db.AddInParameter(dbCommand, "@p_EntidadVinculada", DbType.String, oEntidadRow.EntidadVinculada)
        db.AddInParameter(dbCommand, "@p_EntidadVigilada", DbType.String, oEntidadRow.EntidadVigilada)
        db.AddInParameter(dbCommand, "@p_EntidadReguladora", DbType.String, oEntidadRow.EntidadReguladora)
        db.AddInParameter(dbCommand, "@p_EntidadBroker", DbType.String, oEntidadRow.EntidadBroker)
        db.AddInParameter(dbCommand, "@p_Observaciones", DbType.String, oEntidadRow.Observaciones)
        db.AddInParameter(dbCommand, "@p_PorcentajeComision", DbType.Decimal, oEntidadRow.PorcentajeComision)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oEntidadRow.Situacion)
        db.AddInParameter(dbCommand, "@p_CodigoEntidadFinanciera", DbType.String, oEntidadRow.CodigoEntidadFinanciera)
        db.AddInParameter(dbCommand, "@p_CodigoBeneficiario", DbType.String, oEntidadRow.CodigoBeneficiario)

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oEntidadRow.CodigoPortafolio)

        If oEntidadRow.FactorCastigo = -1 Then
            db.AddInParameter(dbCommand, "@p_FactorCastigo", DbType.Decimal, DBNull.Value)
        Else
            db.AddInParameter(dbCommand, "@p_FactorCastigo", DbType.Decimal, oEntidadRow.FactorCastigo)
        End If

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_Sinonimo", DbType.String, IIf(sinonimo = "", DBNull.Value, sinonimo))
        db.AddInParameter(dbCommand, "@p_CodigoBroker", DbType.String, IIf(oEntidadRow.CodigoBroker = "", DBNull.Value, oEntidadRow.CodigoBroker))

        'RGF 20080822
        db.AddInParameter(dbCommand, "@p_UnidadesEmitidas", DbType.Decimal, IIf(oEntidadRow.UnidadesEmitidas = Nothing, DBNull.Value, oEntidadRow.UnidadesEmitidas))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function Eliminar(ByVal codigoEntidad As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Entidad_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, codigoEntidad)
            db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function

    Public Function Eliminar_Registro_Fisico(ByVal codigoEntidad As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_Eliminar_Fisica")
        Dim exito As Boolean = False

        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, codigoEntidad)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        Try

            db.ExecuteNonQuery(dbCommand)
            exito = True

        Catch exSql As SqlClient.SqlException

            Throw exSql

        Catch ex As Exception

            Throw ex
        End Try

        Return exito

    End Function

#Region " /* Broker Comisiones (Req25) LETV 20090401 */ "

    Public Function ListarBroker(ByVal CodigoEntidad As String, ByVal Descripcion As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_SeleccionarBroker")
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, Descripcion)
        Dim oDS As New DataSet
        db.LoadDataSet(dbCommand, oDS, "Broker")

        Return oDS.Tables("Broker")

    End Function

    Public Function ListarTramoBroker(ByVal CodigoEntidad As String, ByVal descripcion As String, ByVal situacion As String, ByVal tipotramo As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EntidadTramo_SeleccionarBroker")
        Dim oDS As New DataSet
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_tipotramo", DbType.String, tipotramo)
        db.LoadDataSet(dbCommand, oDS, "Broker")

        Return oDS.Tables("Broker")

    End Function

    Public Function InsertarTramoBroker(ByVal oEntidadBE As EntidadBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EntidadTramo_Insertar")
        Dim oEntidadRow As EntidadBE.EntidadRow

        oEntidadRow = DirectCast(oEntidadBE.Entidad.Rows(0), EntidadBE.EntidadRow)
        db.AddInParameter(dbCommand, "@p_Tramo", DbType.String, oEntidadRow.Tramo)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, oEntidadRow.CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, oEntidadRow.TipoTramo)
        db.AddInParameter(dbCommand, "@p_BandaInferior", DbType.Decimal, oEntidadRow.BandaInferior)
        db.AddInParameter(dbCommand, "@p_BandaSuperior", DbType.Decimal, oEntidadRow.BandaSuperior)
        db.AddInParameter(dbCommand, "@p_Costo", DbType.Decimal, oEntidadRow.Costo)
        db.AddInParameter(dbCommand, "@p_TipoCosto", DbType.String, oEntidadRow.TipoCosto)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oEntidadRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function ExisteTramo(ByVal Tramo As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EntidadTramo_Existe")
        db.AddInParameter(dbCommand, "@p_tramo", DbType.String, Tramo)
        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "TramoBroker")
        Dim resultado As Boolean
        If (ds.Tables(0).Rows.Count > 0) Then
            resultado = True
        Else
            resultado = False
        End If
        Return resultado
    End Function

    Public Function ModificarTramoBroker(ByVal oEntidadBE As EntidadBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EntidadTramo_Modificar")

        Dim oEntidadRow As EntidadBE.EntidadRow

        oEntidadRow = DirectCast(oEntidadBE.Entidad.Rows(0), EntidadBE.EntidadRow)

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oEntidadRow.Situacion)
        db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, oEntidadRow.TipoTramo)
        db.AddInParameter(dbCommand, "@p_Tramo", DbType.String, oEntidadRow.Tramo)
        db.AddInParameter(dbCommand, "@p_BandaInferior", DbType.Decimal, oEntidadRow.BandaInferior)
        db.AddInParameter(dbCommand, "@p_BandaSuperior", DbType.Decimal, oEntidadRow.BandaSuperior)
        db.AddInParameter(dbCommand, "@p_Costo", DbType.Decimal, oEntidadRow.Costo)
        db.AddInParameter(dbCommand, "@p_TipoCosto", DbType.String, oEntidadRow.TipoCosto)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function SeleccionarTramoBroker(ByVal tramo As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EntidadTramo_Seleccionar")
        Dim ds As New DataSet

        db.AddInParameter(dbCommand, "@p_tramo", DbType.String, tramo)

        db.LoadDataSet(dbCommand, ds, "EntidadTramo")

        Return ds.Tables("EntidadTramo")
    End Function

    Public Function EliminarTramoBroker(ByVal Tramo As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EntidadTramo_Eliminar")

        db.AddInParameter(dbCommand, "@p_Tramo", DbType.String, Tramo)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

    Public Function SeleccionarCostoBroker(ByVal NroAcciones As Decimal, ByVal CodigoIntermediario As String, ByVal TipoTramo As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EntidadTramo_SeleccionarCosto")
        Dim ds As New DataSet

        db.AddInParameter(dbCommand, "@p_NroAcciones", DbType.Decimal, NroAcciones)
        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, CodigoIntermediario)
        db.AddInParameter(dbCommand, "@p_TipoTramo", DbType.String, TipoTramo)

        db.LoadDataSet(dbCommand, ds, "CostoBroker")

        Return ds.Tables("CostoBroker")
    End Function
#End Region

#Region " /* Broker Excesos HDG OT 60022 20100707 */ "

    Public Function SeleccionarExcesosBroker(ByVal CodigoEntidad As String, ByVal descripcion As String, ByVal situacion As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_EntidadExcesos_SeleccionarBroker")
        Dim oDS As New DataSet
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_situacion", DbType.String, situacion)
        db.LoadDataSet(dbCommand, oDS, "EntidadExcesos")

        Return oDS.Tables("EntidadExcesos")

    End Function

    Public Function InsertarExcesosBroker(ByVal oEntidadExcesosBE As EntidadExcesosBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_EntidadExcesos_insertar")
        Dim oEntidadExcesosRow As EntidadExcesosBE.EntidadExcesosRow

        oEntidadExcesosRow = DirectCast(oEntidadExcesosBE.EntidadExcesos.Rows(0), EntidadExcesosBE.EntidadExcesosRow)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, oEntidadExcesosRow.CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_MontoRestriccion", DbType.Decimal, oEntidadExcesosRow.MontoRestriccion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oEntidadExcesosRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function ModificarExcesosBroker(ByVal oEntidadExcesosBE As EntidadExcesosBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_EntidadExcesos_modificar")
        Dim oEntidadExcesosRow As EntidadExcesosBE.EntidadExcesosRow

        oEntidadExcesosRow = DirectCast(oEntidadExcesosBE.EntidadExcesos.Rows(0), EntidadExcesosBE.EntidadExcesosRow)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, oEntidadExcesosRow.CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oEntidadExcesosRow.Situacion)
        db.AddInParameter(dbCommand, "@p_MontoRestriccion", DbType.Decimal, oEntidadExcesosRow.MontoRestriccion)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function EliminarExcesosBroker(ByVal CodigoEntidad As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_EntidadExcesos_eliminar")

        db.AddInParameter(dbCommand, "@p_codigoentidad", DbType.String, CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

#End Region


#Region " /* Funciones Seleccionar */ "

    Public Function ListarEntidadFinanciera(ByVal dataRequest As DataSet) As EntidadBE

        Dim objeto As New EntidadBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_ListarEntidadFinanciera")

        db.LoadDataSet(dbCommand, objeto, "Entidad")

        Return objeto

    End Function

    Public Function SeleccionarPorCodigoTercero(ByVal codigoTercero As String) As DataSet
        Dim objeto As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Entidad_SeleccionarPorTercero")
            db.AddInParameter(dbCommand, "@p_codigoTercero", DbType.String, codigoTercero)
            db.LoadDataSet(dbCommand, objeto, "Entidad")
        End Using
        Return objeto
    End Function

    Public Function SeleccionarPorReferenciaValores(ByVal codigoEntidad As String) As DataSet
        Dim objeto As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Entidad_Obtener_Referencia_Valores")
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, codigoEntidad)
        db.LoadDataSet(dbCommand, objeto, "Entidad")
        Return objeto
    End Function

#End Region

End Class

