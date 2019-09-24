using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //default camera positions
    private Vector3 cameraDefaultRelativePosition = new Vector3(0f, 6.13f, -6.8f);
    private Quaternion cameraDefaultRotation = Quaternion.Euler(new Vector3(28.76f, 0f, 0f));

    //actual relative position of camera
    private Vector3 cameraRelativePosition;
    private Quaternion cameraRotation;

    //new position of camera (when reaching a new obstacle)
    private Vector3 newCameraRelativePosition;
    private Quaternion newCameraRotation;

    private Vector3 currentVelocity = Vector3.zero;
    private float cameraTotalLerpTime = 0.5f;
    private float cameraLerpTime = 0f;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //set the starting position of the camera
        cameraRelativePosition = cameraDefaultRelativePosition;
        cameraRotation = cameraDefaultRotation;
        transform.position = player.transform.position + cameraRelativePosition;
        transform.rotation = cameraRotation;
    }

    // Update is called once per frame
    void Update()
    {
        //smooth camera movement lerp depending on obstacle
        if (cameraLerpTime > 0f)
        {
            cameraLerpTime -= Time.deltaTime;
            if (cameraLerpTime < 0f)
            {
                cameraLerpTime = 0f;
                cameraRotation = newCameraRotation;
                cameraRelativePosition = newCameraRelativePosition;
            }
            transform.rotation = Quaternion.Slerp(cameraRotation, newCameraRotation, Mathf.Pow(1f - cameraLerpTime / cameraTotalLerpTime, 2f));
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + Vector3.Lerp(cameraRelativePosition, newCameraRelativePosition, Mathf.Pow(1f - cameraLerpTime / cameraTotalLerpTime, 2f)), ref currentVelocity, 0.1f);

        }
        else
        {
            //camera moves smoothly so that player input can be better appreciated
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + cameraRelativePosition, ref currentVelocity, 0.1f);
            transform.rotation = cameraRotation;
        }

    }

    //change camera orientation: depending on the obstacle, camera position changes
    public void ChangeCameraRelativePosition(Vector3 position, Quaternion rotation)
    {
            newCameraRelativePosition = position;
            newCameraRotation = rotation;
            cameraLerpTime = cameraTotalLerpTime;
    }

    //set default camera orientation
    public void DefaultCameraRelativePosition()
    {
        newCameraRelativePosition = cameraDefaultRelativePosition;
        newCameraRotation = cameraDefaultRotation;
        cameraLerpTime = cameraTotalLerpTime;
    }

}
