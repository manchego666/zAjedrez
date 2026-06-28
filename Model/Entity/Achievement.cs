using System;

namespace zAjedrez.Model.Entity
{
    // Entidad que representa un logro o trofeo que puede obtener un usuario
    // Almacena descripción, imagen y requisitos del logro
    public class Achievement
    {
        #region Properties

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int RequiredWins { get; set; }
        public int RequiredElo { get; set; }
        public DateTime UnlockedAt { get; set; }
        public bool IsUnlocked { get; set; }
        public string Category { get; set; } // e.g., "Victories", "Milestones", "Special"

        #endregion

        #region Constructor

        public Achievement()
        {
            Id = Guid.NewGuid().ToString();
            IsUnlocked = false;
        }

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

        // Verifica si se cumplen los requisitos para desbloquear el logro
        public bool CheckRequirements(int userWins, int userElo)
        {
            return userWins >= RequiredWins && userElo >= RequiredElo;
        }

        // Desbloquea el logro
        public void Unlock()
        {
            IsUnlocked = true;
            UnlockedAt = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"Achievement: {Title} | Status: {(IsUnlocked ? "Unlocked" : "Locked")}";
        }

        #endregion
    }
}
