using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher
{
    public class InventoryData
    {
        public int _MaxSlots = 20;
        public List<ItemSlotData> _ItemSlots = new List<ItemSlotData>();

        private readonly Actor _Actor;


        public void AddStack(ItemSlotData addItemSlot)
        {
            if (!RegisterManager._Instance._RegisteredItems._ItemObjects.ContainsKey(addItemSlot._ItemID)) return;

            //Fill existing item slots
            ItemData registeredItem = RegisterManager._Instance._RegisteredItems._ItemObjects[addItemSlot._ItemID];
            for (int i = 0; i < _ItemSlots.Count; i++)
            {
                ItemSlotData itemSlot = _ItemSlots[i];
                if (itemSlot._ItemID == addItemSlot._ItemID)
                {
                    CombineStack(addItemSlot, itemSlot, registeredItem._MaxStack);
                    if (addItemSlot._Stack <= 0) return;
                }
            }

            //Create new item slots
            AddStack(addItemSlot, registeredItem._MaxStack);
        }

        public void MoveStack(int fromSlotIndex, int toSlotIndex)
        {
            ItemSlotData itemSlotFrom = _ItemSlots[fromSlotIndex];
            ItemSlotData itemSlotTo = _ItemSlots[toSlotIndex];
            if (itemSlotFrom._ItemID == itemSlotTo._ItemID)
            {
                if (!RegisterManager._Instance._RegisteredItems._ItemObjects.ContainsKey(itemSlotFrom._ItemID)) return;

                ItemData registeredItem = RegisterManager._Instance._RegisteredItems._ItemObjects[itemSlotFrom._ItemID];
                if (itemSlotTo._Stack < registeredItem._MaxStack)
                {
                    CombineStack(itemSlotFrom, itemSlotTo, registeredItem._MaxStack);
                    if (itemSlotFrom._Stack <= 0) _ItemSlots.RemoveAt(fromSlotIndex);
                    return;
                }
            }

            _ItemSlots[toSlotIndex] = itemSlotFrom;
            _ItemSlots[fromSlotIndex] = itemSlotTo;
        }

        public void DropStack(int slotIndex)
        {
            ItemSlotData itemSlot = _ItemSlots[slotIndex];
            if (!RegisterManager._Instance._RegisteredItems._ItemObjects.ContainsKey(itemSlot._ItemID)) return;

            ItemData registeredItem = RegisterManager._Instance._RegisteredItems._ItemObjects[itemSlot._ItemID];
            Item itemObject = GameObject.Instantiate(registeredItem._ItemEntity, _Actor.transform.position, _Actor.transform.rotation); //TODO: Change positon to be randomly around the player with random rotaion
            itemObject._Stack = itemSlot._Stack;
            _ItemSlots.RemoveAt(slotIndex);
        }

        void AddStack(ItemSlotData addItemSlot, int maxStack)
        {
            if (_ItemSlots.Count >= _MaxSlots) return;

            if (addItemSlot._Stack > maxStack)
            {
                _ItemSlots.Add(new ItemSlotData(addItemSlot._ItemID, maxStack));
                addItemSlot._Stack -= maxStack;
                AddStack(addItemSlot, maxStack);
            }
            else
            {
                _ItemSlots.Add(new ItemSlotData(addItemSlot._ItemID, addItemSlot._Stack));
                addItemSlot._Stack = 0;
            }
        }

        void CombineStack(ItemSlotData itemSlotFrom, ItemSlotData itemSlotTo, int maxStack)
        {
            int stackRemainder = Mathf.Clamp(maxStack - itemSlotTo._Stack, 0, maxStack);
            if (itemSlotFrom._Stack > stackRemainder)
            {
                itemSlotTo._Stack += stackRemainder;
                itemSlotFrom._Stack -= stackRemainder;
            }
            else
            {
                itemSlotTo._Stack += itemSlotFrom._Stack;
                itemSlotFrom._Stack = 0;
            }
        }
    }
}
