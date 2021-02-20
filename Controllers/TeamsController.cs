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
    public class TeamsController : Controller {
  const string BASE_URL = "https://statsapi.web.nhl.com/api/v1/teams";
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
    message.RequestUri = new Uri($"{BASE_URL}");
    message.Headers.Add("Accept", "application/json");

    var client = _clientFactory.CreateClient();
    var response = await client.SendAsync(message);

    if (response.IsSuccessStatusCode) {
        using var responseStream = await response.Content.ReadAsStreamAsync();
        TeamList = await JsonSerializer.DeserializeAsync<Teams>(responseStream);
    } else {
        GetTeamsError = true;
    }

    return View(TeamList.teams);
  }

  public async Task<IActionResult> Details(string id) {
    if (id == null)
      return NotFound();

    var message = new HttpRequestMessage();
    message.Method = HttpMethod.Get;
    message.RequestUri = new Uri($"{BASE_URL}/{id}");
    message.Headers.Add("Accept", "application/json");

    var client = _clientFactory.CreateClient();

    var response = await client.SendAsync(message);

    Team Team = null;

    if (response.IsSuccessStatusCode) {
      using var responseStream = await response.Content.ReadAsStreamAsync();
      Team = await JsonSerializer.DeserializeAsync<Team>(responseStream);
    } else {
      GetTeamsError = true;
    }

    if (Team == null)
      return NotFound();

    return View(Team);
  }
    }
    }