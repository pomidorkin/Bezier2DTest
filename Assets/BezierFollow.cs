using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    [SerializeField] Transform[] routes;

    private int routeToGo;

    private float tParam;

    private Vector3 objectPosition;
    private Vector3 lookPosition; //test

    private float speedModifier;

    private bool coroutineAllowed;

    private bool loopEnabled;

    private void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.5f;
        coroutineAllowed = true;
        loopEnabled = true;
    }

    private void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        coroutineAllowed = false;

            Vector3 p0 = routes[routeNumber].GetChild(0).position;
            Vector3 p1 = routes[routeNumber].GetChild(1).position;
            Vector3 p2 = routes[routeNumber].GetChild(2).position;
            Vector3 p3 = routes[routeNumber].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            transform.position = objectPosition;
            // Here we should rotate the object
            //Test
            // Здесь мы заглядываем на шаг вперёд прибавляя к tParam 0.05f и заставляем объект
            // смотреть на эту точку
            
            float nextPoint  = (tParam + 0.05f);

            if (nextPoint < 1)
            {
                lookPosition = Mathf.Pow(1 - nextPoint, 3) * p0 +
                3 * Mathf.Pow(1 - nextPoint, 2) * (nextPoint) * p1 +
                3 * (1 - nextPoint) * Mathf.Pow(nextPoint, 2) * p2 +
                Mathf.Pow(nextPoint, 3) * p3;

                transform.LookAt(lookPosition);
            }
            
            //EndTest

            yield return new WaitForEndOfFrame();

        }

        if (loopEnabled)
        {

            tParam = 0f;
            routeToGo += 1;

            if (routeToGo > routes.Length - 1)
            {
                routeToGo = 0;
            }

            coroutineAllowed = true;
        }

    }
}
