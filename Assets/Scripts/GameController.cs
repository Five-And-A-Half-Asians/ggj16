using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    private static GameController instance;

    public static GameController Instance
    {
        get { return instance ?? (instance = new GameObject("GameController").AddComponent<GameController>()); }
    }

    void Awake () {
	    if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
	}
	
	void Update () {
	
	}

    public void NotifyTrigger(Collider collider, TargetSphere sphere)
    {
        Debug.Log("notified about trigger");
    }
}
