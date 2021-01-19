using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JsonMethods
{
    /// <summary>
    /// The main <see langword="class"/> that processes Json files locally.
    /// </summary>
    public static class JsonFileProcessor
    {
        /// <summary>
        /// Saves a Json file to a folder.
        /// </summary>
        /// <remarks>
        /// The method will check if the ".json" suffix exists in the <paramref name="fileName"/> parameter.<br/>
        /// If it doesn't exist, it will be added.
        /// </remarks>
        /// <param name="path">The folder path where the file will be saved.</param>
        /// <param name="fileName">The name of the file without its suffix.</param>
        /// <param name="jsonInput">The <see cref="string"/> data that will be written.</param>
        public static void SaveJsonToFolder(string path, string fileName, string jsonInput)
        {
            if (Directory.Exists(path))
            {
                string fileNameFinal = fileName.Contains(".json")
                                        ? fileNameFinal = fileName
                                        : fileNameFinal = String.Format("{0}.json", fileName);

                string pathFinal = Path.Combine(path, fileNameFinal);
                File.WriteAllText(pathFinal, jsonInput);

                return;
            }

            Console.WriteLine("Invalid path.");
        }

        /// <summary>
        /// Loads a Json file from a folder.
        /// </summary>
        /// <remarks>
        /// The method will check if the ".json" suffix exists in the <paramref name="fileName"/> parameter.<br/>
        /// If it doesn't exist, it will be added.
        /// </remarks>
        /// <param name="path">The folder path where the file is stored.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>A <see cref="string"/> object when path exists or <see langword="null"/> when it doesn't.</returns>
        public static string LoadJsonFromFolder(string path, string fileName)
        {
            if (Directory.Exists(path))
            {
                string fileNameFinal = fileName.Contains(".json")
                                        ? fileNameFinal = fileName
                                        : fileNameFinal = String.Format("{0}.json", fileName);

                using (StreamReader reader = new StreamReader(path + @"\" + fileNameFinal))
                {
                    return reader.ReadToEnd();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Combines two <see cref="JArray"/> objects of the same length to a <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <remarks>
        /// The method will create a <see cref="Dictionary{TKey, TValue}"/> of item <paramref name="names"/> and their <paramref name="ids"/>.
        /// </remarks>
        /// <param name="names">The <see cref="JArray"/> whose values will be used as keys.</param>
        /// <param name="ids">The <see cref="JArray"/> whose values will be used as values.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> with <see cref="string"/> keys and <see cref="int"/> values.</returns>
        public static Dictionary<string, int> CombineJArraysToDictionary(JArray names, JArray ids)
        {
            if (!JArraysAreEqual(names, ids))
            {
                return null;
            }

            Dictionary<string, int> nameIDPairs = new Dictionary<string, int>();
            
            for (int i = 0; i < names.Count; i++)
            {
                nameIDPairs.Add(names[i].ToString(), (int)ids[i]);
            }

            return nameIDPairs;
        }

        /// <summary>
        /// Combines two <see cref="JArray"/> objects of the same length to a <see cref="Dictionary{TKey, TValue}"/> of <see cref="List{T}}"/>, where T is an <see cref="int"/>.
        /// </summary>
        /// <remarks>
        /// The method will create a <see cref="Dictionary{TKey, TValue}"/> of item <paramref name="names"/> and a of their <paramref name="ids"/>.<br/>
        /// It can be used when a TKey has multiple TValues.
        /// </remarks>
        /// <param name="names">The <see cref="JArray"/> whose values will be used as keys.</param>
        /// <param name="ids">The <see cref="JArray"/> whose values will be used as values.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> with <see cref="string"/> keys and <see cref="List{T}}"/> values, where T is an <see cref="int"/>.</returns>
        public static Dictionary<string, List<int>> CombineJArraysToDictionaryOfLists(JArray names, JArray ids)
        {
            if (!JArraysAreEqual(names, ids))
            {
                return null;
            }

            Dictionary<string, List<int>> nameIDPairs = new Dictionary<string, List<int>>();

            for (int i = 0; i < names.Count; i++)
            {
                if (nameIDPairs.ContainsKey(names[i].ToString()))
                {
                    nameIDPairs[names[i].ToString()].Add((int)ids[i]);
                }
                else
                {
                    nameIDPairs.Add(names[i].ToString(), new List<int> { (int)ids[i] });
                }
            }

            return nameIDPairs;
        }

        /// <summary>
        /// Compares the length of two <see cref="JArray"/> objects.
        /// </summary>
        /// <param name="firstJArray">The first <see cref="JArray"/>.</param>
        /// <param name="secondJArray">The second <see cref="JArray"/>.</param>
        /// <returns><see langword="true"/> or <see langword="false"/></returns>
        public static bool JArraysAreEqual(JArray firstJArray, JArray secondJArray)
        {
            if (firstJArray.Count == secondJArray.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reads a <see cref="JObject"/> and finds a <see cref="JProperty"/> in it.
        /// </summary>
        /// <remarks>
        /// Ignores casing of the <paramref name="propertyName"/>.
        /// </remarks>
        /// <param name="jObject">The <see cref="JObject"/> that we apply our search to.</param>
        /// <param name="propertyName">The name of the <see cref="JProperty"/> we are looking for in a <see cref="string"/> format.</param>
        /// <returns>The value of the <see cref="JProperty"/> in a <see cref="string"/> format, if we find it, or "".</returns>
        public static string FindJProperty(JObject jObject, string propertyName)
        {
            var properties = jObject.Descendants()
                .OfType<JProperty>()
                .Where(p => p.Name.ToLower() == propertyName.ToLower())
                .Take(1)
                .ToList();

            if (properties != null)
            {
                return properties[0].Value.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}