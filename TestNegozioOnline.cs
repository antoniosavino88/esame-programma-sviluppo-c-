using System;
using System.Collections.Generic;

/*
 * TEST MANUALI SENZA FRAMEWORK ESTERNI
 *
 * Questo file resta separato da Program.cs, ma usa le stesse classi e le stesse
 * signature del template. I test servono allo studente per capire quali parti
 * funzionano e quali parti sono ancora da completare.
 *
 * Come usarlo:
 * 1. tenere questo file nello stesso progetto di Program.cs;
 * 2. chiamare temporaneamente TestNegozioOnline.EseguiTuttiITest() dal Main;
 * 3. leggere a console l'elenco dei test PASS/FAIL.
 *
 * Nota: i metodi ancora vuoti con TODO falliranno con NotImplementedException.
 * È il comportamento atteso del template iniziale.
 */

public static class TestNegozioOnline
{
    public static void EseguiTuttiITest()
    {
        List<TestDaEseguire> test = new List<TestDaEseguire>
        {
            new TestDaEseguire("Prodotto: modifica prezzo valido", TestProdottoModificaPrezzoValido),
            new TestDaEseguire("Prodotto: rifiuta prezzo non valido", TestProdottoRifiutaPrezzoNonValido),
            new TestDaEseguire("Prodotto: modifica quantità e calcola venduto", TestProdottoModificaQuantitaECalcolaVenduto),
            new TestDaEseguire("Prodotto: rifiuta magazzino negativo", TestProdottoRifiutaMagazzinoNegativo),
            new TestDaEseguire("Catalogo: aggiunge, cerca ed elenca prodotti", TestCatalogoAggiungeCercaElencaProdotti),
            new TestDaEseguire("Catalogo: rifiuta codice duplicato", TestCatalogoRifiutaCodiceDuplicato),
            new TestDaEseguire("Catalogo: elimina prodotto", TestCatalogoEliminaProdotto),
            new TestDaEseguire("Catalogo: modifica prezzo prodotto", TestCatalogoModificaPrezzo),
            new TestDaEseguire("Catalogo: modifica quantità prodotto", TestCatalogoModificaQuantita),
            new TestDaEseguire("Carrello: aggiunge prodotto e calcola totale", TestCarrelloAggiungeProdottoECalcolaTotale),
            new TestDaEseguire("Carrello: rifiuta quantità non valide", TestCarrelloRifiutaQuantitaNonValide),
            new TestDaEseguire("Carrello: modifica quantità prodotto", TestCarrelloModificaQuantita),
            new TestDaEseguire("Carrello: rimuove e svuota", TestCarrelloRimuoveESvuota),
            new TestDaEseguire("Storico: registra, elenca e filtra acquisti", TestStoricoRegistraElencaFiltra),
            new TestDaEseguire("Servizio: aggiunge al carrello tramite codice", TestServizioAggiungeAlCarrelloDaCodice),
            new TestDaEseguire("Servizio: conferma acquisto completo", TestServizioConfermaAcquistoCompleto),
            new TestDaEseguire("Servizio: rifiuta carrello vuoto", TestServizioRifiutaCarrelloVuoto),
            new TestDaEseguire("Servizio: crea report prodotti", TestServizioCreaReportProdotti),
            new TestDaEseguire("Acquisto: calcola totale ordine", TestAcquistoCalcolaTotaleOrdine)
        };

        int superati = 0;

        Console.WriteLine("=== TEST NEGOZIO ONLINE ===");

        foreach (TestDaEseguire singoloTest in test)
        {
            try
            {
                singoloTest.Azione();
                superati++;
                Console.WriteLine("[PASS] " + singoloTest.Nome);
            }
            catch (NotImplementedException eccezione)
            {
                Console.WriteLine("[FAIL - TODO] " + singoloTest.Nome + " -> " + eccezione.Message);
            }
            catch (Exception eccezione)
            {
                Console.WriteLine("[FAIL] " + singoloTest.Nome + " -> " + eccezione.Message);
            }
        }

        Console.WriteLine();
        Console.WriteLine("Risultato: " + superati + "/" + test.Count + " test superati.");
        Console.WriteLine("I test FAIL - TODO indicano metodi ancora da implementare.");
    }

