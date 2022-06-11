using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poject_3_Gushurst.Models;

namespace Poject_3_Gushurst.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        /// <summary>
        /// Gets all OrderOptions and allows you to search by OrderOptions attributes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <returns>
        /// The whole list or the list containing the serach params
        /// </returns>
        public ActionResult All(string id, int sortBy = 0)
        {
            var context = new Entities();
            List<OrderOption> allOptions;
            switch (sortBy)
            {

                case 1:
                    {
                        allOptions = context.OrderOptions.OrderBy(c => c.FirstBookShipCharge).ToList();
                        break;
                    }
                case 2:
                    {
                        allOptions = context.OrderOptions.OrderBy(c => c.AdditionalBookShipCharge).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        allOptions = context.OrderOptions.OrderBy(c => c.SalesTaxRate).ToList();
                        break;
                    }
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();
                decimal money = 0.0m;
                if (decimal.TryParse(id, out money))
                {
                    allOptions = allOptions.Where(c =>
                             c.SalesTaxRate == money ||
                             c.FirstBookShipCharge == money ||
                             c.AdditionalBookShipCharge == money
                         ).ToList();
                }

            }
            return View(allOptions);
        }

        //upsert GET
        /// <summary>
        /// take be blank or not but takes in item by id and can add or update
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// The Upsert page with either It filled out for updating or empty for new add
        /// </returns>

        [HttpGet]
        public ActionResult Upsert(decimal id = 0.0m)
        {
            Entities context = new Entities();
            OrderOption order = context.OrderOptions.Where(c => c.AdditionalBookShipCharge == id).FirstOrDefault();
            return View(order);
        }

        //Upsert POST
        /// <summary>
        /// Wither Either Update the item to the list or will Create new Item
        /// </summary>
        /// <param name="newOption"></param>
        /// <returns>
        /// returns you to all Page after completion
        /// </returns>

        [HttpPost]
        public ActionResult Upsert(OrderOption newOption)
        {
            Entities context = new Entities();
            try
            {
                if (context.OrderOptions.Where(c => c.AdditionalBookShipCharge == newOption.AdditionalBookShipCharge).Count() > 0)
                {
                    var orderToSave = context.OrderOptions.Where(c => c.AdditionalBookShipCharge == newOption.AdditionalBookShipCharge).FirstOrDefault();
                    orderToSave.FirstBookShipCharge = newOption.FirstBookShipCharge;
                    orderToSave.AdditionalBookShipCharge = newOption.AdditionalBookShipCharge;

                }
                else
                {
                    context.OrderOptions.Add(newOption);
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
        /// Takes in the id of the item to be deleted 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Back to All after Success 
        /// </returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            Entities context = new Entities();
            decimal invoiceId = 0.0m;
            if (decimal.TryParse(id, out invoiceId))
            {
                try
                {
                    OrderOption opt = context.OrderOptions.Where(c => c.AdditionalBookShipCharge == invoiceId).FirstOrDefault();
                    context.OrderOptions.Remove(opt);
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