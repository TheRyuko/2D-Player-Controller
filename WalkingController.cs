using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingController : Controller
{

    //Settings
    public float speed = 6f;

    public float jumpPressTime;
    Vector2 walkVelocity;

    public override void ReadInput(InputData data)
    {
        //Reset velocity
        walkVelocity = Vector2.zero;
 
        //Check for axis input and apply speed
       if(data.axis[0] != 0f)
        {
            walkVelocity.x = data.axis[0] * speed;
        }

       //Check for button input and see how long it is held
       if(data.buttons[0])
        {
                jumpPressTime += Time.deltaTime;
        }

        newInput = true;
    }

    void LateUpdate()
    {
        //Reset velocity and button timer if no new input
        if (!newInput)
        {
            walkVelocity = Vector2.zero;
            jumpPressTime = 0f;
        }

        //If jump button was pressed this framw while on the ground, jump
        if (jumpPressTime == Time.deltaTime && collisions.bellow)
        {
            jump = true;
        }

        //Apply gravity
        walkVelocity.y = gravity;

        //Move player
        Move(walkVelocity * Time.deltaTime);
        newInput = false;
    }
}
