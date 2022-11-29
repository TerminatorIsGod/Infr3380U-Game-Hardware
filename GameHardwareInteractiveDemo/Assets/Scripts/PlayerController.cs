using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public SerialController serialController;
    public static PlayerController instance;
    PlayerInputActions playerInputActions;
    float scrollDelta;
    Vector2 motion;
    Rigidbody rb;
    public Vector3 restPos;
    public GameObject playerCam;

    public bool bobCast = false;
    public bool fishCaught = false;

    public float timer = 30.0f;
    public TextMeshProUGUI timerUI;
    public int numFishCaught = 0;
    public TextMeshProUGUI fishUI;

    public GameObject hookedUI;
    public GameObject castUI;
    public GameObject tiltUI;
    public GameObject reelUI;

    public GameObject pauseUI;
    int menuOption;
    bool paused = false;

    public RectTransform selectUI;
    Vector2 startPos;

    float roll, pitch;
    float prevRoll, prevPitch;

    bool canPress = false;
    public TextMeshProUGUI highscoreUI;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        //playerInputActions.Menu.Scroll.performed += cntxt => scrollDelta = cntxt.ReadValue<float>();
        playerInputActions.Menu.Select.performed += cntxt => Select();
        playerInputActions.Menu.Motion.performed += cntxt => motion = cntxt.ReadValue<Vector2>();

        rb = GetComponent<Rigidbody>();

        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();

        startPos = selectUI.position;
    }

    // Update is called once per frame
    void Update()
    {
        string message = serialController.ReadSerialMessage();

        if (message != null)
        {
            if (!paused)
            {
                if (message == "Pressed" && canPress)
                {
                    Time.timeScale = 0;
                    paused = true;
                    pauseUI.SetActive(true);
                    canPress = false;

                }

                if (message == "Released")
                {
                    canPress = true;
                }

                if (message == "Clockwise")
                    scrollDelta = 2000;
                else if (message == "CounterClockwise")
                    scrollDelta = -2000;
                Vector3 dir = transform.position - playerCam.transform.position;
                rb.AddForce(8.0f * dir.normalized * scrollDelta * Time.deltaTime);
                scrollDelta = 0;

                if (message.StartsWith('X'))
                    pitch = float.Parse(message.Substring(2));

                if (message.StartsWith('Y'))
                    roll = float.Parse(message.Substring(2));

            }
            else
            {
                if (message == "Pressed" && canPress)
                {
                    Select();
                    canPress = false;
                }

                if (message == "Released")
                {
                    canPress = true;
                }

                if (message == "Clockwise")
                    menuOption = 0;

                else if (message == "CounterClockwise")
                    menuOption = -1;

                selectUI.position = startPos + new Vector2(0.0f, 125.9f) * menuOption;
                highscoreUI.text = "High Score: " + MenuNavigation.highScore;

            }
        }

        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            serialController.SendSerialMessage("I");

            //serialController.StopAllCoroutines();
            //serialController.gameObject.SetActive(false);
            SceneManager.LoadScene(0);
        }
        
        timerUI.text = "Time Remaining: " + timer;
        fishUI.text = "Fish Caught: " + numFishCaught;


        if (!bobCast)
        {
            castUI.SetActive(true);
            tiltUI.SetActive(false);
            reelUI.SetActive(false);

            transform.position = restPos;

            if (roll - prevRoll < -20.0f)
            {
                bobCast = true;
                rb.AddForce(0.0f, 300.0f, 600.0f * Mathf.Abs(roll) * 0.05f);
            }
            prevRoll = roll;

            return;
        }
        else
        {
            castUI.SetActive(false);
            tiltUI.SetActive(true);
            reelUI.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        Vector3 dir = transform.position - playerCam.transform.position;
        rb.AddForce(-pitch * 0.2f , 0.0f, 0.0f);
        
    }

    void Select()
    {
        if (menuOption == 0)
        {
            Time.timeScale = 1;
            paused = false;
            pauseUI.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;

            serialController.SendSerialMessage("I");

            SceneManager.LoadScene(0);
        }
    }

    void OnApplicationQuit()
    {
        serialController.SendSerialMessage("I");
    }
}
