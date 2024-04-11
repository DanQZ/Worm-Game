using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogScript : MonoBehaviour
{
  public float turnSpeed = -20f;
    // Update is called once per frame
    void Update()
    {
      transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + turnSpeed * Time.deltaTime);
    }
}
