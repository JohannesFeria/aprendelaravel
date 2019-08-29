Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class InterfaseBloombergDAM
    Private sqlCommand As String = ""
    Protected TablaErrores As New DataTable

    Public Sub New()

    End Sub

#Region " /* Funciones Insertar */ "


    Public Function InsertarPreciosBloomberg(ByVal Fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String
        Dim CodigoCaracteristicas As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        'Dim cont As Integer  'HDG INC 59941	20100526
        Dim CodigoSBS, CodigoISIN, CodigoMnemonico, Emisor, valor As String
        Dim precio As Decimal

        'cont = 0  'HDG INC 59941	20100526

        For Each filaLinea As DataRow In dtDetalle.Rows
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("VectorPrecioBloomberg_InsertarPreciosBloomberg")
            'If cont <> 0 Then  'HDG INC 59941	20100526
            CodigoSBS = filaLinea(0).ToString().Trim()
            CodigoISIN = filaLinea(1).ToString().Trim()
            Emisor = filaLinea(2).ToString().Trim()
            'HDG INC 59941	20100526
            'CodigoMnemonico = filaLinea(3).ToString().Trim()
            CodigoMnemonico = SeleccionarCodigoMnemonico(CodigoISIN)
            valor = filaLinea(4).ToString().Trim()
            If Not CodigoMnemonico Is Nothing Then  'HDG INC 59941	20100526
                If (valor <> "") Then
                    precio = Convert.ToDecimal(valor.Replace(".", Separador))
                    db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(Fecha))
                    db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, CodigoSBS)
                    db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, CodigoISIN)
                    db.AddInParameter(dbCommand, "@p_Emisor", DbType.String, Emisor)
                    db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
                    db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, precio)
                    db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                    db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                    db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                    db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

                    db.ExecuteNonQuery(dbCommand)
                End If
            End If

            'End If  'HDG INC 59941	20100526
            'cont += 1  'HDG INC 59941	20100526
        Next

        Return CodigoCaracteristicas

    End Function
    Public Function Separador() As String
        Return System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator
    End Function
    Public Sub InicializarTabla()

        Me.TablaErrores = New DataTable
        Me.TablaErrores.Columns.Add("CodigoISIN", GetType(String))

        Me.TablaErrores.GetChanges()

    End Sub
    Public Function InsertarPreciosReales(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        'Dim cont As Integer  'HDG INC 59941	20100526
        Dim CodigoSBS, CodigoISIN, CodigoMnemonico, Emisor, valor As String
        Dim precio As Decimal

        'cont = 0  'HDG INC 59941	20100526
        Me.TablaErrores = New DataTable
        InicializarTabla()
        For Each filaLinea As DataRow In dtDetalle.Rows
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("VectorPrecio_InsertarPrecios")
            'If cont <> 0 Then  'HDG INC 59941	20100526
            CodigoSBS = filaLinea(0).ToString().Trim()
            CodigoISIN = filaLinea(1).ToString().Trim()
            Emisor = filaLinea(2).ToString().Trim()
            CodigoMnemonico = SeleccionarCodigoMnemonico(CodigoISIN)
            valor = filaLinea(4).ToString().Trim()
            If CodigoMnemonico Is Nothing And CodigoISIN <> "ND" Then CodigoMnemonico = "" 'HDG INC 59941	20100526

            If Not CodigoMnemonico Is Nothing Then  'HDG INC 59941	20100526
                If (valor <> "") Then

                    If CodigoMnemonico = "" Then
                        Dim fila As DataRow

                        fila = Me.TablaErrores.NewRow()
                        fila("CodigoISIN") = CodigoISIN
                        TablaErrores.Rows.Add(fila)

                    Else  'HDG INC 59941	20100526
                        precio = Convert.ToDecimal(valor.Replace(".", Separador))
                        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(fecha))
                        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
                        db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, precio)
                        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

                        db.ExecuteNonQuery(dbCommand)
                    End If
                End If
            End If

            'End If  'HDG INC 59941	20100526
            'cont += 1  'HDG INC 59941	20100526

        Next

        Return TablaErrores

    End Function
    Public Function InsertarIndInversiones(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String
        Dim CodigoCaracteristicas As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cont As Integer
        Dim CodigoSBS, CodigoISIN, CodigoMnemonico, Emisor, I1, I2, I3 As String
        Dim Ind1, Ind2, Ind3 As Decimal



        For Each filaLinea As DataRow In dtDetalle.Rows
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("IndicadorBloomberg_InsertarIndicadorInversiones")

            CodigoSBS = filaLinea(0).ToString().Trim()
            CodigoISIN = filaLinea(1).ToString().Trim()
            CodigoMnemonico = filaLinea(2).ToString().Trim()
            I1 = filaLinea(3).ToString().Trim()
            I2 = filaLinea(4).ToString().Trim()
            I3 = filaLinea(5).ToString().Trim()

            If ((I1 <> "") And (I2 <> "") And (I3 <> "")) Then
                Ind1 = Convert.ToDecimal(I1.Replace(".", Separador))
                Ind2 = Convert.ToDecimal(I2.Replace(".", Separador))
                Ind3 = Convert.ToDecimal(I3.Replace(".", Separador))

                db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(fecha))
                db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, CodigoSBS)
                db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, CodigoISIN)
                db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
                db.AddInParameter(dbCommand, "@p_Indicador1", DbType.Decimal, Ind1)
                db.AddInParameter(dbCommand, "@p_Indicador2", DbType.Decimal, Ind2)
                db.AddInParameter(dbCommand, "@p_Indicador3", DbType.Decimal, Ind3)
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))


                db.ExecuteNonQuery(dbCommand)
            End If



        Next

        Return CodigoCaracteristicas



    End Function

    Public Function InsertarIndBloomberg(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String
        Dim CodigoCaracteristicas As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cont As Integer
        Dim CodigoIndicador, CodigoMnemonico, I1 As String
        Dim Ind1 As Decimal



        For Each filaLinea As DataRow In dtDetalle.Rows
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("IndicadorBloomberg_InsertarIndicadorBloomberg")

            CodigoIndicador = filaLinea(0).ToString().Trim()
            CodigoMnemonico = filaLinea(1).ToString().Trim()
            I1 = filaLinea(2).ToString().Trim()


            If ((I1 <> "")) Then
                Ind1 = Convert.ToDecimal(I1.Replace(".", Separador))

                db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(fecha))
                db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
                db.AddInParameter(dbCommand, "@p_CodigoIndicador", DbType.String, CodigoIndicador)
                db.AddInParameter(dbCommand, "@p_Indicador1", DbType.Decimal, Ind1)
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))


                db.ExecuteNonQuery(dbCommand)
            End If



        Next

        Return CodigoCaracteristicas



    End Function

    Public Function InsertarIndDivLib(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String
        Dim CodigoCaracteristicas As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cont As Integer
        Dim CodigoSBS, Descripcion, CodigoMnemonico, Moneda, Factor, Tipo As String
        Dim FechaA, FechaC, FechaE, FechaR As Date
        Dim DecFactor As Decimal

        cont = 0
        For Each filaLinea As DataRow In dtDetalle.Rows
            'Dim dbCommand As dbCommand = db.GetStoredProcCommand("IndicadorBloomberg_InsertarIndicadorDivLib")
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("IndicadorBloomberg_InsertarIndicadorDivLib")
            If cont <> 0 Then
                CodigoSBS = filaLinea(0).ToString().Trim()
                CodigoMnemonico = filaLinea(1).ToString().Trim()
                Descripcion = filaLinea(2).ToString().Trim()
                FechaA = filaLinea(3)
                FechaC = filaLinea(4)
                FechaR = filaLinea(5)
                FechaE = filaLinea(6)
                Moneda = filaLinea(7).ToString().Trim()
                Factor = filaLinea(8).ToString().Trim()
                Tipo = filaLinea(9).ToString().Trim()


                If (Factor <> "") Then

                    DecFactor = Convert.ToDecimal(Factor.Replace(".", Separador))

                    db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(fecha))
                    db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, CodigoSBS)

                    db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
                    db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, Descripcion)

                    db.AddInParameter(dbCommand, "@p_FechaAcuerdo", DbType.Decimal, DataUtility.ObtenerFecha(FechaA))

                    db.AddInParameter(dbCommand, "@p_FechaCorte", DbType.Decimal, DataUtility.ObtenerFecha(FechaC))

                    db.AddInParameter(dbCommand, "@p_FechaRegistro", DbType.Decimal, DataUtility.ObtenerFecha(FechaR))
                    db.AddInParameter(dbCommand, "@p_FechaEntrega", DbType.Decimal, DataUtility.ObtenerFecha(FechaE))
                    db.AddInParameter(dbCommand, "@p_Moneda", DbType.String, Moneda)
                    db.AddInParameter(dbCommand, "@p_Factor", DbType.Decimal, DecFactor)
                    db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, Tipo)

                    db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                    db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                    db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                    db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))


                    db.ExecuteNonQuery(dbCommand)
                End If
            End If

            cont += 1


        Next

        Return CodigoCaracteristicas



    End Function

    Public Function InsertarTipoCambioBloomberg(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String
        Dim CodigoCaracteristicas As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cont As Integer
        Dim Sinonimo, Descripcion, Primario, Secundario As String
        Dim TCPrimario, TCSecundaria As Decimal

        cont = 0

        For Each filaLinea As DataRow In dtDetalle.Rows
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("VectorTipoCambioBloomberg_InsertarTipoCambioBloomberg")

            Sinonimo = filaLinea(0).ToString().Trim()
            Descripcion = filaLinea(1).ToString().Trim()
            Primario = filaLinea(2).ToString().Trim()
            Secundario = filaLinea(3).ToString().Trim()

            If (Primario <> "") Then
                TCPrimario = Convert.ToDecimal(Primario.Replace(".", Separador))
                TCSecundaria = Convert.ToDecimal(Secundario.Replace(".", Separador))
                db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(fecha))
                db.AddInParameter(dbCommand, "@p_SinonimoMoneda", DbType.String, Sinonimo)
                db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, Descripcion)
                db.AddInParameter(dbCommand, "@p_TipoCambioPrimario", DbType.Decimal, TCPrimario)
                db.AddInParameter(dbCommand, "@p_TipoCambioSecundaria", DbType.Decimal, TCSecundaria)
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))


                db.ExecuteNonQuery(dbCommand)
            End If




        Next

        Return CodigoCaracteristicas


    End Function
    Public Function InsertarTipoCambioReales(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As DataTable
        Dim TablaErrores As New DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim cont As Integer
        Dim Sinonimo, Primario, Secundario As String
        Dim TCPrimario, TCSecundaria As Decimal

        cont = 0

        For Each filaLinea As DataRow In dtDetalle.Rows
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("VectorTipoCambio_InsertarTipoCambio")

            Sinonimo = filaLinea(0).ToString().Trim()
            Primario = filaLinea(2).ToString().Trim()
            Secundario = filaLinea(3).ToString().Trim()

            If (Primario <> "") Then
                TCPrimario = Convert.ToDecimal(Primario.Replace(".", Separador))
                TCSecundaria = Convert.ToDecimal(Secundario.Replace(".", Separador))

                db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(fecha))
                db.AddInParameter(dbCommand, "@p_SinonimoMoneda", DbType.String, Sinonimo)
                db.AddInParameter(dbCommand, "@p_TipoCambioPrimario", DbType.Decimal, TCPrimario)
                db.AddInParameter(dbCommand, "@p_TipoCambioSecundaria", DbType.Decimal, TCSecundaria)
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))


                db.ExecuteNonQuery(dbCommand)
            End If




        Next


        Return TablaErrores


    End Function

    Public Function SeleccionarCodigoMnemonico(ByVal ISIN As String) As String
        Dim CodigoMnemonico As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Valores_SeleccionarPorISIN")
        'db.AddInParameter(dbCommand, "@p_CodigoCaracteristica", DbType.String, filaLinea("CodigoCaracteristica").ToString().Trim())
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, ISIN)

        CodigoMnemonico = db.ExecuteScalar(dbCommand)

        Return CodigoMnemonico


    End Function

    Public Function RecuperaDivLibBloombergNoReg_Listar(ByVal FechaInformacion As Decimal, ByVal sEntidadExterna As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("RecuperaDivLibBloombergNoReg_Listar")

        db.AddInParameter(dbCommand, "@FechaInformacion", DbType.Decimal, FechaInformacion)
        db.AddInParameter(dbCommand, "@EntidadExterna", DbType.String, sEntidadExterna)

        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function

    Public Function SeleccionarPrecioBloomberg(ByVal FechaCarga As Date) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VectorPrecioBloomberg_Seleccionar")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))


        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function
    Public Function SeleccionarTipoCambioBloomberg(ByVal FechaCarga As Date) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VectorTipoCambioBloomberg_Seleccionar")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))


        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function
    Public Function SeleccionarIndBloomberg(ByVal FechaCarga As Date) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IndicadorBloomberg_SeleccionarIndBloomberg")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))


        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function
    Public Function SeleccionarIndInversiones(ByVal FechaCarga As Date) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IndicadorBloomberg_SeleccionarIndInversiones")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))


        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function
    Public Function SeleccionarIndDivLib(ByVal FechaCarga As Date) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IndicadorBloomberg_SeleccionarIndDivLib")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))


        Dim DstTabla As New DataSet
        DstTabla = db.ExecuteDataSet(dbCommand)

        Return DstTabla
    End Function
    Public Function EliminarTipoCambioReal(ByVal FechaCarga As Date) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VectorTipoCambio_Eliminar")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))
        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function
    Public Function EliminarTipoCambioBloomberg(ByVal FechaCarga As Date) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VectorTipoCambioBloomberg_Eliminar")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))
        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function
    Public Function EliminarPrecioReal(ByVal FechaCarga As Date) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VectorPrecio_Eliminar")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))
        db.ExecuteNonQuery(dbCommand)


        Return True
    End Function
    Public Function EliminarPrecioBloomberg(ByVal FechaCarga As Date) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VectorPrecioBloomberg_Eliminar")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))
        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function
    Public Function EliminarIndBloomberg(ByVal FechaCarga As Date) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IndicadorBloomberg_EliminarIndBloomberg")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))
        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function
    Public Function EliminarIndInv(ByVal FechaCarga As Date) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IndicadorBloomberg_EliminarIndInversiones")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))
        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function
    Public Function EliminarIndDivLib(ByVal FechaCarga As Date) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("IndicadorBloomberg_EliminarIndDivLib")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))
        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function

    'HDG INC 59941	20100526
    Public Function ActualizarPrecioBloomberg(ByVal Fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String
        Dim CodigoCaracteristicas As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim CodigoSBS, CodigoISIN, CodigoMnemonico, Emisor, valor As String
        Dim precio As Decimal

        For Each filaLinea As DataRow In dtDetalle.Rows
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizarPreciosBloomberg")
            CodigoSBS = filaLinea(0).ToString().Trim()
            CodigoISIN = filaLinea(1).ToString().Trim()
            Emisor = filaLinea(2).ToString().Trim()
            CodigoMnemonico = SeleccionarCodigoMnemonico(CodigoISIN)
            valor = filaLinea(4).ToString().Trim()

            If Not CodigoMnemonico Is Nothing Then
                If (valor <> "") Then
                    precio = Convert.ToDecimal(valor.Replace(".", Separador))
                    db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(Fecha))
                    db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, CodigoSBS)
                    db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String, CodigoISIN)
                    db.AddInParameter(dbCommand, "@p_Emisor", DbType.String, Emisor)
                    db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
                    db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, precio)
                    db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                    db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                    db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                    db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

                    db.ExecuteNonQuery(dbCommand)
                End If
            End If

        Next

        Return CodigoCaracteristicas

    End Function

    'HDG INC 59941	20100526
    Public Function ActualizarPrecioReal(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim CodigoSBS, CodigoISIN, CodigoMnemonico, Emisor, valor As String
        Dim precio As Decimal

        Me.TablaErrores = New DataTable
        InicializarTabla()
        For Each filaLinea As DataRow In dtDetalle.Rows
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizarPrecios")
            CodigoSBS = filaLinea(0).ToString().Trim()
            CodigoISIN = filaLinea(1).ToString().Trim()
            Emisor = filaLinea(2).ToString().Trim()
            CodigoMnemonico = SeleccionarCodigoMnemonico(CodigoISIN)
            valor = filaLinea(4).ToString().Trim()
            If CodigoMnemonico Is Nothing And CodigoISIN <> "ND" Then CodigoMnemonico = ""

            If Not CodigoMnemonico Is Nothing Then
                If (valor <> "") Then

                    If CodigoMnemonico = "" Then
                        Dim fila As DataRow

                        fila = Me.TablaErrores.NewRow()
                        fila("CodigoISIN") = CodigoISIN
                        TablaErrores.Rows.Add(fila)

                    Else

                        precio = Convert.ToDecimal(valor.Replace(".", Separador))
                        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(fecha))
                        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
                        db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal, precio)
                        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

                        db.ExecuteNonQuery(dbCommand)
                    End If
                End If
            End If

        Next

        Return TablaErrores

    End Function

    'HDG OT 65289 20120626
    Public Function InsertarPrecioNav(ByVal objVectorPrecioBloombergNavBE As VectorPrecioBloombergNavBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim nFecha As Decimal
        Dim sCodigoISIN As String
        Dim nPrecio As Decimal
        Dim sUsuario As String
        Dim nFecCre As Decimal
        Dim sHost As String
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizaVectorPrecioNavxExcel")
        dbCommand.CommandTimeout = 1020

        sUsuario = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
        nFecCre = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
        sHost = DataUtility.ObtenerValorRequest(dataRequest, "Host")

        db.AddInParameter(dbCommand, "@p_FechaCarga", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String)
        db.AddInParameter(dbCommand, "@p_Precio", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String)

        For Each filaLinea As DataRow In objVectorPrecioBloombergNavBE.VectorPrecioBloombergNav.Rows
            nPrecio = Val(filaLinea(4).ToString().Trim())

            If (Not nPrecio = 0) Then
                nFecha = filaLinea(0).ToString().Trim()
                sCodigoISIN = filaLinea(2).ToString().Trim()

                db.SetParameterValue(dbCommand, "@p_FechaCarga", nFecha)
                db.SetParameterValue(dbCommand, "@p_CodigoISIN", sCodigoISIN)
                db.SetParameterValue(dbCommand, "@p_Precio", nPrecio)
                db.SetParameterValue(dbCommand, "@p_UsuarioCreacion", sUsuario)
                db.SetParameterValue(dbCommand, "@p_FechaCreacion", nFecCre)
                db.SetParameterValue(dbCommand, "@p_HoraCreacion", DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.SetParameterValue(dbCommand, "@p_Host", sHost)
                db.ExecuteNonQuery(dbCommand)
            End If
        Next

        Return True
    End Function
#End Region
End Class
