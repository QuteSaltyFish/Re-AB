using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour {

    public bool is_dead = false;
    public int move_dir = 0;
    private CharacterController m_characterController;
    private Animator m_animator;

    private int speed_v_ID;
    private int jableft_trig_ID;
    private int jabright_trig_ID;
    private int uppercutleft_trig_ID;
    private int uppercutright_trig_ID;
    private int jableft_attacked_trig_ID;
    private int jabright_attacked_trig_ID;
    private int dead_trig_ID;
    private int block_trig_ID;

    private void Awake(){
        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        speed_v_ID = Animator.StringToHash("speed_v");
        jableft_trig_ID = Animator.StringToHash("jableft_trig");
        jabright_trig_ID = Animator.StringToHash("jabright_trig");
        uppercutleft_trig_ID = Animator.StringToHash("uppercutleft_trig");
        uppercutright_trig_ID = Animator.StringToHash("uppercutright_trig");
        jableft_attacked_trig_ID = Animator.StringToHash("jableft_attacked_trig");
        jabright_attacked_trig_ID = Animator.StringToHash("jabright_attacked_trig");
        dead_trig_ID = Animator.StringToHash("dead_trig");
        block_trig_ID = Animator.StringToHash("block_trig");
    }

    private void Update(){
        if(is_dead) enabled = false;
    }

    public void Movement(){
        if (move_dir == -1){
            m_animator.SetFloat(speed_v_ID, -1);
        }else if (move_dir == 1){
            m_animator.SetFloat(speed_v_ID, 1);
        }else {
            m_animator.SetFloat(speed_v_ID, 0);
        }
    }

    public void Hit(string current_hit_type){
        switch (current_hit_type){
            case "jableft":{
                    m_animator.SetTrigger(jableft_trig_ID);
                    break;
                }
            case "jabright":{
                    m_animator.SetTrigger(jabright_trig_ID);
                    break;
                }
            case "uppercutleft":{
                    m_animator.SetTrigger(uppercutleft_trig_ID);
                    break;
                }
            case "uppercutright":{
                    m_animator.SetTrigger(uppercutright_trig_ID);
                    break;
                }
        }
    }

    public void GotHit(string got_hit_type)
    {
        switch (got_hit_type){
            case "jableft":{
                    m_animator.SetTrigger(jableft_attacked_trig_ID);
                    break;
                }
            case "jabright":
                {
                    m_animator.SetTrigger(jabright_attacked_trig_ID);
                    break;
                }
        }
    }
}
