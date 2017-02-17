using Abrotelia.Core.Data.Persistence;
using Abrotelia.JsonToDb.Extensions;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abrotelia.JsonToDb
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: json2db <path_of_litedb_file> <import_path>");
                Environment.Exit(0);
            }
            var dirImport = args[1];
            if (!Directory.Exists(dirImport))
            {
                Console.WriteLine($"Directory '{dirImport}' does not exist");
                Environment.Exit(0);
            }
            using (var db = new LiteDatabase(args[0]))
            {
                try
                {
                    foreach (var file in Directory.GetFiles(dirImport, "*.json"))
                    {
                        var collection = Path.GetFileNameWithoutExtension(file);
                        var json = File.ReadAllText(file);
                        foreach (var value in JsonSerializer.Deserialize(json).AsArray.ToArray())
                        {
                            var document = value.AsDocument;
                            if ("authors" == collection)
                            {
                                db.GetCollection<PMAuthor>(collection).Insert(document.ToPMAuthor());
                            }
                            else if ("galleryItems" == collection)
                            {
                                db.GetCollection<PMGalleryItem>(collection).Insert(document.ToPMGalleryItem());
                            }
                            else if ("pages" == collection)
                            {
                                db.GetCollection<PMPage>(collection).Insert(document.ToPMPage());
                            }
                            else
                            {
                                db.GetCollection(collection).Insert(document);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Napaka:\n{ex.Message}\n{ex.StackTrace}\n\n");
                }
            } 
        }
    }
}
