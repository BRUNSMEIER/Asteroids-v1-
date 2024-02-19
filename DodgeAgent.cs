using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class DodgeAgent : Agent
{
    public AISHIP aiship;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private Rigidbody2D _rigidbody;
    public Transform player;

    // public GameObject aiship;
    public int Health;
    public override void Initialize()
    {
        aiship = GetComponent<AISHIP>();
        _rigidbody = aiship.GetComponent<Rigidbody2D>();
        startPosition = aiship.transform.position;
        targetPosition = player.position;
    }
    public override void OnEpisodeBegin()
    {
        _rigidbody = aiship.GetComponent<Rigidbody2D>();
        // Reset AI ship position and health
        aiship.transform.position = startPosition;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        aiship._health = 100;
    }

    public override void CollectObservations(VectorSensor sensor)
    {

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        Transform[] asteroidTransforms = new Transform[asteroids.Length];
        for (int i = 0; i < asteroids.Length; i++)
        {
            asteroidTransforms[i] = asteroids[i].transform;
        }

        // Observe AI ship position and velocity
        sensor.AddObservation(aiship.transform.position);
        sensor.AddObservation(_rigidbody.velocity);

        // Observe player and asteroid positions
        if (player != null)
        {
            sensor.AddObservation(player.position);
        }
        else
        {
            sensor.AddObservation(Vector2.zero);
        }

        if (asteroids != null)
        {
            for (int i = 0; i < asteroids.Length; i++)
            {
                sensor.AddObservation(asteroidTransforms[i].position);
            }

        }
        else
        {
            sensor.AddObservation(Vector2.zero);
        }

        // Observe AI ship health
        sensor.AddObservation(aiship._health / 100f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get continuous action outputs
        float horizontal = actions.ContinuousActions[0];
        float vertical = actions.ContinuousActions[1];

        // Move AI ship
        Vector3 newPosition = new Vector3(aiship.transform.position.x + horizontal, aiship.transform.position.y + vertical, aiship.transform.position.z);
        aiship.transform.position = Vector3.MoveTowards(transform.position, newPosition, 5f * Time.deltaTime);

        // Rotate AI ship towards target position
        Vector2 direction = targetPosition - aiship.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        _rigidbody.MoveRotation(angle);

        // Reward for getting closer to target position
        float distanceToTarget = Vector2.Distance(aiship.transform.position, targetPosition);
        AddReward(0.1f / distanceToTarget);
    }

}