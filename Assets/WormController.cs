using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//In Honour of John Choi
public class WormController : MonoBehaviour
{
  public static GameObject curWorm;
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
  
  private float playerMotorSpeed = 100f;
  private float maxTorque = 1000f;
  
  void Start()
  {
    if(curWorm != null){Destroy(curWorm);}
    curWorm = this.gameObject;
    
    InitHinges();
    InitLineRenderers();
  }

  public GameObject GetWormCenter(){
    return centerRB.gameObject;
  }
  public GameObject GetWormLSide(){
    return tailRB.gameObject;
  }
  public GameObject GetWormASide(){
    return headRB.gameObject;
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
    lineFirstHalf.startColor = normalColor;
    lineFirstHalf.endColor = normalColor;
    lineFirstHalf.numCornerVertices	= cornerVertices;
    lineFirstHalf.numCapVertices = capVertices;

    lineSecondHalf.positionCount = 6;
    lineSecondHalf.startWidth = startThickness;
    lineSecondHalf.endWidth = endThickness;
    lineSecondHalf.startColor = normalColor;
    lineSecondHalf.endColor = normalColor;
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
    if(Input.GetKeyDown(keySwapKeys)){
      SwapControlsHands();
    }
  }

  private KeyCode keySwapKeys = KeyCode.Q;

  private KeyCode keyAFriction = KeyCode.A;
  public bool frictionAOn = false;
  private KeyCode keyAUp = KeyCode.S;
  private KeyCode keyADown = KeyCode.W;
  private KeyCode keyAFlex = KeyCode.D;

  private KeyCode keyLFriction = KeyCode.L;
  public bool frictionLOn = false;
  private KeyCode keyLUp = KeyCode.K;
  private KeyCode keyLDown = KeyCode.I;
  private KeyCode keyLFlex = KeyCode.J;

  private KeyCode keyFlexStraight = KeyCode.Space; 

  private Color normalColor = Color.black;
  private Color frictionColor = Color.yellow;

  [SerializeField] WormEnd wormEndA;
  [SerializeField] WormEnd wormEndL;

  private void UpdateWormProperties(){

    if(Input.GetKeyDown(keyAFriction))
    {
      wormEndA.TryToStick();
      frictionLOn = true;
      headRB.sharedMaterial = wormMaterialFriction;
      lineFirstHalf.endColor = frictionColor;
    }
    if(Input.GetKeyDown(keyLFriction))
    {
      wormEndL.TryToStick();
      frictionAOn = true;
      tailRB.sharedMaterial = wormMaterialFriction;
      lineSecondHalf.endColor = frictionColor;
    }
    
    if(Input.GetKeyUp(keyAFriction))
    {
      wormEndA.Unstick();
      frictionLOn = false;
      headRB.sharedMaterial = defaultMaterial;
      headSpriteRenderer.color = normalColor;
      lineFirstHalf.endColor = normalColor;
    
    }
    if(Input.GetKeyUp(keyLFriction))
    {
      wormEndL.Unstick();
      frictionAOn = false;
      tailRB.sharedMaterial = defaultMaterial;
      tailSpriteRenderer.color = normalColor;
      lineSecondHalf.endColor = normalColor;
    }

    // flex straight coloring
    if(Input.GetKeyDown(keyAFlex)){
      lineFirstHalf.startColor = flexStraightColor;
    }
    if(Input.GetKeyUp(keyAFlex) && !Input.GetKey(keyFlexStraight)){
      lineFirstHalf.startColor = normalColor;
    }
    if(Input.GetKeyDown(keyLFlex)){
      lineSecondHalf.startColor = flexStraightColor;
    }
    if(Input.GetKeyUp(keyLFlex) && !Input.GetKey(keyFlexStraight)){
      lineSecondHalf.startColor = normalColor;
    }
    if(Input.GetKeyDown(keyFlexStraight)){
      lineFirstHalf.startColor = flexStraightColor;
      lineSecondHalf.startColor = flexStraightColor;
    }
    if(Input.GetKeyUp(keyFlexStraight)){
      if(!Input.GetKey(keyAFlex)){
        lineFirstHalf.startColor = normalColor;
      }
      if(!Input.GetKey(keyLFlex)){
        lineSecondHalf.startColor = normalColor;
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
    if(Input.GetKey(keyAUp))
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
    else if(Input.GetKey(keyADown))
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

    if(Input.GetKey(keyLUp))
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
    else if(Input.GetKey(keyLDown))
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
    
    if(Input.GetKey(keyFlexStraight)){
      FlexStraight(true);
      FlexStraight(false);
    }
    else{
      if(Input.GetKey(keyAFlex)){
        FlexStraight(false);
      }
      if(Input.GetKey(keyLFlex)){
        FlexStraight(true);
      }
    }
  }

  float flexStraightStrengthMult = 1f;
  Color flexStraightColor = Color.red;
  float flexStraightMaxAngle = 2f;
  private void FlexStraight(bool rightSide){

    List<HingeJoint2D> hingeJointsList = rightSide ? hingeJoints2ndHalf : hingeJoints1stHalf;

    foreach (HingeJoint2D joint in hingeJointsList)
    {
      if(joint.jointAngle > flexStraightMaxAngle){
        joint.useMotor = true;
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = 0f- playerMotorSpeed * flexStraightStrengthMult;
        motor.maxMotorTorque = maxTorque * flexStraightStrengthMult;
        joint.motor = motor;
      }
      else if(joint.jointAngle < (0f - flexStraightMaxAngle)){
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

  [SerializeField] Text textA;
  [SerializeField] Text textL;
  private void SwapControlsHands(){
    KeyCode tempUp = keyAUp;
    KeyCode tempDown = keyADown;
    keyAUp = keyLUp;
    keyADown = keyLDown;
    keyLUp = tempUp;
    keyLDown = tempDown;

    KeyCode tempFriction = keyAFriction;
    keyAFriction = keyLFriction;
    keyLFriction = tempFriction;

    KeyCode tempFlex = keyAFlex;
    keyAFlex = keyLFlex;
    keyLFlex = tempFlex;

    string tempText = textA.text;
    textA.text = textL.text;
    textL.text = tempText;

    Debug.Log("Swapped Controls");
  }
}
