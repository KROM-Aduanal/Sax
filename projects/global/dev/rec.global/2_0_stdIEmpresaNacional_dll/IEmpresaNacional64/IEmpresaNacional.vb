﻿Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes
Imports Rec.Globals.Empresas

Public Interface IEmpresaNacional
    Enum TiposPersona

        Moral = 1

        Fisica = 2

    End Enum


    <BsonIgnore>
    Property rfcNuevo As Boolean?

    Property _idrfc As ObjectId

    Property rfc As String

    Property rfcs As List(Of Rfc)

    <BsonIgnore>
    Property curpNuevo As Boolean?

    <BsonIgnoreIfNull>
    Property _idcurp As ObjectId?

    <BsonIgnoreIfNull>
    Property curp As String

    <BsonIgnoreIfNull>
    Property curps As List(Of Curp)

    <BsonIgnoreIfNull>
    Property regimenesfiscales As List(Of RegimenFiscal)

    Property tipopersona As TiposPersona

End Interface