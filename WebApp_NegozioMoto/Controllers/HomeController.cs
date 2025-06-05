using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NToastNotify;
using WebApp_NegozioMoto.Models;
using WebApp_NegozioMoto.Views.Home;

namespace WebApp_NegozioMoto.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISession _session; //variabile che memorizza i dati della sessione

    private GestioneDati gestione;
    private GestioneSessione _gestioneSessione;
    private readonly IToastNotification _toastNotification;
    private IConfiguration _conf;

    public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor,IToastNotification toastNotification, IConfiguration config)
    {
        _logger = logger;
        _session = httpContextAccessor.HttpContext.Session;
        _conf = config;
        _toastNotification = toastNotification;
        
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
    public IActionResult Login(string username, string password)
    {
        GestioneDati dati = new GestioneDati();
        bool esito = dati.LoginUtente(username, password);

        if (esito) // o qualsiasi criterio usi per riconoscere l'admin
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            _toastNotification.AddSuccessToastMessage("Login effettuato come Admin");
            return View("Home");
        }
        else
        {
            ViewBag.Errore = "Username o password non validi";
            _toastNotification.AddSuccessToastMessage("Username o password non validi");
            return View();
        }
    }

    public IActionResult PaginaAdmin()
    {
        return View("AdminPage");
    }

    public IActionResult Logout()
    {
        // Cancella tutte le chiavi della sessione
        HttpContext.Session.Clear();

        // Reindirizza alla homepage (o dove preferisci)
        return RedirectToAction("Home", "Home");
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
    
    //funzioni admin
    
    /*public IActionResult ElencoOrdini()
    {
        var ordini = List<OrdineProdotto>();
        ordini = gestione.ElencoOrdini(ordini);

        return View(ordini);
    }
    public IActionResult Dettaglio(int id)
    {
        var prodotti = new List<Prodotto>();
        using var conn = new MySqlConnection(connString);
        conn.Open();

        var cmd = new MySqlCommand("SELECT id_prodotto, id_categoria, quantita FROM ordine_prodotto WHERE id_ordine = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            prodotti.Add(new Prodotto
            {
                IdProdotto = reader.GetInt32(0),
                IdCategoria = reader.GetInt32(1),
                Quantita = reader.GetInt32(2)
            });
        }

        var dettaglio = new OrdineDettaglio
        {
            IdOrdine = id,
            Prodotti = prodotti
        };

        return View(dettaglio);
    }
*/
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
        _toastNotification.AddSuccessToastMessage("Elemento aggiunto al carrello");

        // Ottieni l'URL della pagina precedente dall'header "Referer"
        string refererUrl = Request.Headers["Referer"].ToString();
    
        // Se l'header è presente, redirect alla pagina precedente
        if (!string.IsNullOrEmpty(refererUrl))
        {
            return Redirect(refererUrl);
        }
        else
        {
            // Fallback a un'azione di default (es. homepage)
            return RedirectToAction("Index", "Home");
        }
    }
    public IActionResult Cart()
    {
        Carrello c = _gestioneSessione.PrendiCarrello();
        List<Item> carrelloOriginale = c.ListaCarrello;

        List<(int categoria, int idProdotto)> carrelloPerSuggerimenti = carrelloOriginale
            .Select(item => (item.ID_categoria, item.ID_prodotto))
            .ToList();

        List<Item> suggeriti = gestione.SuggerisciProdottiAssociati(carrelloPerSuggerimenti);

        var model = new CarrelloViewModel
        {
            Carrello = carrelloOriginale,
            Suggeriti = suggeriti
        };

        return View(model);
    }

    
    public IActionResult EmptyCart()
    {
        Carrello c = _gestioneSessione.PrendiCarrello();
        c.PulisciCarrello();
        _gestioneSessione.SalvaCarrello(c);
        return RedirectToAction("Cart", c.ListaCarrello);
    }
    
    [HttpPost]
    public IActionResult RemoveFromCart(int id)
    {
        Carrello c = _gestioneSessione.PrendiCarrello();

        // Cerca l'item da rimuovere in base a id_prodotto
        var itemDaRimuovere = c.ListaCarrello.FirstOrDefault(i => i.ID_prodotto == id);
        if (itemDaRimuovere != null)
        {
            c.ListaCarrello.Remove(itemDaRimuovere);
        }
        _gestioneSessione.SalvaCarrello(c);
        _toastNotification.AddSuccessToastMessage("Elemento rimosso dal carrello");
        return RedirectToAction("Cart", c.ListaCarrello);
    }
    
    [HttpPost]
    public IActionResult Checkout()
    {
        Carrello carrello = _gestioneSessione.PrendiCarrello();

        if (carrello == null || carrello.ListaCarrello == null || !carrello.ListaCarrello.Any())
        {
            TempData["MessaggioErrore"] = "Il carrello è vuoto.";
            return RedirectToAction("Cart");
        }

        // Passa la lista ListaCarrello alla vista come Model
        return View(carrello.ListaCarrello);
    }


    [HttpPost]
    public IActionResult ConfermaOrdine(string Nome, string Email, string Indirizzo)
    {
        Carrello c = _gestioneSessione.PrendiCarrello();
        gestione.CreaOrdine(Nome,Email,Indirizzo,c.ListaCarrello);
        gestione.AggiornaTotOrdiniPerProdotti(c.ListaCarrello);
        gestione.AggiornaAssociazioniProdottiConPercentuale(c.ListaCarrello);
        c.PulisciCarrello();
        ViewBag.Nome = Nome;
        _gestioneSessione.SalvaCarrello(c);
        _toastNotification.AddSuccessToastMessage("Ordine confermato");
        return View("OrdineCompletato");
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
    
    //aggiungere vari elementi
    public IActionResult AggiungiMoto(string marca, string modello, int cilindrata, string descrizione, int prezzo, string foto)
    {
        gestione.AggiungiMoto(marca, modello, cilindrata, descrizione, prezzo, foto);
        _toastNotification.AddSuccessToastMessage("Articolo inserito con successo");
        return View("Home");
    }

    public IActionResult AggiungiAbbigliamento(string tipoVestiario, string colore, string materiale, string descrizione, int prezzo, string foto)
    {
        gestione.AggiungiAbbigliamento(tipoVestiario, colore, materiale, descrizione, prezzo, foto);
        _toastNotification.AddSuccessToastMessage("Articolo inserito con successo");
        return View("Home");
    }

    public IActionResult AggiungiAccessori(string tipo, string compatibilita, string descrizione, int prezzo, string foto)
    {
        gestione.AggiungiAccessorio(tipo, compatibilita, descrizione, prezzo, foto);
        _toastNotification.AddSuccessToastMessage("Articolo inserito con successo");
        return View("Home");
    }
    
    
    //Errore-----------------------------------------------------------------------------
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}