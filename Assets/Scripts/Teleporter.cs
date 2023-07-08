using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2Int destination;

    public float timeRemaining = 0;

    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        yield return new WaitForSeconds(.3f);
        Transform movePoint = GameObject.FindWithTag("Move Point").GetComponent<Transform>();
        movePoint.position = new Vector3(destination.x,destination.y, 0);
        other.GetComponent<Transform>().position = movePoint.position;
    }
}
