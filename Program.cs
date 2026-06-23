using System;
using System.Collections.Generic;
using System.Linq;

/*
 * TEMPLATE ESAME C# - NEGOZIO ONLINE
 *
 * Regola scelta per il template:
 * - i metodi di visualizzazione sono già implementati, così lo studente può concentrarsi
 *   sulle operazioni richieste dalla traccia.
 * - i metodi operazionali contengono TODO guidati: lo studente deve completarli senza
 *   modificare firma, nome, parametri o tipo di ritorno.
 *
 * Vincolo richiesto: tutto il codice è in un unico file .cs e senza namespace.
 */

public class Program
{
    public static void Main()
    {
        // Punto di ingresso della Console App.
        ApplicazioneNegozio applicazione = new ApplicazioneNegozio();
        applicazione.Avvia();
        // TestNegozioOnline.EseguiTuttiITest();
    }
}

public class ApplicazioneNegozio
{
    private readonly CatalogoProdotti catalogoProdotti;
    private readonly CarrelloUtente carrelloUtente;
    private readonly StoricoAcquisti storicoAcquisti;
    private readonly ServizioNegozio servizioNegozio;

    public ApplicazioneNegozio()
    {
        catalogoProdotti = new CatalogoProdotti();
        carrelloUtente = new CarrelloUtente();
        storicoAcquisti = new StoricoAcquisti();
        servizioNegozio = new ServizioNegozio(catalogoProdotti, carrelloUtente, storicoAcquisti);

        CaricaDatiIniziali();
    }

    public void Avvia()
    {
        // 1. Mostrare un messaggio di benvenuto
        Console.WriteLine("==================================================");
        Console.WriteLine("       BENVENUTO NEL NOSTRO NEGOZIO ONLINE        ");
        Console.WriteLine("==================================================");

        bool inEsecuzione = true;

        while (inEsecuzione)
        {
            // 2. Chiedere il ruolo o l'uscita tramite il metodo ScegliRuolo()
            string ruolo = ScegliRuolo();

            // 3. & 4. Chiamare i menu dedicati o permettere l'uscita
            switch (ruolo)
            {
                case "utente":
                    GestisciMenuUtente();
                    break;

                case "amministratore":
                    GestisciMenuAmministratore();
                    break;

                case "esci":
                    inEsecuzione = false;
                    Console.WriteLine("\nGrazie per aver usato il sistema. Arrivederci!");
                    break;
            }
        }
    }

    private void CaricaDatiIniziali()
    {
        // Metodo già implementato: fornisce prodotti di partenza per testare subito il sistema.
        catalogoProdotti.AggiungiProdotto(new Prodotto("P001", "Tastiera meccanica", 79.90m, 10));
        catalogoProdotti.AggiungiProdotto(new Prodotto("P002", "Mouse wireless", 24.50m, 25));
        catalogoProdotti.AggiungiProdotto(new Prodotto("P003", "Monitor 24 pollici", 149.99m, 7));
        catalogoProdotti.AggiungiProdotto(new Prodotto("P004", "Cavo USB-C", 9.99m, 40));
    }

    private string ScegliRuolo()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== ACCESSO ===");
            Console.WriteLine("1. Entra come utente");
            Console.WriteLine("2. Entra come amministratore");
            Console.WriteLine("3. Esci");
            Console.Write("Scelta: ");

