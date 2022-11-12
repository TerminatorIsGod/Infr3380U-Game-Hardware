using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MenuNavigation : MonoBehaviour
{
    public SerialController serialController;
    int menuOption;
    PlayerInputActions playerInputActions;
    float scrollDelta;
    public RectTransform selectUI;
    Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

       // playerInputActions.Menu.Scroll.performed += cntxt => scrollDelta = cntxt.ReadValue<float>();
        //playerInputActions.Menu.Select.performed += cntxt => Select();

        startPos = selectUI.position;

        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
    }

    // Update is called once per frame
    void Update()
    {
       
        string message = serialController.ReadSerialMessage();
        if (message != null)
        {
            if (message == "Pressed")
            {
                Debug.Log("ButtonPressed!");
                Select();
            }

            if (message == "Clockwise")
                menuOption = 0;

            else if (message == "CounterClockwise")
                menuOption = -1;
        }

        serialController.SendSerialMessage("I");
        selectUI.position = startPos + new Vector2(0.0f, 125.9f) * menuOption;
    }

    void Select()
    {
        if (menuOption == 0)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
    }
}