    private static void TestProdottoModificaPrezzoValido()
    {
        Prodotto prodotto = new Prodotto("P001", "Tastiera", 50m, 10);

        prodotto.CambiaPrezzo(65.99m);

        AsserisciUguale(65.99m, prodotto.Prezzo, "Il prezzo del prodotto non è stato aggiornato.");
    }

    private static void TestProdottoRifiutaPrezzoNonValido()
    {
        Prodotto prodotto = new Prodotto("P001", "Tastiera", 50m, 10);

        AsserisciEccezione<ArgumentException>(() => prodotto.CambiaPrezzo(0m), "Prezzo zero non rifiutato.");
        AsserisciEccezione<ArgumentException>(() => prodotto.CambiaPrezzo(-5m), "Prezzo negativo non rifiutato.");
    }

    private static void TestProdottoModificaQuantitaECalcolaVenduto()
    {
        Prodotto prodotto = new Prodotto("P001", "Tastiera", 50m, 10);

        prodotto.CambiaQuantita(-3);

        AsserisciUguale(7, prodotto.QuantitaDisponibile, "La quantità disponibile non è corretta.");
        AsserisciUguale(3, prodotto.CalcolaQuantitaVenduta(), "La quantità venduta non è corretta.");
    }

    private static void TestProdottoRifiutaMagazzinoNegativo()
    {
        Prodotto prodotto = new Prodotto("P001", "Tastiera", 50m, 10);

        AsserisciEccezione<InvalidOperationException>(() => prodotto.CambiaQuantita(-11), "Magazzino negativo non rifiutato.");
    }

    private static void TestCatalogoAggiungeCercaElencaProdotti()
    {
        CatalogoProdotti catalogo = new CatalogoProdotti();
        Prodotto prodotto = new Prodotto("P001", "Tastiera", 50m, 10);

        catalogo.AggiungiProdotto(prodotto);

        AsserisciUguale(1, catalogo.OttieniTuttiIProdotti().Count, "Il catalogo dovrebbe contenere un prodotto.");
        AsserisciUguale(prodotto, catalogo.CercaProdottoPerCodice("p001"), "La ricerca per codice dovrebbe ignorare maiuscole e minuscole.");
    }

    private static void TestCatalogoRifiutaCodiceDuplicato()
    {
        CatalogoProdotti catalogo = new CatalogoProdotti();
        catalogo.AggiungiProdotto(new Prodotto("P001", "Tastiera", 50m, 10));

        AsserisciEccezione<InvalidOperationException>(
            () => catalogo.AggiungiProdotto(new Prodotto("P001", "Mouse", 20m, 5)),
            "Il catalogo dovrebbe rifiutare codici duplicati.");
    }

    private static void TestCatalogoEliminaProdotto()
    {
        CatalogoProdotti catalogo = CreaCatalogoBase();

        bool eliminato = catalogo.EliminaProdotto("P001");
        bool eliminatoDiNuovo = catalogo.EliminaProdotto("P001");

        AsserisciVero(eliminato, "Il prodotto esistente dovrebbe essere eliminato.");
        AsserisciFalso(eliminatoDiNuovo, "Un prodotto già eliminato non dovrebbe risultare eliminato di nuovo.");
        AsserisciUguale(null, catalogo.CercaProdottoPerCodice("P001"), "Il prodotto eliminato non dovrebbe più essere cercabile.");
    }

    private static void TestCatalogoModificaPrezzo()
    {
        CatalogoProdotti catalogo = CreaCatalogoBase();

        bool modificato = catalogo.ModificaPrezzoProdotto("P001", 75m);
        Prodotto? prodotto = catalogo.CercaProdottoPerCodice("P001");

        AsserisciVero(modificato, "La modifica del prezzo dovrebbe riuscire.");
        AsserisciNonNull(prodotto, "Il prodotto dovrebbe esistere.");
        AsserisciUguale(75m, prodotto!.Prezzo, "Il prezzo non è stato aggiornato.");
        AsserisciFalso(catalogo.ModificaPrezzoProdotto("NON_ESISTE", 10m), "Un codice inesistente dovrebbe restituire false.");
    }

