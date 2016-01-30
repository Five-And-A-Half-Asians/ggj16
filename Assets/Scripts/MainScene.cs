using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainScene : MonoBehaviour {
    public GameObject collectiblePrefab;
    public int count = 1;
    public float randRange = 50f;

    List<int> keypointIDs;
    int index = 0;
    
	// Use this for initialization
	void Start () {
        keypointIDs = new List<int>();
        SpawnKeypoints();
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
        for(int i = 0; i< count; i++)
        {
            SpawnKeyPoint();
        }
    }

    void SpawnKeyPoint()
    {
        float x = Random.Range(-randRange, randRange);
        float y = Random.Range(-randRange, randRange);
        float z = Random.Range(-randRange, randRange);
        Vector3 spawnLoc = new Vector3(x, y, z);
        Object obj = Instantiate(collectiblePrefab, spawnLoc, new Quaternion(0,0,0,0));
        keypointIDs.Add(obj.GetInstanceID());
    }
}
