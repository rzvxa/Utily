using System;
using System.Collections;
using UnityEngine;

namespace Utils.Unity.Runtime
{
    public class CoExecute : Singleton<CoExecute>
    {
        public static void Execute(Func<IEnumerator> routine)
        {
            CanExecuteTest();
            Instance.StartCoroutine(routine());
        }

        private static void CanExecuteTest()
        {
            if (!Application.isPlaying)
                throw new Exception("Can't run Coroutine in game.");
        }
    }
}
