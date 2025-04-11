namespace WebApp_NegozioMoto.Views.Home;

public class Categoria
{
    public int ID_categoria { get; set; }
    public string nome_categoria { get; set; }
    
    public List<Item> Items { get; set; }
}