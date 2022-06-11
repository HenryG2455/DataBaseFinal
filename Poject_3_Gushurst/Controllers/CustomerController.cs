using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poject_3_Gushurst.Models;

namespace Poject_3_Gushurst.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        /// <summary>
        ///  will serach for Customer attributes and display all csutomers
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"></param>
        /// <returns> List of all Customers</returns>
        public ActionResult All(string id, int sortBy = 0)
        {
            var context = new Entities();
            List<Customer> allCustomers;

            //Sort By Columns
            switch (sortBy)
            {

                case 1:
                    {
                        allCustomers = context.Customers.OrderBy(c => c.Name).ToList();
                        break;
                    }
                case 2:
                    {
                        allCustomers = context.Customers.OrderBy(c => c.Address).ToList();
                        break;
                    }
                case 3:
                    {
                        allCustomers = context.Customers.OrderBy(c => c.City).ToList();
                        break;
                    }
                case 4:
                    {
                        allCustomers = context.Customers.OrderBy(c => c.State).ToList();
                        break;
                    }
                case 5:
                    {
                        allCustomers = context.Customers.OrderBy(c => c.ZipCode).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        allCustomers = context.Customers.OrderBy(c => c.CustomerID).ToList();
                        break;
                    }
            }

            //Search
            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();
                int zipCodeLookup = 0;
                if (int.TryParse(id, out zipCodeLookup))
                {
                    allCustomers = allCustomers.Where(c =>
                            c.CustomerID == zipCodeLookup 
                        ).ToList();
                }
                else
                {
                    allCustomers = allCustomers.Where(c =>
                            c.Name.ToLower().Contains(id) ||
                            c.Address.ToLower().Contains(id) ||
                            c.City.ToLower().Contains(id) ||
                            c.State.ToLower().Contains(id) ||
                            c.ZipCode.ToLower().Contains(id)
                        ).ToList();
                }

            }

            return View(allCustomers);
        }

        //Upsert GET
        /// <summary>
        /// Can be used with no informaion or With informaion 
        /// to update or add a customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Upsert Method for adding or updating
        /// </returns>
        [HttpGet]
        public ActionResult Upsert(int id = 0)
        {
            Entities context = new Entities();
            Customer cust = context.Customers.Where(c => c.CustomerID == id).FirstOrDefault();
            List<State> states = context.States.ToList();

            UpsertCustomerModel viewModel = new UpsertCustomerModel
            {
                Customer = cust,
                States = states
            };


            return View(viewModel);
        }


        //Upsert POST
        /// <summary>
        /// Used for adding the customer in the upsert page
        /// and will save or add the customer 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// will return you back to the all page once completed
        /// </returns>
        [HttpPost]
        public ActionResult Upsert(UpsertCustomerModel model)
        {

            Customer newCustomer = model.Customer;
            Entities context = new Entities();
            try
            {
                if (context.Customers.Where(c => c.CustomerID == newCustomer.CustomerID).Count() > 0)
                {
                    var custToSave = context.Customers.Where(c => c.CustomerID == newCustomer.CustomerID).FirstOrDefault();
                    custToSave.Name = newCustomer.Name;
                    custToSave.Address = newCustomer.Address;
                    custToSave.City = newCustomer.City;
                    custToSave.State = newCustomer.State;
                    custToSave.ZipCode = newCustomer.ZipCode;
                }
                else
                {
                    context.Customers.Add(newCustomer);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("All");
        }

        //Delete GET
        /// <summary>
        /// Will delete the Customer from the table
        /// </summary>
        /// <param name="id"></param>
        /// <returns> back to all </returns>

        [HttpGet]
        public ActionResult Delete(string id)
        {
            Entities context = new Entities();
            int customerId = 0;
            if(int.TryParse(id,out customerId))
            {
                try
                {
                    Customer cust = context.Customers.Where(c => c.CustomerID == customerId).FirstOrDefault();
                    context.Customers.Remove(cust);
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