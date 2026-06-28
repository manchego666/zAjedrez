using System;

namespace zAjedrez.Model.Entity
{
    /// <summary>
    /// BoardEntity - intelligent map/board theme
    /// Stores board properties: visuals, audio, difficulty modifiers
    /// </summary>
    public class BoardEntity
    {
        #region Properties

        /// <summary>
        /// Unique board identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Board name/theme (e.g., "Forest", "Castle", "Modern")
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Board background image path
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Background music track path (optional)
        /// </summary>
        public string BackgroundMusicPath { get; set; }

        /// <summary>
        /// Background color hex (light or dark theme)
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Difficulty modifier (1.0 = standard chess rules)
        /// </summary>
        public float DifficultyModifier { get; set; }

        /// <summary>
        /// Creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR BoardEntity with default values
        /// PUSH Id = GUID // MOV DifficultyModifier = 1.0f
        /// </summary>
        public BoardEntity()
        {
            Id = Guid.NewGuid().ToString();
            DifficultyModifier = 1.0f;
            CreatedAt = DateTime.UtcNow;
        }

        public BoardEntity(string name, string imagePath, string backgroundColor, string musicPath = null)
            : this()
        {
            Name = name;
            ImagePath = imagePath;
            BackgroundColor = backgroundColor;
            BackgroundMusicPath = musicPath;
        }

        #endregion

        #region Methods

        /// <summary>
        /// PUSH formatted board descriptor to output
        /// </summary>
        public override string ToString()
        {
            return $"Board: {Name} | Color: {BackgroundColor} | Music: {BackgroundMusicPath ?? "None"}";
        }

        #endregion
    }
}
