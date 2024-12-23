using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher.UI
{
    /// <summary>
    /// UI link element
    /// </summary>
    public class UILink : MonoBehaviour
    {
        /// <summary>
        /// Open link
        /// </summary>
        public void OpenLink(string URL)
        {
            Application.OpenURL(URL);
        }
    }
}
