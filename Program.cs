using System;
using System.Data.SqlClient;
using System.Windows;

namespace csharp_biblioteca_db
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Biblioteca b = new Biblioteca("Comunale");
            StreamReader reader = new StreamReader("elenco.txt");
            string linea;
            while ((linea = reader.ReadLine()) != null)
            {
                var vett = linea.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                string s = vett[0];
                var cn = s.Split(new char[] { ' ' });
                string nome = cn[0];
                string cognome = "";
                try
                {
                    cognome = s.Substring(cn[0].Length + 1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                string titolo = vett[1];
                Console.WriteLine("Nome: {0}, Cognome: {1}, Titolo: {2}", nome, cognome, titolo);

                string email = nome + "@" + cognome + ".it";

                Autore MioAutore = new Autore(nome, cognome, email);
                List<Autore> listaAutori = new List<Autore>();
                listaAutori.Add(MioAutore);
                b.AggiungiLibro(DB.GetUniqueId(), titolo, "Romanzo", 200, "SS1", listaAutori);
            }

            //b.AggiungiScaffale("SS1");
            //b.AggiungiScaffale("SS2");
            //b.AggiungiScaffale("SS3");

            //b.ScaffaliBiblioteca.ForEach(item => Console.WriteLine(item.Numero));


            //Console.WriteLine("Per inserire un libro premi 1");
            //Console.WriteLine("Per inserire un DVD premi 2");
            //Console.WriteLine("Per cercare un documento per autore premi 3");
            //int choice = Convert.ToInt32(Console.ReadLine());

            //if (choice == 1)
            //{
            //    Console.WriteLine("Inserisci un libro");
            //    Console.WriteLine("Inserisci nome autore");
            //    string nAutore = Console.ReadLine();
            //    Console.WriteLine("Inserisci cognome autore");
            //    string cAutore = Console.ReadLine();
            //    Console.WriteLine("Inserisci mail autore");
            //    string mAutore = Console.ReadLine();

            //    Console.WriteLine("==========================================");

            //    Console.WriteLine("Inserisci il titolo del libro");
            //    string titoloLibro = Console.ReadLine();
            //    Console.WriteLine("Inserisci il settore del libro");
            //    string settoreLibro = Console.ReadLine();
            //    Console.WriteLine("Inserisci il numero di pagine del libro");
            //    int nPagineLibro = Convert.ToInt32(Console.ReadLine());
            //    Console.WriteLine("Scegli tra questi scaffali dove inserire il libro: SS1,SS2,SS3");
            //    string listaLLibro = Console.ReadLine();

            //    List<Autore> listaAutori = new List<Autore>();
            //    Autore autoreMioLibro = new Autore(nAutore, cAutore, mAutore);
            //    listaAutori.Add(autoreMioLibro);

            //    b.AggiungiLibro(DB.GetUniqueId(), titoloLibro, settoreLibro, nPagineLibro, listaLLibro, listaAutori);
            //}
            //else if (choice == 2)
            //{
            //    Console.WriteLine("INSERISCI UN LIBRO");
            //    Console.WriteLine("Inserisci nome autore");
            //    string nAutore = Console.ReadLine();
            //    Console.WriteLine("Inserisci cognome autore");
            //    string cAutore = Console.ReadLine();
            //    Console.WriteLine("Inserisci mail autore");
            //    string mAutore = Console.ReadLine();

            //    Console.WriteLine("==========================================");

            //    Console.WriteLine("Inserisci il titolo del libro");
            //    string titoloDvd = Console.ReadLine();
            //    Console.WriteLine("Inserisci il settore del libro");
            //    string SettoreDvd = Console.ReadLine();
            //    Console.WriteLine("Scegli tra questi scaffali dove inserire il libro: SS1,SS2,SS3");
            //    string listaDvd = Console.ReadLine();
            //    Console.WriteLine("Inserisci il numero di pagine del libro");
            //    int durataDvd = Convert.ToInt32(Console.ReadLine());

            //    List<Autore> listaAutori = new List<Autore>();
            //    Autore autoreMioLibro = new Autore(nAutore, cAutore, mAutore);
            //    listaAutori.Add(autoreMioLibro);

            //    b.AggiungiDvd(DB.GetUniqueId(), titoloDvd, SettoreDvd, listaDvd, durataDvd, listaAutori);
            //}
            //else if (choice == 3)
            //{
            //    b.CercaDocumentoPerAutore("Francesco", "Costa");
            //}
            //else
            //{
            //    Environment.Exit(0);
            //}

            //Console.WriteLine("lista operazioni");
            //    Console.WriteLine("\t1: cerca libro per autore");
            //    Console.WriteLine("Cosa vuoi fare");
            //    string sAppo = Console.ReadLine();
            //    while (sAppo != "")
            //    {
            //        b.GestisciOperazioniBiblioteca(Convert.ToInt32(sAppo));
            //    }

            //Libro l1 = new Libro("ISBN1", "Titolo 1", 2009, "Storia", 220);

            //Autore a1 = new Autore("Nome 1", "Cognome 1");
            //Autore a2 = new Autore("Nome 2", "Cognome 2");

            ////Autori.Add(a1);
            ////Autori.Add(a2);

            ////Scaffale = s1;

            ////b.Documenti.Add(l1);

            //#region "Libro 2"
            //Libro l2 = new Libro("ISBN2", "Titolo 2", 2009, "Saggistica", 180);

            //Autore a3 = new Autore("Nome 3", "Cognome 3");
            //Autore a4 = new Autore("Nome 4", "Cognome 4");

            //l2.Autori.Add(a3);
            //l2.Autori.Add(a4);

            ////l2.Scaffale = s2;
            ////b.Documenti.Add(l2);
            //#endregion

            //#region "DVD"
            //DVD dvd1 = new DVD("Codice1", "Titolo 3", 2019, "Storia", 130);

            //dvd1.Autori.Add(a3);

            ////dvd1.Scaffale = s3;
            ////b.Documenti.Add(dvd1);
            //#endregion

            //Utente u1 = new Utente("Nome 1", "Cognome 1", "Telefono 1", "Email 1", "Password 1");

            ////b.Utenti.Add(u1);

            //Prestito p1 = new Prestito("P00001", new DateTime(2019, 1, 20), new DateTime(2019, 2, 20), u1, l1);
            //Prestito p2 = new Prestito("P00002", new DateTime(2019, 3, 20), new DateTime(2019, 4, 20), u1, l2);

            ////b.Prestiti.Add(p1);
            ////b.Prestiti.Add(p2);

            //Console.WriteLine("\n\nSearchByCodice: ISBN1\n\n");

            //List<Documento> results = b.SearchByCodice("ISBN1");

            //foreach (Documento doc in results)
            //{
            //    Console.WriteLine(doc.ToString());

            //    if (doc.Autori.Count > 0)
            //    {
            //        Console.WriteLine("--------------------------");
            //        Console.WriteLine("Autori");
            //        Console.WriteLine("--------------------------");
            //        foreach (Autore a in doc.Autori)
            //        {
            //            Console.WriteLine(a.ToString());
            //            Console.WriteLine("--------------------------");
            //        }
            //    }
            //}

            //Console.WriteLine("\n\nSearchPrestiti: Nome 1, Cognome 1\n\n");

            //List<Prestito> prestiti = b.SearchPrestiti("Nome 1", "Cognome 1");

            //foreach (Prestito p in prestiti)
            //{
            //    Console.WriteLine(p.ToString());
            //    Console.WriteLine("--------------------------");
            //}
        }
    }
}