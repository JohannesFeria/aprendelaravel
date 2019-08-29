Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Text

Public Class ValorizacionAmortizadaDAM

    Public Function ComprasReferidasAlStock(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_val_rf_amortizada_compras_en_stock")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)

            Dim ds As New DataSet
            db.LoadDataSet(dbCommand, ds, New String() {"Ordenes", "Cuponeras"})
            Return ds
        End Using
    End Function

    Public Function RegNuevoProcesoValorizacion(ByVal codUsuario As String) As Integer
        'Dim sbQuery As New StringBuilder
        'sbQuery.AppendLine(" declare @p_NewId int; ")
        'sbQuery.AppendLine(" set @p_NewId = isnull((select max(IdProcesoValAmortizada) from ProcesoValAmortizada), 0) + 1; ")
        'sbQuery.AppendLine(" insert into ProcesoValAmortizada(IdProcesoValAmortizada, Usuario) values(@p_NewId, @p_Usuario); ")
        'sbQuery.AppendLine(" select @p_NewId as IdProcesoValAmortizada; ")

        'Dim db As Database = DatabaseFactory.CreateDatabase()

        'Using dbCommand As DbCommand = db.GetSqlStringCommand(sbQuery.ToString())
        '    db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, codUsuario)
        '    Return db.ExecuteScalar(dbCommand)
        'End Using

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionAmortizada_Insertar_Proceso")
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, codUsuario)
            Return db.ExecuteScalar(dbCommand)
        End Using
    End Function

    Public Function ActProcesoValorizacionOk(ByVal idProceso As Integer) As Integer
        'Dim sbQuery As New StringBuilder
        'sbQuery.AppendLine(" update ProcesoValAmortizada ")
        'sbQuery.AppendLine(" set FechaFin = getdate(), ProcesoCorrecto = 1 where IdProcesoValAmortizada = @p_IdProcesoValAmortizada; ")

        'Dim db As Database = DatabaseFactory.CreateDatabase()

        'Using dbCommand As DbCommand = db.GetSqlStringCommand(sbQuery.ToString())
        '    db.AddInParameter(dbCommand, "@p_IdProcesoValAmortizada", DbType.Int32, idProceso)
        '    Return db.ExecuteNonQuery(dbCommand)
        'End Using

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionAmortizada_Modificar_Proceso")
            db.AddInParameter(dbCommand, "@p_IdProcesoValAmortizada", DbType.Int32, idProceso)
            db.AddInParameter(dbCommand, "@p_ProcesoCorrecto", DbType.Int32, 1)
            db.AddInParameter(dbCommand, "@p_MensajeError", DbType.String, "")

            Return db.ExecuteNonQuery(dbCommand)
        End Using
    End Function

    Public Function ActProcesoValorizacionError(ByVal idProceso As Integer, ByVal msgError As String) As Integer
        'Dim sbQuery As New StringBuilder
        'sbQuery.AppendLine(" update ProcesoValAmortizada ")
        'sbQuery.AppendLine(" set FechaFin = getdate(), MensajeError = @p_MensajeError where IdProcesoValAmortizada = @p_IdProcesoValAmortizada; ")

        'Dim db As Database = DatabaseFactory.CreateDatabase()

        'Using dbCommand As DbCommand = db.GetSqlStringCommand(sbQuery.ToString())
        '    db.AddInParameter(dbCommand, "@p_IdProcesoValAmortizada", DbType.Int32, idProceso)
        '    db.AddInParameter(dbCommand, "@p_MensajeError", DbType.String, msgError)
        '    Return db.ExecuteNonQuery(dbCommand)
        'End Using

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionAmortizada_Modificar_Proceso")
            db.AddInParameter(dbCommand, "@p_IdProcesoValAmortizada", DbType.Int32, idProceso)
            db.AddInParameter(dbCommand, "@p_ProcesoCorrecto", DbType.Int32, 0)
            db.AddInParameter(dbCommand, "@p_MensajeError", DbType.String, msgError)

            Return db.ExecuteNonQuery(dbCommand)
        End Using
    End Function

    Public Sub EliminarValorizacion(ByVal codigoPortafolio As String, ByVal fechaProceso As Integer)
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionAmortizada_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fechaProceso)

            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

    Public Sub GuardarValorizacionRow(ByVal idProceso As Integer, ByVal rowValorizacion As ValorizacionAmortizadaBE.ValorizacionAmortizadaRow)
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Using dbCommand As DbCommand = db.GetStoredProcCommand("ValorizacionAmortizada_Insertar")
            db.AddInParameter(dbCommand, "@p_IdProcesoValAmortizada", DbType.Int32, idProceso)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, rowValorizacion.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, rowValorizacion.FechaProceso)
            db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, rowValorizacion.CodigoOrden)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, rowValorizacion.CodigoNemonico)
            db.AddInParameter(dbCommand, "@p_CantidadOperacion", DbType.Decimal, rowValorizacion.CantidadOperacion)
            db.AddInParameter(dbCommand, "@p_CantidadEnStock", DbType.Decimal, rowValorizacion.CantidadEnStock)

            db.AddInParameter(dbCommand, "@p_TIRCOM_ValorActual", DbType.Decimal, rowValorizacion.TIRCOM_ValorActual)
            db.AddInParameter(dbCommand, "@p_TIRCOM_InteresCorrido", DbType.Decimal, rowValorizacion.TIRCOM_InteresCorrido)
            db.AddInParameter(dbCommand, "@p_TIRCOM_ValorPrincipal", DbType.Decimal, rowValorizacion.TIRCOM_ValorPrincipal)
            db.AddInParameter(dbCommand, "@p_TIRCOM_PrecioLimpio", DbType.Decimal, rowValorizacion.TIRCOM_PrecioLimpio)
            db.AddInParameter(dbCommand, "@p_TIRCOM_PrecioSucio", DbType.Decimal, rowValorizacion.TIRCOM_PrecioSucio)
            db.AddInParameter(dbCommand, "@p_TIRCOM_TIR", DbType.Decimal, rowValorizacion.TIRCOM_TIR)
            db.AddInParameter(dbCommand, "@p_TIRCOM_FechaFinCuponActual", DbType.Decimal, rowValorizacion.TIRCOM_FechaFinCuponActual)
            db.AddInParameter(dbCommand, "@p_TIRCOM_MontoCuponActual", DbType.Decimal, rowValorizacion.TIRCOM_MontoCuponActual)
            db.AddInParameter(dbCommand, "@p_TIRCOM_CantidadOperacion", DbType.Decimal, rowValorizacion.TIRCOM_CantidadOperacion)
            db.AddInParameter(dbCommand, "@p_TIRCOM_ValorNominal", DbType.Decimal, rowValorizacion.TIRCOM_ValorNominal)
            db.AddInParameter(dbCommand, "@p_TIRCOM_SaldoNominalVigente", DbType.Decimal, rowValorizacion.TIRCOM_SaldoNominalVigente)
            db.AddInParameter(dbCommand, "@p_TIRCOM_PagoCuponVigente", DbType.Decimal, rowValorizacion.TIRCOM_PagoCuponVigente)

            db.AddInParameter(dbCommand, "@p_TIRRAZ_ValorActual", DbType.Decimal, rowValorizacion.TIRRAZ_ValorActual)
            db.AddInParameter(dbCommand, "@p_TIRRAZ_InteresCorrido", DbType.Decimal, rowValorizacion.TIRRAZ_InteresCorrido)
            db.AddInParameter(dbCommand, "@p_TIRRAZ_ValorPrincipal", DbType.Decimal, rowValorizacion.TIRRAZ_ValorPrincipal)
            db.AddInParameter(dbCommand, "@p_TIRRAZ_PrecioLimpio", DbType.Decimal, rowValorizacion.TIRRAZ_PrecioLimpio)
            db.AddInParameter(dbCommand, "@p_TIRRAZ_PrecioSucio", DbType.Decimal, rowValorizacion.TIRRAZ_PrecioSucio)
            db.AddInParameter(dbCommand, "@p_TIRRAZ_TIR", DbType.Decimal, rowValorizacion.TIRRAZ_TIR)
            db.AddInParameter(dbCommand, "@p_TIRRAZ_FechaFinCuponActual", DbType.Decimal, rowValorizacion.TIRRAZ_FechaFinCuponActual)
            db.AddInParameter(dbCommand, "@p_TIRRAZ_MontoCuponActual", DbType.Decimal, rowValorizacion.TIRRAZ_MontoCuponActual)
            db.AddInParameter(dbCommand, "@p_TIRRAZ_CantidadOperacion", DbType.Decimal, rowValorizacion.TIRRAZ_CantidadOperacion)
            db.AddInParameter(dbCommand, "@p_TIRRAZ_ValorNominal", DbType.Decimal, rowValorizacion.TIRRAZ_ValorNominal)
            db.AddInParameter(dbCommand, "@p_TIRRAZ_SaldoNominalVigente", DbType.Decimal, rowValorizacion.TIRRAZ_SaldoNominalVigente)
            db.AddInParameter(dbCommand, "@p_TIRRAZ_PagoCuponVigente", DbType.Decimal, rowValorizacion.TIRRAZ_PagoCuponVigente)

            db.AddInParameter(dbCommand, "@p_VTA_ValorActual", DbType.Decimal, rowValorizacion.VTA_ValorActual)
            db.AddInParameter(dbCommand, "@p_VTA_InteresCorrido", DbType.Decimal, rowValorizacion.VTA_InteresCorrido)
            db.AddInParameter(dbCommand, "@p_VTA_ValorPrincipal", DbType.Decimal, rowValorizacion.VTA_ValorPrincipal)
            db.AddInParameter(dbCommand, "@p_VTA_PrecioLimpio", DbType.Decimal, rowValorizacion.VTA_PrecioLimpio)
            db.AddInParameter(dbCommand, "@p_VTA_PrecioSucio", DbType.Decimal, rowValorizacion.VTA_PrecioSucio)
            db.AddInParameter(dbCommand, "@p_VTA_TIR", DbType.Decimal, rowValorizacion.VTA_TIR)
            db.AddInParameter(dbCommand, "@p_VTA_FechaFinCuponActual", DbType.Decimal, rowValorizacion.VTA_FechaFinCuponActual)
            db.AddInParameter(dbCommand, "@p_VTA_MontoCuponActual", DbType.Decimal, rowValorizacion.VTA_MontoCuponActual)
            db.AddInParameter(dbCommand, "@p_VTA_CantidadOperacion", DbType.Decimal, rowValorizacion.VTA_CantidadOperacion)
            db.AddInParameter(dbCommand, "@p_VTA_ValorNominal", DbType.Decimal, rowValorizacion.VTA_ValorNominal)
            db.AddInParameter(dbCommand, "@p_VTA_SaldoNominalVigente", DbType.Decimal, rowValorizacion.VTA_SaldoNominalVigente)
            db.AddInParameter(dbCommand, "@p_VTA_PagoCuponVigente", DbType.Decimal, rowValorizacion.VTA_PagoCuponVigente)

            db.AddInParameter(dbCommand, "@p_NEG_ValorActual", DbType.Decimal, rowValorizacion.NEG_ValorActual)
            db.AddInParameter(dbCommand, "@p_NEG_InteresCorrido", DbType.Decimal, rowValorizacion.NEG_InteresCorrido)
            db.AddInParameter(dbCommand, "@p_NEG_ValorPrincipal", DbType.Decimal, rowValorizacion.NEG_ValorPrincipal)
            db.AddInParameter(dbCommand, "@p_NEG_PrecioLimpio", DbType.Decimal, rowValorizacion.NEG_PrecioLimpio)
            db.AddInParameter(dbCommand, "@p_NEG_PrecioSucio", DbType.Decimal, rowValorizacion.NEG_PrecioSucio)
            db.AddInParameter(dbCommand, "@p_NEG_TIR", DbType.Decimal, rowValorizacion.NEG_TIR)
            db.AddInParameter(dbCommand, "@p_NEG_FechaFinCuponActual", DbType.Decimal, rowValorizacion.NEG_FechaFinCuponActual)
            db.AddInParameter(dbCommand, "@p_NEG_MontoCuponActual", DbType.Decimal, rowValorizacion.NEG_MontoCuponActual)
            db.AddInParameter(dbCommand, "@p_NEG_CantidadOperacion", DbType.Decimal, rowValorizacion.NEG_CantidadOperacion)
            db.AddInParameter(dbCommand, "@p_NEG_ValorNominal", DbType.Decimal, rowValorizacion.NEG_ValorNominal)
            db.AddInParameter(dbCommand, "@p_NEG_SaldoNominalVigente", DbType.Decimal, rowValorizacion.NEG_SaldoNominalVigente)
            db.AddInParameter(dbCommand, "@p_NEG_PagoCuponVigente", DbType.Decimal, rowValorizacion.NEG_PagoCuponVigente)

            If rowValorizacion.IsPRELIM_ValorActualNull = False Then
                db.AddInParameter(dbCommand, "@p_PRELIM_ValorActual", DbType.Decimal, rowValorizacion.PRELIM_ValorActual)
                db.AddInParameter(dbCommand, "@p_PRELIM_InteresCorrido", DbType.Decimal, rowValorizacion.PRELIM_InteresCorrido)
                db.AddInParameter(dbCommand, "@p_PRELIM_ValorPrincipal", DbType.Decimal, rowValorizacion.PRELIM_ValorPrincipal)
                db.AddInParameter(dbCommand, "@p_PRELIM_PrecioLimpio", DbType.Decimal, rowValorizacion.PRELIM_PrecioLimpio)
                db.AddInParameter(dbCommand, "@p_PRELIM_PrecioSucio", DbType.Decimal, rowValorizacion.PRELIM_PrecioSucio)
                db.AddInParameter(dbCommand, "@p_PRELIM_TIR", DbType.Decimal, rowValorizacion.PRELIM_TIR)
                db.AddInParameter(dbCommand, "@p_PRELIM_FechaFinCuponActual", DbType.Decimal, rowValorizacion.PRELIM_FechaFinCuponActual)
                db.AddInParameter(dbCommand, "@p_PRELIM_MontoCuponActual", DbType.Decimal, rowValorizacion.PRELIM_MontoCuponActual)
                db.AddInParameter(dbCommand, "@p_PRELIM_CantidadOperacion", DbType.Decimal, rowValorizacion.PRELIM_CantidadOperacion)
                db.AddInParameter(dbCommand, "@p_PRELIM_ValorNominal", DbType.Decimal, rowValorizacion.PRELIM_ValorNominal)
                db.AddInParameter(dbCommand, "@p_PRELIM_SaldoNominalVigente", DbType.Decimal, rowValorizacion.PRELIM_SaldoNominalVigente)
                db.AddInParameter(dbCommand, "@p_PRELIM_PagoCuponVigente", DbType.Decimal, rowValorizacion.PRELIM_PagoCuponVigente)
            End If
            
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

End Class
