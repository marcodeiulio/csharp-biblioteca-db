using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{
    internal class Libro : Documento
    {
        public int NumeroPagine { get; set; }


        public Libro(long Codice, string Titolo, string Settore, int NumeroPagine, string Scaffale) : base(Codice, Titolo, Settore,Scaffale)
        {
            this.NumeroPagine = NumeroPagine;
            

        }

        public override string ToString()
        {
            return string.Format("{0}\nNumeroPagine:{1}",
                base.ToString(),
                this.NumeroPagine);
        }
    }
}
