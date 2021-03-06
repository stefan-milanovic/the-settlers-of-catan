﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    
    private enum ChildId
    {
        SETTLEMENT,
        CITY,
        RIPPLE_SYSTEM,
        PLAYER_FLAG_SHORT = 4,
        PLAYER_FLAG_LONG
    }
    
    private bool available = true;

    private bool rippleActive = false;
    private bool hasSettlement = false;
    private bool hasCity = false;

    // private GamePlayer owner = null;

    private GameObject rippleSystem;
    private GameObject settlement;
    private GameObject city;

    private GameObject shortFlagObject;
    private GameObject longFlagObject;

    private GameObject shortFlag;
    private GameObject longFlag;

    private GameObject settlementRoof;

    private GameObject cityRoof1;
    private GameObject cityRoof2;

    private PhotonView photonView;

    [SerializeField]
    private WorldPath[] surroundingPaths;

    private List<Intersection> neighbouringIntersections;

    private int surroundingPathsLength;

    private int ownerId = 0; // at the start, no one owns this intersection

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        surroundingPathsLength = surroundingPaths.Length;

        settlement = transform.GetChild((int)ChildId.SETTLEMENT).gameObject;
        city = transform.GetChild((int)ChildId.CITY).gameObject;
        rippleSystem = transform.GetChild((int) ChildId.RIPPLE_SYSTEM).gameObject;

        shortFlagObject = transform.GetChild((int)ChildId.PLAYER_FLAG_SHORT).gameObject;
        longFlagObject = transform.GetChild((int)ChildId.PLAYER_FLAG_LONG).gameObject;

        shortFlag = shortFlagObject.transform.GetChild(0).gameObject;
        longFlag = longFlagObject.transform.GetChild(0).gameObject;

        settlementRoof = transform.GetChild((int)ChildId.SETTLEMENT).gameObject.transform.GetChild(8).gameObject;

        cityRoof1 = transform.GetChild((int)ChildId.CITY).gameObject.transform.GetChild(33).gameObject;
        cityRoof2 = transform.GetChild((int)ChildId.CITY).gameObject.transform.GetChild(34).gameObject;

        // Set up the neighbouring intersections list.
        neighbouringIntersections = new List<Intersection>();

        foreach (WorldPath path in surroundingPaths)
        {
            Intersection[] pathIntersections = path.GetIntersections();
            foreach (Intersection i in pathIntersections)
            {
                if (i != this)
                {
                    neighbouringIntersections.Add(i);
                }
            }
        }
    }

    public WorldPath[] GetSurroundingPaths() { return surroundingPaths; }

    public List<WorldPath> GetSurroundingPathsExcept(WorldPath pathToIgnore)
    {
        List<WorldPath> result = new List<WorldPath>();
        foreach (WorldPath path in surroundingPaths)
        {
            if (path != pathToIgnore)
            {
                result.Add(path);
            }
        }
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsAvailable()
    {
        return available;
    }

    public void SetAvailable(bool avail)
    {
        this.available = avail;
    }

    public bool HasSettlement()
    {
        return hasSettlement;
    }

    public bool HasCity()
    {
        return hasCity;
    }

    public int GetOwnerId()
    {
        return ownerId;
    }

    public List<Intersection> GetNeighbouringIntersections()
    {
        return neighbouringIntersections;
    }

    public void ToggleRipple()
    {
        if (!rippleActive)
        {
            rippleSystem.SetActive(rippleActive = true);
        }
        else
        {
            rippleSystem.SetActive(rippleActive = false);
        }
        
    }

    public void ConstructSettlement(int ownerId)
    {
       
        photonView.RPC("RPCConstructSettlement", RpcTarget.All, ownerId);
        
    }

    public void ConstructCity()
    {
        photonView.RPC("RPCConstructCity", RpcTarget.All);
    }

    [PunRPC]
    private void RPCConstructSettlement(int ownerId)
    {

        if (rippleActive)
        {
            rippleSystem.SetActive(rippleActive = false);
        }

        settlement.SetActive(true);
        
        this.ownerId = ownerId;

        available = false; // chain this to neighbouring intersections

        foreach (Intersection neighbour in neighbouringIntersections)
        {
            neighbour.SetAvailable(false);
        }

        hasSettlement = true;

        ShowShortFlag();
    }

    [PunRPC]
    private void RPCConstructCity()
    {

        settlement.SetActive(false);
        city.SetActive(true);

        hasSettlement = false;
        hasCity = true;

        HideShortFlag();
        ShowLongFlag();
    }

    public List<WorldPath> GetAvailablePaths()
    {
        List<WorldPath> list = new List<WorldPath>();
        foreach (WorldPath path in surroundingPaths)
        {
            if (path.IsAvailable())
            {
                list.Add(path);
            }
        }

        return list;
    }
    
    private void ShowShortFlag()
    {

        int position = GamePlayer.FindPosition(ownerId) + 1;

        string materialPath = "Materials/PlayerMaterials/Player" + position + "Material";

        ColorUtility.TryParseHtmlString(PhotonNetwork.CurrentRoom.GetPlayer(ownerId).CustomProperties["colour"] as string, out Color playerColour);

        shortFlag.GetComponent<SkinnedMeshRenderer>().material = Resources.Load(materialPath) as Material;
        shortFlag.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", playerColour);
        

        shortFlagObject.SetActive(true);

        // Set roof colour.
        string roofPath = "Materials/PlayerMaterials/RoofP" + position;

        settlementRoof.GetComponent<MeshRenderer>().material = Resources.Load(roofPath) as Material;
        settlementRoof.GetComponent<MeshRenderer>().material.SetColor("_Color", playerColour);

    }
    private void HideShortFlag()
    {
        shortFlagObject.SetActive(false);
    }

    private void ShowLongFlag()
    {

        int position = GamePlayer.FindPosition(ownerId) + 1;

        string materialPath = "Materials/PlayerMaterials/Player" + position + "Material";
        ColorUtility.TryParseHtmlString(PhotonNetwork.CurrentRoom.GetPlayer(ownerId).CustomProperties["colour"] as string, out Color playerColour);

        longFlag.GetComponent<SkinnedMeshRenderer>().material = Resources.Load(materialPath) as Material;
        longFlag.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", playerColour);

        longFlagObject.SetActive(true);

        // Set roof colour.
        string roofPath = "Materials/PlayerMaterials/RoofCityP" + position;

        cityRoof1.GetComponent<MeshRenderer>().material = Resources.Load(roofPath) as Material;
        cityRoof1.GetComponent<MeshRenderer>().material.SetColor("_Color", playerColour);

        cityRoof2.GetComponent<MeshRenderer>().material = Resources.Load(roofPath) as Material;
        cityRoof2.GetComponent<MeshRenderer>().material.SetColor("_Color", playerColour);
    }

    public bool OnHarbour(out HarbourPath.HarbourBonus? bonus)
    {
        bonus = null;

        foreach (WorldPath path in surroundingPaths)
        {
            if (path is HarbourPath)
            {
                bonus = ((HarbourPath)path).GetHarbourBonus();
                return true;
            }
        }

        return false;
    }
}
