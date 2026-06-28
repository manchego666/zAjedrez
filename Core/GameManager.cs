using System;
using zAjedrez.Model.Enums;
using zAjedrez.Model.Contract;

namespace zAjedrez.Core
{
    // Gestor principal del juego
    // Controla el flujo del juego, turnos, movimientos y estado general
    public class GameManager
    {
        #region Properties

        public Board Board { get; private set; }
        public PieceColor CurrentTurn { get; private set; }
        public bool IsGameOver { get; private set; }
        public PieceColor Winner { get; private set; }
        public int MoveCount { get; private set; }

        #endregion

        #region Constructor

        public GameManager()
        {
            Board = new Board();
            Board.InitializeStandardSetup();
            CurrentTurn = PieceColor.White;
            IsGameOver = false;
            Winner = PieceColor.White; // Por defecto
            MoveCount = 0;
        }

        #endregion

        #region Methods

        // Intenta realizar un movimiento
        public MoveResult TryMovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            // Validar estado del juego
            if (IsGameOver)
                return MoveResult.GameOver;

            // Obtener la pieza a mover
            Piece piece = Board.GetPiece(fromRow, fromCol);

            // Validar que existe pieza en la posición origen
            if (piece == null)
                return MoveResult.InvalidSourceSquare;

            // Validar que es el turno del jugador correcto
            if (piece.Color != CurrentTurn)
                return MoveResult.NotYourTurn;

            // Validar que el movimiento es legal según las reglas de la pieza
            if (!piece.IsMoveLegal(toRow, toCol, Board.Cells))
                return MoveResult.InvalidMove;

            // Realizar el movimiento
            Piece capturedPiece = Board.GetPiece(toRow, toCol);
            Board.MovePiece(fromRow, fromCol, toRow, toCol);

            // TODO: Implementar validación de jaque y jaque mate
            // TODO: Implementar historial de movimientos para deshacer

            // Cambiar turno
            CurrentTurn = (CurrentTurn == PieceColor.White) ? PieceColor.Black : PieceColor.White;
            MoveCount++;

            return MoveResult.Success;
        }

        // Reinicia el juego
        public void ResetGame()
        {
            Board = new Board();
            Board.InitializeStandardSetup();
            CurrentTurn = PieceColor.White;
            IsGameOver = false;
            MoveCount = 0;
        }

        // Termina el juego con un ganador
        public void EndGame(PieceColor winner)
        {
            IsGameOver = true;
            Winner = winner;
        }

        // Retorna información del estado actual del juego
        public override string ToString()
        {
            return $"Game State - Turn: {CurrentTurn} | Moves: {MoveCount} | Game Over: {IsGameOver}";
        }

        #endregion
    }
}
