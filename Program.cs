using System;
using System.Data;
using System.IO;
using System.Linq;
using NLog;

namespace MovieLibrary
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        string movieFilePath = Directory.GetCurrentDirectory() + "\\movies.csv";
        private static DataTable movieData = new DataTable();

        static void Main(string[] args)
        {
            logger.Info("Program started");

            if (File.Exists("\\movies.csv"))
            {
                LoadDataFromFile();
            }
            else
            {
                logger.Error("The specified file does not exist: {File}", "\\movies.csv");
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

       private static void DisplayMovies()
{
    if (movieData == null || movieData.Rows.Count == 0)
    {
        Console.WriteLine("No movie data available to display.");
        logger.Info("Attempted to display movies, but no data was found.");
        return;
    }

    Console.WriteLine("\nList of Movies:");
    Console.WriteLine("{0,-10} {1,-50} {2,-50}", "Movie ID", "Title", "Genre");

    foreach (DataRow row in movieData.Rows)
    {
        Console.WriteLine("{0,-10} {1,-50} {2,-50}", 
                          row["movieId"], row["title"], row["genre"]);
    }

    logger.Info("Movies displayed successfully.");
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

                using (StreamReader sr = new StreamReader("\\movies.csv"))
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

        static void AddMovie()
{
    if (movieData == null || movieData.Rows.Count == 0)
    {
        Console.WriteLine("Movie data is not loaded. Cannot add a new movie.");
        return;
    }

    Console.WriteLine("\nEnter the movie ID:");
    if (!int.TryParse(Console.ReadLine().Trim(), out int movieId) || 
        movieData.AsEnumerable().Any(row => row.Field<string>("movieId") == movieId.ToString()))
    {
        Console.WriteLine("Invalid or duplicate movie ID.");
        return;
    }

    Console.WriteLine("Enter the movie title:");
    string title = Console.ReadLine().Trim();

    if (movieData.AsEnumerable().Any(row => 
        row.Field<string>("title")?.Equals(title, StringComparison.OrdinalIgnoreCase) ?? false))
    {
        Console.WriteLine("This movie title already exists in the library.");
        return;
    }

    Console.WriteLine("Enter the genre:");
    string genre = Console.ReadLine().Trim();

    movieData.Rows.Add(movieId.ToString(), title, genre);

    SaveDataToFile();
}


        static void SaveDataToFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("\\movies.csv", false))
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
