using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace ELS.Manager
{
    abstract class Manager
    {
        protected List<PoolObject> Entities = new List<PoolObject>();
        internal Manager()
        {
            
        }
        /// <summary>
        /// Runs garbage collection.
        /// </summary>
        protected void RunGC()
        {
            Entities.RemoveAll(o => !o.Exists());
        }
        
        //TODO finish below
        void CleanUP()
        {
            Entities.RemoveAll(o => !o.Exists());
        }
        protected bool AddIfNotPresint(PoolObject o)
        {
            if (!Entities.Exists(poolObject => poolObject.Handle == o.Handle))
            {
                Entities.Add(o);
                return false;
            }
            return true;
        }
        internal abstract void RunTick();
    }
}
