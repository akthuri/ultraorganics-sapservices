using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SAPbobsCOM;
using UltraorganicsWS.model;

namespace UltraorganicsWS.services
{
    /// <summary>
    /// Documento abstracto que contiene la funcionalidad base
    /// para todos los documentos
    /// </summary>
    public abstract class DocumentoService
    {
        protected ResultadoVO resultadoVO = null;
        protected DocumentoVO documentoVO = null;
        protected SAPbobsCOM.Company company = null;
        protected string codigoError = "";
        protected string mensajeError = "";

	    public DocumentoService()
	    {
            resultadoVO = new ResultadoVO();
            resultadoVO.Success = false;
            resultadoVO.Mensaje = "Mensaje Inicial";
            resultadoVO.DocEntry = 0;
            resultadoVO.DocNum = 0;
	    }

        private void Conectar()
        {
            if (company == null)
            {
                company = new SAPbobsCOM.Company();
            }

            company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
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
                this.mensajeError = company.GetLastErrorDescription();
            }

        } // Conectar

        public ResultadoVO crearDocumento(DocumentoVO documento)
        {
            Sesion sesion = SessionPool.getSession();
            this.company = sesion.company;

            this.documentoVO = documento;

            company.StartTransaction();
            try
            {
                CrearDocumentoSAP();

                if (company.InTransaction)
                {
                    if (this.resultadoVO.Success) company.EndTransaction(BoWfTransOpt.wf_Commit);
                    else company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
            }
            catch (Exception e)
            {
                if (company.InTransaction) company.EndTransaction(BoWfTransOpt.wf_RollBack);
                Console.WriteLine(e);
                throw e;
            }            

            sesion.Close();

            return this.resultadoVO;
        }

        protected abstract void CrearDocumentoSAP();

        protected void ObtenerResultado(bool exito)
        {
            if (exito)
            {
                string docEntry = this.company.GetNewObjectKey();
                resultadoVO = new ResultadoVO();
                resultadoVO.Success = true;
                resultadoVO.DocEntry = int.Parse(docEntry);
                resultadoVO.Mensaje = "";
            }
            else
            {
                raiseError();
            }
        }

        protected void raiseError()
        {
            resultadoVO = new ResultadoVO();
            resultadoVO.Success = false;
            resultadoVO.Mensaje = company.GetLastErrorDescription();
        }

        private void Desconectar()
        {
            if (this.company != null && this.company.Connected)
            {
                this.company.Disconnect();
            }
        }

    } // DocumentoService
} // UltraorganicsWS.services