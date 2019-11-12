using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : ForestCreature
{
    
    public float rotTime;
    public Sprite deadSprite;
    PoolManager pool;
    Coroutine rotter;
    public bool caught = false;
    private Rigidbody2D myBody;
    private Animator anim;
    public AudioClip AnimalDie;
    public Color deadColor;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    private void OnEnable()
    {
        rotTime = 10;
        age = Random.Range(0f, 5f);
        isDead = false;
        GetComponent<Animator>().enabled = true;
        rotter = null;
    }
    void Start()
    {
        age = Random.Range(0, 5);
        lifeSpan = 10;
    }

    void Update()
    {

        if (hitPoint <= 0)
        {
            AudioSource.PlayClipAtPoint(AnimalDie, transform.position);
            gameObject.SetActive(false);
            GameManager.meatBalance--;
        }

        if (!isDead)
        {
            if (age > lifeSpan)
            {
                AudioSource.PlayClipAtPoint(AnimalDie, transform.position);
                isDead = true;
                GetComponent<Animator>().enabled = false;
                GetComponent<SpriteRenderer>().sprite = deadSprite;
                //GetComponent<SpriteRenderer>().color = deadColor;
            }
            else
                age += Time.deltaTime;

            if (!caught)
            {
                if (rotating)
                {
                    float speed = 1f * Time.deltaTime;
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
        }
        if (isDead && rotter == null)
        {
            rotter = StartCoroutine(rottenTime());
        }

        if (!isDead && rotating && !caught)
            anim.enabled = true;
        else
            anim.enabled = false;
    }

    IEnumerator rottenTime()
    {
        while (rotTime > 0)
        {
            rotTime -= Time.deltaTime;


            yield return null;
        }
        dieAnimal();
    }
    void decreaseRotTime()
    {
        if (rotTime > 0)
        {
            rotTime -= 1;

        }
    }

    public void dieAnimal()
    {
        if (isDead && rotTime <= 0)
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.spawnAnimal());
            PoolManager.Instance.Despawn(gameObject);
        }
           
    }

}
