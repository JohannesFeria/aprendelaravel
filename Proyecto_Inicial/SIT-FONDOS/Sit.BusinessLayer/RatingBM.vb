'Creacion BPesantes 06-12-2016 OT 9679
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class RatingBM
    Inherits InvokerCOM

    Public Sub New()
    End Sub

    Public Sub Borrar_Rating(ByVal FechaProceso As Decimal, ByVal tipoArchivo As String, ByVal dataRequest As DataSet)
        Try
            Dim oRatingDAM As New RatingDAM
            oRatingDAM.Borrar_Rating(FechaProceso, tipoArchivo, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Function Insertar(ByVal orowRating As RatingBE.RegistroRatingRow, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String = ""
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {orowRating, dataRequest}
        Try
            Dim oRatingDAM As New RatingDAM
            strCodigo = oRatingDAM.Insertar(orowRating, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return strCodigo
    End Function

    Function SeleccionarPorFecha(ByVal fechaProceso As Decimal, ByVal tipoArchivo As String, ByVal dataRequest As DataSet) As DataTable
        Dim strCodigo As String = ""
        Dim parameters As Object() = {fechaProceso}
        Dim dt As DataTable
        Try
            Dim oRatingDAM As New RatingDAM
            RegistrarAuditora(parameters)
            dt = oRatingDAM.SeleccionarPorFecha(fechaProceso, tipoArchivo, dataRequest)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dt
    End Function


    Function ObtenerCodigoRatingxNombre(ByVal Rating As String) As DataTable

        Dim dt As DataTable
        Try
            Dim oRatingDAM As New RatingDAM
            dt = oRatingDAM.ObtenerCodigoRatingxNombre(Rating)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dt
    End Function

End Class
