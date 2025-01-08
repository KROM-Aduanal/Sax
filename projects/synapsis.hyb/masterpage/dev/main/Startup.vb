
'Imports Microsoft.Owin

'Imports Microsoft.Owin.Security.Cookies
'Imports Microsoft.Owin.Security.Google
'Imports Owin
'Imports System.Configuration

'<Assembly: OwinStartup(GetType(Startup))>

'Public Class Startup
'    Public Sub Configuration(app As IAppBuilder)
'        ' Configuramos la autenticación basada en cookies primero
'        app.UseCookieAuthentication(New CookieAuthenticationOptions With {
'            .AuthenticationType = "ApplicationCookie",
'            .LoginPath = New PathString("/Login.aspx")
'        })

'        ' Luego, configura la autenticación con Google
'        Dim googleOAuth2AuthenticationOptions = New GoogleOAuth2AuthenticationOptions With {
'            .ClientId = ConfigurationManager.AppSettings("GoogleClientId"),
'            .ClientSecret = ConfigurationManager.AppSettings("GoogleClientSecret"),
'            .CallbackPath = New PathString("/signin-google"),
'            .SignInAsAuthenticationType = "ApplicationCookie", ' Asegúrate de especificar el tipo de autenticación
'            .Provider = New GoogleOAuth2AuthenticationProvider With {
'                .OnAuthenticated = Function(context)
'                                       ' Opcional: accede a información del usuario autenticado si es necesario
'                                       Return Threading.Tasks.Task.FromResult(0)
'                                   End Function
'            }
'        }

'        app.UseGoogleAuthentication(googleOAuth2AuthenticationOptions)

'    End Sub
'End Class

Imports Microsoft.Owin
Imports Microsoft.Owin.Security.Cookies
Imports Owin

<Assembly: OwinStartup(GetType(Principalidad.Startup))>

Namespace Principalidad
    Public Class Startup
        Public Sub Configuration(app As IAppBuilder)
            ' Configuración de autenticación basada en cookies
            app.UseCookieAuthentication(New CookieAuthenticationOptions With {
                .AuthenticationType = "ApplicationCookie",
                .LoginPath = New PathString("/Login.aspx"),
                .CookieName = ".AspNet.ApplicationCookie",
                .CookieHttpOnly = True,
                .CookieSecure = CookieSecureOption.Always
            })

        End Sub
    End Class
End Namespace



'Imports Microsoft.AspNet.Identity
'Imports Microsoft.AspNet.Identity.MongoDB
'Imports MongoDB.Driver
'Imports Owin
'Imports Rec.Globals.Controllers

'Public Class Startup
'    Public Sub Configuration(app As IAppBuilder)
'        ' ... otras configuraciones

'        ' Configurar el contexto de MongoDB
'        Dim connectionString As String = "mongodb://localhost:27017"
'        Dim databaseName As String = "YourDatabaseName"
'        Dim client = New MongoClient(connectionString)
'        Dim Database = client.GetDatabase(databaseName)

'        app.CreatePerOwinContext(Function() New ApplicationDbContext())
'        app.CreatePerOwinContext(Function() New UserManager(New UserStore < ApplicationUser > (New ApplicationDbContext(Database))))
'        app.CreatePerOwinContext(Function() New RoleManager(New RoleStore < IdentityRole > (New ApplicationDbContext(Database))))

'        ' ...
'    End Sub
'End Class