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
    public class PlayersController : Controller {
  const string BASE_URL = "https://statsapi.web.nhl.com/api/v1/people";
  private readonly ILogger<PlayersController> _logger;
  private readonly IHttpClientFactory _clientFactory;
  public IEnumerable<Player> Players { get; set; }
  public bool GetPlayersError { get; private set; }

  public PlayersController(ILogger<PlayersController> logger, IHttpClientFactory clientFactory)
  {
    _logger = logger;
    _clientFactory = clientFactory;
  }

  public async Task<IActionResult> Index()
  {
    var message = new HttpRequestMessage();
    message.Method = HttpMethod.Get;
    message.RequestUri = new Uri($"{BASE_URL}api/people");
    message.Headers.Add("Accept", "application/json");

    var client = _clientFactory.CreateClient();

    var response = await client.SendAsync(message);

    if (response.IsSuccessStatusCode) {
        using var responseStream = await response.Content.ReadAsStreamAsync();
        Players = await JsonSerializer.DeserializeAsync<IEnumerable<Player>>(responseStream);
    } else {
        GetPlayersError = true;
        Players = Array.Empty<Player>();
    }

    return View(Players);
  }

  public async Task<IActionResult> Details(string id) {
    if (id == null)
      return NotFound();

    var message = new HttpRequestMessage();
    message.Method = HttpMethod.Get;
    message.RequestUri = new Uri($"{BASE_URL}api/people/{id}");
    message.Headers.Add("Accept", "application/json");

    var client = _clientFactory.CreateClient();

    var response = await client.SendAsync(message);

    Player Player = null;

    if (response.IsSuccessStatusCode) {
      using var responseStream = await response.Content.ReadAsStreamAsync();
      Player = await JsonSerializer.DeserializeAsync<Player>(responseStream);
    } else {
      GetPlayersError = true;
    }

    if (Player == null)
      return NotFound();

    return View(Player);
  }

  // GET: Players/Create
  public IActionResult Create() {
    return View();
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create([Bind("PlayerId,firstName,lastName,school")] Player Player)
  {
    if (ModelState.IsValid) {
      HttpContent httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(Player), Encoding.UTF8);
      httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
      
      var message = new HttpRequestMessage();
      message.Content = httpContent;
      message.Method = HttpMethod.Post;
      message.RequestUri = new Uri($"{BASE_URL}api/people");

      HttpClient client = _clientFactory.CreateClient();
      HttpResponseMessage response = await client.SendAsync(message);

      var result = await response.Content.ReadAsStringAsync();

      return RedirectToAction(nameof(Index));
    }
    return View(Player);
  }

  public async Task<IActionResult> Edit(string id) {
    if (id == null)
      return NotFound();

    var message = new HttpRequestMessage();
    message.Method = HttpMethod.Get;
    message.RequestUri = new Uri($"{BASE_URL}api/people/{id}");
    message.Headers.Add("Accept", "application/json");

    var client = _clientFactory.CreateClient();

    var response = await client.SendAsync(message);

    Player Player = null;

    if (response.IsSuccessStatusCode) {
      using var responseStream = await response.Content.ReadAsStreamAsync();
      Player = await JsonSerializer.DeserializeAsync<Player>(responseStream);
    } else {
      GetPlayersError = true;
    }

    if (Player == null)
      return NotFound();

    return View(Player);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(string id, [Bind("PlayerId,firstName,lastName,school")] Player Player)
  {
    if (id != Player.playerId)
      return NotFound();

    if (ModelState.IsValid) {
      HttpContent httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(Player), Encoding.UTF8);
      httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

      var message = new HttpRequestMessage();
      message.Content = httpContent;
      message.Method = HttpMethod.Put;
      message.RequestUri = new Uri($"{BASE_URL}api/people/{id}");

      HttpClient client = _clientFactory.CreateClient();
      HttpResponseMessage response = await client.SendAsync(message);

      var result = await response.Content.ReadAsStringAsync();

      return RedirectToAction(nameof(Index));
    }
    return View(Player);
  }

  // GET: Players/Delete/5
  public async Task<IActionResult> Delete(string id) {
    if (id == null)
      return NotFound();

    var message = new HttpRequestMessage();
    message.Method = HttpMethod.Get;
    message.RequestUri = new Uri($"{BASE_URL}api/people/{id}");
    message.Headers.Add("Accept", "application/json");

    var client = _clientFactory.CreateClient();

    var response = await client.SendAsync(message);

    Player Player = null;

    if (response.IsSuccessStatusCode) {
      using var responseStream = await response.Content.ReadAsStreamAsync();
      Player = await JsonSerializer.DeserializeAsync<Player>(responseStream);
    } else {
        GetPlayersError = true;
    }

    if (Player == null)
      return NotFound();

    return View(Player);
  }

  [HttpPost, ActionName("Delete")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DeleteConfirmed(string id) {
    var message = new HttpRequestMessage();
    message.Method = HttpMethod.Delete;
    message.RequestUri = new Uri($"{BASE_URL}api/people/{id}");

    HttpClient client = _clientFactory.CreateClient();
    HttpResponseMessage response = await client.SendAsync(message);

    var result = await response.Content.ReadAsStringAsync();

    return RedirectToAction(nameof(Index));
  }
}
}