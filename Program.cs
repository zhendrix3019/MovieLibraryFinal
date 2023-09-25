using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace MovieLibrary
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "movies.csv");
        private static DataTable movieData = new DataTable();
        private static int maxMovieID = int.MinValue;

        static void Main(string[] args)
        {
            logger.Info("Program started");

            while (true)
            {
                switch (DisplayMenuAndGetChoice())
                {
                    case "1":
                        LoadDataFromFile();
                        break;
                    case "2":
                        AddMovie();
                        break;
                    default:
                        logger.Info("Program ended");
                        return;
                }
            }
        }

        static string DisplayMenuAndGetChoice()
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1) Display movies.");
            Console.WriteLine("2) Add a new movie.");
            Console.WriteLine("Any other key to exit.");
            return Console.ReadLine();
        }

        static void LoadDataFromFile()
        {
            if (!File.Exists(file))
            {
                logger.Error("File does not exist: {File}", file);
                Console.WriteLine("Movies file does not exist.");
                return;
            }

            try
            {
                movieData.Clear();
                using (StreamReader sr = new StreamReader(file))
                {
                    // Logic to read from the CSV and populate the DataTable
                }

                DisplayMovies();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        static void DisplayMovies()
        {
            foreach (DataRow row in movieData.Rows)
            {
                // Logic to display each movie, similar to your original code
            }
        }

        static void AddMovie()
        {
            string title = GetMovieTitleFromUser();
            string genre = GetMovieGenreFromUser();
            AddMovieToData(title, genre);
            SaveDataToFile();
        }

        static string GetMovieTitleFromUser()
        {
            Console.Write("Enter movie title: ");
            return Console.ReadLine().Trim();
        }

        static string GetMovieGenreFromUser()
        {
            Console.Write("Enter movie genre: ");
            return Console.ReadLine().Trim();
        }

        static void AddMovieToData(string title, string genre)
        {
            // Logic to add the new movie to the DataTable
        }

        static void SaveDataToFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    // Logic to write the DataTable contents back to the CSV
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
