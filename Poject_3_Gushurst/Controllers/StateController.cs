using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poject_3_Gushurst.Models;

namespace Poject_3_Gushurst.Controllers
{
    public class StateController : Controller
    {
        // GET: State
        /// <summary>
        /// Gets all States and allows you to search and sort by State attributes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortBy"> 0-1 </param>
        /// <returns>
        /// The whole list or the list containing the serach params
        /// </returns>
        public ActionResult All(string id, int sortBy = 0)
        {
            var context = new Entities();
            List<State> allStates;
            switch (sortBy)
            {
                case 1:
                    {
                        allStates = context.States.OrderBy(c => c.StateName).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        allStates = context.States.OrderBy(c => c.StateCode).ToList();
                        break;
                    }
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();
                int zipCodeLookup = 0;
                decimal money = 0.0m;
                allStates = allStates.Where(c =>
                    c.StateCode.ToLower().Contains(id) ||
                            c.StateName.ToLower().Contains(id)
                    ).ToList();

            }

            return View(allStates);
        }

        //Upsert GET
        /// <summary>
        /// can beused for basic adding or can be used for updating
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// the Upsert page to either add or update State
        /// </returns>
        [HttpGet]
        public ActionResult Upsert(string id)
        {
            Entities context = new Entities();
            State state = context.States.Where(c => c.StateCode == id).FirstOrDefault();
            return View(state);
        }

        //Upsert POST
        /// <summary>
        /// Will either add the new state or update current state
        /// </summary>
        /// <param name="newState"></param>
        /// <returns>
        /// Back to All after completion
        /// </returns>
        [HttpPost]
        public ActionResult Upsert(State newState)
        {
            Entities context = new Entities();
            try
            {
                if (context.States.Where(c => c.StateCode == newState.StateCode).Count() > 0)
                {
                    var invToSave = context.States.Where(c => c.StateCode == newState.StateCode).FirstOrDefault();
                    invToSave.StateName = newState.StateName;
                }
                else
                {
                    context.States.Add(newState);
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
                try
                {
                    State state = context.States.Where(c => c.StateCode == id).FirstOrDefault();
                    context.States.Remove(state);
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