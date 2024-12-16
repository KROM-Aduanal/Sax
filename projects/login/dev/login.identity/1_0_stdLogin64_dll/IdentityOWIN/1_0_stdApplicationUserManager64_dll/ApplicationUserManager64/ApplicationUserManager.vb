Imports AspNet.Identity.MongoDB
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin

Public Class ApplicationUserManager
    Inherits UserManager(Of ApplicationUser)

    Public Sub New(store As IUserStore(Of ApplicationUser))
        MyBase.New(store)
    End Sub

    Public Shared Function Create(options As IdentityFactoryOptions(Of ApplicationUserManager), context As IOwinContext) As ApplicationUserManager
        Dim dbContext = context.Get(Of ApplicationDbContext)()
        Dim userStore = New UserStore(Of ApplicationUser)(dbContext.Users)
        Dim manager = New ApplicationUserManager(userStore)
        ' Configura la validación de nombres de usuario, contraseñas, etc.
        Return manager
    End Function
End Class