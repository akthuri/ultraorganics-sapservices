using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAPbobsCOM;
using UltraorganicsWS.model;

namespace UltraorganicsWS.services
{
    /// <summary>
    /// Genera una Orden de Fabricación y después un Recibo de PT
    /// en el módulo de producción de SAP
    /// Se utiliza para la Elaboración de Productos del Portal
    /// </summary>
    public class ElaboracionService : DocumentoService
    {
        private const int BASETYPE_ORDEN_PRODUCCION = 202;
        private ElaboracionVO elaboracionVO;
        private string _docEntryOf = "";
        private string _docNumOf = "";
        private string _docEntryRp = "";
        private string _docNumRp = "";

        protected override void CrearDocumentoSAP()
        {
            this.elaboracionVO = (ElaboracionVO)this.documentoVO;

            bool error = false;
            foreach (ElaboracionPartidaVO itemVO in elaboracionVO.partidas)
            {
                int docEntry = CrearOrden(itemVO, elaboracionVO.WhsCode);

                if (docEntry == -1)
                {
                    error = true;
                    break;
                }

                RecibirProductoTerminado(docEntry);
                if (resultadoVO.Success == false)
                {
                    error = true;
                    break;
                }

                CerrarOrdenFabricacion(docEntry);
            }

            if (error == false)
            {
                this.resultadoVO = new ResultadoVO();
                this.resultadoVO.Success = true;
                this.resultadoVO.Mensaje = "";
                this.resultadoVO.DocEntry = 0;
                this.resultadoVO.DocNum = 0;
            }
        }

        private int CrearOrden(ElaboracionPartidaVO itemVO, String codigoAlmacen)
        {
            int docEntry = -1;
            ProductionOrders order = (ProductionOrders)company.GetBusinessObject(BoObjectTypes.oProductionOrders);

            order.Warehouse = codigoAlmacen;
            order.ItemNo = itemVO.ItemCode;
            order.DueDate = DateTime.Today;
            order.PlannedQuantity = itemVO.Cantidad;
            String referencia = "Generado desde el Portal";
            order.Remarks = referencia;
            order.JournalRemarks = referencia;

            if (order.Add() == 0)
            {
                string strDocEntry = company.GetNewObjectKey();
                docEntry = int.Parse(strDocEntry);

                order.GetByKey(docEntry);
                order.ProductionOrderStatus = BoProductionOrderStatusEnum.boposReleased;

                this._docEntryOf = strDocEntry;
                this._docNumOf = order.DocumentNumber.ToString();

                for (int i = 0; i < order.Lines.Count; i++)
                {
                    order.Lines.SetCurrentLine(i);
                    order.Lines.Warehouse = codigoAlmacen;
                }

                if (order.Update() != 0)
                {
                    raiseError();
                }
            }
            else
            {
                raiseError();
            }

            return docEntry;
        } // CrearOrden

        private void RecibirProductoTerminado(int docEntryOrden)
        {
            Documents reciboProduccion = (Documents)company.GetBusinessObject(BoObjectTypes.oInventoryGenEntry);

            String referencia = "Generado desde el Portal";

            reciboProduccion.Comments = referencia;
            reciboProduccion.JournalMemo = referencia;

            reciboProduccion.Lines.BaseType = BASETYPE_ORDEN_PRODUCCION;
            reciboProduccion.Lines.BaseEntry = docEntryOrden;

            if (reciboProduccion.Add() == 0)
            {
                string docEntry = company.GetNewObjectKey();
                reciboProduccion.GetByKey(int.Parse(docEntry));
                _docEntryRp = docEntry;
                _docNumRp = reciboProduccion.DocNum.ToString();
            }
            else
            {
                raiseError();
            }
        } // RecibirProductoTerminado

        private void CerrarOrdenFabricacion(int docEntry)
        {
            ProductionOrders order = (ProductionOrders)company.GetBusinessObject(BoObjectTypes.oProductionOrders);
            if (order.GetByKey(docEntry))
            {
                order.ProductionOrderStatus = BoProductionOrderStatusEnum.boposClosed;

                if (order.Update() != 0)
                {
                    raiseError();
                }
            }

        } // CerrarOrdenFabricacion

    } // ElaboracionService
} // UltraorganicsWS.services