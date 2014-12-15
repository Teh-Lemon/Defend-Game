/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
////
//// Three kinds of generic object pools to avoid memory deallocations
//// in Unity-based games. See my Gamasutra articles.
//// Released under a Creative Commons Attribution (CC BY) License,
//// see http://creativecommons.org/licenses/
////
//// (c) 2013 Wendelin Reich.
////
/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
// Edited by Teh Lemon on 15/12/2014

using System;
using System.Collections.Generic;
// Custom
using UnityEngine;

namespace MemoryManagment
{
    #region Custom
    // Used to hold gameobjects which are to be instantiated
    // Require user to reset each gameobject manually
    public class GameObjectPool
    {
        Stack<GameObject> m_objectStack;
        GameObject m_prefab;
        GameObject m_parent;

        // Constructor
        public GameObjectPool(int initialBufferSize, GameObject preFab, GameObject parent = null
            , bool lazyInstance = false)
        {
            m_objectStack = new Stack<GameObject>(initialBufferSize);
            m_prefab = preFab;
            m_parent = parent;

            // Pre-instantiate all the gameobjects unless stated otherwise
            if (!lazyInstance)
            {
                for (int i = 0; i < initialBufferSize; i++)
                {
                    GameObject newObj = SpawnGameObject();
                    m_objectStack.Push(newObj);
                }
            }
        }

        // Return an inactive gameobject for use
        // Creates a new one if none are available
        public GameObject New()
        {
            GameObject newObj;

            // Retrieve an unused gameobject from the stack
            if (m_objectStack.Count > 0)
            {
                newObj = m_objectStack.Pop();
            }
            // Create a new one if the stack is empty
            else
            {
                newObj = SpawnGameObject();
            }

            return newObj;
        }

        // Store the newObj for later re-use
        public void Store(GameObject newObj)
        {
            newObj.SetActive(false);
            m_objectStack.Push(newObj);
        }

        // Instantiate and set up a new gameobject
        GameObject SpawnGameObject()
        {
            GameObject newObj = GameObject.Instantiate(m_prefab) as GameObject;

            // Parent this gameobject if a parent is provided
            if (m_parent != null)
            {
                newObj.transform.parent = m_parent.transform;
            }

            newObj.SetActive(false);

            return newObj;
        }
    }
    #endregion

    #region Old
    public class ObjectPool<T> where T : class, new()
    {
        private Stack<T> m_objectStack;

        private Action<T> m_resetAction;
        private Action<T> m_onetimeInitAction;

        public ObjectPool(int initialBufferSize, Action<T> ResetAction = null, Action<T> OnetimeInitAction = null)
        {
            m_objectStack = new Stack<T>(initialBufferSize);
            m_resetAction = ResetAction;
            m_onetimeInitAction = OnetimeInitAction;
        }

        public T New()
        {
            if (m_objectStack.Count > 0)
            {
                T t = m_objectStack.Pop();

                if (m_resetAction != null)
                    m_resetAction(t);

                return t;
            }
            else
            {
                T t = new T();

                if (m_onetimeInitAction != null)
                    m_onetimeInitAction(t);

                return t;
            }
        }

        public void Store(T obj)
        {
            m_objectStack.Push(obj);
        }
    }

    public interface IResetable
    {
        void Reset();
    }

    public class ObjectPoolWithReset<T> where T : class, IResetable, new()
    {
        private Stack<T> m_objectStack;

        private Action<T> m_resetAction;
        private Action<T> m_onetimeInitAction;

        public ObjectPoolWithReset(int initialBufferSize, Action<T> ResetAction = null
            , Action<T> OnetimeInitAction = null)
        {
            m_objectStack = new Stack<T>(initialBufferSize);
            m_resetAction = ResetAction;
            m_onetimeInitAction = OnetimeInitAction;
        }

        public T New()
        {
            if (m_objectStack.Count > 0)
            {
                T t = m_objectStack.Pop();

                t.Reset();

                if (m_resetAction != null)
                    m_resetAction(t);

                return t;
            }
            else
            {
                T t = new T();

                if (m_onetimeInitAction != null)
                    m_onetimeInitAction(t);

                return t;
            }
        }

        public void Store(T obj)
        {
            m_objectStack.Push(obj);
        }
    }

    public class ObjectPoolWithCollectiveReset<T> where T : class, new()
    {
        private List<T> m_objectList;
        private int m_nextAvailableIndex = 0;

        private Action<T> m_resetAction;
        private Action<T> m_onetimeInitAction;

        public ObjectPoolWithCollectiveReset(int initialBufferSize, Action<T> ResetAction = null
            , Action<T> OnetimeInitAction = null)
        {
            m_objectList = new List<T>(initialBufferSize);
            m_resetAction = ResetAction;
            m_onetimeInitAction = OnetimeInitAction;
        }

        public T New()
        {
            if (m_nextAvailableIndex < m_objectList.Count)
            {
                // an allocated object is already available; just reset it
                T t = m_objectList[m_nextAvailableIndex];
                m_nextAvailableIndex++;

                if (m_resetAction != null)
                    m_resetAction(t);

                return t;
            }
            else
            {
                // no allocated object is available; create a new one and grow the internal object list
                T t = new T();
                m_objectList.Add(t);
                m_nextAvailableIndex++;

                if (m_onetimeInitAction != null)
                    m_onetimeInitAction(t);

                return t;
            }
        }

        public void ResetAll()
        {
            m_nextAvailableIndex = 0;
        }
    }
#endregion
}