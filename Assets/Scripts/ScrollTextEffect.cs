using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollTextEffect : MonoBehaviour {

    /**********************************************/
    /***               PROPRIÉTÉS               ***/
    /**********************************************/

    [SerializeField]
    private TextMeshProUGUI textMeshPro;                // Référence au TextMeshPro pour afficher le texte

    [SerializeField]
    private RectTransform conversationBackground;      // Référence au RectTransform de l'image de fond (bulle)

    [SerializeField]
    private Button continueButton;                      // Le bouton pour continuer l'affichage du texte

    [SerializeField]
    private string fullText;                            // Le texte complet que vous voulez afficher

    [SerializeField]
    private float typingSpeed = 0.05f;                   // La vitesse de l'écriture

    private bool _textComplete;                         // Pour vérifier si le texte est complet

    private string _displayedText = "";                 // Le texte déjà affiché

    private string _remainingText = "";                 // Le texte restant à afficher

    /**********************************************/
    /***              CYCLE DE VIE              ***/
    /**********************************************/

    /**
     * Lors du lancement du jeu, on recherche le composant RectTransform du texte
     */
    void Start() {

        continueButton.gameObject.SetActive(false); // Cacher le bouton au départ
        continueButton.onClick.AddListener(OnContinueButtonPressed); // Ajout de l'écouteur de clic une fois

        fullText = fullText.Replace(@"\n", "\n"); // Remplace les '\n' dans le texte par de véritables retours à la ligne
        _remainingText = fullText; // Le texte restant est tout le texte au début
        StartCoroutine(ShowText()); // Commence l'affichage du texte
    }

    /********************************************/
    /***         GETTERS & SETTERS            ***/
    /*******************************************/

    /**
     * Récupère ou définit le texte complet
     */
    public string FullText { get => fullText; set => fullText = value; }

    /**
     * Récupère ou définit la vitesse de l'écriture
     */
    public float TypingSpeed { get => typingSpeed; set => typingSpeed = value; }

    /********************************************/
    /***              ÉVÉNEMENTS              ***/
    /*******************************************/

    /**
     * Lorsque le bouton "Continuer" est appuyé
     */
    public void OnContinueButtonPressed() {

        // Si le texte est complet, affiche la partie suivante
        if(_textComplete) {

            textMeshPro.text = "";  // Supprime le texte
            _remainingText = "... " + _remainingText; // Ajoute "..." au début de la deuxième partie

            continueButton.gameObject.SetActive(false); // Cache le bouton
            StartCoroutine(ShowText(true));  // true signifie qu'on affiche la suite du texte
        }
    }

    /******************************************/
    /***              MÉTHODES              ***/
    /******************************************/

    /**
     * Affiche progressivement le texte caractère par caractère
     */
    private IEnumerator ShowText(bool isContinuation = false) {

        _displayedText = ""; // Vide le texte affiché
        _textComplete = false; // Le texte n'est pas complet

        // On initialise le texte restant au complet si ce n'est pas une continuation
        if(!isContinuation) _remainingText = fullText;

        // On boucle sur chaque caractère du texte restant
        while(_remainingText.Length > 0) {

            _displayedText += _remainingText[0]; // Ajoute un caractère à la fois
            textMeshPro.text = _displayedText; // Met à jour le texte affiché
            _remainingText = _remainingText.Substring(1); // Retire le premier caractère du texte restant

            bool spaceDetected = _displayedText.EndsWith(" "); // Vérifie si un espace a été rencontré


            Debug.Log(IsTextOverflowing());
            
            // Vérifie si le texte dépasse la bulle
            if(spaceDetected && IsTextOverflowing() && !continueButton.gameObject.activeSelf) {

                textMeshPro.text += "...";  // Ajoute "..." à la fin du texte
                continueButton.gameObject.SetActive(true);  // Affiche le bouton*
                _textComplete = true; // Le texte est maintenant complet

                yield break; // Stoppe la coroutine pour attendre l'appui du bouton
            }

            yield return new WaitForSeconds(typingSpeed); // Attendre un certain temps avant d'ajouter le prochain caractère
        }

        _textComplete = true; // Le texte est maintenant complet
    }

    /**
     * Vérifie si le texte dépasse la 'bulle' de conversation, par rapport 0 sa largeur ou sa hauteur
     */
    private bool IsTextOverflowing() {

        /*
         * Vérifie si le texte dépasse la hauteur ou la largeur de la conversation
         */
        return textMeshPro.preferredHeight >= conversationBackground.rect.height &&
               textMeshPro.preferredWidth >= conversationBackground.rect.width;
    }
}
