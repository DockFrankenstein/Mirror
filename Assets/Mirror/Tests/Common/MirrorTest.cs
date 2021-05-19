// base class for networking tests to make things easier.
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Mirror.Tests
{
    public abstract class MirrorTest
    {
        // keep track of networked GameObjects so we don't have to clean them
        // up manually each time.
        // CreateNetworked() adds to the list automatically.
        public List<GameObject> instantiated;

        // we usually need the memory transport
        public MemoryTransport transport;

        [SetUp]
        public virtual void SetUp()
        {
            instantiated = new List<GameObject>();

            // need a transport to send & receive
            Transport.activeTransport = transport = new GameObject().AddComponent<MemoryTransport>();
        }

        [TearDown]
        public virtual void TearDown()
        {
            foreach (GameObject go in instantiated)
                if (go != null)
                    GameObject.DestroyImmediate(go);

            GameObject.DestroyImmediate(transport.gameObject);
            Transport.activeTransport = null;
        }

        // create GameObject + NetworkIdentity
        // add to tracker list if needed (useful for cleanups afterwards)
        protected void CreateNetworked(out GameObject go, out NetworkIdentity identity)
        {
            go = new GameObject();
            identity = go.AddComponent<NetworkIdentity>();
            instantiated.Add(go);
        }

        // create GameObject + NetworkIdentity + NetworkBehaviour<T>
        // add to tracker list if needed (useful for cleanups afterwards)
        protected void CreateNetworked<T>(out GameObject go, out NetworkIdentity identity, out T component)
            where T : NetworkBehaviour
        {
            go = new GameObject();
            identity = go.AddComponent<NetworkIdentity>();
            component = go.AddComponent<T>();
            instantiated.Add(go);
        }
    }
}