using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using zAjedrez.Model.Entity;

namespace zAjedrez.Data
{
    // ACHIEVEMENT REPOSITORY CRUD OPERATIONS
    // SERIALIZE/DESERIALIZE Achievement objects to/from JSON file
    // PERSIST state in [_filePath] @ runtime
    // LoadAll: READ file -> PARSE JSON -> RET List<Achievement>
    // SaveAll: SERIALIZE List<Achievement> -> WRITE file
    // FindById: LOOP through collection, CMP Id, RET match
    // FindByCategory: FILTER by category string, RET filtered list
    // Add: PUSH new Achievement to collection -> CALL SaveAll()
    // Update: FIND existing -> REPLACE -> CALL SaveAll()
    // Delete: FIND existing -> POP from collection -> CALL SaveAll()
    // GetAchievementsByDifficulty: SORT by RequiredElo, RET ordered list
    public class AchievementRepository
    {
        #region Properties

        private readonly string _filePath;                  // MOV [_filePath], path_to_file
        private readonly JsonSerializerOptions _jsonOptions; // MOV [_jsonOptions], indent_flag

        #endregion

        #region Constructor

        // CTOR: PUSH _filePath from parameter OR default "data/achievements.json"
        // INIT JsonSerializerOptions with WriteIndented=true, CamelCase naming
        // CALL EnsureDirectoryExists() to CREATE dir if missing
        public AchievementRepository(string filePath = "data/achievements.json")
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

        // LOAD ALL ACHIEVEMENTS FROM DISK
        // TEST File.Exists(_filePath) // JZ return_empty_list
        // CALL File.ReadAllText() -> PUSH json_string
        // CALL JsonSerializer.Deserialize<List<Achievement>>() -> PUSH list
        // RET list OR empty_list on exception
        public List<Achievement> LoadAll()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<Achievement>();

                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Achievement>>(json, _jsonOptions) ?? new List<Achievement>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading achievements: {ex.Message}");
                return new List<Achievement>();
            }
        }

        // SAVE ALL ACHIEVEMENTS TO DISK
        // CALL JsonSerializer.Serialize() -> PUSH json_string
        // CALL File.WriteAllText(_filePath, json) -> WRITE to disk
        // RET true on success, false on exception
        public bool SaveAll(List<Achievement> achievements)
        {
            try
            {
                string json = JsonSerializer.Serialize(achievements, _jsonOptions);
                File.WriteAllText(_filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving achievements: {ex.Message}");
                return false;
            }
        }

        // FIND ACHIEVEMENT BY ID
        // CALL LoadAll() -> PUSH collection
        // LOOP: CMP a.Id, id // JE found_achievement
        // RET FirstOrDefault() (null if not found)
        public Achievement FindById(string id)
        {
            var achievements = LoadAll();
            return achievements.FirstOrDefault(a => a.Id == id);
        }

        // FIND ACHIEVEMENTS BY CATEGORY (filter operation)
        // CALL LoadAll() -> PUSH collection
        // LOOP: CMP a.Category.ToLower(), category.ToLower() // JE match (PUSH to results)
        // RET filtered list
        public List<Achievement> FindByCategory(string category)
        {
            var achievements = LoadAll();
            return achievements.Where(a => a.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // ADD NEW ACHIEVEMENT
        // CALL LoadAll() -> PUSH collection
        // PUSH achievement to collection
        // CALL SaveAll(collection) -> WRITE disk
        // RET status
        public bool Add(Achievement achievement)
        {
            try
            {
                var achievements = LoadAll();
                achievements.Add(achievement);
                return SaveAll(achievements);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding achievement: {ex.Message}");
                return false;
            }
        }

        // UPDATE EXISTING ACHIEVEMENT
        // CALL LoadAll() -> PUSH collection
        // FIND achievement where Id matches // JE not_found (RET false)
        // PUSH (new) achievement to collection[index] -> REPLACE
        // CALL SaveAll(collection) -> WRITE disk
        // RET status
        public bool Update(Achievement achievement)
        {
            try
            {
                var achievements = LoadAll();
                var existingAchievement = achievements.FirstOrDefault(a => a.Id == achievement.Id);

                if (existingAchievement == null)
                    return false;

                int index = achievements.IndexOf(existingAchievement);
                achievements[index] = achievement;
                return SaveAll(achievements);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating achievement: {ex.Message}");
                return false;
            }
        }

        // DELETE ACHIEVEMENT BY ID
        // CALL LoadAll() -> PUSH collection
        // FIND achievement where Id matches // JE not_found (RET false)
        // POP achievement from collection
        // CALL SaveAll(collection) -> WRITE disk
        // RET status
        public bool Delete(string achievementId)
        {
            try
            {
                var achievements = LoadAll();
                var achievementToRemove = achievements.FirstOrDefault(a => a.Id == achievementId);

                if (achievementToRemove == null)
                    return false;

                achievements.Remove(achievementToRemove);
                return SaveAll(achievements);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting achievement: {ex.Message}");
                return false;
            }
        }

        // RETRIEVE ACHIEVEMENTS BY DIFFICULTY (sort by RequiredElo)
        // CALL LoadAll() -> PUSH collection
        // IF category != null: FILTER by category (optional)
        // SORT by RequiredElo ascending (easiest first)
        // RET sorted list
        public List<Achievement> GetAchievementsByDifficulty(string category = null)
        {
            var achievements = LoadAll();
            if (!string.IsNullOrEmpty(category))
                achievements = achievements.Where(a => a.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

            return achievements.OrderBy(a => a.RequiredElo).ToList();
        }

        // ENSURE DIRECTORY EXISTS OR CREATE
        // PARSE directory path from _filePath
        // TEST Directory.Exists(directory) // JZ create_dir
        // CALL Directory.CreateDirectory(directory)
        private void EnsureDirectoryExists()
        {
            string directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        #endregion
    }
}
