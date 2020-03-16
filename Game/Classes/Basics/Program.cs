using System;
using System.Windows.Forms;

/*
 PRACA INŻYNIERSKA - KAROL KULESZA
 Temat: Realizacja dwuwymiarowej gry platformowej z użyciem biblioteki SFML
 Promotor: dr Piotr Jastrzębski


PROPOZYCJE DO ZROBIENIA:
- generowanie poziomu***
- jak sie uda to shadery/swiatlo ogarnac aby ladnie wygladalo**
- po smierci resp kilka sekund przed*

DO ZROBIENIA:
- levele
- scenka na poczatek i koniec
- async

*/

namespace ChendiAdventures
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                MainGameWindow.Instance.GameStart();
            }
            catch (Exception e)
            {
                string message = $"ERROR: {e.Message}\nSource: {e.Source}\n"; 
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MainGameWindow.Instance.Close();
            }
        }
    }
}