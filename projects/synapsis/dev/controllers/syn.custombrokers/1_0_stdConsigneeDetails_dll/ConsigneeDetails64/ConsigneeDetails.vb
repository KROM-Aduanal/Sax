﻿Imports MongoDB.Bson.Serialization.Attributes

Public Class ConsigneeDetails
    Property consigneedetailsname As String
    Property taxid As String
    <BsonIgnoreIfNull>
    Property address As String
    Property street As String
    Property externalnumber As String
    <BsonIgnoreIfNull>
    Property internalnumber As String
    <BsonIgnoreIfNull>
    Property zipcode As String
    <BsonIgnoreIfNull>
    Property locality As String
    Property city As String
    <BsonIgnoreIfNull>
    Property state As String
    Property country As String
End Class
