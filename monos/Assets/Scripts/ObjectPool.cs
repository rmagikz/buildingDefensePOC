using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject type;
    [SerializeField] int amount;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start() {GeneratePool();}

    public GameObject Spawn() {
        if (pool.TryDequeue(out GameObject instance)) {
            instance.SetActive(true);
            return instance;
        } else {
            instance = Instantiate(type, gameObject.transform);
            return instance;
        }
    }

    public void Remove(GameObject instance) {
        instance.SetActive(false);
        pool.Enqueue(instance);
    }

    public void GeneratePool() {
        while (pool.TryDequeue(out GameObject instance)) {
            Destroy(instance);
        }
        for (int i = 0; i < amount; i++) {
            CreateInstance();
        }
    }

    private void CreateInstance() {
        GameObject instance = Instantiate(type, gameObject.transform);
        instance.GetComponent<Enemy>().parentPool = this;
        instance.SetActive(false);
        pool.Enqueue(instance);
    }
}
