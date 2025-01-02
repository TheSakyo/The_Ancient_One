using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe permettant de gérer le responsive du panneau de dialogue (UI Dynamique).
/// </summary>
///
[RequireComponent(typeof(RectTransform))]
public class ResponsiveDialogue : MonoBehaviour {

    /**********************************************/
    /***               PROPRIÉTÉS               ***/
    /**********************************************/

    /// <summary>
    /// Le texte affiché dans le panneau de dialogue.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI dialogueText;

    /// <summary>
    /// Le bouton de poursuite de dialogue.
    /// </summary>
    [SerializeField]
    private Button dialogueButton;

    /// <summary>
    /// L'image associée au panneau de dialogue.
    /// </summary>
    [SerializeField]
    private Image dialogueImage;

    /// <summary>
    /// Référence au <see cref="RectTransform"/> attaché à ce <see cref="GameObject"/>.
    /// </summary>
    private RectTransform _rectTransform;

    /// <summary>
    /// Référence au <see cref="TextMeshProUGUI"/> étant le texte attaché au bouton de poursuite de ce dialogue.
    /// </summary>
    private TextMeshProUGUI _dialogueButtonLabel;


    /// <summary>
    /// Référence au <see cref="RectTransform"/> attaché au bouton de poursuite de ce dialogue.
    /// </summary>
    private RectTransform _dialogueButtonRectTransform;

    /// <summary>
    /// Référence au <see cref="RectTransform"/> attaché au message de ce dialogue.
    /// </summary>
    private RectTransform _dialogueTextRectTransform;

    /// <summary>
    /// Référence au <see cref="RectTransform"/> attaché a l'image de profil de ce dialogue.
    /// </summary>
    private RectTransform _dialogueImageRectTransform;

    /**********************************************/
    /***              CYCLE DE VIE              ***/
    /**********************************************/

    /// <summary>
    /// Méthode appelée au démarrage, après que toutes les initialisations sont effectuées.
    /// Configure les offsets du RectTransform en fonction des dimensions de l'écran et de la zone safeguard (Safe Area).
    /// </summary>
    private void Start() {

        // Initialisation des variables permettant la recherche des propriétés.
        BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

        // Initialisation de la propriété '_rectTransform' avec le composant 'RectTransform' attaché à ce 'GameObject'.
        _rectTransform = GetComponent<RectTransform>();

        /*
         * Initialisation de la propriété '_dialogueButtonRectTransform' avec le composant 'RectTransform' attaché au bouton de poursuite de ce dialogue.
         * Nous voulons aussi initialiser la propriété '_dialogueButtonLabel' avec le composant 'TextMeshProUGUI' attaché au bouton de poursuite de ce dialogue.
         */
        if(dialogueButton != null) {

            // Initialise la propriété '_dialogueButtonLabel' avec le composant 'TextMeshProUGUI'
            _dialogueButtonLabel = dialogueButton.GetComponentInChildren<TextMeshProUGUI>();

            // Initialise la propriété '_dialogueButtonRectTransform' avec le composant 'RectTransform'
            _dialogueButtonRectTransform = dialogueButton.GetComponent<RectTransform>();
        }

        // Initialisation de la propriété '_dialogueButtonRectTransform' avec le composant 'RectTransform' attaché au text de ce dialogue.
        if(dialogueText != null) _dialogueTextRectTransform = dialogueText.GetComponent<RectTransform>();

        // Initialisation de la propriété '_dialogueImageRectTransform' avec le composant 'RectTransform' attaché à l'image de profil de ce dialogue.
        if(dialogueImage != null) _dialogueImageRectTransform = dialogueImage.GetComponent<RectTransform>();

        // Vérifie si le composant 'RectTransform' est manquant et lance une exception si c'est le cas.
        ComponentUtils.Checker.CheckForMissingComponent<RectTransform>(gameObject);
    }


    /// <summary>
    /// Lors de chaque frame corrigé, ajuste le 'RectTransform' et le 'CanvasScaler' en fonction de la taille de l'écran.
    /// </summary>
    private void FixedUpdate() {

        AdjustDialoguePanel(); // Ajuste la position, la taille et l'échelle du panneau de dialogue
        AdjustDialogueElements(); // Rend les éléments de dialogue responsive
    }

    /*********************************************/
    /***              MÉTHODES                 ***/
    /*********************************************/

