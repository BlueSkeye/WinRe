using System;
using System.Xml.Serialization;

namespace UpdMngr.Data
{
    public abstract class BasePersistenceDocument
    {
        [XmlIgnore()]
        internal XmlBasedPersistenceProvider PersistanceHandler
        {
            get { return _persistanceHandler; }
            set
            {
                if (null == value) { throw new ArgumentNullException(); }
                if (null != _persistanceHandler) {
                    throw new InvalidOperationException();
                }
                _persistanceHandler = value;
            }
        }

        private XmlBasedPersistenceProvider _persistanceHandler;
    }
}