    private static void TestCatalogoModificaQuantita()
    {
        CatalogoProdotti catalogo = CreaCatalogoBase();

        bool aumentata = catalogo.ModificaQuantitaProdotto("P001", 5);
        bool diminuita = catalogo.ModificaQuantitaProdotto("P001", -3);
        Prodotto? prodotto = catalogo.CercaProdottoPerCodice("P001");

        AsserisciVero(aumentata, "L'aumento quantità dovrebbe riuscire.");
        AsserisciVero(diminuita, "La diminuzione quantità valida dovrebbe riuscire.");
        AsserisciNonNull(prodotto, "Il prodotto dovrebbe esistere.");
        AsserisciUguale(12, prodotto!.QuantitaDisponibile, "La quantità finale non è corretta.");
        AsserisciFalso(catalogo.ModificaQuantitaProdotto("NON_ESISTE", 1), "Un codice inesistente dovrebbe restituire false.");
    }

    private static void TestCarrelloAggiungeProdottoECalcolaTotale()
    {
        CarrelloUtente carrello = new CarrelloUtente();
        Prodotto tastiera = new Prodotto("P001", "Tastiera", 50m, 10);
        Prodotto mouse = new Prodotto("P002", "Mouse", 20m, 5);

        AsserisciVero(carrello.AggiungiAlCarrello(tastiera, 2), "L'aggiunta della tastiera dovrebbe riuscire.");
        AsserisciVero(carrello.AggiungiAlCarrello(mouse, 3), "L'aggiunta del mouse dovrebbe riuscire.");

        AsserisciUguale(2, carrello.OttieniElementi().Count, "Il carrello dovrebbe contenere due righe.");
        AsserisciUguale(160m, carrello.CalcolaTotale(), "Il totale del carrello non è corretto.");
    }

    private static void TestCarrelloRifiutaQuantitaNonValide()
    {
        CarrelloUtente carrello = new CarrelloUtente();
        Prodotto prodotto = new Prodotto("P001", "Tastiera", 50m, 3);

        AsserisciFalso(carrello.AggiungiAlCarrello(prodotto, 0), "La quantità zero dovrebbe essere rifiutata.");
        AsserisciFalso(carrello.AggiungiAlCarrello(prodotto, -1), "La quantità negativa dovrebbe essere rifiutata.");
        AsserisciFalso(carrello.AggiungiAlCarrello(prodotto, 4), "La quantità oltre magazzino dovrebbe essere rifiutata.");

        AsserisciVero(carrello.AggiungiAlCarrello(prodotto, 2), "L'aggiunta valida dovrebbe riuscire.");
        AsserisciFalso(carrello.AggiungiAlCarrello(prodotto, 2), "La somma delle quantità oltre magazzino dovrebbe essere rifiutata.");
    }

    private static void TestCarrelloModificaQuantita()
    {
        CarrelloUtente carrello = new CarrelloUtente();
        Prodotto prodotto = new Prodotto("P001", "Tastiera", 50m, 5);

        carrello.AggiungiAlCarrello(prodotto, 2);

        AsserisciVero(carrello.ModificaQuantitaNelCarrello("P001", 4), "La modifica quantità valida dovrebbe riuscire.");
        AsserisciUguale(200m, carrello.CalcolaTotale(), "Il totale dopo modifica quantità non è corretto.");
        AsserisciFalso(carrello.ModificaQuantitaNelCarrello("P001", 0), "La quantità zero dovrebbe essere rifiutata.");
        AsserisciFalso(carrello.ModificaQuantitaNelCarrello("P001", 6), "La quantità oltre magazzino dovrebbe essere rifiutata.");
        AsserisciFalso(carrello.ModificaQuantitaNelCarrello("NON_ESISTE", 1), "Un prodotto assente dal carrello dovrebbe restituire false.");
    }

