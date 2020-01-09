using UnityEngine;

[ExecuteAlways]
public class FlatDistFollowCam : MonoBehaviour
{
    [SerializeField] Vector3 distanceOffset = new Vector3(0,3,-10);
    [SerializeField] float flatSpeed = 100;
    [SerializeField] float lerpSpeed = 10;
    [SerializeField] Transform objectToFollow = default;

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
        transform.rotation = nextFrameRotation;
        UpdatePosition();
    }

    void SnapToTargetPosition(){
        transform.rotation = GetTargetRotation();
        UpdatePosition();
    }

    Quaternion GetTargetRotation(){
        return objectToFollow.rotation;
    }

    void UpdatePosition(){
        transform.position = transform.rotation*distanceOffset+objectToFollow.position;
    }
}