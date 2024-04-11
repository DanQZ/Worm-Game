using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//In Honour of John Choi
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
    float startThickness = 0.9f;
    float endThickness = 0.7f;
    int cornerVertices = 3;
    int capVertices = 7;

    lineFirstHalf.positionCount = 6;
    lineFirstHalf.startWidth = startThickness;
    lineFirstHalf.endWidth = endThickness;
    lineFirstHalf.startColor = Color.black;
    lineFirstHalf.endColor = Color.black;
    lineFirstHalf.numCornerVertices	= cornerVertices;
    lineFirstHalf.numCapVertices = capVertices;

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

  string headFriction = "l";
  string headUp = "k";
  string headDown = "j";

  string tailFriction = "a";
  string tailUp = "s";
  string tailDown = "d";

  private void UpdateWormProperties(){

    if(Input.GetKeyDown(tailFriction))
    {
      headRB.sharedMaterial = wormMaterialFriction;
      headSpriteRenderer.color = Color.red;
      lineFirstHalf.endColor = Color.yellow;
    }
    if(Input.GetKeyDown(headFriction))
    {
      tailRB.sharedMaterial = wormMaterialFriction;
      tailSpriteRenderer.color = Color.red;
      lineSecondHalf.endColor = Color.yellow;
    }
    
    if(Input.GetKeyUp(tailFriction))
    {
      headRB.sharedMaterial = defaultMaterial;
      headSpriteRenderer.color = Color.black;
      lineFirstHalf.endColor = Color.black;
    
    }
    if(Input.GetKeyUp(headFriction))
    {
      tailRB.sharedMaterial = defaultMaterial;
      tailSpriteRenderer.color = Color.black;
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

    // flex straight coloring
    if(Input.GetKeyDown("f")){
      lineFirstHalf.startColor = flexStraightColor;
    }
    if(Input.GetKeyUp("f") && !Input.GetKey(KeyCode.Space)){
      lineFirstHalf.startColor = Color.black;
    }
    if(Input.GetKeyDown("h")){
      lineSecondHalf.startColor = flexStraightColor;
    }
    if(Input.GetKeyUp("h") && !Input.GetKey(KeyCode.Space)){
      lineSecondHalf.startColor = Color.black;
    }
    if(Input.GetKeyDown(KeyCode.Space)){
      lineFirstHalf.startColor = flexStraightColor;
      lineSecondHalf.startColor = flexStraightColor;
    }
    if(Input.GetKeyUp(KeyCode.Space)){
      if(!Input.GetKey("f")){
        lineFirstHalf.startColor = Color.black;
      }
      if(!Input.GetKey("h")){
        lineSecondHalf.startColor = Color.black;
      }
    }
  }
  private void WormTwisting(){
    CurlControls();
    FlexStraightControls();
  }

  private void CurlControls(){
    
    // hide arrows
    kUp.SetActive(false);
    kDown.SetActive(false);
    lUp.SetActive(false);
    lDown.SetActive(false);

    // reset motors
    foreach (HingeJoint2D joint in hingeJoints1stHalf)
    {
      joint.useMotor = false;
    }
    foreach (HingeJoint2D joint in hingeJoints2ndHalf)
    {
      joint.useMotor = false;
    }
    if(Input.GetKey(tailUp))
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
    else if(Input.GetKey(tailDown))
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

    if(Input.GetKey(headUp))
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
    else if(Input.GetKey(headDown))
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
  }

  private void FlexStraightControls(){
    
    if(Input.GetKey(KeyCode.Space)){
      FlexStraight(true);
      FlexStraight(false);
    }
    else{
      if(Input.GetKey("f")){
        FlexStraight(false);
      }
      if(Input.GetKey("h")){
        FlexStraight(true);
      }
    }
  }

  float flexStraightStrengthMult = 1f;
  Color flexStraightColor = Color.red;
  private void FlexStraight(bool rightSide){

    List<HingeJoint2D> hingeJointsList = rightSide ? hingeJoints2ndHalf : hingeJoints1stHalf;

    foreach (HingeJoint2D joint in hingeJointsList)
    {
      if(joint.jointAngle > 0){
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = 0f- playerMotorSpeed * flexStraightStrengthMult;
        motor.maxMotorTorque = maxTorque * flexStraightStrengthMult;
        joint.motor = motor;
      }
      else if(joint.jointAngle < 0){
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = playerMotorSpeed * flexStraightStrengthMult;
        motor.maxMotorTorque = maxTorque * flexStraightStrengthMult;
        joint.motor = motor;
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
