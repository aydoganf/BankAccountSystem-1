using AccountApp.App_Start;
using AccountApp.Utility;
using AydoganFBank.Service.Dispatcher.Api;
using AydoganFBank.Service.Dispatcher.Data;
using System;
using System.Web.Mvc;

namespace AccountApp.Filters
{
    public class AuthorizationFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Session session = filterContext.HttpContext.GetSession();
            TokenInfo sessionToken = session.GetToken();
            
            if(sessionToken == null)
                filterContext.Result = new RedirectResult("/Login/Index");

            var securityManagerService = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<ISecurityManagerService>();

            try
            {
                var validatedToken = securityManagerService.ValidateToken(sessionToken.Token, Application.APPLICATION_ID);

                session.SetToken(validatedToken);
            }
            catch (Exception ex)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
        }
    }
}