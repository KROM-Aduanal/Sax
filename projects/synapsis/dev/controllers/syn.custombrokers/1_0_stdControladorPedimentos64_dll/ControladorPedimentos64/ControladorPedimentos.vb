Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals.Utils.Secuencias
Imports Syn.Documento
Imports Syn.Nucleo.Recursos
Imports Syn.Nucleo.RecursosComercioExterior
Imports Wma.Exceptions
Imports Wma.Exceptions.TagWatcher
Imports Syn.Documento.Componentes
Imports ConfiguracionNodo.TiposVisibilidad
Imports Syn.Nucleo.RecursosComercioExterior.SeccionesPedimento
Imports Syn.Nucleo.RecursosComercioExterior.CamposPedimento
Imports Rec.Globals.Controllers.IControladorPedimentos.TiposPedimento

Public Class ControladorPedimentos
    Implements IControladorPedimentos, ICloneable, IDisposable

#Region "Atributos"

    Private disposedValue As Boolean

    Private _tipoPedimento As IControladorPedimentos.TiposPedimento

    Private _estatus As TagWatcher

    Private _pedimento As DocumentoElectronico

    Private _pedimentos As List(Of DocumentoElectronico)

    Private _listaClavesNormalImpo As New List(Of String)

    Private _listaClavesNormalExpo As New List(Of String)

    Private _listaClavesNormalExpoAN20 As New List(Of String)

    Private _listaClavesNormalImpoAN20 As New List(Of String)

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

    Private Property Pedimento As DocumentoElectronico Implements IControladorPedimentos.Pedimento

        Get

            Return _pedimento

        End Get

        Set(value As DocumentoElectronico)

            _pedimento = value

        End Set

    End Property

    Public Property Pedimentos As List(Of DocumentoElectronico) Implements IControladorPedimentos.Pedimentos

        Get

            Return _pedimentos

        End Get

        Set(value As List(Of DocumentoElectronico))

            _pedimentos = value

        End Set

    End Property

#End Region

#Region "Constructores"

    Sub New()

        _estatus = New TagWatcher

    End Sub

#End Region

