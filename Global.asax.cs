using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using log4net;
using log4net.Config;
using UltraorganicsWS.services;

namespace UltraorganicsWS
{
    public class Global : System.Web.HttpApplication
    {
        private static ILog log = LogManager.GetLogger(typeof(Global));

        protected void Application_Start(object sender, EventArgs e)
        {
            // Iniciar el logger
            XmlConfigurator.Configure();

            log.Info("Iniciando Servicios Ultraorganics");

            // Iniciar el pool de sesiones SAP
            SessionPool.Init();

            log.Info("Servidor Iniciado");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            // Cerrar las sesiones del pool
            SessionPool.ShutDown();
        }
    }
}