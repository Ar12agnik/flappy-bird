using UnityEngine;
using UnityEngine.InputSystem;


public class bird_script : MonoBehaviour
{
    public Rigidbody2D myrigitbody2d;
    public float jumpSpeed = 10;

    void Update()
    {
                if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            myrigitbody2d.linearVelocity = Vector2.up * jumpSpeed;
        }
    }
}
