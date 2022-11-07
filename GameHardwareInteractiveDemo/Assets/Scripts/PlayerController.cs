using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
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

        playerInputActions.Menu.Scroll.performed += cntxt => scrollDelta = cntxt.ReadValue<float>();
        playerInputActions.Menu.Select.performed += cntxt => Select();
        playerInputActions.Menu.Motion.performed += cntxt => motion = cntxt.ReadValue<Vector2>();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            SceneManager.LoadScene(0);
        
        timerUI.text = "Time Remaining: " + timer;
        fishUI.text = "Fish Caught: " + numFishCaught;

        if (!bobCast)
        {
            transform.position = restPos;

            if (motion.y > 0)
            {
                bobCast = true;
                rb.AddForce(0.0f, 300.0f, 600.0f * motion.y);
            }

            return;
        }

        Vector3 dir = transform.position - playerCam.transform.position;
        rb.AddForce(20.0f * dir.normalized * scrollDelta * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector3 dir = transform.position - playerCam.transform.position;
        rb.AddForce(motion.x * 5.0f * dir.normalized.x, 0.0f, 0.0f);

    }

    void Select()
    {

    }
}
