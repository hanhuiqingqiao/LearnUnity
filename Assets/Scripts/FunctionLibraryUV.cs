using UnityEngine;
using static UnityEngine.Mathf;
public static class FunctionLibraryUV
{
    public delegate float Function(float x, float z, float t);

    private static Function[] _functions = {Wave, MultiWave, MultiWaveXZ, Ripple};
    
    public enum FunctionName {Wave, MultiWave, MultiWaveXZ, Ripple}

    public static Function GetFunction(FunctionName index)
    {
        return _functions[(int)index];
    }
    
    public static float Wave(float x, float z, float t)
    {
        return Sin(PI * (x + z + t));
    }
    
    public static float MultiWave(float x, float z, float t)
    {
        float y = Sin(PI * (x + 0.5f * t));
        y += Sin(2f * PI * (z + t)) * 0.5f;
        return y * (2f / 3f);
    }
    
    public static float MultiWaveXZ(float x, float z, float t)
    {
        float y = Sin(PI * (x + 0.5f * t));
        y += Sin(2f * PI * (z + t)) * 0.5f;
        y += Sin(PI * (x + z + 0.25f * t));
        return y * (1f / 2.5f);
    }
    public static float Ripple(float x, float z, float t)
    {
        float d = Sqrt(x * x + z * z);
        float y = Sin(PI * (4f * d - t));
        return y / (1f + 10f * d);
    }
}
