using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    /*
     * Audio Missing!
     * What is levelManager?
     */

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private EnemyAnimation enemy_animation;
    private Animator m_animator;
    private CharacterController m_characterController;

    private enum enemy_state { offensive, idle };

    private float life = 100f;
    private float move_speed = 2.3f;
    private float rot_speed = 5f;
    private enemy_state current_state = enemy_state.idle;

    [SerializeField]
    private float damage = 5f;

    [SerializeField]
    private float attack_distance_min = 3f;
    [SerializeField]
    private float move_distance_min = 2f;

    private bool can_attack = true;
    [SerializeField]
    private float attack_rate = 4f;
    private float attack_timer;

    private bool can_change_state = true;
    [SerializeField]
    private float change_state_rate = 10f;
    private float change_state_timer;

    private bool can_change_action = true;
    [SerializeField]
    private float change_action_rate = 1f;
    private float change_action_timer;

    private bool attack;
    public bool is_blocking = false;

    private string current_hit_type;
    private bool is_dead;
    private float damage_caused;

    private Vector3 move_direction = Vector3.zero;

    private void Awake()
    {
        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if(!player){
            Debug.Log("Set the player in the hierarchy!");
        }
    }

    private void FixedUpdate()
    {
        if(!is_dead){
            if(player){
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(player.transform.position - transform.position), rot_speed * Time.deltaTime);
                transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
            }

            if (change_state_timer > 0){
                can_change_state = false;
                change_state_timer -= Time.deltaTime;
            }else{
                change_state_timer = 0;
                can_change_state = true;
            }

            if (attack_timer > 0){
                attack_timer -= Time.deltaTime;
                can_attack = false;
            }else{
                attack_timer = 0;
                can_attack = true;
            }

            MakeDecisions();

            switch (current_state) {
                case enemy_state.idle:{
                    Idle();
                    break;
                }
                case enemy_state.offensive:{
                    Offensive();
                    break;
                }
            }
        }
    }

    private void MakeDecisions(){
        if (can_change_state) ChangeState();
        if (can_attack) AttackOpponent();
}

    private void ChangeState(){
        int random_state = Random.Range(1, 5);
        switch(random_state){
            case 1:{
                    current_state = enemy_state.idle;
                    break;
                }
            case 2: case 3: case 4:{
                    current_state = enemy_state.offensive;
                    break;
                }
        }
        change_state_timer = change_state_rate;
    }

    private void AttackOpponent(){
        if(can_attack){
            float dis_opponent = Vector3.Distance(transform.position, player.transform.position);
            if(dis_opponent < attack_distance_min){
                Attack();
            }
        }
    }

    private void Idle(){
        move_direction = Vector3.zero;
        m_characterController.Move(move_direction * Time.deltaTime);
        m_animator.SetFloat("speed_v", 0.0f);
    }

    private void Offensive(){
        float dis_opponent = Vector3.Distance(transform.position, player.transform.position);
        if(dis_opponent > move_distance_min){
            move_direction = Vector3.forward;
            move_direction = transform.TransformDirection(move_direction);
            move_direction *= move_speed;
            m_characterController.Move(move_direction * Time.deltaTime);
            m_animator.SetFloat("speed_v", 1.0f);
        }else{
            move_direction = Vector3.zero;
            m_characterController.Move(move_direction * Time.deltaTime);
            m_animator.SetFloat("speed_v", 0.0f);
        }
    }

    private void Attack(){
        int random_attack = Random.Range(1, 5);

        switch (random_attack){
            case 1:
                current_hit_type = "jableft";
                break;
            case 2:
                current_hit_type = "jabright";
                break;
            case 3:
                current_hit_type = "uppercutleft";
                break;
            case 4:
                current_hit_type = "uppercutright";
                break;
        }

        enemy_animation.Hit(current_hit_type);
        attack_timer = attack_rate;
        //attack.judge

        Block();
    }

    private void Block(){
        int random_block = Random.Range(1, 11);
        if(random_block < 4){
            is_blocking = true;
            m_animator.SetBool("block", true);
            StartCoroutine(BlockOff());

            //block.judge
        }
    }

    IEnumerator BlockOff(){
        yield return new WaitForSeconds(attack_rate - 0.5f);
        is_blocking = false;
        m_animator.SetBool("block", false);
    }

    public void LoseLife(float damage){

        float damage_result;

        if(is_blocking){
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
