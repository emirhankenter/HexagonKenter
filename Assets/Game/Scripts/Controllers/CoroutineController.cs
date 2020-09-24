using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mek.Controllers
{
    public class CoroutineController : MonoBehaviour
    {
        private static Dictionary<string, IEnumerator> _coroutineDictionary = new Dictionary<string, IEnumerator>();

        private const string DoAfterGivenTimeKey = "DoAfterGivenTimeRoutine";

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static void StartCoroutine(string key, IEnumerator coroutine)
        {
            Instance.StartMyCoroutine(key, coroutine);
        }

        public static bool IsCoroutineRunning(string key)
        {
            return _coroutineDictionary.ContainsKey(key);
        }

        public static void StopThisCoroutine(string key)
        {
            if (_coroutineDictionary.TryGetValue(key, out IEnumerator value))
            {
                Instance.StopCoroutine(value);
                _coroutineDictionary.Remove(key);
            }
        }

        private Coroutine StartMyCoroutine(string key, IEnumerator coroutine)
        {
            return StartCoroutine(GenericCoroutine(key, coroutine));
        }

        private IEnumerator GenericCoroutine(string key, IEnumerator coroutine)
        {
            _coroutineDictionary.Add(key, coroutine);
            yield return StartCoroutine(coroutine);
            _coroutineDictionary.Remove(key);
        }

        #region Helpers

        public static void DoAfterGivenTime(float time, Action onComplete)
        {
            Instance.DoAfterTime(time, onComplete);
        }

        public static void DoAfterFixedUpdate(Action onComplete)
        {
            StartCoroutine("WaitFixedUpdate", FixedUpdateRoutine());

            IEnumerator FixedUpdateRoutine()
            {
                yield return new WaitForFixedUpdate();
                onComplete?.Invoke();
            }
        }

        private void DoAfterTime(float time, Action onComplete)
        {
            StartCoroutine(TimeRoutine(time, () => { onComplete?.Invoke(); }));
        }

        private IEnumerator TimeRoutine(float timer, Action onComplete)
        {
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            onComplete?.Invoke();
        }

        #endregion


        #region

        private static CoroutineController _instance;

        public static CoroutineController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CoroutineController>();

                    if (_instance == null)
                    {
                        Debug.LogError($"Couldn't find {nameof(CoroutineController)} instance.");
                    }
                }
                return _instance;
            }
        }

        #endregion
    }
}
