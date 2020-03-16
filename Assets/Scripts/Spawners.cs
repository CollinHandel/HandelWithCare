using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using UnityEngine.Advertisements;

public class Spawners : MonoBehaviour {

    public int LevelNum;

    public GameObject WoodBox;
    public GameObject Bomb;
    public GameObject BombDefuserBox;
    public GameObject GoldBox;


    public Transform ScoreText;
    public Transform AdditionalText;
    public Transform HighScoreText;
    public GameObject GameOverText;
    public GameObject TotalNumberText;
    public GameObject BombDefuserGraphic;
    public GameObject SpeedAmpText;
    public GameObject BackgroundObject;
    public GameObject[] BoxSkins;

    private int skinVersion = 0;

    public Sprite[] BackgroundImages;
    public Color32[] BackgroundColor;


    public Transform Explosion;
    public Transform Particles;


    public bool BombDefuser = false;

    public float maxY;
    public float minX;
    public float maxX;
    public float bottomMaxY;
    public float bottomMinX;
    public float bottomMaxX;

    public int counter = 0;
    public int additionalPoints = 0;
    private float timer = 0;
    private float timerSpawn = 0;
    private float bombTimer;
    private float bombTimerSpawn;
    private Vector3 topLeft;
    private Vector3 topRight;
    private Vector3 bottomLeft;
    private Vector3 bottomRight;
    public bool gameOver = false;
    private bool GoldBoxes = false;
    public bool speedAmp = false;
    public int speedAmpNumber = 1000;

    public GameObject ChallengeCompleted;

    Ray ray;
    RaycastHit2D hit;

    bool paused = false;
    public GameObject PausePlayBtn;
    public Sprite Pause;
    public Sprite Play;

    private int difficultyLevel;

    // Use this for initialization
    void Start()
    {
        SetBackground();
        SetSkin();

        difficultyLevel = PlayerPrefs.GetInt("DifficultyLevel", 0);

        topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        minX = topLeft.x + .665f;
        maxX = topRight.x - .665f;
        maxY = topLeft.y + .8f;

        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        bottomMinX = bottomLeft.x + .665f;
        bottomMaxX = bottomRight.x - .665f;
        bottomMaxY = bottomLeft.y - .8f;

        AdditionalText.GetComponent<Text>().text = "";
        SpawnBox();
        ScoreText.GetComponent<Text>().text = counter.ToString();

        bombTimerSpawn = 8f;
        
        if (PlayerPrefs.GetInt("Upgrade0Bought") >= 1)
            GoldBoxes = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (counter >= speedAmpNumber && !speedAmp)
        {
            speedAmp = true;
            SpeedAmpText.SetActive(true);
            SpeedAmpText.GetComponent<FadeText>().enabled = true;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !gameOver && !paused)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);

            hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                if (hit.collider.transform.name == "WoodBox")
                {
                    counter++;
                    Destroy(hit.transform.gameObject);
                    return;
                } else if (hit.collider.transform.name == "GoldBox")
                {
                    counter++;
                    additionalPoints += hit.transform.GetComponent<GoldBox>().Score;
                    Destroy(hit.transform.gameObject);
                    return;
                } else if (hit.collider.transform.name == "Bomb")
                {
                    GameOver();
                    Instantiate(Explosion, hit.transform.position, Quaternion.identity);
                    Destroy(hit.transform.gameObject);
                    return;
                } else if (hit.collider.transform.name == "BombDefuserBox")
                {
                    counter++;
                    BombDefuser = true;
                    BombDefuserGraphic.GetComponent<BombDefuser>().ActivateDefuser();
                    Destroy(hit.transform.gameObject);
                    return;
                }
            }
        }