    private static void TestCarrelloRimuoveESvuota()
    {
        CarrelloUtente carrello = new CarrelloUtente();
        Prodotto tastiera = new Prodotto("P001", "Tastiera", 50m, 10);
        Prodotto mouse = new Prodotto("P002", "Mouse", 20m, 5);

        carrello.AggiungiAlCarrello(tastiera, 1);
        carrello.AggiungiAlCarrello(mouse, 1);

        AsserisciVero(carrello.RimuoviDalCarrello("P001"), "La rimozione di un prodotto presente dovrebbe riuscire.");
        AsserisciFalso(carrello.RimuoviDalCarrello("P001"), "La rimozione di un prodotto non presente dovrebbe restituire false.");

        carrello.SvuotaCarrello();

        AsserisciUguale(0, carrello.OttieniElementi().Count, "Il carrello dovrebbe essere vuoto.");
        AsserisciUguale(0m, carrello.CalcolaTotale(), "Il totale del carrello vuoto dovrebbe essere zero.");
    }

    private static void TestStoricoRegistraElencaFiltra()
    {
        StoricoAcquisti storico = new StoricoAcquisti();
        Acquisto acquistoMario = CreaAcquisto("Mario");
        Acquisto acquistoLucia = CreaAcquisto("Lucia");

        storico.RegistraAcquisto(acquistoMario);
        storico.RegistraAcquisto(acquistoLucia);

        AsserisciUguale(2, storico.OttieniTuttiGliAcquisti().Count, "Lo storico dovrebbe contenere due acquisti.");
        AsserisciUguale(1, storico.OttieniAcquistiPerUtente("mario").Count, "Il filtro utente dovrebbe ignorare maiuscole/minuscole.");
        AsserisciUguale("Mario", storico.OttieniAcquistiPerUtente("mario")[0].NomeUtente, "Il filtro ha restituito l'utente sbagliato.");
    }

    private static void TestServizioAggiungeAlCarrelloDaCodice()
    {
        AmbienteTest ambiente = CreaAmbienteTest();

        AsserisciVero(ambiente.Servizio.AggiungiProdottoAlCarrello("P001", 2), "L'aggiunta tramite codice dovrebbe riuscire.");
        AsserisciFalso(ambiente.Servizio.AggiungiProdottoAlCarrello("NON_ESISTE", 1), "Un codice inesistente dovrebbe restituire false.");
        AsserisciUguale(100m, ambiente.Carrello.CalcolaTotale(), "Il totale carrello non è corretto.");
    }

    private static void TestServizioConfermaAcquistoCompleto()
    {
        AmbienteTest ambiente = CreaAmbienteTest();

        ambiente.Carrello.AggiungiAlCarrello(ambiente.Catalogo.CercaProdottoPerCodice("P001")!, 2);
        ambiente.Carrello.AggiungiAlCarrello(ambiente.Catalogo.CercaProdottoPerCodice("P002")!, 1);

        Acquisto acquisto = ambiente.Servizio.ConfermaAcquisto(new Utente("Mario"));

        AsserisciUguale("Mario", acquisto.NomeUtente, "Il nome utente dell'acquisto non è corretto.");
        AsserisciUguale(120m, acquisto.TotaleOrdine, "Il totale ordine non è corretto.");
        AsserisciUguale(1, ambiente.Storico.OttieniTuttiGliAcquisti().Count, "L'acquisto dovrebbe essere registrato nello storico.");
        AsserisciUguale(0, ambiente.Carrello.OttieniElementi().Count, "Il carrello dovrebbe essere svuotato dopo l'acquisto.");
        AsserisciUguale(8, ambiente.Catalogo.CercaProdottoPerCodice("P001")!.QuantitaDisponibile, "Il magazzino P001 non è stato scalato correttamente.");
        AsserisciUguale(4, ambiente.Catalogo.CercaProdottoPerCodice("P002")!.QuantitaDisponibile, "Il magazzino P002 non è stato scalato correttamente.");
    }

    private static void TestServizioRifiutaCarrelloVuoto()
    {
        AmbienteTest ambiente = CreaAmbienteTest();

        AsserisciEccezione<InvalidOperationException>(
            () => ambiente.Servizio.ConfermaAcquisto(new Utente("Mario")),
            "La conferma con carrello vuoto dovrebbe essere rifiutata.");
    }

