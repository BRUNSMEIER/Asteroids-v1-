using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{   public float speed = 500.0f;
    public float maxLifetime = 10.0f;
    
    private Rigidbody2D _rigidbody;
    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    public void Project(Vector2 direction)
    {//SET DIRECTIONS ADD FORCES
        _rigidbody.AddForce(direction * this.speed);
        Destroy(this.gameObject, this.maxLifetime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {//WHEN IT HITS THINGS GET DESTROYED
        Destroy(this.gameObject);
    }
}
