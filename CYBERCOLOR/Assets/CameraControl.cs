using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public List<Transform> Targets = new();
    [SerializeField] int bufferOut = 100;
    [SerializeField] int bufferIn = 140;
    void Update()
    {
        if (Targets.Count == 0)
            return;

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
            transform.position -= new Vector3(0, 0, 7) * Time.deltaTime;
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
                transform.position += new Vector3(0, 0, 7) * Time.deltaTime;
        }
    }
}
