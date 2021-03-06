using System.Data.SqlClient;

namespace csharp_biblioteca_db
{
    internal class DB
    {
        private static string stringaDiConnessione = "Data Source=localhost;Initial Catalog=biblioteca;Integrated Security=True;Pooling=False";

        private static SqlConnection Connect()
        {
            SqlConnection conn = new SqlConnection(stringaDiConnessione);
            try
            {
                conn.Open();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return conn;

        }

        internal static long GetUniqueId()
        {
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to Database");
            }

            string query = "UPDATE codiceunico SET codice = codice + 1 OUTPUT INSERTED.codice";
            long id;

            using (SqlCommand select = new SqlCommand(query, conn))
            {
                using (SqlDataReader reader = select.ExecuteReader())
                {
                    reader.Read();
                    id = reader.GetInt64(0);
                }
            }
            conn.Close();
            return id;
        }

        internal static bool DoSql(SqlConnection conn, string sql)
        {

            using (SqlCommand sqlCmd = new SqlCommand(sql, conn))
            {
                try
                {

                    sqlCmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }

            }

        }

        internal static int DvdAdd(DVD mioDvd, List<Autore> listaAutori)
        {
            //Devo collegarmi e inviare un comando per inserire uno scaffale
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to Database");
            }

            var ok = DoSql(conn, "begin transaction \n");
            if (!ok)
                throw new Exception("Errore in begin transaction ");

            //Inserisco lo scaffale nella tabella scaffali
            var cmd = String.Format("insert into Documenti (codice,Titolo,Settore,Stato,Tipo,Scaffale) values ('{0}', '{1}', '{2}', '{3}', 'DVD', '{4}')",
                mioDvd.Codice, mioDvd.Titolo, mioDvd.Settore, mioDvd.Stato.ToString(), mioDvd.Scaffale.Numero);

