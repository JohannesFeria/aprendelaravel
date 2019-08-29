Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports System.Data.Common
Imports Sit.BusinessEntities

Public Class PagoFechaComisionDAM
    Public Function Seleccionar(ByVal id As Int32, ByVal codigoFondo As String, ByVal NumeroCuenta As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = db.GetStoredProcCommand("PagoFechaComision_Seleccionar")
            db.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, codigoFondo)
            db.AddInParameter(dbcomand, "@p_NumeroCuenta", DbType.String, NumeroCuenta)
            db.AddInParameter(dbcomand, "@p_Id", DbType.Int32, id)
            Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ListarPortafoliosCustodio(ByVal codigoFondo As String, ByVal fecha As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = db.GetStoredProcCommand("PagoFechaComision_ListarFondosCustodio")
            db.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, codigoFondo)
            db.AddInParameter(dbcomand, "@p_FechaCorte", DbType.Decimal, fecha)
            Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ValidarExistenciaIngresados(ByVal codigoFondo As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = db.GetStoredProcCommand("PagoFechaComision_ValidarExistenciaIngresados")
            db.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, codigoFondo)
            Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ListarPortafolios(ByVal fecha As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = db.GetStoredProcCommand("PagoFechaComision_ListarPortafolios")
            db.AddInParameter(dbcomand, "@p_FechaCorte", DbType.Decimal, fecha)
            Using ds As DataSet = db.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function ListarBancos(ByVal codigoFondo As String, ByVal codigoMoneda As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PagoFechaComision_ListarBancos")
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoFondo)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using

    End Function
    Public Function Eliminar(ByVal identificador As Integer) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PagoFechaComision_Eliminar")
            db.AddInParameter(dbCommand, "@p_Id", DbType.Int32, identificador)
            db.ExecuteDataSet(dbCommand)
            Return True
        End Using

    End Function


    Public Function Insertar(ByVal pagoFechaComisionBE As PagoFechaComisionBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PagoFechaComision_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, pagoFechaComisionBE.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, pagoFechaComisionBE.Estado)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, pagoFechaComisionBE.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoBanco", DbType.String, pagoFechaComisionBE.CodigoBanco)
            db.AddInParameter(dbCommand, "@p_UsuarioSolicitud", DbType.String, pagoFechaComisionBE.UsuarioSolicitud)
            db.AddInParameter(dbCommand, "@p_FechaSolicitud", DbType.Decimal, pagoFechaComisionBE.FechaSolicitud)
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, pagoFechaComisionBE.FechaInicio)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, pagoFechaComisionBE.FechaFin)
            db.AddInParameter(dbCommand, "@p_ComisionAcumulada", DbType.Decimal, pagoFechaComisionBE.ComisionAcumulada)
            db.AddInParameter(dbCommand, "@p_SaldoDisponible", DbType.Decimal, pagoFechaComisionBE.SaldoDisponible)

            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.ExecuteNonQuery(dbCommand)


        End Using

    End Function
    Public Function Actualizar(ByVal pagoFechaComisionBE As PagoFechaComisionBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PagoFechaComision_Actualizar")
            db.AddInParameter(dbCommand, "@p_Id", DbType.Int32, pagoFechaComisionBE.Id)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, pagoFechaComisionBE.Estado)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, pagoFechaComisionBE.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoBanco", DbType.String, pagoFechaComisionBE.CodigoBanco)
            db.AddInParameter(dbCommand, "@p_NumeroCuentaAdministradora", DbType.String, pagoFechaComisionBE.NumeroCuentaAdministradora)
            db.AddInParameter(dbCommand, "@p_CodigoBancoAdministradora", DbType.String, pagoFechaComisionBE.CodigoBancoAdministradora)
            db.AddInParameter(dbCommand, "@p_ComisionAcumulada", DbType.Decimal, pagoFechaComisionBE.ComisionAcumulada)
            db.AddInParameter(dbCommand, "@p_SaldoDisponible", DbType.Decimal, pagoFechaComisionBE.SaldoDisponible)
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_FechaConfirmacion", DbType.Decimal, pagoFechaComisionBE.FechaConfirmacion)
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    Public Function ActualizarEstado(ByVal pagoFechaComisionBE As PagoFechaComisionBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PagoFechaComision_ActualizarEstado")
            db.AddInParameter(dbCommand, "@p_Id", DbType.Int32, pagoFechaComisionBE.Id)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, pagoFechaComisionBE.Estado)
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function

    Public Function ListarNumeroDeCuentas(ByVal codigoFondo As String, ByVal codigoMoneda As String, ByVal codigoBanco As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PagoFechaComision_ListarNumeroDeCuentas")
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoFondo)
            db.AddInParameter(dbCommand, "@p_CodigoBanco", DbType.String, codigoBanco)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ObtenerFechaComision(ByVal fechaOperacion As Decimal, ByVal codigoFondo As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PagoFechaComision_ObtenerFechaComision")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoFondo)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using

    End Function

    Public Function EliminarConfirmado(ByVal id As Int32, ByVal codigoFondo As String, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("PagoFechaComision_EliminarConfirmado")
        Dim strResultado As String

        db.AddInParameter(dbCommand, "@p_IdPagoFechaComision", DbType.Int32, id)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoFondo)

        db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        strResultado = CType(db.ExecuteScalar(dbCommand), String)
        Return strResultado
    End Function
End Class
