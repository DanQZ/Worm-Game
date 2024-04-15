using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  [SerializeField] Transform wormHead;
  [SerializeField] Transform wormTail;
  void Update()
  {
    transform.position = new Vector3((wormHead.position.x + wormTail.position.x) / 2, (wormHead.position.y + wormTail.position.y) / 2, -10);
  }
  public void SetTarget(Transform target1, Transform target2){
    wormHead = target1;
    wormTail = target2;
  }
}
