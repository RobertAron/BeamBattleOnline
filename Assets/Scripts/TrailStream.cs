using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
[NetworkSettings(sendInterval = 0.05f)]
public class TrailStream : NetworkBehaviour, Attachable
{
  [SyncVar] Vector3 startingPosition;
  // Must have BikeMovement or TrailStream on GO
  [SerializeField] [SyncVar] public GameObject attachedTo;
  [SerializeField] float maxLength = 20;
  BikeMovement createdBy;
  public void StartStream(BikeMovement bikeGO)
  {
    createdBy = bikeGO;
    startingPosition = transform.position;
    this.attachedTo = bikeGO.gameObject;
    var playerName = bikeGO.GetPlayerName();
    GetComponent<WallCollision>().SetKillfeedName(playerName);
  }

  public void SetAttachment(GameObject attachedTo)
  {
    this.attachedTo = attachedTo;
  }

  void FixedUpdate()
  {
    var endingPosition = GetEndingPosition();
    float totalLength = GetTotalLength();
    float extraLength = Mathf.Max(totalLength - maxLength, 0);
    startingPosition = Vector3.MoveTowards(startingPosition, endingPosition, extraLength);
    if (
      extraLength > 0 &&
      Vector3.Distance(endingPosition, startingPosition) < float.Epsilon &&
      isServer
    ) NetworkServer.Destroy(this.gameObject);
    transform.position = (startingPosition + endingPosition) / 2;
    float length = GetSegmentLength();
    float additionalForCoverage = transform.localScale.x;
    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, length + additionalForCoverage);
  }

  Vector3 GetEndingPosition()
  {
    if(attachedTo==null) return transform.forward * (transform.localScale.z - transform.localScale.x);
    Attachable potentialBikeMovement = attachedTo?.GetComponent<BikeMovement>();
    Attachable potentialTrailStream = attachedTo?.GetComponent<TrailStream>();
    Attachable attachment = potentialBikeMovement ?? potentialTrailStream;
    return attachment.GetAttachPoint();
  }

  float GetSegmentLength()
  {
    var endingPosition = GetEndingPosition();
    return (startingPosition - endingPosition).magnitude;
  }

  public float GetTotalLength()
  {
    GetEndingPosition();
    float selfLength = GetSegmentLength();
    if(attachedTo == null) return selfLength;
    TrailStream attachedTrail = attachedTo.GetComponent<TrailStream>();
    float forwardLength = attachedTrail?.GetTotalLength() ?? 0;
    return forwardLength + selfLength;
  }

  void OnTriggerEnter(Collider other)
  {
    var otherBike = other.GetComponent<BikeMovement>();
    // todo get the players name and stuff to make killfeed
    if(otherBike!=null) createdBy.IncreaseBoostSize();
  }
  public Vector3 GetAttachPoint()
  {
    return startingPosition;
  }
}
