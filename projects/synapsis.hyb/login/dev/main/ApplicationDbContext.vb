Imports gsol.krom
Imports MongoDB.Driver

Public Class ApplicationDbContext
    Implements IDisposable

    Private _statements As Sax.SaxStatements = Sax.SaxStatements.GetInstance(17)

    Private ReadOnly _database As IMongoDatabase

    Public Sub New()

        _statements.Initialize(17)

        Using enlace_ As IEnlaceDatos = New EnlaceDatos(17)

            Dim colelction_ = enlace_.GetMongoCollection(Of ApplicationUser)("Users")

            _database = enlace_.GetMongoClient.GetDatabase("Identity")

        End Using

    End Sub

    Public ReadOnly Property Users As IMongoCollection(Of ApplicationUser)

        Get

            Return _database.GetCollection(Of ApplicationUser)("Users")

        End Get

    End Property

    Public ReadOnly Property Roles As IMongoCollection(Of ApplicationRole)

        Get

            Return _database.GetCollection(Of ApplicationRole)("Roles")

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
