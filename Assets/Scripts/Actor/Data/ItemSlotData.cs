using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    public class ItemSlotData
    {
        public string _ItemID;
        public int _Stack;

        public ItemSlotData(string itemID, int stack)
        {
            _ItemID = itemID;
            _Stack = stack;
        }
    }
}
