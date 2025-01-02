using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe pour ajuster les <see cref="Canvas"/> qu'il puisse se trouver en plein écran (UI Dynamique).
/// </summary>
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasScaler))]
public class FullScreenCanvas : MonoBehaviour {

    /**********************************************/
    /***               PROPRIÉTÉS               ***/
    /**********************************************/

    /// <summary>
    /// Référence au <see cref="RectTransform"/> attaché à ce <see cref="GameObject"/>.
    /// </summary>
    private RectTransform _rectTransform;

    /// <summary>
    /// Référence au <see cref="CanvasScaler"/> attaché à ce <see cref="GameObject"/>.
    /// </summary>
    private CanvasScaler _canvasScaler;

    /**********************************************/
    /***              CYCLE DE VIE              ***/
    /**********************************************/

    /// <summary>
    /// Méthode appelée au démarrage, après que toutes les initialisations sont effectuées.
    /// Configure les offsets du RectTransform en fonction des dimensions de l'écran et de la zone sécurisée (Safe Area).
    /// </summary>
    private void Start()  {

        // Initialisation de la propriété '_rectTransform' avec le composant 'RectTransform' attaché à ce 'GameObject'.
        _rectTransform = GetComponent<RectTransform>();

        // Récupération du CanvasScaler attaché au Canvas
        _canvasScaler = GetComponentInParent<CanvasScaler>();

        // Vérifie si le GameObject contient un composant 'RectTransform' attaché.
        ComponentUtils.Checker.CheckForMissingComponent<RectTransform>(gameObject);

        // Vérifie si le GameObject contient un composant 'CanvasScaler' attaché.
        ComponentUtils.Checker.CheckForMissingComponent<CanvasScaler>(gameObject);
    }

    /// <summary>
    /// Lors de chaque frame corrigé, ajuste le 'RectTransform' et le 'CanvasScaler' en fonction de la taille de l'écran.
    /// </summary>
    private void FixedUpdate() {

        AdjustCanvasTransform(); // Ajuste le 'RectTransform' en fonction de la taille de l'écran
        AdjustCanvasScaler(); // Ajuste le 'CanvasScaler' en fonction de la taille de l'écran
    }

    /// <summary>
    /// Méthode appelée lors de la fermeture de l'application.
    /// Reinitialise les valeurs du RectTransform.
    /// </summary>
    private void OnApplicationQuit() {

        /*
         * Vérifie si le GameObject contient un composant 'RectTransform' attaché.
         * Si c'est le cas, le composant 'RectTransform' est réinitialisé.
         */
        if(_rectTransform != null) {

            // Réinitialise la position ancrée du 'RectTransform'.
            _rectTransform.anchoredPosition = Vector2.zero;

            // Réinitialise la taille du 'RectTransform'.
            _rectTransform.sizeDelta = Vector2.zero;

            // Réinitialise l'échelle locale du 'RectTransform'.
            _rectTransform.localScale = Vector3.zero;

            // Réinitialise l'ancrage minimal du 'RectTransform'.
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);

            // Réinitialise l'ancrage maximal du 'RectTransform'.
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        }
    }

    /*****************************************/
    /***              GETTERS              ***/
    /*****************************************/

    /// <summary>
    /// Taille de l'écran en tant que <see cref="Vector2"/>.
    /// </summary>
    private Vector2 ScreenSize => new(Screen.width, Screen.height);

    /******************************************/
    /***              MÉTHODES              ***/
    /******************************************/

    /// <summary>
    /// Ajuste le 'RectTransform' en fonction de la taille de l'écran.
    /// </summary>
    private void AdjustCanvasTransform() {

        // Vérifie si le composant 'RectTransform' est attaché au GameObject, sinon on sort de la fonction.
        if(!_rectTransform) return;

        // Définit les ancres pour que le Canvas occupe toute la zone de l'écran.
        _rectTransform.anchorMin = Vector2.zero;   // Coin inférieur gauche (0,0)
        _rectTransform.anchorMax = Vector2.one;    // Coin supérieur droit (1,1)

        // Réinitialise les offsets pour que le 'RectTransform' prenne toute la place.
        _rectTransform.offsetMin = Vector2.zero;
        _rectTransform.offsetMax = Vector2.zero;

        // Ajuste la taille du 'RectTransform' en fonction de la taille de l'écran.
        _rectTransform.sizeDelta = ScreenSize;
    }

    /// <summary>
    /// Ajuste le 'CanvasScaler' en fonction de la taille de l'écran.
    /// </summary>
    private void AdjustCanvasScaler() {

        // Vérifie si le composant 'CanvasScaler' est attaché au GameObject, sinon on sort de la fonction.
        if(!_canvasScaler) return;

        // Ajuste la référence de résolution en fonction de la taille de l'écran
        _canvasScaler.referenceResolution = ScreenSize;

        /*
         * Pour chaque mode de mise à l'échelle, on peut ajuster les valeurs en fonction de la taille de l'écran ou d'autres critères.
         */
        switch(_canvasScaler.uiScaleMode) {

            /*
             * Mode de mise à l'échelle constante
             */
            case CanvasScaler.ScaleMode.ConstantPixelSize:

                // Méthode pour calculer dynamiquement le facteur de mise à l'échelle
                float scaleFactor = ScreenCalculatorUtils.CalculateScaleFactor();

                _canvasScaler.scaleFactor = scaleFactor; // Définit le facteur de mise à l'échelle
                break; // Arrêt du 'switch'

            /*
             * Mode de mise à l'échelle dynamique
             */
            case CanvasScaler.ScaleMode.ScaleWithScreenSize:

                // Méthode pour calculer dynamiquement le facteur de correspondance
                float matchValue = ScreenCalculatorUtils.CalculateMatchValue();

                _canvasScaler.matchWidthOrHeight = matchValue; // Définit le facteur de correspondance
                break; // Arrêt du 'switch'

            /*
             * Mode de mise à l'échelle physique
             */
            case CanvasScaler.ScaleMode.ConstantPhysicalSize:

                // Méthode pour calculer dynamiquement le facteur de taille physique
                float physicalSizeFactor = ScreenCalculatorUtils.CalculatePhysicalSizeFactor();

                _canvasScaler.physicalUnit = CanvasScaler.Unit.Centimeters; // Définit l'unité physique en centimetres
                _canvasScaler.scaleFactor = physicalSizeFactor; // Définit le facteur de mise à l'échelle
                break; // Arrêt du 'switch'

            // Par défaut, le mode de mise à l'échelle n'est pas pris en charge, une exception est lancée
            default: throw new InvalidOperationException("Unsupported uiScaleMode: " + _canvasScaler.uiScaleMode);
        }
    }
}