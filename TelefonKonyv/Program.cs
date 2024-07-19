using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Person
{
    public string Nev { get; set; }
    public string Cim { get; set; }
    public string ApaNeve { get; set; }
    public string AnyaNeve { get; set; }
    public long Telefonszam { get; set; }
    public string Nem { get; set; }
    public string Email { get; set; }
    public string SzemIgSzam { get; set; }
}

class Program
{
    static void Main()
    {
        Console.BackgroundColor = ConsoleColor.Magenta;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();
        Kezd();
    }

    static void Kezd()
    {
        Menu();
    }

    static void Menu()
    {
        Console.Clear();
        Console.WriteLine("\t\t**********ÜDVÖZÖLJÜK A TELEFONKÖNYVBEN*************");
        Console.WriteLine("\n\n\t\t\t MENÜ\t\t\n\n");
        Console.WriteLine("\t1. Új hozzáadása \t2. Lista \t3. Kilépés \n\t4. Módosítás \t5. Keresés \t6. Törlés");
        switch (Console.ReadKey().KeyChar)
        {
            case '1':
                RekordHozzaadas();
                break;
            case '2':
                ListaRekord();
                break;
            case '3':
                Environment.Exit(0);
                break;
            case '4':
                RekordModositas();
                break;
            case '5':
                RekordKereses();
                break;
            case '6':
                RekordTorles();
                break;
            default:
                Console.Clear();
                Console.WriteLine("\nCsak 1-től 6-ig adjon meg értéket");
                Console.WriteLine("\n Nyomjon meg egy gombot");
                Console.ReadKey();
                Menu();
                break;
        }
    }

    static void RekordHozzaadas()
    {
        Console.Clear();
        var szemely = new Person();

        Console.Write("\n Adja meg a nevet: ");
        szemely.Nev = Console.ReadLine();
        Console.Write("\nAdja meg a címet: ");
        szemely.Cim = Console.ReadLine();
        Console.Write("\nAdja meg az apa nevét: ");
        szemely.ApaNeve = Console.ReadLine();
        Console.Write("\nAdja meg az anya nevét: ");
        szemely.AnyaNeve = Console.ReadLine();
        Console.Write("\nAdja meg a telefonszámot: ");
        szemely.Telefonszam = long.Parse(Console.ReadLine());
        Console.Write("Adja meg a nemet: ");
        szemely.Nem = Console.ReadLine();
        Console.Write("\nAdja meg az e-mail címet: ");
        szemely.Email = Console.ReadLine();
        Console.Write("\nAdja meg a személyazonosító számot: ");
        szemely.SzemIgSzam = Console.ReadLine();

        using (var fs = new FileStream("project.dat", FileMode.Append, FileAccess.Write))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(fs, szemely);
        }

