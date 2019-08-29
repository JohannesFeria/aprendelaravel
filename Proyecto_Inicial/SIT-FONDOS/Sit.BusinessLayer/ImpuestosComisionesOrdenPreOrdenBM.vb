Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

     Public  Class ImpuestosComisionesOrdenPreOrdenBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub


    Public Function Insertar(ByVal objIC As ImpuestosComisionesOrdenPreOrdenBE, ByVal dataRequest As DataSet) As String
        Dim strCodigoOI As String
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {objIC, dataRequest}
        Try
            Dim daImpuestoComsiones As New ImpuestosComisionesOrdenPreOrdenDAM
            strCodigoOI = daImpuestoComsiones.Insertar(objIC, dataRequest)
            'Luego de terminar la ejecución de métodos(sin errores) 
            RegistrarAuditora(parameters)
            Return strCodigoOI
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
    Public Sub Modificar(ByVal objIC As ImpuestosComisionesOrdenPreOrdenBE, ByVal dataRequest As DataSet)
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {objIC, dataRequest}
        Try
            Dim daImpuestoComsiones As New ImpuestosComisionesOrdenPreOrdenDAM
            daImpuestoComsiones.Modificar(objIC, dataRequest)
            'Luego de terminar la ejecución de métodos(sin errores) 
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
    End Sub
    Public Function ListarPorCodigoOrden(ByVal CodigoOrden As String, ByVal codigoPortafolio As String) As DataSet
        Dim dsIC As New DataSet
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {CodigoOrden}
        Try
            Dim daImpuestoComsiones As New ImpuestosComisionesOrdenPreOrdenDAM
            dsIC = daImpuestoComsiones.ListarPorCodigoOrden(CodigoOrden, codigoPortafolio)
            'Luego de terminar la ejecución de métodos(sin errores) 
            RegistrarAuditora(parameters)
            Return dsIC
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

    Public Function VerificarExistencia(ByVal CodigoOrden As String, ByVal codigoPortafolio As String, ByVal codigoComision As String, ByVal codigoRenta As String, ByVal codigoMercado As String) As Boolean
        Dim resul As Boolean = False
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {CodigoOrden, codigoPortafolio, codigoComision, codigoRenta, codigoMercado}
        Try
            Dim daImpuestoComsiones As New ImpuestosComisionesOrdenPreOrdenDAM
            resul = daImpuestoComsiones.VerificarExistencia(CodigoOrden, codigoPortafolio, codigoComision, codigoRenta, codigoMercado)
            'Luego de terminar la ejecución de métodos(sin errores) 
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

    ''' Selecciona un solo expediente de ImpuestosComisionesOrdenPreOrden tabla.
    Public Function Seleccionar(ByVal cODIGO As String) As DataSet

    End Function


    ''' Selecciona expedientes de ImpuestosComisionesOrdenPreOrden tabla por una llave extranjera.
    Public Function SeleccionarPorCodigoTarifa(ByVal codigoTarifa As String) As DataSet

    End Function


    ''' Lista todos los expedientes de ImpuestosComisionesOrdenPreOrden tabla.
    Public Function Listar() As DataSet

    End Function

    'RGF 20090113 comentado porq estaba vacio
    ''' Midifica un expediente en ImpuestosComisionesOrdenPreOrden tabla.
    'Public Function Modificar(ByVal cODIGO As String, ByVal codigoTarifa As String, ByVal codigoOrdenPreOrden As String, ByVal valor As Decimal, ByVal situacion As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String)

    'End Function

    ''' Elimina un expediente de ImpuestosComisionesOrdenPreOrden table por una llave primaria compuesta.
    Public Function Eliminar(ByVal strCodigoOrden As String, ByVal strPortafolio As String, ByVal dataRequest As DataSet, Optional ByVal strCodigoMercado As String = "") 'HDG INC 63038	20110427
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {strCodigoOrden, dataRequest}
        Try
            'RGF 20090107 El procedure no hace nada
            'ini HDG INC 63038	20110427 se descomenta
            Dim daImpCom As New ImpuestosComisionesOrdenPreOrdenDAM
            daImpCom.Eliminar(strCodigoOrden, strPortafolio, strCodigoMercado, dataRequest)
            'fin HDG INC 63038	20110427
            'Luego de terminar la ejecución de métodos(sin errores) 
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

    ''' Elimina un expediente de ImpuestosComisionesOrdenPreOrden table por una llave extranjera.
    Public Function EliminarPorCodigoTarifa(ByVal codigoTarifa As String)

    End Function
End Class

