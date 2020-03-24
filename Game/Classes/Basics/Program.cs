using System;
using System.Windows.Forms;

/*
 PRACA INŻYNIERSKA - KAROL KULESZA
 Temat: Realizacja dwuwymiarowej gry platformowej z użyciem biblioteki SFML
 Promotor: dr Piotr Jastrzębski

PROPOZYCJE DO ZROBIENIA:
- generowanie poziomu***
- jak sie uda to shadery/swiatlo ogarnac aby ladnie wygladalo**

DO ZROBIENIA:
- levele
- scenka na poczatek i koniec
- async
- nerf ilosci manasow albo ogolnie nerf manasow

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
                string message = $"ERROR: {e.Message}\nSource: {e.Source}\nStack:\n{e.StackTrace}\nIt is advised to contact developer."; 
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MainGameWindow.Instance.Close();
            }
        }
    }
}