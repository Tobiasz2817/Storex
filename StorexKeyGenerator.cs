using UnityEngine;

namespace Storex {
    public class StorexKeyGenerator : MonoBehaviour {
        void Awake() {
            var keys = StorexEncrypter.GenerateKeyAndIv();
            Debug.Log("Key: " + keys.Item1);
            Debug.Log("Iv: " + keys.Item2);
        }
    }
}