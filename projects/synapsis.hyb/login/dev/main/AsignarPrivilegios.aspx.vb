﻿Imports AspNet.Identity.MongoDB
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports System.Reflection
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports Wma.Exceptions

Public Class AsignarPrivilegios
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Sub AsignarPrivilegios_Click()


        Dim tagwatcher_ = New TagWatcher With {.Status = TagWatcher.TypeStatus.Ok}

        Dim tagwatcherTask_ As Task(Of TagWatcher) = AsignarPrivilegios(tbEmail.Text, tagwatcher_)

        tagwatcherTask_.Wait()

        tagwatcher_ = tagwatcherTask_.Result

        If tagwatcher_.Status = TagWatcher.TypeStatus.Ok Then

            MsgBox("Actualización realizada con éxito")

            Response.Redirect("Login.aspx")

        Else

            MsgBox("error de conexión " & tagwatcher_.ObjectReturned)

        End If



    End Sub

    Sub GoStartSesion_Click()

        Response.Redirect("Login.aspx")

    End Sub


    Public Async Function AsignarPrivilegios(email_ As String, tagwatcher_ As TagWatcher) As Task(Of TagWatcher)

        Dim context_ = HttpContext.Current.GetOwinContext()

        Dim userManager_ = context_.GetUserManager(Of ApplicationUserManager)()

        ' Autenticación basada en Claims

        Dim user_ As ApplicationUser = Await userManager_.FindByNameAsync(email_).ConfigureAwait(False)

        Dim indexPage_ = 2

        user_.AddClaim(New Claim("SYNAPSISLocation", "NULL"))

        user_.AddClaim(New Claim("SYNAPSISPanel General_fas fa-chart-bar_1", "../../../../../../FrontEnd/Modulos/TraficoAA/ConsultasOperaciones/Ges003-001-Consultas.Principal.aspx"))

        user_.AddClaim(New Claim("SYNAPSISModulos_fa fa-laptop_2", "#"))

        If ckSynapsisPanIzqModApendices.Checked Then

            user_.AddClaim(New Claim("SYNAPSISApendices_ _" & indexPage_, "http://localhost:14329/FrontEnd/Modulos/TraficoAA/AltaApendices/Ges022-001-RegistroApendices.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModClientes.Checked Then

            user_.AddClaim(New Claim("SYNAPSISClientes_ _" & indexPage_, "http://localhost:14333/FrontEnd/Modulos/TraficoAA/Clientes/Ges022-001-Clientes.aspx"))

            indexPage_ += 1


        End If

        If ckSynapsisPanIzqModRegistroReferencias.Checked Then

            user_.AddClaim(New Claim("SYNAPSISReferencias_ _" & indexPage_, "http://localhost:14329/FrontEnd/Modulos/TraficoAA/Referencias/Ges022-001-Referencias.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModRegistroFacturasImp.Checked Then

            user_.AddClaim(New Claim("SYNAPSISFacturas Importación_ _" & indexPage_, "http://localhost:14330/FrontEnd/Modulos/TraficoAA/FacturasComerciales/Ges003-001-FacturasComerciales.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModRegistroFacturasImp.Checked Then

            user_.AddClaim(New Claim("SYNAPSISFacturas Exportación_ _" & indexPage_, "http://localhost:14330/FrontEnd/Modulos/TraficoAA/FacturaComercialExportacion/Ges022_FacturaComercialExportacion.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModPedimentos.Checked Then

            user_.AddClaim(New Claim("SYNAPSISPedimentos_ _" & indexPage_, "http://localhost:14329/FrontEnd/Modulos/TraficoAA/MetaforaPedimento/Ges022-001-MetaforaPedimento.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModTarifaArancelaria.Checked Then

            user_.AddClaim(New Claim("SYNAPSISTarifa Arencelaria_ _" & indexPage_, "http://localhost:14335/FrontEnd/Modulos/TraficoAA/TarifaArancelaria/Ges022-001-TarifaArancelaria.aspx"))

            indexPage_ += 1

        End If


        If ckSynapsisPanIzqModRegistroProductos.Checked Then

            user_.AddClaim(New Claim("SYNAPSISProductos_ _" & indexPage_, "http://localhost:14330/FrontEnd/Modulos/TraficoAA/Productos/Ges022-001-RegistroProductos.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModRegistroProveedoresImp.Checked Then

            user_.AddClaim(New Claim("SYNAPSISProveedores Nacionales_ _" & indexPage_, "http://localhost:14330/FrontEnd/Modulos/TraficoAA/Proveedores/Ges022-001-RegistroProveedores.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModRegistroProveedoresExp.Checked Then

            user_.AddClaim(New Claim("SYNAPSISProveedores Extranjeros_ _" & indexPage_, "http://localhost:14330/FrontEnd/Modulos/TraficoAA/ProveedorExtranjero/Ges022_ProveedorExtranjero.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqModViajes.Checked Then

            user_.AddClaim(New Claim("SYNAPSISViajes_ _" & indexPage_, "../../../../../../FrontEnd/Modulos/TraficoAA/Viajes/Ges022-001-Viajes.aspx"))

            indexPage_ += 1

        End If


        If ckSynapsisPanIzqModBusquedaGeneral.Checked Then

            user_.AddClaim(New Claim("SYNAPSISBusqueda general_ _" & indexPage_, "../../../../../../FrontEnd/Modulos/TraficoAA/BusquedaGeneral/BusquedaGeneral.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqProcesamientoElectronico.Checked Then

            user_.AddClaim(New Claim("SYNAPSISProcesamiento_ _" & indexPage_, "http://localhost:14335/FrontEnd/Modulos/TraficoAA/ProcesamientoElectronicoDocumentos/Ges022_ProcesamientoElectronicoDocumentos.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqRevalidacion.Checked Then

            user_.AddClaim(New Claim("SYNAPSISRevalidación_ _" & indexPage_, "http://localhost:14329/FrontEnd/Modulos/TraficoAA/Revalidacion/Ges022-001-Revalidacion.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqAcuseValor.Checked Then

            user_.AddClaim(New Claim("SYNAPSISAcuse de Valor_ _" & indexPage_, "http://localhost:14330/FrontEnd/Modulos/TraficoAA/AcusesValor/Ges022-001-AcuseValor.aspx"))

            indexPage_ += 1

        End If

        If ckSynapsisPanIzqCuboDatos.Checked Then

            user_.AddClaim(New Claim("SYNAPSISCubo de Datos_ _" & indexPage_, "http://localhost:14335/FrontEnd/Modulos/TraficoAA/CuboDatos/Ges022-001-CuboDatos.aspx"))

            indexPage_ += 1

        End If

        If ckAdminSynApsis.Checked Then

            Dim role_ As ApplicationRole

            Dim roleManager_ = New ApplicationRoleManager()

            If Not Await roleManager_.RoleExistsAsync("ADMINSYNAPSIS").ConfigureAwait(False) Then

                role_ = New ApplicationRole

                role_.Name = "ADMINSYNAPSIS"

                Await roleManager_.CreateAsync(role_).ConfigureAwait(False)

            Else

                role_ = roleManager_.FindByName("ADMINSYNAPSIS")

            End If

            user_.AddRole(role_.Id)

            Await userManager_.UpdateAsync(user_)

            'Dim result_ As IdentityResult = Await userManager_.AddToRoleAsync(user_.Id, role_.Id).ConfigureAwait(False)

            'user_.AddRole("ADMINSYNAPSIS")

        End If

        If ckAdminKromBaseWeb.Checked Then

            user_.AddRole("ADMINKROMBASEWEB")

        End If


        Dim result = Await userManager_.UpdateAsync(user_).ConfigureAwait(False)

        If result.Succeeded Then

            Return tagwatcher_

        Else

            tagwatcher_.ObjectReturned = result.Errors(0)

            tagwatcher_.SetError(Me, result.Errors(0))

            Return tagwatcher_

        End If

    End Function

End Class