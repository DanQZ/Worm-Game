using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour
{
  [SerializeField] Rigidbody2D headRB;
  [SerializeField] Rigidbody2D centerRB;
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
  
  private float playerMotorSpeed = 150f;
  private float maxTorque = 1000f;
  
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
  [SerializeField] LineRenderer lineFirstHalf;
  [SerializeField] LineRenderer lineSecondHalf;
  private void InitLineRenderers(){
    float startThickness = 0.75f;
    float endThickness = 0.6f;
    int cornerVertices = 3;
    int capVertices = 7;

    lineFirstHalf.positionCount = 6;
    lineFirstHalf.startWidth = startThickness;
    lineFirstHalf.endWidth = endThickness;
    lineFirstHalf.startColor = Color.black;
    lineFirstHalf.endColor = Color.black;
    lineFirstHalf.numCornerVertices	= cornerVertices;
    lineFirstHalf.numCapVertices = capVertices;
    /*
    float totalThicknessDiff = startThickness - endThickness;
    float thicknessToEnd = totalThicknessDiff / 5;
    int index = 0;
    float thicknessPercent = 1f;
    foreach (HingeJoint2D item in hingeJoints1stHalf)
    {
      thicknessPercent = 1f - thicknessToEnd * index;
      index++;
      item.gameObject.GetComponent<CircleCollider2D>().radius = startThickness * thicknessPercent;
    }
    headRB.gameObject.GetComponent<CircleCollider2D>().radius = endThickness;

    index = 0;
    foreach (HingeJoint2D item in hingeJoints2ndHalf)
    {
      thicknessPercent = 1f - thicknessToEnd * index;
      index++;
      item.gameObject.GetComponent<CircleCollider2D>().radius = startThickness * thicknessPercent;
    }
    tailRB.gameObject.GetComponent<CircleCollider2D>().radius = endThickness;
    */
    lineSecondHalf.positionCount = 6;
    lineSecondHalf.startWidth = startThickness;
    lineSecondHalf.endWidth = endThickness;
    lineSecondHalf.startColor = Color.black;
    lineSecondHalf.endColor = Color.black;
    lineSecondHalf.numCornerVertices	= cornerVertices;
    lineSecondHalf.numCapVertices = capVertices;
  }

  [SerializeField] PhysicsMaterial2D wormMaterialFriction;
  [SerializeField] PhysicsMaterial2D defaultMaterial;
  void FixedUpdate()
  { 
    WormTwisting();
  }

  void Update(){
    UpdateWormProperties();
    UpdateLineRenderer();
  }

  private void UpdateWormProperties(){

    if(Input.GetKeyDown("k"))
    {
      headRB.sharedMaterial = wormMaterialFriction;
      headSpriteRenderer.color = Color.red;
      lineFirstHalf.startColor = Color.black;
      lineFirstHalf.endColor = Color.yellow;
    }
    if(Input.GetKeyDown("l"))
    {
      tailRB.sharedMaterial = wormMaterialFriction;
      tailSpriteRenderer.color = Color.red;
      lineSecondHalf.startColor = Color.black;
      lineSecondHalf.endColor = Color.yellow;
    }
    
    if(Input.GetKeyUp("k"))
    {
      headRB.sharedMaterial = defaultMaterial;
      headSpriteRenderer.color = Color.black;
      lineFirstHalf.startColor = Color.black;
      lineFirstHalf.endColor = Color.black;
    
    }
    if(Input.GetKeyUp("l"))
    {
      tailRB.sharedMaterial = defaultMaterial;
      tailSpriteRenderer.color = Color.black;
      lineSecondHalf.startColor = Color.black;
      lineSecondHalf.endColor = Color.black;
    }

    foreach (HingeJoint2D hingeJoint in allHingeJoints)
    {
      if(hingeJoint.useMotor){
        hingeJoint.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
      }
      else{
        hingeJoint.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
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
        motor.maxMotorTorque = maxTorque;
        joint.motor = motor;
        kDown.SetActive(true);
      }
    }
    else if(Input.GetKey("s"))
    {
      foreach (HingeJoint2D joint in hingeJoints1stHalf)
      {
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = 0f - playerMotorSpeed;
        motor.maxMotorTorque = maxTorque;
        joint.motor = motor;
        kUp.SetActive(true);
      }
    }

    if(Input.GetKey("d"))
    {
      foreach (HingeJoint2D joint in hingeJoints2ndHalf)
      {
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = 0f- playerMotorSpeed;
        motor.maxMotorTorque = maxTorque;
        joint.motor = motor;
        lDown.SetActive(true);
      }
    }
    else if(Input.GetKey("f"))
    {
      foreach (HingeJoint2D joint in hingeJoints2ndHalf)
      {
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = playerMotorSpeed;
        motor.maxMotorTorque = maxTorque;
        joint.motor = motor;
        lUp.SetActive(true);
      }
    }

    if(Input.GetKey(KeyCode.Space)){
      FlexStraight();
    }
  }
  float flexStraightMult = 1f;

  private void FlexStraight(){
    foreach (HingeJoint2D joint in allHingeJoints)
    {
      joint.useMotor = true;
      JointMotor2D motor = joint.motor;
      if(joint.jointAngle > 0f){
        motor.motorSpeed = 0f- playerMotorSpeed * flexStraightMult;
      }
      else{
        motor.motorSpeed = playerMotorSpeed* flexStraightMult;
      }
      motor.maxMotorTorque = maxTorque * flexStraightMult;
      joint.motor = motor;
    }
  }

  private void TwistCenter(){
    foreach (HingeJoint2D joint in centerRB.gameObject.GetComponents<HingeJoint2D>())
    {
      joint.useMotor = false;
    }
    if(Input.GetKey("h"))
    {
      foreach (HingeJoint2D joint in centerRB.gameObject.GetComponents<HingeJoint2D>())
      {
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = 0f - playerMotorSpeed;
        motor.maxMotorTorque = maxTorque;
        joint.motor = motor;
        lDown.SetActive(true);
      }
    }
    else if(Input.GetKey("j"))
    {
      foreach (HingeJoint2D joint in centerRB.gameObject.GetComponents<HingeJoint2D>())
      {
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = playerMotorSpeed;
        motor.maxMotorTorque = maxTorque;
        joint.motor = motor;
        lUp.SetActive(true);
      }
    }
  }

  private void UpdateLineRenderer(){
    lineFirstHalf.SetPosition(5, headRB.transform.position);
    lineFirstHalf.SetPosition(4, hingeJoints1stHalf[0].transform.position);
    lineFirstHalf.SetPosition(3, hingeJoints1stHalf[1].transform.position);
    lineFirstHalf.SetPosition(2, hingeJoints1stHalf[2].transform.position);
    lineFirstHalf.SetPosition(1, hingeJoints1stHalf[3].transform.position);
    lineFirstHalf.SetPosition(0, centerRB.gameObject.transform.position);


    lineSecondHalf.SetPosition(0, centerRB.gameObject.transform.position);
    lineSecondHalf.SetPosition(1, hingeJoints2ndHalf[0].transform.position);
    lineSecondHalf.SetPosition(2, hingeJoints2ndHalf[1].transform.position);
    lineSecondHalf.SetPosition(3, hingeJoints2ndHalf[2].transform.position);
    lineSecondHalf.SetPosition(4, hingeJoints2ndHalf[3].transform.position);
    lineSecondHalf.SetPosition(5, tailRB.transform.position);
  }
}
