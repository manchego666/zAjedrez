using System;
using System.Collections.Generic;
using zAjedrez.Model.Struct;

namespace zAjedrez.Model.Entity
{
    /// <summary>
    /// Bot entity - AI opponent for local game sessions
    /// Implements strategy logic: capture priority > king defense > random move
    /// </summary>
    public class Bot
    {
        #region Properties

        /// <summary>
        /// Unique bot identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Bot difficulty level (1-10, where 10 is hardest)
        /// Affects move calculation depth and strategy sophistication
        /// </summary>
        public int DifficultyLevel { get; set; }

        /// <summary>
        /// Bot personality name (e.g., "Alpha", "Luna", "Phantom")
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Bot avatar image path for UI display
        /// </summary>
        public string AvatarPath { get; set; }

        /// <summary>
        /// Total wins achieved by bot
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Total losses by bot
        /// </summary>
        public int Losses { get; set; }

        /// <summary>
        /// Bot ELO rating (dynamic based on match results)
        /// </summary>
        public int EloRating { get; set; }

        /// <summary>
        /// List of all achievements unlocked by bot
        /// </summary>
        public List<Achievement> Achievements { get; set; } = new();

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR Bot with parameters
        /// PUSH Id = GUID // PUSH DifficultyLevel (1-10)
        /// MOV [EloRating], 1200 (initial rating)
        /// </summary>
        public Bot()
        {
            Id = Guid.NewGuid().ToString();
            EloRating = 1200;
            Wins = 0;
            Losses = 0;
        }

        public Bot(string name, int difficultyLevel, string avatarPath) : this()
        {
            Name = name;
            DifficultyLevel = Math.Clamp(difficultyLevel, 1, 10);
            AvatarPath = avatarPath;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get next move for bot based on difficulty level
        /// BOT_STRATEGY: prioritize capture operations
        /// CMP can_capture_piece // JNE check_king_defense
        /// CALL find_best_capture() // RET move
        /// check_king_defense: CALL find_defense_move() // RET move
        /// ELSE: CALL find_random_legal_move() // RET move
        /// </summary>
        public (int fromRow, int fromCol, int toRow, int toCol)? GetNextMove(Core.Board board, Enums.PieceColor botColor)
        {
            // TODO: Implement difficulty-based move selection
            // Level 1-3: random moves
            // Level 4-7: capture prioritization
            // Level 8-10: advanced tactics (king defense, position evaluation)

            return null; // Placeholder
        }

        /// <summary>
        /// Find best capture move available
        /// LOOP through all pieces // LOOP through legal moves
        /// CMP destination_has_enemy_piece // PUSH to capture_list
        /// SORT by piece_value (queen > rook > bishop > knight > pawn)
        /// RET best_capture_move
        /// </summary>
        public (int fromRow, int fromCol, int toRow, int toCol)? FindBestCapture(Core.Board board, Enums.PieceColor botColor)
        {
            // TODO: Implement capture finding logic
            return null; // Placeholder
        }

        /// <summary>
        /// Find move that defends the king
        /// LOCATE king position // CHECK threatened squares
        /// LOOP through pieces that can move to defense squares
        /// RET move that blocks/defends
        /// </summary>
        public (int fromRow, int fromCol, int toRow, int toCol)? FindDefenseMove(Core.Board board, Enums.PieceColor botColor)
        {
            // TODO: Implement king defense logic
            return null; // Placeholder
        }

        /// <summary>
        /// Find random legal move (fallback)
        /// LOOP through all pieces of bot color
        /// LOOP through all squares on board
        /// CMP IsMoveLegal() // PUSH valid_moves to list
        /// SELECT random from list // RET move
        /// </summary>
        public (int fromRow, int fromCol, int toRow, int toCol)? FindRandomLegalMove(Core.Board board, Enums.PieceColor botColor)
        {
            // TODO: Implement random move finding
            return null; // Placeholder
        }

        /// <summary>
        /// Calculate win percentage
        /// MOV AX, Wins // ADD BX, Losses
        /// CMP BX, 0 // JE zero_division
        /// DIV AX, BX // MUL AX, 100 // RET double
        /// </summary>
        public double GetWinPercentage()
        {
            int totalGames = Wins + Losses;
            if (totalGames == 0) return 0;
            return (double)Wins / totalGames * 100;
        }

        /// <summary>
        /// PUSH formatted bot descriptor to output
        /// </summary>
        public override string ToString()
        {
            return $"Bot: {Name} | Level: {DifficultyLevel}/10 | ELO: {EloRating} | W:{Wins} L:{Losses}";
        }

        #endregion
    }
}
