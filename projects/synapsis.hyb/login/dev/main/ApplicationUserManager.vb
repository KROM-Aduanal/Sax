Imports AspNet.Identity.MongoDB
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin

Public Class ApplicationUserManager
    Inherits UserManager(Of ApplicationUser)

    Public Sub New(store_ As IUserStore(Of ApplicationUser))

        MyBase.New(store_)

    End Sub

    Public Shared Function Create(options_ As IdentityFactoryOptions(Of ApplicationUserManager), context_ As IOwinContext) As ApplicationUserManager

        Dim dbContext_ = context_.Get(Of ApplicationDbContext)()

        Dim userStore_ = New UserStore(Of ApplicationUser)(dbContext_.Users)

        Dim manager_ = New ApplicationUserManager(userStore_)
        ' Configura la validación de nombres de usuario, contraseñas, etc.
        Return manager_

    End Function

End Class