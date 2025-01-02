using UnityEngine;

/// <summary>
/// Classe permettant de gérer tout ce qui est lié au joueur.
/// </summary>
public class PlayerManager : MonoBehaviour {

    /**********************************************/
    /***               PROPRIÉTÉS               ***/
    /**********************************************/

    /// <summary>
    /// Vitesse de mouvement du joueur
    /// </summary>
    [SerializeField]
    private float moveSpeed;

    /// <summary>
    /// Force du saut du joueur
    /// </summary>
    [SerializeField]
    private float jumpForce;

    /// <summary>
    /// Permet de vérifier si le joueur est au sol
    /// </summary>
    private bool _isGrounded;

    /// <summary>
    /// Composant Rigidbody2D du joueur
    /// </summary>
    private Rigidbody _rigideBody;

    /**********************************************/
    /***              CYCLE DE VIE              ***/
    /**********************************************/

    /// <summary>
    /// Lors du lancement du jeu, recherche le composant Rigidbody2D du joueur
    /// </summary>
    void Start() { _rigideBody = GetComponent<Rigidbody>(); }

    /// <summary>
    /// Lors de chaque frame, gestion du saut du joueur
    /// </summary>
    void Update() {

        // Gestion du saut du joueur
        if(Input.GetButtonDown("Jump") && _isGrounded) {

            // Ajoute une force verticale au joueur
            _rigideBody.linearVelocity = new Vector2(_rigideBody.linearVelocity.x, _rigideBody.linearVelocity.y + jumpForce);
        }
    }

    /// <summary>
    /// Lors de chaque frame corrigé, gestion du mouvement du joueur
    /// </summary>
    void FixedUpdate()  {

        // Vérifie si le joueur appuie sur une touche du clavier
        float moveInput = Input.GetAxis("Horizontal");

        // Déplace le joueur avec la vélocité linéaire du Rigidbody
        _rigideBody.linearVelocity = new Vector2(moveInput * moveSpeed, _rigideBody.linearVelocity.y);
    }

    /**********************************************/
    /***              ÉVÈNEMENTS                ***/
    /**********************************************/

    /// <summary>
    /// Lorsqu'un objet entre en collision avec le joueur, on vérifie si le joueur est au sol
    /// pour lui changer le booléen 'isGrounded'.
    /// </summary>
    /// <param name="collision">La collision à laquelle le joueur est entré.</param>
    private void OnCollisionEnter(Collision collision) {

        // Vérifie si le joueur est au sol, si c'est le cas, le booléen 'isGrounded' est vrai
        if(collision.gameObject.CompareTag("Ground")) _isGrounded = true;
    }

    /// <summary>
    /// Lorsqu'un objet sort de la collision avec le joueur, on vérifie si le joueur est au sol
    /// pour lui changer le booléen 'isGrounded'.
    /// </summary>
    /// <param name="collision">La collision à laquelle le joueur est entré.</param>
    private void OnCollisionExit(Collision collision) {

        // Vérifie si le joueur est au sol, si c'est le cas, le booléen 'isGrounded' est faux
        if(collision.gameObject.CompareTag("Ground")) _isGrounded = false;
    }
}