        Console.WriteLine("\nrekord elmentve");
        Console.WriteLine("\n\nNyomjon meg egy gombot");
        Console.ReadKey();
        Menu();
    }

    static void ListaRekord()
    {
        Console.Clear();
        try
        {
            using (var fs = new FileStream("project.dat", FileMode.Open, FileAccess.Read))
            {
                var formatter = new BinaryFormatter();
                while (fs.Position < fs.Length)
                {
                    var szemely = (Person)formatter.Deserialize(fs);
                    Console.WriteLine("\n\n\n AZ ÖN REKORDJA\n\n ");
                    Console.WriteLine($"Név: {szemely.Nev}\nCím: {szemely.Cim}\nApa neve: {szemely.ApaNeve}\nAnya neve: {szemely.AnyaNeve}\nTelefonszám: {szemely.Telefonszam}\nNem: {szemely.Nem}\nE-mail: {szemely.Email}\nSzemélyazonosító szám: {szemely.SzemIgSzam}");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("\nHiba a fájl megnyitásakor a listázásnál:");
        }
        Console.WriteLine("\n Nyomjon meg egy gombot");
        Console.ReadKey();
        Menu();
    }

    static void RekordKereses()
    {
        Console.Clear();
        Console.WriteLine("\nAdja meg a keresett személy nevét\n");
        string nev = Console.ReadLine();
        bool megtalalt = false;

        try
        {
            using (var fs = new FileStream("project.dat", FileMode.Open, FileAccess.Read))
            {
                var formatter = new BinaryFormatter();
                while (fs.Position < fs.Length)
                {
                    var szemely = (Person)formatter.Deserialize(fs);
                    if (szemely.Nev.Equals(nev, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"\n\tRészletes információ {nev} névhez");
                        Console.WriteLine($"Név: {szemely.Nev}\nCím: {szemely.Cim}\nApa neve: {szemely.ApaNeve}\nAnya neve: {szemely.AnyaNeve}\nTelefonszám: {szemely.Telefonszam}\nNem: {szemely.Nem}\nE-mail: {szemely.Email}\nSzemélyazonosító szám: {szemely.SzemIgSzam}");
                        megtalalt = true;
                        break;
                    }
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("\nHiba a fájl megnyitásakor");
        }

        if (!megtalalt)
        {
            Console.WriteLine("Rekord nem található");
        }

        Console.WriteLine("\n Nyomjon meg egy gombot");
        Console.ReadKey();
        Menu();
    }

    static void RekordTorles()
    {
        Console.Clear();
        Console.WriteLine("Adja meg a TÖRLENDŐ NÉVET:");
        string nev = Console.ReadLine();
        bool megtalalt = false;

        try
        {
            using (var fs = new FileStream("project.dat", FileMode.Open, FileAccess.Read))
            {
                var formatter = new BinaryFormatter();
                var ideiglenesLista = new List<Person>();
                while (fs.Position < fs.Length)
                {
                    var szemely = (Person)formatter.Deserialize(fs);
                    if (!szemely.Nev.Equals(nev, StringComparison.OrdinalIgnoreCase))
                    {
                        ideiglenesLista.Add(szemely);
                    }
                    else
                    {
                        megtalalt = true;
                    }
                }

                if (megtalalt)
                {
                    using (var tempFs = new FileStream("temp.dat", FileMode.Create, FileAccess.Write))
                    {
                        foreach (var tempSzemely in ideiglenesLista)
                        {
                            formatter.Serialize(tempFs, tempSzemely);
                        }
                    }

                    File.Delete("project.dat");
                    File.Move("temp.dat", "project.dat");
                    Console.WriteLine("REKORD SIKERESEN TÖRÖLVE.");
                }
                else
                {
                    Console.WriteLine("NINCS REKORD HOGY TÖRÖLJEM");

                }
            }
        }

        catch (FileNotFoundException)
        {
            Console.WriteLine("NINCS HOZZÁADOTT ADAT.");
        }

        Console.WriteLine("\n Nyomjon meg egy gombot");
        Console.ReadKey();
        Menu();
    }

    static void RekordModositas()
    {
        Console.Clear();
        Console.WriteLine("\nAdja meg a MÓDOSÍTANDÓ NÉVET:\n");
        string nev = Console.ReadLine();
        bool megtalalt = false;

        try
        {
            using (var fs = new FileStream("project.dat", FileMode.Open, FileAccess.ReadWrite))
            {
                var formatter = new BinaryFormatter();
                long lastPosition = 0;
                while (fs.Position < fs.Length)
                {
                    lastPosition = fs.Position;
                    var szemely = (Person)formatter.Deserialize(fs);
                    if (szemely.Nev.Equals(nev, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Write("\n Adja meg az új nevet: ");
                        szemely.Nev = Console.ReadLine();
                        Console.Write("\nAdja meg az új címet: ");
                        szemely.Cim = Console.ReadLine();
                        Console.Write("\nAdja meg az apa új nevét: ");
                        szemely.ApaNeve = Console.ReadLine();
                        Console.Write("\nAdja meg az anya új nevét: ");
                        szemely.AnyaNeve = Console.ReadLine();
                        Console.Write("\nAdja meg az új telefonszámot: ");
                        szemely.Telefonszam = long.Parse(Console.ReadLine());
                        Console.Write("\nAdja meg az új nemet: ");
                        szemely.Nem = Console.ReadLine();
                        Console.Write("\nAdja meg az új e-mail címet: ");
                        szemely.Email = Console.ReadLine();
                        Console.Write("\nAdja meg az új személyazonosító számot: ");
                        szemely.SzemIgSzam = Console.ReadLine();

                        fs.Seek(lastPosition, SeekOrigin.Begin);
                        formatter.Serialize(fs, szemely);
                        megtalalt = true;
                        break;
                    }
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("NINCS HOZZÁADOTT ADAT.");
        }
        S
        if (megtalalt)
        {
            Console.WriteLine("\n Az adatok módosítva");
        }
        else
        {
            Console.WriteLine(" \n A megadott név nem található");
        }

        Console.WriteLine("\n Nyomjon meg egy gombot");
        Console.ReadKey();
        Menu();
    }
}
