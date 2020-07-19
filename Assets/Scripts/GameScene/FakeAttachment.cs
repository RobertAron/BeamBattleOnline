using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class FakeAttachment : NetworkBehaviour, Attachable
{
    Rigidbody rb;
    [SerializeField] Vector3 startPosition;

    private void Start() {
        startPosition = transform.position;
        StartCoroutine(DelayedDestroy());
    }

    public Vector3 GetAttachPoint()
    {
        return startPosition;
    }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    [ServerCallback]
    private void FixedUpdate() {
        rb.velocity = transform.forward * BikeMovement.lowSpeed;
    }

    public float GetTotalLength(){
        return Vector3.Distance(startPosition,transform.position);
    }

    IEnumerator DelayedDestroy(){
        yield return new WaitForSeconds(5);
        NetworkManager.Destroy(this.gameObject);
    }
}
