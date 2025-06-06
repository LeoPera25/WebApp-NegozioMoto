
public class Item
{
    //Attributi generici
    public int ID_prodotto { get; set; }
    public int ID_categoria { get; set; }
    public string descrizione { get; set; }
    public int prezzo { get; set; }
    public string foto { get; set; }
    public int totOrdinati { get; set; }
    
    //Attributi Moto
    
    public string modello { get; set; }
    public string marca { get; set; }
    public int cilindrata { get; set; }
    
    //Attributi Abbigliamento
    public string tipo_vestiario { get; set; }
    public string colore { get; set; }
    public string materiale { get; set; }
    
    //Attributi Accessori
    public string tipo { get; set; }
    public string compatibilita { get; set; }
}