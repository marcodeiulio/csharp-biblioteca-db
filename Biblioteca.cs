using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{
    internal class Biblioteca
    {
        public string Nome { get; set; }
        public List<Scaffale> ScaffaliBiblioteca { get; set; }
       

        //public Dictionary<string,Utente> MioDizionario;


    public Biblioteca(string Nome)
        {
            this.Nome = Nome;
            this.ScaffaliBiblioteca = new List<Scaffale>();

            // recupera l'elenco degli scaffali
            List<string> elencoScaffali = DB.scaffaliGet();
            elencoScaffali.ForEach(item =>
            {
                //Scaffale s1 = new Scaffale(item);
                //this.ScaffaliBiblioteca.Add(s1);
                AggiungiScaffale(item, false);
            });
        }

        public void AggiungiScaffale(string NomeScaffale, bool addToDb=true)
        {
            Scaffale s1 = new Scaffale(NomeScaffale);
            ScaffaliBiblioteca.Add(s1);
            DB.scaffaleAdd(s1.Numero);
        }


        

        public void AggiungiLibro(long codice, string titolo, string settore,int numPagine,string scaffale, List<Autore> listaAutori)
        {
            Libro mioLibro = new Libro(codice, titolo, settore, numPagine, scaffale);
            mioLibro.Stato = Stato.Disponibile;

            DB.libroAdd(mioLibro,listaAutori);
        }



        public int GestisciOperazioniBiblioteca(int iCodiceOperazione)
        {
            List<Documento> lResult;
            string sAppo;
            switch (iCodiceOperazione)
            {
                case 1:
                    Console.WriteLine("Inserisci Autore:");
                    sAppo = Console.ReadLine();
                    lResult = SearchByAutore(sAppo);
                    if (lResult == null)
                    {
                        return 1;
                    }
                    else
                    {
                        //StampaListaDocumenti();
                        return 0;
                    }
                    break;
            }
            return 0;
        }

        public void StampaListaDocumenti(List<Documento> lListaDoc)
        {
            return;
        }

        public List<Documento> SearchByCodice(string Codice)
        {
            Console.WriteLine("Metodo da implementare");
            return null;
        }

        public List<Documento> SearchByTitolo(string Titolo)
        {
            Console.WriteLine("Metodo da implementare");
            /*
             * Implementare: SELECT Titolo,Settore,Stato,Tipo 
             * FROM Documenti, Autori_Documenti
             * INNER JOIN Autori_Documenti
             * ON Docuemnti.codice_documento = Autori_Documenti.codice
             *
             */
            return null;
        }

        public List<Documento> SearchByAutore(string Titolo)
        {
            Console.WriteLine("Metodo da implementare");
            return null;
        }

        public List<Prestito> SearchPrestiti(string Numero)
        {
            Console.WriteLine("Metodo da implementare");
            return null;
        }

        public List<Prestito> SearchPrestiti(string Nome, string Cognome)
        {
            Console.WriteLine("Metodo da implementare");
            return null;
        }
    }
}
