using System;

namespace zAjedrez.Model.Entity
{
    // Entidad que representa un usuario del sistema
    // Guarda información de perfil, credenciales y estadísticas
    public class User
    {
        #region Properties

        public string Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public int Elo { get; set; }
        public int WinsCount { get; set; }
        public int LossesCount { get; set; }
        public int DrawsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public bool IsActive { get; set; }

        #endregion

        #region Constructor

        public User()
        {
            Id = Guid.NewGuid().ToString();
            Elo = 1200; // Rating inicial
            WinsCount = 0;
            LossesCount = 0;
            DrawsCount = 0;
            CreatedAt = DateTime.UtcNow;
            LastLoginAt = DateTime.UtcNow;
            IsActive = true;
        }

        #endregion

        #region Methods

        // Calcula el porcentaje de victorias
        public double GetWinPercentage()
        {
            int totalGames = WinsCount + LossesCount + DrawsCount;
            if (totalGames == 0) return 0;
            return (double)WinsCount / totalGames * 100;
        }

        // Retorna el estado general del usuario en formato legible
        public override string ToString()
        {
            return $"User: {Username} | ELO: {Elo} | W:{WinsCount} L:{LossesCount} D:{DrawsCount}";
        }

        #endregion
    }
}
