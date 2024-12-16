Imports MongoDB.Driver

Public Class ApplicationDbContext
    Implements IDisposable


    Private ReadOnly _database As IMongoDatabase

    Public Sub New()

        Dim connectionString_ = "mongodb+srv://superAdmin:4SdBi8XzKwcmRukh@cluster1.eyzwc.mongodb.net"

        Dim client = New MongoClient(connectionString_)

        _database = client.GetDatabase("Identity")

    End Sub

    Public ReadOnly Property Users As IMongoCollection(Of ApplicationUser)
        Get
            Return _database.GetCollection(Of ApplicationUser)("Users")
        End Get
    End Property

#Region "IDisposable Support"
    Private disposedValue As Boolean ' Para detectar llamadas redundantes

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not Me.disposedValue Then

            If disposing Then
                ' TODO: eliminar estado administrado (objetos administrados).
            End If

            'Propiedades no administradas


            'With Me

            '    .InstitucionesBancarias = Nothing

            '    .Estado.Clear()

            '    .ModalidadTrabajo = Nothing

            '    ._conservarBancos = Nothing


            'End With

            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
            ' TODO: Establecer campos grandes como Null.
        End If

        Me.disposedValue = True

    End Sub


    ' Visual Basic agregó este código para implementar correctamente el modelo descartable.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en Dispose(disposing As Boolean).
        Dispose(True)

        GC.SuppressFinalize(Me)

    End Sub

#End Region
End Class
