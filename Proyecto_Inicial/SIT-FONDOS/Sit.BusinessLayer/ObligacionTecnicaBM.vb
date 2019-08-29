Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class ObligacionTecnicaBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltro(ByVal CodObligacionTecnica As String, ByVal CodPortafolio As String, ByVal Descripcion As String, ByVal Fecha As Decimal, ByVal Tipo As String, ByVal dataRequest As DataSet) As DataSet
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Dim parameters As Object() = {Descripcion, FechaOperacion, dataRequest}
        Dim objDS As DataSet
        Try
            'RegistrarAuditora(parameters)
            objDS = New ObligacionTecnicaDAM().SeleccionarPorFiltro(CodObligacionTecnica, CodPortafolio, Descripcion, Fecha, Tipo, dataRequest)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return objDS
    End Function
#End Region

    Public Function Insertar(ByVal oObligacionTecnica As ObligacionTecnicaBE, ByVal dataRequest As DataSet) As Boolean
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Dim parameters As Object() = {oObligacionTecnica, dataRequest}
        Try
            Dim oObligacionTecnicaDAM As New ObligacionTecnicaDAM
            oObligacionTecnicaDAM.Insertar(oObligacionTecnica, dataRequest)
            'RegistrarAuditora(parameters)
        Catch ex As Exception
            'RegistrarAuditora(parameters, ex)
            'Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function

    Public Function Modificar(ByVal ob As ObligacionTecnicaBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Dim parameters As Object() = {ob, dataRequest}
        Try
            Dim oObligacionTecnicaDAM As New ObligacionTecnicaDAM

            actualizado = oObligacionTecnicaDAM.Modificar(ob, dataRequest)
            'RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado
    End Function

    Public Function ValidarRegistro(ByVal CodOT As String, ByVal FechaOT As Decimal, ByVal PortafolioOT As String, ByVal Tipo As String, ByVal DataRequest As DataSet) As ObligacionTecnicaBE
        Try
            'Dim oObligacionTecnicaBEBE As ObligacionTecnicaBE
            Return New ObligacionTecnicaDAM().ValidarRegistro(CodOT, FechaOT, PortafolioOT, Tipo, DataRequest)
            'oObligacionTecnicaBEBE = New ObligacionTecnicaDAM().ValidarRegistro(CodOT, FechaOT, PortafolioOT, Tipo, DataRequest)
            'Return oObligacionTecnicaBEBE
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function


    Public Function Eliminar(ByVal CodigoObligacionTecnica As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Dim parameters As Object() = {codigoTipoCambioDI, dataRequest}
        Try
            Dim oObligacionTecnicaDAM As New ObligacionTecnicaDAM
            eliminado = oObligacionTecnicaDAM.Eliminar(CodigoObligacionTecnica, dataRequest)
            'RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function

    Public Function DesactivarRegistrosExcel(ByVal Fecha As Decimal, ByVal dataRequest As DataSet, ByRef strMensaje As String)
        Try
            Dim oObligacionTecnicaDAM As New ObligacionTecnicaDAM
            oObligacionTecnicaDAM.DesactivarRegistrosExcel(Fecha, dataRequest, strMensaje)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function InsertarRegistrosExcel(ByVal oObligacionTecnica As ObligacionTecnicaBE.ObligacionTecnicaRow, ByVal dataRequest As DataSet) As Boolean
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Dim parameters As Object() = {oObligacionTecnica, dataRequest}
        Try
            Dim oObligacionTecnicaDAM As New ObligacionTecnicaDAM
            oObligacionTecnicaDAM.InsertarObligacionesTecnicasPorExcel(oObligacionTecnica, dataRequest)
            'RegistrarAuditora(parameters)
        Catch ex As Exception
            'RegistrarAuditora(parameters, ex)
            'Return False
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return True
    End Function


    'Public Function InsertarObligacionesTecnicasPorExcel(ByVal FechaObligacionTecnica As Decimal, ByVal dtObligacionTecnica As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
    '    'Dim Codigo = ObtenerCodigoEjecucion(dataRequest)
    '    'Dim parameters As Object() = {dtObligacionTecnica, dataRequest}

    '    Try
    '        Dim oObligacionTecnicaDAM As New ObligacionTecnicaDAM
    '        'Codigo =
    '        oObligacionTecnicaDAM.InsertarObligacionesTecnicasPorExcel(FechaObligacionTecnica, dtObligacionTecnica, dataRequest, strmensaje)
    '        'RegistrarAuditora(parameters)
    '    Catch ex As Exception
    '        Dim sms As String = ex.Message
    '        Dim src As String = ex.Source
    '        'RegistrarAuditora(parameters, ex)
    '        Return False
    '        Dim rethrow As Boolean = True
    '        If (rethrow) Then
    '            Throw
    '        End If
    '    End Try
    '    Return True
    'End Function

End Class
