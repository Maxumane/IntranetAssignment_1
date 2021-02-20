using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using consumeHockeyAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace consumeHockeyAPI.Controllers
{
    public class TeamsController : Controller
    {
        const string BASE_URL = "https://statsapi.web.nhl.com/api/v1";
        private readonly ILogger<TeamsController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        public Teams TeamList { get; set; }
        public bool GetTeamsError { get; private set; }

        public TeamsController(ILogger<TeamsController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri($"{BASE_URL}/teams");
            message.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(message);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                TeamList = await JsonSerializer.DeserializeAsync<Teams>(responseStream);
            }
            else
            {
                GetTeamsError = true;
            }

            return View(TeamList.teams);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri($"{BASE_URL}/teams/{id}/roster");
            message.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(message);

            Roster tempRoster = new Roster();

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                tempRoster = await JsonSerializer.DeserializeAsync<Roster>(responseStream);
            }
            else
            {
                GetTeamsError = true;
            }

            if (tempRoster == null)
                return NotFound();

            return View(tempRoster.roster);
        }
        public async Task<IActionResult> PlayerDetails(string id)
        {
            if (id == null)
                return NotFound();

            var message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri($"{BASE_URL}/people/{id}");
            message.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(message);
            Details tempDetails = new Details();

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                tempDetails = await JsonSerializer.DeserializeAsync<Details>(responseStream);
            }
            else
            {
                GetTeamsError = true;
            }

            if (tempDetails == null)
                return NotFound();

            return View(tempDetails.people);
        }
    }
}