using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
  [SerializeField] Text timerText;
  private string timerPrefix = "Time (seconds): ";
  void Update()
  {
    timerText.text = timerPrefix + Time.time.ToString("F2");
  }
  public void CloseGame(){
    Application.Quit();
  }
}
