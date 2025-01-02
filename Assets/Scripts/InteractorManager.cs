using UnityEngine;

/// <summary>
/// Classe permettant de gérer les interactions du joueur.
/// </summary>
public class InteractorManager : MonoBehaviour {

    /**********************************************/
    /***               PROPRIÉTÉS               ***/
    /**********************************************/

    /// <summary>
    /// Distance entre l'interacteur et le joueur
    /// </summary>
    [SerializeField]
    private float distanceToInteract;

    /// <summary>
    /// Référence du composant permettant de récupérer le joueur cible
    /// </summary>
    [SerializeField]
    private Transform player;

    /**********************************************/
    /***              CYCLE DE VIE              ***/
    /**********************************************/

    /// <summary>
    /// Lors de chaque frame, on gère si le joueur est assez proche de l'interacteur.
    /// Si c'est le cas, on peut effectuer une intéraction avec le joueur.
    /// </summary>
    void Update() {

        // Calcul de la distance entre l'interacteur et le joueur
        float distance = Vector3.Distance(transform.position, player.position);

        // Si la distance est suffisamment petite, on peut effectuer une intéraction
        if(distance < distanceToInteract) {

            // TODO : Afficher le texte de l'UI lié à l'intéraction (Bulle de conversation)
        }
    }
}
