using UnityEngine;

namespace FireByte
{
    public delegate void SimulateEvent();
    public delegate void LightSaberRotateEvent(LightSaberName lightSaberName,PlayerType playerType, RotateDirection rotateDirection, float value);
    public delegate void HitEvent(Vector3 hittedPoint);
    public delegate void ResetEvent();
    public delegate void GameOverEvent();
}