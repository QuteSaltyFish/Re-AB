using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeakness : MonoBehaviour {

    [SerializeField]
    private BoxingManager boxing_manager;
    private string m_name;
    // JabLeftWeakness, JabRightWeakness, BodyWeakness


    private void Start()
    {
        m_name = this.gameObject.name;
        Debug.Log(m_name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Glove") return;
        Debug.Log("HIT!");
        boxing_manager.Enemy_got_hit(m_name);
    }
}
