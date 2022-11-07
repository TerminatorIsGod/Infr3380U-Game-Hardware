using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catchTrigger : MonoBehaviour
{
    public GameObject prefab;
    public GameObject centerPoint;
    public float radius;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.bobCast = false;
            if (PlayerController.instance.fishCaught)
            {
                PlayerController.instance.fishCaught = false;
                PlayerController.instance.timer += 20.0f;
                PlayerController.instance.numFishCaught += 1;
                PlayerController.instance.hookedUI.SetActive(false);

                Destroy(PlayerController.instance.gameObject.transform.GetChild(0).gameObject);

                Vector2 randomPoint = Random.insideUnitCircle * radius;
                Instantiate(prefab, centerPoint.transform.position + new Vector3(randomPoint.x, 0.0f, randomPoint.y), Quaternion.identity);
            }
        }
    }
}
