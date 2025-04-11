namespace WebApp_NegozioMoto.Views.Home;
using MySql.Data.MySqlClient;

public class GestioneDati
{
    private MySqlConnection con;

    public GestioneDati()
    {
        string s = "database=negoziomoto;host=localhost;port=3306;user=root;pwd=root";
        con = new MySqlConnection(s);
        con.Open();
    }
    
    public List<Item> RecuperaTuttiIProdottiDiUnaCategoria(int idCategoria)
    {
        List<Item> prodotti = new List<Item>();
        //if per elenco moto
        if (idCategoria == 1)
        {
            string query = "SELECT * FROM moto " +
                           "inner join categoria on categoria.id_categoria= moto.id_categoria " +
                           "WHERE categoria.id_categoria = @id";

            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", idCategoria);
            MySqlDataReader reader = cmd.ExecuteReader();
            

            while (reader.Read())
            {
                Item p = new Item();
                p.ID_prodotto = (int)reader["id_prodotto"];
                p.descrizione = (string)reader["descrizione"];
                p.marca = (string)reader["marca"];
                p.modello = (string)reader["modello"];
                p.ID_categoria = (int)reader["id_categoria"];
                p.prezzo = (int)reader["prezzo"];
            
                prodotti.Add(p);
            }
            reader.Close();
        }
        //if per elenco abbigliamento
        else if (idCategoria == 2)
        {
            string query = "SELECT * FROM abbigliamento " +
                           "inner join categoria on categoria.id_categoria= abbigliamento.id_categoria " +
                           "WHERE categoria.id_categoria = @id";

            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", idCategoria);
            MySqlDataReader reader = cmd.ExecuteReader();
            

            while (reader.Read())
            {
                Item p = new Item();
                p.ID_prodotto = (int)reader["id_prodotto"];
                p.descrizione = (string)reader["descrizione"];
                p.tipo_vestiario = (string)reader["tipo_vestiario"];
                p.colore = (string)reader["colore"];
                p.materiale = (string)reader["materiale"];
                p.prezzo = (int)reader["prezzo"];
                
                prodotti.Add(p);
            }
            reader.Close();
        }
        //if per elenco accessori
        else if (idCategoria == 3)
        {
            string query = "SELECT * FROM accessori " +
                           "inner join categoria on categoria.id_categoria= accessori.id_categoria " +
                           "WHERE categoria.id_categoria = @id";

            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", idCategoria);
            MySqlDataReader reader = cmd.ExecuteReader();
            

            while (reader.Read())
            {
                Item p = new Item();
                p.ID_prodotto = (int)reader["id_prodotto"];
                p.tipo = (string)reader["tipo"];
                p.compatibilita = (string)reader["compatibilita"];
                p.prezzo = (int)reader["prezzo"];
                
                prodotti.Add(p);
            }
            reader.Close();
        }
        
        return prodotti;
    }

    public Item RecuperaItem(int idprodotto,int idCategoria)
    {
        Item i = new Item();
        
        if (idCategoria == 1)
        {
            string query = "SELECT * From moto " +
                           "inner join categoria on categoria.id_categoria = moto.id_categoria" +
                           "where moto.id_prodotto = @idprod";
            
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idprod", idprodotto);
            MySqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                i.ID_prodotto = (int)reader["id_prodotto"];
                i.ID_categoria = (int)reader["id_categoria"];
                i.descrizione = (string)reader["descrizione"];
                i.marca = (string)reader["marca"];
                i.modello = (string)reader["modello"];
            }
            reader.Close();
        }
        else if (idCategoria == 2)
        {
            string query = "SELECT * From abbigliamento " +
                           "inner join categoria on categoria.id_categoria = abbigliamento.id_categoria" +
                           "where moto.id_prodotto = @idprod";
            
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idprod", idprodotto);
            MySqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                i.ID_prodotto = (int)reader["id_prodotto"];
                i.ID_categoria = (int)reader["id_categoria"];
                i.descrizione = (string)reader["descrizione"];
                i.tipo_vestiario = (string)reader["tipo_vestiario"];
                i.colore = (string)reader["colore"];
            }
            reader.Close();
        }
        else if (idCategoria == 2)
        {
            string query = "SELECT * From accessori " +
                           "inner join categoria on categoria.id_categoria = accessori.id_categoria" +
                           "where moto.id_prodotto = @idprod";
            
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idprod", idprodotto);
            MySqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                i.ID_prodotto = (int)reader["id_prodotto"];
                i.ID_categoria = (int)reader["id_categoria"];
                i.tipo = (string)reader["tipo"];
                i.compatibilita = (string)reader["compatibilita"];
            }
            reader.Close();
        }

        return i;
    }

}