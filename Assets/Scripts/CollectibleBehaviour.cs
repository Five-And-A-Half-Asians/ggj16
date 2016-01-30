using UnityEngine;
using System.Collections;

public class CollectibleBehaviour : MonoBehaviour {
    public GameObject player;
	// Use this for initialization

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == player) {
            player.GetComponent<ScoreManager>().addCollectible("C1");
            //Destroy(gameObject);
            FindObjectOfType<MainScene>().CheckKeyPointCollision(gameObject.GetInstanceID());
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
