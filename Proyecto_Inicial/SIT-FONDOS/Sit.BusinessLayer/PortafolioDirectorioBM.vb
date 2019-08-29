Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class PortafolioDirectorioBM
    Inherits InvokerCOM

    Public Function Seleccionar(ByVal PortafolioCodigo As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New PortafolioDirectorioDAM().Seleccionar(PortafolioCodigo, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Insertar(ByVal oPortafolioDirectorio As PortafolioDirectorioBE, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim oPortafolioDirectorioDAM As New PortafolioDirectorioDAM
            Return oPortafolioDirectorioDAM.Insertar(oPortafolioDirectorio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Modificar(ByVal oPortafolioDirectorio As PortafolioDirectorioBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oPortafolioDirectorioDAM As New PortafolioDirectorioDAM
            actualizado = oPortafolioDirectorioDAM.Modificar(oPortafolioDirectorio, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function

    Public Function Eliminar(ByVal PortafolioCodigo As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Try
            Dim oPortafolioDirectorioDAM As New PortafolioDirectorioDAM
            eliminado = oPortafolioDirectorioDAM.Eliminar(PortafolioCodigo, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function

End Class
