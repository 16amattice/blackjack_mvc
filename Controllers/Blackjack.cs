using drew_blackjack_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace drew_blackjack_mvc.Controllers
{
    public class BlackjackController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new BlackjackViewModel();
            HttpContext.Session.SetObject("BlackjackViewModel", viewModel);
            StartGame(viewModel);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Hit()
        {
            var viewModel = HttpContext.Session.GetObject<BlackjackViewModel>("BlackjackViewModel");
            viewModel.GivePlayerCard();
            HttpContext.Session.SetObject("BlackjackViewModel", viewModel);

            return PartialView("_BlackjackTable", viewModel);
        }

        [HttpGet]
        public IActionResult Stay()
        {
            var viewModel = HttpContext.Session.GetObject<BlackjackViewModel>("BlackjackViewModel");
            viewModel.Stay();
            HttpContext.Session.SetObject("BlackjackViewModel", viewModel);

            return PartialView("_BlackjackTable", viewModel);
        }

        [HttpGet]
        public IActionResult Restart()
        {
            var viewModel = new BlackjackViewModel();
            HttpContext.Session.SetObject("BlackjackViewModel", viewModel);
            StartGame(viewModel);

            return PartialView("_BlackjackTable", viewModel);
        }

        private void StartGame(BlackjackViewModel viewModel)
        {
            viewModel.BuildDeck();
            viewModel.ShuffleDeck();
            viewModel.DealInitialCards();
        }
        [HttpGet("GetGameState")]
        public IActionResult GetGameState()
        {
            var viewModel = HttpContext.Session.GetObject<BlackjackViewModel>("BlackjackViewModel");
            return Json(viewModel);
        }

    }
}
