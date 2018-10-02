using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingManager : MonoBehaviour {

    public int player_health_start = 100;
    [SerializeField]
    private int player_health;

    public int enemy_health_start = 100;
    [SerializeField]
    private int enemy_health;

    [SerializeField]
    private EnemyAI enemy_AI;

    [SerializeField]
    private GameObject gloveL_higher_checker;
    [SerializeField]
    private GameObject gloveL_lower_checker;
    [SerializeField]
    private GameObject gloveR_higher_checker;
    [SerializeField]
    private GameObject gloveR_lower_checker;
    [SerializeField]
    private float glove_checker_max_dist = 0.3f;

    [SerializeField]
    private int damage_to_player = 15;
    [SerializeField]
    private int damage_to_player_blocking = 5;

    [SerializeField]
    private int damage_to_enemy = 15;
    [SerializeField]
    private int damage_to_enemy_blocking = 5;

    private bool enemy_can_get_hit = true;
    [Header("Blank between 2 hits")]
    [SerializeField]
    private float enemy_hit_rate = 1f;
    private float enemy_hit_timer;

    private void Start()
    {
        player_health = player_health_start;
        enemy_health = enemy_health_start;
    }

    private void Update()
    {
        if (enemy_hit_timer > 0){
            enemy_can_get_hit = false;
            enemy_hit_timer -= Time.deltaTime;
        }else{
            enemy_hit_timer = 0;
            enemy_can_get_hit = true;
        }

        Debug.Log(gloveL_higher_checker.transform.position);
        Debug.Log(gloveL_lower_checker.transform.position);
    }


    public void Player_got_hit(){
        bool player_is_blocking = false;
        //Block Judge
        float delta_left_checkers = Mathf.Abs(gloveL_higher_checker.transform.position.y - gloveL_lower_checker.transform.position.y);
        float delta_right_checkers = Mathf.Abs(gloveR_higher_checker.transform.position.y - gloveR_lower_checker.transform.position.y);
        if(delta_left_checkers <= glove_checker_max_dist && delta_right_checkers <= glove_checker_max_dist){
            player_is_blocking = true;
        }
        CalcPlayerDamage(player_is_blocking);
    }

    private void CalcPlayerDamage(bool is_blocking){
        if(is_blocking){
            Debug.Log("Player Damage Blocking!");
            player_health = Mathf.Clamp(player_health - damage_to_player_blocking, 0, player_health);
        }else{
            Debug.Log("Player Damage!");
            player_health = Mathf.Clamp(player_health - damage_to_player, 0, player_health);
        }

        //update player_health_UI

        if(player_health == 0){
            //dead!!!
        }
    }

    public void Enemy_got_hit(string hit_pos){

        //Block Judge
        if(enemy_AI.is_blocking){
            Debug.Log("Enemy Damage Blocking!");
            enemy_health = Mathf.Clamp(enemy_health - damage_to_enemy_blocking, 0, enemy_health);
        }else{
            Debug.Log("Enemy Damage!");
            enemy_health = Mathf.Clamp(enemy_health - damage_to_enemy, 0, enemy_health);
        }

        //update enemy_health_UI

        if(enemy_health == 0){
            //dead!!!
        }

    }
}
