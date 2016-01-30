using UnityEngine;
using System.Collections;

public class CollectibleBehaviour : MonoBehaviour {
    // Use this for initialization
    private Animator anim;
    private MainScene ms;
    private Vector3 startPos;

    void Start()
    {
        ms = FindObjectOfType<MainScene>();
        anim = gameObject.GetComponent<Animator>();
        startPos = transform.position;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") {
            //col.gameObject.GetComponent<ScoreManager>().addCollectible("C1");
            //Destroy(gameObject);
            if (ms.CheckKeyPointCollision(gameObject))
            {
                SetInvisible();
                //anim.SetBool("Active", false);
                //col.gameObject.GetComponent<Rigidbody>().velocity *= 1.5f;
                //while (gameObject.GetComponent<MeshRenderer>().enabled)
                //{
                  //  Vector3 delta = new Vector3(3, 0, 3);
                   // gameObject.transform.position = col.gameObject.transform.position + delta;
                //}
            }
           
        }
    }

    public void SetInvisible()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    /*
    void LateUpdate()
    {
        if (!anim.GetBool("Active"))
        {
            transform.localPosition += 1.4f*startPos;
        }
    }*/
}
