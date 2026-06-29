using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using zAjedrez.Model.Entity;

namespace zAjedrez.Data
{
    /// <summary>
    /// MatchStorage CRUD operations - JSON persistence
    /// SERIALIZE/DESERIALIZE Match objects to/from JSON
    /// File location: C:\Data\zAjedrez\matches\match-{guid}.json
    /// </summary>
    public class MatchStorage
    {
        #region Properties

        private readonly string _directoryPath;
        private readonly JsonSerializerOptions _jsonOptions;

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR MatchStorage initialize storage path
        /// PUSH _directoryPath from parameter OR default
        /// INIT JsonSerializerOptions with WriteIndented=true
        /// </summary>
        public MatchStorage(string directoryPath = "data/matches")
        {
            _directoryPath = directoryPath;
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            EnsureDirectoryExists();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load all matches from storage
        /// LOOP through files in _directoryPath // PARSE JSON // PUSH to list
        /// RET List<Match>
        /// </summary>
        public List<Match> LoadAll()
        {
            try
            {
                if (!Directory.Exists(_directoryPath))
                    return new List<Match>();

                var matches = new List<Match>();
                foreach (var filePath in Directory.GetFiles(_directoryPath, "match-*.json"))
                {
                    string json = File.ReadAllText(filePath);
                    var match = JsonSerializer.Deserialize<Match>(json, _jsonOptions);
                    if (match != null)
                        matches.Add(match);
                }
                return matches;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading matches: {ex.Message}");
                return new List<Match>();
            }
        }

        /// <summary>
        /// Load specific match by ID
        /// MOV filePath = Path.Combine(_directoryPath, match-{id}.json)
        /// TEST File.Exists(filePath) // JZ return_null
        /// CALL File.ReadAllText() -> PARSE JSON // RET Match
        /// </summary>
        public Match LoadById(string matchId)
        {
            try
            {
                string filePath = Path.Combine(_directoryPath, $"match-{matchId}.json");
                if (!File.Exists(filePath))
                    return null;

                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<Match>(json, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading match: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Save match to storage
        /// MOV filePath = Path.Combine(_directoryPath, match-{id}.json)
        /// CALL JsonSerializer.Serialize() -> PUSH json_string
        /// CALL File.WriteAllText() -> WRITE to disk
        /// RET true on success
        /// </summary>
        public bool Save(Match match)
        {
            try
            {
                string filePath = Path.Combine(_directoryPath, $"match-{match.Id}.json");
                string json = JsonSerializer.Serialize(match, _jsonOptions);
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving match: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Find matches by player ID
        /// LOOP through all matches // CMP player1Id OR player2Id // PUSH to results
        /// RET filtered list
        /// </summary>
        public List<Match> FindByPlayerId(string playerId)
        {
            var allMatches = LoadAll();
            return allMatches
                .Where(m => m.Player1Id == playerId || m.Player2Id == playerId)
                .ToList();
        }

        /// <summary>
        /// Find matches by game mode
        /// LOOP through all matches // CMP Mode == gameMode // PUSH to results
        /// RET filtered list
        /// </summary>
        public List<Match> FindByGameMode(zAjedrez.Model.Enums.GameMode gameMode)
        {
            var allMatches = LoadAll();
            return allMatches.Where(m => m.Mode == gameMode).ToList();
        }

        /// <summary>
        /// Find completed matches (winner determined)
        /// LOOP through all matches // TEST Winner != null // PUSH to results
        /// RET filtered list
        /// </summary>
        public List<Match> FindCompleted()
        {
            var allMatches = LoadAll();
            return allMatches.Where(m => m.Winner.HasValue).ToList();
        }

        /// <summary>
        /// Delete match by ID
        /// MOV filePath = Path.Combine(_directoryPath, match-{id}.json)
        /// CALL File.Delete(filePath)
        /// RET true on success
        /// </summary>
        public bool Delete(string matchId)
        {
            try
            {
                string filePath = Path.Combine(_directoryPath, $"match-{matchId}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting match: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Ensure directory exists or create
        /// TEST Directory.Exists(_directoryPath) // JZ create_dir
        /// CALL Directory.CreateDirectory()
        /// </summary>
        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(_directoryPath))
                Directory.CreateDirectory(_directoryPath);
        }

        #endregion
    }
}
