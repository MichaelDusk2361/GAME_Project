using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public List<Transform> Targets = new();
    [SerializeField] int bufferIn = 140;
    [SerializeField] float zoomSpeed = 5;
    [SerializeField] float minY = 5;
    [SerializeField] float zOffset = 0;
    [SerializeField] float zDist = 0;

    float _timer = 0;

    void LateUpdate()
    {
        if (Targets.Count == 0)
            return;

        Vector3 position = transform.position;
        float normPosX = 0;
        float normPosZ = 0;
        for (int i = 0; i < Targets.Count; i++)
        {
            normPosX = Targets[i].position.x;
            normPosZ = Targets[i].position.z;
        }

        normPosX /= Targets.Count;
        normPosZ /= Targets.Count;

        position.x = normPosX;
        position.z = normPosZ;

        Vector3 vp;
        int countIn = 0;
        int cnt = Targets.Count;
        for (int i = 0; i < cnt; ++i)
        {
            vp = Camera.main.WorldToScreenPoint(Targets[i].position);
            if (vp.x > bufferIn &&                  // Inside left
                vp.x < Screen.width - bufferIn &&   // Inside right
                vp.y > bufferIn &&                  // Inside top
                vp.y < Screen.height - bufferIn)    // Inside bottom
            {
                countIn++;
            }
            else break;
        }
        // All targets are in viewport
        if (countIn == cnt)
        {
            _timer += Time.deltaTime;
            if(_timer > 3)
            {
                position.y -= zoomSpeed * Time.deltaTime;
                zDist += zoomSpeed * Time.deltaTime;
            }
        }
        else // Some targets are outside
        {
            _timer = 0;
            position.y += zoomSpeed * Time.deltaTime;
            zDist -= zoomSpeed * Time.deltaTime;
        }

        Vector3 desiredPos = new Vector3(
            normPosX,
            Mathf.Clamp(
                position.y,
                minY,
                float.MaxValue),
            //Mathf.Clamp(
            //    position.z,
            //    float.MinValue,
            //    maxZ));
            Mathf.Clamp(
                zOffset + position.z + zDist,
                float.MinValue,
                zOffset + position.z));

        // Lerp towards pos
        transform.position = desiredPos;
    }
}
