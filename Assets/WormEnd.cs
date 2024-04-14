using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEnd : MonoBehaviour
{ 
  [SerializeField] WormController myWormController;
  private bool tryToStick = false;
  private bool alreadyStuck = false;
  [SerializeField] Rigidbody2D myRB;
  private Vector3 stuckPosition;
  void Update(){
    if(alreadyStuck){
      myRB.AddForce((stuckPosition - transform.position) * 100f * Time.deltaTime);
    }
  }
  void OnTriggerStay2D(Collider2D col)
  {
    if(alreadyStuck){ return;}

    if (col.gameObject.tag == "sticky")
    {
      if(tryToStick){
        myRB.constraints = RigidbodyConstraints2D.FreezePosition;
        stuckPosition = transform.position;
        alreadyStuck = true;
      }
    }
  }
  public void TryToStick(){
    tryToStick = true;
  }
  public void Unstick(){
    tryToStick = false;
    alreadyStuck = false;
    myRB.constraints = RigidbodyConstraints2D.None;
  }
}
