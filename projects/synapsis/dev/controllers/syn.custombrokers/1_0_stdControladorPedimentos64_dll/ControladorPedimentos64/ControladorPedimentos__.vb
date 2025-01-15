@ -0,0 +1,352 @@
﻿Imports MongoDB.Bson
Imports Syn.Documento
Imports Wma.Exceptions
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Rec.Globals.Utils.Secuencias
Imports Wma.Exceptions.TagWatcher
Imports Syn.Nucleo

Public Class ControladorPedimentos
    Implements IControladorPedimentos, ICloneable, IDisposable 'IControladorPedimentos

#Region "Atributos"

    Private disposedValue As Boolean

    Private _tipoPedimento As IControladorPedimentos.TiposPedimento

    Private _estatus As TagWatcher

#End Region

#Region "Propiedades"

    Public Property Estatus As TagWatcher Implements IControladorPedimentos.Estatus

        Get

            Return _estatus

        End Get

        Set(value As TagWatcher)

            _estatus = value

        End Set

    End Property

    Public Property TipoPedimento As IControladorPedimentos.TiposPedimento Implements IControladorPedimentos.TipoPedimento

        Get

            Return _tipoPedimento

        End Get

        Set(value As IControladorPedimentos.TiposPedimento)

            _tipoPedimento = value

        End Set

    End Property

#End Region

#Region "Constructores"



#End Region

