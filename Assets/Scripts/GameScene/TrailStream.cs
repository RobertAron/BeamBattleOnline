using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
[NetworkSettings(sendInterval = 0.01f)]
public class TrailStream : NetworkBehaviour, Attachable
{
    [SerializeField] MeshRenderer trailMR = default;
    private MaterialPropertyBlock trailMaterialBlock = default;
    [SerializeField] [SyncVar(hook = "SetTrailColor")] Color color = PlayerPrefsController.defaultAccentColor;
    public void SetTrailColor(Color color)
    {
        this.color = color;
        trailMaterialBlock.SetColor("_Color", color);
        trailMR.SetPropertyBlock(trailMaterialBlock);
    }


    [SyncVar] Vector3 startingPosition;
    // Must have BikeMovement or TrailStream on GO
    [SerializeField] [SyncVar] public GameObject attachedTo;
    [SerializeField] float maxLength = 20;
    BikeMovement createdBy;

    private void Start() {
        Debug.Log("Started at...");
        Debug.Log(transform.position);
        SetTrailColor(color);
    }

    private void Awake()
    {
        trailMaterialBlock = new MaterialPropertyBlock();
    }

    [Server]
    public void StartStream(BikeMovement bikeGO)
    {
        createdBy = bikeGO;
        transform.position = bikeGO.transform.position;
        startingPosition = bikeGO.transform.position;
        this.attachedTo = bikeGO.gameObject;
        var playerName = bikeGO.GetPlayerName();
        SetTrailColor(bikeGO.GetAccentColor());
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
        if(isServer){
            startingPosition = Vector3.MoveTowards(startingPosition, endingPosition, extraLength);
            if (
                extraLength > 0 &&
                Vector3.Distance(endingPosition, startingPosition) < float.Epsilon &&
                isServer
            ) NetworkServer.Destroy(this.gameObject);
        }
        transform.position = (startingPosition + endingPosition) / 2;
        float length = GetSegmentLength();
        float additionalForCoverage = transform.localScale.x;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, length + additionalForCoverage);
    }

    Vector3 GetEndingPosition()
    {
        if (attachedTo == null) return transform.forward * (transform.localScale.z - transform.localScale.x);
        Attachable potentialBikeMovement = attachedTo.GetComponent<BikeMovement>();
        Attachable potentialTrailStream = attachedTo.GetComponent<TrailStream>();
        Attachable fakeAttachment = attachedTo.GetComponent<FakeAttachment>();
        Attachable attachment = potentialBikeMovement ?? potentialTrailStream ?? fakeAttachment;
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
        if (attachedTo == null) return selfLength;
        TrailStream attachedTrail = attachedTo.GetComponent<TrailStream>();
        FakeAttachment fakeAttachment = attachedTo.GetComponent<FakeAttachment>();
        float forwardLength = attachedTrail?.GetTotalLength() ?? fakeAttachment?.GetTotalLength() ?? 0;
        return forwardLength + selfLength;
    }

    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        var otherBike = other.GetComponent<BikeMovement>();
        // todo get the players name and stuff to make killfeed
        if (otherBike != null) createdBy.IncreaseBoostSize();
    }
    public Vector3 GetAttachPoint()
    {
        return startingPosition;
    }
    public void IncreasePlayerTakedowns(){
        createdBy.IncreasePlayerTakedownCount();
    }
}
