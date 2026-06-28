using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using zAjedrez.Model.Entity;

namespace zAjedrez.Data
{
    // USER REPOSITORY CRUD OPERATIONS
    // SERIALIZE/DESERIALIZE User objects to/from JSON file
    // PERSIST state in [_filePath] @ runtime
    // LoadAll: READ file -> PARSE JSON -> RET List<User>
    // SaveAll: SERIALIZE List<User> -> WRITE file
    // FindById: LOOP through collection, CMP Id, RET match
    // Add: PUSH new User to collection -> CALL SaveAll()
    // Update: FIND existing -> REPLACE -> CALL SaveAll()
    // Delete: FIND existing -> POP from collection -> CALL SaveAll()
    public class UserRepository
    {
        #region Properties

        private readonly string _filePath;                  // MOV [_filePath], path_to_file
        private readonly JsonSerializerOptions _jsonOptions; // MOV [_jsonOptions], indent_flag

        #endregion

        #region Constructor

        // CTOR: PUSH _filePath from parameter OR default "data/users.json"
        // INIT JsonSerializerOptions with WriteIndented=true, CamelCase naming
        // CALL EnsureDirectoryExists() to CREATE dir if missing
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

        // LOAD ALL USERS FROM DISK
        // TEST File.Exists(_filePath) // JZ return_empty_list
        // CALL File.ReadAllText() -> PUSH json_string
        // CALL JsonSerializer.Deserialize<List<User>>() -> PUSH list
        // RET list OR empty_list on exception
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
                Console.WriteLine($"Error loading users: {ex.Message}");
                return new List<User>();
            }
        }

        // SAVE ALL USERS TO DISK
        // CALL JsonSerializer.Serialize() -> PUSH json_string
        // CALL File.WriteAllText(_filePath, json) -> WRITE to disk
        // RET true on success, false on exception
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
                Console.WriteLine($"Error saving users: {ex.Message}");
                return false;
            }
        }

        // FIND USER BY ID
        // CALL LoadAll() -> PUSH collection
        // LOOP: CMP u.Id, id // JE found_user
        // RET FirstOrDefault() (null if not found)
        public User FindById(string id)
        {
            var users = LoadAll();
            return users.FirstOrDefault(u => u.Id == id);
        }

        // FIND USER BY USERNAME (case-insensitive)
        // CALL LoadAll() -> PUSH collection
        // LOOP: CMP u.Username.ToLower(), username.ToLower() // JE found_user
        // RET FirstOrDefault() (null if not found)
        public User FindByUsername(string username)
        {
            var users = LoadAll();
            return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        // ADD NEW USER
        // CALL LoadAll() -> PUSH collection
        // LOOP: CMP u.Username, user.Username // JE already_exists (RET false)
        // PUSH user to collection
        // CALL SaveAll(collection) -> WRITE disk
        // RET status
        public bool Add(User user)
        {
            try
            {
                var users = LoadAll();
                if (users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                    return false;

                users.Add(user);
                return SaveAll(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                return false;
            }
        }

        // UPDATE EXISTING USER
        // CALL LoadAll() -> PUSH collection
        // FIND user where Id matches // JE not_found (RET false)
        // PUSH (new) user to collection[index] -> REPLACE
        // CALL SaveAll(collection) -> WRITE disk
        // RET status
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
                Console.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        // DELETE USER BY ID
        // CALL LoadAll() -> PUSH collection
        // FIND user where Id matches // JE not_found (RET false)
        // POP user from collection
        // CALL SaveAll(collection) -> WRITE disk
        // RET status
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
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return false;
            }
        }

        // RETRIEVE RANKING (TOP N USERS BY ELO)
        // CALL LoadAll() -> PUSH collection
        // SORT by Elo descending (highest first)
        // TAKE limit (default 10)
        // RET sorted list
        public List<User> GetRanking(int limit = 10)
        {
            var users = LoadAll();
            return users.OrderByDescending(u => u.Elo).Take(limit).ToList();
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
