Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Public Class CodigoValorBM
    Dim oCodigoValorDAM As New CodigoValorDAM
    Public Function InsertarCodigoValor(ByVal objCodigoValor As CodigoValorBE, ByVal dataRequest As DataSet) As Boolean
        Try
            Return oCodigoValorDAM.InsertarCodigoValor(objCodigoValor, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ModificarCodigoValor(ByVal objCodigoValor As CodigoValorBE, ByVal dataRequest As DataSet) As Boolean
        Try
            Return oCodigoValorDAM.ModificarCodigoValor(objCodigoValor, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11008 - 18/01/2018 - Ian Pastor M.
    'Cambiar parámetro código valor por el Id de la tabla
    Public Function EliminarCodigoValor(ByVal id As Integer, ByVal dataRequest As DataSet) As Boolean
        Try
            Return oCodigoValorDAM.EliminarCodigoValor(id, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarCodigoValor(Optional ByVal CodigoValor As String = "", Optional ByVal Situacion As String = "", Optional ByVal CodigoNemonico As String = "", _
                                      Optional ByVal CodigoEntidad As String = "", Optional ByVal CodigoMoneda As String = "") As CodigoValorBE
        Try
            Return oCodigoValorDAM.ListarCodigoValor(CodigoValor, Situacion, CodigoNemonico, CodigoEntidad, CodigoMoneda)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT11008 - 18/01/2018 - Ian Pastor M.
    'Descripcion: Agregar nuevo parámetro id
    Public Function ListarCodigoValor(ByVal id As Integer, ByVal codigoValor As String, ByVal codigoNemonico As String, _
                                      ByVal codigoTercero As String, ByVal codigoMoneda As String, ByVal codigoTipoCupon As String, _
                                      ByVal situacion As String) As CodigoValorBE
        Try
            Return oCodigoValorDAM.ListarCodigoValor(id, codigoValor, codigoNemonico, codigoTercero, codigoMoneda, codigoTipoCupon, situacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function NemonicoCodigoValor() As DataTable
        Return oCodigoValorDAM.NemonicoCodigoValor()
    End Function
    Public Function TipoInstrumento_SMV() As DataTable
        Return oCodigoValorDAM.TipoInstrumento_SMV()
    End Function
End Class