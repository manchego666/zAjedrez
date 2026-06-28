using System;

namespace zAjedrez.Model.Struct
{
    /// <summary>
    /// Score struct - represents match result scoring
    /// Part of game session - tracks personal and global rankings
    /// Player can view top 15 scores globally or personal best
    /// </summary>
    public struct Score
    {
        #region Properties

        /// <summary>
        /// User identifier who earned the score
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Match/game session ID that generated score
        /// </summary>
        public string MatchId { get; set; }

        /// <summary>
        /// Points earned in this match
        /// Calculated from: victory bonus + pieces captured - time penalty
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Global ranking position (top 15 leaderboard)
        /// </summary>
        public int GlobalRank { get; set; }

        /// <summary>
        /// User's personal best score achieved
        /// </summary>
        public int PersonalBest { get; set; }

        /// <summary>
        /// Timestamp when score was recorded
        /// </summary>
        public DateTime RecordedAt { get; set; }

        /// <summary>
        /// Game mode: LocalBot, Multiplayer, etc.
        /// </summary>
        public string GameMode { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR Score with parameters
        /// PUSH UserId, MatchId, Points, GlobalRank
        /// PUSH RecordedAt = NOW()
        /// </summary>
        public Score(string userId, string matchId, int points, int globalRank = 0)
        {
            UserId = userId;
            MatchId = matchId;
            Points = points;
            GlobalRank = globalRank;
            PersonalBest = points;
            RecordedAt = DateTime.UtcNow;
            GameMode = "LocalBot";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculate score based on match result
        /// MOV AX, victory_bonus (100) // ADD AX, captured_pieces*10 // SUB AX, time_penalty
        /// RET calculated_points
        /// </summary>
        public static int CalculatePoints(bool isVictory, int capturedPieces, int gameDurationMs)
        {
            int points = 0;

            // Victory bonus
            if (isVictory)
                points += 100;

            // Captured pieces bonus
            points += capturedPieces * 10;

            // Time penalty (1 point per 1000ms = 1 second)
            int timePenalty = gameDurationMs / 1000;
            points -= timePenalty;

            return Math.Max(0, points); // Minimum 0 points
        }

        /// <summary>
        /// PUSH formatted score descriptor to output
        /// </summary>
        public override string ToString()
        {
            return $"Score: {Points} | Rank: {GlobalRank} | Personal Best: {PersonalBest} | Mode: {GameMode}";
        }

        #endregion
    }
}
