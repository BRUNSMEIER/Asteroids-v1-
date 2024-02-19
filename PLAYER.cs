
using UnityEngine;

public class PLAYER : MonoBehaviour
{
    public int hp = 100;
    public AudioClip sound3;
    public AudioSource audioSource1;
    public AudioSource audioSource;
    SpriteRenderer spriteRenderer;
    public Bullets bulletprefab;
    public ROCKETS rocketprefab;
    public float thrustSpeed = 1.0f;
    public float brakeSpeed = 0.78f;
    public float turnSpeed = 1.0f;
    private Rigidbody2D _rigidbody;
    public AudioClip sound1;
    public AudioClip sound2;
    private bool _thrusting;
    private bool _braking;
    private float _turnDirection;
    // Start is called before the first frame update
    public float respawnDelay = 3f;
    public float respawnInvulnerability = 3f;
    private int numofrockets = 3;
    public Sprite ONFIRESPACESHIP;
    public Sprite original;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {   
        SpriteRenderer spriteRenderer;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        _braking = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            
            _turnDirection = 1.0f;
        }else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { _turnDirection = -1.0f; }
        else { _turnDirection = 0.0f; }
        if (_thrusting || _braking || _turnDirection != 0f)
        {   

            spriteRenderer.sprite = ONFIRESPACESHIP;

        }//sets sprite renderers sprite
        else { spriteRenderer.sprite = original; }
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) { Shoot(); }
        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(1)) { ROCKETLAUNCH();
            numofrockets = numofrockets - 1;
            
        }

    }

    private void OnEnable()
    {
        // Turn off collisions for a few seconds after spawning to ensure the
        // player has enough time to safely move away from asteroids
        //gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        //Invoke(nameof(TurnOnCollisions), respawnInvulnerability);
    }
   

     private void FixedUpdate()
    {if (_thrusting)
        { _rigidbody.AddForce(this.transform.up * this.thrustSpeed); }
    if (_braking) { _rigidbody.AddForce(-this.transform.up * this.brakeSpeed); }
    if(_turnDirection != 0.0f) { _rigidbody.AddTorque(_turnDirection * this.turnSpeed); }

    }
    private void Shoot()
    {
        Bullets bullet = Instantiate(this.bulletprefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
        audioSource.clip = sound1;
        audioSource.Play();
    }
    private void ROCKETLAUNCH()
    {
        //if (numofrockets > 1)(ifyoulike to restrict the number of missiles)
        //{
            ROCKETS rocket = Instantiate(this.rocketprefab, this.transform.position, this.transform.rotation);
            rocket.Project(this.transform.up);
          audioSource1.clip = sound2;
          audioSource1.Play();
        //}
    }
    
 
    private void TurnOnCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0f;
            gameObject.SetActive(false);
            audioSource1.clip = sound3;
            audioSource1.Play();
            FindObjectOfType<GameManager>().PlayerDeath(this);
        }
        if (collision.gameObject.CompareTag("Bullet")) { if (hp > 25) { hp = hp - 25; }
            if (hp <= 0) {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = 0f;
                gameObject.SetActive(false);
                audioSource1.clip = sound3;
                audioSource1.Play();
                FindObjectOfType<GameManager>().PlayerDeath(this);
            }
        }
    }
}
