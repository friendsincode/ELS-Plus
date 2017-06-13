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
        private List<PoolObject> Entities = new List<PoolObject>();
        internal Manager()
        {
            
        }
        /// <summary>
        /// Runs garbage collection.
        /// </summary>
        void RunGC()
        {
            Entities.RemoveAll(o => !o.Exists());
        }
        
        //TODO finish below
        void CleanUP()
        {
            Entities.RemoveAll(o => !o.Exists());
        }
        protected void AddIfNotPresint(PoolObject o)
        {
            if (!Entities.Exists(poolObject => poolObject.Handle == o.Handle))
            {
                Entities.Add(o);
            }
        }
        internal abstract void RunTick();
    }
}
