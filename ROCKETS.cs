
using UnityEngine;
public 
  class ROCKETS : MonoBehaviour
{//KINDA SIMILAR TO BULLETS.CS
    
    public AudioSource audioSource;
    public float speed = 350.0f;
    public float maxLifetime = 10.0f;

    private Rigidbody2D _rigidbody;
    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    public void Project(Vector2 direction)
    {
        _rigidbody.AddForce(direction * this.speed);
        Destroy(this.gameObject, this.maxLifetime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioSource.Play();
        Destroy(this.gameObject);
    }

}
