using System;
using UnityEngine.Profiling;

namespace Utils.Unity.Runtime.Profiling
{
    public static class ProfilerHelper
    {
        public static bool SampleIfCondition(string name, Func<bool> condition)
        {
            Profiler.BeginSample(name);
            var rval = condition.Invoke();
            Profiler.EndSample();
            return rval;
        }
    }
}