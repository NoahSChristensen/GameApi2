using System.Text.Json;
using GameApi2.Models;


namespace GameApi2.Services;


/// <summary>
/// Service til at gemme og hente data fra JSON fil.
/// Dette gør det nemt at skifte til database senere, da vi kun ændrer denne klasse.
/// </summary>


public class DataService
{
    private readonly string _dataFilePath;

    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger<DataService> _logger;
    public DataService(ILogger<DataService> logger)
    {
        _logger = logger;
        // Gem data i en "Data" mappe i projektet
        var dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        // Opret Data mappen hvis den ikke findes
        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }
        _dataFilePath = Path.Combine(dataDirectory, "users.json");
        // Konfigurer JSON serialisering
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true, // Gør JSON filen læsbar
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // camelCase i JSON
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }


    /// <summary>
    /// Henter alle brugere fra JSON filen
    /// </sum

    public async Task<List<User>> LoadUsersAsync()
    {
        // Hvis filen ikke findes, returner tom liste
        if (!File.Exists(_dataFilePath))
        {
            return new List<User>();
        }
        try
        {
            var json = await File.ReadAllTextAsync(_dataFilePath);
            // Hvis filen er tom, returner tom liste
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<User>();
            }
            var users = JsonSerializer.Deserialize<List<User>>(json, _jsonOptions);
            return users ?? new List<User>();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Fejl ved læsning af data fra {Path}", _dataFilePath);
            return new List<User>();
        }
    }

    // <summary>
    /// Gemmer alle brugere til JSON filen
    /// </summary>
    /// 
    /// 
    /// 

    public async Task SaveUsersAsync(List<User> users)
    {
        try
        {
            var json = JsonSerializer.Serialize(users, _jsonOptions);
            await File.WriteAllTextAsync(_dataFilePath, json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved gemning af data til {Path}", _dataFilePath);
            throw new Exception("Kunne ikke gemme data", ex);
        }
    }
}
