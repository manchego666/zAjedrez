using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// This namespace contains enumerations for the chess game model, including color types and piece types.     
/// </summary>
namespace zAjedrez.Model.Enums
{
    /// <summary>
    /// This enumeration represents the color types of chess pieces, which can be either Black or White.
    /// </summary>
    public enum  ColorType
    {
        Black,
        White
    }
    /// <summary>
    /// Especifica los tipos de piezas en un juego de ajedrez.
    /// </summary>
    public enum PieceType 
    {
        Pawn, Rook, Knight, Bishop, Queen, King,None
    }
}
