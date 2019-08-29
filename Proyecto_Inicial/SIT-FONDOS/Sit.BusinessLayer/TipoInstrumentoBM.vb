Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

    Public Class TipoInstrumentoBM
    Inherits InvokerCOM
        Public Sub New()

        End Sub


#Region " /* Funciones Seleccionar */ "

    
    Public Function SeleccionarPorFiltro(ByVal codigoClaseInstrumento As String, ByVal sinonimo As String, ByVal TipoRenta As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoClaseInstrumento, sinonimo, TipoRenta, situacion, dataRequest}
        Try

            Return New TipoInstrumentoDAM().SeleccionarPorFiltro(codigoClaseInstrumento, sinonimo, TipoRenta, situacion, dataRequest)
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


    End Function
    Public Function SeleccionarPorFiltroValores(ByVal codigo As String, ByVal codigoSBS As String, ByVal codigoTipoRenta As String, ByVal dataRequest As DataSet) As TipoInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigo, codigoSBS, dataRequest}
        Try

            Return New TipoInstrumentoDAM().SeleccionarPorFiltroValores(codigo, codigoSBS, codigoTipoRenta, dataRequest)
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
    End Function

    Public Function SeleccionarPorFiltroValores(ByVal codigoTipoRenta As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoRenta, dataRequest}
        Try

            Return New TipoInstrumentoDAM().SeleccionarPorFiltroValores(codigoTipoRenta, dataRequest)
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
    End Function

    Public Function SeleccionarPorCodigoyDescripcion(ByVal codigo As String, ByVal Descripcion As String, ByVal dataRequest As DataSet) As TipoInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigo, Descripcion, dataRequest}
        Try
            Return New TipoInstrumentoDAM().SeleccionarPorCodigoyDescripcion(codigo, Descripcion, dataRequest)
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
    End Function

    Public Function Seleccionar(ByVal codigoTipoInstrumento As String, ByVal dataRequest As DataSet) As TipoInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoInstrumento, dataRequest}
        Try

            Return New TipoInstrumentoDAM().Seleccionar(codigoTipoInstrumento, dataRequest)
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
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As DataSet 'TipoInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim TipoInstrumentoDAM As New TipoInstrumentoDAM
        Dim TipoInstrumentoBE As New DataSet

        Try

            TipoInstrumentoBE = TipoInstrumentoDAM.Listar(dataRequest)
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
        Return TipoInstrumentoBE
    End Function
    Public Function Listar_PorTipoRenta(ByVal TipoRenta As String, ByVal datarequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Dim objTipoInstrumentoDAM As New TipoInstrumentoDAM
        Dim dstipoinstrumento As New DataSet
        Try

            dstipoinstrumento = objTipoInstrumentoDAM.Listar_PorTipoRenta(TipoRenta)
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
        Return dstipoinstrumento
    End Function

    Public Function Validar_TipoInstrumento(ByVal TipoInstrumento As String, ByVal datarequest As DataSet) As String
        Dim codigoEjecucion = ObtenerCodigoEjecucion(datarequest)
        Dim parameters As Object() = {datarequest}
        Dim objTipoInstrumentoDAM As New TipoInstrumentoDAM
        Dim sValor As String
        Try

            sValor = objTipoInstrumentoDAM.Validar_TipoInstrumento(TipoInstrumento)
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
        Return sValor
    End Function

    Public Function VerificarExistenciaSBS_Nuevo(ByVal tipoInstrumentoSBS As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Dim resul As Boolean = False
        Try

            resul = New TipoInstrumentoDAM().VerificarExistenciaSBS_Nuevo(tipoInstrumentoSBS)
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
        Return resul
    End Function
    Public Function VerificarExistenciaSBS_Modificar(ByVal tipoInstrumentoSBS As String, ByVal codigoTipoInstrumento As String, ByVal dataRequest As DataSet) As Boolean
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoInstrumento, dataRequest}
        Dim resul As Boolean = False
        Try

            resul = New TipoInstrumentoDAM().VerificarExistenciaSBS_Modificar(tipoInstrumentoSBS, codigoTipoInstrumento)
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
        Return resul
    End Function
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oTipoInstrumentoBE As TipoInstrumentoBE, ByVal oTipoInstrumentoCuentaBCRBE As TipoInstrumentoCuentaBCRBE _
        , ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oTipoInstrumentoBE, dataRequest}
        Try
            Dim oTipoInstrumentoDAM As New TipoInstrumentoDAM

            codigo = oTipoInstrumentoDAM.Insertar(oTipoInstrumentoBE, oTipoInstrumentoCuentaBCRBE, dataRequest)
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

        Return codigo

    End Function


#End Region

#Region " /* Funciones Modificar */"


    Public Function Modificar(ByVal oTipoInstrumentoBE As TipoInstrumentoBE, ByVal oTipoInstrumentoCuentaBCRBE As TipoInstrumentoCuentaBCRBE _
    , ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oTipoInstrumentoBE, dataRequest}
        Try
            Dim oTipoInstrumentoDAM As New TipoInstrumentoDAM

            actualizado = oTipoInstrumentoDAM.Modificar(oTipoInstrumentoBE, oTipoInstrumentoCuentaBCRBE, dataRequest)
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

        Return actualizado

    End Function


#End Region

#Region " /* Funciones Eliminar */"

    Public Function Eliminar(ByVal codigoTipoInstrumento As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTipoInstrumento, dataRequest}
        Try
            Dim oTipoInstrumentoDAM As New TipoInstrumentoDAM

            eliminado = oTipoInstrumentoDAM.Eliminar(codigoTipoInstrumento, dataRequest)
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
        Return eliminado
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

    Public Function ListarTipoInstrumento(ByVal strCodigoClaseInstrumento As String, ByVal dataRequest As DataSet) As TipoInstrumentoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCodigoClaseInstrumento, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New TipoInstrumentoDAM().ListarTipoInstrumento(strCodigoClaseInstrumento, dataRequest)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarTipoInstrumentoPorCategoria(ByVal strCategoria As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {strCategoria, dataRequest}
        Try
            RegistrarAuditora(parameters)
            Return New TipoInstrumentoDAM().ListarTipoInstrumentoPorCategoria(strCategoria, dataRequest).Tables(0)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function



End Class

