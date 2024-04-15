using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
  [SerializeField] Text timerText;
  private string timerPrefix = "Time (seconds): ";
  private float timer = 0f;
  void Update()
  {
    timerText.text = timerPrefix + (timer).ToString("F2");
    timer += Time.deltaTime;
  }
  public void CloseGame(){
    Application.Quit();
  }
  public void ResetTimer(){
    timer = 0f;
  }
}
