Imports MongoDB.Bson.Serialization.Attributes

Public Class ConfiguracionNodo

#Region "Enums"

    Enum TiposVisibilidad

        SinDefinir
        Visible
        Oculto
        Condicionado 'Si se vera si cumple con unos filtros

    End Enum

#End Region

#Region "Propiedades"

    <BsonIgnore>
    Public Property TipoVisibilidad As TiposVisibilidad

    <BsonIgnore>
    Public Property IdPermiso As Integer

#End Region

End Class