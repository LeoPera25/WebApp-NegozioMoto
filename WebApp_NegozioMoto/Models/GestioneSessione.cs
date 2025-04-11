using System.Text.Json;
using WebApp_NegozioMoto.Models;

namespace WebApp_NegozioMoto.Views.Home;
using MySql.Data.MySqlClient;

public class GestioneSessione
{
    private ISession sessione;

    public GestioneSessione(ISession sessione)
    {
        this.sessione = sessione;
        
    }
    public Carrello PrendiCarrello()
    {
        Carrello c;
        // prende il carrello dalla sessione 
        // la sessione memorizza il carello come stringa json 
        if (sessione.Keys.Contains("carrello"))
        {
            string appo = sessione.GetString("carrello");
            c = JsonSerializer.Deserialize<Carrello>(appo);
            
        }
        else
        {
            c = new Carrello();
        }
        return c;
    }

    public void SalvaCarrello(Carrello c)
    {
        string appo = JsonSerializer.Serialize(c);
        sessione.SetString("carrello", appo);
    }
}