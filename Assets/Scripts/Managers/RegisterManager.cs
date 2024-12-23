using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    public class RegisterManager : Singleton<RegisterManager>
    {
        public ItemObject _RegisteredItems;

        #region Domain GC
        /// <summary>
        /// Clears data that presist after runtime ends.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void DomainClear()
        {
            DomainReset();
        }
        #endregion
    }
}
