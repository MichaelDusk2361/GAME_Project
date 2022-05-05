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
    [SerializeField] float maxZ = 0;

    void LateUpdate()
    {
        if (Targets.Count == 0)
            return;

        Vector3 position = transform.position;

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
            position.y += zoomSpeed * Time.deltaTime;
            position.z -= zoomSpeed * Time.deltaTime;
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
                position.y -= zoomSpeed * Time.deltaTime;
                position.z += zoomSpeed * Time.deltaTime;
            }
        }

        float normPosX = 0;
        //float normPosZ = 0;
        for (int i = 0; i < Targets.Count; i++)
        {
            normPosX = Targets[i].position.x;
            //normPosZ = Targets[i].position.z;
        }

        transform.position = new Vector3(
            normPosX / Targets.Count,
            Mathf.Clamp(
                position.y,
                minY,
                float.MaxValue),
            Mathf.Clamp(
                position.z,
                float.MinValue,
                maxZ));
    }
}
