using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using UltraorganicsWS.services;

namespace UltraorganicsWS
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class sessionpool
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        [WebGet]
        public SessionMetrics status()
        {
            SessionMetrics metrics = SessionPool.GetMetrics();

            return metrics;
        }

        [OperationContract]
        [WebGet]
        public String refresh()
        {
            int sesionesMuertas = SessionPool.Refresh();

            return "Sesiones recuperadas: " + sesionesMuertas;
        }

        [OperationContract]
        [WebGet]
        public SessionMetrics reiniciar()
        {
            return SessionPool.Reiniciar();
        }

    }
}
