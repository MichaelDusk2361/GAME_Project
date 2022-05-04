using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public List<Transform> Targets = new();
    [SerializeField] int bufferOut = 100;
    [SerializeField] int bufferIn = 140;
    [SerializeField] float zoomSpeed = 5;
    [SerializeField] float minY = 5;
    [SerializeField] Vector3 offset;

    float yVal = 0;

    void LateUpdate()
    {
        if (Targets.Count == 0)
            return;

        yVal = transform.position.y;

        bool outside = false;
        Vector3 vp;
        for (int i = 0; i < Targets.Count; ++i)
        {
            if (Targets[i].GetComponent<Renderer>().isVisible)
            {
                vp = Camera.main.WorldToScreenPoint(Targets[i].position);
                if (vp.x < bufferOut || vp.x > Screen.width - bufferOut || vp.y < bufferOut || vp.y > Screen.height - bufferOut)
                {
                    outside = true;
                    break;
                }
                continue;
            }
            outside = true;
            break;
        }
        if (outside)
        {
            yVal = transform.position.y + offset.y + zoomSpeed * Time.deltaTime;
        }
        else
        {
            int countIn = 0;
            int cnt = Targets.Count;
            for (int i = 0; i < cnt; ++i)
            {
                vp = Camera.main.WorldToScreenPoint(Targets[i].position);
                if (vp.x > bufferIn && vp.x < Screen.width - bufferIn && vp.y > bufferIn && vp.y < Screen.height - bufferIn) ++countIn;
            }
            if (countIn == cnt)
            {
                yVal = transform.position.y + offset.y - zoomSpeed * Time.deltaTime;
            }
        }

        float normPosX = 0;
        float normPosZ = 0;
        for (int i = 0; i < Targets.Count; i++)
        {
            normPosX = Targets[i].position.x;
            normPosZ = Targets[i].position.z;
        }

        transform.position = new Vector3(
            offset.x + normPosX / Targets.Count,
             Mathf.Clamp(
                    yVal,
                    minY,
                    float.MaxValue),
            offset.z + normPosZ / Targets.Count);
    }
}
