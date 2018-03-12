using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using UltraorganicsWS.model;
using UltraorganicsWS.services;

namespace UltraorganicsWS
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class elaboraciones
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public ResultadoVO crearElaboracion(ElaboracionVO elaboracion)
        {
            ElaboracionService elaboracionService = new ElaboracionService();
            ResultadoVO resultadoVO = new ResultadoVO();

            try
            {
                resultadoVO = elaboracionService.crearDocumento(elaboracion);
            }
            catch (Exception ex)
            {
                resultadoVO.Success = false;
                resultadoVO.DocEntry = 0;
                resultadoVO.Mensaje = ex.Message;
            }

            return resultadoVO;
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
