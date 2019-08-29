Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities
Public Class EncajeDetalleDAM
    Private oEncajeDetalle As EncajeDetalleBE.EncajeDetalleRow
    Public Sub New()

    End Sub


    ''' <summary>
    ''' Selecciona de acuerdo a los parametros enviados
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorFiltro(ByVal objRow As EncajeDetalleBE.EncajeDetalleRow) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EncajeDetalle_SeleccionarPorFiltro")
        oEncajeDetalle = CType(objRow, EncajeDetalleBE.EncajeDetalleRow)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oEncajeDetalle.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaEncaje", DbType.String, oEncajeDetalle.FechaEncaje)
        db.AddInParameter(dbCommand, "@p_NumeroDias", DbType.String, oEncajeDetalle.NumeroDias)
        db.AddInParameter(dbCommand, "@p_PromedioValoracion", DbType.String, oEncajeDetalle.PromedioValoracion)
        db.AddInParameter(dbCommand, "@p_ValorTasa", DbType.String, oEncajeDetalle.ValorTasa)
        db.AddInParameter(dbCommand, "@p_ValorEncaje", DbType.String, oEncajeDetalle.ValorEncaje)
        db.AddInParameter(dbCommand, "@p_ValorPromedioMantenido", DbType.String, oEncajeDetalle.ValorPromedioMantenido)
        db.AddInParameter(dbCommand, "@p_CodigoCalificacion", DbType.String, oEncajeDetalle.CodigoCalificacion)
        'Return db.ExecuteDataSet(dbCommand)
        Dim dsEncajeDetalle As New DataSet
        db.LoadDataSet(dbCommand, dsEncajeDetalle, "VectorPrecio")
        Return dsEncajeDetalle
    End Function
    Public Function ResultadosEncaje(ByVal Portafolio As String, ByVal fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EncajeDetalle_ResultadosEncaje")
        dbCommand.CommandTimeout = 600
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_FechaEncaje", DbType.Decimal, fecha)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ReporteResultadosEncaje(ByVal Portafolio As String, ByVal fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("EncajeDetalle_ReporteResultadosEncaje")
        dbCommand.CommandTimeout = 600
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_FechaEncaje", DbType.Decimal, fecha)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    ' OT 61609 REQ 37 20101122 PLD
    Public Function ProvisionContableImpuesto(ByVal p_fechainicio As Decimal, _
                                      ByVal p_fechafin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteProvisionContableImpuestoRentaAnual")
        dbCommand.CommandTimeout = 600 'RGF 20101227
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Int32, p_fechainicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Int32, p_fechafin)
        Return db.ExecuteDataSet(dbCommand)

    End Function

    'RGF 20101123 OT 61609
    Public Function ReporteImpuestoRentaAnualPorNemonico(ByVal CodigoNemonico As String, ByVal Portafolio As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteImpuestoRentaAnualPorNemonico")
        dbCommand.CommandTimeout = 600
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, CodigoNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFin)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'CMB 20101124 OT 61609
    Public Function ObtenerUtilidadPorNemonico(ByVal Portafolio As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal CodigoNemonico As String)
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ObtenerUtilidadPorNemonico")
        dbCommand.CommandTimeout = 1620
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFin)
        db.AddInParameter(dbCommand, "@p_Nemonico", DbType.String, CodigoNemonico)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    'HDG OT 64771 20120227
    Public Function EliminarTablaTmpNemonicosFondoRenta() As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_Inicializar_TmpNemonicosFondoRenta")
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'HDG OT 64771 20120227
    Public Function ActualizarInstrumentosPorExcel(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sMnemonico As String
        Dim sFondo As String
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_ActualizaTmpNemonicosFondoRentaxExcel")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String)

        For Each filaLinea As DataRow In dtDetalle.Rows
            sMnemonico = filaLinea(0).ToString().Trim()
            sFondo = filaLinea(1).ToString().Trim()

            db.SetParameterValue(dbCommand, "@p_CodigoPortafolioSBS", sFondo)
            db.SetParameterValue(dbCommand, "@p_CodigoMnemonico", sMnemonico)
            db.ExecuteNonQuery(dbCommand)
        Next
        Return True
    End Function
End Class
