﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{
    internal class DVD : Documento
    {
        public int Durata { get; set; }

        public DVD(long Codice, string Titolo, string Settore,string Scaffale, int Durata) : base(Codice, Titolo, Settore, Scaffale)
        {
            this.Durata = Durata;

        }

        public override string ToString()
        {
            return string.Format("{0}\nDurata:{1}",
                base.ToString(),
                this.Durata);
        }
    }
}
