using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainScene : MonoBehaviour {
    public GameObject[] collectiblePrefabs;
    public int count = 1;
    public float randRange = 50f;
    
    Color[] colors = {new Color(7/255f,114/255f,222/255f),// new Color(95/255f,164/255f,223/255f),
                                    //new Color(0f, 196/255f, 196/255f),// new Color(91/255f,216/255f,216/255f)
                                    new Color(0, 217/255f, 108/255f),// new Color(91/255f,230,160),
                                    //new Color(123/255f, 212/255f, 34/255f),// new Color(170, 227, 113),
                                    new Color(245/255f, 204/255f, 0f),// new Color(248, 222, 91),
                                    //new Color(242/255f, 162/255f, 0f),// new Color(246, 195, 91),
                                    new Color(255/255f,149/255f,0f),// new Color(255, 187,91),
                                    //new Color(224/255f,75/255f,0f),// new Color(235,139,91),
                                    new Color(247/255f,32/255f,32/255f),// new Color(249,111,111),
                                    //new Color(248/255f,0f,148/255f),// new Color(250, 91, 186),
                                    new Color(164/255f,64/255f,184/255f) };// new Color(196,132,109),
                                    //new Color(108/255f,64/255f,184/255f) };// new Color(160,132,209)};

    List<int> keypointIDs;
    int index = 0;
    
	// Use this for initialization
	void Start () {
        keypointIDs = new List<int>();
        SpawnKeypoints();
        Time.timeScale = 0;
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
        int p = Random.Range(0, collectiblePrefabs.Length);
        Debug.Log(collectiblePrefabs.Length-1);
        GameObject obj = (GameObject) Instantiate(collectiblePrefabs[p], spawnLoc, new Quaternion(0,0,0,0));
        keypointIDs.Add(obj.GetInstanceID());
        int c = Random.Range(0, colors.Length);
        obj.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[c]);
    }
}