            string? input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1": return "utente";
                case "2": return "amministratore";
                case "3": return "esci";
                default:
                    Console.WriteLine("Scelta non valida. Digita 1, 2 o 3.");
                    break;
            }
        }
    }

    private void GestisciMenuUtente()
    {
        bool continua = true;

        while (continua)
        {
            Console.WriteLine();
            Console.WriteLine("=== MENU UTENTE ===");
            Console.WriteLine("1. Visualizza catalogo");
            Console.WriteLine("2. Aggiungi prodotto al carrello");
            Console.WriteLine("3. Visualizza carrello");
            Console.WriteLine("4. Modifica quantità nel carrello");
            Console.WriteLine("5. Rimuovi prodotto dal carrello");
            Console.WriteLine("6. Svuota carrello");
            Console.WriteLine("7. Conferma acquisto");
            Console.WriteLine("8. Visualizza storico acquisti");
            Console.WriteLine("9. Torna indietro");
            Console.Write("Scelta: ");

            string? scelta = Console.ReadLine()?.Trim();

            switch (scelta)
            {
                case "1":
                    MostraCatalogo();
                    break;

                case "2":
                    Console.Write("Codice prodotto: ");
                    string? codiceAggiunta = Console.ReadLine()?.Trim();
                    int quantitaAggiunta = LeggiInteroPositivo("Quantità: ");
                    bool aggiunto = servizioNegozio.AggiungiProdottoAlCarrello(codiceAggiunta ?? "", quantitaAggiunta);
                    Console.WriteLine(aggiunto ? "Prodotto aggiunto al carrello." : "Impossibile aggiungere il prodotto.");
                    break;

                case "3":
                    MostraCarrello();
                    break;

                case "4":
                    Console.Write("Codice prodotto da modificare: ");
                    string? codiceModifica = Console.ReadLine()?.Trim();
                    int nuovaQuantita = LeggiInteroPositivo("Nuova quantità: ");
                    bool modificato = carrelloUtente.ModificaQuantitaNelCarrello(codiceModifica ?? "", nuovaQuantita);
                    Console.WriteLine(modificato ? "Quantità aggiornata." : "Impossibile modificare la quantità.");
                    break;

                case "5":
                    Console.Write("Codice prodotto da rimuovere: ");
                    string? codiceRimozione = Console.ReadLine()?.Trim();
                    bool rimosso = carrelloUtente.RimuoviDalCarrello(codiceRimozione ?? "");
                    Console.WriteLine(rimosso ? "Prodotto rimosso dal carrello." : "Prodotto non trovato nel carrello.");
                    break;

                case "6":
                    carrelloUtente.SvuotaCarrello();
                    Console.WriteLine("Carrello svuotato.");
                    break;

                case "7":
                    Console.Write("Inserisci il tuo nome per confermare l'acquisto: ");
                    string? nomeUtente = Console.ReadLine()?.Trim();
                    try
                    {
                        Utente utente = new Utente(nomeUtente ?? "");
                        Acquisto acquisto = servizioNegozio.ConfermaAcquisto(utente);
                        Console.WriteLine("Acquisto confermato!");
                        servizioNegozio.StampaAcquisto(acquisto);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Errore: " + ex.Message);
                    }
                    break;

                case "8":
                    MostraStoricoUtente();
                    break;

                case "9":
                    continua = false;
                    break;

                default:
                    Console.WriteLine("Scelta non valida. Digita un numero da 1 a 9.");
                    break;
            }
        }
    }

    private void GestisciMenuAmministratore()
    {
        bool continua = true;

        while (continua)
        {
            Console.WriteLine();
            Console.WriteLine("=== MENU AMMINISTRATORE ===");
            Console.WriteLine("1. Visualizza catalogo");
            Console.WriteLine("2. Aggiungi prodotto");
            Console.WriteLine("3. Elimina prodotto");
            Console.WriteLine("4. Modifica prezzo prodotto");
            Console.WriteLine("5. Modifica quantità disponibile");
            Console.WriteLine("6. Visualizza tutti gli acquisti");
            Console.WriteLine("7. Report quantità prodotti");
            Console.WriteLine("8. Torna indietro");
            Console.Write("Scelta: ");

            string? scelta = Console.ReadLine()?.Trim();

            switch (scelta)
            {
                case "1":
                    MostraCatalogo();
                    break;

                case "2":
                    Console.Write("Codice prodotto: ");
                    string? codice = Console.ReadLine()?.Trim() ?? "";
                    Console.Write("Nome prodotto: ");
                    string? nome = Console.ReadLine()?.Trim() ?? "";
                    decimal prezzo = LeggiPrezzoPositivo("Prezzo (euro): ");
                    int quantitaIniziale = LeggiInteroPositivo("Quantità disponibile: ");
                    try
                    {
                        catalogoProdotti.AggiungiProdotto(new Prodotto(codice, nome, prezzo, quantitaIniziale));
                        Console.WriteLine("Prodotto aggiunto.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Errore: " + ex.Message);
                    }
                    break;

                case "3":
                    Console.Write("Codice prodotto da eliminare: ");
                    string? codiceElimina = Console.ReadLine()?.Trim() ?? "";
                    bool eliminato = catalogoProdotti.EliminaProdotto(codiceElimina);
                    Console.WriteLine(eliminato ? "Prodotto eliminato." : "Prodotto non trovato.");
                    break;

                case "4":
                    Console.Write("Codice prodotto: ");
                    string? codicePrezzo = Console.ReadLine()?.Trim() ?? "";
                    decimal nuovoPrezzo = LeggiPrezzoPositivo("Nuovo prezzo (euro): ");
                    bool prezzoModificato = catalogoProdotti.ModificaPrezzoProdotto(codicePrezzo, nuovoPrezzo);
                    Console.WriteLine(prezzoModificato ? "Prezzo aggiornato." : "Prodotto non trovato.");
                    break;

                case "5":
                    Console.Write("Codice prodotto: ");
                    string? codiceQuantita = Console.ReadLine()?.Trim() ?? "";
                    Console.Write("Variazione (positiva per aumentare, negativa per diminuire): ");
                    string? inputVariazione = Console.ReadLine()?.Trim();
                    if (int.TryParse(inputVariazione, out int variazione))
                    {
                        try
                        {
                            bool quantitaModificata = catalogoProdotti.ModificaQuantitaProdotto(codiceQuantita, variazione);
                            Console.WriteLine(quantitaModificata ? "Quantità aggiornata." : "Prodotto non trovato.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Errore: " + ex.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Valore non valido.");
                    }
                    break;

                case "6":
                    List<Acquisto> tuttiAcquisti = storicoAcquisti.OttieniTuttiGliAcquisti();
                    Console.WriteLine();
                    Console.WriteLine("=== TUTTI GLI ACQUISTI ===");
                    if (tuttiAcquisti.Count == 0)
                    {
                        Console.WriteLine("Nessun acquisto registrato.");
                    }
                    else
                    {
                        foreach (Acquisto acquisto in tuttiAcquisti)
                        {
                            servizioNegozio.StampaAcquisto(acquisto);
                        }
                    }
                    break;

                case "7":
                    servizioNegozio.StampaReportProdotti();
                    break;

                case "8":
                    continua = false;
                    break;

                default:
                    Console.WriteLine("Scelta non valida. Digita un numero da 1 a 8.");
                    break;
            }
        }
    }

    private void MostraCatalogo()
    {
        // Metodo già implementato: mostra a video tutti i prodotti del catalogo.
        List<Prodotto> prodotti = catalogoProdotti.OttieniTuttiIProdotti();

        Console.WriteLine();
        Console.WriteLine("=== CATALOGO PRODOTTI ===");

        if (prodotti.Count == 0)
        {
            Console.WriteLine("Il catalogo è vuoto.");
            return;
        }

        foreach (Prodotto prodotto in prodotti)
        {
            Console.WriteLine(
                prodotto.CodiceProdotto + " - " +
                prodotto.Nome + " - " +
                prodotto.Prezzo.ToString("0.00") + " euro - " +
                "Disponibili: " + prodotto.QuantitaDisponibile);
        }
    }

    private void MostraCarrello()
    {
        // Metodo già implementato: mostra contenuto del carrello e totale corrente.
        List<ElementoCarrello> elementi = carrelloUtente.OttieniElementi();

        Console.WriteLine();
        Console.WriteLine("=== CARRELLO ===");

        if (elementi.Count == 0)
        {
            Console.WriteLine("Il carrello è vuoto.");
            return;
        }

        foreach (ElementoCarrello elemento in elementi)
        {
            Console.WriteLine(
                elemento.ProdottoSelezionato.CodiceProdotto + " - " +
                elemento.ProdottoSelezionato.Nome + " - " +
                "Quantità: " + elemento.QuantitaScelta + " - " +
                "Prezzo unitario: " + elemento.PrezzoUnitario.ToString("0.00") + " euro - " +
                "Parziale: " + elemento.CalcolaTotaleParziale().ToString("0.00") + " euro");
        }

        Console.WriteLine("Totale carrello: " + carrelloUtente.CalcolaTotale().ToString("0.00") + " euro");
    }

    private void MostraStoricoUtente()
    {
        // Metodo già implementato: chiede un nome e mostra gli acquisti collegati.
        Console.Write("Inserisci nome utente: ");
        string? nomeUtente = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nomeUtente))
        {
            Console.WriteLine("Nome utente non valido.");
            return;
        }

        List<Acquisto> acquistiUtente = storicoAcquisti.OttieniAcquistiPerUtente(nomeUtente);

        Console.WriteLine();
        Console.WriteLine("=== STORICO ACQUISTI DI " + nomeUtente.Trim() + " ===");

        if (acquistiUtente.Count == 0)
        {
            Console.WriteLine("Nessun acquisto trovato per questo utente.");
            return;
        }

        foreach (Acquisto acquisto in acquistiUtente)
        {
            servizioNegozio.StampaAcquisto(acquisto);
        }
    }

    private int LeggiInteroPositivo(string messaggio)
    {
        while (true)
        {
            Console.Write(messaggio);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int valore) && valore > 0)
            {
                return valore;
            }

            Console.WriteLine("Valore non valido. Inserisci un numero intero maggiore di zero.");
        }
    }

    private decimal LeggiPrezzoPositivo(string messaggio)
    {
        while (true)
        {
            Console.Write(messaggio);
            string? input = Console.ReadLine();

            if (decimal.TryParse(input, out decimal valore) && valore > 0)
            {
                return valore;
            }

            Console.WriteLine("Valore non valido. Inserisci un prezzo maggiore di zero.");
        }
    }
}