#Region "Funciones"

    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not disposedValue Then

            If disposing Then

                ' TODO: eliminar el estado administrado (objetos administrados)
                _listaObjetos = Nothing

                _listaFolios = Nothing

            End If

            ' TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
            ' TODO: establecer los campos grandes como NULL

            disposedValue = True

        End If

    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose

        ' No cambie este código. Coloque el código de limpieza en el método "Dispose(disposing As Boolean)".
        Dispose(disposing:=True)

        GC.SuppressFinalize(Me)

    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone

        Throw New NotImplementedException()

    End Function

    Public Function CrearPedimentos(listaIdsReferencias_ As List(Of ObjectId)) As TagWatcher Implements IControladorPedimentos.CrearPedimentos

        Throw New NotImplementedException()

    End Function

    Public Function CrearPedimentos(ByRef listaReferencias_ As List(Of DocumentoElectronico)) As TagWatcher Implements IControladorPedimentos.CrearPedimentos

        Throw New NotImplementedException()

    End Function

    Public Function RegenerarSecuenciaPedimento(idPedimento_ As ObjectId) As TagWatcher Implements IControladorPedimentos.RegenerarSecuenciaPedimento

        Throw New NotImplementedException()

    End Function

    Public Function RegenerarSecuenciaPedimento(ByRef pedimento_ As DocumentoElectronico) As TagWatcher Implements IControladorPedimentos.RegenerarSecuenciaPedimento

        Throw New NotImplementedException()

    End Function

    Public Function ObtenerEstructuraPedimento(idPedimento_ As ObjectId) As TagWatcher Implements IControladorPedimentos.ObtenerEstructuraPedimento

        Throw New NotImplementedException()

    End Function

    Public Function ObtenerEstructuraPedimento(ByRef pedimento_ As DocumentoElectronico) As TagWatcher Implements IControladorPedimentos.ObtenerEstructuraPedimento

        Throw New NotImplementedException()

    End Function

    Public Function ReplicarCamposReferencia(idReferencia_ As ObjectId, listaCamposValor_ As Dictionary(Of [Enum], String)) As TagWatcher Implements IControladorPedimentos.ReplicarCamposReferencia

        Throw New NotImplementedException()

    End Function

    Public Function ValidarPedimentos(listaIdsPedimentos_ As List(Of ObjectId)) As TagWatcher Implements IControladorPedimentos.ValidarPedimentos

        Throw New NotImplementedException()

    End Function

    Public Function ValidarPedimentos(ByRef listaPedimentos_ As List(Of DocumentoElectronico)) As TagWatcher Implements IControladorPedimentos.ValidarPedimentos

        Throw New NotImplementedException()

    End Function

    Public Function ValidarGenerarArchivoM3(idPedimento_ As ObjectId) As TagWatcher Implements IControladorPedimentos.ValidarGenerarArchivoM3

        Throw New NotImplementedException()

    End Function

    Public Function ValidarGenerarArchivoM3(ByRef pedimento_ As DocumentoElectronico) As TagWatcher Implements IControladorPedimentos.ValidarGenerarArchivoM3

        Throw New NotImplementedException()

    End Function

    Public Function FirmarValidacionPedimentos(listaIdsPedimentos_ As List(Of ObjectId)) As TagWatcher Implements IControladorPedimentos.FirmarValidacionPedimentos

        Throw New NotImplementedException()

    End Function

    Public Function FirmarValidacionPedimentos(ByRef listaPedimentos_ As List(Of DocumentoElectronico)) As TagWatcher Implements IControladorPedimentos.FirmarValidacionPedimentos

        Throw New NotImplementedException()

    End Function

    Public Function BorrarFirmarValidacion(idPedimento_ As ObjectId) As TagWatcher Implements IControladorPedimentos.BorrarFirmarValidacion

        Throw New NotImplementedException()

    End Function

    Public Function BorrarFirmarValidacion(ByRef pedimento_ As DocumentoElectronico) As TagWatcher Implements IControladorPedimentos.BorrarFirmarValidacion

        Throw New NotImplementedException()

    End Function

    Public Function PagarPedimentos(listaIdsPedimentos_ As List(Of ObjectId)) As TagWatcher Implements IControladorPedimentos.PagarPedimentos

        Throw New NotImplementedException()

    End Function

    Public Function PagarPedimentos(ByRef listaPedimentos_ As List(Of DocumentoElectronico)) As TagWatcher Implements IControladorPedimentos.PagarPedimentos

        Throw New NotImplementedException()

    End Function

    Public Function PagarGenerarArchivoPago(idPedimento_ As ObjectId) As TagWatcher Implements IControladorPedimentos.PagarGenerarArchivoPago

        Throw New NotImplementedException()

    End Function

    Public Function PagarGenerarArchivoPago(ByRef pedimento_ As DocumentoElectronico) As TagWatcher Implements IControladorPedimentos.PagarGenerarArchivoPago

        Throw New NotImplementedException()

    End Function

    Public Function AplicarFirmasPagoPedimentos(listaIdsPedimentos_ As List(Of ObjectId)) As TagWatcher Implements IControladorPedimentos.AplicarFirmasPagoPedimentos

        Throw New NotImplementedException()

    End Function

    Public Function AplicarFirmasPagoPedimentos(ByRef listaPedimentos_ As List(Of DocumentoElectronico)) As TagWatcher Implements IControladorPedimentos.AplicarFirmasPagoPedimentos

        Throw New NotImplementedException()

    End Function

    Public Function PublicarPedimento(idPedimento_ As ObjectId) As TagWatcher Implements IControladorPedimentos.PublicarPedimento

        Throw New NotImplementedException()

    End Function

    Public Function PublicarPedimento(ByRef pedimento_ As DocumentoElectronico) As TagWatcher Implements IControladorPedimentos.PublicarPedimento

        Throw New NotImplementedException()

    End Function

    Public Function RelacionarDocumentos(idPedimento_ As ObjectId, listaDocumentosAsociar_ As Dictionary(Of ObjectId, TiposDocumentoDigital)) As TagWatcher Implements IControladorPedimentos.RelacionarDocumentos

        Throw New NotImplementedException()

    End Function

    Public Function ConsultarDocumentosRelacionados(idPedimento_ As ObjectId, listaDocumentosConsultar_ As Dictionary(Of ObjectId, TiposDocumentoDigital)) As TagWatcher Implements IControladorPedimentos.ConsultarDocumentosRelacionados

        Throw New NotImplementedException()

    End Function

    Private Function ExtraerDatosReferencia(ByVal referencia_ As DocumentoElectronico) As DocumentoElectronico

        Dim pedimento_ As New DocumentoElectronico

        If referencia_ IsNot Nothing Then

            Select Case TipoPedimento

                Case 1 'Normal

                    pedimento_ = New ConstructorPedimentoNormal
                    pedimento_.Attribute(CamposPedimento.CA_TIPO_OPERACION).Valor = referencia_.Attribute(CamposPedimento.CA_TIPO_OPERACION).Valor
                    pedimento_.Attribute(CamposPedimento.CA_TIPO_OPERACION).ValorPresentacion = referencia_.Attribute(CamposPedimento.CA_TIPO_OPERACION).ValorPresentacion
                    pedimento_.Attribute(CamposPedimento.CP_REFERENCIA).Valor = referencia_.Attribute(CamposPedimento.CP_REFERENCIA).Valor
                    pedimento_.Attribute(CamposPedimento.CP_TIPO_PEDIMENTO).Valor = referencia_.Attribute(CamposPedimento.CP_TIPO_PEDIMENTO).Valor
                    pedimento_.Attribute(CamposPedimento.CA_CVE_PEDIMENTO).Valor = referencia_.Attribute(CamposPedimento.CA_CVE_PEDIMENTO).Valor
                    pedimento_.Attribute(CamposPedimento.CA_CVE_PEDIMENTO).ValorPresentacion = referencia_.Attribute(CamposPedimento.CA_CVE_PEDIMENTO).ValorPresentacion
                    pedimento_.Attribute(CamposPedimento.CA_REGIMEN).Valor = referencia_.Attribute(CamposPedimento.CA_REGIMEN).Valor
                    pedimento_.Attribute(CamposPedimento.CA_REGIMEN).ValorPresentacion = referencia_.Attribute(CamposPedimento.CA_REGIMEN).ValorPresentacion
                    pedimento_.Attribute(CamposPedimento.CP_TIPO_DESPACHO).Valor = referencia_.Attribute(CamposPedimento.CP_TIPO_DESPACHO).Valor
                    pedimento_.Attribute(CamposPedimento.CA_CLAVE_SAD).Valor = referencia_.Attribute(CamposPedimento.CA_CLAVE_SAD).Valor
                    pedimento_.Attribute(CamposPedimento.CA_PATENTE).Valor = referencia_.Attribute(CamposPedimento.CA_PATENTE).Valor
                    pedimento_.Attribute(CamposPedimento.CA_RAZON_SOCIAL_IOE).Valor = referencia_.Attribute(CamposClientes.CA_RAZON_SOCIAL).Valor
                    pedimento_.Attribute(CamposPedimento.CA_RFC_IOE).Valor = referencia_.Attribute(CamposClientes.CA_RAZON_SOCIAL).Valor

                Case 2 'Complementario

                Case 3 'Tránsito

                Case 4 'Rectificación

                Case 5 'Global complementario

                Case Else

            End Select


            If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                _estatus.ObjectReturned = pedimento_

                _estatus.SetOK()

            Else

                _estatus.SetError(Me, "No se pueden procesar las referencias recibidas.")

            End If

        Else

            _estatus.SetError(Me, "La referencia que se recibio esta vacía y en borrador.")

        End If

        Return pedimento_

    End Function

    Private Function GenerarSecuenciaPedimento(ByRef pedimento_ As DocumentoElectronico) As Secuencia

        Dim controladorSecuencias_ = New ControladorSecuencia

        Dim secuencia_ = New Secuencia With {.nombre = SecuenciasComercioExterior.Pedimentos.ToString,
            .environment = 0,
            .anio = ,
            .mes = 0,
            .area = 0,
            .compania = 0,
            .tiposecuencia = 0,
            .subtiposecuencia = 0,
            .prefijo = pedimento_.Attribute(CamposPedimento.CA_PATENTE).Valor,
            .sufijo = pedimento_.Attribute(CamposPedimento.CA_CLAVE_SAD).Valor,
            .estado = 1
        }

        Dim sec_ As Int32 = 0

        '_estatus = controladorSecuencias_.Generar(SecuenciasComercioExterior.Pedimentos.ToString, 1, 1, 1, 1, 1)
        _estatus = controladorSecuencias_.Generar(secuencia_, Nothing)

        With _estatus

            If .Status = TypeStatus.Ok Then

                secuencia_ = DirectCast(.ObjectReturned, Secuencia)

            End If

        End With

        Return secuencia_

    End Function

#End Region

End Class