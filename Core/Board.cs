using System;
using zAjedrez.Model.Contract;
using zAjedrez.Model.Struct;
using zAjedrez.Model.Enums;

namespace zAjedrez.Core
{
    // Clase que representa el estado del tablero de ajedrez 8x8
    // Maneja la inicialización de piezas y el estado general del juego
    public class Board
    {
        #region Properties

        public Piece[,] Cells { get; private set; }
        public const int BOARD_SIZE = 8;

        #endregion

        #region Constructor

        public Board()
        {
            Cells = new Piece[BOARD_SIZE, BOARD_SIZE];
            InitializeEmptyBoard();
        }

        #endregion

        #region Methods

        // Inicializa un tablero vacío
        private void InitializeEmptyBoard()
        {
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    Cells[row, col] = null;
                }
            }
        }

        // Coloca una pieza en una posición del tablero
        public void PlacePiece(Piece piece, int row, int column)
        {
            if (IsValidPosition(row, column))
                Cells[row, column] = piece;
        }

        // Obtiene la pieza en una posición
        public Piece GetPiece(int row, int column)
        {
            if (IsValidPosition(row, column))
                return Cells[row, column];
            return null;
        }

        // Mueve una pieza de una posición a otra
        public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (!IsValidPosition(fromRow, fromCol) || !IsValidPosition(toRow, toCol))
                return false;

            Piece piece = Cells[fromRow, fromCol];
            if (piece == null)
                return false;

            // Registra que la pieza se ha movido
            piece.HasMoved = true;
            piece.Row = toRow;
            piece.Column = toCol;

            Cells[toRow, toCol] = piece;
            Cells[fromRow, fromCol] = null;

            return true;
        }

        // Elimina una pieza del tablero
        public bool RemovePiece(int row, int column)
        {
            if (IsValidPosition(row, column))
            {
                Cells[row, column] = null;
                return true;
            }
            return false;
        }

        // Verifica si una posición es válida en el tablero
        public bool IsValidPosition(int row, int column)
        {
            return row >= 0 && row < BOARD_SIZE && column >= 0 && column < BOARD_SIZE;
        }

        // Inicializa el tablero con la posición estándar de ajedrez
        public void InitializeStandardSetup()
        {
            InitializeEmptyBoard();

            // Peones blancos (fila 6)
            for (int col = 0; col < BOARD_SIZE; col++)
                PlacePiece(new PawnPiece(PieceColor.White, 6, col), 6, col);

            // Peones negros (fila 1)
            for (int col = 0; col < BOARD_SIZE; col++)
                PlacePiece(new PawnPiece(PieceColor.Black, 1, col), 1, col);

            // Torres blancas
            PlacePiece(new RookPiece(PieceColor.White, 7, 0), 7, 0);
            PlacePiece(new RookPiece(PieceColor.White, 7, 7), 7, 7);

            // Torres negras
            PlacePiece(new RookPiece(PieceColor.Black, 0, 0), 0, 0);
            PlacePiece(new RookPiece(PieceColor.Black, 0, 7), 0, 7);

            // Caballos blancos
            PlacePiece(new KnightPiece(PieceColor.White, 7, 1), 7, 1);
            PlacePiece(new KnightPiece(PieceColor.White, 7, 6), 7, 6);

            // Caballos negros
            PlacePiece(new KnightPiece(PieceColor.Black, 0, 1), 0, 1);
            PlacePiece(new KnightPiece(PieceColor.Black, 0, 6), 0, 6);

            // Alfiles blancos
            PlacePiece(new BishopPiece(PieceColor.White, 7, 2), 7, 2);
            PlacePiece(new BishopPiece(PieceColor.White, 7, 5), 7, 5);

            // Alfiles negros
            PlacePiece(new BishopPiece(PieceColor.Black, 0, 2), 0, 2);
            PlacePiece(new BishopPiece(PieceColor.Black, 0, 5), 0, 5);

            // Reinas blancas
            PlacePiece(new QueenPiece(PieceColor.White, 7, 3), 7, 3);

            // Reinas negras
            PlacePiece(new QueenPiece(PieceColor.Black, 0, 3), 0, 3);

            // Reyes blancos
            PlacePiece(new KingPiece(PieceColor.White, 7, 4), 7, 4);

            // Reyes negros
            PlacePiece(new KingPiece(PieceColor.Black, 0, 4), 0, 4);
        }

        // Imprime el tablero en la consola para debugging
        public void PrintBoard()
        {
            Console.WriteLine("\n  0 1 2 3 4 5 6 7");
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                Console.Write(row + " ");
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    Piece piece = Cells[row, col];
                    if (piece == null)
                        Console.Write(". ");
                    else
                        Console.Write($"{(piece.Color == PieceColor.White ? piece.Type.ToString()[0] : piece.Type.ToString()[0].ToString().ToLower())} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        #endregion
    }
}
