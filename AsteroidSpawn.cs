using UnityEngine;

public class AsteroidSpawn : MonoBehaviour
{
    public Collider2D boundaryCollider; // Collider used to bound the playable area of the game
    public int maxnum = 10; // Maximum number of asteroids allowed
    public Asteroid asteroidPrefab; // Asteroid prefab
    public float spawnDistance = 12f; // Distance at which asteroids spawn
    public float spawnRate = 1f; // Rate at which asteroids spawn
    public int spawnAmount = 1; // Number of asteroids spawned per spawn event
    [Range(0f, 45f)]
    public float trajectoryVariance = 15f; // Degree of variance in asteroid trajectory
    public int flag1 = 0;

    
// Start is called before the first frame update
     private void Start()
    {
        // Call Spawn() function every spawnRate seconds
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }

    private void Spawn()
    {
        // Find the number of existing asteroids in the scene
        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
        int asteroidCount = asteroids.Length;

        // Spawn new asteroids if maximum asteroid count has not been reached
        if (asteroidCount <= maxnum)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                // Generate a random direction for the asteroid to spawn in and normalize it
                Vector2 spawnDirection = Random.insideUnitCircle.normalized;

                // Calculate the position at which the asteroid will spawn
                Vector3 spawnPoint = spawnDirection * spawnDistance;

                // Offset spawn point to the position of the spawner
                spawnPoint += transform.position;

                // Generate a random trajectory variance and calculate rotation angle
                float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
                Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

                // Clone the asteroid prefab and set its size randomly
                Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
                asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

                // Set the asteroid's trajectory
                Vector2 trajectory = rotation * -spawnDirection;
                asteroid.SetTrajectory(trajectory);
            }
        }
    }
}



