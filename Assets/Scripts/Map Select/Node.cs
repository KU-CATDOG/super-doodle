using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour
{

    [Header("Node Destinations")]
    public GameObject upDestination;
    public GameObject downDestination;
    public GameObject leftDestination;
    public GameObject rightDestination;

    [SerializeField]
    private GameObject player;
    public bool locked = false;
    private bool currNode;

    [SerializeField]
    private int mapIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition == player.transform.position)
        {
            currNode = true;
        }
        else
        {
            currNode = false;
        }

        if (currNode)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(mapIndex < SceneManager.sceneCountInBuildSettings)
                {
                    //SelectedLevel.index = mapIndex;
                    SceneManager.LoadScene(mapIndex);
                    //SceneManager.LoadScene(1); // samplescene
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Debug.Log("Return to Menu");
                SceneManager.LoadScene("MenuScene");
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(upDestination != null && !upDestination.GetComponent<Node>().locked)
                {
                    currNode = false;
                    StartCoroutine(MoveToward(() => upDestination));
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if(downDestination != null && !downDestination.GetComponent<Node>().locked)
                {
                    currNode = false;
                    StartCoroutine(MoveToward(() => downDestination));
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (leftDestination != null && !leftDestination.GetComponent<Node>().locked)
                {
                    currNode = false;
                    StartCoroutine(MoveToward(() => leftDestination));
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (rightDestination != null && !rightDestination.GetComponent<Node>().locked)
                {
                    currNode = false;
                    StartCoroutine(MoveToward(() => rightDestination));
                }
            }
        }


    }

    delegate GameObject Destination();

    IEnumerator MoveToward(Destination destination)
    {
        while (player.transform.position != destination().transform.localPosition)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, destination().transform.localPosition, 8f * Time.deltaTime);
            yield return null;
        }
    }
}
