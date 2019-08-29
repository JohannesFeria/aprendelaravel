Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports System.Data.Common

Public Class PrevisionParametroBM

    Public Function Listar(ByVal strParametro As String) As DataSet
        Dim oParametroDAM As New PrevisionParametroDAM
        Dim oData As New DataSet
        Try
            oData = oParametroDAM.Listar(strParametro)
        Catch ex As Exception
            Throw ex
        End Try
        Return oData
    End Function

    Public Function Eliminar(ByVal strParametro As String) As Boolean
        Dim oParametroDAM As New PrevisionParametroDAM
        Try
            oParametroDAM.Eliminar(strParametro)
        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Public Function SeleccionarPorCodigo(ByVal strParametro As String) As DataSet
        Dim oParametroDAM As New PrevisionParametroDAM
        Dim oData As New DataSet
        Try
            oData = oParametroDAM.Seleccionar(strParametro)
        Catch ex As Exception
            Throw ex
        End Try
        Return oData
    End Function

    Public Function Insertar(ByVal IdTabla As Integer, ByVal strParametro As String, ByVal strValor As String) As String
        Dim oParametroDAM As New PrevisionParametroDAM
        Dim Codigo As String
        Try
            Codigo = oParametroDAM.Insertar(IdTabla, strParametro, strValor)
        Catch ex As Exception
            Throw ex
        End Try
        Return Codigo
    End Function

    Public Function SeleccionarPorCodigos(ByVal strParametro As String) As DataSet
        Dim oParametroDAM As New PrevisionParametroDAM
        Dim oData As New DataSet
        Try
            oData = oParametroDAM.SeleccionarPorCodigos(strParametro)
        Catch ex As Exception
            Throw ex
        End Try
        Return oData
    End Function

    Public Function SeleccionarDescripcion(ByVal iIdTabla As Integer, ByVal Valor As String) As String
        Dim oParametroDAM As New PrevisionParametroDAM
        Dim oData As String
        Try
            oData = oParametroDAM.SeleccionarDescripcion(iIdTabla, Valor)
        Catch ex As Exception
            Throw ex
        End Try
        Return oData
    End Function

    Public Function SeleccionarPorCodigos2(ByVal strParametro As String) As DataSet
        Dim oParametroDAM As New PrevisionParametroDAM
        Dim oData As New DataSet
        Try
            oData = oParametroDAM.SeleccionarPorCodigos2(strParametro)
        Catch ex As Exception
            Throw ex
        End Try
        Return oData
    End Function

End Class
