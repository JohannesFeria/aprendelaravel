Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class SectorEmpresarialBM
    'Inherits InvokerCOM
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltros(ByVal codigoSectorEmpresarial As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As SectorEmpresarialBE
        Try
            Return New SectorEmpresarialDAM().SeleccionarPorFiltros(codigoSectorEmpresarial, descripcion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoSectorEmpresarial As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As SectorEmpresarialBE
        Try
            Dim oSectorEmpresarialDAM As New SectorEmpresarialDAM
            Dim ds As DataSet = oSectorEmpresarialDAM.SeleccionarPorFiltro(codigoSectorEmpresarial, descripcion, situacion, dataRequest)
            Return ds
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Seleccionar(ByVal codigoSectorEmpresarial As String, ByVal dataRequest As DataSet) As SectorEmpresarialBE
        Try
            Return New SectorEmpresarialDAM().Seleccionar(codigoSectorEmpresarial, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As SectorEmpresarialBE
        Try
            Return New SectorEmpresarialDAM().Listar(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region " /* Funciones Insertar */ "
    Public Function Insertar(ByVal oSectorEmpresarial As SectorEmpresarialBE, ByVal dataRequest As DataSet)
        Try
            Dim oSectorEmpresarialDAM As New SectorEmpresarialDAM
            oSectorEmpresarialDAM.Insertar(oSectorEmpresarial, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region " /* Funciones Modificar */ "
    Public Function Modificar(ByVal oSectorEmpresarial As SectorEmpresarialBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oSectorEmpresarialDAM As New SectorEmpresarialDAM
            actualizado = oSectorEmpresarialDAM.Modificar(oSectorEmpresarial, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function
#End Region

#Region " /* Funciones Eliminar */ "
    Public Function Eliminar(ByVal codigoSectorEmpresarial As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Try
            Dim oSectorEmpresarialDAM As New SectorEmpresarialDAM
            eliminado = oSectorEmpresarialDAM.Eliminar(codigoSectorEmpresarial, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function
#End Region

    '#Region " /* Funciones Personalizadas*/"
    '    Public Sub Ingresar(ByVal dataRequest As DataSet)
    '        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '        Dim parameters As Object() = {dataRequest}
    '        RegistrarAuditora(parameters)
    '    End Sub
    '    Public Sub Actualizar(ByVal dataRequest As DataSet)
    '        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '        Dim parameters As Object() = {dataRequest}
    '        RegistrarAuditora(parameters)
    '    End Sub

    '    Public Sub Salir(ByVal dataRequest As DataSet)
    '        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '        Dim parameters As Object() = {dataRequest}
    '        RegistrarAuditora(parameters)
    '    End Sub
    '#End Region

End Class

