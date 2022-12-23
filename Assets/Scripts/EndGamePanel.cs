using System;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
   public static Text winLose;
   public static Text reward;
   private static int coinsCollected = 0;

   private void Awake()
   {
      winLose = this.gameObject.transform.Find("winner").GetComponent<Text>();
      reward = this.gameObject.transform.Find("Reward").GetComponent<Text>();
   }

   public static void updateWin(bool val, int killcount)
   {
      if (val)
      {
         winLose.text = "YOU WON!";
         coinsCollected =killcount + 3;
      }
      else
      {
         winLose.text = "Sorry you lost";
         coinsCollected = killcount;
      }

      reward.text = "Coins Collected: " + coinsCollected;
      PlayerData.Instance.winMoney(coinsCollected);
   }
   
}
