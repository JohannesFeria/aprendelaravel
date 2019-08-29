Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
Public Class TipoTituloBM
    Inherits InvokerCOM
    Public Sub New()
    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltro(ByVal codigoTipoTitulo As String, ByVal codigoMoneda As String, ByVal codigoTipoInstrumento As String, ByVal descripcion As String,
    ByVal Codigo As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoTituloBE
        Try
            Dim oTipoTitulo As TipoTituloBE
            oTipoTitulo = New TipoTituloDAM().SeleccionarPorFiltro(codigoTipoTitulo, codigoMoneda, codigoTipoInstrumento, descripcion, Codigo, situacion, dataRequest)
            Return oTipoTitulo
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Seleccionar(ByVal codigoTipoTitulo As String, ByVal dataRequest As DataSet) As TipoTituloBE
        Try
            Return New TipoTituloDAM().Seleccionar(codigoTipoTitulo, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As TipoTituloBE
        Try
            Return New TipoTituloDAM().Listar(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarPorDepositoPlazos(ByVal dataRequest As DataSet) As TipoTituloBE
        Try
            Return New TipoTituloDAM().ListarPorDepositoPlazo(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Lista los tipos de titulo por categoria de instrumento
    Public Function ListarTipoTitulo_CCI(ByVal Categoria As String) As DataTable
        Try
            Return New TipoTituloDAM().ListarTipoTitulo_CCI(Categoria)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ObtenerFechaPorTipoTitulo(ByVal fechaOperacion As Decimal, ByVal codigoTipoInstrumentoSBS As String, ByVal dataRequest As DataSet) As String
        Dim resul As String = ""
        Try
            resul = New TipoTituloDAM().ObtenerFechaPorTipoTitulo(fechaOperacion, codigoTipoInstrumentoSBS)
        Catch ex As Exception
            Throw ex
        End Try
        Return resul
    End Function
    Public Function ListarOI(ByVal dataRequest As DataSet) As TipoTituloBE
        Try
            Return New TipoTituloDAM().ListarOI(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarPorCI(ByVal claseIns As String, ByVal dataRequest As DataSet) As TipoTituloBE
        Dim auxBE As TipoTituloBE
        Try
            auxBE = New TipoTituloDAM().ListarPorCI(claseIns)
        Catch ex As Exception
            Throw ex
        End Try
        Return auxBE
    End Function
    Public Function ListarPorTipoInstrumento(ByVal strCodigoTipoInstrumentoSBS As String, ByVal dataRequest As DataSet) As TipoTituloBE
        Dim auxBE As TipoTituloBE
        Try
            auxBE = New TipoTituloDAM().ListarPorTipoInstrumento(strCodigoTipoInstrumentoSBS)
        Catch ex As Exception
            Throw ex
        End Try
        Return auxBE
    End Function
    Public Function ObtenerTasaEncaje(ByVal strCodigoTipoTitulo As String, ByVal strCodigoMnemonico As String, ByVal dataRequest As DataSet) As String
        Dim resul As String = ""
        Try
            resul = New TipoTituloDAM().ObtenerTasaEncaje(strCodigoTipoTitulo, strCodigoMnemonico)
        Catch ex As Exception
            Throw ex
        End Try
        Return resul
    End Function
#End Region
#Region " /* Funciones Insertar */ "
    Public Function Insertar(ByVal oTipoTituloBE As TipoTituloBE, ByVal dataRequest As DataSet) As String
        Dim codigo As String = String.Empty
        Try
            Dim oTipoTituloDAM As New TipoTituloDAM
            codigo = oTipoTituloDAM.Insertar(oTipoTituloBE, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return codigo
    End Function
#End Region
#Region " /* Funciones Modificar */"
    Public Function Modificar(ByVal oTipoTituloBE As TipoTituloBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oTipoTituloDAM As New TipoTituloDAM
            actualizado = oTipoTituloDAM.Modificar(oTipoTituloBE, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function
#End Region
#Region " /* Funciones Eliminar */"
    Public Function Eliminar(ByVal codigoTipoTitulo As String, ByVal dataRequest As DataSet)
        Dim eliminado As Boolean = False
        Try
            Dim oTipoTituloDAM As New TipoTituloDAM
            eliminado = oTipoTituloDAM.Eliminar(codigoTipoTitulo, dataRequest)
        Catch ex As Exception
            Throw ex
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
End Class