Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class VectorPrecioDAM
    Private oVectorPrecio As VectorPrecioBE.VectorPrecioRow
    Public Sub Insertar(ByVal objVectorPrecio As VectorPrecioBE.VectorPrecioRow, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("VectorPrecio_Insertar")
            oVectorPrecio = CType(objVectorPrecio, VectorPrecioBE.VectorPrecioRow)
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oVectorPrecio.CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_CodigoIsin", DbType.String, oVectorPrecio.CodigoISIN)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oVectorPrecio.Fecha)
            db.AddInParameter(dbCommand, "@p_ValorPrecio", DbType.Decimal, oVectorPrecio.Valor)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_EntidadExt", DbType.String, oVectorPrecio.EntidadExt)

            'OTXXXX Ian Pastor M. 15/08/2018
            'Cambio: Se agregó nuevos parámetros por haberse agregado nuevos campos a la tabla "VectorPrecio"
            db.AddInParameter(dbCommand, "@p_DescripcionEmisor", DbType.String, oVectorPrecio.DescripcionEmisor)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oVectorPrecio.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_TipoTasa", DbType.String, oVectorPrecio.TipoTasa)
            db.AddInParameter(dbCommand, "@p_TasaCupon", DbType.Decimal, oVectorPrecio.TasaCupon)
            db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, oVectorPrecio.FechaVencimiento)
            db.AddInParameter(dbCommand, "@p_ClasificacionRiesgo", DbType.String, oVectorPrecio.ClasificacionRiesgo)
            db.AddInParameter(dbCommand, "@p_ValorNominal", DbType.Decimal, oVectorPrecio.ValorNominal)
            db.AddInParameter(dbCommand, "@p_PorcPrecioLimpio", DbType.Decimal, oVectorPrecio.PorcPrecioLimpio)
            db.AddInParameter(dbCommand, "@p_TIR", DbType.Decimal, oVectorPrecio.TIR)
            db.AddInParameter(dbCommand, "@p_PorcPrecioSucio", DbType.Decimal, oVectorPrecio.PorcPrecioSucio)
            db.AddInParameter(dbCommand, "@p_SobreTasa", DbType.Decimal, oVectorPrecio.SobreTasa)
            db.AddInParameter(dbCommand, "@p_PrecioLimpio", DbType.Decimal, oVectorPrecio.PrecioLimpio)
            db.AddInParameter(dbCommand, "@p_PrecioSucio", DbType.Decimal, oVectorPrecio.PrecioSucio)
            db.AddInParameter(dbCommand, "@p_IC", DbType.Decimal, oVectorPrecio.IC)
            db.AddInParameter(dbCommand, "@p_Duracion", DbType.Decimal, oVectorPrecio.Duracion)
            db.AddInParameter(dbCommand, "@p_DetalleMoneda", DbType.String, oVectorPrecio.DetalleMoneda)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function BorrarVectorPrecio(ByVal Fecha As String, ByVal Manual As String, ByVal EntidadExt As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_BorrarVectorPrecio")
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
            db.AddInParameter(dbCommand, "@p_Manual", DbType.String, Manual)
            db.AddInParameter(dbCommand, "@p_EntidadExt", DbType.String, EntidadExt)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    'OT 10238 - 18/04/2017 - Carlos Espejo
    'Descripcion: Se agrega el precio sucio
    Public Function Modificar(ByVal ob As VectorPrecioBE, ByVal dataRequest As DataSet) As Boolean
        Modificar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("VectorPrecio_Modificar")
            oVectorPrecio = CType(ob.VectorPrecio.Rows(0), VectorPrecioBE.VectorPrecioRow)
            db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, oVectorPrecio.CodigoMnemonico)
            db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, oVectorPrecio.Fecha)
            db.AddInParameter(dbCommand, "@EntidadExt", DbType.String, oVectorPrecio.EntidadExt)
            db.AddInParameter(dbCommand, "@ValorPrecio", DbType.Decimal, oVectorPrecio.Valor)
            db.AddInParameter(dbCommand, "@p_ValorPrecioSucio", DbType.Decimal, oVectorPrecio.PrecioSucio)
            db.AddInParameter(dbCommand, "@PorcPrecioLimpio", DbType.Decimal, oVectorPrecio.PorcPrecioLimpio)
            db.AddInParameter(dbCommand, "@PorcPrecioSucio", DbType.Decimal, oVectorPrecio.PorcPrecioSucio)
            db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@Manual", DbType.String, oVectorPrecio.Manual)
            db.ExecuteNonQuery(dbCommand)
            Modificar = True
        End Using
    End Function
    Public Function SeleccionarPorFecha(ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorPrecio_SeleccionarPorFecha")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, fecha)
        Dim oVectorPrecio As New DataSet
        db.LoadDataSet(dbCommand, oVectorPrecio, "VectorPrecio")
        Return oVectorPrecio
    End Function
    Public Function SeleccionarPorMnemonicoFecha(ByVal fecha As Decimal, ByVal codigoMnemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorPrecio_SeleccionarPorNemonicoFecha")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoMnemonico)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, fecha)
        Dim oVectorPrecio As New DataSet
        db.LoadDataSet(dbCommand, oVectorPrecio, "VectorPrecio")
        Return oVectorPrecio
    End Function
    Public Function Listar(ByVal sFecha As String, ByVal sCodigoTipoInstrumentoSBS As String, ByVal sCodigoMnemonico As String, ByVal sCodigoIsin As String, ByVal sEntidadExt As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorPrecio_Listar")
        db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, sFecha)
        db.AddInParameter(dbCommand, "@CodigoTipoInstrumentoSBS", DbType.String, sCodigoTipoInstrumentoSBS)
        db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, sCodigoMnemonico)
        db.AddInParameter(dbCommand, "@CodigoIsin", DbType.String, sCodigoIsin)
        db.AddInParameter(dbCommand, "@EntidadExt", DbType.String, sEntidadExt)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    'OT 10238 - 18/04/2017 - Carlos Espejo
    'Descripcion: Se quita el parametro TipoInstrumento
    Public Function ListarRango(ByVal sFecha As String, ByVal sFechaFin As String, ByVal sCodigoMnemonico As String, ByVal sCodigoIsin As String, _
    ByVal sEntidadExt As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("VectorPrecio_ListarRango")
            dbCommand.CommandTimeout = 300
            db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, sFecha)
            db.AddInParameter(dbCommand, "@FechaFin", DbType.Decimal, sFechaFin)
            db.AddInParameter(dbCommand, "@CodigoMnemonico", DbType.String, sCodigoMnemonico)
            db.AddInParameter(dbCommand, "@CodigoIsin", DbType.String, sCodigoIsin)
            db.AddInParameter(dbCommand, "@EntidadExt", DbType.String, sEntidadExt)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarCodigoMnemonico(ByVal SBS As String) As String
        Dim CodigoMnemonico As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Valores_SeleccionarCodigoMnemonicoPorCodigoSBS")
        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, SBS)
        CodigoMnemonico = db.ExecuteScalar(dbCommand)
        Return CodigoMnemonico
    End Function
    Public Function EliminarPrecioReal(ByVal FechaCarga As Date) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VectorPrecio_EliminarSBS")
        dbCommand.CommandTimeout = 360
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    'OT 10238 - 18/04/2017 - Carlos Espejo
    'Descripcion: Se agrega el precio sucio
    Public Function IngresarNuevo(ByVal CodigoMnemonico As String, ByVal Fecha As Decimal, ByVal EntidadExt As String, ByVal Valor As Decimal, _
    ByVal PrecioSucio As Decimal, ByVal PorcPrecioLimpio As Decimal, ByVal PorcPrecioSucio As Decimal, ByVal datarequest As DataSet, ByVal Manual As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("VectorPrecio_Ingresar")
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
            db.AddInParameter(dbCommand, "@p_EntidadExt", DbType.String, EntidadExt)
            db.AddInParameter(dbCommand, "@p_ValorPrecio", DbType.Decimal, Valor)
            db.AddInParameter(dbCommand, "@p_ValorPrecioSucio", DbType.Decimal, PrecioSucio)
            db.AddInParameter(dbCommand, "@p_PorcPrecioLimpio", DbType.Decimal, PorcPrecioLimpio)
            db.AddInParameter(dbCommand, "@p_PorcPrecioSucio", DbType.Decimal, PorcPrecioSucio)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Host"))
            db.AddInParameter(dbCommand, "@p_Manual", DbType.String, Manual)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    Public Function SeleccionarUltimoVectorPrecio() As String
        Dim CodigoMnemonico As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorPrecio_UltimaCarga")
        dbCommand.CommandTimeout = 200
        CodigoMnemonico = db.ExecuteScalar(dbCommand)
        Return CodigoMnemonico
    End Function
    Public Function CargarArchivoVectorPrecio(ByVal rutaArchivoDatos As String, ByVal rutaArchivoFormato As String, ByVal fechaCarga As Decimal, ByVal dataRequest As DataSet) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorPrecio_CargarDesdeArchivo")
        dbCommand.CommandTimeout = 1200
        db.AddInParameter(dbCommand, "@p_RutaArchivoDatos", DbType.String, rutaArchivoDatos)
        db.AddInParameter(dbCommand, "@p_RutaArchivoFormato", DbType.String, rutaArchivoFormato)
        db.AddInParameter(dbCommand, "@p_FechaCarga", DbType.Decimal, fechaCarga)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        Return db.ExecuteDataSet(dbCommand).Tables(1)
    End Function
    Public Function ListarValores_ISIN() As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Valores_Validar_ISIN")
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                ListarValores_ISIN = ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function InsertarTasaInteresPrecioForward(ByVal Fecha As Decimal, ByVal TasaInteres As DataTable, ByVal PrecioForward As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim oRowTasaInteres As DataRow
        Dim oRowPrecioForward As DataRow
        EliminarTasaInteres(Fecha)
        For Each oRowTasaInteres In TasaInteres.Rows
            InsertarTasaInteres(oRowTasaInteres, Fecha, dataRequest)
        Next
        EliminarPrecioForward(Fecha)
        For Each oRowPrecioForward In PrecioForward.Rows
            InsertarPrecioForward(oRowPrecioForward, Fecha, dataRequest)
        Next
        Return True
    End Function
    Public Function EliminarTasaInteres(ByVal Fecha As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Forward_EliminarTasaIntereses")
        dbCommand.CommandTimeout = 1020  'HDG 20120103
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function EliminarPrecioForward(ByVal Fecha As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Forward_EliminarPrecioForward")
        dbCommand.CommandTimeout = 1020  'HDG 20120103
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function EliminarPrecioVector(ByVal Fecha As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorPrecio_EliminarPrecioVector")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function InsertarTasaInteres(ByVal TasaImpuesto As DataRow, ByVal Fecha As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Forward_InsertarTasaInteres")
        dbCommand.CommandTimeout = 1020  'HDG 20120103
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CStr(TasaImpuesto("Moneda")))
        If (TasaImpuesto("Activa") Is DBNull.Value) Then
            db.AddInParameter(dbCommand, "@p_TasaActiva", DbType.Decimal, DBNull.Value)
        Else
            If Trim(CStr(TasaImpuesto("Activa"))).Length = 0 Then
                db.AddInParameter(dbCommand, "@p_TasaActiva", DbType.Decimal, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_TasaActiva", DbType.Decimal, CDec(TasaImpuesto("Activa")))
            End If
        End If
        If (TasaImpuesto("Pasiva") Is DBNull.Value) Then
            db.AddInParameter(dbCommand, "@p_TasaPasiva", DbType.Decimal, DBNull.Value)
        Else
            If Trim(CStr(TasaImpuesto("Pasiva"))).Length = 0 Then
                db.AddInParameter(dbCommand, "@p_TasaPasiva", DbType.Decimal, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_TasaPasiva", DbType.Decimal, CDec(TasaImpuesto("Pasiva")))
            End If
        End If
        db.AddInParameter(dbCommand, "@p_Plazo", DbType.String, CInt(TasaImpuesto("Plazo")))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function InsertarPrecioForward(ByVal PrecioForward As DataRow, ByVal Fecha As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Forward_InsertarPrecioForward")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_NumeroPoliza", DbType.String, PrecioForward("REF"))
        db.AddInParameter(dbCommand, "@p_PrecioForward", DbType.Decimal, PrecioForward("Precio Forward"))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function InsertarOperacionesBVL(ByVal Fecha As Decimal, ByVal dtOperacionBVL As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim oRowOperacion As DataRow
        Dim OperacionesBVL As String
        EliminarOperacionBVL(Fecha)
        For Each oRowOperacion In dtOperacionBVL.Rows
            InsertarOperacionBVL(oRowOperacion, Fecha, dataRequest)
        Next
        Return True
    End Function
    Public Function EliminarOperacionBVL(ByVal fecha As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OperacionBVL_Eliminar")
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fecha)
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function InsertarOperacionBVL(ByVal OperacionBVL As DataRow, ByVal FechaOperacion As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("OperacionBVL_Insertar")
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, OperacionBVL("Valor"))
        db.AddInParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, OperacionBVL("Cantidad"))
        db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function ListarValores_Facturas(ByVal codigosFacturas As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Valores_Validar_Facturas")
            db.AddInParameter(dbCommand, "@p_Facturas", DbType.String, codigosFacturas)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                ListarValores_Facturas = ds.Tables(0)
            End Using
        End Using
    End Function
End Class
