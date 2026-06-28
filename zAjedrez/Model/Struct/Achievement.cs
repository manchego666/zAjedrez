using System;
using System.Collections.Generic;

namespace zAjedrez.Model.Struct
{
    /// <summary>
    /// Achievement badge struct
    /// Part of User entity - represents milestone achievement
    /// Locked/Unlocked status with unlock conditions
    /// </summary>
    public struct Achievement
    {
        #region Properties

        /// <summary>
        /// Unique achievement identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Achievement title (e.g., "Victory10Bots")
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Achievement description for UI display
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Badge icon/image path
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Required victories to unlock this achievement
        /// </summary>
        public int RequiredVictories { get; set; }

        /// <summary>
        /// Required minimum pieces remaining to unlock
        /// </summary>
        public int RequiredPiecesRemaining { get; set; }

        /// <summary>
        /// Required maximum game duration in milliseconds
        /// 0 = no time constraint
        /// </summary>
        public int MaxDurationMs { get; set; }

        /// <summary>
        /// Unlock status flag
        /// </summary>
        public bool IsUnlocked { get; set; }

        /// <summary>
        /// Timestamp when badge was unlocked
        /// </summary>
        public DateTime UnlockedAt { get; set; }

        /// <summary>
        /// Achievement category (e.g., "Victory", "Defense", "Speed")
        /// </summary>
        public string Category { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR Achievement with parameters
        /// PUSH all properties to struct stack
        /// </summary>
        public Achievement(string title, string description, string imagePath, string category,
                          int requiredVictories = 1, int requiredPieces = 0, int maxDurationMs = 0)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            ImagePath = imagePath;
            Category = category;
            RequiredVictories = requiredVictories;
            RequiredPiecesRemaining = requiredPieces;
            MaxDurationMs = maxDurationMs;
            IsUnlocked = false;
            UnlockedAt = DateTime.MinValue;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validate if player meets unlock requirements
        /// CMP playerVictories, RequiredVictories // JL condition_failed
        /// CMP piecesRemaining, RequiredPiecesRemaining // JL condition_failed
        /// CMP gameDuration, MaxDurationMs // JG condition_failed
        /// MOV AL, 1 // RET (all conditions met)
        /// </summary>
        public bool CheckRequirements(int playerVictories, int piecesRemaining, int gameDurationMs)
        {
            bool victoryMet = playerVictories >= RequiredVictories;
            bool piecesMet = piecesRemaining >= RequiredPiecesRemaining;
            bool timeMet = (MaxDurationMs == 0) || (gameDurationMs <= MaxDurationMs);

            return victoryMet && piecesMet && timeMet;
        }

        /// <summary>
        /// Unlock achievement badge
        /// MOV IsUnlocked = TRUE // PUSH UnlockedAt = NOW()
        /// </summary>
        public void Unlock()
        {
            IsUnlocked = true;
            UnlockedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// PUSH formatted achievement descriptor to output
        /// </summary>
        public override string ToString()
        {
            return $"[{Category}] {Title} | Status: {(IsUnlocked ? "UNLOCKED" : "LOCKED")}";
        }

        #endregion
    }
}
