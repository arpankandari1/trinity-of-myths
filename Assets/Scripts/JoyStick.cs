using UnityEngine;

public class JoyStick : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        // Find the player GameObject by its tag
        player = GameObject.FindWithTag("Player");

        // Check if player GameObject is found
        if (player == null)
        {
            Debug.LogError("Player GameObject not found. Make sure it has the correct tag.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided GameObject has the tag "JoyStickBg"
        if (collision.gameObject.CompareTag("JoyStickBg"))
        {
            // Get the MovementController component from the player GameObject
            MovementController movementController = player.GetComponent<MovementController>();

            // Check if the MovementController component exists
            if (movementController != null)
            {
                // Set the isRunningStick field to true
                movementController.SetIsRunning(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the collided GameObject has the tag "JoyStickBg"
        if (collision.gameObject.CompareTag("JoyStickBg"))
        {
            // Get the MovementController component from the player GameObject
            MovementController movementController = player.GetComponent<MovementController>();

            // Check if the MovementController component exists
            if (movementController != null)
            {
                // Set the isRunningStick field to false
                movementController.SetIsRunning(false);
            }
        }
    }
}
