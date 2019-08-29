Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class CarteraIndirectaBM
    Public Sub New()
    End Sub
    Public Function SeleccionarPorFiltro(ByVal CodigoCarteraI As String, ByVal FechaCarteraI As Decimal, ByVal GrupoEconomico As String, ByVal Fondo As String, ByVal Emisor As String, ByVal Tipo As String, ByVal CodigoPortafolio As String, ByVal CodigoEntidad As String, ByVal dataRequest As DataSet) As DataSet
        Dim objDS As DataSet
        Try
            'RegistrarAuditora(parameters)
            objDS = New CarteraIndirectaDAM().SeleccionarPorFiltro(CodigoCarteraI, FechaCarteraI, GrupoEconomico, Fondo, Emisor, Tipo, CodigoPortafolio, CodigoEntidad, dataRequest)
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

    Public Function Eliminar(ByVal CodigoCarteraIndirecta As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Dim parameters As Object() = {codigoTipoCambioDI, dataRequest}
        Try
            Dim oCarteraIndirectaDAM As New CarteraIndirectaDAM
            eliminado = oCarteraIndirectaDAM.Eliminar(CodigoCarteraIndirecta, dataRequest)
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

    Public Function Insertar(ByVal oCateraIndirecta As CarteraIndirectaBE, ByVal dataRequest As DataSet) As Boolean
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Dim parameters As Object() = {oObligacionTecnica, dataRequest}
        Try
            Dim oCarteraIndirectaDAM As New CarteraIndirectaDAM
            oCarteraIndirectaDAM.Insertar(oCateraIndirecta, dataRequest)
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

    Public Function Modificar(ByVal ob As CarteraIndirectaBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Dim parameters As Object() = {ob, dataRequest}
        Try
            Dim oCarteraIndirectaDAM As New CarteraIndirectaDAM

            actualizado = oCarteraIndirectaDAM.Modificar(ob, dataRequest)
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

    Public Function Listar(ByVal CodigoEntidad As String, ByVal dataRequest As DataSet) As DataSet
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Dim parameters As Object() = {dataRequest}
        Try
            Return New CarteraIndirectaDAM().Listar_DatosEntidad(CodigoEntidad)
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
    End Function

    Public Function DesactivarRegistrosExcel(ByVal Fecha As Decimal, ByVal dataRequest As DataSet, ByRef strMensaje As String)
        Try
            Dim oCarteraIndirectaDAM As New CarteraIndirectaDAM
            oCarteraIndirectaDAM.DesactivarRegistrosExcel(Fecha, dataRequest, strMensaje)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function InsertarRegistrosExcel(ByVal oCateraIndirecta As CarteraIndirectaBE, ByVal dataRequest As DataSet) As Boolean
        'Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Dim parameters As Object() = {oObligacionTecnica, dataRequest}
        Try
            Dim oCarteraIndirectaDAM As New CarteraIndirectaDAM
            oCarteraIndirectaDAM.InsertarRegistrosExcel(oCateraIndirecta, dataRequest)
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
End Class
