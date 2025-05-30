using WebApp_NegozioMoto.Models;

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
                p.foto = (string)reader["foto"];

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
                p.ID_categoria = (int)reader["id_categoria"];
                p.prezzo = (int)reader["prezzo"];
                p.foto = (string)reader["foto"];

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
                p.ID_categoria = (int)reader["id_categoria"];
                p.prezzo = (int)reader["prezzo"];
                p.foto = (string)reader["foto"];

                prodotti.Add(p);
            }

            reader.Close();
        }

        return prodotti;
    }

    public Item RecuperaItem(int idprodotto, int idCategoria)
    {
        Item i = new Item();

        if (idCategoria == 1)
        {
            string query = "SELECT * From moto " +
                           "inner join categoria on categoria.id_categoria = moto.id_categoria " +
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
                i.prezzo = (int)reader["prezzo"];
                i.foto = (string)reader["foto"];
            }

            reader.Close();
        }
        else if (idCategoria == 2)
        {
            string query = "SELECT * From abbigliamento " +
                           "inner join categoria on categoria.id_categoria = abbigliamento.id_categoria " +
                           "where abbigliamento.id_prodotto = @idprod";

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
                i.prezzo = (int)reader["prezzo"];
                i.foto = (string)reader["foto"];
            }

            reader.Close();
        }
        else if (idCategoria == 3)
        {
            string query = "SELECT * From accessori " +
                           "inner join categoria on categoria.id_categoria = accessori.id_categoria " +
                           "where accessori.id_prodotto = @idprod";

            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idprod", idprodotto);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                i.ID_prodotto = (int)reader["id_prodotto"];
                i.ID_categoria = (int)reader["id_categoria"];
                i.tipo = (string)reader["tipo"];
                i.compatibilita = (string)reader["compatibilita"];
                i.prezzo = (int)reader["prezzo"];
                i.foto = (string)reader["foto"];
            }

            reader.Close();
        }

        return i;
    }

    public bool InserisciUtente(string username, string password, string indirizzo)
    {
        try
        {
            string query =
                "INSERT INTO utente (username, password, indirizzo) VALUES (@username, @password, @indirizzo)";

            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@indirizzo", indirizzo);

            int righeInserite = cmd.ExecuteNonQuery();

            return true; // true se almeno 1 riga inserita
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errore: " + ex.Message);
            // Puoi loggare l'errore qui se vuoi
            return false;
        }
    }

    public bool LoginUtente(string username, string password)
    {
        try
        {
            string query = "SELECT * FROM utente WHERE username = @username AND password = @password";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            object result = cmd.ExecuteScalar();

            int count = Convert.ToInt32(result);

            return count > 0; // true se esiste almeno un utente
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errore login: " + ex.Message);
            return false;
        }
    }

    public void CreaOrdine(string utente, string indirizzo, string mail, List<Item> prodotti)
    {
        {
            try
            {
                // 1. Inserisci l'ordine
                string insertOrdine =
                    "INSERT INTO ordine (utente, indirizzo, mail) VALUES (@utente, @indirizzo, @mail)";
                var cmdOrdine = new MySqlCommand(insertOrdine, con);
                cmdOrdine.Parameters.AddWithValue("@utente", utente);
                cmdOrdine.Parameters.AddWithValue("@indirizzo", indirizzo);
                cmdOrdine.Parameters.AddWithValue("@mail", mail);
                cmdOrdine.ExecuteNonQuery();

                // 2. Recupera l'id_ordine generato
                long idOrdine = cmdOrdine.LastInsertedId;

                // Raggruppa articoli per ID_prodotto
                var articoliRaggruppati = prodotti
                    .GroupBy(i => new { i.ID_prodotto, i.ID_categoria })
                    .Select(g => new
                    {
                        Prodotto = g.First(),
                        Quantità = g.Count()
                    })
                    .ToList();
                // 3. Inserisci i prodotti dell’ordine
                string insertProdotto = @"INSERT INTO ordine_prodotto (id_ordine, id_prodotto, id_categoria, quantita) 
                                      VALUES (@id_ordine, @id_prodotto, @id_categoria, @quantita)";

                foreach (var p in articoliRaggruppati)
                {
                    var cmdProdotto = new MySqlCommand(insertProdotto, con);
                    cmdProdotto.Parameters.AddWithValue("@id_ordine", idOrdine);
                    cmdProdotto.Parameters.AddWithValue("@id_prodotto", p.Prodotto.ID_prodotto);
                    cmdProdotto.Parameters.AddWithValue("@id_categoria", p.Prodotto.ID_categoria);
                    cmdProdotto.Parameters.AddWithValue("@quantita", p.Quantità);
                    cmdProdotto.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante la creazione dell'ordine: " + ex.Message);
            }
        }
    }

    public void AggiornaAssociazioniProdottiConPercentuale(List<Item> prodotti)
    {
        for (int i = 0; i < prodotti.Count; i++)
        {
            for (int j = i + 1; j < prodotti.Count; j++)
            {
                var a = prodotti[i];
                var b = prodotti[j];

                int cat1, cat2;
                int id1, id2;

                if (a.ID_categoria < b.ID_categoria ||
                    (a.ID_categoria == b.ID_categoria && a.ID_prodotto < b.ID_prodotto))
                {
                    cat1 = a.ID_categoria;
                    id1 = a.ID_prodotto;
                    cat2 = b.ID_categoria;
                    id2 = b.ID_prodotto;
                }
                else
                {
                    cat1 = b.ID_categoria;
                    id1 = b.ID_prodotto;
                    cat2 = a.ID_categoria;
                    id2 = a.ID_prodotto;
                }

                // 1. Inserisci o aggiorna la frequenza della coppia
                string q = @"
                INSERT INTO prodotto_associazione (categoria1, id_prodotto1, categoria2, id_prodotto2, frequenza)
                VALUES (@cat1, @id1, @cat2, @id2, 1)
                ON DUPLICATE KEY UPDATE frequenza = frequenza + 1";

                using (var cmd = new MySqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@cat1", cat1);
                    cmd.Parameters.AddWithValue("@id1", id1);
                    cmd.Parameters.AddWithValue("@cat2", cat2);
                    cmd.Parameters.AddWithValue("@id2", id2);
                    cmd.ExecuteNonQuery();
                }

                // 2. Recupera Tot_ordinati del prodotto di riferimento
                string tabellaTot = cat1 switch
                {
                    1 => "moto",
                    2 => "abbigliamento",
                    3 => "accessori",
                    _ => throw new Exception("Categoria non valida")
                };

                string queryTot = $"SELECT Tot_ordinati FROM {tabellaTot} WHERE id_prodotto = @id1";
                int totOrdinati = 1;

                using (var cmd = new MySqlCommand(queryTot, con))
                {
                    cmd.Parameters.AddWithValue("@id1", id1);
                    var result = cmd.ExecuteScalar();
                    if (result != null) totOrdinati = Convert.ToInt32(result);
                }

                // 3. Aggiorna la percentuale (basata su frequenza e Tot_ordinati)
                string updatePercentuale = @"
                UPDATE prodotto_associazione
                SET percentuale = ROUND((frequenza / @tot) * 100, 2)
                WHERE categoria1 = @cat1 AND id_prodotto1 = @id1 AND categoria2 = @cat2 AND id_prodotto2 = @id2";

                using (var cmd = new MySqlCommand(updatePercentuale, con))
                {
                    cmd.Parameters.AddWithValue("@cat1", cat1);
                    cmd.Parameters.AddWithValue("@id1", id1);
                    cmd.Parameters.AddWithValue("@cat2", cat2);
                    cmd.Parameters.AddWithValue("@id2", id2);
                    cmd.Parameters.AddWithValue("@tot", totOrdinati);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public void AggiornaTotOrdiniPerProdotti(List<Item> prodotti)
    {
        foreach (var prodotto in prodotti)
        {
            string nomeTabella = prodotto.ID_categoria switch
            {
                1 => "moto",
                2 => "abbigliamento",
                3 => "accessori",
                _ => throw new ArgumentException($"Categoria non valida: {prodotto.ID_categoria}")
            };

            string query = $@"
                UPDATE {nomeTabella}
                SET Tot_ordinati = Tot_ordinati + 1
                WHERE id_prodotto = @id_prodotto;
            ";

            using (var command = new MySqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@id_prodotto", prodotto.ID_prodotto);
                command.ExecuteNonQuery();
            }
        }
    }
    
    
}
