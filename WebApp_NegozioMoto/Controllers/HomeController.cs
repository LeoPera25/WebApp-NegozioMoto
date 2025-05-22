using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp_NegozioMoto.Models;
using WebApp_NegozioMoto.Views.Home;

namespace WebApp_NegozioMoto.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISession _session; //variabile che memorizza i dati della sessione

    private GestioneDati gestione;
    private GestioneSessione _gestioneSessione;
    private IConfiguration _conf;

    public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IConfiguration config)
    {
        _logger = logger;
        _session = httpContextAccessor.HttpContext.Session;
        _conf = config;
        
        if (!_session.Keys.Contains("contatore"))
        {
            _session.SetInt32("contatore", 0);
        }

        gestione = new GestioneDati();
        _gestioneSessione = new GestioneSessione(_session);
    }
//Già presente nel file-----------------------------------------------------------
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
//-----------------------------------------------------------
    
//Aggiunto-----------------------------------------------------------  

    // Pagina di login
    public ActionResult Login()
    {
        // Qui potrai aggiungere eventuale logica di autenticazione
        return View();
    }

    public ActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult AggiungiUtente(string username, string password, string email)
    {
        GestioneDati gestione = new GestioneDati();
        bool esito = gestione.InserisciUtente(username, password, email);

        if (esito)
            return RedirectToAction("Login");
        else
        {
            ViewBag.Errore = "Registrazione fallita. Riprova.";
            return View("Register");
        }
    }

    // Pagina Home del sito
    public ActionResult Home()
    {
        // Puoi eventualmente recuperare alcuni dati di presentazione
        return View();
    }

    // Pagina per Gestire il carrello dell'utente
    
    public IActionResult AggiungiAlCarrello(int idProd, int idCat)
    {
        Item i = gestione.RecuperaItem(idProd, idCat);
        Carrello c = _gestioneSessione.PrendiCarrello();
        c.AggiungiItem(i);
        _gestioneSessione.SalvaCarrello(c);
        return RedirectToAction("Cart");
    }
    public IActionResult Cart()
    {
        Carrello c = _gestioneSessione.PrendiCarrello();
        List<Item> list = c.ListaCarrello.ToList();
        return View(list);
    }
    public IActionResult EmptyCart()
    {
        Carrello c = _gestioneSessione.PrendiCarrello();
        c.PulisciCarrello();
        
        return RedirectToAction("Cart", c.ListaCarrello);
    }

    //Visualizza vari elenchi
    public IActionResult ElencoMoto()
    {
        Categoria c = new Categoria();
        List<Item> elementi = gestione.RecuperaTuttiIProdottiDiUnaCategoria(1);
        c.Items = elementi.ToList();
        if (c != null)
        {
            return View(c);
        }
        else
        {
            return View("NotFound");
        }
    }
    
    public IActionResult ElencoAbbigliamento()
    {
        Categoria c = new Categoria();
        List<Item> elementi = gestione.RecuperaTuttiIProdottiDiUnaCategoria(2);
        c.Items = elementi.ToList();
        if (c != null)
        {
            return View(c);
        }
        else
        {
            return View("NotFound");
        }
    }
    
    public IActionResult ElencoAccessori()
    {
        Categoria c = new Categoria();
        List<Item> elementi = gestione.RecuperaTuttiIProdottiDiUnaCategoria(3);
        c.Items = elementi.ToList();
        if (c != null)
        {
            return View(c);
        }
        else
        {
            return View("NotFound");
        }
    }
    
    
    
    
    //Errore-----------------------------------------------------------------------------
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}