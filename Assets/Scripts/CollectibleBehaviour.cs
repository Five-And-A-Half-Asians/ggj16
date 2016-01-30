using UnityEngine;
using System.Collections;

public class CollectibleBehaviour : MonoBehaviour {
	// Use this for initialization

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") {
            //col.gameObject.GetComponent<ScoreManager>().addCollectible("C1");
            //Destroy(gameObject);
            gameObject.GetComponent<MeshRenderer>().enabled = !FindObjectOfType<MainScene>().CheckKeyPointCollision(gameObject);
           
        }
    }
}
