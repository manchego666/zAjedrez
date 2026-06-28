using System;
using zAjedrez.Model.Contract;
using zAjedrez.Model.Struct;
using zAjedrez.Model.Enums;

namespace zAjedrez.Core
{
    // CHESS BOARD STATE STRUCT (8x8 grid)
    // ALLOC Cells[8,8] array of Piece objects
    // InitializeStandardSetup: PLACE pieces in starting positions per chess rules
    // PlacePiece: PUSH piece to Cells[row,col]
    // GetPiece: RET Cells[row,col] reference
    // MovePiece: POP from [fromRow,fromCol] -> PUSH to [toRow,toCol]
    // RemovePiece: CLEAR Cells[row,col]
    // IsValidPosition: TEST 0 <= row,col <= 7 // RET boundary_flag
    public class Board
    {
        #region Properties

        public Piece[,] Cells { get; private set; }  // MOV [Cells], array_8x8_of_Piece
        public const int BOARD_SIZE = 8;            // MOV BOARD_SIZE, 8 (const)

        #endregion

        #region Constructor

        // CTOR new Board() // POP return address
        // ALLOC Cells = new Piece[8,8]
        // CALL InitializeEmptyBoard()
        public Board()
        {
            Cells = new Piece[BOARD_SIZE, BOARD_SIZE];
            InitializeEmptyBoard();
        }

        #endregion

        #region Methods

        // INITIALIZE EMPTY BOARD (all squares = null)
        // LOOP row=0 to 7 // LOOP col=0 to 7 // MOV Cells[row,col], null
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

        // PLACE PIECE AT POSITION
        // CALL IsValidPosition(row, column) // JZ invalid_position
        // MOV Cells[row,col], piece
        public void PlacePiece(Piece piece, int row, int column)
        {
            if (IsValidPosition(row, column))
                Cells[row, column] = piece;
        }

        // GET PIECE AT POSITION
        // CALL IsValidPosition(row, column) // JZ return_null
        // RET Cells[row,col]
        public Piece GetPiece(int row, int column)
        {
            if (IsValidPosition(row, column))
                return Cells[row, column];
            return null;
        }

        // MOVE PIECE FROM SOURCE TO DEST
        // CALL IsValidPosition(fromRow,fromCol) AND IsValidPosition(toRow,toCol) // JZ invalid
        // MOV piece = Cells[fromRow,fromCol] // TEST piece != null // JZ no_piece
        // MOV piece.HasMoved, TRUE // MOV piece.Row, toRow // MOV piece.Column, toCol
        // MOV Cells[toRow,toCol], piece
        // MOV Cells[fromRow,fromCol], null // RET true
        public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (!IsValidPosition(fromRow, fromCol) || !IsValidPosition(toRow, toCol))
                return false;

            Piece piece = Cells[fromRow, fromCol];
            if (piece == null)
                return false;

            piece.HasMoved = true;
            piece.Row = toRow;
            piece.Column = toCol;

            Cells[toRow, toCol] = piece;
            Cells[fromRow, fromCol] = null;

            return true;
        }

        // REMOVE PIECE FROM POSITION
        // CALL IsValidPosition(row, column) // JZ invalid
        // MOV Cells[row,col], null // RET true
        public bool RemovePiece(int row, int column)
        {
            if (IsValidPosition(row, column))
            {
                Cells[row, column] = null;
                return true;
            }
            return false;
        }

        // VALIDATE BOARD POSITION
        // TEST 0 <= row < 8 AND 0 <= column < 8
        // JL out_of_bounds // RET flag
        public bool IsValidPosition(int row, int column)
        {
            return row >= 0 && row < BOARD_SIZE && column >= 0 && column < BOARD_SIZE;
        }

        // INITIALIZE STANDARD CHESS SETUP
        // CALL InitializeEmptyBoard() first
        // LOOP col=0 to 7: PlacePiece(PawnPiece WHITE @ [6,col], PawnPiece BLACK @ [1,col])
        // PlacePiece(RookPiece WHITE @ [7,0] + [7,7])
        // PlacePiece(RookPiece BLACK @ [0,0] + [0,7])
        // PlacePiece(KnightPiece WHITE @ [7,1] + [7,6])
        // PlacePiece(KnightPiece BLACK @ [0,1] + [0,6])
        // PlacePiece(BishopPiece WHITE @ [7,2] + [7,5])
        // PlacePiece(BishopPiece BLACK @ [0,2] + [0,5])
        // PlacePiece(QueenPiece WHITE @ [7,3], BLACK @ [0,3])
        // PlacePiece(KingPiece WHITE @ [7,4], BLACK @ [0,4])
        public void InitializeStandardSetup()
        {
            InitializeEmptyBoard();

            for (int col = 0; col < BOARD_SIZE; col++)
                PlacePiece(new PawnPiece(PieceColor.White, 6, col), 6, col);

            for (int col = 0; col < BOARD_SIZE; col++)
                PlacePiece(new PawnPiece(PieceColor.Black, 1, col), 1, col);

            PlacePiece(new RookPiece(PieceColor.White, 7, 0), 7, 0);
            PlacePiece(new RookPiece(PieceColor.White, 7, 7), 7, 7);
            PlacePiece(new RookPiece(PieceColor.Black, 0, 0), 0, 0);
            PlacePiece(new RookPiece(PieceColor.Black, 0, 7), 0, 7);

            PlacePiece(new KnightPiece(PieceColor.White, 7, 1), 7, 1);
            PlacePiece(new KnightPiece(PieceColor.White, 7, 6), 7, 6);
            PlacePiece(new KnightPiece(PieceColor.Black, 0, 1), 0, 1);
            PlacePiece(new KnightPiece(PieceColor.Black, 0, 6), 0, 6);

            PlacePiece(new BishopPiece(PieceColor.White, 7, 2), 7, 2);
            PlacePiece(new BishopPiece(PieceColor.White, 7, 5), 7, 5);
            PlacePiece(new BishopPiece(PieceColor.Black, 0, 2), 0, 2);
            PlacePiece(new BishopPiece(PieceColor.Black, 0, 5), 0, 5);

            PlacePiece(new QueenPiece(PieceColor.White, 7, 3), 7, 3);
            PlacePiece(new QueenPiece(PieceColor.Black, 0, 3), 0, 3);

            PlacePiece(new KingPiece(PieceColor.White, 7, 4), 7, 4);
            PlacePiece(new KingPiece(PieceColor.Black, 0, 4), 0, 4);
        }

        // PRINT BOARD TO CONSOLE (debugging)
        // LOOP row,col: PUSH piece symbol to stdout
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
