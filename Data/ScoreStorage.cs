using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using zAjedrez.Model.Struct;

namespace zAjedrez.Data
{
    /// <summary>
    /// ScoreStorage CRUD operations - JSON persistence
    /// SERIALIZE/DESERIALIZE Score objects to/from JSON
    /// File location: C:\Data\zAjedrez\scores\scores.json
    /// </summary>
    public class ScoreStorage
    {
        #region Properties

        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR ScoreStorage initialize storage path
        /// PUSH _filePath from parameter OR default
        /// INIT JsonSerializerOptions with WriteIndented=true
        /// </summary>
        public ScoreStorage(string filePath = "data/scores/scores.json")
        {
            _filePath = filePath;
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
        /// Load all scores from storage
        /// TEST File.Exists(_filePath) // JZ return_empty_list
        /// CALL File.ReadAllText() -> PARSE JSON // RET List<Score>
        /// </summary>
        public List<Score> LoadAll()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<Score>();

                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Score>>(json, _jsonOptions) ?? new List<Score>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading scores: {ex.Message}");
                return new List<Score>();
            }
        }

        /// <summary>
        /// Save all scores to storage
        /// CALL JsonSerializer.Serialize() -> PUSH json_string
        /// CALL File.WriteAllText() -> WRITE to disk
        /// RET true on success
        /// </summary>
        public bool SaveAll(List<Score> scores)
        {
            try
            {
                string json = JsonSerializer.Serialize(scores, _jsonOptions);
                File.WriteAllText(_filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving scores: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Add new score record
        /// CALL LoadAll() -> PUSH collection
        /// PUSH score to collection
        /// CALL SaveAll(collection) -> WRITE disk
        /// RET true
        /// </summary>
        public bool Add(Score score)
        {
            try
            {
                var scores = LoadAll();
                scores.Add(score);
                return SaveAll(scores);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding score: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get global top scores ranking (top 15)
        /// CALL LoadAll() -> PUSH collection
        /// SORT by Points descending // TAKE 15
        /// RET top_scores
        /// </summary>
        public List<Score> GetGlobalRanking(int limit = 15)
        {
            var scores = LoadAll();
            return scores
                .OrderByDescending(s => s.Points)
                .Take(limit)
                .ToList();
        }

        /// <summary>
        /// Get user personal best scores
        /// CALL LoadAll() -> PUSH collection
        /// FILTER by userId // SORT by Points descending
        /// RET personal_scores
        /// </summary>
        public List<Score> GetUserScores(string userId)
        {
            var scores = LoadAll();
            return scores
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.Points)
                .ToList();
        }

        /// <summary>
        /// Get user personal best (highest score)
        /// CALL GetUserScores(userId) // TAKE 1
        /// RET first_or_null
        /// </summary>
        public Score? GetUserPersonalBest(string userId)
        {
            var userScores = GetUserScores(userId);
            return userScores.FirstOrDefault();
        }

        /// <summary>
        /// Find scores by game mode
        /// CALL LoadAll() -> PUSH collection
        /// FILTER by GameMode // SORT by Points descending
        /// RET filtered_scores
        /// </summary>
        public List<Score> FindByGameMode(string gameMode)
        {
            var scores = LoadAll();
            return scores
                .Where(s => s.GameMode == gameMode)
                .OrderByDescending(s => s.Points)
                .ToList();
        }

        /// <summary>
        /// Recalculate global ranking with rank positions
        /// CALL GetGlobalRanking() -> PUSH sorted_list
        /// LOOP with index // MOV [GlobalRank], index+1
        /// CALL SaveAll() -> WRITE disk
        /// </summary>
        public void RecalculateGlobalRanking()
        {
            var scores = LoadAll();
            var sorted = scores.OrderByDescending(s => s.Points).ToList();

            for (int i = 0; i < sorted.Count; i++)
            {
                sorted[i].GlobalRank = i + 1;
            }

            SaveAll(sorted);
        }

        /// <summary>
        /// Ensure directory exists or create
        /// PARSE directory path from _filePath
        /// TEST Directory.Exists() // JZ create_dir
        /// </summary>
        private void EnsureDirectoryExists()
        {
            string directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        #endregion
    }
}
