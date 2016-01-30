using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainScene : MonoBehaviour {
    
    public float randRange = 100f;

    List<int> keypointIDs;
    int index = 0;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool CheckKeyPointCollision(int ID)
    {
        if(keypointIDs[index] == ID)
        {
            index++;
            return true;
        } else
        {
            //game over
            return false;
        }
    }

    void SpawnKeypoints()
    {
        float x = Random.Range(-randRange, randRange);
        float y = Random.Range(-randRange, randRange);
        float z = Random.Range(-randRange, randRange);
        Vector3 spawn = new Vector3(x, y, z);


    }
}
