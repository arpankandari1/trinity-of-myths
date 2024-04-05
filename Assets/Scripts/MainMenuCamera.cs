using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    public GameObject gameStartedPosition;
    public GameObject characterSelectPosition;

    private bool reachedGameStartedPosition;
    private bool reachedCharacterSelectPosition;
    private bool canClick = true;
    private bool backToMainMenu;

    // Speed at which camera moves
    public float moveSpeed = 5f;

    private void Update()
    {
        if (!reachedGameStartedPosition && !backToMainMenu)
            MoveTo(gameStartedPosition);
        else if (!reachedCharacterSelectPosition && !backToMainMenu)
            MoveTo(characterSelectPosition);
        else if (backToMainMenu)
            MoveTo(gameStartedPosition);
    }

    private void MoveTo(GameObject targetPosition)
    {
        // Calculate interpolation factor based on moveSpeed and distance
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.transform.position, step);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetPosition.transform.rotation, step * 100f);

        if (Vector3.Distance(transform.position, targetPosition.transform.position) < 0.2f)
        {
            if (targetPosition == gameStartedPosition)
                reachedGameStartedPosition = true;
            else if (targetPosition == characterSelectPosition)
                reachedCharacterSelectPosition = true;

            canClick = true;
        }
    }

    public void ChangePosition(int index)
    {
        reachedGameStartedPosition = index == 0;
        reachedCharacterSelectPosition = index != 0;
        backToMainMenu = false;
        canClick = false;

        if (index == 0)
        {
            transform.position = gameStartedPosition.transform.position;
            transform.rotation = gameStartedPosition.transform.rotation;
        }
        else
        {
            transform.position = characterSelectPosition.transform.position;
            transform.rotation = characterSelectPosition.transform.rotation;
        }
    }

    public bool ReachedCharacterSelectPosition => reachedCharacterSelectPosition;

    public bool CanClick => canClick;

    public bool BackToMainMenu => backToMainMenu;
}
