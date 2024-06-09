using System;
using System.Text.RegularExpressions;

namespace NeuroStrugachi
{
    public static class BookAdder
    {
        public static void AddBooks(IEnumerable<string> names, string glavDivider, int startIndexInFolder)
        {
            var tasks = new List<Task<string>>();
            foreach (var e in names)
            {
                tasks.Add(File.ReadAllTextAsync(e + ".txt"));
            }

            var glTasks = new List<Task<string>>();
            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].Wait();
                var glavy = tasks[i].Result.Split(glavDivider);
                for (int j = 1; j < glavy.Length; j++)
                {
                    string reg = "(</[a-zA-Z\\-]*>|<[a-zA-Z\\-]*>|<[a-zA-Z\\-]*/>)";
                    var abzac = glavy[j].Split("<p>");
                    for (int k = 0; k < abzac.Length; k++)
                    {
                        abzac[k] = Regex.Replace(abzac[k], reg, "");
                    }

                    File.WriteAllLines("glavs/" + (i+startIndexInFolder) + "_" + j + ".txt", abzac);
                }

            }
        }
    }

    class Program
    {
        
        public static void Main()
        {
            string[] books = { "lebediveshipoputka" };
            BookAdder.AddBooks(books, "<title>", 2);

        }
    }
}
