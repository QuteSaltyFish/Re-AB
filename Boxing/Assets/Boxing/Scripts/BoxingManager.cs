using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SnazzlebotTools.ENPCHealthBars;

public class BoxingManager : MonoBehaviour {

    public ENPCHealthBar enemy_health_bar;
    public ENPCHealthBar enemy_stamina_bar;
    public ENPCHealthBar enemy_satis_bar;

    public int player_health_start = 100;
    [SerializeField]
    private int player_health;

    public float enemy_health_start = 100f;
    [SerializeField]
    private float enemy_health;

    public float enemy_stamina_start = 100f;
    [SerializeField]
    private float enemy_stamina;

    public float enemy_satis_start = 0f;
    [SerializeField]
    private float enemy_satis;

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
    private int damage_to_player = 10;
    [SerializeField]
    private int damage_to_player_blocking = 3;

    [SerializeField]
    private float e_health_damage_face = 10f;
    [SerializeField]
    private float e_health_damage_body = 15f;
    [SerializeField]
    private float e_health_restore_persec = 5f;

    [SerializeField]
    private float e_stamina_damage_face = 5f;
    [SerializeField]
    private float e_stamina_damage_body = 10f;
    [SerializeField]
    private float e_stamina_restore_persec = 5f;
    [SerializeField]
    private float e_stamina_restore_pretime = 2f;
    private float e_stamina_restore_timer = 0f;

    [SerializeField]
    private float e_satis_inc_face = 5f;
    [SerializeField]
    private float e_satis_inc_body = 10f;

    private bool enemy_can_get_hit = true;
    [Header("Blank between 2 hits")]
    [SerializeField]
    private float enemy_hit_rate = 1f;
    private float enemy_hit_timer = 0f;

    public bool is_EndingA = false;
    public bool is_EndingB = false;
    public bool is_EndingC = false;

    [SerializeField]
    private EndingDialogs ending_dialogs;

    public BleedBehavior bleed_behavior;

    private void Start()
    {
        player_health = player_health_start;
        enemy_health = enemy_health_start;
        enemy_stamina = enemy_stamina_start;
        enemy_satis = enemy_satis_start;
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

        if (enemy_can_get_hit && !enemy_AI.is_blocking){
            if (!(is_EndingA || is_EndingB || is_EndingC)) {
                EnemyRestoreHealth();
                e_stamina_restore_timer += Time.deltaTime;
                if (e_stamina_restore_timer >= e_stamina_restore_pretime) {
                    EnemyRestoreStamina();
                }
            }
        }else{
            e_stamina_restore_timer = 0f;
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
        bleed_behavior.minAlpha = 0.2f + (100 - player_health) * 0.004f;   //100 - 0, 0 - 0.6

        if (player_health == 0){
            //dead!!!
        }
    }

    public void EnemyGotHit(string hit_pos){

        //Block Judge
        if (enemy_AI.is_blocking && !is_EndingA){
            Debug.Log("Enemy Damage Blocking!");
        }else if(!enemy_AI.is_blocking && !is_EndingA){
            if (enemy_can_get_hit && !is_EndingA)
            {
                if (hit_pos == "JabLeftWeakness")
                {
                    enemy_animation.GotHit("jableft");
                    enemy_health = Mathf.Clamp(enemy_health - e_health_damage_face, 0f, enemy_health);
                    enemy_stamina = Mathf.Clamp(enemy_stamina - e_stamina_damage_face, 0f, enemy_stamina);
                    enemy_satis = Mathf.Clamp(enemy_satis + e_satis_inc_face, enemy_satis, 100f);
                }
                else if (hit_pos == "JabRightWeakness")
                {
                    enemy_animation.GotHit("jabright");
                    enemy_health = Mathf.Clamp(enemy_health - e_health_damage_face, 0f, enemy_health);
                    enemy_stamina = Mathf.Clamp(enemy_stamina - e_stamina_damage_face, 0f, enemy_stamina);
                    enemy_satis = Mathf.Clamp(enemy_satis + e_satis_inc_face, enemy_satis, 10f);
                }
                else
                {
                    enemy_transform.Translate(transform.worldToLocalMatrix * new Vector3(0, 0, -0.5f));
                    enemy_health = Mathf.Clamp(enemy_health - e_health_damage_body, 0f, enemy_health);
                    enemy_stamina = Mathf.Clamp(enemy_stamina - e_stamina_damage_body, 0f, enemy_stamina);
                    enemy_satis = Mathf.Clamp(enemy_satis + e_satis_inc_body, enemy_satis, 10f);
                }

                Debug.Log("Enemy Damage!");
                enemy_hit_timer = enemy_hit_rate;
            }
        }else{      //is_EndingA
            is_EndingA = false;
            is_EndingB = true;
            ending_dialogs.PlayEndingDialog("endingB");
        }

        //update UI
        enemy_health_bar.Value = (int)enemy_health;
        enemy_stamina_bar.Value = (int)enemy_stamina;
        enemy_satis_bar.Value = (int)enemy_satis;
        

        if (enemy_health > 0f && (enemy_stamina - 0f <= Mathf.Epsilon || 100f - enemy_satis <= Mathf.Epsilon) ){
            is_EndingA = true;
            //End A
            ending_dialogs.PlayEndingDialog("endingA");
            //Stop Motion
            enemy_AI.is_dead = true;
            enemy_animation.is_dead = true;
        }

        if(enemy_health - 0f < Mathf.Epsilon){
            //End C
            ending_dialogs.PlayEndingDialog("endingC");
            //Stop Motion
            enemy_AI.is_dead = true;
            enemy_animation.is_dead = true;
        }

    }

    private void EnemyRestoreHealth(){
        enemy_health = Mathf.Clamp(enemy_health + e_health_restore_persec * Time.deltaTime, enemy_health, 100f);
        //update UI
        enemy_health_bar.Value = (int)enemy_health;
    }

    private void EnemyRestoreStamina(){
        enemy_stamina = Mathf.Clamp(enemy_stamina + e_stamina_restore_persec * Time.deltaTime, enemy_stamina, 100f);
        //update UI
        enemy_stamina_bar.Value = (int)enemy_stamina;
    }

}
