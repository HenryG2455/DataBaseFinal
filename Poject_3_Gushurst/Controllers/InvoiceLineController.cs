using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poject_3_Gushurst.Models;

namespace Poject_3_Gushurst.Controllers
{
    public class InvoiceLineController : Controller
    {
        // GET: InvoiceLine
        /// <summary>
        /// Gets all invoices and allows you to search by InvoiceLine attributes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <returns>
        /// The whole list or the list containing the serach params
        /// </returns>
        public ActionResult All(string id, int sortBy = 0)
        {
            var context = new Entities();
            List<InvoiceLineItem> allLineItem;
            switch (sortBy)
            {

                case 1:
                    {
                        allLineItem = context.InvoiceLineItems.OrderBy(c => c.ProductCode).ToList();
                        break;
                    }
                case 2:
                    {
                        allLineItem = context.InvoiceLineItems.OrderBy(c => c.UnitPrice).ToList();
                        break;
                    }
                case 3:
                    {
                        allLineItem = context.InvoiceLineItems.OrderBy(c => c.Quantity).ToList();
                        break;
                    }
                case 4:
                    {
                        allLineItem = context.InvoiceLineItems.OrderBy(c => c.ItemTotal).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        allLineItem = context.InvoiceLineItems.OrderBy(c => c.InvoiceID).ToList();
                        break;
                    }
            }

            //Search
            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();
                int zipCodeLookup = 0;
                decimal deci = 0.0m;
                if (int.TryParse(id, out zipCodeLookup))
                {
                    allLineItem = allLineItem.Where(c =>
                            c.InvoiceID == zipCodeLookup ||
                            c.Quantity == zipCodeLookup
                        ).ToList();
                }
                else if(decimal.TryParse(id, out deci))
                {
                    allLineItem = allLineItem.Where(c =>
                            c.ItemTotal == deci ||
                            c.UnitPrice == deci
                        ).ToList();
                }
                else
                {
                    allLineItem = allLineItem.Where(c => c.ProductCode.ToLower().Contains(id)).ToList();
                }

            }

            return View(allLineItem);
        }

        //Upsert GET
        /// <summary>
        /// can beused for basic adding or can be used for updating
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// the Upsert page to either add or update invoiceLine
        /// </returns>
        [HttpGet]
        public ActionResult Upsert(int id = 0)
        {
            Entities context = new Entities();
            InvoiceLineItem invoiceLine = context.InvoiceLineItems.Where(c => c.InvoiceID == id).FirstOrDefault();
            return View(invoiceLine);
        }
        //Upsert Post
        /// <summary>
        /// Will either add teh new invoiceLine to table or update current invoiceLine
        /// </summary>
        /// <param name="newInvoice"></param>
        /// <returns>
        /// back to All after completeion
        /// </returns>
        [HttpPost]
        public ActionResult Upsert(InvoiceLineItem newInvoice)
        {
            Entities context = new Entities();
            try
            {
                if (context.InvoiceLineItems.Where(c => c.InvoiceID == newInvoice.InvoiceID).Count() > 0)
                {
                    var invToSave = context.InvoiceLineItems.Where(c => c.InvoiceID == newInvoice.InvoiceID).FirstOrDefault();
                    invToSave.ProductCode = newInvoice.ProductCode;
                    invToSave.UnitPrice = newInvoice.UnitPrice;
                    invToSave.Quantity = newInvoice.Quantity;
                    invToSave.ItemTotal = newInvoice.ItemTotal;
                }
                else
                {
                    context.InvoiceLineItems.Add(newInvoice);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("All");
        }


        //DELETE GET
        /// <summary>
        /// Takes in the id of teh item to be deleted 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Back to All after Success 
        /// </returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            Entities context = new Entities();
            int invoiceId = 0;
            if (int.TryParse(id, out invoiceId))
            {
                try
                {
                    Invoice inv = context.Invoices.Where(c => c.InvoiceID == invoiceId).FirstOrDefault();
                    context.Invoices.Remove(inv);
                    context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return RedirectToAction("All");
        }

    }
}