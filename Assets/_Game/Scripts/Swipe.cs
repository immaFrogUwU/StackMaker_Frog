using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{

    private Vector2 startTouchPos; //vị trí bắt đầu chạm
    private Vector2 endTouchPos; //vị trí sau khi vuốt
    public Touch touch;
    private IEnumerator goCoroutine;
    private bool coroutineAllowed;

    private void Start()
    {
        coroutineAllowed = true;
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
        }
        if (touch.phase == TouchPhase.Began)
        {
            startTouchPos = touch.position;

        }
        if (Input.touchCount > 0 && touch.phase == TouchPhase.Ended && coroutineAllowed)
        {
            endTouchPos = touch.position;
            if (endTouchPos.y > startTouchPos.y && Mathf.Abs(touch.deltaPosition.y) > Mathf.Abs(touch.deltaPosition.x))
            {
                goCoroutine = Go(new Vector3(0, 0, 1f));
                StartCoroutine(goCoroutine);
            }
            else if( endTouchPos.y < startTouchPos.y && Mathf.Abs(touch.deltaPosition.y) > Mathf.Abs(touch.deltaPosition.x))
            {
                goCoroutine = Go(new Vector3(0, 0, -1f));
                StartCoroutine(goCoroutine);
            }
            else if (endTouchPos.x < startTouchPos.x && Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
            {
                goCoroutine = Go(new Vector3(-1f, 0, 0));
                StartCoroutine(goCoroutine);
            }
            else if (endTouchPos.x > startTouchPos.x && Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
            {
                goCoroutine = Go(new Vector3(1f, 0, 0));
                StartCoroutine(goCoroutine);
            }

        }
    }

    private IEnumerator Go(Vector3 direction)
    {
        coroutineAllowed = false; //prevent starting another coroutine until this one is finished
        float distance = 1f; //the distance to move in each direction
        float speed = 4f; //the speed of movement
        Vector3 startPos = transform.position; //the initial position of the object
        Vector3 endPos = startPos + direction * distance; //the target position of the object
        float t = 0; //a parameter to interpolate between start and end positions
        while (t < 1) //while the object has not reached the end position
        {
            t += Time.deltaTime * speed / distance; //increase t by a fraction of the distance traveled per frame
            transform.position = Vector3.Lerp(startPos, endPos, t); //move the object along the direction vector
            yield return null; //wait for the next frame
        }
        StopCoroutine(goCoroutine); //stop the coroutine
        coroutineAllowed = true; //allow starting another coroutine
    }

    }

