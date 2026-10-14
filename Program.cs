using System.IO;
using System.Text.Json;

Console.WriteLine("==BUDGET TRACKER==");
Console.WriteLine("Moin, bei deinem Budget Tracker!");

bool running = true;

string dateiname = DateTime.Now.ToString("yyyy-MM-dd") + ".json";

List<Transaktion> transaktionen = new List<Transaktion>();

// Gespeicherte Transaktionen laden
if (File.Exists(dateiname))
{
    string gespeicherterJsonText = File.ReadAllText(dateiname);

    transaktionen = JsonSerializer.Deserialize<List<Transaktion>>(gespeicherterJsonText)
        ?? new List<Transaktion>();
}

// Einnahmen und Ausgaben aus den gespeicherten Daten berechnen
decimal gesamteinnahmen = 0;
decimal gesamtausgaben = 0;

foreach (Transaktion transaktion in transaktionen)
{
    if (transaktion.Art == TransaktionsArt.Einnahmen)
    {
        gesamteinnahmen = gesamteinnahmen + transaktion.Betrag;
    }
    else if (transaktion.Art == TransaktionsArt.Ausgaben)
    {
        gesamtausgaben = gesamtausgaben + transaktion.Betrag;
    }
}

while (running)
{
    Console.WriteLine();
    Console.WriteLine("1. Einnahmen hinzufügen");
    Console.WriteLine("2. Ausgaben hinzufügen");
    Console.WriteLine("3. Transaktionen anzeigen");
    Console.WriteLine("4. Transaktionen löschen");
    Console.WriteLine("5. Bericht anzeigen");
    Console.WriteLine("0. Beenden");
    Console.Write("Deine Auswahl: ");

    string auswahl = Console.ReadLine() ?? "";

    if (auswahl == "0")
    {
        running = false;
    }
    else if (auswahl == "1")
    {
        Console.Write("Wie hoch sind deine Einnahmen? ");
        string eingabe = Console.ReadLine() ?? "";

        if (decimal.TryParse(eingabe, out decimal einnahmen) && einnahmen > 0)
        {
            gesamteinnahmen = gesamteinnahmen + einnahmen;

            transaktionen.Add(
                new Transaktion(einnahmen, TransaktionsArt.Einnahmen, DateTime.Now));

            Console.WriteLine("Gesamteinnahmen: " + gesamteinnahmen + " Euro");
        }
        else
        {
            Console.WriteLine("Bitte gib einen gültigen Betrag größer als 0 ein.");
        }
    }
    else if (auswahl == "2")
    {
        Console.Write("Wie hoch sind deine Ausgaben? ");
        string eingabe = Console.ReadLine() ?? "";

        if (decimal.TryParse(eingabe, out decimal ausgaben) && ausgaben > 0)
        {
            gesamtausgaben = gesamtausgaben + ausgaben;

            transaktionen.Add(
                new Transaktion(ausgaben, TransaktionsArt.Ausgaben, DateTime.Now));

            Console.WriteLine("Gesamtausgaben: " + gesamtausgaben + " Euro");
        }
        else
        {
            Console.WriteLine("Bitte gib einen gültigen Betrag größer als 0 ein.");
        }
    }
    else if (auswahl == "3")
    {
        if (transaktionen.Count == 0)
        {
            Console.WriteLine("Noch keine Transaktionen vorhanden.");
        }

        for (int i = 0; i < transaktionen.Count; i++)
        {
            Transaktion transaktion = transaktionen[i];

            Console.WriteLine(
                (i + 1) + ". " +
                transaktion.Datum + " | " +
                transaktion.Art + " | " +
                transaktion.Betrag + " Euro");
        }
    }
    else if (auswahl == "4")
{
    for (int i = 0; i < transaktionen.Count; i++)
    {
        Transaktion transaktion = transaktionen[i];

        Console.WriteLine(
            (i + 1) + ". " +
            transaktion.Art + " | " +
            transaktion.Betrag + " Euro");
    }

    Console.Write("Welche Transaktion möchtest du löschen? ");
    string eingabe = Console.ReadLine() ?? "";

    if (int.TryParse(eingabe, out int nummer) &&
        nummer >= 1 &&
        nummer <= transaktionen.Count)
    {
        Transaktion entfernt = transaktionen[nummer - 1];

        transaktionen.RemoveAt(nummer - 1);

        if (entfernt.Art == TransaktionsArt.Einnahmen)
        {
            gesamteinnahmen = gesamteinnahmen - entfernt.Betrag;
        }
        else
        {
            gesamtausgaben = gesamtausgaben - entfernt.Betrag;
        }

        Console.WriteLine("Transaktion wurde gelöscht.");
    }
    else
    {
        Console.WriteLine("Ungültige Transaktionsnummer.");
    }
}


else if (auswahl == "5")
{
    Console.Write("Startdatum (TT.MM.JJJJ): ");
    string startEingabe = Console.ReadLine() ?? "";

    Console.Write("Enddatum (TT.MM.JJJJ): ");
    string endeEingabe = Console.ReadLine() ?? "";

    if (DateTime.TryParse(startEingabe, out DateTime startDatum) &&
        DateTime.TryParse(endeEingabe, out DateTime endeDatum) &&
        startDatum.Date <= endeDatum.Date)
    {
        var zeitraum = transaktionen
            .Where(t => t.Datum.Date >= startDatum.Date &&
                        t.Datum.Date <= endeDatum.Date)
            .ToList();

        var einnahmenZeitraum = zeitraum
            .Where(t => t.Art == TransaktionsArt.Einnahmen)
            .Sum(t => t.Betrag);

        var ausgabenZeitraum = zeitraum
            .Where(t => t.Art == TransaktionsArt.Ausgaben)
            .Sum(t => t.Betrag);

        Console.WriteLine("=== BUDGET-BERICHT ===");
        Console.WriteLine("Von: " + startDatum.ToString("dd.MM.yyyy"));
        Console.WriteLine("Bis: " + endeDatum.ToString("dd.MM.yyyy"));
        Console.WriteLine("Einnahmen: " + einnahmenZeitraum + " Euro");
        Console.WriteLine("Ausgaben: " + ausgabenZeitraum + " Euro");
        Console.WriteLine("Restbetrag: " +
            (einnahmenZeitraum - ausgabenZeitraum) + " Euro");
    }
    else
    {
        Console.WriteLine("Bitte gib gültige Daten ein. Das Startdatum darf nicht nach dem Enddatum liegen.");
    }
}
 

    else
    {
        Console.WriteLine("Ungültige Auswahl. Bitte versuche es erneut.");
    }
}

// Transaktionen für heute speichern
string json = JsonSerializer.Serialize(transaktionen);
File.WriteAllText(dateiname, json);

Console.WriteLine("Programm beendet!");

decimal rest = gesamteinnahmen - gesamtausgaben;

Console.WriteLine("Restbetrag: " + rest + " Euro");

if (rest >= 0)
{
    Console.WriteLine("Du hast noch Kohle übrig!");
}
else
{
    Console.WriteLine("Pass auf, du bist inne miesen!");
}

Console.WriteLine("Gesamteinnahmen: " + gesamteinnahmen + " Euro");
Console.WriteLine("Gesamtausgaben: " + gesamtausgaben + " Euro");

enum TransaktionsArt
{
    Einnahmen,
    Ausgaben
}

record Transaktion(decimal Betrag, TransaktionsArt Art, DateTime Datum);