#endif

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !gameOver && !paused)
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            Debug.DrawRay(ray.origin, ray.direction * 20, Color.red);

            hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                if (hit.collider.transform.name == "WoodBox")
                {
                    counter++;
                    Destroy(hit.transform.gameObject);
                    return;
                }
                else if (hit.collider.transform.name == "GoldBox")
                {
                    counter++;
                    additionalPoints += hit.transform.GetComponent<GoldBox>().Score;
                    Destroy(hit.transform.gameObject);
                    return;
                }
                else if (hit.collider.transform.name == "Bomb")
                {
                    GameOver();
                    Instantiate(Explosion, hit.transform.position, Quaternion.identity);
                    Destroy(hit.transform.gameObject);
                    return;
                }
                else if (hit.collider.transform.name == "BombDefuserBox")
                {
                    counter++;
                    BombDefuser = true;
                    BombDefuserGraphic.GetComponent<BombDefuser>().ActivateDefuser();
                    Destroy(hit.transform.gameObject);
                    return;
                }
            }
        }

        if (!gameOver)
        {
            SpawnerTime();
            SpawnBomb();
            SpawnBox();
            UpdateAdditionalPoints();
        }
        MoveAdditionalPoints();

        ScoreText.GetComponent<Text>().text = counter.ToString();
    }

    public void PlayAndPause()
    {
        if (!gameOver)
        {
            if (!paused)
            {
                paused = true;
                Time.timeScale = 0.0f;
                PausePlayBtn.GetComponent<Image>().sprite = Play;
            }
            else
            {
                paused = false;
                Time.timeScale = 1.0f;
                PausePlayBtn.GetComponent<Image>().sprite = Pause;
            }
        }
    }

    void SpawnBox()
    {
        if (timer >= timerSpawn && !gameOver)
        {
            GameObject go;

            float i = Random.value;
            float RandomDirection = Random.value;

            if (i <= PlayerPrefs.GetFloat("Upgrade0Rate") && GoldBoxes)
            {
                if (RandomDirection <= .5f && LevelNum != 2 || LevelNum == 1)
                {
                    go = (GameObject)Instantiate(GoldBox, new Vector2(Random.Range(minX, maxX), maxY), Quaternion.identity);
                    go.name = "GoldBox";
                    go.GetComponent<GoldBox>().Direction = 0;
                    go.transform.SetParent(this.transform);
                }
                else if (RandomDirection > .5f || LevelNum == 2)
                {
                    go = (GameObject)Instantiate(GoldBox, new Vector2(Random.Range(bottomMinX, bottomMaxX), bottomMaxY), Quaternion.identity);
                    go.name = "GoldBox";
                    go.GetComponent<GoldBox>().MaxY = topLeft.y;
                    go.GetComponent<GoldBox>().Direction = 1;
                    go.transform.SetParent(this.transform);
                }
            }
            else if (i >= .95f && !BombDefuser && PlayerPrefs.HasKey("Upgrade2Bought"))
            {
                if (RandomDirection <= .5f && LevelNum != 2 || LevelNum == 1)
                {
                    go = (GameObject)Instantiate(BombDefuserBox, new Vector2(Random.Range(minX, maxX), maxY), Quaternion.identity);
                    go.name = "BombDefuserBox";
                    go.GetComponent<BombDefuserBox>().Direction = 0;
                    go.transform.SetParent(this.transform);
                }
                else if (RandomDirection > .5f || LevelNum == 2)
                {
                    go = (GameObject)Instantiate(BombDefuserBox, new Vector2(Random.Range(bottomMinX, bottomMaxX), bottomMaxY), Quaternion.identity);
                    go.name = "BombDefuserBox";
                    go.GetComponent<BombDefuserBox>().MaxY = topLeft.y;
                    go.GetComponent<BombDefuserBox>().Direction = 1;
                    go.transform.SetParent(this.transform);
                }
            }
            else
            {
                if (RandomDirection <= .5f && LevelNum != 2 || LevelNum == 1)
                {
                    go = (GameObject)Instantiate(BoxSkins[skinVersion], new Vector2(Random.Range(minX, maxX), maxY), Quaternion.identity);
                    go.name = "WoodBox";
                    go.GetComponent<BoxScript>().Direction = 0;
                    go.transform.SetParent(this.transform);
                }
                else if (RandomDirection > .5f || LevelNum == 2)
                {
                    go = (GameObject)Instantiate(BoxSkins[skinVersion], new Vector2(Random.Range(bottomMinX, bottomMaxX), bottomMaxY), Quaternion.identity);
                    go.name = "WoodBox";
                    go.GetComponent<BoxScript>().MaxY = topLeft.y;
                    go.GetComponent<BoxScript>().Direction = 1;
                    go.transform.SetParent(this.transform);
                }
            }
            timer = 0;
        }
        timer += Time.deltaTime;
    }

    void SpawnBomb()
    {
        if (bombTimer >= bombTimerSpawn && !BombDefuser)
        {
            float RandomDirection = Random.value;
            if (RandomDirection <= .5f || LevelNum == 1)
            {
                GameObject go = (GameObject)Instantiate(Bomb, new Vector3(Random.Range(minX, maxX), maxY, .5f), Quaternion.identity);
                go.transform.SetParent(this.transform);
                go.name = "Bomb";
                go.GetComponent<BombScript>().Direction = 0;
            }
            else if (RandomDirection > .5f || LevelNum == 2)
            {
                GameObject go = (GameObject)Instantiate(Bomb, new Vector3(Random.Range(bottomMinX, bottomMaxX), bottomMaxY, .5f), Quaternion.identity);
                go.transform.SetParent(this.transform);
                go.name = "Bomb";
                go.GetComponent<BombScript>().MaxY = topLeft.y;
                go.GetComponent<BombScript>().Direction = 1;
            }

            bombTimer = 0;
            BombSpawnerTime();
        }
        bombTimer += Time.deltaTime;
    }

    public void GameOver()
    {
        gameOver = true;

        ShowAd();

        if (PlayerPrefs.GetInt("Level" + LevelNum.ToString() + "HighScore", 0) < counter) 
        {
            PlayerPrefs.SetInt("Level" + LevelNum.ToString() + "HighScore", counter);
        }

        HighScoreText.GetComponent<Text>().text = PlayerPrefs.GetInt("Level" + LevelNum.ToString() + "HighScore").ToString();

        int TotalBoxesCollected = PlayerPrefs.GetInt("TotalBoxesCollected", 0);
        TotalBoxesCollected += counter;
        PlayerPrefs.SetInt("TotalBoxesCollected", TotalBoxesCollected);

        int BoxCount = PlayerPrefs.GetInt("TotalBoxCount");
        BoxCount += counter;
        PlayerPrefs.SetInt("TotalBoxCount", BoxCount);

        TotalNumberText.GetComponent<Text>().text = PlayerPrefs.GetInt("TotalBoxCount").ToString();
        BoxCount += additionalPoints;
        PlayerPrefs.SetInt("TotalBoxCount", BoxCount);

        CheckChallengeCompletion();
        GooglePlayStatistics();

        GameOverText.SetActive(true);
    }

    void ShowAd()
    {
        float i = Random.Range(0f, 100f);
        if (i <= 10f)
        {
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }
        }
    }

    void GooglePlayStatistics()
    {

        if (Social.localUser.authenticated)
        {
            // Check for Look Mom, I Did It! Achievement
            if (PlayerPrefs.GetInt("TotalBoxCount", 0) >= 10)
            {
                Social.ReportProgress("CgkIvbbbuOQaEAIQBA", 100.0f, (bool success) =>
                {

                });
            }


            // Check for 100000 BOXES achievement
            if (PlayerPrefs.GetInt("TotalBoxCount", 0) >= 100000)
            {
                Social.ReportProgress("CgkIvbbbuOQaEAIQAg", 100.0f, (bool success) =>
                {

                });
            }


            // Post On Hand Box Count to Leaderboard
            Social.ReportScore(PlayerPrefs.GetInt("TotalBoxCount", 0), "CgkIvbbbuOQaEAIQCg", (bool success) =>
            {

            });


            // Post Score to Leaderboard
            if (LevelNum == 1)
            {
                Social.ReportScore(PlayerPrefs.GetInt("Level1HighScore", 0), "CgkIvbbbuOQaEAIQBg", (bool success) =>
                {

                });
            } else if (LevelNum == 2)
            {
                Social.ReportScore(PlayerPrefs.GetInt("Level2HighScore", 0), "CgkIvbbbuOQaEAIQCA", (bool success) =>
                {

                });
            } else if (LevelNum == 3)
            {
                Social.ReportScore(PlayerPrefs.GetInt("Level3HighScore", 0), "CgkIvbbbuOQaEAIQCQ", (bool success) =>
                {

                });
            }
        }
    }

    void CheckChallengeCompletion()
    {
        int spawningText = 0;
        ChallengeInfo Info = this.GetComponent<ChallengeInfo>();

        for (int i = 1; i <= 3; i++)
        {
            int levelChallenge = PlayerPrefs.GetInt("Level" + i + "Challenge", 0);

            if (PlayerPrefs.GetInt("Level" + i.ToString() + "Challenge", 0) < Info.ReturnLength(i))
            {
                if (PlayerPrefs.GetInt("Level" + i.ToString() + "HighScore", 0) >= Info.LevelChallenge(i, levelChallenge))
                {
                    PlayerPrefs.SetInt("Level" + i + "ChallengeAwaiting", 1);
                    SpawnCompletedText("Level " + i + " Challenge Completed", spawningText);
                    spawningText++;
                }
            }
        }

        if (PlayerPrefs.GetInt("TotalBoxesCollected", 0) >= Info.TotalBoxesResult())
        {
            if (PlayerPrefs.GetInt("TotalBoxesCollectedVersion", 0) < Info.ReturnLength(0))
            {
                PlayerPrefs.SetInt("TotalBoxesCollectedAwaiting", 1);
                SpawnCompletedText("Total Boxes Challenge Completed", spawningText);
                spawningText++;
            }
        }
        
    }

    void SpawnCompletedText(string text, int spawnNum)
    {
        GameObject go = (GameObject)Instantiate(ChallengeCompleted, ChallengeCompleted.transform);

        foreach (Transform child in go.transform)
        {
            Destroy(child.gameObject);
        }
        
        go.GetComponent<Text>().text = text;
        go.GetComponent<MoveChallengeCompleted>().counter = spawnNum;
    }

    void SetBackground()
    {
        int background = PlayerPrefs.GetInt("BackgroundSelected");
        GameObject mainCamera = GameObject.Find("Main Camera");

        if (background == 0)
        {
            BackgroundObject.GetComponent<Image>().color = new Color32(255, 255, 255, 164);
        }
        
        BackgroundObject.GetComponent<Image>().sprite = BackgroundImages[background];
        mainCamera.GetComponent<Camera>().backgroundColor = BackgroundColor[background];
    }

    void SetSkin()
    {
        skinVersion = PlayerPrefs.GetInt("SkinSelected", 0);
    }

    void UpdateAdditionalPoints()
    {
        if (additionalPoints > 0)
        {
            AdditionalText.GetComponent<Text>().text = "+" + additionalPoints;
        }
    }

    void MoveAdditionalPoints()
    {
        if (gameOver && additionalPoints > 0)
        {
            if (Vector3.Distance(AdditionalText.transform.position, TotalNumberText.transform.position) > 2)
            {
                AdditionalText.transform.position = Vector3.MoveTowards(AdditionalText.transform.position, TotalNumberText.transform.position, Time.deltaTime * 10f);
            }
            else
            {
                AdditionalText.gameObject.SetActive(false);
                TotalNumberText.GetComponent<Text>().text = PlayerPrefs.GetInt("TotalBoxCount").ToString();
                Particles.gameObject.SetActive(true);
            }
        }
    }

    void SpawnerTime()
    {
        if (difficultyLevel == 0)
        {
            if (counter <= 0)
                timerSpawn = 3f;
            else if (counter <= 1)
                timerSpawn = 2.5f;
            else if (counter <= 3)
                timerSpawn = 2f;
            else if (counter <= 7)
                timerSpawn = 1.5f;
            else if (counter <= 15)
                timerSpawn = 1f;
            else if (counter <= 25)
                timerSpawn = .75f;
            else if (counter <= 40)
                timerSpawn = .5f;
            else if (counter <= 60)
                timerSpawn = .4f;
            else if (counter < 100)
                timerSpawn = .35f;
            else if (counter >= 100)
                timerSpawn = .3f;
            else if (counter >= 500)
                timerSpawn = .23f;
        } else if (difficultyLevel == 1)
        {
            if (counter <= 0)
                timerSpawn = 2f;
            else if (counter <= 3)
                timerSpawn = 1.5f;
            else if (counter <= 7)
                timerSpawn = 1f;
            else if (counter <= 15)
                timerSpawn = .75f;
            else if (counter <= 25)
                timerSpawn = .5f;
            else if (counter <= 40)
                timerSpawn = .4f;
            else if (counter <= 60)
                timerSpawn = .35f;
            else if (counter < 100)
                timerSpawn = .3f;
            else if (counter >= 100)
                timerSpawn = .23f;
            else if (counter >= 500)
                timerSpawn = .2f;
        } else if (difficultyLevel == 2)
        {
            if (counter <= 0)
                timerSpawn = 1.5f;
            else if (counter <= 3)
                timerSpawn = 1f;
            else if (counter <= 7)
                timerSpawn = .75f;
            else if (counter <= 15)
                timerSpawn = .5f;
            else if (counter <= 25)
                timerSpawn = .4f;
            else if (counter <= 40)
                timerSpawn = .35f;
            else if (counter <= 60)
                timerSpawn = .3f;
            else if (counter < 100)
                timerSpawn = .23f;
            else if (counter >= 100)
                timerSpawn = .2f;
            else if (counter >= 500)
                timerSpawn = .17f;
        }

        if (LevelNum == 3 && counter > 20)
        {
            timerSpawn += .1f;
        }

        return;
    }

    void BombSpawnerTime()
    {
        if (counter <= 10)
        {
            bombTimerSpawn = Random.Range(2f, 6f);
        } else if (counter <= 25)
        {
            bombTimerSpawn = Random.Range(1f, 4f);
        } else if (counter <= 50)
        {
            bombTimerSpawn = Random.Range(.5f, 4f);
        } else if (counter > 50)
        {
            bombTimerSpawn = Random.Range(.5f, 3f);
        }
    }
}