    private static void TestServizioCreaReportProdotti()
    {
        AmbienteTest ambiente = CreaAmbienteTest();

        ambiente.Carrello.AggiungiAlCarrello(ambiente.Catalogo.CercaProdottoPerCodice("P001")!, 2);
        ambiente.Servizio.ConfermaAcquisto(new Utente("Mario"));

        List<ReportProdotto> report = ambiente.Servizio.CreaReportProdotti();
        ReportProdotto reportTastiera = report.Find(riga => riga.CodiceProdotto == "P001")!;

        AsserisciNonNull(reportTastiera, "Il report del prodotto P001 dovrebbe esistere.");
        AsserisciUguale(10, reportTastiera.QuantitaIniziale, "Quantità iniziale errata nel report.");
        AsserisciUguale(2, reportTastiera.QuantitaVenduta, "Quantità venduta errata nel report.");
        AsserisciUguale(8, reportTastiera.QuantitaDisponibile, "Quantità disponibile errata nel report.");
    }

    private static void TestAcquistoCalcolaTotaleOrdine()
    {
        List<ElementoAcquistato> elementi = new List<ElementoAcquistato>
        {
            new ElementoAcquistato("P001", "Tastiera", 2, 50m),
            new ElementoAcquistato("P002", "Mouse", 3, 20m)
        };

        Acquisto acquisto = new Acquisto(new Utente("Mario"), elementi);

        AsserisciUguale(160m, acquisto.TotaleOrdine, "Il totale ordine non è corretto.");
    }

    private static CatalogoProdotti CreaCatalogoBase()
    {
        CatalogoProdotti catalogo = new CatalogoProdotti();
        catalogo.AggiungiProdotto(new Prodotto("P001", "Tastiera", 50m, 10));
        catalogo.AggiungiProdotto(new Prodotto("P002", "Mouse", 20m, 5));
        return catalogo;
    }

    private static AmbienteTest CreaAmbienteTest()
    {
        CatalogoProdotti catalogo = CreaCatalogoBase();
        CarrelloUtente carrello = new CarrelloUtente();
        StoricoAcquisti storico = new StoricoAcquisti();
        ServizioNegozio servizio = new ServizioNegozio(catalogo, carrello, storico);

        return new AmbienteTest(catalogo, carrello, storico, servizio);
    }

    private static Acquisto CreaAcquisto(string nomeUtente)
    {
        List<ElementoAcquistato> prodotti = new List<ElementoAcquistato>
        {
            new ElementoAcquistato("P001", "Tastiera", 1, 50m)
        };

        return new Acquisto(new Utente(nomeUtente), prodotti);
    }

    private static void AsserisciVero(bool valore, string messaggio)
    {
        if (!valore)
        {
            throw new Exception(messaggio);
        }
    }

    private static void AsserisciFalso(bool valore, string messaggio)
    {
        if (valore)
        {
            throw new Exception(messaggio);
        }
    }

    private static void AsserisciUguale<T>(T atteso, T ottenuto, string messaggio)
    {
        if (!EqualityComparer<T>.Default.Equals(atteso, ottenuto))
        {
            throw new Exception(messaggio + " Atteso: " + atteso + ", ottenuto: " + ottenuto + ".");
        }
    }

    private static void AsserisciNonNull(object? valore, string messaggio)
    {
        if (valore == null)
        {
            throw new Exception(messaggio);
        }
    }

    private static void AsserisciEccezione<T>(Action azione, string messaggio) where T : Exception
    {
        try
        {
            azione();
        }
        catch (T)
        {
            return;
        }

        throw new Exception(messaggio);
    }

    private class TestDaEseguire
    {
        public string Nome { get; private set; }
        public Action Azione { get; private set; }

        public TestDaEseguire(string nome, Action azione)
        {
            Nome = nome;
            Azione = azione;
        }
    }

    private class AmbienteTest
    {
        public CatalogoProdotti Catalogo { get; private set; }
        public CarrelloUtente Carrello { get; private set; }
        public StoricoAcquisti Storico { get; private set; }
        public ServizioNegozio Servizio { get; private set; }

        public AmbienteTest(CatalogoProdotti catalogo, CarrelloUtente carrello, StoricoAcquisti storico, ServizioNegozio servizio)
        {
            Catalogo = catalogo;
            Carrello = carrello;
            Storico = storico;
            Servizio = servizio;
        }
    }
}
