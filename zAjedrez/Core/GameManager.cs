using System;
using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;

namespace zAjedrez.Core
{
    // GAME STATE MACHINE CONTROLLER
    // PUSH Board instance + CurrentTurn flag
    // TryMovePiece: VALIDATE move -> UPDATE board -> TOGGLE turn
    // ResetGame: REINIT board + state flags
    // EndGame: SET IsGameOver flag + store Winner
    // MoveCount: TRACK number of half-moves (plies)
    // TODO: Checkmate/stalemate detection
    // TODO: Move history stack (undo operations)
    // TODO: Three-fold repetition rule
    public class GameManager
    {
        #region Properties

        public Board Board { get; private set; }         // MOV [Board], board_instance
        public PieceColor CurrentTurn { get; private set; } // MOV [CurrentTurn], color_flag
        public bool IsGameOver { get; private set; }     // MOV [IsGameOver], bool_flag
        public PieceColor Winner { get; private set; }   // MOV [Winner], color_flag
        public int MoveCount { get; private set; }       // MOV [MoveCount], int_counter

        #endregion

        #region Constructor

        // CTOR new GameManager() // POP return address
        // CALL new Board() // CALL Board.InitializeStandardSetup()
        // MOV [CurrentTurn], White (first player)
        // MOV [IsGameOver], FALSE
        // MOV [MoveCount], 0
        public GameManager()
        {
            Board = new Board();
            Board.InitializeStandardSetup();
            CurrentTurn = PieceColor.White;
            IsGameOver = false;
            Winner = PieceColor.White;
            MoveCount = 0;
        }

        #endregion

        #region Methods

        // ATTEMPT PIECE MOVEMENT
        // TEST IsGameOver // JZ game_running
        // RET MoveResult.GameOver
        // game_running: MOV piece = Board.GetPiece(fromRow, fromCol)
        // TEST piece != null // JZ return_InvalidSourceSquare
        // CMP piece.Color, CurrentTurn // JNE return_NotYourTurn
        // CALL piece.IsMoveLegal(toRow, toCol, Board.Cells) // JZ return_InvalidMove
        // CALL Board.MovePiece() // TOGGLE CurrentTurn // INC MoveCount
        // RET MoveResult.Success
        public MoveResult TryMovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (IsGameOver)
                return MoveResult.GameOver;

            Piece piece = Board.GetPiece(fromRow, fromCol);

            if (piece == null)
                return MoveResult.InvalidSourceSquare;

            if (piece.Color != CurrentTurn)
                return MoveResult.NotYourTurn;

            if (!piece.IsMoveLegal(toRow, toCol, Board.Cells))
                return MoveResult.InvalidMove;

            Piece capturedPiece = Board.GetPiece(toRow, toCol);
            Board.MovePiece(fromRow, fromCol, toRow, toCol);

            // TODO: Implement check/checkmate detection
            // TODO: Implement move history stack

            CurrentTurn = (CurrentTurn == PieceColor.White) ? PieceColor.Black : PieceColor.White;
            MoveCount++;

            return MoveResult.Success;
        }

        // RESET GAME TO INITIAL STATE
        // CALL new Board() // CALL Board.InitializeStandardSetup()
        // MOV [CurrentTurn], White
        // MOV [IsGameOver], FALSE
        // MOV [MoveCount], 0
        public void ResetGame()
        {
            Board = new Board();
            Board.InitializeStandardSetup();
            CurrentTurn = PieceColor.White;
            IsGameOver = false;
            MoveCount = 0;
        }

        // TERMINATE GAME WITH WINNER
        // MOV [IsGameOver], TRUE
        // MOV [Winner], winner_color
        public void EndGame(PieceColor winner)
        {
            IsGameOver = true;
            Winner = winner;
        }

        // PUSH formatted game state string to output
        public override string ToString()
        {
            return $"Game State - Turn: {CurrentTurn} | Moves: {MoveCount} | Game Over: {IsGameOver}";
        }

        #endregion
    }
}
