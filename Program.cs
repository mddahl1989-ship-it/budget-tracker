
Console.WriteLine("==BUDGET TRACKER==");
Console.WriteLine("Moin, bei deinem Budget Tracker!");

bool running = true;
decimal gesamteinnahmen = 0;
decimal gesamtausgaben = 0;

while (running)
{
    Console.WriteLine();
    Console.WriteLine("1. Einnahmen hinzufügen");
    Console.WriteLine("2. Ausgaben hinzufügen");
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

        if (decimal.TryParse(eingabe, out decimal einnahmen))
        {
            gesamteinnahmen = gesamteinnahmen + einnahmen;
            Console.WriteLine("Gesamteinnahmen: " + gesamteinnahmen + " Euro");
        }
        else
        {
            Console.WriteLine("Bitte gib einen gültigen Betrag ein.");
        }
    }
    else if (auswahl == "2")
    {
        Console.Write("Wie hoch sind deine Ausgaben? ");
        string eingabe = Console.ReadLine() ?? "";

        if (decimal.TryParse(eingabe, out decimal ausgaben))
        {
            gesamtausgaben = gesamtausgaben + ausgaben;
            Console.WriteLine("Gesamtausgaben: " + gesamtausgaben + " Euro");
        }
        else
        {
            Console.WriteLine("Bitte gib einen gültigen Betrag ein.");
        }
    }
    else
    {
        Console.WriteLine("Ungültige Auswahl. Bitte versuche es erneut.");
    }
}

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

