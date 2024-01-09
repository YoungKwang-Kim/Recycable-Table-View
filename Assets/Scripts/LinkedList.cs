using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedList : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LinkedList<string> list = new LinkedList<string>();

        list.AddFirst("È«±æµ¿");
        list.AddLast("±è±æµ¿");

        LinkedListNode<string> node = list.First;

        Debug.Log(node.Next);
    }
}
