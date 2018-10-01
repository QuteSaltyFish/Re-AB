using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour {

    public bool is_dead = false;
    public bool is_walking = false;
    private CharacterController m_characterController;
    private Animation m_animation;

    private void Awake(){
        m_characterController = GetComponent<CharacterController>();
        m_animation = GetComponent<Animation>();
    }

    private void Update(){
        if(!is_dead){
            if(is_walking){
                if(m_characterController.velocity.z < 0){
                    m_animation.Play("move_backward");
                }else if(m_characterController.velocity.z > 0){
                    m_animation.Play("move_forward");
                }
            }else{
                m_animation.Play("idle");
            }
        }else{
            m_animation.Play("dead");
            enabled = false;
        }
    }
}
