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
  private List<HingeJoint2D> hingeJoints1stHalf = new List<HingeJoint2D>();
  private List<HingeJoint2D> hingeJoints2ndHalf = new List<HingeJoint2D>();
  private List<HingeJoint2D> allHingeJoints = new List<HingeJoint2D>();

  
  [SerializeField] GameObject kUp;
  [SerializeField] GameObject kDown;
  [SerializeField] GameObject lUp;
  [SerializeField] GameObject lDown;
  
  void Start()
  {
    InitHinges();
    InitLineRenderers();
  }

  private void InitHinges(){
    int index = 0;
    foreach (HingeJoint2D hingeJoint in bodyParentObject.GetComponentsInChildren<HingeJoint2D>())
    {
      allHingeJoints.Add(hingeJoint);
      if(index < 4){
        hingeJoints1stHalf.Add(hingeJoint);
      }
      else{
        hingeJoints2ndHalf.Add(hingeJoint);
      }
      index++;
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

  private float playerMotorSpeed = 50f;
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

    foreach (HingeJoint2D hingeJoint in allHingeJoints)
    {
      if(hingeJoint.useMotor){
        hingeJoint.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
      }
      else{
        hingeJoint.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
      }
    }
  }
  
  private void WormTwisting(){

    kUp.SetActive(false);
    kDown.SetActive(false);
    lUp.SetActive(false);
    lDown.SetActive(false);

    foreach (HingeJoint2D joint in hingeJoints1stHalf)
    {
      joint.useMotor = false;
    }

    foreach (HingeJoint2D joint in hingeJoints2ndHalf)
    {
      joint.useMotor = false;
    }

    if(Input.GetKey("a"))
    {
      foreach (HingeJoint2D joint in hingeJoints1stHalf)
      {
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = 0f + playerMotorSpeed;
        joint.motor = motor;
        kDown.SetActive(true);
      }
    }
    if( Input.GetKey("s"))
    {
      foreach (HingeJoint2D joint in hingeJoints1stHalf)
      {
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = 0f - playerMotorSpeed;
        joint.motor = motor;
        kUp.SetActive(true);
      }
    }

    if( Input.GetKey("d"))
    {
      foreach (HingeJoint2D joint in hingeJoints2ndHalf)
      {
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = 0f- playerMotorSpeed;
        joint.motor = motor;
        lDown.SetActive(true);
      }
    }
    if( Input.GetKey("f"))
    {
      foreach (HingeJoint2D joint in hingeJoints2ndHalf)
      {
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = playerMotorSpeed;
        joint.motor = motor;
        lUp.SetActive(true);
      }
    }
  }
}
