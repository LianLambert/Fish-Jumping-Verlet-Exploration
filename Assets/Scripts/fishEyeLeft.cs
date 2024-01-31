using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishEyeLeft : MonoBehaviour
{
    [SerializeField] private GameObject fishLeft;
    [SerializeField] private GameObject fishBody;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float launchSpeed;
    [SerializeField] private float launchAngle;
    [SerializeField] public Vector2 velocity;
    public float bumpFactor = 0.05f;

    void Start()
    {
        // instantiate a fish body and set the reference to this eye
        fishBody = Instantiate(fishLeft, transform.position, transform.rotation);
        fishBody.GetComponent<fishLeft>().eye = this.gameObject;

        // launch the fish eye
        launchFish();

        // destroy the fish after an appropriate amount of time
        Destroy(gameObject, 10);
        Destroy(fishBody, 10);
    }

    // Update is called once per frame
    void Update()
    {
        // update position based on velocity and gravity
        velocity.y -= gravity * Time.deltaTime;
        transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;

        checkBounds();
        checkCollisions();
    }

    void launchFish()
    {
        // select random launch speed and angle
        launchSpeed = Random.Range(10, 15);
        launchAngle = Random.Range(50, 60);
        float radians = launchAngle * Mathf.Deg2Rad;

        // determine velocity vector
        velocity = new Vector2(launchSpeed * Mathf.Cos(radians), launchSpeed * Mathf.Sin(radians));
    }

    void checkBounds()
    {
        // destroy the fish if it is outside of the camera bounds
        if (Mathf.Abs(transform.position.y) >= 5.261 || Mathf.Abs(transform.position.x) >= 9.875)
        {
            Destroy(gameObject);
            Destroy(fishBody);
        }
    }

    void checkCollisions()
    {
        // check which colliders overlap the eye, ignoring the eye itself
        GetComponent<CircleCollider2D>().enabled = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.25f);
        GetComponent<CircleCollider2D>().enabled = true;

        // iterate through collision objects
        foreach (Collider2D col in colliders)
        {

            // if the fish eye collides with another fish eye
            if (col.gameObject.tag == "fishEyeLeft" || col.gameObject.tag == "fishEyeRight")
            {
                // find the vector between the two fish eyes and the normal vector
                Vector2 direction = col.transform.position - transform.position;
                Vector2 collisionNormal = direction.normalized;

                // calculate vector to move away from collision
                Vector2 moveAmount = collisionNormal * bumpFactor;

                // update position and velocity
                transform.position -= new Vector3(moveAmount.x, moveAmount.y, 0);
                velocity = new Vector3(-velocity.x, velocity.y);
            }

            // if the fish eye collides with the pool, destroy the fish eye
            else if (col.gameObject.tag == "pool")
            {
                Destroy(gameObject);
                Destroy(fishBody);
            }

            //  if the fish eye collides with the ground, decrease its x velocity and stop its y velocity
            else if (col.gameObject.tag == "ground")
            {
                velocity.x = velocity.x * bumpFactor;
                velocity.y = 0;

                // move the fish eye so that it does not penetrate the ground
                transform.position = new Vector3(transform.position.x, -2.74f, transform.position.z);
            }

            // if the fish eye collides with the triangle, have it bounce off and switch x velocities
            else
            {
                // move the fish eye so that it is not currently colliding with the triangle
                transform.position -= new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;
                velocity = new Vector3(-velocity.x, velocity.y + 0.5f);
            }
        }
    }
}
