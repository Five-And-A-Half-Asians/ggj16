using UnityEngine;
using System.Collections;

public class FPUIController : MonoBehaviour {

    private Vector3 target;
    public GameObject cylinder;

	public void setTarget(Vector3 newTarget)
    {
        target = newTarget;
    }
	
	// Update is called once per frame
	void Update () {
        target = new Vector3(0, 0, 0);
        cylinder.transform.LookAt(target);
	}
}
