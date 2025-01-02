using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Effet de texte affichant les caractères un par un et réglages du dépassement du texte (Effet UI).
/// </summary>
public class ScrollTextEffect : MonoBehaviour {

    /**********************************************/
    /***               PROPRIÉTÉS               ***/
    /**********************************************/

    /// <summary>
    /// Le composant TextMeshPro pour afficher le texte
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI textMeshPro;

    /// <summary>
    /// Le composant RectTransform de l'image de fond (bulle de conversation)
    /// </summary>
    [SerializeField]
    private RectTransform conversationBackground;

    /// <summary>
    /// Le bouton pour continuer l'affichage du texte
    /// </summary>
    [SerializeField]
    private Button continueButton;


    /// <summary>
    /// Le texte complet que vous voulez afficher
    /// </summary>
    [SerializeField]
    private string fullText;

    /// <summary>
    /// La vitesse de l'écriture
    /// </summary>
    [SerializeField]
    private float typingSpeed = 0.05f;

    /// <summary>
    /// Booléen pour savoir si le texte est complet
    /// </summary>
    private bool _textComplete;

    /// <summary>
    /// Le texte qu'il faut afficher dans le composant 'TextMeshProUGUI'
    /// </summary>
    private string _displayedText = string.Empty;

   /// <summary>
   ///  Le texte restant à afficher dans le composant 'TextMeshProUGUI'
   /// </summary>
   private string _remainingText = string.Empty;

   /// <summary>
   /// Le composant 'TextMeshProUGUI' temporaire pour la mesure des lignes
   /// </summary>
   private TextMeshProUGUI _tempTextMeshPro;

   /// <summary>
   /// Un booléen pour savoir si le texte est en cours d'affichage
   /// </summary>
   private bool _isTyping;

    /**********************************************/
    /***              CYCLE DE VIE              ***/
    /**********************************************/

    /// <summary>
    /// Lors du lancement du jeu, On ajoute un événement sur le bouton "Continuer".
    /// Ensuite, on remplace les \n par des retours à la ligne, on essaie de trouver le texte restant
    /// et on lance l'affichage du texte.
    /// </summary>
    void Start() {

        // Création du 'TextMeshProUGUI' temporaire utilisant une copie du 'TextMeshProUGUI' principal
        if(textMeshPro) _tempTextMeshPro = ComponentUtils.TextMeshProUGUIHandler.CloneWithTransform(textMeshPro, true);

        continueButton.gameObject.SetActive(false); // Cache le bouton "Continuer"
        continueButton.onClick.AddListener(OnContinueButtonPressed); // Ajoute un événement sur le bouton

        fullText = fullText.Replace(@"\n", "\n"); // Remplace les \n par des retours à la ligne
        _remainingText = fullText; // Enregistre le texte à afficher dans le texte restant à afficher
        StartCoroutine(ShowText()); // Commence l'affichage du texte
    }

    private void FixedUpdate() {

        /*
         * Si le texte est en cours d'affichage et qu'il n'y a pas de 'TextMeshProUGUI' temporaire,
         * on crée un 'TextMeshProUGUI' temporaire utilisant une copie du 'TextMeshProUGUI' principal
         */
        if(_isTyping && !_tempTextMeshPro && textMeshPro)
            _tempTextMeshPro = ComponentUtils.TextMeshProUGUIHandler.CloneWithTransform(textMeshPro, true);

        // Si le texte n'est pas en cours d'affichage et qu'il y a un 'TextMeshProUGUI' temporaire, on le détruit
        if(!_isTyping && _tempTextMeshPro) Destroy(_tempTextMeshPro.gameObject);

    }

    /********************************************/
    /***         GETTERS & SETTERS            ***/
    /*******************************************/

    /**
      * <summary>
      * Gets or sets the continue button.
      * </summary>
      */
    public Button ContinueButton { get => continueButton; set => continueButton = value; }

    /// <summary>
    /// Récupère ou définit le texte complet
    /// </summary>
    public string FullText { get => fullText; set => fullText = value; }

    /// <summary>
    /// Récupère ou définit la vitesse de l'écriture
    /// </summary>
    public float TypingSpeed { get => typingSpeed; set => typingSpeed = value; }

    /********************************************/
    /***              ÉVÉNEMENTS              ***/
    /*******************************************/

    /// <summary>
    /// Lorsque le bouton "Continuer" est appuyé, on affiche la suite du texte
    /// </summary>
    private void OnContinueButtonPressed() {

        Debug.Log("work");
        
        /*
         * Si le texte est complet, on affiche la partie suivante
         */
        if(_textComplete) {

            textMeshPro.text = string.Empty;  // Supprime le texte actuellement affiché
            _remainingText = "... " + _remainingText; // Ajoute "..." au début de la deuxième partie

            continueButton.gameObject.SetActive(false); // Cache le bouton "Continuer"

            // Commence l'affichage du texte avec la partie suivante
            StartCoroutine(ShowText(true));
        }
    }

    /******************************************/
    /***              MÉTHODES              ***/
    /******************************************/

