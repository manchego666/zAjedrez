using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using zAjedrez.Model.Entity;

namespace zAjedrez.Data
{
    // Repositorio para gestionar la persistencia de usuarios en JSON
    // Maneja lectura, escritura y operaciones CRUD de usuarios
    public class UserRepository
    {
        #region Properties

        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        #endregion

        #region Constructor

        public UserRepository(string filePath = "data/users.json")
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

        // Carga todos los usuarios desde el archivo JSON
        public List<User> LoadAll()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<User>();

                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<User>>(json, _jsonOptions) ?? new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar usuarios: {ex.Message}");
                return new List<User>();
            }
        }

        // Guarda todos los usuarios en el archivo JSON
        public bool SaveAll(List<User> users)
        {
            try
            {
                string json = JsonSerializer.Serialize(users, _jsonOptions);
                File.WriteAllText(_filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar usuarios: {ex.Message}");
                return false;
            }
        }

        // Busca un usuario por ID
        public User FindById(string id)
        {
            var users = LoadAll();
            return users.FirstOrDefault(u => u.Id == id);
        }

        // Busca un usuario por nombre de usuario
        public User FindByUsername(string username)
        {
            var users = LoadAll();
            return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        // Añade un nuevo usuario
        public bool Add(User user)
        {
            try
            {
                var users = LoadAll();
                if (users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                    return false; // Usuario ya existe

                users.Add(user);
                return SaveAll(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al añadir usuario: {ex.Message}");
                return false;
            }
        }

        // Actualiza un usuario existente
        public bool Update(User user)
        {
            try
            {
                var users = LoadAll();
                var existingUser = users.FirstOrDefault(u => u.Id == user.Id);

                if (existingUser == null)
                    return false;

                int index = users.IndexOf(existingUser);
                users[index] = user;
                return SaveAll(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar usuario: {ex.Message}");
                return false;
            }
        }

        // Elimina un usuario
        public bool Delete(string userId)
        {
            try
            {
                var users = LoadAll();
                var userToRemove = users.FirstOrDefault(u => u.Id == userId);

                if (userToRemove == null)
                    return false;

                users.Remove(userToRemove);
                return SaveAll(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
                return false;
            }
        }

        // Retorna los usuarios ordenados por ELO (ranking)
        public List<User> GetRanking(int limit = 10)
        {
            var users = LoadAll();
            return users.OrderByDescending(u => u.Elo).Take(limit).ToList();
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
