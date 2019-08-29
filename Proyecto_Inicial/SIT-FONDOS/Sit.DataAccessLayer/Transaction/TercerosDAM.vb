Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports System.Collections.Generic
Public Class TercerosDAM
    Private oTercerosRow As TercerosBE.TercerosRow
    Private strVacio As String = ""
    Private decVacio As String = 0
    Public Sub New()
    End Sub
    Public Sub Insertar(ByVal codigoTerceroSBS As Decimal, ByVal situacion As String, ByVal codigoTercero As String, ByVal descripcion As String,
    ByVal direccion As String, ByVal numeroDocumento As String, ByVal numeroCuenta As Decimal, ByVal codigoPostal As String, ByVal codigoPais As Decimal,
    ByVal codigoMoneda As String, ByVal codigoTipoDocumento As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal,
    ByVal codigoSectorEmpresarial As String, ByVal horaCreacion As String, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal,
    ByVal host As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Terceros_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoTerceroSBS", DbType.Decimal, codigoTerceroSBS)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
            db.AddInParameter(dbCommand, "@p_Direccion", DbType.String, direccion)
            db.AddInParameter(dbCommand, "@p_NumeroDocumento", DbType.String, numeroDocumento)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.Decimal, numeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, IIf(codigoPostal = strVacio, DBNull.Value, codigoPostal))
            db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.Decimal, codigoPais)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, codigoTipoDocumento)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
            db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
            db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
            db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
            db.AddInParameter(dbCommand, "@p_Rating", DbType.String, IIf(oTercerosRow.Rating.ToString.Equals(""), DBNull.Value, oTercerosRow.Rating))
            db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Terceros_SeleccionarPorCodigoMoneda")
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarBancoPorMercPortMone(ByVal sCodigoMercado As String, ByVal sCodigoPortafolioSBS As String, ByVal sCodigoMoneda As String) As TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TerceroBanco_SeleccionPorMercPortMone")
            db.AddInParameter(dbCommand, "@nvcCodigoMercado", DbType.String, sCodigoMercado)
            db.AddInParameter(dbCommand, "@nvcCodigoPortafolio", DbType.String, sCodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@nvcCodigoMoneda", DbType.String, sCodigoMoneda)
            Using objeto As New TercerosBE
                db.LoadDataSet(dbCommand, objeto, "Terceros")
                Return objeto
            End Using
        End Using
    End Function
    Public Function SeleccionarContactoPorBancoPorMercPortMone(ByVal sCodigoMercado As String, ByVal sCodigoPortafolioSBS As String, ByVal sCodigoMoneda As String,
    ByVal sCodigoBanco As String) As TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("TerceroBanco_SeleccionContactoPorMercPortMone")
        db.AddInParameter(dbCommand, "@nvcCodigoMercado", DbType.String, sCodigoMercado)
        db.AddInParameter(dbCommand, "@nvcCodigoPortafolio", DbType.String, sCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@nvcCodigoMoneda", DbType.String, sCodigoMoneda)
        db.AddInParameter(dbCommand, "@nvcCodigoBanco", DbType.String, sCodigoBanco)
        Dim objeto As New TercerosBE
        db.LoadDataSet(dbCommand, objeto, "Terceros")
        Return objeto
    End Function
    Public Function SeleccionarBancoPorCodigoMercado(ByVal codigoMercado As String) As TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("TerceroBanco_SeleccionarPorMercado")
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        Dim objeto As New TercerosBE
        db.LoadDataSet(dbCommand, objeto, "Terceros")
        Return objeto
    End Function
    Public Function SeleccionarBancoPorCodigoMercadoYPortafolio(ByVal codigoMercado As String, ByVal codigoPortafolioSBS As String) As TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("TerceroBanco_SeleccionarPorMercadoYPortafolio")
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        Dim objeto As New TercerosBE
        db.LoadDataSet(dbCommand, objeto, "Terceros")
        Return objeto
    End Function
    Public Function SeleccionarPorCodigoMercado(ByVal codigoMercado As String) As TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_SeleccionarPorMercado")
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        Dim objeto As New TercerosBE
        db.LoadDataSet(dbCommand, objeto, "Terceros")
        Return objeto
    End Function
    Public Function SeleccionarPorFiltroMercado(ByVal clasificacionTercero As String, ByVal tipoTercero As String, ByVal sCodigoPais As String) As TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_SeleccionarPorFiltroMercado")
        db.AddInParameter(dbCommand, "@p_ClasificacionTercero", DbType.String, clasificacionTercero)
        db.AddInParameter(dbCommand, "@p_TipoTercero", DbType.String, tipoTercero)
        db.AddInParameter(dbCommand, "@CodigoPais", DbType.String, sCodigoPais)
        Dim objeto As New TercerosBE
        db.LoadDataSet(dbCommand, objeto, "Terceros")
        Return objeto
    End Function
    Public Function SeleccionarPorFiltro(ByVal clasificacionTercero As String, ByVal tipoTercero As String) As TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_SeleccionarPorFiltro2")
        db.AddInParameter(dbCommand, "@p_ClasificacionTercero", DbType.String, clasificacionTercero)
        db.AddInParameter(dbCommand, "@p_TipoTercero", DbType.String, tipoTercero)
        Dim objeto As New TercerosBE
        db.LoadDataSet(dbCommand, objeto, "Terceros")
        Return objeto
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoTercero As String, ByVal clasificacionTercero As String, ByVal descripcion As String,
    ByVal codigoTipoDocumento As String, ByVal codigoSectorEmpresarial As String, ByVal situacion As String) As TercerosBE
        Dim oTercerosBE As New TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, codigoTipoDocumento)
        db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)
        db.AddInParameter(dbCommand, "@p_ClasificacionTercero", DbType.String, clasificacionTercero)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.LoadDataSet(dbCommand, oTercerosBE, "Terceros")
        Return oTercerosBE
    End Function
    Public Function SeleccionarPorFiltroActivo(ByVal codigoTercero As String, ByVal clasificacionTercero As String, ByVal descripcion As String,
    ByVal codigoTipoDocumento As String, ByVal codigoSectorEmpresarial As String, ByVal situacion As String) As TercerosBE
        Dim oTercerosBE As New TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_SeleccionarPorFiltro")
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, codigoTipoDocumento)
        db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)
        db.AddInParameter(dbCommand, "@p_ClasificacionTercero", DbType.String, clasificacionTercero)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, "A")
        db.LoadDataSet(dbCommand, oTercerosBE, "Terceros")
        Return oTercerosBE
    End Function
    Public Function Seleccionar(ByVal codigoTercero As String) As TercerosBE
        Dim oTerceroBE As New TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_Seleccionar")
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            db.LoadDataSet(dbCommand, oTerceroBE, "Terceros")
            Return oTerceroBE
        End Using
    End Function
    Public Function SeleccionarPorCodigoSectorEmpresarial(ByVal codigoSectorEmpresarial As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Terceros_SeleccionarPorCodigoSectorEmpresarial")
        db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, codigoSectorEmpresarial)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarBanco(ByVal codigoTercero As String, ByVal CodigoMoneda As String) As TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("TerceroBanco_SeleccionarPorTercero")
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
        Dim objeto As New TercerosBE
        db.LoadDataSet(dbCommand, objeto, "Terceros")
        Return objeto
    End Function
    Public Function SeleccionarPorCodigoPais(ByVal codigoPais As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Terceros_SeleccionarPorCodigoPais")
        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.Decimal, codigoPais)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarPorCodigoTipoDocumento(ByVal codigoTipoDocumento As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Terceros_SeleccionarPorCodigoTipoDocumento")
        db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, codigoTipoDocumento)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarPorCodigoPostal(ByVal codigoPostal As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Terceros_SeleccionarPorCodigoPostal")
        db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, codigoPostal)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Terceros_Listar")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Sub Insertar(ByVal oTercerosBE As TercerosBE, ByVal oCuentaTerceros As CuentaTercerosBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_Insertar")
            oTercerosRow = DirectCast(oTercerosBE.Terceros.Rows(0), TercerosBE.TercerosRow)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTercerosRow.Situacion)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oTercerosRow.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oTercerosRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_Direccion", DbType.String, oTercerosRow.Direccion)
            db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, IIf(oTercerosRow.CodigoPostal = strVacio, DBNull.Value, oTercerosRow.CodigoPostal))
            db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.String, oTercerosRow.CodigoPais)
            db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, oTercerosRow.CodigoTipoDocumento)
            db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, oTercerosRow.CodigoSectorEmpresarial)
            db.AddInParameter(dbCommand, "@p_Beneficiario", DbType.String, oTercerosRow.Beneficiario)
            db.AddInParameter(dbCommand, "@p_TipoTercero", DbType.String, oTercerosRow.TipoTercero)
            db.AddInParameter(dbCommand, "@p_ClasificacionTercero", DbType.String, oTercerosRow.ClasificacionTercero)
            db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, oTercerosRow.CodigoCustodio)
            db.AddInParameter(dbCommand, "@p_TerceroBanco", DbType.String, oTercerosRow.TerceroBanco)
            db.AddInParameter(dbCommand, "@p_CodigoDocumento", DbType.String, oTercerosRow.CodigoDocumento)
            db.AddInParameter(dbCommand, "@p_Rating", DbType.String, IIf(oTercerosRow.Rating.ToString.Equals(""), DBNull.Value, oTercerosRow.Rating))
            db.AddInParameter(dbCommand, "@p_CodigoEmision", DbType.String, IIf(oTercerosRow.CodigoEmision.ToString.Equals(""), DBNull.Value, oTercerosRow.CodigoEmision))
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

            db.AddInParameter(dbCommand, "@p_CodigoCalificionOficial", DbType.String, oTercerosRow.Rating)
            db.AddInParameter(dbCommand, "@p_RatingInterno", DbType.String, oTercerosRow.RatingInterno)
            db.AddInParameter(dbCommand, "@p_CodigoRatingFF", DbType.String, oTercerosRow.RatingFF)
            db.AddInParameter(dbCommand, "@p_SectorGIGS", DbType.String, oTercerosRow.SectorGIGS)
            db.ExecuteNonQuery(dbCommand)
            EliminarDet(oTercerosRow.CodigoTercero)
            InsertarDet(oCuentaTerceros, dataRequest)
        End Using
    End Sub
    Public Sub InsertarDet(ByVal oCuentaTerceros As CuentaTercerosBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oCuentaTercerosRow As CuentaTercerosBE.CuentaTercerosRow
        Dim intNroFilas, intIndice As Integer
        intNroFilas = oCuentaTerceros.CuentaTerceros.Rows.Count
        For intIndice = 0 To intNroFilas - 1
            oCuentaTercerosRow = DirectCast(oCuentaTerceros.CuentaTerceros.Rows(intIndice), CuentaTercerosBE.CuentaTercerosRow)
            Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaTerceros_Insertar")
                db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, oCuentaTercerosRow.NumeroCuenta)
                db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oCuentaTercerosRow.CodigoMoneda)
                db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, IIf(oCuentaTercerosRow.CodigoClaseCuenta = strVacio, DBNull.Value, DBNull.Value))
                db.AddInParameter(dbCommand, "@p_CuentaInterBancario", DbType.String, oCuentaTercerosRow.CuentaInterBancario)
                db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oCuentaTercerosRow.CodigoMercado)
                db.AddInParameter(dbCommand, "@p_EntidadFinanciera ", DbType.String, oCuentaTercerosRow.EntidadFinanciera)
                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, IIf(oCuentaTercerosRow.Situacion = "Activo", "A", "I"))
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oCuentaTercerosRow.CodigoPortafolioSBS)
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oCuentaTercerosRow.CodigoTercero)
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.AddInParameter(dbCommand, "@p_LiquidacionAutomatica", DbType.String, oCuentaTercerosRow.LiquidacionAutomatica)
                db.ExecuteNonQuery(dbCommand)
            End Using
        Next
    End Sub
    Public Sub EliminarDet(ByVal codigoTercero As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaTerceros_EliminarDet")
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub ModificarDet(ByVal oCuentaTerceros As CuentaTercerosBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oCuentaTercerosRow As CuentaTercerosBE.CuentaTercerosRow
        Dim intNroFilas, intIndice As Integer
        intNroFilas = oCuentaTerceros.CuentaTerceros.Rows.Count
        For intIndice = 0 To intNroFilas - 1
            oCuentaTercerosRow = DirectCast(oCuentaTerceros.CuentaTerceros.Rows(intIndice), CuentaTercerosBE.CuentaTercerosRow)
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuentaTerceros_Modificar1")
            db.AddInParameter(dbCommand, "@p_Secuencial", DbType.Int32, oCuentaTercerosRow.Secuencial)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, oCuentaTercerosRow.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oCuentaTercerosRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CuentaInterBancario", DbType.String, oCuentaTercerosRow.CuentaInterBancario)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oCuentaTercerosRow.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_EntidadFinanciera ", DbType.String, oCuentaTercerosRow.EntidadFinanciera)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, IIf(oCuentaTercerosRow.Situacion = "Activo", "A", "I"))
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oCuentaTercerosRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oCuentaTercerosRow.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        Next
    End Sub
    Public Function Modificar(ByVal oTercerosBE As TercerosBE, ByVal oCuentaTerceros As CuentaTercerosBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_Modificar")
            oTercerosRow = DirectCast(oTercerosBE.Terceros.Rows(0), TercerosBE.TercerosRow)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTercerosRow.Situacion)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oTercerosRow.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oTercerosRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_Direccion", DbType.String, oTercerosRow.Direccion)
            db.AddInParameter(dbCommand, "@p_CodigoPostal", DbType.String, IIf(oTercerosRow.CodigoPostal = strVacio, DBNull.Value, oTercerosRow.CodigoPostal))
            db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.String, oTercerosRow.CodigoPais)
            db.AddInParameter(dbCommand, "@p_CodigoTipoDocumento", DbType.String, oTercerosRow.CodigoTipoDocumento)
            db.AddInParameter(dbCommand, "@p_CodigoSectorEmpresarial", DbType.String, oTercerosRow.CodigoSectorEmpresarial)
            db.AddInParameter(dbCommand, "@p_Beneficiario", DbType.String, oTercerosRow.Beneficiario)
            db.AddInParameter(dbCommand, "@p_TipoTercero", DbType.String, oTercerosRow.TipoTercero)
            db.AddInParameter(dbCommand, "@p_ClasificacionTercero", DbType.String, oTercerosRow.ClasificacionTercero)
            db.AddInParameter(dbCommand, "@p_CodigoCustodio", DbType.String, oTercerosRow.CodigoCustodio)
            db.AddInParameter(dbCommand, "@p_TerceroBanco", DbType.String, oTercerosRow.TerceroBanco)
            db.AddInParameter(dbCommand, "@p_Rating", DbType.String, oTercerosRow.Rating)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_CodigoEmision", DbType.String, IIf(oTercerosRow.CodigoEmision.ToString.Equals(""), DBNull.Value, oTercerosRow.CodigoEmision))
            db.AddInParameter(dbCommand, "@p_CodigoDocumento", DbType.String, oTercerosRow.CodigoDocumento)

            db.AddInParameter(dbCommand, "@p_CodigoCalificionOficial", DbType.String, oTercerosRow.Rating)
            db.AddInParameter(dbCommand, "@p_RatingInterno", DbType.String, oTercerosRow.RatingInterno)
            db.AddInParameter(dbCommand, "@p_CodigoRatingFF", DbType.String, oTercerosRow.RatingFF)
            db.AddInParameter(dbCommand, "@p_SectorGIGS", DbType.String, oTercerosRow.SectorGIGS)
            db.ExecuteNonQuery(dbCommand)
            EliminarDet(oTercerosRow.CodigoTercero)
            InsertarDet(oCuentaTerceros, dataRequest)
            Return True
        End Using
    End Function
    Public Function Eliminar(ByVal codigoTercero As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As TercerosBE
        Dim objeto As New TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_Listar")
        db.LoadDataSet(dbCommand, objeto, "Terceros")
        Return objeto
    End Function
    Public Function ListarTerceroPorGrupoIntermediario(ByVal Clasificacion As String, ByVal strFiltro As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_ListarPorGrupoIntermediario")
        db.AddInParameter(dbCommand, "@clasificacion", DbType.String, Clasificacion)
        db.AddInParameter(dbCommand, "@filtro", DbType.String, strFiltro)
        Return db.ExecuteDataSet(dbCommand)
    End Function
#Region "Funciones Alberto"
    Public Function ListarTerceroPorClasificacion(ByVal Clasificacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_ListarPorClasificacion")
        db.AddInParameter(dbCommand, "@p_clasificacion", DbType.String, Clasificacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarTerceroPorClasificacionSoloBancos(ByVal Clasificacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_ListarPorClasificacionSoloBancos")
        db.AddInParameter(dbCommand, "@p_clasificacion", DbType.String, Clasificacion)
        Return db.ExecuteDataSet(dbCommand)
    End Function
#End Region
    Public Function ListarTerceroPorClasificacionCustodio(ByVal Clasificacion As String, ByVal custodio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_ListarPorClasificacionCustodio")
        db.AddInParameter(dbCommand, "@p_clasificacion", DbType.String, Clasificacion)
        db.AddInParameter(dbCommand, "@p_custodio", DbType.String, custodio)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarTerceroPorClasificacionCustodioXGrupoInter(ByVal Clasificacion As String, ByVal custodio As String, ByVal strFiltro As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_ListarPorClasificacionCustodioXGrupoInter")
        db.AddInParameter(dbCommand, "@p_clasificacion", DbType.String, Clasificacion)
        db.AddInParameter(dbCommand, "@p_custodio", DbType.String, custodio)
        db.AddInParameter(dbCommand, "@p_filtro", DbType.String, strFiltro)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarTerceroPorClasificacionValores(ByVal Clasificacion As String, ByVal codigoNemonico As String, ByVal codigoPortafolioSBS As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_ListarPorClasificacionValor")
        db.AddInParameter(dbCommand, "@p_clasificacion", DbType.String, Clasificacion)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, codigoNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function Tercero_ListarEntidadFinanciera(ByVal dataRequest As DataSet) As TercerosBE
        'Funcion para listar los terceros que son entidades 
        Dim objeto As New TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_ListarEntidadFinanciera")
        db.LoadDataSet(dbCommand, objeto, "Terceros")
        Return objeto
    End Function
    Public Function Tercero_SeleccionarEntidadFinanciera(ByVal strCodigo As String, ByVal dataRequest As DataSet) As DataSet
        Dim objeto As New TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_SeleccionarEntidadFinanciera")
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, strCodigo)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function Tercero_ObtenerSaldoCustodio(ByVal strCodigoTercero As String, ByVal strCodigoPortafolioSBS As String, ByVal strCodigoMnemonico As String,
    ByVal decFecha As Decimal) As DataSet
        Dim objeto As New TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_CustodioSaldo")
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, strCodigoTercero)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, strCodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, strCodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, decFecha)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ListarTerceroReporte() As DataTable
        Dim objeto As New TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Reporte_Tercero")
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function GrupoIntermediario_SeleccionarPorTercero(ByVal CodigoTercero As String) As String
        Dim objeto As New TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_GrupoIntermediario_SeleccionarPorTercero")
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
        Return Convert.ToString(db.ExecuteScalar(dbCommand))
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Listar intermediarios por descripcion
    Public Function ListarIntermediarios(ByVal Descripcion As String) As TercerosEList
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oReader As IDataReader
        Dim lValorList As New TercerosEList
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_lis_Intermediarios")
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, Descripcion)
        oReader = db.ExecuteReader(dbCommand)
        While oReader.Read()
            Dim lValor As New TercerosE
            lValor.Descripcion = oReader.GetString(0)
            lValor.CodigoTercero = oReader.GetString(1)
            lValorList.Add(lValor)
        End While
        oReader.Close()
        Return lValorList
    End Function

    Public Function ListarTercerPorTipoEntidad(ByVal codigoTipoEntidad As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_ListarPorTipoEntidad")
            db.AddInParameter(dbCommand, "@p_CodigoTipoEntidad", DbType.String, codigoTipoEntidad)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Sub InsertarFondoCliente(ByVal objListFondo As ListFondoCliente, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        For Each objFondoCliente As Terceros_FondoClienteBE In objListFondo.objListFondoCliente
            Using dbCommand As DbCommand = db.GetStoredProcCommand("InsertarFondoClientes")
                db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, objFondoCliente.CodigoTercero)
                db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, objFondoCliente.CodigoPortafolio)
                db.AddInParameter(dbCommand, "@p_CodigoTerceroCliente", DbType.String, objFondoCliente.CodigoTerceroCliente)
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.ExecuteNonQuery(dbCommand)
            End Using
        Next
    End Sub

    Public Sub EliminarFondoCliente(ByVal p_CodigoTercero As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("EliminarFondoClientes")
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, p_CodigoTercero)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

    Public Function SeleccionarTercero_FondoCliente(ByVal objTerceroFC As Terceros_FondoClienteBE) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SeleccionarTercero_FondoClientes")
            db.AddInParameter(dbCommand, "@p_Id", DbType.Int32, objTerceroFC.Id)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, objTerceroFC.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, objTerceroFC.CodigoPortafolio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function SeleccionarRatingTercero_Historia(ByVal p_CodigoTercero As String, ByVal p_CodigoEntidad As String, ByVal p_Fecha As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SeleccionarRatingTercero_Historia")
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, p_CodigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, p_CodigoEntidad)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Int32, p_Fecha)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function ListarClientesMandato(ByVal p_Descripcion As String, ByVal p_Situacion As String, ByVal p_CodigoTercero As String) As TercerosBE
        Dim oTercerosBE As New TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_ListarClienteMandato")
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, p_Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, p_Situacion)
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, p_CodigoTercero)
        db.LoadDataSet(dbCommand, oTercerosBE, "Terceros")
        Return oTercerosBE
    End Function

    Public Function ListarMandatos() As TercerosBE
        Dim objeto As New TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Mandatos_Listar")
        db.LoadDataSet(dbCommand, objeto, "Terceros")
        Return objeto
    End Function

    Public Function ListarBancosMandatos() As TercerosBE
        Dim objeto As New TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Tercero_Financiero")
        db.LoadDataSet(dbCommand, objeto, "Terceros")
        Return objeto
    End Function
End Class