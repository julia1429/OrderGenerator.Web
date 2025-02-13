using Microsoft.AspNetCore.Mvc;
using OrderGenerator.Web.Services;
using OrderGenerator.Web.Services.Interfaces;

namespace OrderGenerator.Web.Controllers
{
    public class OrderController : Controller 
    {
        private readonly IFixInitiator _fixInitiator;

        public OrderController(IFixInitiator fixInitiator)
        {
            _fixInitiator = fixInitiator;
        }

        [HttpPost]
        public IActionResult CreateOrder(string symbol, string side, int quantity, decimal price)
        {
            _fixInitiator.SendNewOrder(symbol, side, quantity, price);
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
