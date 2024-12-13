using UnityEngine;
using System;

namespace Storex {
    public class StorexExample : MonoBehaviour {
        public int SpaceValue;
        public bool IsValuet;
        public string SpaceName;
        public float SpaceSpeed;
        public OperationType Type;

        public int x;
        public int y;
        public int z;
        public bool over;

        public string fileName;
        
        void Update() {
            if (Input.GetKeyUp(KeyCode.Q)) 
                Debug.Log(StorexVault.Load<Hello>(fileName));
            if(Input.GetKeyUp(KeyCode.W))
                Debug.Log(StorexVault.Load<Position>(fileName));
            if (Input.GetKeyUp(KeyCode.E)) {
                var ids = StorexVault.Load<Ids>(fileName);
                foreach (var f in ids.x) 
                    Debug.Log(f);
            }
            
            if(Input.GetKeyUp(KeyCode.A))
                StorexVault.Save(new Hello() {k = "Helllo"}, fileName);
            if(Input.GetKeyUp(KeyCode.S))
                StorexVault.Save(new Position(), fileName);
            if(Input.GetKeyUp(KeyCode.D))
                StorexVault.Save(new Ids(){ x = new float[]{x,y,z,5,6}}, fileName);
        }
        
        [Serializable]
        public struct Hello {
            public float x;
            public float d;
            public float t;
            public string k;
        }
        
        [Serializable]
        public struct Ids {
            public float[] x;
        }
        
        [Serializable]
        public class Position {
            public float x = 5;
            public float y = 2;
            public float z = 1;
        }
    }
}