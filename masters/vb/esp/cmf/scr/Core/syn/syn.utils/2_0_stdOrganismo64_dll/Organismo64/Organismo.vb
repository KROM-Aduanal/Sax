﻿Imports System.Web
Imports gsol.krom
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports Rec.Globals.Utils
Imports Syn.Documento
Imports Syn.Documento.Componentes
Imports Syn.Documento.Componentes.Nodo
Imports MongoDB.Bson.Serialization
Imports System.Globalization
Imports System.Text
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports PdfiumViewer

Public Class Organismo
    Inherits System.Web.UI.Page

#Region "Enum"
    Public Enum Datos
        SinDefinir = 0
        PaginaReciente = 1
        OficinaReciente = 2
        PerfilUsuario = 3
        SessionID = 4
    End Enum

    Public Enum Cookies
        MiSesion = 1
        MiCache = 2

    End Enum


    Public Enum Modalidad
        Intrinseco = 1
        Extrinseco = 2
    End Enum

#End Region

#Region "Constructores"

    Sub New()

    End Sub

#End Region

#Region "Propiedades"

#End Region

#Region "Métodos"


    Public Function Preferencias(ByVal variableGlobal_ As Cookies,
                                 Optional ByVal eliminar_ As Boolean = False) As HttpCookie

        If eliminar_ Then
            'misdatos_
            Response.Cookies.Item(variableGlobal_.ToString).Value = Nothing
            Response.Cookies.Remove(variableGlobal_.ToString)
            Response.Cookies.Clear()
            Response.Cookies.Add(New HttpCookie(variableGlobal_.ToString, ""))
            Response.Cookies(variableGlobal_.ToString).Expires = DateTime.Now.AddDays(-1)
            Session.Abandon()

            Return Nothing

        Else

            Return Request.Cookies(variableGlobal_.ToString)

        End If

    End Function
    Public Function Preferencias(ByVal variableGlobal_ As Cookies,
                                          ByVal dato_ As Datos,
                                          ByVal crearSiNoExiste_ As Boolean,
                                          Optional ByVal valorAsignado_ As Object = Nothing) As HttpCookie
        Return PreferenciasUsuario(variableGlobal_, dato_, crearSiNoExiste_, valorAsignado_)


    End Function

    Function PreferenciasUsuario(ByVal variableGlobal_ As Cookies,
                                 ByVal dato_ As Datos,
                                 ByVal crearSiNoExiste_ As Boolean,
                                 Optional ByVal valorAsignado_ As Object = Nothing,
                                 Optional ByVal eliminar_ As Boolean = False) As HttpCookie

        Dim cookie_ As HttpCookie

        If eliminar_ Then
            'misdatos_
            Response.Cookies.Item(variableGlobal_.ToString).Value = Nothing

            Response.Cookies.Remove(variableGlobal_.ToString)

            Response.Cookies.Clear()

            Response.Cookies.Add(New HttpCookie(variableGlobal_.ToString, ""))

            Response.Cookies(variableGlobal_.ToString).Expires = DateTime.Now.AddDays(-1)

            Session.Abandon()

            Return Nothing

        End If

        cookie_ = Request.Cookies(variableGlobal_.ToString)

        If crearSiNoExiste_ Then

            If cookie_ Is Nothing Then
                'No existe
                cookie_ = New HttpCookie(variableGlobal_.ToString)

                cookie_.Values.Add(dato_.ToString, valorAsignado_)

                cookie_.Expires = DateTime.MaxValue 'Nunca caduca

                System.Web.HttpContext.Current.Response.AppendCookie(cookie_)
            Else
                'Existe Request.RawUrl
                cookie_ = Request.Cookies(variableGlobal_.ToString)

                cookie_.Values.Set(dato_.ToString, valorAsignado_)

                cookie_.Expires = DateTime.MaxValue 'Nunca caduca

                Response.Cookies.Set(cookie_)

            End If

        End If

        Return cookie_

    End Function

    Public Shared Function ObtenerNombrePagina(ByVal pagina_ As String) As String

        Dim indice_ As Integer, finindice_ As Integer

        Dim nombrePagina_ As String = ""

        indice_ = pagina_.LastIndexOf("/") 'Encontrar el ultimo / para determinar donde inicia el nombre

        indice_ += 1

        finindice_ = pagina_.LastIndexOf("?")

        If (finindice_ = -1) Then 'No hay parametros se toma todo el tamaño de la cadena

            finindice_ = pagina_.Length

        End If

        nombrePagina_ = pagina_.Substring(indice_, (finindice_ - indice_))

        Return nombrePagina_

    End Function

    Public Sub GetHistoricalUser(Of T As {Class, New})(historical As List(Of T), ByRef accionDate_ As List(Of String), ByRef userName_ As List(Of String), userLimit_ As Int32)

        Dim cuenta_ = 1

        For Each roomhstory_ In historical

            If cuenta_ <= userLimit_ Then

                Dim user As T = TryCast(roomhstory_, T)

                If user IsNot Nothing Then

                    userName_(cuenta_ - 1) = DirectCast(GetType(T).GetProperty("username").GetValue(user), String)

                    accionDate_(cuenta_ - 1) = DirectCast(GetType(T).GetProperty("createat").GetValue(user), String)

                End If

            Else

                Exit For

            End If


            cuenta_ += 1

        Next

    End Sub

    Public Function DateDiffUX(initial_ As DateTime, final_ As DateTime, Optional tipoConstruccion_ As Integer = 1) As String 'DateDiffUX param tipo de construcción, 


        Dim pastedTime As TimeSpan

        Dim currentUser_ As String


        pastedTime = final_ - initial_

        Select Case tipoConstruccion_

            Case 1

                If pastedTime.Days > 0 Then

                    If pastedTime.Days = 1 Then

                        currentUser_ = "Hace 1 día"

                    Else

                        currentUser_ = "Hace " & pastedTime.Days & " días"

                    End If

                Else

                    If pastedTime.Hours > 0 Then

                        If pastedTime.Hours = 1 Then

                            currentUser_ = "Hace 1 hr"

                        Else

                            currentUser_ = "Hace " & pastedTime.Hours & " hrs"

                        End If

                    Else

                        If pastedTime.Minutes > 0 Then

                            If pastedTime.Minutes = 1 Then

                                currentUser_ = "Hace 1 min"

                            Else

                                currentUser_ = "Hace " & pastedTime.Minutes & " mins"

                            End If

                        Else


                            If pastedTime.Seconds = 1 Then

                                currentUser_ = "Hace 1s"

                            Else

                                currentUser_ = "Hace " & pastedTime.Seconds & "s"

                            End If

                        End If

                    End If

                End If

            Case Else

                currentUser_ = "Hace 1s"

        End Select



        Return currentUser_

    End Function

    Public Function SeparacionPalabras(oracion_ As String,
                                       campo_ As String,
                                       anexo_ As String,
                                       valoranexo_ As String,
                                       tipo_ As String) As String

        Dim sentencia_ As String

        Dim arreglopalabras_ = New ArrayList

        Dim arreglopalabras2_ = New ArrayList

        Dim palabras_ As String()

        If tipo_ = "itext" Then

            sentencia_ = "{$text:{$search:" & Chr(34) & Chr(92) & Chr(34) &
                              String.Join(Chr(92) & Chr(34) & " " & Chr(92) & Chr(34), oracion_.Split(" ")) &
                     Chr(92) & Chr(34) & Chr(34) & "}}"
        Else

            oracion_ = oracion_.Replace("a", "[Á,Ä,À,Â,Ã,a,á,à,ä,ã,â,å,ã]").Replace("e", "[É,Ë,È,Ê,ê,e,é,è,ë]").Replace("i", "[Ì,Í,Î,Ï,i,í,ì,ï,î,]").Replace("o", "[Ó,Ô,Ò,Õ,Ö,o,ó,ò,ö,ô,ð,õ]").Replace("u", "[Ú,Û,Ù,Ü,u,ú,ù,ü,û,]").Trim

            palabras_ = oracion_.Split(" ")

            For Each palabra_ In palabras_

                Dim pos_ = arreglopalabras2_.IndexOf(palabra_)

                If pos_ = -1 Then

                    arreglopalabras_.Add(palabra_)

                    arreglopalabras2_.Add(palabra_)

                Else

                    arreglopalabras_(pos_) = arreglopalabras_(pos_) & ".*" & palabra_

                End If

            Next

            sentencia_ = ""

            For Each elemento_ In arreglopalabras_

                If elemento_.IndexOf("*") >= 0 Then

                    sentencia_ = sentencia_ & "{" & campo_ & ":{$regex:'" & elemento_ & "', $options:'si'}},"

                Else

                    If elemento_.length = 3 Then

                        sentencia_ = sentencia_ & "{" & campo_ & ":{$regex:'^" & elemento_ & " ', $options:'i'}},"

                    Else

                        sentencia_ = sentencia_ & "{" & campo_ & ":{$regex:'" & elemento_ & "', $options:'i'}},"

                    End If

                End If

            Next

            If sentencia_ <> "" Then

                If anexo_ <> "" Then

                    sentencia_ = sentencia_ & "{" & anexo_ & ": " & valoranexo_ & "},"

                End If

                sentencia_ = "{$and:[" & sentencia_.Substring(0, sentencia_.Length - 1) & "]}"

            End If

        End If

        Return sentencia_

    End Function




    Public Function ObtenerSelectOption(SelectControl_ As Object,
                                        ListaSelectOption_ As List(Of ValorProvisionalOption)) As List(Of SelectOption)

        Dim ultimoValor_ As ValorProvisionalOption

        Dim temporal_ As New List(Of SelectOption)

        Dim cuenta_ As Int16 = 0

        If SelectControl_.Value = "" Then

            ultimoValor_ = Nothing

        Else

            ultimoValor_ = New ValorProvisionalOption With {.Id = New ObjectId(SelectControl_.Value.ToString),
                                                            .Valor = SelectControl_.Text.ToString.ToUpper}

        End If

        If ultimoValor_ IsNot Nothing Then

            If ListaSelectOption_.Find(Function(e) e.Id = ultimoValor_.Id) Is Nothing Then

                temporal_.Add(New SelectOption With {.Value = ultimoValor_.Id.ToString,
                                                     .Indice = cuenta_,
                                                     .Text = ultimoValor_.Valor.ToUpper})

                cuenta_ += 1

            End If

        End If

        For Each SelectOption_ In ListaSelectOption_

            temporal_.Add(New SelectOption With {.Value = SelectOption_.Id.ToString,
                                                 .Indice = cuenta_,
                                                 .Text = SelectOption_.Valor.ToUpper})

            cuenta_ += 1

        Next

        Return temporal_

    End Function

    Public Function ObtenerSelectOption(elementList_ As List(Of String)) As List(Of SelectOption)

        Dim temporal_ As New List(Of SelectOption)

        Dim cuenta_ As Int16 = 0


        For Each element_ In elementList_

            temporal_.Add(New SelectOption With {.Value = cuenta_,
                                                 .Indice = cuenta_,
                                                 .Text = element_})

            cuenta_ += 1

        Next

        Return temporal_

    End Function

    Public Function GetPredicates(sentence_ As String, Optional separator_ As String = " ") As Predicate(Of String)

        Dim listString_ = sentence_.ToString.Split(separator_)

        Dim listmatch_ As New List(Of Predicate(Of String))

        For Each string_ In listString_

            listmatch_.Add(Function(ch) ch.Contains(string_))

        Next

        Return CombinePredicates(listmatch_)

    End Function

    Public Function CombinePredicates(Of T)(predicates As List(Of Predicate(Of T))) As Predicate(Of T)

        Return Function(item) predicates.All(Function(p) p(item))

    End Function

    Public Function ObtenerRutaCampo(ByVal documentoElectronico_ As DocumentoElectronico,
                                     idUnicoSeccion_ As Integer,
                                     idUnicoCampo_ As Integer) As String

        For Each parDatos_ As KeyValuePair(Of String, List(Of Nodo)) In documentoElectronico_.EstructuraDocumento.Parts

            Dim rutaSeccion_ = BuscarRutaNodo(parDatos_.Value,
                                              idUnicoSeccion_,
                                              idUnicoCampo_,
                                              Nodo.TiposNodo.Nodo)

            If rutaSeccion_ <> "" Then

                Return parDatos_.Key & rutaSeccion_

            End If

        Next

        Return Nothing

    End Function

    Private Function BuscarRutaNodo(ByVal nodos_ As List(Of Nodo),
                                    ByVal idUnico_ As Integer,
                                    ByVal idUnicoCampo_ As Integer,
                                    ByVal tipoNodo_ As TiposNodo,
                                    Optional indice_ As Int32 = 0) As String

        If nodos_ IsNot Nothing Then

            For Each nodoContexto_ As Nodo In nodos_

                Select Case nodoContexto_.TipoNodo

                    Case TiposNodo.Campo

                        Dim campo_ = CType(nodoContexto_, DecoradorCampo)

                        If campo_.IDUnico = idUnicoCampo_ Then

                            Return "(" & indice_ & ")SI"

                        End If

                    Case TiposNodo.Seccion

                        Dim seccion_ = CType(nodoContexto_, DecoradorSeccion)

                        If seccion_.IDUnico = idUnico_ Then

                            If idUnicoCampo_ = 0 Then

                                Return "(" & indice_ & ")SI"
                            Else

                                Return "(" & indice_ & ").Nodos" & BuscarRutaNodo(nodoContexto_.Nodos,
                                                                                  idUnico_,
                                                                                  idUnicoCampo_,
                                                                                  TiposNodo.Nodo)

                            End If
                            'Else
                            '    If seccion_.Nodos IsNot Nothing Then
                            '        If seccion_.Nodos.Count > 0 Then
                            '            Return "(" & indice_ & ").Nodos" & BuscarRutaNodo(seccion_.Nodos,
                            '                                                          idUnico_,
                            '                                                          idUnicoCampo_,
                            '                                                          TiposNodo.Nodo)

                            '        End If
                            '    End If

                        End If


                    Case TiposNodo.Nodo

                        Dim respuesta_ = BuscarRutaNodo(nodoContexto_.Nodos,
                                                        idUnico_,
                                                        idUnicoCampo_,
                                                        TiposNodo.Nodo)

                        If respuesta_ IsNot Nothing Then

                            If respuesta_.Contains("SI") Then

                                Return "(" & indice_ & ").Nodos" & respuesta_

                            Else

                                Return ""

                            End If

                        End If

                End Select

                indice_ += 1

            Next

        Else

            Return ""

        End If

    End Function

    ' ESTÁ OBSOLETA
    Public Function RemplazaNodo(DocumentoId_ As ObjectId,
                                 ByVal documentoElectronico_ As DocumentoElectronico,
                                 seccion_ As Int32) As String

        Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
             {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Dim operationsDB_ As IMongoCollection(Of BsonDocument) =
                _enlaceDatos.GetMongoCollection(Of BsonDocument)(documentoElectronico_.GetType.Name)

            Dim match_ As String = "{'_id':ObjectId('" & DocumentoId_.ToString & "'),'Nodillo.Nodos':{$exists:true}}"

            Dim numcampo_ As Int32 = 1

            Dim ruta_ = ObtenerRutaCampo(documentoElectronico_, seccion_, 0)

            ruta_ = ruta_.Substring(0, ruta_.Length - 2)

            Dim puntos_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                            ruta_.Replace("(", ".").Replace(")", "") & ".Nodos"

            Dim corchete_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                               ruta_.Replace("(", "[").Replace(")", "]") & ".Nodos[0]"

            Dim consulta_ As String = "{NodoAux:" & ObtenerRutaMongo(ObtenerRutaCampo(documentoElectronico_, seccion_, 0)) & "}"

            Dim fieldValue_ As BsonValue = Nothing

            operationsDB_.Aggregate().Project(BsonDocument.Parse(consulta_)).
                                      Project(BsonDocument.Parse("{NodoAux:1," & "NN:{$type:'$NodoAux.Nodos.0'}}")).
                                      Project(BsonDocument.Parse("{NodoAux:1," & "NN:{$cond:{if:{$eq:['$NN','array']}, then: '$NodoAux.Nodos'," &
                                                                                           " else:['$NodoAux.Nodos.0']}}}")).
                                      Match(BsonDocument.Parse(match_)).
                                      ToList().ForEach(Sub(estatus_)

                                                           fieldValue_ = estatus_.GetValue("NN")

                                                       End Sub)
            If fieldValue_ IsNot Nothing Then

                Dim filter_ As BsonDocument = New BsonDocument("_id", New BsonObjectId(DocumentoId_))

                Dim updateField_ As BsonDocument = New BsonDocument(puntos_, fieldValue_)

                ' Crear el objeto de actualización
                Dim update_ As BsonDocument = New BsonDocument("$set", updateField_)

                ' Realizar la actualización
                operationsDB_.UpdateOne(filter_, update_)

            End If

        End Using

    End Function

    ' ESTÁ OBSOLETA
    Public Function RemplazaNodo(OperacionGenerica_ As OperacionGenerica,
                                 ByVal documentoElectronico_ As DocumentoElectronico,
                                 seccion_ As Int32, modalidad_ As Modalidad) As OperacionGenerica

        If modalidad_ = Modalidad.Extrinseco Then

            Using _enlaceDatos As IEnlaceDatos = New EnlaceDatos With
             {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

                Dim operationsDB_ As IMongoCollection(Of BsonDocument) =
                        _enlaceDatos.GetMongoCollection(Of BsonDocument)(documentoElectronico_.GetType.Name)

                Dim match_ As String = "{'_id':ObjectId('" & OperacionGenerica_.Id.ToString & "')}"

                Dim numcampo_ As Int32 = 1

                Dim ruta_ = ObtenerRutaCampo(documentoElectronico_, seccion_, 0)

                ruta_ = ruta_.Substring(0, ruta_.Length - 2)

                Dim puntos_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                               ruta_.Replace("(", ".").Replace(")", "") & ".Nodos"

                Dim corchete_ = "Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts." &
                                ruta_.Replace("(", "[").Replace(")", "]") & ".Nodos[0]"

                Dim consulta_ As String = "{NodoAux:" & ObtenerRutaMongo(ObtenerRutaCampo(documentoElectronico_, seccion_, 0)) & "}"

                Dim fieldValue_ As BsonValue

                operationsDB_.Aggregate().Project(BsonDocument.Parse(consulta_)).
                                          Project(BsonDocument.Parse("{NodoAux:1," & "NN:{$type:'$NodoAux.Nodos.0'}}")).
                                          Project(BsonDocument.Parse("{NodoAux:1," & "NN:{$cond:{if:{$eq:['$NN','array']}, " &
                                                                     "then: '$NodoAux.Nodos', else:['$NodoAux.Nodos.0']}}}")).
                                          Match(BsonDocument.Parse(match_)).
                                          ToList().ForEach(Sub(estatus_)

                                                               fieldValue_ = estatus_.GetValue("NN")

                                                           End Sub)

                Dim filter_ As BsonDocument = New BsonDocument("_id", New BsonObjectId(OperacionGenerica_.Id))

                Dim updateField_ As BsonDocument = New BsonDocument(puntos_, fieldValue_)

                ' Crear el objeto de actualización
                Dim update_ As BsonDocument = New BsonDocument("$set", updateField_)

                ' Realizar la actualización

                operationsDB_.UpdateOne(filter_, update_)

            End Using

            Dim Nodo_ = OperacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.
                        EstructuraDocumento.Parts.Item("Cuerpo")(2).Nodos(0).Nodos(0).Nodos

            OperacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.
             EstructuraDocumento.Parts.Item("Cuerpo")(2).Nodos(0).Nodos = Nodo_

        Else

            Dim Nodo_ = OperacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.
                        EstructuraDocumento.Parts.Item("Cuerpo")(2).Nodos(0).Nodos(0).Nodos

            OperacionGenerica_.Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.
                               EstructuraDocumento.Parts.Item("Cuerpo")(2).Nodos(0).Nodos = Nodo_

        End If

        Return OperacionGenerica_

    End Function
    Private Function ObtenerRutaMongo(ruta_ As String) As String

        Dim indiceInicial_, indiceFinal_ As Int32

        Dim temporal_ = ruta_

        Dim instruccionMongo_ = "'$Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.Documento.Parts"

        Dim posiciones_ As New List(Of String)

        While temporal_ <> ""

            indiceInicial_ = temporal_.IndexOf("(")

            indiceFinal_ = temporal_.IndexOf(")")

            If indiceInicial_ < 0 Then

                temporal_ = ""

            Else

                posiciones_.Add(temporal_.Substring(indiceInicial_ + 1, indiceFinal_ - indiceInicial_ - 1))

                instruccionMongo_ &= "." & temporal_.Substring(0, indiceInicial_)

                If indiceFinal_ + 2 > temporal_.Length Then

                    temporal_ = ""

                Else

                    temporal_ = temporal_.Substring(indiceFinal_ + 2, temporal_.Length - indiceFinal_ - 2)

                End If

            End If

        End While

        instruccionMongo_ &= "'"

        For Each posicion_ In posiciones_

            instruccionMongo_ = "{$arrayElemAt:[" & instruccionMongo_ & "," & posicion_ & "]}"

        Next

        Return instruccionMongo_

    End Function
    'dim f1 as Costrucu = Obtee(of ConstructorFacCom)()
    Public Function ObtenerCamposSeccionExterior(listaDocumentoId_ As List(Of ObjectId),
                                                 ByVal documentoElectronico_ As DocumentoElectronico,
                                                 listaSeccion_ As Dictionary(Of [Enum], List(Of [Enum]))) As Dictionary(Of ObjectId, List(Of Nodo))

        'Dim respuesta_ As New TagWatcher

        'respuesta_.SetOK()
        'respuesta_.ObjectReturn = DirectCast(1, T)


        Dim bulkCamposPedidos_ As Dictionary(Of ObjectId, List(Of Nodo))

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos With
            {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Dim consulta_ As String = ""

            Dim match_ As String = ""

            Dim operationsDB_ As IMongoCollection(Of BsonDocument) = enlaceDatos_.
                                  GetMongoCollection(Of BsonDocument)(documentoElectronico_.GetType.Name)

            Dim numeroCampo_ As Int32 = 1

            For Each seccion_ As KeyValuePair(Of [Enum], List(Of [Enum])) In listaSeccion_

                If seccion_.Value Is Nothing Then

                    Dim seccionAuxiliar_ As Object = seccion_.Key

                    consulta_ = consulta_ & "campo" & numeroCampo_ & ":" &
                                ObtenerRutaMongo(ObtenerRutaCampo(documentoElectronico_,
                                                          seccionAuxiliar_, 0)) & ","

                    numeroCampo_ += 1

                Else

                    For Each campo_ In seccion_.Value

                        Dim campoEntero_ As Object = campo_

                        Dim seccionEntero_ As Object = seccion_.Key

                        consulta_ = consulta_ & "campo" & numeroCampo_ & ":" &
                                    ObtenerRutaMongo(ObtenerRutaCampo(documentoElectronico_,
                                                                      seccionEntero_,
                                                                      campoEntero_)) & ","

                        numeroCampo_ += 1

                    Next

                End If

            Next

            Dim algo_ = consulta_
            consulta_ = "{" & consulta_.Substring(0, consulta_.Length - 1) & "}"

            bulkCamposPedidos_ = New Dictionary(Of ObjectId, List(Of Nodo))

            For Each documentoId_ In listaDocumentoId_

                match_ &= "ObjectId('" & documentoId_.ToString & "'),"

                bulkCamposPedidos_.Add(documentoId_, New List(Of Nodo))

            Next

            match_ = "{'_id':{$in:[" & match_.Substring(0, match_.Length - 1) & "]}}"

            operationsDB_.Aggregate().
                          Project(BsonDocument.Parse(consulta_)).Match(BsonDocument.Parse(match_)).
                          ToList().ForEach(Sub(estatus_)

                                               For cadena = 1 To estatus_.ElementCount - 1

                                                   bulkCamposPedidos_(New ObjectId(estatus_.GetElement("_id").Value.ToString)).
                                                   Add(BsonSerializer.Deserialize(Of Nodo)(estatus_.GetElement("campo" & cadena).Value.AsBsonDocument))

                                               Next

                                           End Sub)

        End Using

        Return bulkCamposPedidos_

    End Function

    Public Function ObtenerCamposSeccionExterior(listaDocumentoFolio_ As List(Of String),
                                                  ByVal documentoElectronico_ As DocumentoElectronico,
                                                  listaSecciones_ As Dictionary(Of [Enum], List(Of [Enum]))) As Dictionary(Of String, List(Of Nodo))

        Dim bulkCamposPedidos_ As Dictionary(Of String, List(Of Nodo))

        Using enlaceDatos_ As IEnlaceDatos = New EnlaceDatos With
            {.EspacioTrabajo = System.Web.HttpContext.Current.Session("EspacioTrabajoExtranet")}

            Dim consulta_ As String = ""

            Dim match_ As String = ""

            Dim operationsDB_ As IMongoCollection(Of BsonDocument) =
                enlaceDatos_.GetMongoCollection(Of BsonDocument)(documentoElectronico_.GetType.Name)

            Dim numeroCampo_ As Int32 = 1

            For Each seccion_ As KeyValuePair(Of [Enum], List(Of [Enum])) In listaSecciones_

                If seccion_.Value Is Nothing Then

                    Dim seccionAuxiliar_ As Object = seccion_.Key

                    consulta_ &= "campo" & numeroCampo_ & ":" &
                                  ObtenerRutaMongo(ObtenerRutaCampo(documentoElectronico_,
                                                                    seccionAuxiliar_, 0)) & ","

                    numeroCampo_ += 1

                Else

                    For Each campo_ In seccion_.Value

                        Dim campoAuxiliar_ As Object = campo_

                        Dim seccionAuxiliar_ As Object = seccion_.Key

                        consulta_ = consulta_ & "campo" & numeroCampo_ & ":" &
                                    ObtenerRutaMongo(ObtenerRutaCampo(documentoElectronico_,
                                                                      seccionAuxiliar_,
                                                                      campoAuxiliar_)) & ","

                        numeroCampo_ += 1

                    Next

                End If

            Next

            consulta_ = "{" & consulta_.Substring(0, consulta_.Length - 1) &
                        ", FolioDocumento:'$Borrador.Folder.ArchivoPrincipal.Dupla.Fuente.FolioDocumento'}"

            bulkCamposPedidos_ = New Dictionary(Of String, List(Of Nodo))

            For Each documentoFolio_ In listaDocumentoFolio_

                match_ &= "'" & documentoFolio_ & "',"

                bulkCamposPedidos_.Add(documentoFolio_, New List(Of Nodo))

            Next

            match_ = "{FolioDocumento:{$in:[" & match_.Substring(0, match_.Length - 1) & "]}}"

            operationsDB_.Aggregate().
                          Project(BsonDocument.Parse(consulta_)).Match(BsonDocument.Parse(match_)).
                          ToList().ForEach(Sub(estatus_)

                                               If estatus_.ElementCount = 2 Then


                                                   bulkCamposPedidos_(estatus_.GetElement("FolioDocumento").Value.ToString).
                                                       Add(BsonSerializer.Deserialize(Of Nodo) _
                                                       (estatus_.GetElement("campo1").Value.AsBsonDocument))

                                               Else
                                                   For cadena_ = 1 To estatus_.ElementCount - 2

                                                       If estatus_.GetElement("campo" & cadena_).Value = BsonNull.Value Then

                                                           Dim nodoNull_ =
                                               BsonSerializer.Deserialize(Of Nodo)(estatus_.GetElement("campo1").Value.AsBsonDocument)

                                                           While nodoNull_.DescripcionTipoNodo <> "Campo"

                                                               nodoNull_ = nodoNull_.Nodos(0)

                                                           End While

                                                           DirectCast(nodoNull_, Campo).Valor = ""

                                                           DirectCast(nodoNull_, Campo).ValorPresentacion = ""

                                                           DirectCast(nodoNull_, Campo).Nombre = "campo" & cadena_

                                                           bulkCamposPedidos_(estatus_.GetElement("FolioDocumento").Value.ToString).Add(nodoNull_)


                                                       Else
                                                           bulkCamposPedidos_(estatus_.GetElement("FolioDocumento").Value.ToString).
                                                       Add(BsonSerializer.Deserialize(Of Nodo) _
                                                       (estatus_.GetElement("campo" & cadena_).Value.AsBsonDocument))

                                                       End If



                                                   Next

                                               End If


                                               Dim nodoId_ =
                                               BsonSerializer.Deserialize(Of Nodo)(estatus_.GetElement("campo1").Value.AsBsonDocument)

                                               While nodoId_.DescripcionTipoNodo <> "Campo"

                                                   nodoId_ = nodoId_.Nodos(0)

                                               End While

                                               DirectCast(nodoId_, Campo).Valor = estatus_.GetElement("_id").Value.ToString

                                               DirectCast(nodoId_, Campo).ValorPresentacion = estatus_.GetElement("_id").Value.ToString

                                               DirectCast(nodoId_, Campo).Nombre = "ID"

                                               bulkCamposPedidos_(estatus_.GetElement("FolioDocumento").Value.ToString).Add(nodoId_)

                                           End Sub)
        End Using

        Return bulkCamposPedidos_

    End Function

    Public Function ConvertirPDFaByte(pdfMemory As MemoryStream) As List(Of Byte())

        Dim listImage_ As New List(Of Byte())

        Using pdfDocument_ As PdfDocument = PdfDocument.Load(pdfMemory)

            For page_ As Integer = 0 To pdfDocument_.PageCount - 1

                Using image_ As Image = RenderPageToImage(pdfDocument_, page_, 450)

                    Using bitmap As New Bitmap(image_)

                        Using brightImage As Bitmap = AdjustBrightness(bitmap, 1.3)

                            Using newMemoryStream_ As New MemoryStream()

                                brightImage.Save(newMemoryStream_, ImageFormat.Png)

                                listImage_.Add(newMemoryStream_.ToArray())

                            End Using

                        End Using

                    End Using

                End Using

            Next

        End Using

        Return listImage_

    End Function

    Private Function RenderPageToImage(pdfDocument As PdfDocument, pageIndex As Integer, dpi As Integer) As Image

        Return pdfDocument.Render(pageIndex, dpi, dpi, PdfRenderFlags.Grayscale)

    End Function

    Private Function AdjustBrightness(original As Bitmap, brightness As Single) As Bitmap

        Dim adjustedImage As New Bitmap(original.Width, original.Height)

        Using g As Graphics = Graphics.FromImage(adjustedImage)

            ' Create color matrix
            Dim brightnessMatrix As Single()() = {
                New Single() {brightness, 0, 0, 0, 0},
                New Single() {0, brightness, 0, 0, 0},
                New Single() {0, 0, brightness, 0, 0},
                New Single() {0, 0, 0, 1, 0},
                New Single() {0, 0, 0, 0, 1}
            }

            Dim colorMatrix As New Imaging.ColorMatrix(brightnessMatrix)

            Dim imageAttributes As New Imaging.ImageAttributes()

            imageAttributes.SetColorMatrix(colorMatrix)

            g.DrawImage(original, New Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, imageAttributes)

        End Using

        Return adjustedImage

    End Function

    '    Public Function ActualizaMultiplesCampos(Of T)(builder_ As UpdateDefinitionBuilder(Of T), objeto_ As T) As UpdateDefinition(Of T)

    '        Dim propiedades_ = objeto_.GetType().GetProperties()
    '        Dim definicion_ As UpdateDefinition(Of T) = Nothing
    '        For Each propiedad_ In propiedades_

    '            If definicion_ Is Nothing Then
    '                definicion_ = Builders(Of T).Update.Set(Of T)(propiedad_.Name, propiedad_.GetValue(objeto_))
    '            Else
    '                definicion_ = definicion_.Set(Of T)(propiedad_.Name, propiedad_.GetValue(objeto_))
    '            End If
    '        Next
    '        Return definicion_

    '    End Function
#End Region

End Class

'Public Class SeccionesCamposGenerales
'    Public Property Seccion As Int32
'    Public Property Campos As List(Of Int32)
'    Public Property Valor As List(Of String)
'End Class

Public NotInheritable Class NumeroLetras

#Region "Miembros estáticos"

    Private Const UNI As Integer = 0, DIECI As Integer = 1, DECENA As Integer = 2, CENTENA As Integer = 3
    Private Shared _matriz As String(,) = New String(CENTENA, 9) {
            {Nothing, " uno", " dos", " tres", " cuatro", " cinco", " seis", " siete", " ocho", " nueve"},
            {" diez", " once", " doce", " trece", " catorce", " quince", " dieciséis", " diecisiete", " dieciocho", " diecinueve"},
            {Nothing, Nothing, Nothing, " treinta", " cuarenta", " cincuenta", " sesenta", " setenta", " ochenta", " noventa"},
            {Nothing, Nothing, Nothing, Nothing, Nothing, " quinientos", Nothing, " setecientos", Nothing, " novecientos"}}

    Private Shared _matriz2 As String(,) = New String(CENTENA, 9) {
            {Nothing, " one", " two", " three", " four", " five", " six", " seven", " eight", " nine"},
            {" ten", " eleven", " twelve", " thirteen", " fourteen", " fifteen", " sixteen", " seventeen", " seventeen", " nineteen"},
            {Nothing, Nothing, Nothing, " thirty ", " fourty", " fifty", " sixty", " seventy", " eighty", " ninety"},
            {Nothing, Nothing, Nothing, Nothing, Nothing, " five hundred", Nothing, " seven hundred", Nothing, " nine hundred"}} ' Andrea para importe en ingles

    Private Const [sub] As Char = CChar(ChrW(26))
    'Cambiar acá si se quiere otro comportamiento en los métodos de clase
    Public Const SeparadorDecimalSalidaDefault As String = "con"
    Public Const MascaraSalidaDecimalDefault As String = "00'/100.-'"
    Public Const DecimalesDefault As Int32 = 2
    Public Const LetraCapitalDefault As Boolean = False
    Public Const ConvertirDecimalesDefault As Boolean = False
    Public Const ApocoparUnoParteEnteraDefault As Boolean = False
    Public Const ApocoparUnoParteDecimalDefault As Boolean = False

#End Region

#Region "Propiedades"

    Private _decimales As Int32 = DecimalesDefault
    Private _cultureInfo As CultureInfo = Globalization.CultureInfo.CurrentCulture
    Private _separadorDecimalSalida As String = SeparadorDecimalSalidaDefault
    Private _posiciones As Int32 = DecimalesDefault
    Private _mascaraSalidaDecimal As String, _mascaraSalidaDecimalInterna As String = MascaraSalidaDecimalDefault
    Private _esMascaraNumerica As Boolean = True
    Private _letraCapital As Boolean = LetraCapitalDefault
    Private _convertirDecimales As Boolean = ConvertirDecimalesDefault
    Private _apocoparUnoParteEntera As Boolean = False
    Private _apocoparUnoParteDecimal As Boolean

    ''' <summary>
    ''' Indica la cantidad de decimales que se pasarán a entero para la conversión
    ''' </summary>
    ''' <remarks>Esta propiedad cambia al cambiar MascaraDecimal por un valor que empieze con '0'</remarks>
    Public Property Decimales() As Int32
        Get
            Return _decimales
        End Get
        Set(ByVal value As Int32)
            If value > 10 Then
                Throw New ArgumentException(value.ToString() + " excede el número máximo de decimales admitidos, solo se admiten hasta 10.")
            End If
            _decimales = value
        End Set
    End Property

    ''' <summary>
    ''' Objeto CultureInfo utilizado para convertir las cadenas de entrada en números
    ''' </summary>
    Public Property CultureInfo() As CultureInfo
        Get
            Return _cultureInfo
        End Get
        Set(ByVal value As CultureInfo)
            _cultureInfo = value
        End Set
    End Property

    ''' <summary>
    ''' Indica la cadena a intercalar entre la parte entera y la decimal del número
    ''' </summary>
    Public Property SeparadorDecimalSalida() As String
        Get
            Return _separadorDecimalSalida
        End Get
        Set(ByVal value As String)
            _separadorDecimalSalida = value
            'Si el separador decimal es compuesto, infiero que estoy cuantificando algo,
            'por lo que apocopo el "uno" convirtiéndolo en "un"
            If value.Trim().IndexOf(" ") > 0 Then
                _apocoparUnoParteEntera = True
            Else
                _apocoparUnoParteEntera = False
            End If
        End Set
    End Property

    ''' <summary>
    ''' Indica el formato que se le dara a la parte decimal del número
    ''' </summary>
    Public Property MascaraSalidaDecimal() As String
        Get
            If Not [String].IsNullOrEmpty(_mascaraSalidaDecimal) Then
                Return _mascaraSalidaDecimal
            Else
                Return ""
            End If
        End Get
        Set(ByVal value As String)
            'determino la cantidad de cifras a redondear a partir de la cantidad de '0' o ''
            'que haya al principio de la cadena, y también si es una máscara numérica
            Dim i As Integer = 0
            While i < value.Length AndAlso (value(i) = "0"c OrElse value(i) = "#")
                i += 1
            End While
            _posiciones = i
            If i > 0 Then
                _decimales = i
                _esMascaraNumerica = True
            Else
                _esMascaraNumerica = False
            End If
            _mascaraSalidaDecimal = value
            If _esMascaraNumerica Then
                _mascaraSalidaDecimalInterna = value.Substring(0, _posiciones) + "'" + value.Substring(_posiciones).Replace("''", [sub].ToString()).Replace("'", [String].Empty).Replace([sub].ToString(), "'") + "'"
            Else
                _mascaraSalidaDecimalInterna = value.Replace("''", [sub].ToString()).Replace("'", [String].Empty).Replace([sub].ToString(), "'")
            End If
        End Set
    End Property

    ''' <summary>
    ''' Indica si la primera letra del resultado debe estár en mayúscula
    ''' </summary>
    Public Property LetraCapital() As Boolean
        Get
            Return _letraCapital
        End Get
        Set(ByVal value As Boolean)
            _letraCapital = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si se deben convertir los decimales a su expresión nominal
    ''' </summary>
    Public Property ConvertirDecimales() As Boolean
        Get
            Return _convertirDecimales
        End Get
        Set(ByVal value As Boolean)
            _convertirDecimales = value
            _apocoparUnoParteDecimal = value
            If value Then
                ' Si la máscara es la default, la borro
                If _mascaraSalidaDecimal = MascaraSalidaDecimalDefault Then
                    MascaraSalidaDecimal = ""
                End If
            ElseIf [String].IsNullOrEmpty(_mascaraSalidaDecimal) Then
                MascaraSalidaDecimal = MascaraSalidaDecimalDefault
                'Si no hay máscara dejo la default
            End If
        End Set
    End Property

    ''' <summary>
    ''' Indica si de debe cambiar "uno" por "un" en las unidades.
    ''' </summary>
    Public Property ApocoparUnoParteEntera() As Boolean
        Get
            Return _apocoparUnoParteEntera
        End Get
        Set(ByVal value As Boolean)
            _apocoparUnoParteEntera = value
        End Set
    End Property

    ''' <summary>
    ''' Determina si se debe apococopar el "uno" en la parte decimal
    ''' </summary>
    ''' <remarks>El valor de esta propiedad cambia al setear ConvertirDecimales</remarks>
    Public Property ApocoparUnoParteDecimal() As Boolean
        Get
            Return _apocoparUnoParteDecimal
        End Get
        Set(ByVal value As Boolean)
            _apocoparUnoParteDecimal = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MascaraSalidaDecimal = MascaraSalidaDecimalDefault
        SeparadorDecimalSalida = SeparadorDecimalSalidaDefault
        LetraCapital = LetraCapitalDefault
        ConvertirDecimales = _convertirDecimales
    End Sub

    Public Sub New(ByVal ConvertirDecimales As Boolean, ByVal MascaraSalidaDecimal As String, ByVal SeparadorDecimalSalida As String, ByVal LetraCapital As Boolean)
        If Not [String].IsNullOrEmpty(MascaraSalidaDecimal) Then
            Me.MascaraSalidaDecimal = MascaraSalidaDecimal
        End If
        If Not [String].IsNullOrEmpty(SeparadorDecimalSalida) Then
            _separadorDecimalSalida = SeparadorDecimalSalida
        End If
        _letraCapital = LetraCapital
        _convertirDecimales = ConvertirDecimales
    End Sub

#End Region

#Region "Conversores de instancia"
    ' Andrea para importe en ingles se agrega el parametro opcional idioma 
    Public Function ToCustomCardinal(ByVal Numero As Double, Optional ByVal Idioma As Boolean = False) As String
        Return Convertir(Convert.ToDecimal(Numero), _decimales, _separadorDecimalSalida, _mascaraSalidaDecimalInterna, _esMascaraNumerica, _letraCapital,
            _convertirDecimales, _apocoparUnoParteEntera, _apocoparUnoParteDecimal, Idioma)
    End Function


    Public Function ToCustomCardinal(ByVal Numero As Double) As String
        Return Convertir(Convert.ToDecimal(Numero), _decimales, _separadorDecimalSalida, _mascaraSalidaDecimalInterna, _esMascaraNumerica, _letraCapital,
            _convertirDecimales, _apocoparUnoParteEntera, _apocoparUnoParteDecimal)
    End Function

    Public Function ToCustomCardinal(ByVal Numero As String, Optional ByVal Idioma As Boolean = False) As String
        Dim dNumero As Double
        If [Double].TryParse(Numero, NumberStyles.Float, _cultureInfo, dNumero) Then
            Return ToCustomCardinal(dNumero, Idioma)
        Else
            Throw New ArgumentException("'" + Numero + "' no es un número válido.")
        End If
    End Function

    Public Function ToCustomCardinal(ByVal Numero As Decimal, Optional ByVal Idioma As Boolean = False) As String
        Return ToCardinal(Numero, Idioma)
    End Function

    Public Function ToCustomCardinal(ByVal Numero As Int32, Optional ByVal Idioma As Boolean = False) As String
        Return Convertir(Convert.ToDecimal(Numero), 0, _separadorDecimalSalida, _mascaraSalidaDecimalInterna, _esMascaraNumerica, _letraCapital,
            _convertirDecimales, _apocoparUnoParteEntera, False, Idioma)
    End Function

#End Region

#Region "Conversores estáticos"
    ' Andrea para importe en ingles se agrega el parametro opcional idioma 
    Public Shared Function ToCardinal(ByVal Numero As Int32, Optional ByVal Idioma As Boolean = False) As String
        Return Convertir(Convert.ToDecimal(Numero), 0, Nothing, Nothing, True, LetraCapitalDefault,
            ConvertirDecimalesDefault, ApocoparUnoParteEnteraDefault, ApocoparUnoParteDecimalDefault, Idioma)
    End Function

    Public Shared Function ToCardinal(ByVal Numero As Double, Optional ByVal Idioma As Boolean = False) As String
        Return Convertir(Convert.ToDecimal(Numero), DecimalesDefault, SeparadorDecimalSalidaDefault, MascaraSalidaDecimalDefault, True, LetraCapitalDefault,
            ConvertirDecimalesDefault, ApocoparUnoParteEnteraDefault, ApocoparUnoParteDecimalDefault, Idioma)
    End Function

    Public Shared Function ToCardinal(ByVal Numero As String, ByVal ReferenciaCultural As CultureInfo, Optional ByVal Idioma As Boolean = False) As String
        Dim dNumero As Double
        If [Double].TryParse(Numero, NumberStyles.Float, ReferenciaCultural, dNumero) Then
            Return ToCardinal(dNumero, Idioma)
        Else
            Throw New ArgumentException("'" + Numero + "' no es un número válido.")
        End If
    End Function

    Public Shared Function ToCardinal(ByVal Numero As String, Optional ByVal Idioma As Boolean = False) As String
        Return NumeroLetras.ToCardinal(Numero, CultureInfo.CurrentCulture, Idioma)
    End Function

    Public Shared Function ToCardinal(ByVal Numero As Decimal, Optional ByVal Idioma As Boolean = False) As String
        Return ToCardinal(Convert.ToDouble(Numero), Idioma)
    End Function

#End Region

    Private Shared Function Convertir(ByVal Numero As Decimal, ByVal Decimales As Int32, ByVal SeparadorDecimalSalida As String, ByVal MascaraSalidaDecimal As String, ByVal EsMascaraNumerica As Boolean, ByVal LetraCapital As Boolean,
        ByVal ConvertirDecimales As Boolean, ByVal ApocoparUnoParteEntera As Boolean, ByVal ApocoparUnoParteDecimal As Boolean, Optional ByVal Idioma As Boolean = False) As String
        Dim Num As Int64
        Dim terna As Int32, centenaTerna As Int32, decenaTerna As Int32, unidadTerna As Int32, iTerna As Int32
        Dim cadTerna As String
        Dim Resultado As New StringBuilder()

        Num = Math.Floor(Math.Abs(Numero))

        If Num >= 1000000000001 OrElse Num < 0 Then
            Throw New ArgumentException("El número '" + Numero.ToString() + "' excedió los límites del conversor: [0;1.000.000.000.001]")
        End If
        If Num = 0 Then
            If Idioma = False Then
                Resultado.Append(" cero")
            Else
                Resultado.Append(" zero")
            End If

        Else
            iTerna = 0

            Do Until Num = 0

                iTerna += 1
                cadTerna = String.Empty
                terna = Num Mod 1000

                centenaTerna = Int(terna / 100)
                decenaTerna = terna - centenaTerna * 100 'Decena junto con la unidad
                unidadTerna = (decenaTerna - Math.Floor(decenaTerna / 10) * 10)

                Select Case decenaTerna
                    Case 1 To 9
                        If Idioma = False Then
                            cadTerna = _matriz(UNI, unidadTerna) + cadTerna
                        Else
                            cadTerna = _matriz2(UNI, unidadTerna) + cadTerna
                        End If

                    Case 10 To 19
                        If Idioma = False Then
                            cadTerna = cadTerna + _matriz(DIECI, unidadTerna)
                        Else
                            cadTerna = cadTerna + _matriz2(DIECI, unidadTerna)
                        End If

                    Case 20
                        If Idioma = False Then
                            cadTerna = cadTerna + " veinte"
                        Else
                            cadTerna = cadTerna + " twenty"
                        End If

                    Case 21 To 29
                        If Idioma = False Then
                            cadTerna = " veinti" + _matriz(UNI, unidadTerna).Substring(1)
                        Else
                            cadTerna = " twenty" + _matriz2(UNI, unidadTerna).Substring(1)
                        End If

                    Case 30 To 99
                        If unidadTerna <> 0 Then
                            If Idioma = False Then
                                cadTerna = _matriz(DECENA, Int(decenaTerna / 10)) + " y" + _matriz(UNI, unidadTerna) + cadTerna
                            Else
                                cadTerna = _matriz2(DECENA, Int(decenaTerna / 10)) + "" + _matriz2(UNI, unidadTerna) + cadTerna
                            End If

                        Else
                            If Idioma = False Then
                                cadTerna += _matriz(DECENA, Int(decenaTerna / 10))
                            Else
                                cadTerna += _matriz2(DECENA, Int(decenaTerna / 10))
                            End If

                        End If
                End Select

                Select Case centenaTerna
                    Case 1
                        If Idioma = False Then
                            If decenaTerna > 0 Then
                                cadTerna = " ciento" + cadTerna
                            Else
                                cadTerna = " cien" + cadTerna
                            End If

                        Else
                            cadTerna = " one hundred" + cadTerna
                        End If
                        Exit Select
                    Case 5, 7, 9
                        If Idioma = False Then
                            cadTerna = _matriz(CENTENA, Int(terna / 100)) + cadTerna
                        Else
                            cadTerna = _matriz2(CENTENA, Int(terna / 100)) + cadTerna
                        End If

                        Exit Select
                    Case Else
                        If Int(terna / 100) > 1 Then
                            If Idioma = False Then
                                cadTerna = _matriz(UNI, Int(terna / 100)) + "cientos" + cadTerna
                            Else
                                cadTerna = _matriz2(UNI, Int(terna / 100)) + " hundred" + cadTerna
                            End If

                        End If
                        Exit Select
                End Select
                'Reemplazo el 'uno' por 'un' si no es en las únidades o si se solicító apocopar
                If Idioma = False Then
                    If (iTerna > 1 OrElse ApocoparUnoParteEntera) AndAlso decenaTerna = 21 Then
                        cadTerna = cadTerna.Replace("veintiuno", "veintiún")
                    ElseIf (iTerna > 1 OrElse ApocoparUnoParteEntera) AndAlso unidadTerna = 1 AndAlso decenaTerna <> 11 Then
                        cadTerna = cadTerna.Substring(0, cadTerna.Length - 1)
                        'Acentúo 'veintidós', 'veintitrés' y 'veintiséis'
                    ElseIf decenaTerna = 22 Then
                        cadTerna = cadTerna.Replace("veintidos", "veintidós")
                    ElseIf decenaTerna = 23 Then
                        cadTerna = cadTerna.Replace("veintitres", "veintitrés")
                    ElseIf decenaTerna = 26 Then
                        cadTerna = cadTerna.Replace("veintiseis", "veintiséis")
                    End If


                End If

                'Completo miles y millones
                Select Case iTerna
                    Case 3
                        If Numero < 2000000 Then
                            If Idioma = False Then
                                cadTerna += " millón"
                            Else
                                cadTerna += " million"
                            End If

                        Else
                            If Idioma = False Then
                                cadTerna += " millones"
                            Else
                                cadTerna += " millions"
                            End If

                        End If
                    Case 2, 4
                        If terna > 0 Then
                            If Idioma = False Then
                                cadTerna += " mil"
                            Else
                                cadTerna += " thousand"
                            End If
                        End If

                End Select
                Resultado.Insert(0, cadTerna)
                Num = Int(Num / 1000)
            Loop
        End If

        'Se agregan los decimales si corresponde
        If Decimales > 0 Then
            Resultado.Append(" " + SeparadorDecimalSalida + " ")
            Dim EnteroDecimal As Int32 = Int(Math.Round((Numero - Int(Numero)) * Math.Pow(10, Decimales)))
            If ConvertirDecimales Then
                Dim esMascaraDecimalDefault As Boolean = MascaraSalidaDecimal = MascaraSalidaDecimalDefault
                Resultado.Append(Convertir(Convert.ToDecimal(EnteroDecimal), 0, Nothing, Nothing, EsMascaraNumerica, False,
                    False, (ApocoparUnoParteDecimal AndAlso Not EsMascaraNumerica), False) + " " + (IIf(EsMascaraNumerica, "", MascaraSalidaDecimal)))
            ElseIf EsMascaraNumerica Then
                Resultado.Append(EnteroDecimal.ToString(MascaraSalidaDecimal))
            Else
                Resultado.Append(EnteroDecimal.ToString() + " " + MascaraSalidaDecimal)
            End If
        End If
        'Se pone la primer letra en mayúscula si corresponde y se retorna el resultado
        If LetraCapital Then
            Return Resultado(1).ToString().ToUpper() + Resultado.ToString(2, Resultado.Length - 2)
        Else
            Return Resultado.ToString().Substring(1)
        End If
    End Function


    'No soporta Framework 4.5, habría que convertir todo.

    'Private Shared Function EnviarCorreo(ByVal destinatarios_ As List(Of String),
    '                                     ByVal subject_ As String,
    '                                     ByVal mensajeSimple_ As String,
    '                                     Optional ByVal mensajeEnvoltura_ As StringBuilder = Nothing,
    '                                     Optional ByVal espacioTrabajo_ As IEspacioTrabajo = Nothing) As Boolean

    '    Dim operacionesCorreo_ As IOperacionesCorreoElectronico

    '    operacionesCorreo_ = New OperacionesCorreoElectronico64


    '    Dim operacionesCatalogo_ = New OperacionesCatalogo

    '    Dim correo_ = New MensajeCorreoElectronico

    '    With operacionesCatalogo_

    '        .EspacioTrabajo = espacioTrabajo_

    '    End With

    '    With correo_

    '        .Too = destinatarios_

    '        .Subject = subject_

    '        If Not mensajeEnvoltura_ Is Nothing Then

    '            .Body = mensajeEnvoltura_.ToString

    '        Else

    '            'PENDIENTE DE IMPLEMENTAR

    '            .Body = "<!DOCTYPE html> <html lang=""en""> <head> <meta charset=""UTF-8""> <title>Recuperar Contraseña</title><link href=""https://fonts.googleapis.com/css?family=Roboto"" rel=""stylesheet""> <style> * { padding: 0; margin: 0; box-sizing: border-box; font-family: 'Roboto', sans-serif; } .container { width: 100%; } .wrapper { max-width: 400px; min-height: 500px; margin: auto; text-align: center; } .text { color: #424241; font-size: 14px; padding: 0px 10px; margin-top: 15px; margin-bottom: 50px; text-align: justify; } .text p { margin-bottom: 10px; } span.password { font-size: 18px; } .text p:nth-child(3) { font-size: 13px; } </style> </head> <body> <div class=""container""> <div class=""wrapper""> <!--<img src=""data:image_/jpeg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAABkAAD/4QNvaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjMtYzAxMSA2Ni4xNDU2NjEsIDIwMTIvMDIvMDYtMTQ6NTY6MjcgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdFJlZj0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlUmVmIyIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6RUJGQjFBM0NBMDEyRTkxMTk1Rjc5Q0Q4NTM1MEI1QUYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6QUEzNEJDNkQxMkFDMTFFOUE0Q0VDMjJBRkFDM0U1Q0EiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6QUEzNEJDNkMxMkFDMTFFOUE0Q0VDMjJBRkFDM0U1Q0EiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpFRkZCMUEzQ0EwMTJFOTExOTVGNzlDRDg1MzUwQjVBRiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpFQkZCMUEzQ0EwMTJFOTExOTVGNzlDRDg1MzUwQjVBRiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6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" & _
    '                "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"" alt="""">--> <img src=""data:image_/png;base64,iVBORw0KGgoAAAANSUhEUgAAAZAAAACiCAMAAABYpGvsAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAA2ZpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDpFQkZCMUEzQ0EwMTJFOTExOTVGNzlDRDg1MzUwQjVBRiIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDozQjEwNTM0QTEyQUUxMUU5OTU5RUQxREY5QjQzMUQyOCIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDozQjEwNTM0OTEyQUUxMUU5OTU5RUQxREY5QjQzMUQyOCIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgQ1M2IChXaW5kb3dzKSI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOkVGRkIxQTNDQTAxMkU5MTE5NUY3OUNEODUzNTBCNUFGIiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOkVCRkIxQTNDQTAxMkU5MTE5NUY3OUNEODUzNTBCNUFGIi8+IDwvcmRmOkRlc2NyaXB0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" & _
    '                "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" & _
    '                "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" & _
    '                "tcoeMrX4oKkk+T4CTxwTuLjsgicFtjviLNG0UHkwjUXIamOcQQ9VsSSPaI7I4wWkXvIEOENbUmvukXKGknZ4NpTzabx3non10Y8iJwEGqwi8TQFuM+ErauMD6fpNSJZQZYOvv5YwMGbSGJ52Kp52CpzRKh1TUy3jcl5ys94boFpRZnttbIF18v4bXD6QdrukuYsj/HgJhe/neCpKw1IcrM4HxamOkhSNIJUx4VB8IIwTse5WuVFp5Ih4haC8SbbGYKmn8JxfMewIGkMYnNfIMrqhSTxRiDcSQ0n+2IenyM7NT3AUv23AAMAKcdPx49sw94AAAAASUVORK5CYII="" alt=""""> <div class=""text""> " & _
    '                "<p>!Hola " & "NOMBRE USUARIOOO!!!!" & "¡ Lamentamos que perdieras tu contraseña, pero no te preocupes te proporcionamos una nueva :D</p> <p>Esta es tu nueva contraseña: <span class=""password"">" & _
    '                "<b>" & "CONTRASEÑA!!!" & "</b></span></p> <p>Te recordamos que una vez ingresando a nuestra extranet, en tu perfil puedes cambiar tu contraseña a una más de tu agrado.</p> </div> <img src=""data:image_/png;base64,iVBORw0KGgoAAAANSUhEUgAAARcAAABaCAYAAACSYhLSAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6MTRBOUEwQTNFQTA4MTFFNDlFMUJGRTUyQTBBODI5QzYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6MTRBOUEwQTRFQTA4MTFFNDlFMUJGRTUyQTBBODI5QzYiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDoxNEE5QTBBMUVBMDgxMUU0OUUxQkZFNTJBMEE4MjlDNiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDoxNEE5QTBBMkVBMDgxMUU0OUUxQkZFNTJBMEE4MjlDNiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PmuGHKsAAA6ySURBVHja7F2LteI4D/bs+RvIlMAtAUoIfwdQApQAJUAJpAToYEkJpIRJCZMSZiNW2qvr61dCeH/fOTl3hvip2LIk29KPP3/+GAAAgKHxF0gAAMA18D+Q4D4o/p5k7Z9j+4ytV037TBf/P1VP2q/f7Z/M8WpCfWrfz9p/7x3vm/b9T4wMMBcwh78ni/ZP3j4jxSCIMVT8FO1kqQNF7F+NsTCyyO+jjvkAMJe3YSrEUHaeSZIxw8mZUWw9ZWw4jQYxovmTMxYAAHO5QFrZXVgGqQUr6+eKJZbmDcjYdPwdAHN5ecZCksrm0nJaBnJo//x4Vzq2/S+InBhRYC7AJ1aX2gWYQYmtZmypRGX7HNrJVzryUdqTo8g15eG2jfkp+SGbT8PS1kypaiQlle27racesSONuL+2XahUba170MDXF6dB12q/lnLOtq02zzqRxqWJ2MJY5Z0pWn6rj7/V4U2kzIvwA+dckifF7wBzkcFn1IRc6wnc5l8lSj4F522sQX90pK0ck1+/a8x3285/79s6JgmTPqTebG0m1ZbjG1Ck9pWBvpBU80OV49tNC+XZOFROF9aOdu+YKZk++QFILn0ZyzjAWGhFnIdWMl59U1UqGeDLhLTjnu/O74nhqUnSVSqj9Ju2jIZVnaFxTOhDX+ZA7TbSd2b8C4z0YYFDdGkYR1axEGPJethqFrzCX90EMkAZG1ZFhmTmi46MJe/RlxV/m6HoAIC59EIWEMurhAmc3WnixzBSE+wS2szuzPQWAdWtNO6dqEzlG2GIg7k8I/KATWRq/DsnqRN2bsK7L/R+fYH6ZLidU/NpV0rt49CSYqHaIk+oflJXp0yDPu3eqnqEjvRbjWEdB2wuN5AOfAOXDZyVb+UldSO2I0Nb22T38JXBW99i7OwnRvAOVlsGTax9TwbVRcVJVkFZ8spC7WY6h77NwcPMV8yAiivZlSC5AFdhLg0P/KZH3nuh6ao2DqoruWk1DjCqP/JE6FtEpKhdW8YvPgAJgLkAQCfJLLY7R4xoz7tSAJjLYKgDq+MY5HlaNIrBkPTyYeKnhxe8mwWAuVyXuZj4oa2gKhHZrXk0w+Fdby57aBVSK6eRZ2JJMHX7LBOYDJhLAmDQTRObKzaaugb3jE/v+k7o0u8uI+WKy5wF6n0I5qKMrD5GWg6poniMr0KzskP6PNQ2oa+SROj/Ff++ZGO7Sw2CtArmMigOgRUrM/5tzdLzjgboMVLfo+AYeT+0mwjftYaVg8H9CNB4FZIsW+YxZXvLzvo9WZ0CoBYNgW3PQVVckO9ZUNy5vFvT6oDpAOYypGpEovK6R76mR77CdTv6QbEeWn1j42rVIf3hhgymMXAZAeZyBQYjJ0TrHvlSGUyXtI/AWK51O3jakcEsjcfzX4BJ1B3VHEq3hLfANMDm0p3BkETxkeBD92Dl27Z5xG7TyZ+LiXtvS/Hu5jNIN4mTKlP2EPEXU0fS9m2rSHyTmD8XK8/aovHIonFtPv3AHCxGlpuvvmzsug4Gflw6Af5cgH/FpUQ/KwAAtQgAADAXAADAXAAAAMBcgE5AyA9gUMCgCwAAJBcAAMBcAAAAcwEAAABzAQAAzOVxQN7iAn5Un7L9Xfv07DQAwFweFaMXbP/ozWgAgLm8H9jD/K29k9ElPJxBAR4WuBU9rHRxs6v4fO3/J8gOQHIBAACSyxOrJyQ9UGRBch8gPkUan/qggsTP" & _
    '                "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"" alt=""""> </div> </div></body> </html>"


    '        End If

    '    End With

    '    operacionesCorreo_.EnviarCorreo(operacionesCatalogo_,
    '                                    correo_,
    '                                    IOperacionesCorreoElectronico.Prioridades.Alta,
    '                                    IOperacionesCorreoElectronico.CantidadIntentosEnvio.Normal,
    '                                    IOperacionesCorreoElectronico.Modalidades.Individual)

    '    If operacionesCorreo_.Estatus.Status = TagWatcher.TypeStatus.Ok Then

    '        Return True

    '    Else

    '        Return False

    '    End If

    'End Function

End Class


Public Class ValorProvisionalOption
    Public Property Id As ObjectId
    Public Property Valor As String
End Class
