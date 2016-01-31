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
    public Text fuelText;

	public float randRange = 5f; // real value set in Reset()
	public float randRangeStep = 1f;
    
	public float playerMoveSpeed = 0f;

	List<GameObject> keypointIDs;
	int nextKeypointIndex;


    private int score = 0;
    private bool gameRunning;
    private float timeElapsed = 0;
    private float fuel;
    private float minutes;
    private float seconds;

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

    public bool roundTransition = false;
    public Vector3 transitionStart;

    public GameObject fader;

    // Use this for initialization
    void Start()
	{
		Time.timeScale = 1;
        keypointIDs = new List<GameObject>();
        Reset("Tap to Start");
    }

    // called on loss
    void GameOver()
    {
        Reset("Game over\nscore: " + score + "\n time elapsed: " + 
            string.Format("{0:00}:{1:00}", minutes, seconds) + "\ntap to start");
    }

    // clean up the game
    void Reset(string proceedText)
    {
		score = 0;
		timeElapsed = 0f;
		randRange = 20f;
        gameRunning = false;
        roundTransition = false;
        fuel = 10f;
        player.transform.position = new Vector3(0, 0, 0);
        foreach (GameObject go in keypointIDs)
        {
            Destroy(go.gameObject);
        }
        keypointIDs = new List<GameObject>(); // needed to clear the list
		NewRound();
        centerText.text = proceedText;
		fader.GetComponent<Fader>().SetTween(new Color(0 / 255f, 0 / 255f, 0 / 255f), Tween.tweenMode.FADE_IN, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        // Listen for esc to quit
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("escape pressed");
            Reset("Tap to Start");
        }

        // don't update during round transitions
        if (roundTransition)
            return;

        // Update HUD time
		if (gameRunning) timeElapsed += Time.deltaTime;
        scoreText.text = "SCOrE " + score;
        roundText.text = "LEVEL " + keypointIDs.Count;
        minutes = timeElapsed / 60;
        seconds = timeElapsed % 60;
        //var fraction = (timeElapsed * 100) % 100;
        //timerText.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
        //timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Handle fuel tickdown
		if (gameRunning && playerMoveSpeed > 0)
        {
			fuel = Mathf.Max(0, fuel - Time.deltaTime);
            fuelText.text = fuel.ToString("#.00");
            
			if (fuel == 0)
                GameOver();
        }

        // HUD prompt for acceleration at start of game
        if (gameRunning) {
            if (timeElapsed < 5f)
                centerText.text = "Hold to accelerate";
            else
                centerText.text = "";
        }

        // Listen for tap to start
        if (!gameRunning) {
            if (Input.GetMouseButton(0) || Input.GetButton("Fire1"))
            {
                gameRunning = true;
            }
            else
            {
                return;
            }
		}            

        // Movement
		PlayerMove();

		// debug distribution
		//		SpawnRandomKeyPoint (randRange);
        // Spawning
		if (nextKeypointIndex == keypointIDs.Count)
			NewRound();
    }


    void NewRound()
    {
		fuel += 0f;
		nextKeypointIndex = 0;
		switch (keypointIDs.Count)
		{
		case 0:
			SpawnKeyPoint(Vector3.Normalize(Camera.main.transform.forward) * 10f);
			break;
		case 1:
			Vector3 newPos = keypointIDs[0].transform.position;
			newPos += Vector3.Normalize(Camera.main.transform.forward) * Random.Range(randRange / 3, randRange / 2);
			newPos += Vector3.Normalize(Camera.main.transform.up) * Random.Range(-randRange / 2, randRange / 2);
			newPos += Vector3.Normalize(Camera.main.transform.right) * Random.Range(-randRange / 2, randRange / 2);

			SpawnKeyPoint(newPos);
			break;
//		case 2:
//			SpawnKeyPoint(RandomPoint(randRange / 2));
//			break;
		default:
			SpawnRandomKeyPoint(randRange);
			break;
		}
		// volume grows slightly faster than # points
		randRange += randRangeStep; // code for uniform distr * Mathf.Pow(keypointIDs.Count, 0.4f);
		Debug.Log("RandRange = " + randRange);
		roundTransition = true;
        transitionStart = player.transform.position;
        foreach (GameObject go in keypointIDs)
        {
            go.GetComponent<MeshRenderer>().enabled = true;
        }
        //player.transform.position = new Vector3(0, 0, 0);
        nextKeypointIndex = 0;
		playerMoveSpeed = 0f;
        if (keypointIDs.Count > 3)
        {
            centerText.text = "Good job!";
        }
        else
        {
            centerText.text = "Tap to Continue";
        }
    }

    public bool CheckKeyPointCollision(GameObject go)
    {
		if (roundTransition)
			return false;
		
        if (keypointIDs[nextKeypointIndex] == go&&!roundTransition) {
            nextKeypointIndex++;
            score++;
			fuel += nextKeypointIndex * randRangeStep; // increase fuel when object picked up
			return true;
        } else {
            //game over
            GameOver();
            return false;
        }
    }

	void FixedUpdate()
	{
	    if (roundTransition)
		{
            if (player.transform.position.magnitude != 0)
            {
                player.transform.position *= (1f - (1.5f * Time.deltaTime) / (player.transform.position.magnitude));
                player.transform.position *= 1f - 1.5f * Time.deltaTime;
            }

			if (player.transform.position.magnitude <= 0.05f)
			{
				player.transform.position = Vector3.zero;
				player.GetComponent<Rigidbody>().velocity = Vector3.zero;
				playerMoveSpeed = 0f;
				roundTransition = false;
			}
		}
	}

	void SpawnRandomKeyPoint(float range) {
		bool invalidPoint;
		Vector3 point;
		do {
			point = keypointIDs[keypointIDs.Count -1].transform.position + RandomPoint(range/3);
			invalidPoint = false;
			foreach (GameObject go in keypointIDs) {
				if (Vector3.Distance(go.transform.position, point) < 5f) {
					invalidPoint = true;
					break;
				}
			}						
		} while (invalidPoint);
		SpawnKeyPoint (point);
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
			float playerAccel = 0.2f * Mathf.Pow (playerMoveSpeed, 0.3f) + 0.02f * Mathf.Pow(1.1f, playerMoveSpeed);
			playerAccel = Mathf.Min (10, playerAccel);
			playerMoveSpeed = Mathf.Max (1f, playerMoveSpeed + playerAccel);
		}

		if (playerMoveSpeed > 0f) { // don't start moving until tap
			float playerAccel = 0.01f * Mathf.Pow(playerMoveSpeed/2f, 0.6f);
			playerMoveSpeed = Mathf.Max (1f, playerMoveSpeed - playerAccel);
		}

		// make it harder to escape
		if (playerMoveSpeed > 10f) playerMoveSpeed *= 0.99f;
		if (playerMoveSpeed > 60f) playerMoveSpeed *= 0.99f;
		if (playerMoveSpeed > 90f) playerMoveSpeed *= 0.90f;
		playerMoveSpeed = Mathf.Min (1000, playerMoveSpeed);

		player.transform.position = player.transform.position + Camera.main.transform.forward * playerMoveSpeed * Time.deltaTime;
    }
}
