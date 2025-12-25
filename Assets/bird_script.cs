using UnityEngine;
using UnityEngine.InputSystem;


public class bird_script : MonoBehaviour
{
    public Rigidbody2D myrigitbody2d;
    public float jumpSpeed = 10;
    public LogicScript logic;
    public bool BirdisAlive = true;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    void Update()
    {
        bool flapInput =
            (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) ||
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
            (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame);

        if (flapInput && BirdisAlive)
        {
            myrigitbody2d.linearVelocity = Vector2.up * jumpSpeed;
        }

        if (transform.position.y < -6f || transform.position.y > 6f)
        {
            logic.gameover();
            BirdisAlive = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        logic.gameover();
        BirdisAlive = false;
    }
}