public interface IGestioneCatalogo
{
    void AggiungiProdotto(Prodotto prodotto);
    bool EliminaProdotto(string codiceProdotto);
    Prodotto? CercaProdottoPerCodice(string codiceProdotto);
    List<Prodotto> OttieniTuttiIProdotti();
    bool ModificaPrezzoProdotto(string codiceProdotto, decimal nuovoPrezzo);
    bool ModificaQuantitaProdotto(string codiceProdotto, int variazioneQuantita);
}

public interface IGestioneCarrello
{
    bool AggiungiAlCarrello(Prodotto prodotto, int quantita);
    bool ModificaQuantitaNelCarrello(string codiceProdotto, int nuovaQuantita);
    bool RimuoviDalCarrello(string codiceProdotto);
    void SvuotaCarrello();
    decimal CalcolaTotale();
    List<ElementoCarrello> OttieniElementi();
}

public interface IGestioneAcquisti
{
    void RegistraAcquisto(Acquisto acquisto);
    List<Acquisto> OttieniTuttiGliAcquisti();
    List<Acquisto> OttieniAcquistiPerUtente(string nomeUtente);
}

public class Utente
{
    public string Nome { get; private set; }

    public Utente(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("Il nome utente non può essere vuoto.");
        }

        Nome = nome.Trim();
    }
}

