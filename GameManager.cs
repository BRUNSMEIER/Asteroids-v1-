using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AISHIP aiship;
    public float spawnDistance = 7f;
    public AISHIP aishipPrefab;
    public AudioSource src2;
    public AudioClip bgm1;
    public AudioClip bgm2;
    public AudioClip bgm3;
    public AudioSource audioSource;
    public PLAYER player;
    public ParticleSystem explosionEffect;
    public GameObject gameOverUI;
    public GameObject gameStartUI;
    public GameObject LevelUPUI;
    public GameObject INSTRUCTIONS;
    public GameObject PAUSED;
    public int score { get; private set; }
    public Text scoreText;
    public int flag2 = 0;//levelupflag
    public int flag1 = 0;// instruction flag
    public int flag3 = 0;//start flag
    public int flag4 = 0;// timefreezed flag
    public int flag5 = 0;// aiship flag
    public int lives { get; private set; }
    public Text livesText;

    private void Start()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        //audioSource = GetComponent<AudioSource>();
        PAUSED.SetActive(false);
        INSTRUCTIONS.SetActive(false);
        LevelUPUI.SetActive(false);
        gameStartUI.SetActive(true);
        gameOverUI.SetActive(false);
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        //aiship.SetActive(false);



    }

    private void Update()

    { // Play background music based on game state
        if (aiship.nextFireTime > 0.5f)
        { aiship.nextFireTime = aiship.nextFireTime - score / 20000; }
        
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        if (flag3 == 0)
        {
            audioSource.clip = bgm1;
            
        }
        if (flag3 == 1 && flag2 == 0)
        {
            audioSource.clip = bgm2;
           
        }
        if (flag2 == 1)
        {
            audioSource.clip = bgm3;
            
        }


        if (Input.GetKeyDown(KeyCode.I))
        {
            gameStartUI.SetActive(false);
            INSTRUCTIONS.SetActive(true)
              ;
            Time.timeScale = 0;
            flag1 = 1;      }
        if (Input.GetKeyDown(KeyCode.Escape)&&flag1==1&&flag3==0)
        {   
            INSTRUCTIONS.SetActive(false);
            gameStartUI.SetActive(true);
            Time.timeScale = 1;
            flag1 = 0;
        }else if(Input.GetKeyDown(KeyCode.Escape) && flag1 == 1 && flag3 == 1)
        {
            INSTRUCTIONS.SetActive(false);
            Time.timeScale = 1;
            flag1 = 0;
        }
        if (Input.GetKeyDown(KeyCode.P)&&flag3==1 )//&&flag3==1 &&flag4==0
        { if(Time.timeScale == 0) { Time.timeScale = 1;
                PAUSED.SetActive(false);
            } else { Time.timeScale = 0;
                PAUSED.SetActive(true);
            }

            
        }

        // Handle game state changes
        if (flag3==0&& Input.GetKeyDown(KeyCode.Return))
        { gameStartUI.SetActive(false);
            NewGame();
            flag3 = 1;
        }
        if (lives <=0 && Input.GetKeyDown(KeyCode.Return))
        {
            gameStartUI.SetActive(false);
            NewGame();
            flag3 = 1;
        }
        if (score >= 5000&&flag2==0)
        {
            flag2 = 1;
            LevelUPUI.SetActive(true);
            this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
           
        }
        if (Input.GetKeyDown(KeyCode.Return)&&flag2==1)
        {
            levelup();
            LevelUPUI.SetActive(false);
            this.player.Invoke(nameof(TurnOnCollisions), 3.0f);
        }
        if (flag2 == 1) { levelup(); }
       // if (score >= 1000&&flag5==0)
        //{
          //  aiship.SetActive(true);
            //flag5 = 1;
        //}
    }
    

    public void NewGame()
    {
        flag2 = 0;
        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();

        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i].gameObject);
        }

        gameOverUI.SetActive(false);

        SetScore(0);
        SetLives(3);
        Respawn();
    }

    public void playBGM()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (flag3 == 0)
        {
            audioSource.clip = bgm1;
            audioSource.Play();
        }
        if (flag3 == 1 && flag2 == 0)
        {
            audioSource.clip = bgm2;
            audioSource.Play();
        }
        if (flag2 == 1)
        {
            audioSource.clip = bgm3;
            audioSource.Play();
        }
    }
    public void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");

        this.player.gameObject.SetActive(true);
        this.player.Invoke(nameof(TurnOnCollisions),3.0f);

    }
    public void aiSpawn()
    {
        // Find the number of existing asteroids in the scene
        
        if (score >= 1000)
        {
            
            Vector2 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = spawnDirection * spawnDistance;

            // Offset spawn point to the position of the spawner
            spawnPoint += transform.position;
            // Calculate the position at which the asteroid will spawn
            float variance = Random.Range(-10f, 10f);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
            AISHIP aiship = Instantiate(aishipPrefab, spawnPoint,rotation);
            

        }
        // Spawn new asteroids if maximum asteroid count has not been reached


        // Clone the asteroid prefab and set its size randomly
        

                
        }
    

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        src2.Play();
        explosionEffect.transform.position = asteroid.transform.position;
        explosionEffect.Play();

        if (asteroid.size < 0.7f)
        {
            SetScore(score + 100); // small asteroid
        }
        else if (asteroid.size < 1.4f)
        {
            SetScore(score + 50); // medium asteroid
        }
        else
        {
            SetScore(score + 25); // large asteroid
        }
    }

    public void PlayerDeath(PLAYER player)
    {

        explosionEffect.transform.position = player.transform.position;
        explosionEffect.Play();

        SetLives(lives - 1);

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), player.respawnDelay);
        }
    }
    public void levelup()
    { Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
        for (int i = 0; i < asteroids.Length; i++)
        {
           asteroids[i].movementSpeed= 160f;
        }
    }
    private void TurnOnCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }

}
