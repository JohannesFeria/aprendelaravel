Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class VectorTipoCambioBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
    'OT11004 - 21/12/2017 - Ian Pastor M.
    'Descripcion: Refactorizar código
    Public Function Insertar(ByVal objVectorTipoCambio As VectorTipoCambio.VectorTipoCambioRow, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim daVectorTipoCambio As New VectorTipoCambioDAM
            Return daVectorTipoCambio.Insertar(objVectorTipoCambio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub Actualiza_ValorCambioMoneda(ByVal Fecha As Decimal, ByVal entidadExt As String)
        Try
            Dim daVectorTipoCambio As New VectorTipoCambioDAM
            daVectorTipoCambio.Actualiza_ValorCambioMoneda(Fecha, entidadExt)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function Borrar_TipoCambio(ByVal Fecha As Decimal)
        Try
            Dim daVectorTipoCambio As New VectorTipoCambioDAM
            daVectorTipoCambio.Borrar_TipoCambio(Fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub Copiar_TipoCambio(ByVal Fecha As Decimal, ByVal entidadExt As String)
        Try
            Dim daVectorTipoCambio As New VectorTipoCambioDAM
            daVectorTipoCambio.Copiar_TipoCambio(Fecha, entidadExt)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    'OT11004 - Fin
    Public Function InsertarTipoCambioSBS(ByVal objVectorTipoCambio As VectorTipoCambio.VectorTipoCambioRow, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {objVectorTipoCambio, dataRequest}
        Try
            Dim daVectorTipoCambio As New VectorTipoCambioDAM
            strCodigo = daVectorTipoCambio.InsertarTipoCambioSBS(objVectorTipoCambio, dataRequest)
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
        Return strCodigo
    End Function
    Public Function InsertarTipoCambioSPOT(ByVal objVectorTipoCambio As VectorTipoCambio, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {objVectorTipoCambio, dataRequest}
        Try
            Dim daVectorTipoCambio As New VectorTipoCambioDAM
            strCodigo = daVectorTipoCambio.InsertarTipoCambioSPOT(objVectorTipoCambio, dataRequest)
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
        Return strCodigo
    End Function
    Public Function InsertarPorMantenimiento(ByVal oVectorTipoCambio As VectorTipoCambio, ByVal dataRequest As DataSet) As Boolean
        Dim Ingresado As Boolean = False
        Try
            Dim oVectorTipoCambioDAM As New VectorTipoCambioDAM
            Ingresado = oVectorTipoCambioDAM.InsertarPorMantenimiento(oVectorTipoCambio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return Ingresado
    End Function
    Public Function Modificar(ByVal oVectorTipoCambio As VectorTipoCambio, ByVal dataRequest As DataSet) As Boolean
        Dim Actualizado As Boolean = False
        Try
            Dim oVectorTipoCambioDAM As New VectorTipoCambioDAM
            Actualizado = oVectorTipoCambioDAM.Modificar(oVectorTipoCambio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return Actualizado
    End Function
    Public Function ModificarFila(ByVal oVectorTipoCambio As VectorTipoCambio.VectorTipoCambioRow, ByVal dataRequest As DataSet) As Boolean
        Dim Actualizado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oVectorTipoCambio, dataRequest}
        Try
            Dim oVectorTipoCambioDAM As New VectorTipoCambioDAM
            Actualizado = oVectorTipoCambioDAM.ModificarFila(oVectorTipoCambio, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return Actualizado
    End Function
    Public Function ModificarTipoCambioSBS(ByVal objVectorTipoCambio As VectorTipoCambio.VectorTipoCambioRow, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {objVectorTipoCambio, dataRequest}
        Try
            Dim daVectorTipoCambio As New VectorTipoCambioDAM
            strCodigo = daVectorTipoCambio.ModificarTipoCambioSBS(objVectorTipoCambio, dataRequest)
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
        Return strCodigo
    End Function
    Public Function Listar(ByVal sFecha As String, ByVal sEntidadExt As String, ByVal sCodigoMoneda As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try
            Return New VectorTipoCambioDAM().Listar(sFecha, sEntidadExt, sCodigoMoneda)
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
    Public Function SeleccionarTipoCambio(ByVal sFecha As String, ByVal sEntidadExt As String, ByVal sCodigoMoneda As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {sFecha, sEntidadExt, sCodigoMoneda, dataRequest}
        Dim oDSVectorTipoCambio As New DataSet
        Try
            oDSVectorTipoCambio = New VectorTipoCambioDAM().SeleccionarTipoCambio(sFecha, sEntidadExt, sCodigoMoneda, dataRequest)
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
        Return oDSVectorTipoCambio
    End Function
    Public Function SeleccionarPorFecha(ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fecha, dataRequest}
        Dim oDSVectorTipoCambio As New DataSet
        Try
            oDSVectorTipoCambio = New VectorTipoCambioDAM().SeleccionarPorFecha(fecha, dataRequest)
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
        Return oDSVectorTipoCambio
    End Function
    Public Function EliminarTipoCambioSBS(ByVal FechaCarga As Date, ByVal dataRequest As DataSet)
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaCarga, dataRequest}
        Try
            eliminado = New VectorTipoCambioDAM().EliminarTipoCambioSBS(FechaCarga)
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
    'OT11004 - 21/12/2017 - Ian Pastor M.
    'Descripcion: Refactorizar código
    Public Function EliminarTipoCambioReal_SBS(ByVal FechaCarga As Decimal, ByVal entidadExt As String) As Boolean
        Dim eliminado As Boolean = False
        Try
            eliminado = New VectorTipoCambioDAM().EliminarTipoCambioReal_SBS(FechaCarga, entidadExt)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function
    'OT11004 - Fin
    Public Function EliminarVectorTipoCambio(ByVal FechaCarga As Date, ByVal dataRequest As DataSet)
        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaCarga, dataRequest}
        Try
            eliminado = New VectorTipoCambioDAM().EliminarTipoCambioReal(FechaCarga)
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
    Public Function Validar_VectorTipoCambio(ByVal fecha As Decimal, ByVal datarequest As DataSet) As Integer
      
        Dim objTipoInstrumentoDAM As New TipoInstrumentoDAM
        Dim nValor As Integer
        Try
            nValor = New VectorTipoCambioDAM().Validar_VectorTipoCambio(fecha)
        Catch ex As Exception
            Throw ex
        End Try
        Return nValor
    End Function
    Public Function SeleccionarTipoCambio(ByVal Fecha As Decimal, ByVal sEntidadExt As String, ByVal sCodigoMoneda As String) As Decimal
        Dim Tipocambio As Decimal
        Try
            Tipocambio = New VectorTipoCambioDAM().SeleccionarTipoCambio(Fecha, sEntidadExt, sCodigoMoneda)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return Tipocambio
    End Function

    Public Function TasaLibor_EliminarVectorCarga_PIP(ByVal FechaCarga As Decimal, ByVal fuente As String) As Boolean
        Dim eliminado As Boolean = False
        Try
            eliminado = New VectorTipoCambioDAM().TasaLibor_EliminarVectorCarga_PIP(FechaCarga, fuente)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function
    Public Function TasaLibor_InsertarVectorCarga_PIP(ByVal plazoCurva As Integer, ByVal valorCurva As Decimal, ByVal fechaVector As Decimal, ByVal codigoIndicador As String, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim daVectorTipoCambio As New VectorTipoCambioDAM
            Return daVectorTipoCambio.TasaLibor_InsertarVectorCarga_PIP(plazoCurva, valorCurva, fechaVector, codigoIndicador, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class