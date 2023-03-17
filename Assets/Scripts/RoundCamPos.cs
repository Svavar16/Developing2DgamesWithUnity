using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundCamPos : CinemachineExtension
{
    public float PixelPerUnit = 32.0f;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            Vector3 pos = state.FinalPosition;
            Vector3 pos2 = new Vector3(Round(pos.x), Round(pos.y), pos.z);

            state.PositionCorrection += pos2 - pos;
        }

        float Round(float x)
        {
            return Mathf.Round(x * PixelPerUnit) / PixelPerUnit;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
