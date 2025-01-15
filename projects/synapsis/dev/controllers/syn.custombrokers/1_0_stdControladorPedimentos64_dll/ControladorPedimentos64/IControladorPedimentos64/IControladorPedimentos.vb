Imports Wma.Exceptions
Imports MongoDB.Bson
Imports Syn.Documento
Imports Syn.Nucleo.Recursos

Public Interface IControladorPedimentos

#Region "Enum"

    Enum TiposPedimento

        SINDEFINIR = 0
        NORMAL = 1
        COMPLEMENTARIO = 2
        TRANSITO = 3
        RECTIFICACION = 4
        GLOBALCOMPLEMENTARIO = 5

    End Enum

#End Region

#Region "Propiedades"

    Property TipoPedimento As TiposPedimento

    Property Estatus As TagWatcher

    Property Pedimento As DocumentoElectronico

    Property Pedimentos As List(Of DocumentoElectronico)

#End Region

#Region "Funciones"

    Function CrearPedimentosAsync(listaIdsReferencias_ As List(Of ObjectId)) As Task(Of TagWatcher)

    Function CrearPedimentos(ByRef listaReferencias_ As List(Of DocumentoElectronico)) As TagWatcher

    Function RegenerarSecuenciaPedimentoAsync(idPedimento_ As ObjectId) As Task(Of TagWatcher)

    Function RegenerarSecuenciaPedimento(ByRef pedimento_ As DocumentoElectronico) As TagWatcher

    Function ObtenerEstructuraPedimento(idPedimento_ As ObjectId) As TagWatcher

    Function EvaluarSecciones(ByRef configuracionesSeccion_ As Dictionary(Of [Enum], ConfiguracionNodo),
                              Optional ByVal pedimento_ As DocumentoElectronico = Nothing) As TagWatcher 'Lo agrego pero esto puede ser el de Obtener estructura

    Function ObtenerEstructuraPedimento(ByRef pedimento_ As DocumentoElectronico) As TagWatcher

    Function ReplicarCamposReferencia(idReferencia_ As ObjectId, listaCamposValor_ As Dictionary(Of [Enum], String)) As TagWatcher

    Function ValidarPedimentos(listaIdsPedimentos_ As List(Of ObjectId)) As TagWatcher

    Function ValidarPedimentos(ByRef listaPedimentos_ As List(Of DocumentoElectronico)) As TagWatcher

    Function ValidarGenerarArchivoM3(idPedimento_ As ObjectId) As TagWatcher

    Function ValidarGenerarArchivoM3(ByRef pedimento_ As DocumentoElectronico) As TagWatcher

    Function FirmarValidacionPedimentos(listaIdsPedimentos_ As List(Of ObjectId)) As TagWatcher

    Function FirmarValidacionPedimentos(ByRef listaPedimentos_ As List(Of DocumentoElectronico)) As TagWatcher

    Function BorrarFirmarValidacion(idPedimento_ As ObjectId) As TagWatcher

    Function BorrarFirmarValidacion(ByRef pedimento_ As DocumentoElectronico) As TagWatcher

    Function PagarPedimentos(listaIdsPedimentos_ As List(Of ObjectId)) As TagWatcher

    Function PagarPedimentos(ByRef listaPedimentos_ As List(Of DocumentoElectronico)) As TagWatcher

    Function PagarGenerarArchivoPago(idPedimento_ As ObjectId) As TagWatcher

    Function PagarGenerarArchivoPago(ByRef pedimento_ As DocumentoElectronico) As TagWatcher

    Function AplicarFirmasPagoPedimentos(listaIdsPedimentos_ As List(Of ObjectId)) As TagWatcher

    Function AplicarFirmasPagoPedimentos(ByRef listaPedimentos_ As List(Of DocumentoElectronico)) As TagWatcher

    Function PublicarPedimento(idPedimento_ As ObjectId) As TagWatcher

    Function PublicarPedimento(ByRef pedimento_ As DocumentoElectronico) As TagWatcher

    Function RelacionarDocumentos(idPedimento_ As ObjectId, listaDocumentosAsociar_ As Dictionary(Of ObjectId, TiposDocumentoDigital)) As TagWatcher

    Function ConsultarDocumentosRelacionados(idPedimento_ As ObjectId, listaDocumentosConsultar_ As Dictionary(Of ObjectId, TiposDocumentoDigital)) As TagWatcher

#End Region

End Interface