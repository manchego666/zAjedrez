using System;
using System.Collections.Generic;
using zAjedrez.Model.Enums;

namespace zAjedrez.Model.Struct
{
    /// <summary>
    /// GameSession struct - represents active game state
    /// Tracks players, board, timing, captured pieces, and move history
    /// Used for both local bot matches and multiplayer sessions
    /// </summary>
    public class GameSession
    {
        #region Properties

        /// <summary>
        /// Unique session identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Player user ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Opponent ID (bot ID or other player ID)
        /// </summary>
        public string OpponentId { get; set; }

        /// <summary>
        /// Board/map theme ID
        /// </summary>
        public string BoardId { get; set; }

        /// <summary>
        /// Game mode: LocalBot, Multiplayer, etc.
        /// </summary>
        public GameMode Mode { get; set; }

        /// <summary>
        /// Session start timestamp
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Session end timestamp (0 if ongoing)
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Current turn timer remaining in milliseconds
        /// </summary>
        public int TurnTimeRemaining { get; set; }

        /// <summary>
        /// Captured pieces counter: PieceType -> count
        /// MOV [CapturedPieces], dict // PUSH captured_piece // INC count
        /// </summary>
        public Dictionary<PieceType, int> CapturedPieces { get; set; } = new();

        /// <summary>
        /// Lost pieces counter: PieceType -> count
        /// MOV [LostPieces], dict // PUSH lost_piece // INC count
        /// </summary>
        public Dictionary<PieceType, int> LostPieces { get; set; } = new();

        /// <summary>
        /// Move history for replay/undo functionality
        /// PUSH move notation (e.g., "e2-e4", "Nf3-h5")
        /// </summary>
        public List<string> MoveHistory { get; set; } = new();

        /// <summary>
        /// Winner color (if game ended)
        /// </summary>
        public PieceColor? Winner { get; set; }

        /// <summary>
        /// Game status: active, completed, abandoned
        /// </summary>
        public GameStatus Status { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR GameSession initialize new session
        /// PUSH Id = GUID // PUSH StartTime = NOW()
        /// CALL InitializePieceCounts()
        /// </summary>
        public GameSession()
        {
            Id = Guid.NewGuid().ToString();
            StartTime = DateTime.UtcNow;
            TurnTimeRemaining = 30000; // 30 seconds per turn
            Status = GameStatus.Active;
            InitializePieceCounts();
        }

        public GameSession(string userId, string opponentId, string boardId, GameMode mode)
            : this()
        {
            UserId = userId;
            OpponentId = opponentId;
            BoardId = boardId;
            Mode = mode;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize piece counters with standard chess piece counts
        /// 8 pawns, 2 rooks, 2 knights, 2 bishops, 1 queen, 1 king per side
        /// </summary>
        private void InitializePieceCounts()
        {
            CapturedPieces[PieceType.Pawn] = 0;
            CapturedPieces[PieceType.Rook] = 0;
            CapturedPieces[PieceType.Knight] = 0;
            CapturedPieces[PieceType.Bishop] = 0;
            CapturedPieces[PieceType.Queen] = 0;
            CapturedPieces[PieceType.King] = 0;

            LostPieces[PieceType.Pawn] = 0;
            LostPieces[PieceType.Rook] = 0;
            LostPieces[PieceType.Knight] = 0;
            LostPieces[PieceType.Bishop] = 0;
            LostPieces[PieceType.Queen] = 0;
            LostPieces[PieceType.King] = 0;
        }

        /// <summary>
        /// Record captured piece
        /// PUSH piece type -> INC CapturedPieces[type]
        /// </summary>
        public void RecordCapturedPiece(PieceType pieceType)
        {
            if (CapturedPieces.ContainsKey(pieceType))
                CapturedPieces[pieceType]++;
            else
                CapturedPieces[pieceType] = 1;
        }

        /// <summary>
        /// Record lost piece
        /// PUSH piece type -> INC LostPieces[type]
        /// </summary>
        public void RecordLostPiece(PieceType pieceType)
        {
            if (LostPieces.ContainsKey(pieceType))
                LostPieces[pieceType]++;
            else
                LostPieces[pieceType] = 1;
        }

        /// <summary>
        /// Calculate total game duration
        /// MOV AX, EndTime - StartTime // RET milliseconds
        /// </summary>
        public int GetDurationMs()
        {
            var endTime = EndTime == DateTime.MinValue ? DateTime.UtcNow : EndTime;
            return (int)(endTime - StartTime).TotalMilliseconds;
        }

        /// <summary>
        /// Count total pieces captured by player
        /// LOOP through CapturedPieces // ADD counts // RET total
        /// </summary>
        public int GetTotalCapturedPieces()
        {
            int total = 0;
            foreach (var count in CapturedPieces.Values)
                total += count;
            return total;
        }

        /// <summary>
        /// Count total pieces lost by player
        /// LOOP through LostPieces // ADD counts // RET total
        /// </summary>
        public int GetTotalLostPieces()
        {
            int total = 0;
            foreach (var count in LostPieces.Values)
                total += count;
            return total;
        }

        /// <summary>
        /// Calculate remaining pieces on player's side
        /// MOV AX, 16 (standard chess pieces) // SUB AX, GetTotalLostPieces()
        /// RET pieces_remaining
        /// </summary>
        public int GetRemainingPieces()
        {
            return 16 - GetTotalLostPieces();
        }

        /// <summary>
        /// End session and record winner
        /// MOV [EndTime], NOW() // MOV [Winner], color // MOV [Status], Completed
        /// </summary>
        public void EndSession(PieceColor winner)
        {
            EndTime = DateTime.UtcNow;
            Winner = winner;
            Status = GameStatus.Completed;
        }

        /// <summary>
        /// PUSH formatted game session descriptor to output
        /// </summary>
        public override string ToString()
        {
            return $"Session: {Id} | Duration: {GetDurationMs()}ms | Captured: {GetTotalCapturedPieces()} | Lost: {GetTotalLostPieces()}";
        }

        #endregion
    }
}
