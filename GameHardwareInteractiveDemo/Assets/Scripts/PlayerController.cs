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

    float roll, pitch;
    float prevRoll, prevPitch;

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
    }

    // Update is called once per frame
    void Update()
    {
        string message = serialController.ReadSerialMessage();

        if (message != null)
        {
            

            if (message == "Pressed")
                SceneManager.LoadScene(0);

            if (message == "Clockwise")
                scrollDelta = 2000;
            else if (message == "CounterClockwise")
                scrollDelta = -2000;
            Vector3 dir = transform.position - playerCam.transform.position;
            rb.AddForce(20.0f * dir.normalized * scrollDelta * Time.deltaTime);
            scrollDelta = 0;

            if (message.StartsWith('P'))
                pitch = float.Parse(message.Substring(2));

            if (message.StartsWith('R'))
                roll = float.Parse(message.Substring(2));

        }

        if (timer > 0)
            timer -= Time.deltaTime;
        else
            SceneManager.LoadScene(0);
        
        timerUI.text = "Time Remaining: " + timer;
        fishUI.text = "Fish Caught: " + numFishCaught;

        if (!bobCast)
        {
            transform.position = restPos;

            if (roll - prevRoll > 20.0f)
            {
                bobCast = true;
                rb.AddForce(0.0f, 300.0f, 600.0f * Mathf.Abs(roll) * 0.1f);
            }
            prevRoll = roll;


            return;
        }

     



    }

    private void FixedUpdate()
    {
        Vector3 dir = transform.position - playerCam.transform.position;
        rb.AddForce(pitch * 0.2f , 0.0f, 0.0f);
        
    }

    void Select()
    {

    }
}
