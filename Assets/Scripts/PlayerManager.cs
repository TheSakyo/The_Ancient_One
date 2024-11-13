using UnityEngine;

public class PlayerManager : MonoBehaviour {

    /**********************************************/
    /***               PROPRIÉTÉS               ***/
    /**********************************************/

    [SerializeField]
    private float moveSpeed;         // Vitesse de mouvement

    [SerializeField]
    private float jumpForce;        // Force du saut

    private bool _isGrounded;             // Permet de vérifier si le joueur est au sol

    private Rigidbody _rigideBody;        // Composant Rigidbody2D du joueur

    /**********************************************/
    /***              CYCLE DE VIE              ***/
    /**********************************************/

    /**
     * Lors du lancement du jeu, recherche le composant Rigidbody2D du joueur
     */
    void Start() {

        _rigideBody = GetComponent<Rigidbody>();
    }

    /**
     * Lors de chaque frame, vérifie si le joueur appuie sur une touche du clavier
     */
    void Update()
    {

        // Vérifie si le joueur appuie sur une touche du clavier
        float moveInput = Input.GetAxis("Horizontal");

        // Déplace le joueur
        _rigideBody.velocity = new Vector2(moveInput * moveSpeed, _rigideBody.velocity.y);

        // Gestion du saut du joueur
        if(Input.GetButtonDown("Jump") && _isGrounded) {

            // Ajoute une force verticale au joueur
            _rigideBody.velocity = new Vector2(_rigideBody.velocity.x, _rigideBody.velocity.y + jumpForce);
        }
    }

    void FixedUpdate()  {

        // Vérifie si le joueur appuie sur une touche du clavier
        float moveInput = Input.GetAxis("Horizontal");

        // Déplace le joueur
        _rigideBody.velocity = new Vector2(moveInput * moveSpeed, _rigideBody.velocity.y);
    }

    /**********************************************/
    /***              ÉVÈNEMENTS                ***/
    /**********************************************/

    /**
      * Lorsqu'un objet entre en collision avec le joueur, on vérifie si le joueur est au sol
      * pour lui changer le booléen 'isGrounded'.
      */
    private void OnCollisionEnter(Collision collision) {

        if(collision.gameObject.CompareTag("Ground")) _isGrounded = true;
    }

    /**
      * Lorsqu'un objet sort de la collision avec le joueur, on vérifie si le joueur est au sol
      * pour lui changer le booléen 'isGrounded'.
      */
    private void OnCollisionExit(Collision collision) {

        if(collision.gameObject.CompareTag("Ground")) _isGrounded = false;
    }
}