public class Prodotto
{
    public string CodiceProdotto { get; private set; }
    public string Nome { get; private set; }
    public decimal Prezzo { get; private set; }
    public int QuantitaDisponibile { get; private set; }
    public int QuantitaIniziale { get; private set; }

    public Prodotto(string codiceProdotto, string nome, decimal prezzo, int quantitaDisponibile)
    {
        CodiceProdotto = codiceProdotto;
        Nome = nome;
        Prezzo = prezzo;
        QuantitaDisponibile = quantitaDisponibile;
        QuantitaIniziale = quantitaDisponibile;
    }

    public void CambiaPrezzo(decimal nuovoPrezzo)
    {
        // Metodo già implementato: centralizza la validazione del prezzo.
        if (nuovoPrezzo <= 0)
        {
            throw new ArgumentException("Il prezzo deve essere maggiore di zero.");
        }

        Prezzo = nuovoPrezzo;
    }

    public void CambiaQuantita(int variazioneQuantita)
    {
        // Metodo già implementato: impedisce di portare il magazzino sotto zero.
        int nuovaQuantita = QuantitaDisponibile + variazioneQuantita;

        if (nuovaQuantita < 0)
        {
            throw new InvalidOperationException("La quantità disponibile non può diventare negativa.");
        }

        QuantitaDisponibile = nuovaQuantita;
    }

    public int CalcolaQuantitaVenduta()
    {
        // Metodo già implementato: serve per il report amministratore.
        return QuantitaIniziale - QuantitaDisponibile;
    }
}

public class ElementoCarrello
{
    public Prodotto ProdottoSelezionato { get; private set; }
    public int QuantitaScelta { get; private set; }
    public decimal PrezzoUnitario { get; private set; }

    public ElementoCarrello(Prodotto prodottoSelezionato, int quantitaScelta)
    {
        ProdottoSelezionato = prodottoSelezionato;
        QuantitaScelta = quantitaScelta;
        PrezzoUnitario = prodottoSelezionato.Prezzo;
    }

