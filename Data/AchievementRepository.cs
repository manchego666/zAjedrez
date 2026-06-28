using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using zAjedrez.Model.Entity;

namespace zAjedrez.Data
{
    // Repositorio para gestionar la persistencia de logros en JSON
    // Maneja lectura, escritura y operaciones CRUD de logros
    public class AchievementRepository
    {
        #region Properties

        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        #endregion

        #region Constructor

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

        // Carga todos los logros desde el archivo JSON
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
                Console.WriteLine($"Error al cargar logros: {ex.Message}");
                return new List<Achievement>();
            }
        }

        // Guarda todos los logros en el archivo JSON
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
                Console.WriteLine($"Error al guardar logros: {ex.Message}");
                return false;
            }
        }

        // Busca un logro por ID
        public Achievement FindById(string id)
        {
            var achievements = LoadAll();
            return achievements.FirstOrDefault(a => a.Id == id);
        }

        // Busca logros por categoría
        public List<Achievement> FindByCategory(string category)
        {
            var achievements = LoadAll();
            return achievements.Where(a => a.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Añade un nuevo logro
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
                Console.WriteLine($"Error al añadir logro: {ex.Message}");
                return false;
            }
        }

        // Actualiza un logro existente
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
                Console.WriteLine($"Error al actualizar logro: {ex.Message}");
                return false;
            }
        }

        // Elimina un logro
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
                Console.WriteLine($"Error al eliminar logro: {ex.Message}");
                return false;
            }
        }

        // Retorna todos los logros de una categoría ordenados por dificultad (ELO requerido)
        public List<Achievement> GetAchievementsByDifficulty(string category = null)
        {
            var achievements = LoadAll();
            if (!string.IsNullOrEmpty(category))
                achievements = achievements.Where(a => a.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

            return achievements.OrderBy(a => a.RequiredElo).ToList();
        }

        // Verifica que exista el directorio para el archivo
        private void EnsureDirectoryExists()
        {
            string directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        #endregion
    }
}
