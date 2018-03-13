using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using SAPbobsCOM;
using Queue = System.Collections.Queue;

namespace UltraorganicsWS.services
{
    public class SessionPool
    {
        private static Queue<Company> sesionesDisponibles = new Queue<Company>();
        private static ArrayList sesionesUso = new ArrayList();

        static SessionPool ()
        {
            CrearSesiones(5);
        }

        private static void CrearSesiones(int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                sesionesDisponibles.Enqueue(CrearSesion());
            }
        }

        // Este metodo es solo para invocar el constructor
        public static void Init()
        {
            return;
        }

        public static void ShutDown()
        {
            // Cerrar las sesiones disponibles
            foreach (Company company in sesionesDisponibles)
            {
                if (company != null && company.Connected) company.Disconnect();
            }

            // Cerrar las sesiones en uso, hacer rollback a transacciones pendientes
            foreach (Company company in sesionesUso)
            {
                if (company != null && company.Connected)
                {
                    if (company.InTransaction) company.EndTransaction(BoWfTransOpt.wf_RollBack);
                    company.Disconnect();
                }
            }
        }

        private static Company CrearSesion()
        {
            Company company = new Company();

            company.DbServerType = BoDataServerTypes.dst_MSSQL2012;
            company.Server = ConfigurationManager.AppSettings["DBServer"];
            company.DbUserName = ConfigurationManager.AppSettings["DBUserName"];
            company.DbPassword = ConfigurationManager.AppSettings["DBPassword"];
            company.CompanyDB = ConfigurationManager.AppSettings["CompanyDB"];
            company.UseTrusted = false;

            company.LicenseServer = ConfigurationManager.AppSettings["LicenseServer"];
            company.UserName = ConfigurationManager.AppSettings["SAPUserName"];
            company.Password = ConfigurationManager.AppSettings["SAPPassword"];

            company.language = BoSuppLangs.ln_Spanish_La;

            if (company.Connect() != 0)
            {
                throw new Exception(company.GetLastErrorDescription());
            }

            return company;
        } // Conectar

        public static Sesion getSession()
        {
            Sesion sesion = new Sesion();

            // TODO: revisar si hay sesiones disponibles
            sesion.company = sesionesDisponibles.Dequeue();
            sesion.index = sesionesUso.Add(sesion.company);

            return sesion;
        }

        public static void DevolverSesion (int index)
        {
            Company company = (Company)sesionesUso[index];
            sesionesDisponibles.Enqueue(company);
            sesionesUso.RemoveAt(index);
        }

        public static SessionMetrics GetMetrics()
        {
            SessionMetrics metrics = new SessionMetrics();
            metrics.Disponibles = sesionesDisponibles.Count;
            metrics.EnUso = sesionesUso.Count;

            return metrics;
        }

        public static int Refresh()
        {
            int sesionesMuertas = 0;
            if (sesionesDisponibles.Count > 2)
            {
                for (int i = 0; i < sesionesDisponibles.Count; i++)
                {
                    Company company = sesionesDisponibles.Dequeue();
                    if (company == null)
                    {
                        sesionesMuertas++;
                        continue;
                    }

                    if (company.Connected == false)
                    {
                        sesionesMuertas++;
                        continue;
                    }

                    sesionesDisponibles.Enqueue(company);
                }

                CrearSesiones(sesionesMuertas);
            }

            return sesionesMuertas;
        }

    } // SessionPool
} // UltraorganicsWS.services