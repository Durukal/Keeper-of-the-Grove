using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static float speed = 2f;

    public static void Move(Rigidbody2D body, float horizontalSpeed, float verticalSpeed)
    {
        body.velocity = new Vector2(horizontalSpeed * speed, verticalSpeed * speed);
    }

    public static void SetAnimations(Rigidbody2D myBody, Animator anim, float speedx = 0f, float speedy = 0f)
    {
        if (speedx == 0f)
        {
            speedx = myBody.velocity.x;
            speedy = myBody.velocity.y;
        }

        if (Mathf.Abs(speedx) < Mathf.Abs(speedy))
        {
            if (speedy > 0)
            {
                anim.SetBool("Move Right", false);
                anim.SetBool("Move Left", false);
                anim.SetBool("Move Front", false);
                anim.SetBool("Move Back", true);
            }
            else
            {
                anim.SetBool("Move Right", false);
                anim.SetBool("Move Left", false);
                anim.SetBool("Move Front", true);
                anim.SetBool("Move Back", false);
            }

        }
        else if (Mathf.Abs(speedx) > Mathf.Abs(speedy))
        {
            if (speedx > 0)
            {
                anim.SetBool("Move Right", true);
                anim.SetBool("Move Left", false);
                anim.SetBool("Move Front", false);
                anim.SetBool("Move Back", false);
            }
            else if (speedx < 0)
            {
                anim.SetBool("Move Right", false);
                anim.SetBool("Move Left", true);
                anim.SetBool("Move Front", false);
                anim.SetBool("Move Back", false);
            }
        }
        else if ((speedx == 0) && (speedy == 0))
        {
            anim.SetBool("Move Right", false);
            anim.SetBool("Move Left", false);
            anim.SetBool("Move Front", false);
            anim.SetBool("Move Back", false);
        }
    }
}