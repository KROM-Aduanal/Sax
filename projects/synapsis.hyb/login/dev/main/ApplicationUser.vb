Imports AspNet.Identity.MongoDB
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Class ApplicationUser

    Inherits IdentityUser

    Public Property NormalizedUserName As String

    Public Property NormalizedEmail As String
    Public Property AccessFiledCount As Int32
    Public Property ConcurrencyStamp As String
    Public Property LockoutEnd As String

    Public Property Version As Int32
    Public Property CreatedOn As DateTime

    Public Property Tokens As List(Of String)

End Class