            using (SqlCommand insert = new SqlCommand(cmd, conn))
            {
                try
                {

                    var numrows = insert.ExecuteNonQuery();
                    if (numrows != 1)
                    {
                        conn.Close();
                        throw new Exception("Valore di ritorno errato prima query");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    DoSql(conn, "rollback transaction");
                    conn.Close();
                    return 0;
                }

            }

            var cmd2 = String.Format("insert into Dvd(codice, Durata) values ('{0}', '{1}')",
                mioDvd.Codice, mioDvd.Durata);

            using (SqlCommand insert = new SqlCommand(cmd2, conn))
            {
                try
                {

                    var numrows = insert.ExecuteNonQuery();
                    if (numrows != 1)
                    {
                        DoSql(conn, "rollback transaction");
                        conn.Close();
                        throw new Exception("Valore di ritorno errato seconda query");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    DoSql(conn, "rollback transaction");
                    conn.Close();
                    return 0;
                }

            }

            // VERIFICO SE L'AUTORE ESISTE
            string cmd4;
            foreach (Autore autore in listaAutori)
            {
                long uCodiceAutore = 0;
                cmd4 = String.Format("select codice from Autori where Nome='{0}' and Cognome='{0}' and email='{0}';", autore.Nome, autore.Cognome, autore.email);
                using (SqlCommand query = new SqlCommand(cmd4, conn))
                {
                    using (SqlDataReader reader = query.ExecuteReader())
                    {
                        Console.WriteLine(reader.FieldCount);
                        while (reader.Read())
                        {

                            uCodiceAutore = reader.GetInt64(0);
                        }
                    }
                }
            }

            string cmdAutori;
            foreach (Autore autore in listaAutori)
            {
                long lCodiceAutore = 0;
                int iINsertFlag = 0;
                cmdAutori = String.Format("select codice from Autori where Nome='{0}' and Cognome='{1}' and email='{2}'", autore.Nome, autore.Cognome, autore.email);
                using (SqlCommand select = new SqlCommand(cmdAutori, conn))
                {
                    using (SqlDataReader reader = select.ExecuteReader())
                    {
                        Console.WriteLine(reader.FieldCount);
                        if (reader.Read())
                        {
                            lCodiceAutore = reader.GetInt64(0);
                        }
                        else
                        {
                            lCodiceAutore = autore.CodiceAutore;
                            iINsertFlag = 1;
                        }
                        reader.Close();

                    }

                }

                if (iINsertFlag == 1)
                {
                    string cmd5 = string.Format("INSERT INTO Autori(codice,Nome, Cognome, email) values ('{0}', '{1}','{2}','{3}')",
                                    autore.CodiceAutore, autore.Nome, autore.Cognome, autore.email);
                    using (SqlCommand insert = new SqlCommand(cmd5, conn))
                    {
                        try
                        {
                            var numrows = insert.ExecuteNonQuery();
                            if (numrows != 1)
                            {
                                DoSql(conn, "rollback transaction");
                                conn.Close();
                                throw new System.Exception("Valore di ritorno errato seconda query");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            DoSql(conn, "rollback transaction");
                            conn.Close();
                            return 0;
                        }
                    }

                }
                string cmd3 = string.Format(@"INSERT INTO Autori_Documenti(codice_autore, codice_documento) values({0},{1})", lCodiceAutore, mioDvd.Codice);
                using (SqlCommand insert = new SqlCommand(cmd3, conn))
                {
                    try
                    {
                        var numrows = insert.ExecuteNonQuery();
                        if (numrows != 1)
                        {
                            DoSql(conn, "rollback transaction");
                            conn.Close();
                            throw new System.Exception("Valore di ritorno errato seconda query");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        DoSql(conn, "rollback transaction");
                        conn.Close();
                        return 0;
                    }
                }
            }

            DoSql(conn, "commit transaction");
            conn.Close();
            return 0;


        }

        // METODO PER AGGIUNGERE UN LIBRO
        internal static int libroAdd(Libro libro, List<Autore> listaAutori)
        {

            //Devo collegarmi e inviare un comando per inserire uno scaffale
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to Database");
            }

            var ok = DoSql(conn, "begin transaction \n");
            if (!ok)
                throw new Exception("Errore in begin transaction ");


            //Inserisco lo scaffale nella tabella scaffali
            var cmd = String.Format("insert into Documenti (codice,Titolo,Settore,Stato,Tipo,Scaffale) values ('{0}', '{1}', '{2}', '{3}', 'Libro', '{4}')",
                libro.Codice, libro.Titolo, libro.Settore, libro.Stato.ToString(), libro.Scaffale.Numero);

            using (SqlCommand insert = new SqlCommand(cmd, conn))
            {
                try
                {

                    var numrows = insert.ExecuteNonQuery();
                    if (numrows != 1)
                    {
                        conn.Close();
                        throw new Exception("Valore di ritorno errato prima query");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    DoSql(conn, "rollback transaction");
                    conn.Close();
                    return 0;
                }

            }


            var cmd2 = String.Format("insert into Libri(codice, NumPagine) values ('{0}', '{1}')",
                libro.Codice, libro.NumeroPagine);

            using (SqlCommand insert = new SqlCommand(cmd2, conn))
            {
                try
                {

                    var numrows = insert.ExecuteNonQuery();
                    if (numrows != 1)
                    {
                        DoSql(conn, "rollback transaction");
                        conn.Close();
                        throw new Exception("Valore di ritorno errato seconda query");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    DoSql(conn, "rollback transaction");
                    conn.Close();
                    return 0;
                }

            }

            // VERIFICO SE L'AUTORE ESISTE
            string cmd4;
            foreach (Autore autore in listaAutori)
            {
                long uCodiceAutore = 0;
                cmd4 = String.Format("select codice from Autori where Nome='{0}' and Cognome='{0}' and email='{0}';", autore.Nome, autore.Cognome, autore.email);
                using (SqlCommand query = new SqlCommand(cmd4, conn))
                {
                    using (SqlDataReader reader = query.ExecuteReader())
                    {
                        Console.WriteLine(reader.FieldCount);
                        while (reader.Read())
                        {

                            uCodiceAutore = reader.GetInt64(0);
                        }
                    }
                }
            }

            string cmdAutori;
            foreach (Autore autore in listaAutori)
            {
                long lCodiceAutore = 0;
                int iINsertFlag = 0;
                cmdAutori = String.Format("select codice from Autori where Nome='{0}' and Cognome='{1}' and email='{2}'", autore.Nome, autore.Cognome, autore.email);
                using (SqlCommand select = new SqlCommand(cmdAutori, conn))
                {
                    using (SqlDataReader reader = select.ExecuteReader())
                    {
                        Console.WriteLine(reader.FieldCount);
                        if (reader.Read())
                        {
                            lCodiceAutore = reader.GetInt64(0);
                        }
                        else
                        {
                            lCodiceAutore = autore.CodiceAutore;
                            iINsertFlag = 1;
                        }
                        reader.Close();

                    }

                }

                if (iINsertFlag == 1)
                {
                    string cmd5 = string.Format("INSERT INTO Autori(codice,Nome, Cognome, email) values ('{0}', '{1}','{2}','{3}')",
                                    autore.CodiceAutore, autore.Nome, autore.Cognome, autore.email);
                    using (SqlCommand insert = new SqlCommand(cmd5, conn))
                    {
                        try
                        {
                            var numrows = insert.ExecuteNonQuery();
                            if (numrows != 1)
                            {
                                DoSql(conn, "rollback transaction");
                                conn.Close();
                                throw new System.Exception("Valore di ritorno errato seconda query");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            DoSql(conn, "rollback transaction");
                            conn.Close();
                            return 0;
                        }
                    }

                }
                string cmd3 = string.Format(@"INSERT INTO Autori_Documenti(codice_autore, codice_documento) values({0},{1})", lCodiceAutore, libro.Codice);
                using (SqlCommand insert = new SqlCommand(cmd3, conn))
                {
                    try
                    {
                        var numrows = insert.ExecuteNonQuery();
                        if (numrows != 1)
                        {
                            DoSql(conn, "rollback transaction");
                            conn.Close();
                            throw new System.Exception("Valore di ritorno errato seconda query");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        DoSql(conn, "rollback transaction");
                        conn.Close();
                        return 0;
                    }
                }
            }

            DoSql(conn, "commit transaction");
            conn.Close();
            return 0;

        }

        // METODO PER AGGIUNGERE SCAFFALE
        internal static int scaffaleAdd(string s1)
        {

            //Devo collegarmi e inviare un comando per inserire uno scaffale
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to Database");
            }

            //Inserisco lo scaffale nella tabella scaffali
            var cmd = String.Format("insert into Scaffale (scaffale) values ('{0}')", s1);

            using (SqlCommand insert = new SqlCommand(cmd, conn))
            {
                try
                {

                    var numrows = insert.ExecuteNonQuery();
                    return numrows;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        // METODO PER LEGGERE LA LISTA DEGLI SCAFFALI
        internal static List<string> scaffaliGet()
        {
            List<string> ls = new List<string>();

            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to Database");
            }

            //Inserisco lo scaffale nella tabella scaffali
            var cmd = String.Format("select scaffale from Scaffale");

            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
                using (SqlDataReader reader = select.ExecuteReader())
                {
                    Console.WriteLine(reader.FieldCount);
                    while (reader.Read())
                    {
                        ls.Add(reader.GetString(0));
                    }
                }
            }
            conn.Close();

            return ls;
        }

        internal static List<Tuple<int, string, string, string, string, string>> documentiGet()
        {
            var ld = new List<Tuple<int, string, string, string, string, string>>();
            var conn = Connect();
            if (conn == null)
                throw new Exception("Unable to connect to the dabatase");
            var cmd = String.Format("select codice, Titolo, Settore, Stato, Tipo, Scaffale from Documenti");  //Li prendo tutti
            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
                using (SqlDataReader reader = select.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var data = new Tuple<Int32, string, string, string, string, string>(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5));
                        ld.Add(data);
                    }
                }
            }
            conn.Close();
            return ld;
        }
        // Implementa un metodo per leggere tutti i documenti dal DB, dell'autore passando nome e cognome
        internal static void GetAllDocumentByAuthor(string nome, string cognome)
        {
            var listDoc = new List<Tuple<string, string, string, string, string>>();

            var conn = Connect();
            if (conn == null)
                throw new Exception("Unable to connect to the dabatase");

            var sel = String.Format("SELECT Titolo, Settore, Stato, Tipo, Scaffale FROM Documenti " +
                "inner join Autori_Documenti on Documenti.codice = Autori_Documenti.codice_documento " +
                "inner join Autori on Autori_Documenti.codice_autore = Autori.codice " +
                "WHERE Autori.Nome = '{0}' AND Autori.Cognome = '{1}'", nome, cognome);

            using (SqlCommand select = new SqlCommand(sel, conn))
            {
                using (SqlDataReader reader = select.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var data = new Tuple<string, string, string, string, string>(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4));

                        listDoc.Add(data);
                    }
                }
            }
            conn.Close();

            foreach (Tuple<string, string, string, string, string> t in listDoc)
            {
                Console.WriteLine(t.ToString());
            }

        }
    }
}