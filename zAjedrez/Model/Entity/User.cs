using System;

namespace zAjedrez.Model.Entity
{
    // USER PROFILE STRUCT
    // PUSH Id, Username, PasswordHash, Email onto profile stack
    // MOV [Elo], initial_rating (1200)
    // TRACK: WinsCount, LossesCount, DrawsCount via increment ops
    // PERSIST: CreatedAt, LastLoginAt timestamps
    public class User
    {
        #region Properties

        public string Id { get; set; }              // GUID identifier, unique key
        public string Username { get; set; }        // ASCII username, case-insensitive lookup
        public string PasswordHash { get; set; }    // SHA256 hash, never plain text
        public string Email { get; set; }           // Email address, optional contact
        public int Elo { get; set; }                // Rating integer, initial 1200
        public int WinsCount { get; set; }          // Victory counter, INC operation
        public int LossesCount { get; set; }        // Loss counter, INC operation
        public int DrawsCount { get; set; }         // Draw counter, INC operation
        public DateTime CreatedAt { get; set; }     // Timestamp of account creation
        public DateTime LastLoginAt { get; set; }   // Timestamp of last login event
        public bool IsActive { get; set; }          // Flag: active vs deleted account

        #endregion

        #region Constructor

        // CALL new User() // POP return address
        // PUSH Id = GUID // MOV [Elo], 1200 // PUSH CreatedAt = NOW()
        public User()
        {
            Id = Guid.NewGuid().ToString();
            Elo = 1200;
            WinsCount = 0;
            LossesCount = 0;
            DrawsCount = 0;
            CreatedAt = DateTime.UtcNow;
            LastLoginAt = DateTime.UtcNow;
            IsActive = true;
        }

        #endregion

        #region Methods

        // CALCULATE win percentage
        // MOV AX, WinsCount // ADD BX, LossesCount // ADD BX, DrawsCount
        // CMP BX, 0 // JE zero_division
        // DIV AX, BX // MUL AX, 100 // RET double
        public double GetWinPercentage()
        {
            int totalGames = WinsCount + LossesCount + DrawsCount;
            if (totalGames == 0) return 0;
            return (double)WinsCount / totalGames * 100;
        }

        // PUSH formatted user string to output
        public override string ToString()
        {
            return $"User: {Username} | ELO: {Elo} | W:{WinsCount} L:{LossesCount} D:{DrawsCount}";
        }

        #endregion
    }
}
