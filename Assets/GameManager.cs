using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
      public static GameManager instance;

    [SerializeField]
    private GameObject[] character;

    [HideInInspector]
    public int selectedCharacter;

    private void Awake()
    {
        MakeSingleton();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded += LevelFinishedLoading;
    }

    void LevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "MainMenu")
        {
            Vector3 pos = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
            Instantiate(character[selectedCharacter], pos, Quaternion.identity);
        }
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
