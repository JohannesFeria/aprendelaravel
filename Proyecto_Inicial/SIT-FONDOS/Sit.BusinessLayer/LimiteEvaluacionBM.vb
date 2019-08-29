Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer

Public Class LimiteEvaluacionBM
    Inherits InvokerCOM

#Region " /* Declaraciòn de variables */ "

    'Private m_dsLimite As New LimiteBE
    'Private m_dsReporte As New DataSet
    'Private m_dsDatarequest As New DataSet
    'Private m_blnResultado As System.Boolean = True
    'Private m_strCodigoNemonico As String = String.Empty
    'Private m_oLimite As ILimiteBM = New LimiteBM
    'Private m_oThread() As Threading.Thread

#End Region

#Region " /* Propiedades */ "

    'Private Property Limite() As LimiteBE
    '    Get
    '        Return m_dsLimite
    '    End Get
    '    Set(ByVal Value As LimiteBE)
    '        Value = m_dsLimite
    '    End Set
    'End Property

    'Public Property Reporte() As DataSet
    '    Get
    '        Return m_dsReporte
    '    End Get
    '    Set(ByVal Value As DataSet)
    '        Value = m_dsReporte
    '    End Set
    'End Property

    'Private Property [Datarequest]() As DataSet
    '    Get
    '        Return m_dsDatarequest
    '    End Get
    '    Set(ByVal Value As DataSet)
    '        Value = m_dsDatarequest
    '    End Set
    'End Property

    'Private Property [Nemonico]() As String
    '    Get
    '        Return m_strCodigoNemonico
    '    End Get
    '    Set(ByVal Value As String)
    '        Value = m_strCodigoNemonico
    '    End Set
    'End Property

#End Region

#Region " /* Constructor */ "

    Public Sub New()

    End Sub

#End Region
#Region " /* Funciones Principales */ "
    Public Function Evaluar(ByVal codigoOperacion As String, ByVal codigoNemonico As String, ByVal cantidadValor As Decimal, ByVal montoNominal As Decimal, ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal, ByVal DataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(DataRequest)
        Dim parameters As Object() = {codigoOperacion, codigoNemonico, cantidadValor, montoNominal, codigoPortafolio, fechaOperacion, DataRequest}
        Dim dsLimitesEvaluados As DataSet
        '---------------------------------------------------
        '1. Buscar Limites Asociados al Nemonico
        '2. Procesar Limites Asociados al Nemonico
        '3. Obtener Resultado General
        '---------------------------------------------------
        Try
            Dim oObjectDAM As New LimiteDAM
            dsLimitesEvaluados = oObjectDAM.LimiteEvaluar(codigoOperacion, codigoNemonico, cantidadValor, montoNominal, codigoPortafolio, fechaOperacion, DataRequest)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        '---------------------------------------------------  
        Return (dsLimitesEvaluados)
    End Function
    'RGF 20081009
    Public Function ListarExcesosLimitesOnLine(ByVal codigoNemonico As String, ByVal CantidadOperacion As Decimal, _
                ByVal CodigoPortafolio As String, ByVal codigoOperacion As String, ByVal DataRequest As DataSet, Optional ByVal codigoTercero As String = "") As DataSet 'HDG 20120112
        Dim codigoEjecucion = ObtenerCodigoEjecucion(DataRequest)
        'Dim parameters As Object() = {codigoOperacion, codigoNemonico, cantidadValor, montoNominal, codigoPortafolio, fechaOperacion, DataRequest}
        Dim dtLimitesEvaluados As DataSet
        Try
            Dim oObjectDAM As New LimiteDAM
            dtLimitesEvaluados = oObjectDAM.ListarExcesosLimitesOnLine(codigoNemonico, CantidadOperacion, CodigoPortafolio, codigoOperacion, DataRequest, codigoTercero)    'HDG 20120112
        Catch ex As Exception
            'RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        '---------------------------------------------------  
        Return (dtLimitesEvaluados)
    End Function
#End Region
End Class