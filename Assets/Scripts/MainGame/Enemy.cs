using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public Color color;
    public MeshRenderer[] colorableObjects;
    public int id;

    Transform player;
    Animator animator;

    public bool bigEnemy;
    public float health = 50;
    public float oil;

    public AudioClip[] clips;
    AudioSource source;

    NavMeshAgent agent;

    float scale;

    NavMeshHit hit;
    bool blocked;

    bool isAttacking;
    bool hasAttacked;

    public float leanOffset;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        source = transform.GetChild(1).GetComponent<AudioSource>();

        id = Random.Range(0, 4);
        color = id == 0 ? Color.red : (id == 1 ? Color.blue : (id == 2 ? Color.yellow : Color.green));

        oil = Random.Range(2f, 4f);
        oil *= bigEnemy ? 2 : 1;

        foreach (MeshRenderer rend in colorableObjects) {
            rend.material.color = color;
        }

        scale = transform.GetChild(0).localScale.x;
    }

    void Update() {
        transform.GetChild(0).localEulerAngles = new Vector3(-45 - leanOffset, -transform.eulerAngles.y + 180, 0);
    }

    void FixedUpdate() {
        animator.SetFloat("WalkSpeed", agent.velocity.magnitude);

        if (Vector3.Distance(transform.position, player.position) < 2) {
            animator.SetBool("IsAttacking", true);
            isAttacking = true;
        } else if (!isAttacking) {
            agent.destination = player.position;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= (bigEnemy ? 0.7f : 0.35f) && !hasAttacked) {
                hasAttacked = true;
                if (Vector3.Distance(transform.position, player.position) < (bigEnemy ? 4 : 2)) {
                    player.GetComponent<Player>().RemoveOil(id, bigEnemy ? 7 : 2);
                    oil += bigEnemy ? 5 : 1;
                }
                source.PlayOneShot(clips[0]);
            }

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f && hasAttacked) {
                isAttacking = false;
                animator.SetBool("IsAttacking", false);
            }
        } else {
            isAttacking = false;
            hasAttacked = false;
        }

        if (player.position.x > transform.position.x) {
            transform.GetChild(0).localScale = new Vector3(scale, scale, scale);
        } else {
            transform.GetChild(0).localScale = new Vector3(-scale, scale, scale);
        }
    }

    public bool Damage(float amount) {
        health -= amount;

        if(health <= 0) {
            return true;
        }
        return false;
    }
}
