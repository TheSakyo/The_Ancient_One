using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorManager : MonoBehaviour {

    /**********************************************/
    /***               PROPRIÉTÉS               ***/
    /**********************************************/

    [SerializeField]
    private float distanceToInteract;    // Distance entre l'ennemi et le joueur pour pouvoir interagir

    [SerializeField]
    private Transform player;           // Référence du composant permettant de récupérer le joueur cible

    /**********************************************/
    /***              CYCLE DE VIE              ***/
    /**********************************************/

    void Update() {

        // Calculate the distance between enemy's and the player
        float distance = Vector3.Distance(transform.position, player.position);

        // If the player is within follow distance, move towards the player
        if(distance < distanceToInteract) {


        }
    }
}