#Region "Funciones"

    Public Async Function CrearPedimentosAsync(listaIdsReferencias_ As List(Of ObjectId)) As Task(Of TagWatcher) Implements IControladorPedimentos.CrearPedimentosAsync

        Dim _listaReferencias = New List(Of OperacionGenerica)

        If listaIdsReferencias_ IsNot Nothing Then

            If listaIdsReferencias_.Count > 0 Then

                Using iEnlace_ As IEnlaceDatos = New EnlaceDatos

                    Dim auxCount As Integer = listaIdsReferencias_.Count

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorReferencia).GetType.Name)

                    operationsDB_.Aggregate().
                    Match(Function(a) listaIdsReferencias_.Contains(a.Id)).
                    ToList().ForEach(Sub(item)
                                         item.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Id = item.Id.ToString
                                         _listaReferencias.Add(item)
                                     End Sub)

                    If _listaReferencias.Count > 0 Then

                        If _listaReferencias.Count = auxCount Then

                            _pedimentos = New List(Of DocumentoElectronico)

                            For Each item_ In _listaReferencias

                                Dim pedimento_ = ExtraerDatosReferencia(item_)

                                If pedimento_.EstructuraDocumento IsNot Nothing Then

                                    _pedimentos.Add(pedimento_)

                                    _estatus.SetOK()

                                Else

                                    _estatus.SetError(Me, "No se pudieron extraer los datos de la referencia al pedimento.")

                                End If

                            Next

                        Else

                            _estatus.SetOKBut(Me, "Referencias no encontradas")

                        End If

                        If _estatus.Status = TypeStatus.Ok Then

                            'Using _session = iEnlace_.GetMongoClient().StartSession
                            Using _session = Await iEnlace_.GetMongoClient().StartSessionAsync()

                                _session.StartTransaction()

                                For Each pedimento_ In _pedimentos

                                    _estatus = GuardarPedimentoAsync(pedimento_, pedimento_.Attribute(CP_TIPO_PEDIMENTO).Valor, _session, iEnlace_).Result

                                    If _estatus.Status = TypeStatus.Ok Then

                                        Dim idReferencia_ = From docAso_ In pedimento_.DocumentosAsociados
                                                            Where docAso_.identificadorrecurso = "ConstructorReferencia"
                                                            Select docAso_._iddocumentoasociado

                                        _estatus = ActualizarPedimentoReferenciaAsync(New ObjectId(idReferencia_(0).ToString), _session, iEnlace_).Result

                                        If _estatus.Status = TypeStatus.Ok Then

                                            Await _session.CommitTransactionAsync().ConfigureAwait(False)

                                            _estatus.SetOK()

                                        Else

                                            _estatus.SetError(Me, "Hubo un error al actualizar la referencia.")

                                            Await _session.AbortTransactionAsync()

                                            Exit For

                                        End If

                                    Else

                                        _estatus.SetError(Me, "Hubo un error al guardar el pedimento.")

                                        Await _session.AbortTransactionAsync()

                                        Exit For

                                    End If

                                Next

                                If _estatus.Status = TypeStatus.Ok Then

                                    _estatus.ObjectReturned = _pedimentos
                                    Pedimentos = _pedimentos

                                Else

                                    _estatus.ObjectReturned = Nothing

                                End If

                            End Using

                        End If

                    Else

                        _estatus.SetOKBut(Me, "Referencias no encontradas")

                    End If

                End Using

            End If

        Else

            _estatus.SetError(Me, "No se tienen datos de alguna referencia.")

        End If

        Return _estatus

    End Function

    Public Function CrearPedimentos(ByRef listaReferencias_ As List(Of DocumentoElectronico)) As TagWatcher Implements IControladorPedimentos.CrearPedimentos

        Throw New NotImplementedException()

    End Function

    Public Async Function RegenerarSecuenciaPedimentoAsync(idPedimento_ As ObjectId) As Task(Of TagWatcher) Implements IControladorPedimentos.RegenerarSecuenciaPedimentoAsync

        Dim secuencia_ As Secuencia

        Dim result_ As Task = Nothing

        _pedimento = New DocumentoElectronico

        If idPedimento_ <> Nothing Then

            Select Case _tipoPedimento

                Case NORMAL

                    'Debe de a ver antes algo que valide el anio de validación y el anio en curso cuando das clic en regenerar
                    Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With
                        {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                        Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorPedimentoNormal).GetType.Name)

                        Dim filter_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, idPedimento_)

                        _estatus.ObjectReturned = operationsDB_.Find(filter_).ToList()

                        _pedimento = _estatus.ObjectReturned(0).Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

                        If _pedimento.Attribute(CP_ANIO_CURSO).Valor <> Year(Now) Then

                            'Generar secuencia
                            secuencia_ = GenerarSecuenciaPedimento(_pedimento)

                            If secuencia_ IsNot Nothing Then

                                If secuencia_.sec > 0 Then

                                    'Crear número de pedimento
                                    _pedimento.Attribute(CA_NUMERO_PEDIMENTO).Valor = Mid(_pedimento.Attribute(CP_ANIO_CURSO).Valor, 4, 1) + Microsoft.VisualBasic.Format(CLng(secuencia_.sec), "000000")

                                    'Crear número de pedimento completo
                                    _pedimento.Attribute(CA_NUMERO_PEDIMENTO_COMPLETO).Valor = CrearNumeroPedimentoCompleto(Mid(_pedimento.Attribute(CA_ANIO_VALIDACION).Valor, 3, 2),
                                                                                                                                            Mid(_pedimento.Attribute(CA_CLAVE_SAD).Valor, 1, 2),
                                                                                                                                            _pedimento.Attribute(CA_PATENTE).Valor,
                                                                                                                                            _pedimento.Attribute(CA_NUMERO_PEDIMENTO).Valor
                                                                                                                                            )
                                    _pedimento.FolioOperacion = _pedimento.Attribute(CA_NUMERO_PEDIMENTO_COMPLETO).Valor

                                Else

                                    _estatus.SetError(Me, "Se obtuvo una secuencia igual con 0, favor de revisar sus datos.")

                                End If

                            Else

                                _estatus.SetError(Me, "No se tiene la secuencia de pedimento, el controlador no responde.")

                            End If

                            Using _session = Await iEnlace_.GetMongoClient().StartSessionAsync().ConfigureAwait(False)

                                _session.StartTransaction()

                                'Actualizar numero de pedimento y numero completo en pedimento
                                Using entidadDatos_ As IEntidadDatos = New ConstructorPedimentoNormal()

                                    Dim collection_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorPedimentoNormal).GetType.Name)

                                    Dim filterPed_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, idPedimento_)

                                    Dim setStructureOfSubs_ = Builders(Of OperacionGenerica).Update.
                                                                  Set(Of String)("FolioOperacion", _pedimento.FolioOperacion).
                                                                  Set(Of String)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.FolioOperacion", _pedimento.FolioOperacion).
                                                                  Set(Of String)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.0.Nodos.0.Valor", _pedimento.FolioOperacion).
                                                                  Set(Of String)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Cuerpo.3.Nodos.0.Nodos.1.Nodos.0.Valor", _pedimento.Attribute(CA_NUMERO_PEDIMENTO).Valor)

                                    Await collection_.UpdateOneAsync(filterPed_, setStructureOfSubs_).ConfigureAwait(False)

                                    result_ = collection_.CountDocumentsAsync(filter_)

                                End Using

                                With _estatus

                                    If result_.Id <> Nothing Then

                                        .ObjectReturned = _pedimento

                                        .SetOK()

                                    Else

                                        .SetOKBut(Me, "Hubo un detalle al actualizar el nuevo número de pedimento.")

                                    End If

                                End With

                                Dim idReferencia_ = From docAso_ In _pedimento.DocumentosAsociados
                                                    Where docAso_.identificadorrecurso = "ConstructorReferencia"
                                                    Select docAso_._iddocumentoasociado

                                _estatus = ActualizarPedimentoReferenciaAsync(New ObjectId(idReferencia_.ToString), _session, iEnlace_).Result

                                If _estatus.Status = TypeStatus.Ok Then

                                    _estatus.ObjectReturned = _pedimento

                                End If

                            End Using

                        Else

                            _estatus.SetOKInfo(Me, "No se actualizará porque el año sigue siendo igual y no afecta el proceso")

                        End If

                    End Using

                Case COMPLEMENTARIO

                Case TRANSITO

                Case RECTIFICACION

                Case GLOBALCOMPLEMENTARIO

            End Select

        Else

            _estatus.SetError(Me, "No se tienen datos de algún pedimento.")

        End If

        Return _estatus

    End Function

    Public Function RegenerarSecuenciaPedimento(ByRef pedimento_ As DocumentoElectronico) As TagWatcher Implements IControladorPedimentos.RegenerarSecuenciaPedimento

        Throw New NotImplementedException()

    End Function

    Public Function EvaluarSecciones(ByRef configuracionesSeccion_ As Dictionary(Of [Enum], ConfiguracionNodo),
                                   Optional ByVal pedimento_ As DocumentoElectronico = Nothing) As TagWatcher Implements IControladorPedimentos.EvaluarSecciones

        _estatus = New TagWatcher
        Dim configuracionesSeccionCondicionada_ = New Dictionary(Of [Enum], ConfiguracionNodo)

        'Mandar tagwatcher de ok o de error cuando no se encuentre un campo o este vacio
        If pedimento_ IsNot Nothing Then

            _pedimento = pedimento_

        Else

            If _pedimento IsNot Nothing Then

                If configuracionesSeccion_ IsNot Nothing And configuracionesSeccion_.Count > 0 Then

                    For Each configuracion_ In configuracionesSeccion_

                        If configuracion_.Value.TipoVisibilidad = Condicionado Then

                            configuracionesSeccionCondicionada_.Add(configuracion_.Key, configuracion_.Value)

                        End If

                    Next

                    'No responde a la condición en lin-Q y pasa todas :(

                    _estatus.SetOK()

                Else

                    _estatus.SetError(Me, "No se tienen las configuraciones de las secciones del pedimento ¡Verificar!.")

                End If

                If _estatus.Status = TypeStatus.Ok Then

                    CrearListas(_pedimento.TipoDocumentoElectronico)

                    Select Case _pedimento.TipoDocumentoElectronico

                        Case TiposDocumentoElectronico.PedimentoNormal

                            For Each seccionRevisar_ In configuracionesSeccionCondicionada_

                                Dim seccion_ As SeccionesPedimento = seccionRevisar_.Key
                                Dim configuracion_ As ConfiguracionNodo = seccionRevisar_.Value
                                Dim valor_ = Nothing

                                Select Case seccion_

                                    Case ANS6, ANS7, ANS24

                                        valor_ = _pedimento.Attribute(CA_NUMERO_TOTAL_PARTIDAS).Valor

                                        If valor_ <> 0 And valor_ IsNot Nothing Then

                                            If valor_ >= 1 Then

                                                configuracion_.TipoVisibilidad = Visible

                                            End If

                                        Else

                                            configuracion_.TipoVisibilidad = Oculto

                                        End If

                                        _estatus.SetOK()

                                    Case ANS9

                                        valor_ = _pedimento.Attribute(CA_LINEA_CAPTURA).Valor

                                        If valor_ = "" Then

                                            configuracion_.TipoVisibilidad = Oculto

                                        ElseIf valor_ <> "" Then

                                            configuracion_.TipoVisibilidad = Visible

                                        End If

                                        _estatus.SetOK()

                                    Case ANS11

                                        valor_ = _pedimento.Attribute(CA_TIPO_OPERACION).Valor

                                        If valor_ IsNot Nothing Then

                                            If valor_ = IRecursosSistemas.TipoOperacion.Exportacion Then

                                                configuracion_.TipoVisibilidad = Visible

                                            Else

                                                configuracion_.TipoVisibilidad = Oculto

                                            End If

                                            _estatus.SetOK()

                                        Else

                                            _estatus.SetError(Me, "No se tiene valor en el campo " + CA_TIPO_OPERACION.ToString + " se requiere para configurar.")

                                        End If

                                    Case ANS12

                                        valor_ = _pedimento.Attribute(CA_CVE_PEDIMENTO).Valor
                                        configuracion_.TipoVisibilidad = Visible
                                        Dim valorOperacion_ = _pedimento.Attribute(CA_TIPO_OPERACION).Valor

                                        If valor_ IsNot Nothing And valor_ <> "" Then

                                            If valorOperacion_ IsNot Nothing And valorOperacion_ > 0 Then

                                                If valorOperacion_ = IRecursosSistemas.TipoOperacion.Exportacion Then

                                                    If _listaClavesNormalExpo.Contains(valor_) Then

                                                        configuracion_.TipoVisibilidad = Oculto

                                                    End If

                                                ElseIf valorOperacion_ = IRecursosSistemas.TipoOperacion.Importacion Then

                                                    If _listaClavesNormalImpo.Contains(valor_) Then

                                                        configuracion_.TipoVisibilidad = Oculto

                                                    End If

                                                End If

                                                _estatus.SetOK()

                                            Else

                                                _estatus.SetError(Me, "No se tiene valor en el campo " + CA_TIPO_OPERACION.ToString + "  se requiere para configurar.")

                                            End If

                                        Else

                                            _estatus.SetError(Me, "No se tiene valor en el campo " + CA_CVE_PEDIMENTO.ToString + " se requiere para configurar.")

                                        End If

                                    Case ANS19

                                        valor_ = _pedimento.Attribute(CA_CVE_PEDIMENTO).Valor
                                        Dim formasPago_ = _pedimento.ObtenerCamposSeccion(ANS55)
                                        Dim encontrado_ = False

                                        For Each item_ In formasPago_

                                            For Each i_ In item_.Nodos

                                                If DirectCast(i_, CampoGenerico).IDUnico = CA_FORMA_PAGO Then

                                                    If DirectCast(i_, CampoGenerico).Valor = 4 Or DirectCast(i_, Campo).Valor = 15 Then

                                                        encontrado_ = True

                                                    End If

                                                End If

                                            Next

                                        Next

                                        If valor_ IsNot Nothing And valor_ <> "" Then

                                            If valor_ = "S2" Or encontrado_ = True Then

                                                configuracion_.TipoVisibilidad = Visible

                                            Else

                                                configuracion_.TipoVisibilidad = Oculto

                                            End If

                                            _estatus.SetOK()

                                        Else

                                            _estatus.SetError(Me, "No se tiene valor en el campo " + CA_CVE_PEDIMENTO.ToString + " se requiere para configurar.")

                                        End If

                                    Case ANS20

                                        valor_ = _pedimento.Attribute(CA_CVE_PEDIMENTO).Valor
                                        configuracion_.TipoVisibilidad = Oculto
                                        Dim valorOperacion_ = _pedimento.Attribute(CA_TIPO_OPERACION).Valor

                                        If valor_ IsNot Nothing And valor_ <> "" Then

                                            If valorOperacion_ IsNot Nothing Then

                                                If valorOperacion_ = IRecursosSistemas.TipoOperacion.Exportacion Then

                                                    If _listaClavesNormalExpoAN20.Contains(valor_) Then

                                                        configuracion_.TipoVisibilidad = Visible

                                                    End If

                                                ElseIf valorOperacion_ = IRecursosSistemas.TipoOperacion.Importacion Then

                                                    If _listaClavesNormalImpoAN20.Contains(valor_) Then

                                                        configuracion_.TipoVisibilidad = Visible

                                                    End If

                                                End If

                                                _estatus.SetOK()

                                            Else

                                                _estatus.SetError(Me, "No se tiene valor en el campo " + CA_TIPO_OPERACION.ToString + " se requiere para configurar.")

                                            End If

                                        Else

                                            _estatus.SetError(Me, "No se tiene valor en el campo " + CA_CVE_PEDIMENTO.ToString + " se requiere para configurar.")

                                        End If

                                    Case ANS21

                                        Dim formasPago_ = _pedimento.ObtenerCamposSeccion(ANS55)
                                        Dim encontrado_ = False

                                        For Each item_ In formasPago_

                                            For Each i_ In item_.Nodos

                                                If DirectCast(i_, CampoGenerico).Nombre = CA_FORMA_PAGO.ToString Then

                                                    If DirectCast(i_, CampoGenerico).Valor = 12 Then

                                                        encontrado_ = True

                                                    End If

                                                End If

                                            Next

                                        Next

                                        If encontrado_ Then

                                            configuracion_.TipoVisibilidad = Visible

                                        Else

                                            configuracion_.TipoVisibilidad = Oculto

                                        End If

                                        _estatus.SetOK()

                                    Case ANS22

                                        Dim formasPago_ = _pedimento.ObtenerCamposSeccion(ANS55)
                                        Dim encontrado_ = False

                                        For Each item_ In formasPago_

                                            For Each i_ In item_.Nodos

                                                If DirectCast(i_, CampoGenerico).Nombre = CA_FORMA_PAGO.ToString Then

                                                    If DirectCast(i_, CampoGenerico).Valor = 2 _
                                                        Or DirectCast(i_, CampoGenerico).Valor = 4 _
                                                        Or DirectCast(i_, CampoGenerico).Valor = 7 _
                                                        Or DirectCast(i_, CampoGenerico).Valor = 12 _
                                                        Or DirectCast(i_, CampoGenerico).Valor = 15 _
                                                        Or DirectCast(i_, CampoGenerico).Valor = 19 _
                                                        Or DirectCast(i_, CampoGenerico).Valor = 22 Then

                                                        encontrado_ = True

                                                    End If

                                                End If

                                            Next

                                        Next

                                        If encontrado_ Then

                                            configuracion_.TipoVisibilidad = Visible

                                        Else

                                            configuracion_.TipoVisibilidad = Oculto

                                        End If

                                        _estatus.SetOK()

                                    Case Else

                                End Select

                            Next

                        Case TiposDocumentoElectronico.PedimentoRectificacion

                        Case TiposDocumentoElectronico.PedimentoComplementario

                        Case TiposDocumentoElectronico.PedimentoGlobalComplementario

                    End Select

                    'Para actualizar la configuración
                    For Each configuacion_ In configuracionesSeccion_

                        If configuracionesSeccionCondicionada_.Contains(configuacion_) Then

                            configuracionesSeccion_.Item(configuacion_.Key).TipoVisibilidad = configuracionesSeccionCondicionada_.Item(configuacion_.Key).TipoVisibilidad

                        End If

                    Next

                    _estatus.ObjectReturned = configuracionesSeccion_

                End If

            Else

                _estatus.SetError(Me, "No se pudo cargar el pedimento correctamente.")

            End If

        End If

        Return _estatus

    End Function

    Private Sub CrearListas(ByVal tipoDocumento_ As TiposDocumentoElectronico)

        Select Case tipoDocumento_

            Case TiposDocumentoElectronico.PedimentoNormal

                'Creamos las listas de las claves que le tocan a ciertos tipos para hacer la comparación
                _listaClavesNormalImpo = New List(Of String) From {"C3", "E1", "E2", "G1", "G2", "F4", "F5", "BB", "G9", "V1",
                                                                    "V2", "V3", "V5", "V6", "V7", "V8", "V9"}

                _listaClavesNormalExpo = New List(Of String) From {"G1", "F4", "BB", "V1", "G9", "V2", "V4", "V6", "V7", "V9", "VD"}

                _listaClavesNormalImpoAN20 = New List(Of String) From {"K1", "F4", "F5", "A3", "BR", "BO", "H1", "H8", "I1", "E1", "E2", "E3", "E4", "G1", "G2", "C3", "K2", "D1", "F8", "M3", "V1", "V5", "V7", "V9", "BB"}

                _listaClavesNormalExpoAN20 = New List(Of String) From {"K1", "F4", "BR", "H1", "H8", "I1", "G1", "K2", "K3", "J3", "J4", "D1", "S2", "F9", "V1", "V7", "V9", "BB"}

            Case TiposDocumentoElectronico.PedimentoRectificacion

            Case TiposDocumentoElectronico.PedimentoComplementario

            Case TiposDocumentoElectronico.PedimentoGlobalComplementario

        End Select

    End Sub

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

    Private Function ExtraerDatosReferencia(ByVal referencia_ As OperacionGenerica) As DocumentoElectronico

        Dim secuencia_ As Secuencia
        Dim aduanaPresentacion_ As String = Nothing

        With referencia_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente

            _tipoPedimento = .Attribute(CamposReferencia.CP_TIPO_PEDIMENTO).Valor
            Dim aduanaSAD_ = .Attribute(CA_CLAVE_SAD).Valor

            'Consultamos el valor presentación del apéndice de aduanas
            Using iEnlace_ As IEnlaceDatos = New EnlaceDatos With
                     {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                Dim operationsDB_ = iEnlace_.GetMongoCollection(Of BsonDocument)("[SynapsisN].[dbo].[Vt022AduanaSeccionA01]")

                Dim resultado_ = operationsDB_.Aggregate.Match("{'t_Cve_AduanaSeccion':'" & aduanaSAD_.ToString & "'}").Project(BsonDocument.Parse("{" & "t_ClaveDescripcion:1}")).ToList

                aduanaPresentacion_ = resultado_(0).Elements(1).Value.ToString

            End Using

            If referencia_ IsNot Nothing Then

                Select Case TipoPedimento

                    Case NORMAL

                        _pedimento = New ConstructorPedimentoNormal

                        _pedimento.Attribute(CA_TIPO_OPERACION).Valor = .Attribute(CA_TIPO_OPERACION).Valor
                        _pedimento.Attribute(CA_TIPO_OPERACION).ValorPresentacion = .Attribute(CA_TIPO_OPERACION).ValorPresentacion
                        _pedimento.Attribute(CA_CVE_TIPO_OPERACION).Valor = .Attribute(CA_TIPO_OPERACION).Valor
                        _pedimento.Attribute(CP_REFERENCIA).Valor = .Attribute(CamposReferencia.CP_REFERENCIA).Valor
                        _pedimento.Attribute(CP_TIPO_PEDIMENTO).Valor = .Attribute(CamposReferencia.CP_TIPO_PEDIMENTO).Valor
                        _pedimento.Attribute(CA_CVE_PEDIMENTO).Valor = .Attribute(CA_CVE_PEDIMENTO).Valor
                        _pedimento.Attribute(CA_CVE_PEDIMENTO).ValorPresentacion = .Attribute(CA_CVE_PEDIMENTO).ValorPresentacion
                        _pedimento.Attribute(CA_REGIMEN).Valor = .Attribute(CA_REGIMEN).Valor
                        _pedimento.Attribute(CA_REGIMEN).ValorPresentacion = .Attribute(CA_REGIMEN).ValorPresentacion
                        _pedimento.Attribute(CP_TIPO_DESPACHO).Valor = .Attribute(CamposReferencia.CP_TIPO_DESPACHO).Valor
                        _pedimento.Attribute(CA_CLAVE_SAD).Valor = .Attribute(CA_CLAVE_SAD).Valor
                        _pedimento.Attribute(CA_CLAVE_SAD).ValorPresentacion = aduanaPresentacion_
                        _pedimento.Attribute(CA_ADUANA_ENTRADA_SALIDA).Valor = .Attribute(CA_CLAVE_SAD).Valor
                        _pedimento.Attribute(CA_ADUANA_ENTRADA_SALIDA).ValorPresentacion = aduanaPresentacion_
                        _pedimento.Attribute(CA_PATENTE).Valor = .Attribute(CA_PATENTE).Valor
                        _pedimento.Attribute(CA_PATENTE).ValorPresentacion = .Attribute(CA_PATENTE).ValorPresentacion
                        _pedimento.Attribute(CP_NUMERO_CLIENTE).Valor = .Attribute(CamposClientes.CP_CVE_CLIENTE).Valor
                        _pedimento.Attribute(CA_RAZON_SOCIAL_IOE).Valor = .Attribute(CamposClientes.CA_RAZON_SOCIAL).Valor
                        _pedimento.Attribute(CA_RAZON_SOCIAL_IOE).ValorPresentacion = .Attribute(CamposClientes.CA_RAZON_SOCIAL).ValorPresentacion
                        _pedimento.Attribute(CA_RFC_IOE).Valor = .Attribute(CamposClientes.CA_RFC_CLIENTE).Valor
                        _pedimento.Attribute(CP_MODALIDAD).Valor = .Attribute(CP_MODALIDAD).Valor
                        _pedimento.Attribute(CP_MODALIDAD_ADUANA_PATENTE).Valor = .Attribute(CP_MODALIDAD_ADUANA_PATENTE).Valor
                        _pedimento.Attribute(CP_MODALIDAD_ADUANA_PATENTE).ValorPresentacion = .Attribute(CP_MODALIDAD_ADUANA_PATENTE).ValorPresentacion
                        _pedimento.Attribute(CP_EJECUTIVO_CUENTA).Valor = .Attribute(CP_EJECUTIVO_CUENTA).Valor
                        _pedimento.Attribute(CP_EJECUTIVO_CUENTA).ValorPresentacion = .Attribute(CP_EJECUTIVO_CUENTA).ValorPresentacion
                        _pedimento.Attribute(CA_ANIO_VALIDACION).Valor = Year(Now).ToString
                        _pedimento.Attribute(CP_ANIO_CURSO).Valor = Year(Now).ToString

                        If _pedimento.Attribute(CA_TIPO_OPERACION).Valor = IRecursosSistemas.TipoOperacion.Importacion Then

                            _pedimento.Attribute(CA_FECHA_ENTRADA).Valor = .Attribute(CA_FECHA_ENTRADA).Valor

                        Else

                            _pedimento.Attribute(CA_FECHA_PRESENTACION).Valor = .Attribute(CamposReferencia.CP_FECHA_PRESENTACION).Valor

                        End If

                        _pedimento.ObjectIdPropietario = .Attribute(CamposClientes.CP_OBJECTID_CLIENTE).Valor

                        _pedimento.DocumentosAsociados = New List(Of DocumentoAsociado) From {
                        New DocumentoAsociado With {
                        ._iddocumentoasociado = referencia_.Id,
                        .identificadorrecurso = "ConstructorReferencia",
                        .idcampo = CamposReferencia.CP_REFERENCIA,
                        .idsection = SeccionesReferencias.SREF1,
                        .firmaelectronica = referencia_.FirmaElectronica
                        }}

                        'Generar secuencia
                        secuencia_ = GenerarSecuenciaPedimento(_pedimento)

                        If secuencia_ IsNot Nothing Then

                            If secuencia_.sec > 0 Then

                                'Crear número de pedimento
                                _pedimento.Attribute(CA_NUMERO_PEDIMENTO).Valor = Mid(_pedimento.Attribute(CP_ANIO_CURSO).Valor, 4, 1) + Microsoft.VisualBasic.Format(CLng(secuencia_.sec), "000000")

                                'Crear número de pedimento completo
                                _pedimento.Attribute(CA_NUMERO_PEDIMENTO_COMPLETO).Valor = CrearNumeroPedimentoCompleto(Mid(_pedimento.Attribute(CA_ANIO_VALIDACION).Valor, 3, 2),
                                                                                                                                        Mid(_pedimento.Attribute(CA_CLAVE_SAD).Valor, 1, 2),
                                                                                                                                        _pedimento.Attribute(CA_PATENTE).Valor,
                                                                                                                                        _pedimento.Attribute(CA_NUMERO_PEDIMENTO).Valor
                                                                                                                                        )

                            Else

                                _estatus.SetError(Me, "Se obtuvo una secuencia igual con 0, favor de revisar sus datos.")

                            End If

                        Else

                            _estatus.SetError(Me, "No se tiene la secuencia de pedimento, el controlador no responde.")

                        End If


                    Case COMPLEMENTARIO 'Complementario

                    Case TRANSITO 'Tránsito

                    Case RECTIFICACION 'Rectificación

                    Case GLOBALCOMPLEMENTARIO 'Global complementario

                    Case Else

                End Select

                If _estatus.Status = TagWatcher.TypeStatus.Ok Then

                    _estatus.ObjectReturned = _pedimento

                    _estatus.SetOK()

                Else

                    _estatus.SetError(Me, "No se pueden procesar las referencias recibidas.")

                End If

            Else

                _estatus.SetError(Me, "La referencia que se recibio esta vacía y en borrador.")

            End If

        End With

        Return _pedimento

    End Function

    Private Function GenerarSecuenciaPedimento(ByVal pedimento_ As DocumentoElectronico) As Secuencia

        Dim controladorSecuencias_ = New ControladorSecuencia

        Dim secuencia_ = New Secuencia With {.nombre = SecuenciasComercioExterior.Pedimentos.ToString,
            .environment = 0,
            .anio = Year(Now),
            .mes = 0,
            .area = 0,
            .compania = 0,
            .tiposecuencia = 0,
            .subtiposecuencia = 0,
            .prefijo = pedimento_.Attribute(CA_PATENTE).Valor,
            .sufijo = Mid(pedimento_.Attribute(CA_CLAVE_SAD).Valor, 1, 2),
            .estado = 1
        }

        _estatus = controladorSecuencias_.Generar(secuencia_, Nothing)

        With _estatus

            If .Status = TypeStatus.Ok Then

                secuencia_ = DirectCast(.ObjectReturned, Secuencia)

            End If

        End With

        Return secuencia_

    End Function

    Private Function CrearNumeroPedimentoCompleto(ByVal anioValidacion_ As String,
                                                  ByVal aduanaDespacho_ As String,
                                                  ByVal patente_ As String,
                                                  ByVal numeroPedimento_ As String) As String

        Dim numeroPedimentoCompleto_ As String = anioValidacion_.PadRight(4) + aduanaDespacho_.PadRight(4) + patente_.PadRight(6) + numeroPedimento_

        Return numeroPedimentoCompleto_

    End Function

    Private Async Function GuardarPedimentoAsync(ByVal pedimento_ As DocumentoElectronico,
                                                 ByVal tipoPedimento_ As IControladorPedimentos.TiposPedimento,
                                                 ByVal session_ As IClientSessionHandle,
                                                 ByVal iEnlace_ As IEnlaceDatos) As Task(Of TagWatcher)

        _pedimento = pedimento_

        Dim result_ As Task = Nothing

        'Crear operación generica
        If _pedimento IsNot Nothing Then

            With _pedimento
                .FolioOperacion = _pedimento.Attribute(CA_NUMERO_PEDIMENTO_COMPLETO).Valor
                .NombreCliente = _pedimento.Attribute(CA_RAZON_SOCIAL_IOE).ValorPresentacion
                .FolioDocumento = _pedimento.Attribute(CP_REFERENCIA).Valor
            End With

            Dim operacionGenericaPedimento_ As New OperacionGenerica(_pedimento) With {
                            .FolioOperacion = _pedimento.FolioOperacion,
                            .Id = New ObjectId
                        }

            If _estatus.Status = TypeStatus.Ok Then

                Select Case tipoPedimento_

                    Case NORMAL

                        Using entidadDatos_ As IEntidadDatos = New ConstructorPedimentoNormal()

                            Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorPedimentoNormal).GetType.Name)

                            Await operationsDB_.InsertOneAsync(session_, operacionGenericaPedimento_).ConfigureAwait(False)

                            Dim filter_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, operacionGenericaPedimento_.Id)

                            result_ = operationsDB_.CountDocumentsAsync(filter_)

                        End Using

                    Case COMPLEMENTARIO

                            'No implementado

                    Case TRANSITO

                            'No implementado

                    Case RECTIFICACION

                            'No implementado

                    Case GLOBALCOMPLEMENTARIO

                        'No implementado

                    Case Else

                End Select

                With _estatus

                    If result_.Id <> 0 Then

                        .ObjectReturned = result_

                        .SetOK()

                    Else

                        .SetOKBut(Me, "Hubo un detalle al insertar el pedimento." + _pedimento.Attribute(CA_NUMERO_PEDIMENTO_COMPLETO).Valor)

                    End If

                End With

            Else

                _estatus.SetError(Me, "No se pudo guardar el pedimento: " + _pedimento.Attribute(CA_NUMERO_PEDIMENTO_COMPLETO).Valor)

            End If

        Else

            _estatus.SetError(Me, "No se tiene pedimento, verifique su información.")

        End If

        Return _estatus

    End Function

    Private Async Function ActualizarPedimentoReferenciaAsync(ByVal idReferencia_ As ObjectId,
                                                         ByVal session_ As IClientSessionHandle,
                                                         ByVal iEnlace_ As IEnlaceDatos) As Task(Of TagWatcher)

        Dim result_ As Task = Nothing

        'Crear operación generica
        If idReferencia_ <> Nothing Then

            If _estatus.Status = TypeStatus.Ok Then

                Using entidadDatos_ As IEntidadDatos = New ConstructorReferencia()

                    Dim operationsDB_ = iEnlace_.GetMongoCollection(Of OperacionGenerica)((New ConstructorReferencia).GetType.Name)

                    Dim filter_ = Builders(Of OperacionGenerica).Filter.Eq(Function(x) x.Id, idReferencia_)

                    Dim setStructureOfSubs_ = Builders(Of OperacionGenerica).Update.
                        Set(Of String)("Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts.Encabezado.0.Nodos.0.Nodos.1.Nodos.0.Valor", _pedimento.FolioOperacion)

                    Await operationsDB_.UpdateOneAsync(session_, filter_, setStructureOfSubs_).ConfigureAwait(False)

                    result_ = operationsDB_.CountDocumentsAsync(filter_)

                End Using

                With _estatus

                    If result_.Id <> Nothing Then

                        .ObjectReturned = result_

                        .SetOK()

                    Else

                        .SetOKBut(Me, "Hubo un detalle al actualizar el numero de pedimento completo en la referencia.")

                    End If

                End With

            Else

                _estatus.SetError(Me, "No se pudo actualizar el numero de pedimento en la referencia.")

            End If

        Else

            _estatus.SetError(Me, "No se tiene referencia, verifique su información.")

        End If

        Return _estatus

    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not disposedValue Then

            If disposing Then

                ' TODO: eliminar el estado administrado (objetos administrados)
                _estatus = Nothing

                _tipoPedimento = Nothing

                _pedimento = Nothing

                _pedimentos = Nothing

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

#End Region

End Class