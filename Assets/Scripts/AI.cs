using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    protected Vector2 rotateVector;
    protected bool rotating = false;

    protected GameObject FindClosestObject(string tag)
    {
        GameObject[] results = GameObject.FindGameObjectsWithTag(tag);
        GameObject deadClosest = null, secondClosest = null;
        float deadClosestDist = Mathf.Infinity, secondClosestDist = Mathf.Infinity;

        foreach (var obj in results)
        {
            if (Vector2.Distance(obj.transform.position, transform.position) < 10f &&
                    obj.GetComponent<ForestCreature>().isDead &&
                    Vector2.Distance(transform.position, obj.transform.position) < deadClosestDist)
            {
                if (obj.GetComponent<GameTree>() != null && obj.GetComponent<GameTree>().isChopped)
                    continue;

                deadClosest = obj;
                deadClosestDist = Vector2.Distance(transform.position, obj.transform.position);
                continue;
            }

            if (Vector2.Distance(obj.transform.position, transform.position) < 10f &&
                    Vector2.Distance(transform.position, obj.transform.position) < secondClosestDist)
            {
                if (obj.GetComponent<GameTree>() != null && obj.GetComponent<GameTree>().isChopped)
                    continue;

                secondClosest = obj;
                secondClosestDist = Vector2.Distance(transform.position, obj.transform.position);
            }
        }

        return deadClosest ?? secondClosest;
    }

    protected Vector2 RandomRotator(float x, float y)
    {
        return new Vector2(Random.Range(-x, x), Random.Range(-y, y));
    }
}
