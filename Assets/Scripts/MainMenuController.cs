using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject buttonPanel, characterSelectPanel, createCharacterPanel;

    private MainMenuCamera mainMenuCamera;

    private void Awake()
    {
        mainMenuCamera = Camera.main.GetComponent<MainMenuCamera>();
    }

    public void PlayGame()
    {
        mainMenuCamera.ChangePosition(1);
        buttonPanel.SetActive(false);
        characterSelectPanel.SetActive(true);
        /*if (mainMenuCamera.CanClick)
        {
            mainMenuCamera.CanClick = false;
            buttonPanel.SetActive(false);
            characterSelectPanel.SetActive(true);
            mainMenuCamera.ReachedCharacterSelectPosition = false;
        }*/
    }

    public void BackToMainMenu()
    {
        mainMenuCamera.ChangePosition(0);
        buttonPanel.SetActive(true);
        characterSelectPanel.SetActive(false);  
       /* if (mainMenuCamera.CanClick)
        {
            mainMenuCamera.CanClick = false;
            mainMenuCamera.BackToMainMenu = true;
            buttonPanel.SetActive(true);
            characterSelectPanel.SetActive(false);
        }*/
    }

    public void StartGame()
    {
        SceneLoader.instance.LoadLevel("LevelSelect");
    }


    public void CreateCharacter()
    {
        characterSelectPanel.SetActive(false);
        createCharacterPanel.SetActive(true);
    }

    public void Accept()
    {
        characterSelectPanel.SetActive(true);
        createCharacterPanel.SetActive(false);
    }

    public void Cancel()
    {
        characterSelectPanel.SetActive(true);
        createCharacterPanel.SetActive(false);
    }
}
