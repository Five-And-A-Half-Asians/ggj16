using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainScene : MonoBehaviour {
    public GameObject player;
    public GameObject[] collectiblePrefabs;
    public Text scoreText;
    //public Text gameoverText; //TODO: put gameovertext back in

    public int count = 1;
    public float randRange = 50f;
    public float distance = 5f;

    private int score;

    
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

    List<GameObject> keypointIDs;
    int targetIndex = 0;
    
	// Use this for initialization
	void Start () {
        keypointIDs = new List<GameObject>();
        //SpawnKeypoints();
        SpawnKeyPoint(new Vector3(0,0,10));
        scoreText.fontSize = Screen.height / 20;
        //gameoverText.fontSize = Screen.height / 20;
        score = 0;
        UpdateScore();
        Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButton(0))
        {
            Time.timeScale = 1;
            //gameoverText.text = "";
        }
        PlayerMove();
        if (targetIndex == keypointIDs.Count)
        {
            if (targetIndex == 1)
            {
                float randX = Random.Range(0, 10);
                float randY = Random.Range(0,10);
                SpawnKeyPoint(new Vector3(randX, randY, 30));
            } else {
                SpawnKeyPoint(RandomPoint(randRange));
            }
            NewRound();
            score++;
            UpdateScore();
        }
	}

    void NewRound()
    {
        foreach(GameObject go in keypointIDs)
        {
            go.GetComponent<MeshRenderer>().enabled = true;
        }
        player.transform.position = new Vector3(0, 0, 0);
        targetIndex = 0;
        Time.timeScale = 0;
    }

    public bool CheckKeyPointCollision(GameObject go)
    {
        if(keypointIDs[targetIndex] == go)
        {
            targetIndex++;
            Debug.Log("Found " + targetIndex + " items.");
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
            SpawnKeyPoint(RandomPoint(randRange));
        }
    }

    Vector3 RandomPoint(float range)
    {
        range = Mathf.Abs(range);
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);
        float z = Random.Range(-range, range);
        return new Vector3(x, y, z);
    }

    void SpawnKeyPoint(Vector3 loc)
    {
        int p = Random.Range(0, collectiblePrefabs.Length);
        GameObject obj = (GameObject) Instantiate(collectiblePrefabs[p], loc, new Quaternion(0,0,0,0));
        keypointIDs.Add(obj);
        int c = Random.Range(0, colors.Length);
        obj.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[c]);
    }

    void PlayerMove()
    {
        player.transform.position = player.transform.position + Camera.main.transform.forward * distance * Time.deltaTime;
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
        scoreText.fontSize = 3;
    }
}
