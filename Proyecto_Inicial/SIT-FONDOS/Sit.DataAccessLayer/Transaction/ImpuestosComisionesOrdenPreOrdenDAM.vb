Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class ImpuestosComisionesOrdenPreOrdenDAM

    Private oImpuestosComisionesOrdenPreOrden As ImpuestosComisionesOrdenPreOrdenBE.ImpuestosComisionesOrdenPreOrdenRow
		Public Sub New()

		End Sub


    Public Function Insertar(ByVal objIC As ImpuestosComisionesOrdenPreOrdenBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisionesOrdenesPreordenes_Insertar")
        oImpuestosComisionesOrdenPreOrden = CType(objIC.ImpuestosComisionesOrdenPreOrden.Rows(0), ImpuestosComisionesOrdenPreOrdenBE.ImpuestosComisionesOrdenPreOrdenRow)

        db.AddInParameter(dbCommand, "@p_CodigoOrdenPreOrden", DbType.String, oImpuestosComisionesOrdenPreOrden.CodigoOrdenPreorden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oImpuestosComisionesOrdenPreOrden.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoTarifa", DbType.String, oImpuestosComisionesOrdenPreOrden.CodigoTarifa)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, oImpuestosComisionesOrdenPreOrden.CodigoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oImpuestosComisionesOrdenPreOrden.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_CodigoPlaza", DbType.String, oImpuestosComisionesOrdenPreOrden.CodigoPlaza)
        db.AddInParameter(dbCommand, "@p_ValorCalculado", DbType.Decimal, oImpuestosComisionesOrdenPreOrden.ValorCalculado)
        db.AddInParameter(dbCommand, "@p_ValorComision", DbType.Decimal, oImpuestosComisionesOrdenPreOrden.ValorReal)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function Modificar(ByVal objIC As ImpuestosComisionesOrdenPreOrdenBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisionesOrdenesPreordenes_Modificar")
        oImpuestosComisionesOrdenPreOrden = CType(objIC.ImpuestosComisionesOrdenPreOrden.Rows(0), ImpuestosComisionesOrdenPreOrdenBE.ImpuestosComisionesOrdenPreOrdenRow)

        db.AddInParameter(dbCommand, "@p_CodigoOrdenPreOrden", DbType.String, oImpuestosComisionesOrdenPreOrden.CodigoOrdenPreorden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oImpuestosComisionesOrdenPreOrden.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoTarifa", DbType.String, oImpuestosComisionesOrdenPreOrden.CodigoTarifa)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, oImpuestosComisionesOrdenPreOrden.CodigoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oImpuestosComisionesOrdenPreOrden.CodigoMercado)
        db.AddInParameter(dbCommand, "@p_ValorCalculado", DbType.Decimal, oImpuestosComisionesOrdenPreOrden.ValorCalculado)
        db.AddInParameter(dbCommand, "@p_ValorComision", DbType.Decimal, oImpuestosComisionesOrdenPreOrden.ValorReal)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function ListarPorCodigoOrden(ByVal CodigoOrden As String, ByVal codigoPortafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisionesOrdenPreOrden_ListarporCodigoOrden")
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, CodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function VerificarExistencia(ByVal CodigoOrden As String, ByVal codigoPortafolio As String, ByVal codigoComision As String, ByVal codigoRenta As String, ByVal codigoMercado As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim resul As Integer = 0
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisionesOrdenesPreordenes_VerificarExistencia")
        db.AddInParameter(dbCommand, "@p_CodigoOrdenPreorden", DbType.String, CodigoOrden)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoTarifa", DbType.String, codigoComision)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, codigoRenta)
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
        resul = CType(db.ExecuteScalar(dbCommand), Integer)
        If resul = 0 Then
            Return False
        ElseIf resul > 0 Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de ImpuestosComisionesOrdenPreOrden tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal cODIGO As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisionesOrdenPreOrden_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CODIGO", DbType.String, cODIGO)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de ImpuestosComisionesOrdenPreOrden tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoTarifa"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoTarifa(ByVal codigoTarifa As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisionesOrdenPreOrden_SeleccionarPorCodigoTarifa")

        db.AddInParameter(dbCommand, "@p_CodigoTarifa", DbType.String, codigoTarifa)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de ImpuestosComisionesOrdenPreOrden tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisionesOrdenPreOrden_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function


    ''' <summary>
    ''' Elimina un expediente de ImpuestosComisionesOrdenPreOrden table por una llave primaria compuesta.
    ''' <summary>
    'RGF 20090107 El procedure no hace nada
    'ini HDG INC 63038	20110427 se descomenta
    Public Function Eliminar(ByVal strCodigo As String, ByVal strPortafolio As String, ByVal strCodigoMercado As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisionesOrdenPreOrden_Eliminar")

        db.AddInParameter(dbCommand, "@p_PortafolioSBS", DbType.String, strPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoOrden", DbType.String, strCodigo)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, strCodigoMercado)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    'fin HDG INC 63038	20110427

    ''' <summary>
    ''' Elimina un expediente de ImpuestosComisionesOrdenPreOrden table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTarifa(ByVal codigoTarifa As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ImpuestosComisionesOrdenPreOrden_EliminarPorCodigoTarifa")

        db.AddInParameter(dbCommand, "@p_CodigoTarifa", DbType.String, codigoTarifa)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
End Class