    /// <summary>
    /// Ajuste la position, la taille et l'échelle du panneau de dialogue.
    /// </summary>
    private void AdjustDialoguePanel() {

        _rectTransform.anchorMin = new Vector2(0.5f, 0f); // Ancrage horizontal centré, vertical en bas
        _rectTransform.anchorMax = new Vector2(0.5f, 0f); // Même ancrage haut/bas
        _rectTransform.pivot = new Vector2(0.5f, 0f);     // Pivot centré horizontalement, en bas

        _rectTransform.anchoredPosition = new Vector2(0f, 0f); // Positionnement au centre de l'écran

        float panelWidth = Screen.width; // Largeur du panneau
        float panelHeight = Screen.height * 0.22f; // Hauteur du panneau : 22% de l'écran

        // Applique les dimensions calculées de la largeur du panneau sans distortion à l'axis horizontal
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, panelWidth);

        // Applique les dimensions calculées de la hauteur du panneau sans distortion à l'axis vertical
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight);

        _rectTransform.localScale = Vector3.one; // Échelle uniforme pour éviter les déformations
    }

    /// <summary>
    /// Rend les éléments de dialogue responsive en fonction de la taille du panneau de dialogue.
    /// </summary>
    private void AdjustDialogueElements() {

        // Si le 'RectTransform' du panneau de dialogue n'existe pas, on ne fait rien
        if(!_rectTransform) return;

        /*
         * Rend le texte de ce dialogue responsive en fonction de la taille du panneau de dialogue.
         * Nous effectuons un décalage de 11% à gauche, -16% à droite et -10% vers le haut pour définir
         * son emplacement dans la zone du panneau de dialogue. Ensuite, on rend la taille de la police
         * responsive en fonction de la taille du panneau de dialogue.
         */
        if(dialogueText && _dialogueTextRectTransform) {

            /*
             * Ajuste les marges du texte en fonction de la taille du panneau de dialogue
             */
            ComponentUtils.RectTransformHandler.AdjustAnchors(_dialogueTextRectTransform, _rectTransform,
                new AnchorAdjustments(0.11f, 0), new AnchorAdjustments(0.16f, 0.1f));

            // Rend le texte responsive en fonction de la taille du panneau de dialogue
            ComponentUtils.RectTransformHandler.SyncWithAnotherRectTransform(_dialogueTextRectTransform, _rectTransform);

            // Ajuste la taille de la police dynamiquement en fonction de la taille du panneau de dialogue
            ComponentUtils.TextMeshProUGUIHandler.AdjustDynamicFontSize(dialogueText, 0.04f);
        }

        /*
         * Rend le bouton de ce dialogue responsive en fonction de la taille du panneau de dialogue.
         * Nous effectuons une marge de 85% et -2% horizontalement et de -70% et 10% verticalement pour définir
         * son emplacement dans la zone du panneau de dialogue. Ensuite, on essaie de rendre la taille du texte
         * du bouton responsive en fonction de la taille du panneau de dialogue.
         */
        if(dialogueButton && _dialogueButtonRectTransform) {

            /*
             * Ajuste les marges du bouton en fonction de la taille du panneau de dialogue
             */
            ComponentUtils.RectTransformHandler.AdjustAnchors(_dialogueButtonRectTransform, _rectTransform,
                new AnchorAdjustments(0.85f, 0.1f), new AnchorAdjustments(0.02f, 0.7f));

            // Rend le bouton responsive en fonction de la taille du panneau de dialogue
            ComponentUtils.RectTransformHandler.SyncWithAnotherRectTransform(_dialogueButtonRectTransform, _rectTransform);

            /*
             * Si le texte du bouton de dialogue existe, on le rend responsive
             * en fonction de la taille de l'écran.
             */
            if(_dialogueButtonLabel) ComponentUtils.TextMeshProUGUIHandler.AdjustDynamicFontSize(_dialogueButtonLabel, 0.04f);
        }

        /*
         * Rend l'image de profil de ce dialogue responsive en fonction de la taille du panneau de dialogue.
         * Nous effectuons une marge de -90% sur la droite pour définir son emplacement dans la zone du panneau de dialogue.
         */
        if(dialogueImage && _dialogueImageRectTransform) {

            /*
             * Ajuste les marges de l'image en fonction de la taille du panneau de dialogue
             */
            ComponentUtils.RectTransformHandler.AdjustAnchors(_dialogueImageRectTransform, _rectTransform,
                new AnchorAdjustments(0, 0), new AnchorAdjustments(0.9f, 0));

            // Rend l'image responsive en fonction de la taille du panneau de dialogue
            ComponentUtils.RectTransformHandler.SyncWithAnotherRectTransform(_dialogueImageRectTransform, _rectTransform);
        }
    }
}