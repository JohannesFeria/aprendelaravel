Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class ArchivoPlanoEstructuraBM
    Inherits InvokerCOM

#Region "/* Funciones Seleccionar */"

    Public Function Seleccionar(ByVal ArchivoCodigo As String, ByVal ColumnaOrden As Integer, ByVal dataRequest As DataSet) As CustodioArchivoBE
        Try
            Return New ArchivoPlanoEstructuraDAM().Seleccionar(ArchivoCodigo, ColumnaOrden, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Listar(ByVal ArchivoCodigo As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New ArchivoPlanoEstructuraDAM().Listar(ArchivoCodigo)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

    Public Function Insertar(ByVal oArchivoPlanoEstructura As ArchivoPlanoEstructuraBE, ByVal dataRequest As DataSet)
        Try
            Dim oArchivoPlanoEstructuraDAM As New ArchivoPlanoEstructuraDAM
            oArchivoPlanoEstructuraDAM.Insertar(oArchivoPlanoEstructura, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Modificar(ByVal oArchivoPlanoEstructura As ArchivoPlanoEstructuraBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oArchivoPlanoEstructuraDAM As New ArchivoPlanoEstructuraDAM
            actualizado = oArchivoPlanoEstructuraDAM.Modificar(oArchivoPlanoEstructura, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function

    Public Function Eliminar(ByVal ArchivoCodigo As String, ByVal ColumnaOrden As Integer, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Try
            Dim oArchivoPlanoEstructuraDAM As New ArchivoPlanoEstructuraDAM
            eliminado = oArchivoPlanoEstructuraDAM.Eliminar(ArchivoCodigo, ColumnaOrden, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function

End Class
