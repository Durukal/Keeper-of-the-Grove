using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTree : ForestCreature
{
    public Sprite originalSprite, deadSprite, choppedSprite;
    public bool isChopped;
    public AudioClip TreeDie;

    void Start()
    {
        age = Random.Range(0, 15);
        lifeSpan = 40;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (age >= lifeSpan)
            {
                AudioSource.PlayClipAtPoint(TreeDie, transform.position);
                isDead = true;
                GetComponent<SpriteRenderer>().sprite = deadSprite;
            }
            else
            {
                age += Time.deltaTime;

                if (!isChopped)
                    GetComponent<SpriteRenderer>().sprite = originalSprite;
            }
                

        }

        if (hitPoint <= 0)
        {
            if(!isChopped)
                AudioSource.PlayClipAtPoint(TreeDie, transform.position);
            isChopped = true;
            GetComponent<SpriteRenderer>().sprite = choppedSprite;
        }
    }

}
