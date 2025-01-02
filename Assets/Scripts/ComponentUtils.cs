using System.Reflection;
using TMPro;
using UnityEngine;

/// <summary>
/// Un utilitaire pour gérer les composants (<see cref="Component"/>) du jeu.
/// </summary>
public static class ComponentUtils {

    /// <summary>
    /// Un utilitaire pour vérifier si un composant est manquant dans un <see cref="GameObject"/>.
    /// </summary>
    public static class Checker {

        /// <summary>
        /// Vérifie si le composant spécifié sur le <see cref="GameObject"/> est manquant (null).
        /// Dans ce cas : On lance une exception
        /// </summary>
        /// <typeparam name="T">Le type du composant à vérifier.</typeparam>
        /// <param name="gameObject">Le <see cref="GameObject"/> sur lequel vérifier la présence du composant.</param>
        /// <exception cref="MissingComponentException">Exception lancée si la propriété est manquante.</exception>
        public static void CheckForMissingComponent<T>(GameObject gameObject) where T : Component {

            // Récupère le composant du type spécifié attaché au GameObject
            T component = gameObject.GetComponent<T>();

            // Si le composant de la propriété existe et n'est pas null, on sort de la fonction
            if(component != null) return;

            // Sinon, on construit un message d'erreur dynamiquement en fonction du type de composant manquant
            string errorMessage = $"{typeof(T).Name} is missing on {gameObject.name}.";

            Debug.LogError(errorMessage); // Affichage du message d'erreur dans la console
            throw new MissingComponentException(errorMessage); // Lancement de l'exception avec le message d'erreur dynamique
        }

        /// <summary>
        /// Vérifie si le champ spécifié dans un <see cref="MonoBehaviour"/> est manquant (null).
        /// Dans ce cas : On lance une exception
        /// </summary>
        /// <typeparam name="T">Le type du champ à vérifier.</typeparam>
        /// <param name="monoBehaviour">Le <see cref="MonoBehaviour"/> sur lequel vérifier la présence du champ.</param>
        /// <param name="fieldName">Le nom du champ à vérifier.</param>
        /// <param name="bindingFlags">Les flags de liaison pour la recherche du champ.</param>
        /// <exception cref="MissingComponentException">Exception lancée si le champ est manquant.</exception>
        public static void CheckForMissingField<T>(MonoBehaviour monoBehaviour, string fieldName, BindingFlags bindingFlags) where T : Component {

            /*
             * Utilise la réflexion pour obtenir la champ spécifiée dans le 'MonoBehaviour'
             */
            FieldInfo property = monoBehaviour.GetType().GetField(fieldName, bindingFlags);

            // Message d'erreur dynamiquement en cas de champ introuvable
            string notFoundMessage = $"Field {fieldName} not found in {monoBehaviour.GetType().Name}.";

            // Message d'erreur dynamiquement en cas de champ manquante
            string missingMessage = $"{fieldName} is missing in {monoBehaviour.GetType().Name}.";

            // Essaie d'obtenir la valeur du champ, si elle existe, sinon on envoie 'null'
            object valueField = property != null ? property.GetValue(monoBehaviour) : null;

            // Si la valeur de champ existe et n'est pas null, on sort de la fonction
            if(valueField != null) return;

            /*
             * Sinon, on construit un message d'erreur dynamiquement en fonction
             * du type d'erreur (champ introuvable ou manquante)
             */
            string errorMessage = property != null ? missingMessage : notFoundMessage;

            Debug.LogError(errorMessage); // Affiche un message d'erreur dans la console
            throw new MissingComponentException(errorMessage); // Lancement de l'exception
        }
    }

    /// <summary>
    /// Un utilitaire pour les composants <see cref="RectTransform"/>
    /// </summary>
    public static class RectTransformHandler {

        /// <summary>
        /// Ajuste les ancrages d'un <see cref="RectTransform"/> en fonction d'un autre <see cref="RectTransform"/>
        /// </summary>
        /// <param name="targetRectTransform">Le <see cref="RectTransform"/> cible</param>
        /// <param name="rectTransform">Le <see cref="RectTransform"/> de réference</param>
        /// <param name="anchorMin">Le décalage minimum de l'ancrage</param>
        /// <param name="anchorMax">Le décalage maximum de l'ancrage</param>
        public static void AdjustAnchors(RectTransform targetRectTransform, RectTransform rectTransform, AnchorAdjustments anchorMin = new(), AnchorAdjustments anchorMax = new()) {

            // Définit le décalage minimum de l'ancrage en fonction de celle du 'rectTransform' original
            targetRectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x + anchorMin.X, rectTransform.anchorMin.y + anchorMin.Y);

