using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MenuNavigation : MonoBehaviour
{
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

        playerInputActions.Menu.Scroll.performed += cntxt => scrollDelta = cntxt.ReadValue<float>();
        playerInputActions.Menu.Select.performed += cntxt => Select();

        startPos = selectUI.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        menuOption += (int)scrollDelta / 120;
        if (menuOption > 0)
        {
            menuOption = 0;
        }
        else if (menuOption < 0)
        {
            menuOption = -1;
        }

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
