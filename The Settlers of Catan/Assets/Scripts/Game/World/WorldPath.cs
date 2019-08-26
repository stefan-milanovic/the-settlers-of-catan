﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPath : MonoBehaviour
{

    private const int ROAD_CHILD_ID = 0;
    private const int BLINK_CHILD_ID = 1;

    private bool available = true;
    private bool blinkActive = false;

    private int ownerId;

    private GameObject road;
    private GameObject emissionObject;

    private PhotonView photonView;

    // ADD SOON
    private Intersection[] intersections;

    // WHEN LOOKING FOR PATHS WHERE PLAYER CAN BUILD A ROAD
    // ASK:
    // 1) player's settlements - all adjacent available roads
    // 2) player's roads -- all available roads connected to this road (we can find that by going through the intersections and then checking if their surroundingRoads are available)

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    protected void Init()
    {
        photonView = GetComponent<PhotonView>();
        road = gameObject.transform.GetChild(ROAD_CHILD_ID).gameObject;
        emissionObject = gameObject.transform.GetChild(BLINK_CHILD_ID).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsAvailable() { return available;  }

    public void ToggleBlink()
    {
        if (!blinkActive)
        {
            emissionObject.SetActive(blinkActive = true);
        }
        else
        {
            emissionObject.SetActive(blinkActive = false);
        }
    }

    public void ConstructRoad(int ownerId)
    {
        photonView.RPC("RPCConstructRoad", RpcTarget.All, ownerId);
    }

    [PunRPC]
    public void RPCConstructRoad(int ownerId)
    {
        if (blinkActive)
        {
            emissionObject.SetActive(blinkActive = false);
        }

        road.SetActive(true);
        this.ownerId = ownerId;

        available = false;
    }
}
