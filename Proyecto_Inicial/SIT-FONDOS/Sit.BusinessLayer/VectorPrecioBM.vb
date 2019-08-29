Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class VectorPrecioBM
    Inherits InvokerCOM
    Public Sub Insertar(ByVal objVectorPrecio As VectorPrecioBE.VectorPrecioRow, ByVal dataRequest As DataSet)
        Try
            Dim daVectorPrecio As New VectorPrecioDAM
            daVectorPrecio.Insertar(objVectorPrecio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function BorrarVectorPrecio(ByVal Fecha As String, ByVal Manual As String, ByVal EntidadExt As String) As String
        Try
            Dim daVectorPrecio As New VectorPrecioDAM
            daVectorPrecio.BorrarVectorPrecio(Fecha, Manual, EntidadExt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10238 - 18/04/2017 - Carlos Espejo
    'Descripcion: Se agrega el precio sucio
    Public Function IngresarNuevo(ByVal CodigoMnemonico As String, ByVal Fecha As Decimal, ByVal EntidadExt As String, ByVal Valor As Decimal, _
    ByVal PrecioSucio As Decimal, ByVal PorcPrecioLimpio As Decimal, ByVal PorcPrecioSucio As Decimal, ByVal datarequest As DataSet, ByVal Manual As String) As String
        Try
            Dim daVectorPrecio As New VectorPrecioDAM, strCodigo As String
            strCodigo = daVectorPrecio.IngresarNuevo(CodigoMnemonico, Fecha, EntidadExt, Valor, PrecioSucio, PorcPrecioLimpio, PorcPrecioSucio, datarequest, Manual)
            Return strCodigo
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10238 - 18/04/2017 - Carlos Espejo
    'Descripcion: Se agrega el precio sucio
    Public Function Modificar(ByVal oVectorPrecio As VectorPrecioBE, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim oVectorPrecioDAM As New VectorPrecioDAM
            Return oVectorPrecioDAM.Modificar(oVectorPrecio, dataRequest)
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function Listar(ByVal sFecha As String, ByVal sCodigoTipoInstrumentoSBS As String, ByVal sCodigoMnemonico As String, ByVal sCodigoIsin As String, ByVal sEntidadExt As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New VectorPrecioDAM().Listar(sFecha, sCodigoTipoInstrumentoSBS, sCodigoMnemonico, sCodigoIsin, sEntidadExt)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    'OT 10238 - 18/04/2017 - Carlos Espejo
    'Descripcion: Se quita el parametro TipoInstrumento
    Public Function ListarRango(ByVal sFecha As String, ByVal sFechaFin As String, ByVal sCodigoMnemonico As String, ByVal sCodigoIsin As String, _
    ByVal sEntidadExt As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New VectorPrecioDAM().ListarRango(sFecha, sFechaFin, sCodigoMnemonico, sCodigoIsin, sEntidadExt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorFecha(ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fecha, dataRequest}
        Dim oDSVectorPrecio As New DataSet
        Try
            oDSVectorPrecio = New VectorPrecioDAM().SeleccionarPorFecha(fecha, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDSVectorPrecio
    End Function
    Public Function SeleccionarPorMnemonicoFecha(ByVal fecha As Decimal, ByVal codigoMnemonico As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fecha, codigoMnemonico, dataRequest}
        Dim oDSVectorPrecio As New DataSet
        Try
            oDSVectorPrecio = New VectorPrecioDAM().SeleccionarPorMnemonicoFecha(fecha, codigoMnemonico, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return oDSVectorPrecio
    End Function
    Public Function SeleccionarMnemonico(ByVal SBS As String, ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {SBS, dataRequest}
        Dim Codigo As String
        Try
            RegistrarAuditora(parameters)
            Codigo = New VectorPrecioDAM().SeleccionarCodigoMnemonico(SBS)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return Codigo
    End Function
    Public Function EliminarPreciosReal(ByVal FechaCarga As Date, ByVal dataRequest As DataSet)
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaCarga, dataRequest}
        Try
            eliminado = New VectorPrecioDAM().EliminarPrecioReal(FechaCarga)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function
    Public Function SeleccionarUltimoVectorPrecio(ByVal dataRequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim Codigo As String
        Try
            RegistrarAuditora(parameters)
            Codigo = New VectorPrecioDAM().SeleccionarUltimoVectorPrecio()
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return Codigo
    End Function
    Public Function CargarArchivoVectorPrecio(ByVal rutaArchivoDatos As String, ByVal rutaArchivoFormato As String, ByVal fechaCarga As Decimal, ByVal datarequest As DataSet) As DataTable
        Try
            Dim daVectorPrecio As New VectorPrecioDAM
            Return daVectorPrecio.CargarArchivoVectorPrecio(rutaArchivoDatos, rutaArchivoFormato, fechaCarga, datarequest)
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function ListarValores_ISIN() As DataTable
        Try
            Dim daVectorPrecio As New VectorPrecioDAM
            Return daVectorPrecio.ListarValores_ISIN()
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function InsertarTasaInteresPrecioForward(ByVal Fecha As Decimal, ByVal TasaInteres As DataTable, ByVal PrecioForward As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim oDM As New VectorPrecioDAM
        Try
            oDM.InsertarTasaInteresPrecioForward(Fecha, TasaInteres, PrecioForward, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function InsertarOperacionesBVL(ByVal Fecha As Decimal, ByVal dtOperacionBVL As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim oDM As New VectorPrecioDAM
        Try
            oDM.InsertarOperacionesBVL(Fecha, dtOperacionBVL, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function EliminarPrecioVector(ByVal Fecha As Decimal) As Boolean
        Dim oDM As New VectorPrecioDAM
        Try
            Return oDM.EliminarPrecioVector(Fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarValores_Facturas(ByVal codigosFacturas As String) As DataTable
        Try
            Dim daVectorPrecio As New VectorPrecioDAM
            Return daVectorPrecio.ListarValores_Facturas(codigosFacturas)
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
