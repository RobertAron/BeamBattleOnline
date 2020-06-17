using UnityEngine;


[ExecuteAlways]
public class FlatDistFollowCam : MonoBehaviour
{
    [SerializeField] Vector3 distanceOffsetMin = new Vector3(0,3,-10);
    [SerializeField] Vector3 distanceOffsetMax = new Vector3(0,3,-13);
    [SerializeField] float flatSpeed = 100;
    [SerializeField] float lerpSpeed = 10;
    public Transform objectToFollow = default;
    [SerializeField] float xRotation = 0;
    [SerializeField] float followPositionalSpeed = 5;

    void Start()
    {
        SnapToTargetPosition();
    }

    void Update()
    {
        if(objectToFollow==null) return;
        if(!Application.IsPlaying(gameObject)) SnapToTargetPosition();
        Quaternion targetRotation = GetTargetRotation();
        Quaternion nextFrameRotation = Quaternion.Lerp(transform.rotation,targetRotation,Time.deltaTime*lerpSpeed);
        nextFrameRotation = Quaternion.RotateTowards(nextFrameRotation,targetRotation,Time.deltaTime*flatSpeed);
        transform.rotation = Quaternion.Euler(xRotation,nextFrameRotation.eulerAngles.y,nextFrameRotation.eulerAngles.z);
        UpdatePosition();
    }

    void SnapToTargetPosition(){
        transform.rotation = GetTargetRotation();
        transform.position = GetTargetPosition();
    }

    Quaternion GetTargetRotation(){
        return objectToFollow.rotation;
    }

    float distPercent = 0;
    Vector3 GetTargetPosition(){
        bool isBikeBoosting = IsBikeAndBoosting();
        if(isBikeBoosting) distPercent = Mathf.MoveTowards(distPercent,1,.07f);
        else distPercent = Mathf.MoveTowards(distPercent,0,.07f);
        Vector3 distanceOffset = Vector3.Lerp(distanceOffsetMin,distanceOffsetMax,distPercent);
        Vector3 targetPosition = transform.rotation*distanceOffset+objectToFollow.position;
        return targetPosition;
    }

    bool IsBikeAndBoosting(){
        var crbs = objectToFollow.GetComponent<CustomRBSync>();
        if(crbs==null) return false;
        if(Mathf.Approximately(crbs.velocity.magnitude, BikeMovement.lowSpeed)) return false;
        return true;
    }

    void UpdatePosition(){
        Vector3 targetPosition = GetTargetPosition();
        transform.position = targetPosition;
    }

}