            // Définit le décalage maximal de l'ancrage en fonction de celle du 'rectTransform' original
            targetRectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x - anchorMax.X, rectTransform.anchorMax.y - anchorMax.Y);
        }


        /// <summary>
        /// Rend un <see cref="RectTransform"/> responsive en fonction de la taille d'un autre <see cref="RectTransform"/>.
        /// </summary>
        /// <param name="targetRectTransform">Le <see cref="RectTransform"/> cible</param>
        /// <param name="rectTransform">Le <see cref="RectTransform"/> de réference</param>
        public static void SyncWithAnotherRectTransform(RectTransform targetRectTransform, RectTransform rectTransform) {

            targetRectTransform.offsetMin = rectTransform.offsetMin; // Assure que le texte reste dans la zone inférieure définie par les ancrages
            targetRectTransform.offsetMax = rectTransform.offsetMax; // Assure que le texte reste dans la zone supérieure définie par les ancrages

            targetRectTransform.sizeDelta = rectTransform.sizeDelta; // Assure que le texte soit dépendant de la taille du 'rectTransform' original

            targetRectTransform.localScale = Vector3.one; // Assure que le texte soit au niveau de la taille de l'écran
        }
    }

    /// <summary>
    /// Un utilitaire pour les composants <see cref="TextMeshProUGUI"/>
    /// </summary>
    public static class TextMeshProUGUIHandler {

        /// <summary>
        /// Ajuste la taille de police dynamiquement en fonction de la taille de l'écran avec un pourcentage
        /// est basé sur la taille de l'écran. La taille du texte est ensuite redimensionné avec des valeurs
        /// minimales (entre 22px et 28px) et maximales (entre 28px et 32px).
        /// </summary>
        /// <param name="textMeshProUGUI"> Le <see cref="TextMeshProUGUI"/> </param>
        /// <param name="percentage">Le pourcentage de la taille de l'écran</param>
        public static void AdjustDynamicFontSize(TextMeshProUGUI textMeshProUGUI, float percentage) {

            float dynamicFontSize = Screen.height * percentage; // Calcule la taille de police dynamiquement en fonction de la taille de l'écran

            // Définit le mode d'habillage du texte pour qu'il s'affiche normalement sans débordement
            textMeshProUGUI.textWrappingMode = TextWrappingModes.Normal;

            // Active l'ajustement automatique de la taille de police (auto-sizing)
            textMeshProUGUI.enableAutoSizing = true;

            // Taille minimale dynamique entre '12' et '32'
            textMeshProUGUI.fontSizeMin = Mathf.Clamp(dynamicFontSize, 22f, 28f);

            // Taille maximale dynamique entre '32' et '72'
            textMeshProUGUI.fontSizeMax = Mathf.Clamp(dynamicFontSize, 28f, 72f);
        }

        /// <summary>
        /// Crée une copie exacte d'un <see cref="TextMeshProUGUI"/> en s'assurant de copier
        /// également les propriétés de transformation (<see cref="RectTransform"/>).
        /// </summary>
        /// <param name="textMeshProUGUI">Le <see cref="TextMeshProUGUI"/> à cloner</param>
        /// <param name="temporary">Si 'true', la copie sera temporaire</param>
        /// <returns>Un composant <see cref="TextMeshProUGUI"/> cloné</returns>
        public static TextMeshProUGUI CloneWithTransform(TextMeshProUGUI textMeshProUGUI, bool temporary = false) {

            // Essaie de récupérer le 'RectTransform' du composant 'ResponsiveDialogue' du composant 'ResponsiveDialogue' du parent du 'TextMeshProUGUI' principal
            RectTransform responsiveDialogueComponentRectTransform = textMeshProUGUI.GetComponentInParent<ResponsiveDialogue>()?.GetComponent<RectTransform>();

            /*
             * Crée une copie du 'TextMeshProUGUI' principal permettant de mesurer la
             * largeur de la ligne depuis un composant 'TextMeshProUGUI' temporaire
             */
            TextMeshProUGUI textMeshProUGUIClone = Object.Instantiate(textMeshProUGUI, textMeshProUGUI.rectTransform.position,
                textMeshProUGUI.rectTransform.rotation, temporary ? null : textMeshProUGUI.transform.parent);

            /*
             * Si la copie du 'TextMeshProUGUI' temporaire est temporaire, on supprime son parent,
             * on le cache dans l'éditeur et on le désactive.
             */
            if(temporary) {

                textMeshProUGUIClone.transform.SetParent(null); // Supprime le parent du 'TextMeshProUGUI' temporaire
                textMeshProUGUIClone.hideFlags = HideFlags.HideAndDontSave; // Cache la copie du 'TextMeshProUGUI' temporaire dans l'éditeur
                textMeshProUGUIClone.gameObject.SetActive(false); // Cache la copie du 'TextMeshProUGUI' temporaire
            }

            /*
             * Si le composant 'ResponsiveDialogue' existe, on modifie l'ancrage de la copie du 'TextMeshProUGUI'
             * temporaire afin de se rapprocher de celui du 'TextMeshProUGUI' principal et on synchronise la copie
             * du 'TextMeshProUGUI' temporaire de la même manière que le 'TextMeshProUGUI' principal
             */
            if(responsiveDialogueComponentRectTransform) {

                /*
                 * On modifie l'ancrage de la copie du 'TextMeshProUGUI' temporaire afin de se rapprocher de celui du 'TextMeshProUGUI' principal
                 */
                RectTransformHandler.AdjustAnchors(textMeshProUGUIClone.rectTransform, responsiveDialogueComponentRectTransform,
                    new AnchorAdjustments(0.11f, 0), new AnchorAdjustments(0.16f, 0.1f));

                // On synchronise la copie du 'TextMeshProUGUI' temporaire de la même manière que le 'TextMeshProUGUI' principal
                RectTransformHandler.SyncWithAnotherRectTransform(textMeshProUGUIClone.rectTransform, responsiveDialogueComponentRectTransform);

                // On ajuste la taille de police dynamiquement en fonction de la taille de l'écran
                AdjustDynamicFontSize(textMeshProUGUIClone, 0.04f);
            }

            return textMeshProUGUIClone;  // On retourne la copie du 'TextMeshProUGUI' temporaire
        }
    }
}