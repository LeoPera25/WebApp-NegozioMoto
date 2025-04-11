using WebApp_NegozioMoto.Views.Home;

namespace WebApp_NegozioMoto.Models;

//Mostra il contenuto del carrello
public class Carrello
{
    public List<Item> ListaCarrello { get; set; }

    public Carrello()
    {
        ListaCarrello = new List<Item>();
    }

    public void AggiungiItem(Item i)
    {
        ListaCarrello.Add(i);
    }

    public void RimuoviItem(Item i)
    {
        ListaCarrello.Remove(i);
    }

    public void PulisciCarrello()
    {
        ListaCarrello.Clear();
    }
}