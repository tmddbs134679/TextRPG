using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TextRPG
{
    internal class SaveManager
    {
        private static readonly string path_ = "player.json";


        public static void Save(Player player, string path = null)
        {
            if (path == null)
                path = path_;

            var op = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };

            string json = JsonSerializer.Serialize(player, op);
            File.WriteAllText(path, json);
        }


        public static Player Load(string path = null)
        {
            if (path == null)
                path = path_;

            if (!File.Exists(path_))
            {
                return null;
            }

            string json = File.ReadAllText(path);

            Player player = JsonSerializer.Deserialize<Player>(json);

            if(player?.Status != null)
            {
                player.Status.SetOwner(player);
            }

            return player;
        }

        public static void Reset(string path = null)
        {
            if (path == null)
                path = path_;


            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static bool Exists(string path = null)
        {
            if (path == null)
                path = path_;

            return File.Exists(path);
        }

        public static void ExitGame()
        {
            Console.WriteLine("");

            Console.ReadLine();
        }

    }
}
