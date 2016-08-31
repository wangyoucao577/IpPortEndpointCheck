using System;
using System.Collections.Generic;
using System.Text;

namespace Observer
{
    class Subject
    {
        protected List<IObserver> m_objectList = new List<IObserver>();

        public void Attach(IObserver obj)
        {
            m_objectList.Add(obj);
        }

        public void Detach(IObserver obj)
        {
            m_objectList.Remove(obj);
        }

        public void DetachAll()
        {
            m_objectList.Clear();
        }

        public virtual void Notify()
        {
            foreach (IObserver item in m_objectList)
            {
                item.Update(this);
            }
        }
    }
}
