Imports AspNet.Identity.MongoDB
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin
Imports MongoDB.Bson

Public Class ApplicationRoleManager
    Inherits RoleManager(Of ApplicationRole)
    Implements IRole

    Private _id As String

    Private _Name As String

    Public Sub New()

        MyBase.New(New RoleStore(Of ApplicationRole)(New ApplicationDbContext().Roles))

    End Sub

    Public Sub New(store_ As IRoleStore(Of ApplicationRole))

        MyBase.New(store_)

    End Sub


    Public ReadOnly Property Id As String Implements IRole(Of String).Id


        Get
            Return _id

        End Get


    End Property

    Public Property Name As String Implements IRole(Of String).Name

        Get
            Return _Name

        End Get

        Set(value As String)

            _Name = value

        End Set

    End Property

    Public Shared Function Create(options_ As IdentityFactoryOptions(Of ApplicationRoleManager), context_ As IOwinContext) As ApplicationRoleManager

        Dim dbContext_ = context_.Get(Of ApplicationDbContext)()

        Dim roleStore_ As New RoleStore(Of ApplicationRole)(dbContext_.Roles)

        Dim manager_ = New ApplicationRoleManager(roleStore_)

        ' Configura la validación de nombres de usuario, contraseñas, etc.
        Return manager_

    End Function

    'Public Shared Function Create(options_ As IdentityFactoryOptions(Of ApplicationRoleManager), context_ As IOwinContext) As ApplicationRoleManager

    '    Dim dbContext_ = context_.Get(Of ApplicationDbContext)()

    '    Dim roleStore_ As New RoleStore(Of ApplicationRole)(dbContext_.Roles)

    '    Dim manager_ = New ApplicationRoleManager(roleStore_)

    '    ' Configura la validación de nombres de usuario, contraseñas, etc.
    '    Return manager_

    'End Function

End Class
