using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormControllerV2 : MonoBehaviour
{
  [SerializeField] Rigidbody2D headRB;
  [SerializeField] Rigidbody2D tailRB;
  [SerializeField] SpriteRenderer headSpriteRenderer;
  [SerializeField] SpriteRenderer tailSpriteRenderer;
  [SerializeField] GameObject bodyParentObject;
  private List<Rigidbody2D> body1stHalf = new List<Rigidbody2D>();
  private List<Rigidbody2D> body2ndHalf = new List<Rigidbody2D>();

  [SerializeField] GameObject kUp;
  [SerializeField] GameObject kDown;
  [SerializeField] GameObject lUp;
  [SerializeField] GameObject lDown;
  
  void Start()
  {
    int index = 0;
    body1stHalf.Add(headRB);
    foreach (Rigidbody2D item in bodyParentObject.GetComponentsInChildren<Rigidbody2D>())
    {
        if(index < 4)
        {
          body1stHalf.Add(item);
        }
        else
        {
          body2ndHalf.Add(item);
        }
        index++;
    }
    body2ndHalf.Add(tailRB);
    InitLineRenderers();
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

  private float wormForce = 100f;
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

    if(Input.GetKey("k"))
    {
      headRB.sharedMaterial = wormMaterialFriction;
      headSpriteRenderer.color = Color.red;
    }
    if(Input.GetKey("l"))
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

    kUp.SetActive(false);
    kDown.SetActive(false);
    lUp.SetActive(false);
    lDown.SetActive(false);

    foreach (Rigidbody2D bodyPartRB in body1stHalf)
    {
        bodyPartRB.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    foreach (Rigidbody2D bodyPartRB in body2ndHalf)
    {
        bodyPartRB.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    if(Input.GetKey("a"))
    {
      foreach (Rigidbody2D bodyPartRB in body1stHalf)
      {
        bodyPartRB.AddForce(bodyPartRB.gameObject.transform.right *-1f* wormForce);
        bodyPartRB.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
      }
    }
    if( Input.GetKey("s"))
    {
      foreach (Rigidbody2D bodyPartRB in body1stHalf)
      {
        bodyPartRB.AddForce(bodyPartRB.gameObject.transform.right *wormForce);
        bodyPartRB.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
      }
    }

    if( Input.GetKey("d"))
    {
      foreach (Rigidbody2D bodyPartRB in body2ndHalf)
      {
        bodyPartRB.AddForce(bodyPartRB.gameObject.transform.right *-1f* wormForce);
        bodyPartRB.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
      }
    }
    if( Input.GetKey("f"))
    {
      foreach (Rigidbody2D bodyPartRB in body2ndHalf)
      {
        bodyPartRB.AddForce(bodyPartRB.gameObject.transform.right *wormForce);
        bodyPartRB.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
      }
    }
  }
}
