using drew_blackjack_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using drew_blackjack_mvc.Extensions;

namespace drew_blackjack_mvc.Controllers
{
    public class BlackjackController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new BlackjackViewModel();
            HttpContext.Session.SetObject("BlackjackViewModel", viewModel);

            viewModel.BuildDeck();
            viewModel.ShuffleDeck();
            viewModel.DealInitialCards();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Hit()
        {
            var viewModel = HttpContext.Session.GetObject<BlackjackViewModel>("BlackjackViewModel");
            viewModel.GivePlayerCard();
            HttpContext.Session.SetObject("BlackjackViewModel", viewModel);
            return Json(viewModel);
        }

        [HttpPost]
        public IActionResult Stay()
        {
            var viewModel = HttpContext.Session.GetObject<BlackjackViewModel>("BlackjackViewModel");
            viewModel.Stay();
            HttpContext.Session.SetObject("BlackjackViewModel", viewModel);
            return Json(viewModel);
        }

        [HttpGet]
        [Route("Blackjack/GetGameState")]
        public IActionResult GetGameState()
        {
            var viewModel = HttpContext.Session.GetObject<BlackjackViewModel>("BlackjackViewModel");

            // Check if the viewModel is null, and initialize it if necessary
            if (viewModel == null)
            {
                viewModel = new BlackjackViewModel();
                HttpContext.Session.SetObject("BlackjackViewModel", viewModel);
            }

            // Serialize the viewModel object
            var jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            var json = JsonConvert.SerializeObject(viewModel, jsonSettings);

            return Content(json, "application/json");
        }
        [HttpPost]
        public IActionResult Init()
        {
            var viewModel = HttpContext.Session.GetObject<BlackjackViewModel>("BlackjackViewModel");

            if (viewModel == null)
            {
                viewModel = new BlackjackViewModel();
                HttpContext.Session.SetObject("BlackjackViewModel", viewModel);
            }

            viewModel.BuildDeck();
            viewModel.ShuffleDeck();
            viewModel.DealInitialCards();

            Console.WriteLine("Initialized game state:");
            Console.WriteLine(JsonConvert.SerializeObject(viewModel, Formatting.Indented));

            HttpContext.Session.SetObject("BlackjackViewModel", viewModel);
            return Json(viewModel);
        }



    }
}
