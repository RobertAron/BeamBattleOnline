using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class BikeMovement : NetworkBehaviour
{
  Rigidbody rb;
  public float speed;
  [SerializeField] GameObject trailPrefab = default;
  [SerializeField] string playerName;
  TrailStream currentStream = null;
  [SerializeField] float streamLifetime = 4;
  void Start()
  {
    rb = GetComponent<Rigidbody>();
    StartNewTrail();
  }

  void FixedUpdate()
  {
    UpdatePosition();
  }

  [ServerCallback]
  void UpdatePosition()
  {
    // transform.position += transform.forward*speed*Time.deltaTime;
    rb.velocity = transform.forward * speed;
  }

  public void Turn(bool left)
  {
    float direction = left ? -1 : 1;
    transform.rotation = transform.rotation * Quaternion.Euler(0, 90 * direction, 0);
    StartNewTrail();
  }

  [ServerCallback]
  void StartNewTrail()
  {
    if (currentStream != null) currentStream.BreakStream(transform.position);
    GameObject go = Instantiate(trailPrefab, transform.position, transform.rotation);
    currentStream = go.GetComponent<TrailStream>();
    currentStream.StartStream(transform.position, this, streamLifetime, speed, playerName);
    NetworkServer.Spawn(go);
  }

  public void IncraseStreamSize()
  {
    ++streamLifetime;
  }

  public void SetPlayerName(string newPlayerName){
    playerName = newPlayerName;
  }
  public string GetPlayerName(){
    return playerName;
  }
}
