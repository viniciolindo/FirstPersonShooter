using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController cc;

    public float speed = 12f;

    public float gravity = -9.81f;

    Vector3 velocity;

    private bool groundedPlayer;

    [SerializeField] private float jumpHeight = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 1. Controllo se il giocatore tocca terra
        groundedPlayer = cc.isGrounded;
        
        // Reset della velocità verticale quando è a terra
        if (groundedPlayer && velocity.y < 0)
        {
            velocity.y = -2f; // Un piccolo valore negativo per tenerlo "incollato" al suolo
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        cc.Move(move * speed * Time.deltaTime);

        // 3. Logica del Salto
        // Il salto viene attivato solo se il giocatore è a terra
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            // Formula fisica per calcolare la velocità necessaria a raggiungere un'altezza specifica
            velocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
}
