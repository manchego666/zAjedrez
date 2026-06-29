using System;
using System.Collections.Generic;
using zAjedrez.Model.Enums;

namespace zAjedrez.Model.Entity
{
    /// <summary>
    /// Match entity - represents complete chess game session
    /// Tracks players, board state, moves, results, timestamps
    /// Strong entity: always has GUID Id, persists to storage
    /// </summary>
    public class Match
    {
        #region Properties

        /// <summary>
        /// Unique match identifier (GUID)
        /// MOV [Id], guid_new() // PRIMARY KEY
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Player 1 (White) user ID
        /// </summary>
        public string Player1Id { get; set; }

        /// <summary>
        /// Player 2 (Black) user ID or Bot ID
        /// </summary>
        public string Player2Id { get; set; }

        /// <summary>
        /// Board/map theme ID used in this match
        /// </summary>
        public string BoardId { get; set; }

        /// <summary>
        /// Game mode: LocalBot vs Multiplayer
        /// </summary>
        public GameMode Mode { get; set; }

        /// <summary>
        /// Match status: Active, Completed, Abandoned
        /// </summary>
        public GameStatus Status { get; set; }

        /// <summary>
        /// Exact timestamp when match started
        /// PUSH StartTime = DateTime.UtcNow
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Exact timestamp when match ended (0 if ongoing)
        /// MOV EndTime = NOW() when game concludes
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Winner color (White/Black) or null if ongoing/abandoned
        /// </summary>
        public PieceColor? Winner { get; set; }

        /// <summary>
        /// Current board state (8x8 grid of pieces)
        /// SERIALIZE to JSON as PieceType array
        /// </summary>
        public int[,] BoardState { get; set; }

        /// <summary>
        /// Complete move history in algebraic notation
        /// e.g., ["e2-e4", "e7-e5", "Nf1-f3", ...]
        /// PUSH move to list after each legal move
        /// </summary>
        public List<string> MoveHistory { get; set; } = new();

        /// <summary>
        /// Player 1 captured pieces counter: PieceType -> count
        /// MOV [CapturedByPlayer1], dict // PUSH piece // INC count
        /// </summary>
        public Dictionary<PieceType, int> CapturedByPlayer1 { get; set; } = new();

        /// <summary>
        /// Player 2 captured pieces counter: PieceType -> count
        /// MOV [CapturedByPlayer2], dict // PUSH piece // INC count
        /// </summary>
        public Dictionary<PieceType, int> CapturedByPlayer2 { get; set; } = new();

        /// <summary>
        /// Player 1 lost pieces counter: PieceType -> count
        /// </summary>
        public Dictionary<PieceType, int> LostByPlayer1 { get; set; } = new();

        /// <summary>
        /// Player 2 lost pieces counter: PieceType -> count
        /// </summary>
        public Dictionary<PieceType, int> LostByPlayer2 { get; set; } = new();

        /// <summary>
        /// Current turn: White or Black
        /// MOV [CurrentTurn], color // TOGGLE after each valid move
        /// </summary>
        public PieceColor CurrentTurn { get; set; }

        /// <summary>
        /// Total half-moves (plies) executed in match
        /// INC MoveCount after each legal move
        /// </summary>
        public int MoveCount { get; set; }

        /// <summary>
        /// Time remaining for current player (milliseconds)
        /// 0 = no time constraint
        /// </summary>
        public int TurnTimeRemaining { get; set; }

