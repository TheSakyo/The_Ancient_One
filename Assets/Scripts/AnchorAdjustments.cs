/// <summary>
///  Une structure pour représenter un ajustement d'ancrage.
/// </summary>
public struct AnchorAdjustments {

    /**********************************************/
    /***               PROPRIÉTÉS               ***/
    /**********************************************/

    /// <summary>
    ///  Le décalage 'x' horizontal de l'ancrage
    /// </summary>
    public readonly float X;

    /// <summary>
    ///  Le décalage 'y' horizontal de l'ancrage
    /// </summary>
    public readonly float Y;


    /**********************************************/
    /***               CONSTRUCTEUR             ***/
    /*********************************************/

    /// <summary>
    ///  Nouvelle instance de <see cref="AnchorAdjustments"/> pour définir un ajustement d'ancrage
    /// </summary>
    /// <param name="x">Le décalage 'x' horizontal</param>
    /// <param name="y">Le décalage 'y' horizontal</param>
    public AnchorAdjustments(float x, float y) { X = x; Y = y; }
}