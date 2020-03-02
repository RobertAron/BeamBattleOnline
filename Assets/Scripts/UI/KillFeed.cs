using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFeed : MonoBehaviour
{

  #region  Singleton
  public static KillFeed instance;
  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
  }
  #endregion

  [SerializeField] GameObject killFeedItem;

  public void AddKillFeedItem(string slayer, string victim)
  {
    var go = Instantiate(killFeedItem, transform);
    KillFeedText kft = go.GetComponent<KillFeedText>();
    kft.SetActors(slayer, victim);

  }
}