    public decimal CalcolaTotaleParziale()
    {
        // Metodo già implementato: evita di duplicare il calcolo del parziale.
        return PrezzoUnitario * QuantitaScelta;
    }

    public void CambiaQuantitaScelta(int nuovaQuantita)
    {
        if (nuovaQuantita <= 0)
        {
            throw new ArgumentException("La quantità scelta deve essere maggiore di zero.");
        }

        QuantitaScelta = nuovaQuantita;
    }
}

public class Acquisto
{
    public Utente Utente { get; private set; }
    public string NomeUtente
    {
        get { return Utente.Nome; }
    }

    public List<ElementoAcquistato> ProdottiAcquistati { get; private set; }
    public decimal TotaleOrdine { get; private set; }
    public DateTime DataAcquisto { get; private set; }

    public Acquisto(Utente utente, List<ElementoAcquistato> prodottiAcquistati)
    {
        Utente = utente;
        ProdottiAcquistati = prodottiAcquistati;
        DataAcquisto = DateTime.Now;
        TotaleOrdine = CalcolaTotaleOrdine();
    }

    private decimal CalcolaTotaleOrdine()
    {
        // Metodo già implementato: somma tutti i parziali dei prodotti acquistati.
        return ProdottiAcquistati.Sum(prodotto => prodotto.TotaleParziale);
    }
}

public class ElementoAcquistato
{
    public string CodiceProdotto { get; private set; }
    public string NomeProdotto { get; private set; }
    public int QuantitaAcquistata { get; private set; }
    public decimal PrezzoUnitario { get; private set; }
    public decimal TotaleParziale { get; private set; }

    public ElementoAcquistato(string codiceProdotto, string nomeProdotto, int quantitaAcquistata, decimal prezzoUnitario)
    {
        CodiceProdotto = codiceProdotto;
        NomeProdotto = nomeProdotto;
        QuantitaAcquistata = quantitaAcquistata;
        PrezzoUnitario = prezzoUnitario;
        TotaleParziale = prezzoUnitario * quantitaAcquistata;
    }
}

public class CatalogoProdotti : IGestioneCatalogo
{
    private readonly List<Prodotto> prodotti;

    public CatalogoProdotti()
    {
        prodotti = new List<Prodotto>();
    }

    public void AggiungiProdotto(Prodotto prodotto)
    {
        // Metodo già implementato: evita codici duplicati nel catalogo.
        bool codiceGiaPresente = prodotti.Any(p => p.CodiceProdotto == prodotto.CodiceProdotto);

        if (codiceGiaPresente)
        {
            throw new InvalidOperationException("Esiste già un prodotto con lo stesso codice.");
        }

        prodotti.Add(prodotto);
    }

    public bool EliminaProdotto(string codiceProdotto)
    {
        Prodotto? prodotto = CercaProdottoPerCodice(codiceProdotto);

        if (prodotto == null)
        {
            return false;
        }

        prodotti.Remove(prodotto);
        return true;
    }

    public Prodotto? CercaProdottoPerCodice(string codiceProdotto)
    {
        // Metodo già implementato: ricerca case-insensitive per rendere più comodo l'input da console.
        return prodotti.FirstOrDefault(prodotto =>
            prodotto.CodiceProdotto.Equals(codiceProdotto, StringComparison.OrdinalIgnoreCase));
    }

    public List<Prodotto> OttieniTuttiIProdotti()
    {
        // Metodo già implementato: restituisce una copia per proteggere la lista interna.
        return new List<Prodotto>(prodotti);
    }

    public bool ModificaPrezzoProdotto(string codiceProdotto, decimal nuovoPrezzo)
    {
        Prodotto? prodotto = CercaProdottoPerCodice(codiceProdotto);

        if (prodotto == null)
        {
            return false;
        }

        prodotto.CambiaPrezzo(nuovoPrezzo);
        return true;
    }

    public bool ModificaQuantitaProdotto(string codiceProdotto, int variazioneQuantita)
    {
        Prodotto? prodotto = CercaProdottoPerCodice(codiceProdotto);

        if (prodotto == null)
        {
            return false;
        }

        prodotto.CambiaQuantita(variazioneQuantita);
        return true;
    }
}

public class CarrelloUtente : IGestioneCarrello
{
    private readonly List<ElementoCarrello> elementiCarrello;

