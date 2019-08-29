Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' Clase para el acceso de los datos para TipoCambio tabla.
Public Class TipoCambioBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

#Region "/* Funciones Seleccionar */"

    ''' Selecciona un solo expediente de TipoCambio tabla.
    Public Function Seleccionar(ByVal codigoTipoCambio As String, ByVal DataRequest As DataSet) As TipoCambioBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(DataRequest)
        Dim parameters As Object() = {codigoTipoCambio, DataRequest}
        Try
            Dim oTipoCambioBE As TipoCambioBE
            oTipoCambioBE = New TipoCambioDAM().Seleccionar(codigoTipoCambio, DataRequest)
            RegistrarAuditora(parameters)
            Return oTipoCambioBE
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function SeleccionarTCOrigen_TCDestino(ByVal codigoTCOrigen As String, ByVal codigoTCDestino As String, ByVal fecha As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTCOrigen, codigoTCDestino, dataRequest}
        Try
            Dim oDS As DataSet
            oDS = New TipoCambioDAM().SeleccionarTCOrigen_TCDestino(codigoTCOrigen, codigoTCDestino, fecha, dataRequest)
            RegistrarAuditora(parameters)
            Return oDS
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function SeleccionarValorTCOrigen_TCDestino(ByVal codigoTCOrigen As String, ByVal codigoTCDestino As String, ByVal fecha As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTCOrigen, codigoTCDestino, dataRequest}
        Try
            Dim oDS As DataSet
            oDS = New TipoCambioDAM().SeleccionarValorTCOrigen_TCDestino(codigoTCOrigen, codigoTCDestino, fecha, dataRequest)
            RegistrarAuditora(parameters)
            Return oDS
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

    Public Function SeleccionarPorFiltros(ByVal codigoTipoCambio As String, ByVal situacion As String, ByVal descripcion As String, ByVal DataRequest As DataSet) As TipoCambioBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(DataRequest)
        Dim parameters As Object() = {codigoTipoCambio, situacion, descripcion, DataRequest}
        Try
            Dim oTipoCambioBE As TipoCambioBE
            oTipoCambioBE = New TipoCambioDAM().SeleccionarPorFiltros(codigoTipoCambio, situacion, descripcion, DataRequest)
            RegistrarAuditora(parameters)
            Return oTipoCambioBE
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

    ''' Selecciona expedientes de TipoCambio tabla por una llave extranjera.
    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet

    End Function

    ''' Lista los expedientes por distintas caracteristicas de la tabla TipoCambio.
    Public Function SeleccionarPorVariosCampos(ByVal oTipoCambioBE As TipoCambioBE, ByVal datosRequest As DataSet) As Boolean

    End Function

    ''' Lista todos los expedientes de TipoCambio tabla.
    Public Function Listar() As DataSet

    End Function

    Public Function SeleccionarFechaSistema(ByVal DataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(DataRequest)
        Dim parameters As Object() = {DataRequest}
        Try
            Dim oDataSet As DataSet
            oDataSet = New TipoCambioDAM().SeleccionarFechaSistema(DataRequest)
            RegistrarAuditora(parameters)
            Return oDataSet
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

  
#End Region

#Region "/* Funciones Insertar */"

    ''' Inserta un expediente en TipoCambio tabla.
    Public Function Insertar(ByVal oTipoCambioBE As TipoCambioBE, ByVal dataRequest As DataSet) As Boolean
        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oTipoCambioBE, dataRequest}
        Try
            Dim oTipoCambioDAM As New TipoCambioDAM
            codigo = oTipoCambioDAM.Insertar(oTipoCambioBE, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return codigo
    End Function

#End Region

#Region "/* Funciones Modificar */"

    ''' Modifica un expediente en TipoCambio tabla.
    Public Function Modificar(ByVal oTipoCambioBE As TipoCambioBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oTipoCambioBE, dataRequest}
        Try
            Dim oTipoCambioDAM As New TipoCambioDAM

            actualizado = oTipoCambioDAM.Modificar(oTipoCambioBE, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return actualizado
    End Function

#End Region

#Region "/* Funciones Eliminar */"

    ''' Elimina un expediente de TipoCambio table por una llave primaria compuesta.
    Public Function Eliminar(ByVal codigoTipoCambio As String, ByVal dataRequest As DataSet)

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoCambio, dataRequest}
        Try
            Dim oTipoCambioDAM As New TipoCambioDAM

            actualizado = oTipoCambioDAM.Eliminar(codigoTipoCambio, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return actualizado

    End Function

    ''' Elimina un expediente de TipoCambio table por una llave extranjera.
    Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String)

    End Function

#End Region

#Region " /* Funciones Personalizadas*/"

    Public Sub Salir(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Ingresar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub

#End Region


    'LETV 20090718
    Public Function ExisteTipoCambio(ByVal Entidad As String, ByVal CodigoMoneda As String, ByVal Fecha As Decimal) As Boolean
        Try
            Dim resultado As Boolean
            resultado = New TipoCambioDAM().ExisteTipoCambio(Entidad, CodigoMoneda, Fecha)

            Return resultado
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            ' RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'LETV 20090718
    Public Function SeleccionarValorTCOrigen_TCDestinoXEntidad(ByVal Moneda As String, ByVal MonedaDestino As String, ByVal fecha As Decimal, ByVal Entidad As String) As Decimal
        Try
            Dim resultado As Decimal
            resultado = New TipoCambioDAM().SeleccionarValorTCOrigen_TCDestinoXEntidad(Moneda, MonedaDestino, fecha, Entidad)

            Return resultado
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            ' RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
End Class

