using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    /*
     * Audio Missing!
     * What is levelManager?
     * transform.rotation.x.z = rotInitial
     */

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private EnemyAnimation enemy_animation;
    private CharacterController m_characterController;

    private enum enemy_state { offensive, defensive, idle };
    private enum hit_type { jab, cross, uppercutleft, uppercutright };


    private float life = 100f;
    private float move_speed = 2.3f;
    private float rot_speed = 5f;
    private enemy_state current_state;

    [SerializeField]
    private float damage = 5f;

    private bool can_attack = true;
    [SerializeField]
    private float attack_rate = 1f;
    private float attack_timer;

    private bool can_change_state = true;
    [SerializeField]
    private float change_state_rate = 1f;
    private float change_state_timer;

    private bool can_change_action = true;
    private float change_action_rate = 1f;
    private float change_action_timer;

    private bool attack;            //Random from 1 ~ 10 to decide attack
    private bool is_covered;        //or defense or idle

    private hit_type current_hit_type;
    private bool is_dead;
    private float damage_caused;

    private Vector3 move_direction = Vector3.zero;

    private void Awake()
    {
        m_characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if(!player){
            Debug.Log("Set the player in the hierarchy!");
        }
        current_state = enemy_state.idle;
    }

    private void Update()
    {
        if(!is_dead){
            if(player){
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(player.transform.position - transform.position), rot_speed * Time.deltaTime);

            }

            MakeDecisions();
            switch (current_state) {
                case enemy_state.idle:{
                    break;
                }
                case enemy_state.offensive:{
                    Offensive();
                    break;
                }
                case enemy_state.defensive:{
                    Defensive();
                    break;
                }
            }
        }
    }

    private void MakeDecisions()
    {
        if (change_state_timer > 0)
        {
            can_change_state = false;
            change_state_timer -= Time.deltaTime;
        }
        else
        {
            change_state_timer = 0;
            can_change_state = true;
        }

        if (change_action_timer > 0)
        {
            can_change_action = false;
            change_action_timer -= Time.deltaTime;
        }
        else
        {
            change_action_timer = 0;
            can_change_action = true;
        }

        if (attack_timer > 0)
        {
            attack_timer -= Time.deltaTime;
            can_attack = false;
        }
        else
        {
            attack_timer = 0;
            can_attack = true;
        }

        if (can_change_state) ChangeState();
        if (can_change_action) ChangeAction();
        if (attack) AttackOpponent();
}

    private void ChangeState(){
        int random_state = Random.Range(1, 4);
        if (random_state == 1)
        {
            current_state = enemy_state.idle;
            enemy_animation.is_walking = false;
            change_state_timer += change_state_rate;
        }
        else if (random_state == 2)
        {
            current_state = enemy_state.defensive;
            enemy_animation.is_walking = true;
            change_state_timer += change_state_rate / 2;
        }
        else
        {
            current_state = enemy_state.offensive;
            enemy_animation.is_walking = true;
            change_state_timer += change_state_rate;
        }
    }

    private void ChangeAction(){
        int random_action = Random.Range(1, 11);
        if(random_action < 6){          //Attack
            attack = true;
            is_covered = false;
        }else if(random_action < 9){    //defense
            is_covered = true;
            change_action_timer += change_action_rate;
        }else{                          //idle
            attack = false;
            is_covered = false;
            change_action_timer += change_action_rate;
        }
    }

    private void AttackOpponent(){
        if(can_attack){
            float dis_opponent = Vector3.Distance(transform.position, player.transform.position);
            if(dis_opponent < 3){
                Attack();
                attack_timer = attack_rate;
            }
        }
    }

    private void Offensive(){
        move_direction = Vector3.forward;
        move_direction = transform.TransformDirection(move_direction);
        move_direction *= move_speed * Time.deltaTime;
        m_characterController.Move(move_direction);
    }

    private void Defensive(){
        move_direction = Vector3.back;
        move_direction = transform.TransformDirection(move_direction);
        move_direction *= move_speed * Time.deltaTime;
        m_characterController.Move(move_direction);
    }

    private void Attack(){
        int random_attack = Random.Range(1, 5);

        switch (random_attack){
            case 1:
                current_hit_type = hit_type.jab;
                break;
            case 2:
                current_hit_type = hit_type.cross;
                break;
            case 3:
                current_hit_type = hit_type.uppercutleft;
                break;
            case 4:
                current_hit_type = hit_type.uppercutright;
                break;
        }

        //anim.hit(hit_type);

        attack_timer += attack_rate;

        //attack.judge
    }

    public void LoseLife(float damage){

        float damage_result;

        if(is_covered){
            damage_result = damage / 5;
        }else{
            damage_result = damage;
        }

        life -= damage_result;

        if (life <= 0){
            life = 0;
            is_dead = true;
        }
    }
}
