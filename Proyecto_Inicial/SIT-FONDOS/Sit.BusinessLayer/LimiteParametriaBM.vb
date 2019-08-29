Imports System
Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class LimiteParametriaBM
#Region "GrupoPorTipoMoneda"
    Public Sub GrupoPorTipoMoneda_Insertar(ByVal CodigoTipoMoneda As String, ByVal Descripcion As String, ByVal MonedaPortafolio As String, ByVal Forward As String,
    ByVal Cartera As String, ByVal Situacion As String, ByVal dataRequest As DataSet)
        Try
            Dim oLP As New LimiteParametriaDAM
            oLP.GrupoPorTipoMoneda_Insertar(CodigoTipoMoneda, Descripcion, MonedaPortafolio, Forward, Cartera, Situacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GrupoPorTipoMoneda_Modificar(ByVal CodigoTipoMoneda As String, ByVal Descripcion As String, ByVal MonedaPortafolio As String, ByVal Forward As String,
    ByVal Cartera As String, ByVal Situacion As String, ByVal dataRequest As DataSet)
        Try
            Dim oLP As New LimiteParametriaDAM
            oLP.GrupoPorTipoMoneda_Modificar(CodigoTipoMoneda, Descripcion, MonedaPortafolio, Forward, Cartera, Situacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function GrupoPorTipoMoneda_Seleccionar(ByVal CodigoTipoMoneda As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorTipoMoneda_Seleccionar(CodigoTipoMoneda)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GrupoPorTipoMoneda_Buscar(ByVal CodigoTipoMoneda As String, ByVal Descripcion As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorTipoMoneda_Buscar(CodigoTipoMoneda, Descripcion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
#Region "Clase Instrumento"
    Public Function GrupoClaseInstrumento_Grupo(ByVal CodigoClaseInstrumento As String, ByVal Descripcion As String, ByVal Existe As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoClaseInstrumento_Grupo(CodigoClaseInstrumento, Descripcion, Existe)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GrupoClaseInstrumento_Buscar(ByVal CodigoTipoMoneda As String, ByVal Descripcion As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoClaseInstrumento_Buscar(CodigoTipoMoneda, Descripcion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GrupoClaseInstrumento_Seleccionar(ByVal CodigoClaseInstrumento As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoClaseInstrumento_Seleccionar(CodigoClaseInstrumento)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub GrupoClaseInstrumento_Insertar(ByVal CodigoClaseInstrumento As String, ByVal Descripcion As String, ByVal ClaseInstrumento As String,
    ByVal Situacion As String, ByVal dataRequest As DataSet)
        Try
            Dim oLP As New LimiteParametriaDAM
            oLP.GrupoClaseInstrumento_Insertar(CodigoClaseInstrumento, Descripcion, ClaseInstrumento, Situacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GrupoClaseInstrumento_Borrar(ByVal CodigoClaseInstrumento As String)
        Try
            Dim oLP As New LimiteParametriaDAM
            oLP.GrupoClaseInstrumento_Borrar(CodigoClaseInstrumento)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
#Region "Entidad"
    Public Function GrupoPorEntidad_Grupo(ByVal CodigoClaseInstrumento As String, Descripcion As String, ByVal Existe As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorEntidad_Grupo(CodigoClaseInstrumento, Descripcion, Existe)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GrupoPorEntidad_Buscar(ByVal CodigoTipoMoneda As String, ByVal Descripcion As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorEntidad_Buscar(CodigoTipoMoneda, Descripcion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GrupoPorEntidad_Seleccionar(ByVal CodigoClaseInstrumento As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorEntidad_Seleccionar(CodigoClaseInstrumento)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub GrupoPorEntidad_Insertar(ByVal CodigoClaseInstrumento As String, ByVal Descripcion As String, ByVal ClaseInstrumento As String,
    ByVal Situacion As String, ByVal dataRequest As DataSet)
        Try
            Dim oLP As New LimiteParametriaDAM
            oLP.GrupoPorEntidad_Insertar(CodigoClaseInstrumento, Descripcion, ClaseInstrumento, Situacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GrupoPorEntidad_Borrar(ByVal CodigoClaseInstrumento As String)
        Try
            Dim oLP As New LimiteParametriaDAM
            oLP.GrupoPorEntidad_Borrar(CodigoClaseInstrumento)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
#Region "GrupoPorDerivados"
    Public Sub GrupoPorDerivados_Insertar(ByVal CodigoGrupoDerivado As String, ByVal Descripcion As String, ByVal MonedaPortafolio As String, ByVal Forward As String,
    ByVal Swap As String, ByVal NominalRecibir As String, ByVal Situacion As String, ByVal dataRequest As DataSet)
        Try
            Dim oLP As New LimiteParametriaDAM
            oLP.GrupoPorDerivados_Insertar(CodigoGrupoDerivado, Descripcion, MonedaPortafolio, Forward, Swap, NominalRecibir, Situacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GrupoPorDerivados_Modificar(ByVal CodigoGrupoDerivado As String, ByVal Descripcion As String, ByVal MonedaPortafolio As String, ByVal Forward As String,
    ByVal Swap As String, ByVal NominalRecibir As String, ByVal Situacion As String, ByVal dataRequest As DataSet)
        Try
            Dim oLP As New LimiteParametriaDAM
            oLP.GrupoPorDerivados_Modificar(CodigoGrupoDerivado, Descripcion, MonedaPortafolio, Forward, Swap, NominalRecibir, Situacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function GrupoPorDerivados_Seleccionar(ByVal CodigoGrupoDerivado As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorDerivados_Seleccionar(CodigoGrupoDerivado)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GrupoPorDerivados_Buscar(ByVal CodigoGrupoDerivado As String, ByVal Descripcion As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorDerivados_Buscar(CodigoGrupoDerivado, Descripcion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
#Region "GrupoPorNemonico"
    Public Function GrupoPorNemonico_Grupo(ByVal CodigoGrupoNemonico As String, Descripcion As String, ByVal Existe As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorNemonico_Grupo(CodigoGrupoNemonico, Descripcion, Existe)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GrupoPorNemonico_Buscar(ByVal CodigoGrupoNemonico As String, ByVal Descripcion As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorNemonico_Buscar(CodigoGrupoNemonico, Descripcion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GrupoPorNemonico_Seleccionar(ByVal CodigoGrupoNemonico As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorNemonico_Seleccionar(CodigoGrupoNemonico)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub GrupoPorNemonico_Insertar(ByVal CodigoGrupoNemonico As String, ByVal Descripcion As String, ByVal CodigoNemonico As String,
    ByVal Situacion As String, ByVal dataRequest As DataSet)
        Try
            Dim oLP As New LimiteParametriaDAM
            oLP.GrupoPorNemonico_Insertar(CodigoGrupoNemonico, Descripcion, CodigoNemonico, Situacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GrupoPorNemonico_Borrar(ByVal CodigoGrupoNemonico As String)
        Try
            Dim oLP As New LimiteParametriaDAM
            oLP.GrupoPorNemonico_Borrar(CodigoGrupoNemonico)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
#Region "GrupoPorCalificacion"
    Public Function GrupoPorCalificacion_Grupo(ByVal CodigoGrupoClasificacion As String, ByVal Existe As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorCalificacion_Grupo(CodigoGrupoClasificacion, Existe)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GrupoPorCalificacion_Buscar(ByVal CodigoGrupoClasificacion As String, ByVal Descripcion As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorCalificacion_Buscar(CodigoGrupoClasificacion, Descripcion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GrupoPorCalificacion_Seleccionar(ByVal CodigoGrupoClasificacion As String) As DataTable
        Try
            Dim oLP As New LimiteParametriaDAM
            Return oLP.GrupoPorCalificacion_Seleccionar(CodigoGrupoClasificacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub GrupoPorCalificacion_Insertar(ByVal CodigoGrupoClasificacion As String, ByVal Local As String, ByVal DPZ As String, ByVal Descripcion As String, ByVal CodigoCalificacion As String,
    ByVal Situacion As String, ByVal dataRequest As DataSet)
        Try
            Dim oLP As New LimiteParametriaDAM
            oLP.GrupoPorCalificacion_Insertar(CodigoGrupoClasificacion, Descripcion, CodigoCalificacion, Local, DPZ, Situacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub GrupoPorCalificacion_Borrar(ByVal CodigoGrupoClasificacion As String)
        Try
            Dim oLP As New LimiteParametriaDAM
            oLP.GrupoPorCalificacion_Borrar(CodigoGrupoClasificacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
End Class