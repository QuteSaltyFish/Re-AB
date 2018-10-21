using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SnazzlebotTools.ENPCHealthBars;

public class BoxingManager : MonoBehaviour {

    public ENPCHealthBar enemy_health_bar;

    public int player_health_start = 100;
    [SerializeField]
    private int player_health;

    public int enemy_health_start = 100;
    [SerializeField]
    private int enemy_health;

    [SerializeField]
    private Transform enemy_transform;
    [SerializeField]
    private EnemyAI enemy_AI;
    [SerializeField]
    private EnemyAnimation enemy_animation;

    [SerializeField]
    private GameObject gloveL;
    [SerializeField]
    private GameObject gloveR;
    [SerializeField]
    private float block_check_max_angle = 30f;

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
    private float enemy_hit_timer = 0f;

    private void Start()
    {
        player_health = player_health_start;
        enemy_health = enemy_health_start;
    }

    private void Update()
    {
        enemy_health_bar.Value -= 1;
        if (enemy_hit_timer > 0){
            enemy_can_get_hit = false;
            enemy_hit_timer -= Time.deltaTime;
        }else{
            enemy_hit_timer = 0;
            enemy_can_get_hit = true;
        }

        
    }

    public void Player_got_hit(){
        bool player_is_blocking = false;
        //Block Judge
        Vector3 from_dir_1 = -gloveL.transform.up;
        Vector3 to_dir_1 = Vector3.up;
        float delta_angle_1 = Vector3.Angle(from_dir_1, to_dir_1);
        Vector3 from_dir_2 = -gloveR.transform.up;
        Vector3 to_dir_2 = Vector3.up;
        float delta_angle_2 = Vector3.Angle(from_dir_2, to_dir_2);
        if (delta_angle_1 <= block_check_max_angle && delta_angle_2 <= block_check_max_angle) player_is_blocking = true;

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
            if (enemy_can_get_hit)
            {
                if (hit_pos == "JabLeftWeakness")
                {
                    enemy_animation.GotHit("jableft");
                }
                else if (hit_pos == "JabRightWeakness")
                {
                    enemy_animation.GotHit("jabright");
                }
                else
                {
                    enemy_transform.Translate(transform.worldToLocalMatrix * new Vector3(0, 0, -0.5f));
                }

                Debug.Log("Enemy Damage!");
                enemy_health = Mathf.Clamp(enemy_health - damage_to_enemy, 0, enemy_health);
                enemy_hit_timer = enemy_hit_rate;
            }
        }

        //update enemy_health_UI

        if(enemy_health == 0){
            //dead!!!
        }

    }
}
