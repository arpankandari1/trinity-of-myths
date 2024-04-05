using System;
using UnityEngine;

public class CharacterMenu : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject charPosition;

    private int Player1 = 0;
    private int Player2 = 1;
    private int Player3 = 2;

    void Start()
    {
        characters[Player1].SetActive(true);
        characters[Player1].transform.position = charPosition.transform.position;
    }

    public void SelectCharacter()
    {
        
        
            int index = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            TurnOffCharacters();

            characters[index].SetActive(true);
            characters[index].transform.position = charPosition.transform.position;

        GameManager.instance.selectedCharacter = index;
    }

    void TurnOffCharacters()
    {
        for(int i = 0; i< characters.Length; i++)
        {
            characters[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
