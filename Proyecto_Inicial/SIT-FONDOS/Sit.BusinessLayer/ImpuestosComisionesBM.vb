Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' Clase para el acceso de los datos para ImpuestosComisiones tabla.
Public Class ImpuestosComisionesBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub



#Region " /* Funciones Seleccionar */ "


    Public Function SeleccionarPorFiltro(ByVal codigoImpuestosComisiones As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As ImpuestosComisionesBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoImpuestosComisiones, descripcion, dataRequest}
        Try

            Return New ImpuestosComisionesDAM().SeleccionarPorFiltro(codigoImpuestosComisiones, descripcion, dataRequest)
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
    Public Function SeleccionarPorFiltro(ByVal descripcion As String, ByVal tipoRenta As String, ByVal mercado As String, ByVal situacion As String, ByVal dataRequest As DataSet) As ImpuestosComisionesBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {descripcion, tipoRenta, mercado, situacion, dataRequest}
        Try
            Dim oImpuestosComisionesBE As ImpuestosComisionesBE
            oImpuestosComisionesBE = New ImpuestosComisionesDAM().SeleccionarPorFiltro(descripcion, tipoRenta, mercado, situacion, dataRequest)
            RegistrarAuditora(parameters)
            Return oImpuestosComisionesBE
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

    Public Function Seleccionar(ByVal codigoComision As String, ByVal codigoMercado As String, ByVal codigoTipoRenta As String, ByVal dataRequest As DataSet) As ImpuestosComisionesBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoComision, codigoMercado, codigoTipoRenta, dataRequest}
        Try
            Dim oImpuestosComisionesBE As ImpuestosComisionesBE
            oImpuestosComisionesBE = New ImpuestosComisionesDAM().Seleccionar(codigoComision, codigoMercado, codigoTipoRenta, dataRequest)
            RegistrarAuditora(parameters)
            Return oImpuestosComisionesBE
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

    'Public Function Seleccionar(ByVal codigoTarifa As String, ByVal codigoMercado As String, ByVal codigoRenta As String, ByVal dataRequest As DataSet) As ImpuestosComisionesBE
    '    Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '    Dim parameters As Object() = {codigoTarifa, codigoMercado, codigoRenta, dataRequest}
    '    Try
    '        Dim oImpuestosComisionesBE As ImpuestosComisionesBE
    '        oImpuestosComisionesBE = New ImpuestosComisionesDAM().Seleccionar(codigoTarifa, codigoMercado, codigoRenta, dataRequest)
    '        RegistrarAuditora(parameters)
    '        Return oImpuestosComisionesBE
    '    Catch ex As Exception
    '        'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
    '        RegistrarAuditora(parameters, ex)
    '        'Las siguientes 4 líneas deben agregarse para el Exception app block
    '        Dim rethrow As Boolean = true
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Function
    Public Function Listar(ByVal dataRequest As DataSet) As ImpuestosComisionesBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New ImpuestosComisionesDAM().Listar(dataRequest)
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
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oImpuestosComisionesBE As ImpuestosComisionesBE, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oImpuestosComisionesBE, dataRequest}
        Try
            Dim oImpuestosComisionesDAM As New ImpuestosComisionesDAM

            codigo = oImpuestosComisionesDAM.Insertar(oImpuestosComisionesBE, dataRequest)
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

#Region " /* Funciones Modificar */"


    Public Function Modificar(ByVal oImpuestosComisionesBE As ImpuestosComisionesBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oImpuestosComisionesBE, dataRequest}
        Try
            Dim oImpuestosComisionesDAM As New ImpuestosComisionesDAM

            actualizado = oImpuestosComisionesDAM.Modificar(oImpuestosComisionesBE, dataRequest)
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

#Region " /* Funciones Eliminar */"

    Public Function Eliminar(ByVal codigoTarifa As String, ByVal codigoMercado As String, ByVal codigoRenta As String, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTarifa, codigoMercado, codigoRenta, dataRequest}
        Try
            Dim oImpuestosComisionesDAM As New ImpuestosComisionesDAM

            eliminado = oImpuestosComisionesDAM.Eliminar(codigoTarifa, codigoMercado, codigoRenta, dataRequest)
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
#Region "Funciones Alberto"
    Public Function SeleccionarFiltroDinamico(ByVal CodigoTipoRenta As String, ByVal CodigoMercado As String) As DataSet
        Try

            Return New ImpuestosComisionesDAM().SeleccionarFiltroDinamico(CodigoTipoRenta, CodigoMercado)


        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
#End Region

    Public Function SeleccionarFiltroDinamico_Fijos(ByVal strClasificacion As String, ByVal strMercado As String, ByVal strTipoRenta As String) As DataSet
        'Dim parameters As Object() = {CodigoTipoRenta, CodigoMercado, dataRequest}
        Try
            Return New ImpuestosComisionesDAM().SeleccionarFiltroDinamico_Fijos(strClasificacion, strMercado, strTipoRenta)
            'RegistrarAuditora(parameters)
        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function
    'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se añade tres Input: fechaOperacion, codigoMnemonico y codigoTercero a la función para calcular Comisiones para una nueva OI individual: Bonos | 13/06/18 
    Public Function GetImpuestosComisiones(ByVal codigoPreOrden As String, Optional ByVal fechaOperacion As Decimal = 0, Optional ByVal codigoMnemonico As String = "", Optional ByVal codigoTercero As String = "") As DataTable
        Dim impuestos As New ImpuestosComisionesDAM
        Dim dtImpuestos As New DataTable
        Try
            'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se incluye los tres nuevos Input - Bonos| 13/06/18 
            dtImpuestos = impuestos.GetImpuestosComisiones(codigoPreOrden, fechaOperacion, codigoMnemonico, codigoTercero)
            'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se incluye los tres nuevos Input - Bonos| 13/06/18 
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dtImpuestos
    End Function
    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF012 - Se añade tres Input: fechaOperacion, codigoMnemonico y codigoTercero a la función para calcular Comisiones para una nueva OI individual: Bonos | 13/06/18 
End Class