    /// <summary>
    /// Affiche progressivement le texte caractère par caractère
    /// </summary>
    ///
    private IEnumerator ShowText(bool isContinuation = false) {

        _isTyping = true; // Définit le texte étant en cours d'affichage

        _displayedText = string.Empty; // Vide le texte à afficher avant de commencer
        _textComplete = false; // Définit le texte n'étant pas complet

        // On initialise le texte restant au texte complet si ce n'est pas une continuation
        if(!isContinuation) _remainingText = fullText;

        // Définit une variable sauvegardée la ligne actuelle du texte à afficher (vide par défaut)
        string currentLine = string.Empty;

        // On boucle sur chaque caractère du texte restant
        while(_remainingText.Length > 0) {

            // Ajoute un caractère à la fois au texte à afficher
            _displayedText += _remainingText[0];

            // Ajoute un caractère à la fois à la ligne actuelle sauvegardée
            currentLine += _remainingText[0];

            /*
             * Si nous sommes en fin de mot et que le texte est trop long, on ajoute un retour à la ligne
             */
            if(_displayedText.EndsWith(" ") && IsTextOverflowingWithoutBreakWord(currentLine)) _displayedText += "\n"; // Ajoute un retour à la ligne

            textMeshPro.text = _displayedText; // Met à jour le texte affiché
            _remainingText = _remainingText.Substring(1); // Retire le premier caractère du texte restant

            /*
             * Si le texte dépasse la bulle de conversation, par rapport à sa largeur et/ou sa hauteur, si c'est le cas,
             * on vérifie également si le bouton "Continuer" n'est pas affiché, dans ce cas, on affiche le bouton.
             */
            if(IsTextOverflowingWithoutBreakWord(currentLine, true) && !continueButton.gameObject.activeSelf) {

                    // Ajoute "..." à la fin du texte, s'il n'est pas terminé par un retour à la ligne
                    if(!textMeshPro.text.EndsWith("\n")) textMeshPro.text += "...";

                    continueButton.gameObject.SetActive(true);  // Affiche le bouton "Continuer"

                    _isTyping = false; // Définit le texte n'étant pas en cours d'affichage
                    _textComplete = true; // Définit le texte étant complet

                    yield break; // Stoppe la coroutine pour attendre l'appui du bouton
            }

            // Vérifie si texte affiché contient un retour à la ligne, alors on vide la ligne actuelle sauvegardée
            if(textMeshPro.text.EndsWith("\n")) currentLine = string.Empty;

            // Attendre un certain temps avant d'ajouter le prochain caractère
            yield return new WaitForSeconds(typingSpeed);
        }

        _isTyping = false; // Définit le texte n'est plus en cours d'affichage
        _textComplete = true; // Définit le texte étant complet
    }

    /// <summary>
    /// Vérifie si un texte et/ou l'ensemble du texte de la 'bulle' dépasse sa taille initiale
    /// dans la 'bulle' de conversation, par rapport à sa largeur et/ou sa hauteur.
    /// Si le texte est en fin de ligne, nous vérifierons aussi si le texte est en fin de mot.
    /// Ceci permet de garantir que le mot ne soit pas coupé.
    /// </summary>
    /// <param name="text">Le texte à verifier</param>
    /// <param name="endLine">Si le texte est en fin de ligne</param>
    /// <returns>'True' Si le texte déborde sans coupure de mot, sinon 'False'</returns>
    private bool IsTextOverflowingWithoutBreakWord(string text, bool endLine = false) {

        // Vérifie si le texte se termine par un retour à la ligne
        bool lineBreak = text.EndsWith("\n");

        /*
         * Vérifie si le texte est en fin de ligne, mais qu'il
         * n'est pas en fin de mot, alors on retourne 'false'
         */
        if(endLine && !text.EndsWith(" ") && !lineBreak) return false;

        // Récupère la largeur maximale de la bulle de conversation
        float maxWidth = textMeshPro.rectTransform.rect.width;

        // Récupère la hauteur maximale de la bulle de conversation
        float maxHeight = textMeshPro.rectTransform.rect.height;

        // Vérifie si la largeur du texte en question dépasse la largeur maximale de la 'bulle'
        bool isWidthOverflow = IsLineOverflowing(text, maxWidth);

        /*
         * Vérifie si la hauteur de l'ensemble du texte de la 'bulle' dépasse la hauteur maximale (à partir d'une
         * largeur de 1250 pixels, on soustrait 50 pixels de la hauteur pour évider de compter les marges)
         */
        bool isHeightOverflow = textMeshPro.preferredHeight >= (maxWidth >= 1250 ? maxHeight - 50 : maxHeight);

        // Renvoie la comparaison du dépassement du texte et celle du fond de la conversation en fonction de la valeur de 'endLine'
        return endLine ? (isWidthOverflow || lineBreak) && isHeightOverflow : isWidthOverflow;
    }

    /// <summary>
    /// Vérifie si une ligne spécifique dépasse une certaine largeur maximale.
    /// Nous nous aidons d'un 'TextMeshProUGUI' temporaire pour effectuer la mesure
    /// </summary>
    /// <param name="line">La ligne du texte à vérifier</param>
    /// <param name="maxWidth">La largeur maximale à comparer</param>
    /// <returns>'True' si la ligne dépasse la largeur maximale, sinon 'False'</returns>
    private bool IsLineOverflowing(string line, float maxWidth) {

        /*
         * Vérifie si le 'TextMeshProUGUI' temporaire n'existe pas,
         * on renvoie alors 'false' (faux)
         */
        if(!_tempTextMeshPro) return false;

        // Met à jour la ligne dans le 'TextMeshProUGUI' temporaire
        _tempTextMeshPro.text = line;

        /*
         * Vérifie si la largeur préférée de la ligne dépasse la largeur maximale (à partir de 1250 pixels,
         * on soustrait 100 pixels pour évider de compter les marges, sinon on ajoute 100 pixels)
         */
        return _tempTextMeshPro.preferredWidth >= (maxWidth >= 1250 ?  maxWidth - 100 : maxWidth + 100);
    }
}
