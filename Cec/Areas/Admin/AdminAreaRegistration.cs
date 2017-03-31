using System.Web.Mvc;

namespace Cec.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName { get { return "admin"; } }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Admin_default",
                url: "admin/{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}