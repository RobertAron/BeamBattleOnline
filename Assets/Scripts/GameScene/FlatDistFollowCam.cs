using UnityEngine;

[ExecuteAlways]
public class FlatDistFollowCam : MonoBehaviour
{
    [SerializeField] Vector3 distanceOffset = new Vector3(0,3,-10);
    [SerializeField] float flatSpeed = 100;
    [SerializeField] float lerpSpeed = 10;
    public Transform objectToFollow = default;
    [SerializeField] float xRotation = 0;
    [SerializeField] float followPositionalSpeed = 5;

    void Start()
    {
        SnapToTargetPosition();
    }

    void FixedUpdate()
    {
        if(objectToFollow==null) return;
        if(!Application.IsPlaying(gameObject)) SnapToTargetPosition();
        Quaternion targetRotation = GetTargetRotation();
        Quaternion nextFrameRotation = Quaternion.Lerp(transform.rotation,targetRotation,Time.fixedDeltaTime*lerpSpeed);
        nextFrameRotation = Quaternion.RotateTowards(nextFrameRotation,targetRotation,Time.fixedDeltaTime*flatSpeed);
        transform.rotation = Quaternion.Euler(xRotation,nextFrameRotation.eulerAngles.y,nextFrameRotation.eulerAngles.z);;
        UpdatePosition();
    }

    void SnapToTargetPosition(){
        transform.rotation = GetTargetRotation();
        transform.position = GetTargetPosition();
    }

    Quaternion GetTargetRotation(){
        return objectToFollow.rotation;
    }

    Vector3 GetTargetPosition(){
        Vector3 targetPosition = transform.rotation*distanceOffset+objectToFollow.position;
        return targetPosition;
    }

    void UpdatePosition(){
        Vector3 targetPosition = GetTargetPosition();
        transform.position = Vector3.Lerp(transform.position,targetPosition,Time.fixedDeltaTime*followPositionalSpeed);
    }

}