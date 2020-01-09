using UnityEngine;
using UnityEngine.Events;


public class InputManager : MonoBehaviour
{
    public UnityEvent turnLeft;
    public UnityEvent turnRight;

    void Update(){
        if(Input.GetKeyDown(KeyCode.LeftArrow)) turnLeft.Invoke();
        if(Input.GetKeyDown(KeyCode.RightArrow)) turnRight.Invoke();
    }
}