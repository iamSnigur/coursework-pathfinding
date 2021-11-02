using System.Threading;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct BlendColorJob : IJob
{
    public Color NewColor;
    public NativeArray<Color> PreviosColor;
    public NativeArray<Color> CurrentColor;

    public void Execute()
    {
        float colorDelta = 0f;

        while (!(colorDelta >= 1f))
        {
            colorDelta += 0.006f;
            colorDelta = Mathf.Clamp01(colorDelta);
            CurrentColor[0] = Color.Lerp(PreviosColor[0], NewColor, colorDelta);

            Thread.Sleep(5);
        }

        PreviosColor[0] = NewColor;
    }
}