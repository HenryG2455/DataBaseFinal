using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poject_3_Gushurst.Models;


namespace Poject_3_Gushurst.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        /// <summary>
        /// Gets all products and allows you to search and sort by product attributes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <returns>
        /// The whole list or the list containing the serach params
        /// </returns>
        public ActionResult All(string id, int sortBy = 0)
        {
            var context = new Entities();
            List <Product> allProducts;
            switch (sortBy)
            {

                case 1:
                    {
                        allProducts = context.Products.OrderBy(c => c.Description).ToList();
                        break;
                    }
                case 2:
                    {
                        allProducts = context.Products.OrderBy(c => c.UnitPrice).ToList();
                        break;
                    }
                case 3:
                    {
                        allProducts = context.Products.OrderBy(c => c.OnHandQuantity).ToList();
                        break;
                    }

                default:
                    {
                        allProducts = context.Products.OrderBy(c => c.ProductCode).ToList();
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
                    allProducts = allProducts.Where(c =>
                            c.OnHandQuantity == zipCodeLookup 
                        ).ToList();
                }
                else if (decimal.TryParse(id, out money))
                {
                    allProducts = allProducts.Where(c =>
                             c.UnitPrice == money
                         ).ToList();
                }
                else
                {
                    allProducts = allProducts.Where(c =>
                    c.Description.ToLower().Contains(id) ||
                            c.ProductCode.ToLower().Contains(id)
                    ).ToList();
                }

            }
            return View(allProducts);
        }

        //Upsert GET
        /// <summary>
        /// can be used for adding or can be used for updating items in products table
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// the Upsert page to wither add or update product
        /// </returns>

        [HttpGet]
        public ActionResult Upsert(string id)
        {
            Entities context = new Entities();
            Product product = context.Products.Where(c => c.ProductCode == id).FirstOrDefault();
            return View(product);
        }

        //Upsert GET
        /// <summary>
        /// Will either add the new product to table or update current product
        /// </summary>
        /// <param name="newproduct"></param>
        /// <returns>
        /// Return you back to the all page after completion
        /// </returns>
        [HttpPost]
        public ActionResult Upsert(Product newproduct)
        {
            Entities context = new Entities();
            try
            {
                if (context.Products.Where(c => c.ProductCode == newproduct.ProductCode).Count() > 0)
                {
                    var invToSave = context.Products.Where(c => c.ProductCode == newproduct.ProductCode).FirstOrDefault();
                    invToSave.Description = newproduct.Description;
                    invToSave.UnitPrice = newproduct.UnitPrice;
                    invToSave.OnHandQuantity = newproduct.OnHandQuantity;

                }
                else
                {
                    context.Products.Add(newproduct);
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
                try
                {
                    Product prod = context.Products.Where(c => c.ProductCode == id).FirstOrDefault();
                    context.Products.Remove(prod);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            return RedirectToAction("All");
        }
    }
}