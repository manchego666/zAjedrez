using System;

namespace zAjedrez.Model.Entity
{
    // ACHIEVEMENT BADGE STRUCT
    // PUSH Id, Title, Description, ImagePath onto stack
    // LOAD RequiredWins, RequiredElo thresholds (unlock conditions)
    // FLAG IsUnlocked tracks state (0=locked, 1=unlocked)
    // TIMESTAMP UnlockedAt records event time when badge earned
    public class Achievement
    {
        #region Properties

        public string Id { get; set; }              // GUID identifier, unique key
        public string Title { get; set; }           // ASCII badge name/title
        public string Description { get; set; }     // Text description of achievement
        public string ImagePath { get; set; }       // Path to badge icon/image file
        public int RequiredWins { get; set; }       // Threshold: wins needed to unlock
        public int RequiredElo { get; set; }        // Threshold: Elo rating needed to unlock
        public DateTime UnlockedAt { get; set; }    // Timestamp when badge was earned
        public bool IsUnlocked { get; set; }        // Flag: 0=locked, 1=unlocked state
        public string Category { get; set; }        // String category ("Victories", "Milestones", etc.)

        #endregion

        #region Constructor

        // CALL new Achievement() // POP return address
        // PUSH Id = GUID // MOV [IsUnlocked], FALSE
        public Achievement()
        {
            Id = Guid.NewGuid().ToString();
            IsUnlocked = false;
        }

        // OVERLOAD: new Achievement(title, desc, path, wins_req, elo_req, category)
        // CALL base() constructor first // POP // PUSH parameters
        public Achievement(string title, string description, string imagePath, int requiredWins, int requiredElo, string category)
            : this()
        {
            Title = title;
            Description = description;
            ImagePath = imagePath;
            RequiredWins = requiredWins;
            RequiredElo = requiredElo;
            Category = category;
        }

        #endregion

        #region Methods

        // VALIDATE unlock conditions
        // CMP userWins, RequiredWins // JL condition_failed
        // CMP userElo, RequiredElo // JL condition_failed
        // MOV AL, 1 // RET (all thresholds met)
        public bool CheckRequirements(int userWins, int userElo)
        {
            return userWins >= RequiredWins && userElo >= RequiredElo;
        }

        // UNLOCK achievement badge
        // MOV [IsUnlocked], TRUE // PUSH UnlockedAt = NOW()
        public void Unlock()
        {
            IsUnlocked = true;
            UnlockedAt = DateTime.UtcNow;
        }

        // PUSH formatted achievement string to output
        public override string ToString()
        {
            return $"Achievement: {Title} | Status: {(IsUnlocked ? "Unlocked" : "Locked")}";
        }

        #endregion
    }
}
