using Labb3.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Labb3.Data
{
    internal static class JSON
    {
        private static string appFolder = Path.Combine(
         Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
         "Labb3");

        private static string jsonFilePath = Path.Combine(appFolder, "questions.json");

        private static void FolderExist()
        {
            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }
        }

        public static void SaveQuestionPacks(List<QuestionPack> packs)
        {
            FolderExist();

           
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() } 
            };

            string jsonString = JsonSerializer.Serialize(packs, options); 
            File.WriteAllText(jsonFilePath, jsonString);
        }

        public static List<QuestionPack> LoadQuestionPacks()
        {
          
            FolderExist();

            if (!File.Exists(jsonFilePath))
            {
                return new List<QuestionPack>();
            }


            string jsonString = File.ReadAllText(jsonFilePath);

            
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

           
            List<QuestionPack> packs = JsonSerializer.Deserialize<List<QuestionPack>>(jsonString, options);

            return packs ?? new List<QuestionPack>();

        }




    }
}
