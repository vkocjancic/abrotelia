using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrotelia.DbToJson
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: db2json <path_to_litedb_file> <output_path>");
                Environment.Exit(0);
            }
            var dirOutput = args[1];
            if (!Directory.Exists(dirOutput))
            {
                Directory.CreateDirectory(dirOutput);
            }
            using(var db = new LiteDatabase(args[0]))
            {
                try
                {
                    var collections = db.GetCollectionNames();
                    Console.WriteLine($"Found {collections.Count()} collections");
                    foreach(var collection in collections)
                    {
                        Console.WriteLine($"Exporting collection '{collection}'");
                        var json = JsonSerializer.Serialize(new BsonArray(db.GetCollection(collection).FindAll()));
                        File.WriteAllText(Path.Combine(dirOutput, $"{collection}.json"), json);
                    }
                    Console.WriteLine("Done");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Napaka:\n{ex.Message}\n{ex.StackTrace}\n\n");
                }
            }
        }
    }
}
