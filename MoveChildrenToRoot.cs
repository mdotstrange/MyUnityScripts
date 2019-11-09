using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChildrenToRoot : MonoBehaviour
{
    [Header("Script moves children to scene root at runtime")]
    [Space]
    public bool DestroyThisAfterMoving;

    List<Transform> children = new List<Transform>();

    private void Awake()
    {
        children.Clear();

        for (int index = 0; index < transform.childCount; index++)
        {
            var child = transform.GetChild(index);
            children.Add(child);
        }

       
        for (int index = 0; index < children.Count; index++)
        {
            var i = children[index];
            i.parent = null;
        }

        if(DestroyThisAfterMoving)
        {
            Destroy(gameObject);
        }
    }

 

}
