
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]

public class Asteroid : MonoBehaviour
{
    private Vector2 lastVelocity;
    private float boundaryOffset = 0.1f;
    public bool firstSpawn = true;
    public AudioSource sc3;
    public AudioSource sc1;
    public AudioSource sc2;
    public AudioClip sound6;
    public AudioClip sound4;
    public AudioClip sound5;
    public Sprite[] sprites;
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float movementSpeed = 50f;
    public float maxLifetime = 30f;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;
        _rigidbody.mass = this.size;
        // Destroy the asteroid after it reaches its max lifetime
        Destroy(gameObject, maxLifetime);
    }
    private void FixedUpdate()
    {
        lastVelocity = _rigidbody.velocity;
    }

    public void SetTrajectory(Vector2 direction)
    {
        // The asteroid only needs a force to be added once since they have no
        // drag to make them stop moving
        _rigidbody.AddForce(direction * movementSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            sc1.clip = sound4;
            sc1.Play();
            // Check if the asteroid is large enough to split in half
            // (both parts must be greater than the minimum size)
            if ((size * 0.75f) >= minSize)
            {
                CreateSplit();
                CreateSplit();
            }
            FindObjectOfType<GameManager>().AsteroidDestroyed(this);


            // Destroy the current asteroid since it is either replaced by two
            // new asteroids or small enough to be destroyed by the bullet
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Rocket"))
        {
            sc2.clip = sound5;
            sc2.Play();
            FindObjectOfType<GameManager>().AsteroidDestroyed(this);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {

            Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();

            Vector2 relativeVelocity = _rigidbody.velocity - otherRb.velocity;
            Vector2 normal = collision.contacts[0].normal;

            float impulse = Vector2.Dot(relativeVelocity, normal) * 2.0f / (_rigidbody.mass + otherRb.mass);

            Vector2 impulseForce = impulse * normal;

            _rigidbody.velocity -= _rigidbody.mass * impulseForce;
            otherRb.velocity += otherRb.mass * impulseForce;
        }
        else if (collision.gameObject.CompareTag("Boundary"))
        {
            //if (firstSpawn == true)
            //{

            //  gameObject.layer = LayerMask.NameToLayer("Firstentry");



            //Invoke(nameof(TurnOnCollisions), 8f);
            //firstSpawn = false;

            //}
            //if (transform.position.y > collision.transform.position.y ||
            // transform.position.y < collision.transform.position.y)
            //    {
            //      firstSpawn = false;
            //}
            //Vector2 surfaceNormal = collision.contacts[0].normal;
            ///_rigidbody.velocity = Vector2.Reflect(lastVelocity, surfaceNormal);
            //else if
            // {

            //Vector2 boundaryCenter = collider.bounds.center;
            //transform.position = new Vector2(boundaryCenter.x + boundaryOffset, boundaryCenter.y + boundaryOffset);

            //  Vector2 reflectDirection = -_rigidbody.velocity;

            // _rigidbody.AddForce(reflectDirection * 50f);
            //}(SOME OF THE ORIGINAL STUFF TRYING TO SIMULATE BOUNCE WITH SCRIPT CODES)
            //}

        }
    }

        
    

    private Asteroid CreateSplit()
    {
        // Set the new asteroid poistion to be the same as the current asteroid
        // but with a slight offset so they do not spawn inside each other
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;
        sc3.clip = sound6;
        sc3.Play();
        // Create the new asteroid at half the size of the current
        Asteroid half = Instantiate(this, position, transform.rotation);
        half.size = size * 0.5f;

        // Set a random trajectory
        half.SetTrajectory(Random.insideUnitCircle.normalized);

        return half;
    }
    public void fasterstrongerOctane() { movementSpeed = movementSpeed + 30f;
    }
    private void TurnOnCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Asteroid");
    }
}