    public CarrelloUtente()
    {
        elementiCarrello = new List<ElementoCarrello>();
    }

    public bool AggiungiAlCarrello(Prodotto prodotto, int quantita)
    {
        if (quantita <= 0)
        {
            return false;
        }

        ElementoCarrello? elementoEsistente = elementiCarrello
            .FirstOrDefault(e => e.ProdottoSelezionato.CodiceProdotto == prodotto.CodiceProdotto);

        if (elementoEsistente != null)
        {
            int quantitaTotale = elementoEsistente.QuantitaScelta + quantita;

            if (quantitaTotale > prodotto.QuantitaDisponibile)
            {
                return false;
            }

            elementoEsistente.CambiaQuantitaScelta(quantitaTotale);
            return true;
        }

        if (quantita > prodotto.QuantitaDisponibile)
        {
            return false;
        }

        elementiCarrello.Add(new ElementoCarrello(prodotto, quantita));
        return true;
    }

    public bool ModificaQuantitaNelCarrello(string codiceProdotto, int nuovaQuantita)
    {
        ElementoCarrello? elemento = elementiCarrello
            .FirstOrDefault(e => e.ProdottoSelezionato.CodiceProdotto.Equals(codiceProdotto, StringComparison.OrdinalIgnoreCase));

        if (elemento == null)
        {
            return false;
        }

        if (nuovaQuantita <= 0 || nuovaQuantita > elemento.ProdottoSelezionato.QuantitaDisponibile)
        {
            return false;
        }

        elemento.CambiaQuantitaScelta(nuovaQuantita);
        return true;
    }

    public bool RimuoviDalCarrello(string codiceProdotto)
    {
        ElementoCarrello? elemento = elementiCarrello
            .FirstOrDefault(e => e.ProdottoSelezionato.CodiceProdotto.Equals(codiceProdotto, StringComparison.OrdinalIgnoreCase));

        if (elemento == null)
        {
            return false;
        }

        elementiCarrello.Remove(elemento);
        return true;
    }

    public void SvuotaCarrello()
    {
        // Metodo già implementato: cancella tutti gli elementi del carrello.
        elementiCarrello.Clear();
    }

    public decimal CalcolaTotale()
    {
        // Metodo già implementato: ricalcola sempre il totale dai parziali correnti.
        return elementiCarrello.Sum(elemento => elemento.CalcolaTotaleParziale());
    }

    public List<ElementoCarrello> OttieniElementi()
    {
        // Metodo già implementato: restituisce una copia per evitare modifiche esterne dirette.
        return new List<ElementoCarrello>(elementiCarrello);
    }
}

public class StoricoAcquisti : IGestioneAcquisti
{
    private readonly List<Acquisto> acquisti;

    public StoricoAcquisti()
    {
        acquisti = new List<Acquisto>();
    }

    public void RegistraAcquisto(Acquisto acquisto)
    {
        // Metodo già implementato: conserva l'acquisto in memoria durante l'esecuzione.
        acquisti.Add(acquisto);
    }

    public List<Acquisto> OttieniTuttiGliAcquisti()
    {
        // Metodo già implementato: restituisce una copia dello storico.
        return new List<Acquisto>(acquisti);
    }

