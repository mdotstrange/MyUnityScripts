using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class GenerateNavLinks : MonoBehaviour
{
    public float linkWidth;
    public bool bidirectionalLinks;
    public NavLinkTag[] navtags;
    Vector3 closestPointFromAToB;
    Vector3 closestPointFromBToA;
    public float linkCompenstationAmount;
    public List<BoxCollider> floors = new List<BoxCollider>();
    public List<BoxCollider> walls = new List<BoxCollider>();
    public List<BoxCollider> ceilings = new List<BoxCollider>();
    public bool debugLines;
    public float wallConnectThreshold;
    BoxCollider[] allBoxes;

    [Button]
    public void DoGenerateLinks()
    {
        GetNavLinkTagTypes();
        SeparateLinkTagTypes();
        ConnectThemAll();
    }

    public void ConnectThemAll()
    {
        IfDistanceOkThenConnect(floors, walls);
        IfDistanceOkThenConnect(ceilings, walls);
        IfDistanceOkThenConnect(walls, walls);
        IfDistanceOkThenConnect(floors, floors);
        IfDistanceOkThenConnect(ceilings, ceilings);
    }
    public void GetNavLinkTagTypes()
    {
        navtags = GetComponentsInChildren<NavLinkTag>();
        allBoxes = GetComponentsInChildren<BoxCollider>();
    }

    public void SeparateLinkTagTypes()
    {
        for (int index = 0; index < allBoxes.Length; index++)
        {
            var box = allBoxes[index];

            var i = box.gameObject.GetComponent<NavLinkTag>();

            if(i != null)
            {
                var ii = i.RoomPieceType;

                if (ii == NavLinkTag.typo.Floor)
                {
                    //var iii = i.gameObject.GetComponents<BoxCollider>();
                    floors.Add(box);
                }

                if (ii == NavLinkTag.typo.Wall)
                {
                    //var iii = i.gameObject.GetComponents<BoxCollider>();
                    walls.Add(box);
                }

                if (ii == NavLinkTag.typo.Ceiling)
                {
                    //var iii = i.gameObject.GetComponents<BoxCollider>();
                    ceilings.Add(box);
                }
            }

           
        }
    }

    public void ConnectAListToBList(List<BoxCollider> aList, List<BoxCollider> bList)
    {
        for (int index = 0; index < aList.Count; index++)
        {
            var i = aList[index];

            for (int index1 = 0; index1 < bList.Count; index1++)
            {
                var ii = bList[index1];
                ConnectTheLinks(i, ii);
            }
        }

    }

    public void IfDistanceOkThenConnect(List<BoxCollider> aList, List<BoxCollider> bList)
    {
        for (int index = 0; index < aList.Count; index++)
        {
            var i = aList[index];

            for (int index1 = 0; index1 < bList.Count; index1++)
            {
                var ii = bList[index1];

                if (IsObjectCloseEnough(i, ii))
                {
                    ConnectTheLinks(i, ii);
                }

            }
        }

    }

    public bool IsObjectCloseEnough(Collider a, Collider b)
    {

        if (string.Compare(a.gameObject.name, b.gameObject.name) == 0)
        {
            return false;
        }

        var boxCenter = a.GetComponent<BoxCollider>().center;
        var aCenter = a.transform.TransformPoint(boxCenter);

        var closestFromAToB = a.ClosestPoint(b.ClosestPoint(aCenter));
        var closestFromBToA = b.ClosestPoint(closestFromAToB);
        var distance = Vector3.Distance(closestFromAToB, closestFromBToA);

        if (distance <= wallConnectThreshold)
        {
            return true;
        } else
        {
            return false;
        }

    }

    public void ConnectTheLinks(Collider a, Collider b)
    {
        GetClosestPointsToEachOther(a, b);
        var link = CreateLinkOnCollider(a);
        SetNavMeshLinkData(link, a);
        AdjustLinks(link, a, b);
    }

    public void GetClosestPointsToEachOther(Collider a, Collider b)
    {
        var aCenter = GetBoxCenterPosition(a, a.transform);
        closestPointFromAToB = a.ClosestPoint(b.ClosestPoint(aCenter));
        closestPointFromBToA = b.ClosestPoint(closestPointFromAToB);
    }

    public NavMeshLink CreateLinkOnCollider(Collider coll)
    {
        return coll.gameObject.AddComponent<NavMeshLink>();
    }

    public void SetNavMeshLinkData(NavMeshLink link, Collider a)
    {
        link.startPoint = a.transform.InverseTransformPoint(closestPointFromAToB);
        link.endPoint = a.transform.InverseTransformPoint(closestPointFromBToA);
        link.bidirectional = bidirectionalLinks;
        link.width = linkWidth;
    }

    public void AdjustLinks(NavMeshLink link, Collider a, Collider b)
    {
        var aCenter = GetBoxCenterPosition(a, a.transform);

        var directionFromACenterToLinkStart = -(closestPointFromAToB - aCenter).normalized;
        if (debugLines == true)
        {
            Debug.DrawRay(closestPointFromAToB, directionFromACenterToLinkStart, Color.green, 99);
        }

        Ray aRay = new Ray(closestPointFromAToB, directionFromACenterToLinkStart);
        var aPos = aRay.GetPoint(linkCompenstationAmount);

        var bCenter = GetBoxCenterPosition(b, b.transform);

        var directionFromBTransformToLinkEnd = -(closestPointFromBToA - bCenter).normalized;
        if (debugLines == true)
        {
            Debug.DrawRay(closestPointFromBToA, directionFromBTransformToLinkEnd, Color.red, 99);
        }

        Ray bRay = new Ray(closestPointFromBToA, directionFromBTransformToLinkEnd);
        var bPos = bRay.GetPoint(linkCompenstationAmount);


        link.startPoint = a.transform.InverseTransformPoint(aPos);
        link.endPoint = a.transform.InverseTransformPoint(bPos);
    }

    public Vector3 GetBoxCenterPosition(Collider coll, Transform trans)
    {
        var box = coll.GetComponent<BoxCollider>().center;
        return trans.transform.TransformPoint(box);
    }

}
