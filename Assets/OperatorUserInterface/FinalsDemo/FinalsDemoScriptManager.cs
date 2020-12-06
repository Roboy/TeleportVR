using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalsDemoScriptManager : MonoBehaviour
{
    public GameObject TrashBin;
    public GameObject TrashCube;
    public int NumberOfCubes;

    private RoomArea room;

    GameObject trashBin;

    /// <summary>
    /// Set reference to instances. Assert valid value for NumberOfCubes.
    /// </summary>
    void Start()
    {
        room = GameObject.FindObjectOfType<RoomArea>();
        if(NumberOfCubes <= 0)
        {
            NumberOfCubes = 10;
        }
    }


    /// <summary>
    /// Activates quest: update roboy posture, show trashbin, spawn trash cubes randomly within room
    /// </summary>
    public void StartQuest()
    {
        Transform roboyArms = GameObject.FindGameObjectWithTag("RoboyArms").transform;
        roboyArms.localEulerAngles = new Vector3(-60f, roboyArms.localEulerAngles.y, roboyArms.localEulerAngles.z);
        GameObject roboy = GameObject.FindGameObjectWithTag("Roboy");
        trashBin = Instantiate(TrashBin, roboy.transform);
        for(int i = 0; i < NumberOfCubes; i++)
        {
            Instantiate(TrashCube, GetRandomPositionInRoom(), Quaternion.identity);
        }
    }

    /// <summary>
    /// Stop quest and clean up quest objects: reset roboy posture, hide trashbin, destroy trash cubes
    /// </summary>
    public void StopQuest()
    {
        Transform roboyArms = GameObject.FindGameObjectWithTag("RoboyArms").transform;
        GameObject[] allTrashCubes = GameObject.FindGameObjectsWithTag("Trash");
        foreach (GameObject trash in allTrashCubes)
        {
            Destroy(trash);
        }
        Destroy(trashBin);
        roboyArms.localEulerAngles = new Vector3(0f, roboyArms.localEulerAngles.y, roboyArms.localEulerAngles.z);
    }

    /// <summary>
    /// Generate a random position within the room boundaries
    /// </summary>
    /// <returns>Vector3 result the position in the room</returns>
    Vector3 GetRandomPositionInRoom()
    {
        float y = Random.Range(0.1f, 1.7f);
        Vector3 result = new Vector3(Random.Range(-4.5f, 34.5f), y, Random.Range(-44.5f, 4.5f));
        while (!room.validatePositionInRoom(result))
        {
            result = new Vector3(Random.Range(-4.5f, 34.5f), y, Random.Range(-44.5f, 4.5f));
        }
        return result;
    }
}
