using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public enum AIStateType
    {
        None = 0,
        Patrol = 1,
        Combat = 2,
        Abort = 3,
    }
    public abstract class AIStateBase : MonoBehaviour
    {
        public abstract AIStateType StateType { get; }
        public abstract void onEnterState(AIBrain brain);
        public abstract void onExitState(AIBrain brain);
        public abstract void onUpdateState(AIBrain brain);
    }
}
