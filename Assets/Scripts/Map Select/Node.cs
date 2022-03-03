using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour
{
    public bool isPlayerOn = false;

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

    [Header("Post-it")]
    [SerializeField]
    private List<GameObject> postItsNearby = new List<GameObject>();

    public Vector3 offset = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        if (isPlayerOn)
        {
            offset = player.transform.position - transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOn)
        {
            currNode = true;
            postItsNearby.ForEach(e =>
            {
                e.SetActive(true);
            });
        }
        else
        {
            currNode = false;
            postItsNearby.ForEach(e =>
            {
                e.SetActive(false);
            });
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
        isPlayerOn = false;
        while (player.transform.position - offset != destination().transform.localPosition)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, destination().transform.localPosition + offset, 8f * Time.deltaTime);
            yield return null;
        }
        var nextNode = destination().GetComponent<Node>();
        nextNode.isPlayerOn = true;
        nextNode.offset = offset;
    }
}
