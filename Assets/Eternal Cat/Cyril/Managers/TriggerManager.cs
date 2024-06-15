using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static TriggerManager Instance;

    public bool playerInRange = false;

    private int playerInRangeCount = 0;

    public PathFinding pnjToStop;

    void Awake()
    {
        Instance = this;
    }

    public void AddPlayerInRange()
    {
        playerInRangeCount++;
        if (playerInRangeCount >= 1)
        {
            playerInRange = true;
        }
    }

    public void RemovePlayerInRange()
    {
        playerInRangeCount--;
        if (playerInRangeCount == 0)
        {
            playerInRange = false;
        }
    }
}
