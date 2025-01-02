using System;
using UnityEngine;

/// <summary>
/// Utilitaire pour calculer des facteurs de mise à l'échelle en fonction de la taille de l'écran.
/// </summary>
public static class ScreenCalculatorUtils {

    /// <summary>
    /// Calcule le facteur de mise à l'échelle en fonction de la taille de l'écran.
    /// </summary>
    /// <returns>Le facteur de mise à l'échelle calculée.</returns>
   public static float CalculateScaleFactor() {

        // Ajuste la mise à l'échelle en fonction de la largeur de l'écran
        float screenWidth = Screen.width;

        /*
         * Renvoie le calcul du facteur de mise à l'échelle en fonction de la largeur de l'écran.
         */
        return screenWidth switch {

            < 800 => 0.5f, // Si la largeur de l'écran est inférieure à 800, on renvoie '0.5'
            >= 800 and < 1200 => 1f, // Si la largeur de l'écran est comprise entre 800 et 1200, on renvoie '1'
            >= 1200 => 1.5f, // Si la largeur de l'écran est supérieure à 1200, on renvoie '1.5'

            // Renvoie une exception, si la largeur de l'écran est inattendue
            _ => throw new ArgumentException("Unexpected screenWidth: " + screenWidth)
        };
    }

    /// <summary>
    /// Calcule le facteur de correspondance en fonction de la taille de l'écran.
    /// </summary>
    /// <returns>Le facteur de correspondance calculé.</returns>
   public static float CalculateMatchValue() {

        // Ajuste 'matchWidthOrHeight' en fonction du ratio de l'écran
        float aspectRatio = (float)Screen.width / Screen.height;

        /*
         * Renvoi le calcul du facteur de correspondance en fonction du ratio de l'écran.
         */
        return aspectRatio switch {

            < 1f => 1f, // Si l'aspect ratio est inférieur à 1, on renvoie '1'
            >= 1f and <= 1.5f => 0.5f, // Si l'aspect ratio est comprise entre 1 et 1.5, on renvoie '0.5'
            > 1.5f => 0f, // Si l'aspect ratio est supérieure à 1.5, on renvoie '0'

            // Renvoie une exception, si l'aspect ratio est inattendu
            _ => throw new ArgumentException("Unexpected aspect ratio: " + aspectRatio)
        };
    }

   /// <summary>
   /// Calcule le facteur de taille physique en fonction de la 'DPI' (Dots Per Inch) de l'écran.
   /// </summary>
   /// <returns>Le facteur de taille physique calculé.</returns>
   public static float CalculatePhysicalSizeFactor() {

        float dpi = Screen.dpi; // Ajuste la taille physique en fonction de la 'DPI' (dots per inch)

        /*
         * Renvoi le calcul du facteur de taille physique en fonction de la 'DPI' de l'écran.
         */
        return dpi switch {

            < 160 => 0.8f, // Si la 'DPI' est inférieure à 160, on renvoie '0.8'
            >= 160 and < 320 => 1f, // Si la 'DPI' est comprise entre 160 et 320, on renvoie '1'
            >= 320 => 1.2f, // Si la 'DPI' est supérieure à 320, on renvoie '1.2'

            // Renvoie une exception, si la 'DPI' est inattendue
            _ => throw new ArgumentException("Unexpected dpi: " + dpi)
        };
    }
}