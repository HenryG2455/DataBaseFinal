using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poject_3_Gushurst.Models;

namespace Poject_3_Gushurst.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        /// <summary>
        /// Gets all invoices and allows you to search by Invoice attributes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <returns>
        /// The whole list or the list containing the serach params
        /// </returns>
        public ActionResult All(string id, int sortBy = 0)
        {
            var context = new Entities();
            List<Invoice> allInvoices;

            switch (sortBy)
            {

                case 1:
                    {
                        allInvoices = context.Invoices.OrderBy(c => c.CustomerID).ToList();
                        break;
                    }
                case 2:
                    {
                        allInvoices = context.Invoices.OrderBy(c => c.InvoiceDate).ToList();
                        break;
                    }
                case 3:
                    {
                        allInvoices = context.Invoices.OrderBy(c => c.ProductTotal).ToList();
                        break;
                    }
                case 4:
                    {
                        allInvoices = context.Invoices.OrderBy(c => c.SalesTax).ToList();
                        break;
                    }
                case 5:
                    {
                        allInvoices = context.Invoices.OrderBy(c => c.Shipping).ToList();
                        break;
                    }
                case 6:
                    {
                        allInvoices = context.Invoices.OrderBy(c => c.InvoiceTotal).ToList();
                        break;
                    } 
                case 0:
                default:
                    {
                        allInvoices = context.Invoices.OrderBy(c => c.InvoiceID).ToList();
                        break;
                    }
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();
                int zipCodeLookup = 0;
                decimal money = 0.0m;
                if (int.TryParse(id, out zipCodeLookup))
                {
                    allInvoices = allInvoices.Where(c =>
                            c.CustomerID == zipCodeLookup ||
                            c.InvoiceID == zipCodeLookup
                        ).ToList();
                }
                else if(decimal.TryParse(id, out money))
                {
                    allInvoices = allInvoices.Where(c =>
                             c.ProductTotal == money ||
                             c.SalesTax == money ||
                             c.Shipping == money ||
                             c.InvoiceTotal == money
                         ).ToList();
                }
                else
                {
                    allInvoices = allInvoices.Where(c => c.InvoiceDate == DateTime.Parse(id)).ToList();
                }

            }
            return View(allInvoices);
        }


        //Upsert GET
        /// <summary>
        /// can beused for basic adding or can be used for updating
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// the Upsert page to wither add or update invoice
        /// </returns>
        [HttpGet]
        public ActionResult Upsert(int id = 0)
        {
            Entities context = new Entities();
            Invoice invoice = context.Invoices.Where(c => c.InvoiceID == id).FirstOrDefault();
            return View(invoice);
        }


        //Uspert POST
        /// <summary>
        /// Will either add teh new invoice to table or update current invoice
        /// </summary>
        /// <param name="newInvoice"></param>
        /// <returns>
        /// When done return you to the All page</returns>

        [HttpPost]
        public ActionResult Upsert(Invoice newInvoice)
        {
            Entities context = new Entities();
            try
            {
                if (context.Invoices.Where(c => c.InvoiceID == newInvoice.InvoiceID).Count() > 0)
                {
                    var invToSave = context.Invoices.Where(c => c.InvoiceID == newInvoice.InvoiceID).FirstOrDefault();
                    invToSave.CustomerID = newInvoice.CustomerID;
                    invToSave.InvoiceDate = newInvoice.InvoiceDate;
                    invToSave.ProductTotal = newInvoice.ProductTotal;
                    invToSave.SalesTax = newInvoice.SalesTax;
                    invToSave.Shipping = newInvoice.Shipping;
                    invToSave.InvoiceTotal = newInvoice.InvoiceTotal;
                }
                else
                {
                    context.Invoices.Add(newInvoice);
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