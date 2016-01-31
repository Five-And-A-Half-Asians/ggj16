using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainScene : MonoBehaviour {
    // with the all-uppercase Elemental End typeface, the capital letters are used to denote stylization when available (notably, A and E look good)
    private string UI_TAP_TO_START = "tAp to stArt";
    private string UI_TAP_TO_CONTINUE = "TAp to Continue";
    private string UI_GAME_OVER = "gAmE ovEr";
    private string UI_SCORE = "scorE";
    private string UI_TIME = "timE";
    private string UI_LEVEL = "lEvEl";
    private string UI_HOLD_TO_ACCELERATE = "hold to AccElErAtE";
    private string UI_GOOD_JOB = "good job!";

    public GameObject player;
    public GameObject particleEmitter;
    public ParticleSystem accelEmitter;
    public ParticleSystem celebratoryEmitter;
    public Text roundText;
    public Text scoreText;
    public Text centerText;
    public Text fuelText;

	float randRange = 10f; // real value set in Reset()
	float randRangeStep = 0.5f;

	float minDist = 3f;
    
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
    private int? lastColor = null;

    // Use this for initialization
    void Start()
	{
		Time.timeScale = 1;
        keypointIDs = new List<GameObject>();
        Reset(UI_TAP_TO_START);
    }

    // called on loss
    void GameOver()
    {
        Reset(UI_GAME_OVER + "\n" +
                UI_SCORE + ": " + score + "\n" +
                UI_TIME + ": " + string.Format("{0:00}:{1:00}", minutes, seconds) + "\n" +
                UI_TAP_TO_START);
    }

    // clean up the game
    void Reset(string proceedText)
    {
		score = 0;
		timeElapsed = 0f;
		randRange = 10f;
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
        particleEmitter.GetComponent<TrailRenderer>().Clear();
        particleEmitter.GetComponent<TrailRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Listen for esc to quit
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButton("Cancel"))
        {
            Reset(UI_TAP_TO_START);
        }

        // don't update during round transitions
        if (roundTransition)
            return;

        // Update HUD time
		if (gameRunning) timeElapsed += Time.deltaTime;
        scoreText.text = UI_SCORE + "\n" + score;
        roundText.text = UI_LEVEL + "\n" + keypointIDs.Count;
        minutes = timeElapsed / 60;
        seconds = timeElapsed % 60;
        //var fraction = (timeElapsed * 100) % 100;
        //timerText.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);

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
                centerText.text = UI_HOLD_TO_ACCELERATE;
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
//        SpawnRandomKeyPoint (randRange);
        // Spawning
        if (nextKeypointIndex == keypointIDs.Count)
            NewRound();
    }


    void NewRound()
    {
		fuel += randRangeStep;
		nextKeypointIndex = 0;
		fader.GetComponent<Fader>().SetTween(new Color(1f, 1f, 1f), 0.07f, 0.0f, 1f, Tween.tweenMode.FADE_IN, 0.05f);

		switch (keypointIDs.Count)
		{
		case 0:
			fader.GetComponent<Fader>().SetTween(new Color(0f, 0f, 0f), 1f, 0.0f, 1f, Tween.tweenMode.FADE_IN, 0.6f);
			SpawnKeyPoint(Vector3.Normalize(Camera.main.transform.forward) * randRange);
			break;
		case 1:
			Vector3 newPos = keypointIDs[0].transform.position;
			newPos += Vector3.Normalize(Camera.main.transform.forward) * Random.Range(0.5f * randRange, randRange);
			newPos += Vector3.Normalize(Camera.main.transform.up) * Random.Range(-randRange * 0.5f , randRange * 0.5f);
			newPos += Vector3.Normalize(Camera.main.transform.right) * Random.Range(-randRange * 0.5f, randRange * 0.5f);

			SpawnKeyPoint(newPos);
			break;
//		case 2:
//			for (int i = 0; i < 10; i++) {
//				SpawnRandomKeyPoint (randRange);
//			}
//			break;
		default:
			SpawnRandomKeyPoint(randRange);
			break;
		}
		// volume grows slightly faster than # points
		randRange += randRangeStep; // code for uniform distr * Mathf.Pow(keypointIDs.Count, 0.4f);
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
            centerText.text = UI_GOOD_JOB;
        }
        else
        {
            centerText.text = UI_TAP_TO_CONTINUE;
        }
    }

    public bool CheckKeyPointCollision(GameObject go)
    {
		if (roundTransition)
			return false;
		
        if (keypointIDs[nextKeypointIndex] == go&&!roundTransition) {
            PlayCollectJingle();
            nextKeypointIndex++;
            score++;
			fuel += 0.4f * (minDist + nextKeypointIndex); // increase fuel when object picked up
			fader.GetComponent<Fader>().SetTween(new Color(1f, 1f, 1f), 0.03f, 0.0f, 1f, Tween.tweenMode.FADE_IN, 0.02f);
			return true;
        } else {
            //game over
            GameOver();
            return false;
        }
    }

    void PlayCollectJingle()
    {
        AudioSource pa = player.GetComponent<AudioSource>();
        //raises pitch by one note, but won't go above one octave
        pa.pitch = Mathf.Clamp(1f + (Mathf.Pow(Mathf.Pow(1.05946f, 2), nextKeypointIndex) - 1), 1f, 2f);
        pa.Play();
    }

	void FixedUpdate()
	{
	    if (roundTransition)
		{
            celebratoryEmitter.Emit(1);
            if (player.transform.position.magnitude != 0)
            {
                particleEmitter.GetComponent<TrailRenderer>().enabled = false;
                player.transform.position *= (1f - (1.5f * Time.deltaTime) / (player.transform.position.magnitude));
                player.transform.position *= 1f - 1.5f * Time.deltaTime;
            }

			if (player.transform.position.magnitude <= 0.05f)
			{
				player.transform.position = Vector3.zero;
				player.GetComponent<Rigidbody>().velocity = Vector3.zero;
				playerMoveSpeed = 0f;
				roundTransition = false;
                particleEmitter.GetComponent<TrailRenderer>().Clear();
                particleEmitter.GetComponent<TrailRenderer>().enabled = true;
            }
		} else
        {
            //celebratoryEmitter.Stop();
        }
	}

	void SpawnRandomKeyPoint(float range) {
		bool invalidPoint;
		Vector3 point;
		do {
			GameObject lastKeyPoint = keypointIDs[keypointIDs.Count -1];
			point = lastKeyPoint.transform.position + RandomPoint(range);
			invalidPoint = false;
			if (Vector3.Distance(lastKeyPoint.transform.position, point) < (minDist + randRange)/2f) {
				invalidPoint = true;
				continue;
			}
			foreach (GameObject go in keypointIDs) {
				if (Vector3.Distance(go.transform.position, point) < minDist) {
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
        if (lastColor != null)
        {
            while (c == lastColor)
                c = Random.Range(0, colors.Length);
        }
        lastColor = c;
        obj.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[c]);
        float r = Mathf.Clamp(colors[c].r - 0.5f, 0f, 1f);
        float g = Mathf.Clamp(colors[c].g - 0.5f, 0f, 1f);
        float b = Mathf.Clamp(colors[c].b - 0.5f, 0f, 1f);
        obj.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(r,g,b));
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
        if(playerMoveSpeed > 15f || roundTransition)
        {
            accelEmitter.startSpeed = playerMoveSpeed/2f;
            accelEmitter.Play();
        } else
        {
            accelEmitter.Stop();
        }
    }
}
