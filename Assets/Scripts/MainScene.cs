using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainScene : MonoBehaviour {
    public GameObject player;
    public Text roundText;
    public Text scoreText;
    public Text centerText;
    public Text timerText;

    public float randRange = 2f;
	public float randRangeStep = 0.5f;
    
	public float playerMoveSpeed = 0f;

	List<GameObject> keypointIDs;
	int nextKeypointIndex;


    private int score = 0;
    private bool gameOver;
    private float timeElapsed = 0;

	public GameObject[] collectiblePrefabs;
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

    // Use this for initialization
    void Start()
	{
		Time.timeScale = 1;
        timeElapsed = 0;
        gameOver = false;
        player.transform.position = new Vector3(0, 0, 0);
        keypointIDs = new List<GameObject>();
        nextKeypointIndex = 0;
        SpawnKeyPoint(new Vector3(0, 0, 10));
        score = 0;
		playerMoveSpeed = 0f;
        centerText.text = "Tap to Start";
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (gameOver) {
			if (Input.GetMouseButton (0) || Input.GetButton ("Fire1"))
				Start ();
			else
				return;
		}
        if (Input.GetKeyUp(KeyCode.Escape))
            GameOver();

		PlayerMove();

		if (nextKeypointIndex == keypointIDs.Count)
		{
			switch (nextKeypointIndex) {
			case 0:
				SpawnKeyPoint (new Vector3 (0, 0, 10));
				break;
			case 1:
				float randX = Random.Range (0, randRange/2);
				float randY = Random.Range (0, randRange/2);
				SpawnKeyPoint (new Vector3 (randX, randY, randRange));
				break;
			default:
				SpawnKeyPoint (RandomPoint (randRange));
				break;
			}
			randRange += randRangeStep;
			NewRound();
		}

        // Update HUD
        scoreText.text = score + " collected";
        roundText.text = "Round " + keypointIDs.Count;
        var minutes = timeElapsed / 60;
        var seconds = timeElapsed % 60;
        //var fraction = (timeElapsed * 100) % 100;
        //timerText.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

		if (timeElapsed < 5)
			centerText.text = "Hold to accelerate";
		else
			centerText.text = "";
    }

    void NewRound()
    {
        foreach (GameObject go in keypointIDs)
        {
            go.GetComponent<MeshRenderer>().enabled = true;
        }
        player.transform.position = new Vector3(0, 0, 0);
        nextKeypointIndex = 0;
		playerMoveSpeed = 0f;
        centerText.text = "Tap to Continue";
    }

    public bool CheckKeyPointCollision(GameObject go)
    {
        if (keypointIDs[nextKeypointIndex] == go)
        {

            nextKeypointIndex++;
            Debug.Log("Found " + nextKeypointIndex + " items.");
            score++;
            return true;
        }
        else
        {
            //game over
            GameOver();
            return false;
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
        GameObject obj = (GameObject)Instantiate(collectiblePrefabs[p], loc, new Quaternion(0, 0, 0, 0));
        keypointIDs.Add(obj);
        int c = Random.Range(0, colors.Length);
        obj.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[c]);
    }

    void PlayerMove()
    {
		if (Input.GetMouseButton (0) || Input.GetButton ("Fire1")) {
			float playerAccel = 0.1f * Mathf.Pow (playerMoveSpeed, 0.3f) + 0.01f * Mathf.Pow(1.1f, playerMoveSpeed);
			playerAccel = Mathf.Min (10, playerAccel);
			playerMoveSpeed = Mathf.Max (1f, playerMoveSpeed + playerAccel);
		}

		if (playerMoveSpeed > 0f) { // don't start moving until tap
			float playerAccel = 0.01f * Mathf.Pow(playerMoveSpeed/2f, 0.6f);
			playerMoveSpeed = Mathf.Max (1f, playerMoveSpeed - playerAccel);

		}

		// make it harder to escape
		if (playerMoveSpeed > 10f) playerMoveSpeed *= 0.95f;
		if (playerMoveSpeed > 20f) playerMoveSpeed *= 0.90f;
		playerMoveSpeed = Mathf.Min (100, playerMoveSpeed);
        
		player.transform.position = player.transform.position + Camera.main.transform.forward * playerMoveSpeed * Time.deltaTime;
    }

    void GameOver()
    {
        foreach (GameObject go in keypointIDs)
        {
            Destroy(go.gameObject);
        }
        
		player.transform.position = new Vector3(0, 0, 0);
        centerText.text = "GAME OVER \nSCORE: " + score + "\nTAP TO START";

        gameOver = true;
    }
}
