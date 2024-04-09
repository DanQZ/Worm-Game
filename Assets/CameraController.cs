using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField] GameObject wormHead;
  [SerializeField] GameObject wormTail;
  void Update()
  {
    transform.position = new Vector3((wormHead.transform.position.x + wormTail.transform.position.x) / 2, (wormHead.transform.position.y + wormTail.transform.position.y) / 2, -10);
  }
}
