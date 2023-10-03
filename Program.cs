using System;
using System.Data;
using System.IO;
using System.Linq;
using NLog;

namespace MovieLibraryFinal
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly string file = "movies.csv";
        private static DataTable movieData = new DataTable();
        private static int maxMovieID = int.MinValue;

        static void Main(string[] args)
        {
            logger.Info("Program started");

            if (File.Exists(file))
            {
                // Load data from the existing movies.csv file
                LoadDataFromFile();
            }
            else
            {
                logger.Error("The specified file does not exist: {File}", file);
                // Handle the scenario when the file doesn't exist
            }

            while (true)
            {
                switch (DisplayMenuAndGetChoice())
                {
                    case "1":
                        DisplayMovies();
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
            try
            {
                movieData.Clear();
                movieData = new DataTable();
                movieData.Columns.Add("movieId");
                movieData.Columns.Add("title");
                movieData.Columns.Add("genre");

                using (StreamReader sr = new StreamReader(file))
                {
                    sr.ReadLine(); // Skip header row
                    while (!sr.EndOfStream)
                    {
                        string[] movieDetails = sr.ReadLine().Split(',');
                        movieData.Rows.Add(movieDetails);
                    }
                }
                logger.Info("Data loaded from movies.csv");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error loading movies data from file.");
            }
        }

        static void DisplayMovies()
        {
            Console.WriteLine("\nList of Movies:");
            foreach (DataRow row in movieData.Rows)
            {
                Console.WriteLine($"ID: {row["movieId"]}, Title: {row["title"]}, Genre: {row["genre"]}");
            }
        }

        static void AddMovie()
        {
            Console.WriteLine("\nEnter the movie title:");
            string title = Console.ReadLine().Trim();

            if (movieData.AsEnumerable().Any(row => row.Field<string>("title").Equals(title, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("This movie already exists in the library.");
                return;
            }

            Console.WriteLine("Enter the genre:");
            string genre = Console.ReadLine().Trim();

            maxMovieID = movieData.AsEnumerable().Max(row => Convert.ToInt32(row.Field<string>("movieId")));
            int newMovieID = maxMovieID + 1;

            movieData.Rows.Add(newMovieID.ToString(), title, genre);

            SaveDataToFile();
        }

        static void SaveDataToFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(file, false))
                {
                    sw.WriteLine("movieId,title,genre");

                    foreach (DataRow row in movieData.Rows)
                    {
                        sw.WriteLine($"{row["movieId"]},{row["title"]},{row["genre"]}");
                    }
                }

                Console.WriteLine("New movie added successfully!");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error writing new movie data to file.");
            }
        }
    }
}
