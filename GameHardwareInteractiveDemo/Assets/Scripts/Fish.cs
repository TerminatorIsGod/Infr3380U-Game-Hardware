using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = PlayerController.instance.gameObject.transform.position - transform.position;

        if (PlayerController.instance.fishCaught)
            return;

        if (dir.magnitude < 4.0f)
        {
            rb.AddForce(Time.deltaTime * dir.normalized);
        }

        if (dir.magnitude < 0.3f)
        {
            PlayerController.instance.fishCaught = true;
            rb.isKinematic = true;
            PlayerController.instance.hookedUI.SetActive(true);
            transform.parent = PlayerController.instance.gameObject.transform;
            PlayerController.instance.serialController.SendSerialMessage("V");
        }
    }
}
