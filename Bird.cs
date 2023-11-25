using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour

{
    [SerializeField] float maxDragDistance = 50;
    [SerializeField] float launchPower = 150;
    Vector3 startingPosition;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.enabled = false;
        Vector3 startingPosition = transform.position;
    }

    void OnMouseUp()
    {
        Vector3 directionAndMagnitude = startingPosition - transform.position;
        GetComponent<Rigidbody2D>().AddForce(directionAndMagnitude * launchPower);
        GetComponent<Rigidbody2D>().gravityScale = 1;
        lineRenderer.enabled = false;

    }
    void OnMouseDrag()
    {
        Vector3 destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        destination.z = 0;
        if (Vector2.Distance(destination,startingPosition) > maxDragDistance)
        {
            destination = Vector3.MoveTowards(startingPosition, destination, maxDragDistance);
        }
        transform.position = destination;
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.enabled = true;

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }
        if(FindAnyObjectByType<Enemy>(FindObjectsInactive.Exclude) == null)
        {
            Debug.Log("Game Over");
            int levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(levelToLoad);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Invoke(nameof(ReloadLevel), 5);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
