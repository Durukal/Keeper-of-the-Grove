using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : AI
{
    private bool collided = false, isFleeing = false;
    private int carryCapacity = 3, carried = 0;
    private float waitTime = 5f;
    private GameObject target = null;
    private Rigidbody2D myBody;
    private Animator anim;
    public AudioClip AxeHit, BladeHit;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
        float speed = 1f * Time.deltaTime;

        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < 0.4f)
            {
                if(anim.enabled)
                {
                    if (gameObject.tag == "Hunter")
                    {
                        AudioSource.PlayClipAtPoint(BladeHit, transform.position);
                    }
                    else if (gameObject.tag == "Chopper")
                    {
                        AudioSource.PlayClipAtPoint(AxeHit, transform.position);
                    }
                }
                collided = true;
                if (target.GetComponent<Animal>() != null)
                    target.GetComponent<Animal>().caught = true;
                anim.enabled = false;
            }

            if (collided)
                waitTime -= Time.deltaTime;

            if (waitTime <= 0)
            {
                if (gameObject.tag == "Hunter")
                {
                    Hunt(target);
                }
                else if (gameObject.tag == "Chopper")
                {
                    Chop(target);
                }
                target = null;
            }
        }

        if (carried == carryCapacity)
        {
            Flee();
        }


        if (!isFleeing && !collided)
        {
            Attack();
            if (target != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed);
                Movement.SetAnimations(myBody, anim, (target.transform.position.x - transform.position.x) / speed, (target.transform.position.y - transform.position.y) / speed);
            }
            else if (rotating)
            {
                transform.position = Vector2.MoveTowards(transform.position, rotateVector, speed);
                Movement.SetAnimations(myBody, anim, (rotateVector.x - transform.position.x) / speed, (rotateVector.y - transform.position.y) / speed);
                if (transform.position.x == rotateVector.x && transform.position.y == rotateVector.y)
                    rotating = false;
            }
            else
            {
                rotateVector = RandomRotator(8.5f, 4.5f);
                rotating = true;
            }
        }
        else if(isFleeing)
        {
            anim.enabled = true;
            transform.position = Vector2.MoveTowards(transform.position, rotateVector, speed);
            Movement.SetAnimations(myBody, anim, (rotateVector.x - transform.position.x) / speed, (rotateVector.y - transform.position.y) / speed);
        }
    }

    void Attack()
    {
        if (gameObject.tag == "Hunter")
        {
            target = FindClosestObject("Animal");
        }
        else if (gameObject.tag == "Chopper")
        {
            target = FindClosestObject("Tree");
        }
    }

    void Hunt(GameObject obj)
    {
        obj.GetComponent<Animal>().DecreaseHP();
        carried += 3;
        if (!obj.GetComponent<Animal>().isDead)
            GameManager.meatBalance--;
    }

    void Chop(GameObject obj)
    {
        obj.GetComponent<GameTree>().DecreaseHP();
        carried += 3;
        if (!obj.GetComponent<GameTree>().isDead)
            GameManager.woodBalance--;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject == target)
        //{
        //    collided = true;
        //    if (target.GetComponent<Animal>() != null)
        //        target.GetComponent<Animal>().caught = true;
        //    anim.enabled = false;
        //}

        if (collision.gameObject.tag == "Finish")   //GAME BORDERS
        {
            gameObject.SetActive(false);
        }
    }

    public void Flee()
    {
        target = null;
        isFleeing = true;

        if (transform.position.x > 0)
        {
            if (transform.position.y > 0)
            {
                if (5 - transform.position.y < 9 - transform.position.x)
                    rotateVector = new Vector2(transform.position.x, 5);
                else
                    rotateVector = new Vector2(9, transform.position.y);
            }
            else
            {
                if (5 + transform.position.y < 9 - transform.position.x)
                    rotateVector = new Vector2(transform.position.x, -5);
                else
                    rotateVector = new Vector2(9, transform.position.y);
            }
        }
        else
        {
            if (transform.position.y > 0)
            {
                if (5 - transform.position.y < 9 + transform.position.x)
                    rotateVector = new Vector2(transform.position.x, 5);
                else
                    rotateVector = new Vector2(-9, transform.position.y);
            }
            else
            {
                if (5 + transform.position.y < 9 + transform.position.x)
                    rotateVector = new Vector2(transform.position.x, -5);
                else
                    rotateVector = new Vector2(-9, transform.position.y);
            }
        }
    }

}