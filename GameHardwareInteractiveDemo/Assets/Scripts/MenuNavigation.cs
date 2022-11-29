using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MenuNavigation : MonoBehaviour
{
    public SerialController serialController;
    int menuOption;
    PlayerInputActions playerInputActions;
    float scrollDelta;
    public RectTransform selectUI;
    Vector2 startPos;
    public TextMeshProUGUI highscoreUI;

    public GameObject loadingUI;

    public static int highScore = 0;
    bool changineScene = false;
    AsyncOperation asyncLoad;

    bool canPress = false;

    // Start is called before the first frame update
    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

       // playerInputActions.Menu.Scroll.performed += cntxt => scrollDelta = cntxt.ReadValue<float>();
        //playerInputActions.Menu.Select.performed += cntxt => Select();

        startPos = selectUI.position;

        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();

        highscoreUI.text = "High Score: " + highScore;
    }

    // Update is called once per frame
    void Update()
    {
        serialController.maxUnreadMessages = 0;


        string message = serialController.ReadSerialMessage();
        if (message != null)
        {
            if (message == "Pressed" && canPress)
            {
                //Debug.Log("ButtonPressed!");
                canPress = false;
                Select();
            }

            if (message == "Released")
            {
                canPress = true;
            }

            if (message == "Clockwise")
                menuOption = 0;

            else if (message == "CounterClockwise")
                menuOption = -1;
        }

        
        selectUI.position = startPos + new Vector2(0.0f, 125.9f) * menuOption;
    }

    void Select()
    {
        if (menuOption == 0 && !changineScene)
        {
            changineScene = true;
            loadingUI.SetActive(true);

            //serialController.StopAllCoroutines();
            //serialController.gameObject.SetActive(false);

            SceneManager.LoadScene(1);


        }
        else
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
    }

    void OnApplicationQuit()
    {
        serialController.SendSerialMessage("I");
    }
}