        /// <summary>
        /// Match difficulty modifier (1.0 = standard)
        /// Used for score calculation
        /// </summary>
        public float DifficultyModifier { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// CTOR Match - initialize new game session
        /// PUSH Id = GUID // PUSH StartTime = NOW()
        /// CALL InitializePieceCounts() // MOV CurrentTurn = White
        /// </summary>
        public Match()
        {
            Id = Guid.NewGuid().ToString();
            StartTime = DateTime.UtcNow;
            CurrentTurn = PieceColor.White;
            MoveCount = 0;
            TurnTimeRemaining = 0;
            Status = GameStatus.Active;
            DifficultyModifier = 1.0f;
            InitializePieceCounts();
        }

        /// <summary>
        /// CTOR Match with parameters
        /// </summary>
        public Match(string player1Id, string player2Id, string boardId, GameMode mode)
            : this()
        {
            Player1Id = player1Id;
            Player2Id = player2Id;
            BoardId = boardId;
            Mode = mode;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize piece counters with standard chess piece counts
        /// 8 pawns, 2 rooks, 2 knights, 2 bishops, 1 queen, 1 king per player
        /// PUSH piece type -> MOV count = 0
        /// </summary>
        private void InitializePieceCounts()
        {
            CapturedByPlayer1[PieceType.Pawn] = 0;
            CapturedByPlayer1[PieceType.Rook] = 0;
            CapturedByPlayer1[PieceType.Knight] = 0;
            CapturedByPlayer1[PieceType.Bishop] = 0;
            CapturedByPlayer1[PieceType.Queen] = 0;
            CapturedByPlayer1[PieceType.King] = 0;

            LostByPlayer1[PieceType.Pawn] = 0;
            LostByPlayer1[PieceType.Rook] = 0;
            LostByPlayer1[PieceType.Knight] = 0;
            LostByPlayer1[PieceType.Bishop] = 0;
            LostByPlayer1[PieceType.Queen] = 0;
            LostByPlayer1[PieceType.King] = 0;

            CapturedByPlayer2[PieceType.Pawn] = 0;
            CapturedByPlayer2[PieceType.Rook] = 0;
            CapturedByPlayer2[PieceType.Knight] = 0;
            CapturedByPlayer2[PieceType.Bishop] = 0;
            CapturedByPlayer2[PieceType.Queen] = 0;
            CapturedByPlayer2[PieceType.King] = 0;

            LostByPlayer2[PieceType.Pawn] = 0;
            LostByPlayer2[PieceType.Rook] = 0;
            LostByPlayer2[PieceType.Knight] = 0;
            LostByPlayer2[PieceType.Bishop] = 0;
            LostByPlayer2[PieceType.Queen] = 0;
            LostByPlayer2[PieceType.King] = 0;
        }

        /// <summary>
        /// Record move in match history
        /// PUSH move notation to MoveHistory // INC MoveCount
        /// </summary>
        public void RecordMove(string moveNotation)
        {
            MoveHistory.Add(moveNotation);
            MoveCount++;
        }

        /// <summary>
        /// Record piece captured by player
        /// MOV [player_captured_dict], pieceType // INC count
        /// </summary>
        public void RecordCapturedPiece(PieceColor capturedBy, PieceType pieceType)
        {
            var capturedDict = (capturedBy == PieceColor.White) ? CapturedByPlayer1 : CapturedByPlayer2;
            if (capturedDict.ContainsKey(pieceType))
                capturedDict[pieceType]++;
            else
                capturedDict[pieceType] = 1;
        }

        /// <summary>
        /// Record piece lost by player
        /// MOV [player_lost_dict], pieceType // INC count
        /// </summary>
        public void RecordLostPiece(PieceColor lostBy, PieceType pieceType)
        {
            var lostDict = (lostBy == PieceColor.White) ? LostByPlayer1 : LostByPlayer2;
            if (lostDict.ContainsKey(pieceType))
                lostDict[pieceType]++;
            else
                lostDict[pieceType] = 1;
        }

        /// <summary>
        /// End match and record winner
        /// MOV [EndTime], NOW() // MOV [Winner], color // MOV [Status], Completed
        /// </summary>
        public void EndMatch(PieceColor winner)
        {
            EndTime = DateTime.UtcNow;
            Winner = winner;
            Status = GameStatus.Completed;
        }

        /// <summary>
        /// Calculate total match duration in milliseconds
        /// MOV AX, EndTime - StartTime // RET milliseconds
        /// 0 if match still ongoing
        /// </summary>
        public int GetDurationMs()
        {
            if (Status != GameStatus.Completed)
                return 0;
            return (int)(EndTime - StartTime).TotalMilliseconds;
        }

        /// <summary>
        /// Count total pieces captured by specified player
        /// LOOP through CapturedBy dict // ADD counts // RET total
        /// </summary>
        public int GetTotalCapturedPieces(PieceColor player)
        {
            var capturedDict = (player == PieceColor.White) ? CapturedByPlayer1 : CapturedByPlayer2;
            int total = 0;
            foreach (var count in capturedDict.Values)
                total += count;
            return total;
        }

        /// <summary>
        /// Count total pieces lost by specified player
        /// LOOP through LostBy dict // ADD counts // RET total
        /// </summary>
        public int GetTotalLostPieces(PieceColor player)
        {
            var lostDict = (player == PieceColor.White) ? LostByPlayer1 : LostByPlayer2;
            int total = 0;
            foreach (var count in lostDict.Values)
                total += count;
            return total;
        }

        /// <summary>
        /// Calculate remaining pieces for player
        /// MOV AX, 16 (standard chess pieces) // SUB AX, GetTotalLostPieces()
        /// RET pieces_remaining
        /// </summary>
        public int GetRemainingPieces(PieceColor player)
        {
            return 16 - GetTotalLostPieces(player);
        }

        /// <summary>
        /// Get opponent color
        /// CMP player, White // JE return_Black
        /// RET White
        /// </summary>
        public PieceColor GetOpponentColor(PieceColor player)
        {
            return (player == PieceColor.White) ? PieceColor.Black : PieceColor.White;
        }

        /// <summary>
        /// Toggle current turn to opponent
        /// MOV [CurrentTurn], opponent_color
        /// </summary>
        public void SwitchTurn()
        {
            CurrentTurn = GetOpponentColor(CurrentTurn);
        }

        /// <summary>
        /// PUSH formatted match descriptor to output
        /// </summary>
        public override string ToString()
        {
            return $"Match: {Id} | Players: {Player1Id} vs {Player2Id} | Duration: {GetDurationMs()}ms | Status: {Status} | Winner: {Winner?.ToString() ?? "N/A"}";
        }

        #endregion
    }
}
