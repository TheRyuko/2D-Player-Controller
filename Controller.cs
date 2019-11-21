using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public abstract class Controller : MonoBehaviour
{
    //Add method read input
    public abstract void ReadInput(InputData data);

    protected Collider2D c2d;
    protected bool newInput;

    //Settings
    public float gravity;
    public float jumpVelocity;
    public float jumpHeight = 1f;
    public float timeToJumpApex = .4f;
    const float skinWidth = .015f;
    public float horizontalRayCount = 4f;
    public float verticalRayCount = 4f;
    float horizontalRaySpacing;
    float verticalRaySpacing;
    public LayerMask collisionMask;

    public float jumpTime = 0f;
    RaycastOrigins raycastOrigins;
    public CollisionsInfo collisions;

    public bool jump;

    void Awake()
    {
        c2d = GetComponent<Collider2D>();
    }
    private void Start()
    {
        CalculateRaySpacing();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity" + gravity + "Jump velocity" + jumpVelocity);
    }

    public void Move(Vector2 velocity)
    {

        UpdateRaycastOrigins();
        collisions.Reset();

        //Check for collisions only if trying to move
        if (velocity.x != 0f)
        {
            HorizontalCollisions(ref velocity);
        }
        
        //Check for collisions only if trying to move
        if (velocity.y != 0f)
        {
            VerticalCollisions(ref velocity);
        }

        //If  trying to jump, jump
        if (jump && jumpTime < timeToJumpApex)
        {
            velocity.y = jumpVelocity * Time.deltaTime;
            jumpTime += Time.deltaTime;
        }

        //If we have reached jump apex, fall
        if (jumpTime>=timeToJumpApex)
        {
            jump = false;
            jumpTime = 0f;
        }

        c2d.transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.bellow = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }
    void CalculateRaySpacing()
    {
        Bounds bounds = c2d.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = c2d.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public struct CollisionsInfo
    {
        public bool above, bellow;
        public bool left, right;
        public float hitDistance;

        public void Reset()
        {
            above = bellow = false;
            left = right = false;
        }
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}

