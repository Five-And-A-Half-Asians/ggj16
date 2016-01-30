using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainScene : MonoBehaviour {
    
    public float min_rand = 0f;
    public float max_rand = 100f;

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
        float x = Random.Range(min_rand, max_rand);
        float y = Random.Range(min_rand, max_rand);
        float z = Random.Range(min_rand, max_rand);
        Vector3 spawn = new Vector3(x, y, z);


    }
}
