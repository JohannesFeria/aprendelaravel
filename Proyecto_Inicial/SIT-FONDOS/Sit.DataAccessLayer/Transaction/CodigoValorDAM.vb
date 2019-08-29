Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Public Class CodigoValorDAM
    Private oOrdenInversionRow As CodigoValorBE.CodigoValorBERow
    Public Function InsertarCodigoValor(ByVal objCodigoValor As CodigoValorBE, ByVal dataRequest As DataSet) As Boolean
        InsertarCodigoValor = False
        oOrdenInversionRow = CType(objCodigoValor._CodigoValorBE.Rows(0), CodigoValorBE.CodigoValorBERow)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ins_CodigoValor")
            db.AddInParameter(dbCommand, "@CodigoValor", DbType.String, oOrdenInversionRow.CodigoValor)
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, oOrdenInversionRow.CodigoNemonico)
            db.AddInParameter(dbCommand, "@CodigoTercero", DbType.String, oOrdenInversionRow.CodigoEntidad)
            db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, oOrdenInversionRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@Situacion", DbType.String, oOrdenInversionRow.Situacion)
            db.AddInParameter(dbCommand, "@Opcion", DbType.String, oOrdenInversionRow.Opcion)
            db.AddInParameter(dbCommand, "@CodigoTipoInstrumentoSMV", DbType.String, oOrdenInversionRow.TipoInstrumento)
            db.AddInParameter(dbCommand, "@CodigoTipoCupon", DbType.String, oOrdenInversionRow.CodigoTipoCupon)
            db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            InsertarCodigoValor = True
        End Using
    End Function
    'OT11008 - 18/01/2018 - Ian Pastor M.
    'Cambiar parámetro código valor por el Id de la tabla
    Public Function ModificarCodigoValor(ByVal objCodigoValor As CodigoValorBE, ByVal dataRequest As DataSet) As Boolean
        ModificarCodigoValor = False
        oOrdenInversionRow = CType(objCodigoValor._CodigoValorBE.Rows(0), CodigoValorBE.CodigoValorBERow)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_mod_CodigoValor")
            db.AddInParameter(dbCommand, "@Id", DbType.Int32, oOrdenInversionRow.Id)
            db.AddInParameter(dbCommand, "@CodigoValor", DbType.String, oOrdenInversionRow.CodigoValor)
            db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, oOrdenInversionRow.CodigoNemonico)
            db.AddInParameter(dbCommand, "@CodigoTercero", DbType.String, oOrdenInversionRow.CodigoEntidad)
            db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, oOrdenInversionRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@Situacion", DbType.String, oOrdenInversionRow.Situacion)
            db.AddInParameter(dbCommand, "@Opcion", DbType.String, oOrdenInversionRow.Opcion)
            db.AddInParameter(dbCommand, "@CodigoTipoInstrumentoSMV", DbType.String, oOrdenInversionRow.TipoInstrumento)
            db.AddInParameter(dbCommand, "@CodigoTipoCupon", DbType.String, oOrdenInversionRow.CodigoTipoCupon)
            db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            ModificarCodigoValor = True
        End Using
    End Function
    'OT11008 - 18/01/2018 - Ian Pastor M.
    'Cambiar parámetro código valor por el Id de la tabla
    Public Function EliminarCodigoValor(ByVal id As String, ByVal dataRequest As DataSet) As Boolean
        EliminarCodigoValor = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_del_CodigoValor")
            'db.AddInParameter(dbCommand, "@CodigoValor", DbType.String, id)
            db.AddInParameter(dbCommand, "@Id", DbType.Int32, id)
            db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            EliminarCodigoValor = True
        End Using
    End Function
    Public Function ListarCodigoValor(Optional ByVal CodigoValor As String = "", Optional ByVal Situacion As String = "", Optional ByVal CodigoNemonico As String = "", _
                                      Optional ByVal CodigoEntidad As String = "", Optional ByVal CodigoMoneda As String = "") As CodigoValorBE
        Try
            Dim oCodigoValorBE As New CodigoValorBE
            Dim oRow As CodigoValorBE.CodigoValorBERow
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_Listar_CodigoValor")
                db.AddInParameter(dbCommand, "@CodigoValor", DbType.String, CodigoValor)
                db.AddInParameter(dbCommand, "@Situacion", DbType.String, Situacion)
                db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, CodigoNemonico)
                db.AddInParameter(dbCommand, "@CodigoTercero", DbType.String, CodigoEntidad)
                db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, CodigoMoneda)
                db.AddInParameter(dbCommand, "@CodigoTipoCupon", DbType.String, "")
                Using dsOI As DataSet = db.ExecuteDataSet(dbCommand)
                    For Each dr As DataRow In dsOI.Tables(0).Rows
                        oRow = oCodigoValorBE._CodigoValorBE.NewRow()
                        oRow.CodigoValor = IIf(dr("CodigoValor") Is DBNull.Value, "", dr("CodigoValor"))
                        oRow.CodigoNemonico = IIf(dr("CodigoNemonico") Is DBNull.Value, "", dr("CodigoNemonico"))
                        oRow.CodigoEntidad = IIf(dr("CodigoEntidad") Is DBNull.Value, "", dr("CodigoEntidad"))
                        oRow.Sinonimo = IIf(dr("sinonimo") Is DBNull.Value, "", dr("sinonimo"))
                        oRow.CodigoMoneda = IIf(dr("CodigoMoneda") Is DBNull.Value, "", dr("CodigoMoneda"))
                        oRow.Situacion = IIf(dr("Situacion") Is DBNull.Value, "", dr("Situacion"))
                        oRow.Dsituacion = IIf(dr("DSituacion") Is DBNull.Value, "", dr("DSituacion"))
                        oRow.UsuarioCreacion = IIf(dr("UsuarioCreacion") Is DBNull.Value, "", dr("UsuarioCreacion"))
                        oRow.FechaCreacion = IIf(dr("FechaCreacion") Is DBNull.Value, "", dr("FechaCreacion"))
                        oRow.HoraCreacion = IIf(dr("HoraCreacion") Is DBNull.Value, "", dr("HoraCreacion"))
                        oRow.UsuarioModificacion = IIf(dr("UsuarioModificacion") Is DBNull.Value, "", dr("UsuarioModificacion"))
                        oRow.FechaModificacion = IIf(dr("FechaModificacion") Is DBNull.Value, "", dr("FechaModificacion"))
                        oRow.HoraModificacion = IIf(dr("HoraModificacion") Is DBNull.Value, "", dr("HoraModificacion"))
                        oRow.Host = IIf(dr("Host") Is DBNull.Value, "", dr("Host"))
                        oRow.Moneda = IIf(dr("Moneda") Is DBNull.Value, "", dr("Moneda"))
                        oRow.CodigoTipoCupon = IIf(dr("CodigoTipoCupon") Is DBNull.Value, "", dr("CodigoTipoCupon"))
                        oRow.TipoInstrumento = IIf(dr("CodigoTipoInstrumentoSMV") Is DBNull.Value, "", dr("CodigoTipoInstrumentoSMV"))
                        oRow.CodigoISIN = IIf(dr("CodigoISIN") Is DBNull.Value, "", dr("CodigoISIN"))
                        oRow.Opcion = IIf(dr("Opcion") Is DBNull.Value, "", dr("Opcion"))
                        oRow.Id = IIf(dr("Id") Is DBNull.Value, 0, dr("Id"))
                        oCodigoValorBE._CodigoValorBE.AddCodigoValorBERow(oRow)
                        oCodigoValorBE._CodigoValorBE.AcceptChanges()
                    Next
                End Using
            End Using
            Return oCodigoValorBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11008 - 18/01/2018 - Ian Pastor M.
    'Descripcion: Agregar nuevo parámetro id
    Public Function ListarCodigoValor(ByVal id As Integer, ByVal codigoValor As String, ByVal codigoNemonico As String, _
                                      ByVal codigoTercero As String, ByVal codigoMoneda As String, ByVal codigoTipoCupon As String, _
                                      ByVal situacion As String) As CodigoValorBE
        Try
            Dim oCodigoValorBE As New CodigoValorBE
            Dim oRow As CodigoValorBE.CodigoValorBERow
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_Listar_CodigoValor")
                db.AddInParameter(dbCommand, "@Id", DbType.Int32, id)
                db.AddInParameter(dbCommand, "@CodigoValor", DbType.String, codigoValor)
                db.AddInParameter(dbCommand, "@Situacion", DbType.String, situacion)
                db.AddInParameter(dbCommand, "@CodigoNemonico", DbType.String, codigoNemonico)
                db.AddInParameter(dbCommand, "@CodigoTercero", DbType.String, codigoTercero)
                db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, codigoMoneda)
                db.AddInParameter(dbCommand, "@CodigoTipoCupon", DbType.String, codigoTipoCupon)
                Using dsOI As DataSet = db.ExecuteDataSet(dbCommand)
                    For Each dr As DataRow In dsOI.Tables(0).Rows
                        oRow = oCodigoValorBE._CodigoValorBE.NewRow()
                        oRow.CodigoValor = IIf(dr("CodigoValor") Is DBNull.Value, "", dr("CodigoValor"))
                        oRow.CodigoNemonico = IIf(dr("CodigoNemonico") Is DBNull.Value, "", dr("CodigoNemonico"))
                        oRow.CodigoEntidad = IIf(dr("CodigoEntidad") Is DBNull.Value, "", dr("CodigoEntidad"))
                        oRow.Sinonimo = IIf(dr("sinonimo") Is DBNull.Value, "", dr("sinonimo"))
                        oRow.CodigoMoneda = IIf(dr("CodigoMoneda") Is DBNull.Value, "", dr("CodigoMoneda"))
                        oRow.Situacion = IIf(dr("Situacion") Is DBNull.Value, "", dr("Situacion"))
                        oRow.Dsituacion = IIf(dr("DSituacion") Is DBNull.Value, "", dr("DSituacion"))
                        oRow.UsuarioCreacion = IIf(dr("UsuarioCreacion") Is DBNull.Value, "", dr("UsuarioCreacion"))
                        oRow.FechaCreacion = IIf(dr("FechaCreacion") Is DBNull.Value, "", dr("FechaCreacion"))
                        oRow.HoraCreacion = IIf(dr("HoraCreacion") Is DBNull.Value, "", dr("HoraCreacion"))
                        oRow.UsuarioModificacion = IIf(dr("UsuarioModificacion") Is DBNull.Value, "", dr("UsuarioModificacion"))
                        oRow.FechaModificacion = IIf(dr("FechaModificacion") Is DBNull.Value, "", dr("FechaModificacion"))
                        oRow.HoraModificacion = IIf(dr("HoraModificacion") Is DBNull.Value, "", dr("HoraModificacion"))
                        oRow.Host = IIf(dr("Host") Is DBNull.Value, "", dr("Host"))
                        oRow.Moneda = IIf(dr("Moneda") Is DBNull.Value, "", dr("Moneda"))
                        oRow.CodigoTipoCupon = IIf(dr("CodigoTipoCupon") Is DBNull.Value, "", dr("CodigoTipoCupon"))
                        oRow.TipoInstrumento = IIf(dr("CodigoTipoInstrumentoSMV") Is DBNull.Value, "", dr("CodigoTipoInstrumentoSMV"))
                        oRow.CodigoISIN = IIf(dr("CodigoISIN") Is DBNull.Value, "", dr("CodigoISIN"))
                        oRow.Opcion = IIf(dr("Opcion") Is DBNull.Value, "", dr("Opcion"))
                        oRow.Id = IIf(dr("Id") Is DBNull.Value, 0, dr("Id"))
                        oCodigoValorBE._CodigoValorBE.AddCodigoValorBERow(oRow)
                        oCodigoValorBE._CodigoValorBE.AcceptChanges()
                    Next
                End Using
            End Using
            Return oCodigoValorBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function NemonicoCodigoValor() As DataTable
        Dim oDS As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_Nemocodvalor")
            db.LoadDataSet(dbCommand, oDS, "Nemonico")
            Return oDS.Tables(0)
        End Using
    End Function
    Public Function TipoInstrumento_SMV() As DataTable
        Dim oDS As New DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_TipoInstrumento_SMV")
            db.LoadDataSet(dbCommand, oDS, "TipoInstrumento")
            Return oDS.Tables(0)
        End Using
    End Function
End Class