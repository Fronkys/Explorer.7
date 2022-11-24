using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drives
{

    internal class Menu
    {
        
        private int selectedIndex = 0;
        private List<Fso> items;

        public void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine();
            items = Explorer.Dir();
            for (int i = 0; i < items.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.Write("->");
                }
                Console.WriteLine("\t" + items[i].Name);
            }
        }

        public (string, bool) MainLoop()
        {
            while (true)
            {
                PrintMenu();
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.DownArrow)
                {
                    Down();
                }
                if (key.Key == ConsoleKey.UpArrow)
                {
                    Up();
                }
                if (key.Key == ConsoleKey.Escape)
                {

                    items = Explorer.Up();

                }
                if (key.Key == ConsoleKey.Enter)
                {
                    var selected = items[selectedIndex];
                    if (selected.FsoType == FsoType.Folder || selected.FsoType == FsoType.Drive)
                    {
                        items = Explorer.Cd(selected.Name);
                    }
                    if (selected.FsoType == FsoType.File)
                    {
                        Explorer.OpenWithDefaultProgram(selected.FullFilename);
                    }


                }
                if (key.Key == ConsoleKey.F1)
                {
                    var selected = items[selectedIndex];
                    if (selected.FsoType == FsoType.File)
                    {
                        File.Delete(selected.FullFilename);

                    }
                    if (selected.FsoType == FsoType.Folder)
                    {
                        Directory.Delete(selected.FullFilename, false);
                    }
                    items = Explorer.Dir();
                }
                if (key.Key == ConsoleKey.F2)    
                {
                    var selected = items[selectedIndex];
                    
                    if (selected.FsoType == FsoType.File)
                    {
                        string path1 = "C:\\" + Console.ReadLine();

                        if (!File.Exists(path1))
                        {
                            File.Create(path1);
                        }
                    }
                    
                    if (selected.FsoType == FsoType.Folder)
                    {
                        string path2 = "C:\\" + Console.ReadLine();
                        if (!Directory.Exists(path2))
                        {
                            Directory.CreateDirectory(path2);
                        }
                    }
                    items = Explorer.Dir();
                }

            }
        }



        private void Down()
        {
            selectedIndex = (selectedIndex + 1) % items.Count;
        }
        private void Up()
        {
            if (selectedIndex == 0)
            {
                selectedIndex = items.Count;
            }
            selectedIndex--;
        }

    }
}