    public List<Acquisto> OttieniAcquistiPerUtente(string nomeUtente)
    {
        return acquisti
            .Where(a => a.NomeUtente.Equals(nomeUtente, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}

public class ServizioNegozio
{
    private readonly CatalogoProdotti catalogoProdotti;
    private readonly CarrelloUtente carrelloUtente;
    private readonly StoricoAcquisti storicoAcquisti;

    public ServizioNegozio(CatalogoProdotti catalogoProdotti, CarrelloUtente carrelloUtente, StoricoAcquisti storicoAcquisti)
    {
        this.catalogoProdotti = catalogoProdotti;
        this.carrelloUtente = carrelloUtente;
        this.storicoAcquisti = storicoAcquisti;
    }

    public bool AggiungiProdottoAlCarrello(string codiceProdotto, int quantita)
    {
        Prodotto? prodotto = catalogoProdotti.CercaProdottoPerCodice(codiceProdotto);

        if (prodotto == null)
        {
            return false;
        }

        return carrelloUtente.AggiungiAlCarrello(prodotto, quantita);
    }

    public Acquisto ConfermaAcquisto(Utente utente)
    {
        List<ElementoCarrello> elementi = carrelloUtente.OttieniElementi();

        if (elementi.Count == 0)
        {
            throw new InvalidOperationException("Il carrello è vuoto: impossibile confermare l'acquisto.");
        }

        foreach (ElementoCarrello elemento in elementi)
        {
            if (elemento.QuantitaScelta > elemento.ProdottoSelezionato.QuantitaDisponibile)
            {
                throw new InvalidOperationException(
                    "Quantità non più disponibile per il prodotto: " + elemento.ProdottoSelezionato.Nome);
            }
        }

        List<ElementoAcquistato> prodottiAcquistati = elementi
            .Select(e => new ElementoAcquistato(
                e.ProdottoSelezionato.CodiceProdotto,
                e.ProdottoSelezionato.Nome,
                e.QuantitaScelta,
                e.PrezzoUnitario))
            .ToList();

        foreach (ElementoCarrello elemento in elementi)
        {
            catalogoProdotti.ModificaQuantitaProdotto(
                elemento.ProdottoSelezionato.CodiceProdotto,
                -elemento.QuantitaScelta);
        }

        Acquisto acquisto = new Acquisto(utente, prodottiAcquistati);
        storicoAcquisti.RegistraAcquisto(acquisto);
        carrelloUtente.SvuotaCarrello();

        return acquisto;
    }

    public List<ReportProdotto> CreaReportProdotti()
    {
        // Metodo già implementato: prepara il report richiesto per l'amministratore.
        return catalogoProdotti.OttieniTuttiIProdotti()
            .Select(prodotto => new ReportProdotto(
                prodotto.CodiceProdotto,
                prodotto.Nome,
                prodotto.QuantitaIniziale,
                prodotto.CalcolaQuantitaVenduta(),
                prodotto.QuantitaDisponibile))
            .ToList();
    }

    public void StampaAcquisto(Acquisto acquisto)
    {
        // Metodo già implementato: mostra i dettagli di un acquisto completato.
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Utente: " + acquisto.NomeUtente);
        Console.WriteLine("Data: " + acquisto.DataAcquisto.ToString("dd/MM/yyyy HH:mm"));
        Console.WriteLine("Prodotti acquistati:");

        foreach (ElementoAcquistato elemento in acquisto.ProdottiAcquistati)
        {
            Console.WriteLine(
                "- " + elemento.CodiceProdotto + " - " +
                elemento.NomeProdotto + " - " +
                "Quantità: " + elemento.QuantitaAcquistata + " - " +
                "Prezzo unitario: " + elemento.PrezzoUnitario.ToString("0.00") + " euro - " +
                "Parziale: " + elemento.TotaleParziale.ToString("0.00") + " euro");
        }

        Console.WriteLine("Totale ordine: " + acquisto.TotaleOrdine.ToString("0.00") + " euro");
    }

    public void StampaReportProdotti()
    {
        // Metodo già implementato: mostra il report quantità richiesto all'amministratore.
        List<ReportProdotto> report = CreaReportProdotti();

        Console.WriteLine();
        Console.WriteLine("=== REPORT PRODOTTI ===");

        if (report.Count == 0)
        {
            Console.WriteLine("Nessun prodotto presente nel catalogo.");
            return;
        }

        foreach (ReportProdotto riga in report)
        {
            Console.WriteLine(
                riga.CodiceProdotto + " - " +
                riga.NomeProdotto + " - " +
                "Iniziale: " + riga.QuantitaIniziale + " - " +
                "Venduta: " + riga.QuantitaVenduta + " - " +
                "Disponibile: " + riga.QuantitaDisponibile);
        }
    }
}

public class ReportProdotto
{
    public string CodiceProdotto { get; private set; }
    public string NomeProdotto { get; private set; }
    public int QuantitaIniziale { get; private set; }
    public int QuantitaVenduta { get; private set; }
    public int QuantitaDisponibile { get; private set; }

    public ReportProdotto(string codiceProdotto, string nomeProdotto, int quantitaIniziale, int quantitaVenduta, int quantitaDisponibile)
    {
        CodiceProdotto = codiceProdotto;
        NomeProdotto = nomeProdotto;
        QuantitaIniziale = quantitaIniziale;
        QuantitaVenduta = quantitaVenduta;
        QuantitaDisponibile = quantitaDisponibile;
    }
}
