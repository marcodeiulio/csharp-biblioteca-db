using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{
    internal class Autore : Persona
    {
        public long CodiceAutore;
        public string email;
        public Autore(string Nome, string Cognome, string Email) : base(Nome, Cognome)
        {
            this.email = Email;
            this.CodiceAutore = GeneraCodiceAutore();

        }

        public long GeneraCodiceAutore()
        {
            return DB.GetUniqueId();
        }
    }
}
