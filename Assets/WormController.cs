using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour
{
  [SerializeField] Rigidbody2D headRB;
  [SerializeField] Rigidbody2D tailRB;
  [SerializeField] SpriteRenderer headSpriteRenderer;
  [SerializeField] SpriteRenderer tailSpriteRenderer;
  [SerializeField] GameObject bodyParentObject;
  private List<HingeJoint2D> hingeJointList = new List<HingeJoint2D>();
  void Start()
  {
    InitHinges();
    InitLineRenderers();
  }

  private void InitHinges(){
    hingeJointList.Add(tailRB.GetComponent<HingeJoint2D>());
    foreach (HingeJoint2D item in bodyParentObject.GetComponentsInChildren<HingeJoint2D>())
    {
      hingeJointList.Add(item);
    }
  }
  private List<LineRenderer> lineRendererList = new List<LineRenderer>();
  private void InitLineRenderers(){
    LineRenderer headLineRenderer = headRB.gameObject.GetComponent<LineRenderer>();
    headLineRenderer.positionCount = 10;
    headLineRenderer.startWidth = 0.1f;
    headLineRenderer.endWidth = 0.1f;
    headLineRenderer.startColor = Color.white;
    headLineRenderer.endColor = Color.white;
  }

  private float playerMotorSpeed = 25f;
  [SerializeField] PhysicsMaterial2D wormMaterialFriction;
  [SerializeField] PhysicsMaterial2D defaultMaterial;
  void FixedUpdate()
  { 
    WormTwisting();
  }

  void Update(){
    UpdateWormProperties();
  }

  private void UpdateWormProperties(){

    if(Input.GetKeyDown("k"))
    {
      headRB.sharedMaterial = wormMaterialFriction;
      headSpriteRenderer.color = Color.red;
    }
    if(Input.GetKeyDown("l"))
    {
      tailRB.sharedMaterial = wormMaterialFriction;
      tailSpriteRenderer.color = Color.red;
    }
    
    if(Input.GetKeyUp("k"))
    {
      headRB.sharedMaterial = defaultMaterial;
      headSpriteRenderer.color = Color.white;
    
    }
    if(Input.GetKeyUp("l"))
    {
      tailRB.sharedMaterial = defaultMaterial;
      tailSpriteRenderer.color = Color.white;
    }
  }
  
  private void WormTwisting(){

    foreach (HingeJoint2D joint in hingeJointList)
    {
      joint.useMotor = false;
    }

    if(Input.GetKey("a"))
    {
      foreach (HingeJoint2D joint in hingeJointList)
      {
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = 0f + playerMotorSpeed;
        joint.motor = motor;
      }
    }
    if( Input.GetKey("d"))
    {
      foreach (HingeJoint2D joint in hingeJointList)
      {
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = 0f - playerMotorSpeed;
        joint.motor = motor;
      }
    }
  }
}
