#define ENABLED_FIXEDUPDATE_FUNTION_CALLBACK
#define ENABLED_LATEUPDATE_FUNTION_CALLBACK
#define ENABLED_UPDATE_FUNTION_CALLBACK


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class GameThreadHandler : MonoBehaviour
    {
        delegate void Test(params object[] args);

        private static GameThreadHandler _instance;
        public static GameThreadHandler Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<GameThreadHandler>();
                if (_instance == null)
                {
                    var go = new GameObject("UnityThread");
                    //go.hideFlags = HideFlags.HideAndDontSave;
                    _instance = go.AddComponent<GameThreadHandler>();
                }
                return _instance;
            }
        }



#if ENABLED_UPDATE_FUNTION_CALLBACK
        private static List<System.Action> actionQueueUpdateFunc = new List<System.Action>();

        private List<System.Action> actionCopiedQueueUpdateFunc = new List<System.Action>();

        private volatile static bool noActionQueueToExecuteUpdateFunc = true;
#endif
#if ENABLED_LATEUPDATE_FUNTION_CALLBACK
        private static List<System.Action> actionQueueLateUpdateFunc = new List<System.Action>();

        private List<System.Action> actionCopiedQueueLateUpdateFunc = new List<System.Action>();

        private volatile static bool noActionQueueToExecuteLateUpdateFunc = true;

#endif
#if ENABLED_FIXEDUPDATE_FUNTION_CALLBACK
        private static List<System.Action> actionQueueFixedUpdateFunc = new List<System.Action>();

        private List<System.Action> actionCopiedQueueFixedUpdateFunc = new List<System.Action>();

        private volatile static bool noActionQueueToExecuteFixedUpdateFunc = true;
#endif

        private System.Action<float> Events;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            InvokeRepeating(nameof(UpdatePeriodic), 0, 1);
        }

        public static void Schedule(System.Action action, float delay)
        {
            _instance.StartCoroutine(_instance.RunScheduleAsync(action, delay));
        }

        private IEnumerator RunScheduleAsync(System.Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }


        public static void AddPeriodicUpdate(System.Action<float> action)
        {
            Instance.Events += action;
        }

        private void UpdatePeriodic()
        {
            Events?.Invoke(Time.deltaTime);
        }

#if ENABLED_UPDATE_FUNTION_CALLBACK
        public static void ExecuteInUpdate(System.Action action)
        {
            if (action == null)
                throw new System.ArgumentNullException(nameof(action));

            lock (actionQueueUpdateFunc)
            {
                actionQueueUpdateFunc.Add(action);
                noActionQueueToExecuteUpdateFunc = false;
            }
        }

        internal static void NetworkUpdate()
        {
            
            if (noActionQueueToExecuteUpdateFunc)
                return;

            Instance.actionCopiedQueueUpdateFunc.Clear();
            lock (actionQueueUpdateFunc)
            {
                Instance.actionCopiedQueueUpdateFunc.AddRange(actionQueueUpdateFunc);
                actionQueueUpdateFunc.Clear();
                noActionQueueToExecuteUpdateFunc = true;
            }
            int count = Instance.actionCopiedQueueUpdateFunc.Count;

            for (int i = 0; i < count; i++)
            {
                Instance.actionCopiedQueueUpdateFunc[i].Invoke();
            }
        }
#endif

#if ENABLED_LATEUPDATE_FUNTION_CALLBACK
        public static void ExecuteInLateUpdate(System.Action action)
        {
            if (action == null)
                throw new System.ArgumentNullException(nameof(action));

            lock (actionQueueLateUpdateFunc)
            {
                actionQueueLateUpdateFunc.Add(action);
                noActionQueueToExecuteLateUpdateFunc = false;
            }
        }

        internal void NetworkLateUpdate()
        {
            if (noActionQueueToExecuteLateUpdateFunc)
                return;

            actionCopiedQueueLateUpdateFunc.Clear();
            lock (actionQueueLateUpdateFunc)
            {
                actionCopiedQueueLateUpdateFunc.AddRange(actionQueueLateUpdateFunc);
                actionQueueLateUpdateFunc.Clear();
                noActionQueueToExecuteLateUpdateFunc = true;
            }
            int count = actionCopiedQueueLateUpdateFunc.Count;
            for (int i = 0; i < count; i++)
            {
                actionCopiedQueueLateUpdateFunc[i].Invoke();
            }
        }
#endif

#if ENABLED_FIXEDUPDATE_FUNTION_CALLBACK

        public static void ExecuteInFixedUpdate(System.Action action)
        {
            if (action == null)
                throw new System.ArgumentNullException(nameof(action));

            lock (actionQueueFixedUpdateFunc)
            {
                actionQueueFixedUpdateFunc.Add(action);
                noActionQueueToExecuteFixedUpdateFunc = false;
            }
        }

        internal void NetworkFixedUpdate()
        {
            if (noActionQueueToExecuteFixedUpdateFunc)
                return;

            actionCopiedQueueFixedUpdateFunc.Clear();
            lock (actionQueueFixedUpdateFunc)
            {
                actionCopiedQueueFixedUpdateFunc.AddRange(actionQueueFixedUpdateFunc);
                actionQueueFixedUpdateFunc.Clear();
                noActionQueueToExecuteFixedUpdateFunc = true;
            }
            int count = actionCopiedQueueFixedUpdateFunc.Count;
            for (int i = 0; i < count; i++)
            {
                actionCopiedQueueFixedUpdateFunc[i].Invoke();
            }
        }
#endif
        private void OnDisable()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }

}

