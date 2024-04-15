using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRestart : MonoBehaviour
{
  [SerializeField] GameObject wormPrefab;
  [SerializeField] GameObject mainCamera;
  [SerializeField] GameObject spawnPoint;
  [SerializeField] GameObject timerText;

  public void RestartGame(){
    Destroy(WormController.curWorm);
    GameObject worm = Instantiate(wormPrefab, spawnPoint.transform.position, Quaternion.identity);
    WormController curController = worm.GetComponent<WormController>();
    mainCamera.GetComponent<CameraController>().SetTarget(curController.GetWormASide().transform, curController.GetWormLSide().transform);
    timerText.GetComponent<TimerScript>().ResetTimer();
  }
}
