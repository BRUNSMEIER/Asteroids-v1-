using UnityEngine;


public class AISHIP : MonoBehaviour
{
    public float trajectoryVariance = 1.5f;
    public float speed = 2f;
    public float attackRange = 5f;
    public float fireRate = 1f;
    public float timeBetweenShots = 1f;
    public float nextFireTime = 2f;
    public Bullets bulletprefab;
    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    private Rigidbody2D _rigidbody;
    private float _turnDirection;
    public int _health = 100;
    private Transform player;

    private void Start()
    {   if (GameObject.FindGameObjectWithTag("Player") != null)
        { player = GameObject.FindGameObjectWithTag("Player").transform; }
            
    }

    private void Awake()
    {
        
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        //float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
        

        if (player == null)
            return;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    

        // move towards the player
      
        // attack the player if in range and not on cooldown
        if (Vector3.Distance(transform.position, player.position) <= attackRange && Time.time >= nextFireTime)
        {
            
            nextFireTime = Time.time + timeBetweenShots;
            Shoot();
            // add code to make the bullet move towards the player
        }
       
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(this.transform.up * this.thrustSpeed);
        _rigidbody.AddTorque(_turnDirection * this.turnSpeed);
    }

    private void Shoot()
    {
        Bullets bullet = Instantiate(this.bulletprefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            _health -= 25;
            if (_health <= 0)
            {
                Die();
            }
        }
       // if (collision.gameObject.CompareTag("Asteroid"))
        //{
            
          //  Die();
        //}

    }

    private void Die()
    {
        Destroy(gameObject);
    }
}