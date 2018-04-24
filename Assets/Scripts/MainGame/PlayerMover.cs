using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {

    public GameObject energyBeam, explosion, explosionBig;

    public float speed = 6.0F;
    public float sprintSpeed = 12.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    CharacterController charController;
    public Animator animator, animatorBack;
    Player player;

    float lastOrientaion = 1;

    bool isAttacking;
    bool isRange;
    bool hasAttacked;

    bool jump;
    float jumpTimer;

    void Start() {
        charController = GetComponent<CharacterController>();
        player = GetComponent<Player>();
    }

    void Update() {
        animator.SetBool("Jump", false);

        if (!isAttacking || !isRange) {
            if (charController.isGrounded) { 
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= Input.GetButton("Sprint") && player.blueOil >= 50 ? sprintSpeed : speed;

                if(animator.GetBool("Jump"))
                    animator.SetBool("Jump", false);
                if (animator.GetBool("Air"))
                    animator.SetBool("Air", false);
            }

            if (Input.GetButtonDown("Jump") && !isAttacking) {
                Jump();
            }

            if (Input.GetAxis("Horizontal") > 0) {
                animator.gameObject.SetActive(true);
                animatorBack.gameObject.SetActive(false);

                animator.SetFloat("WalkSpeed", Input.GetAxis("Horizontal") + (player.blueOil >= 50 ? Input.GetAxis("Sprint") : 0));
                lastOrientaion = 1;
            } else if (Input.GetAxis("Horizontal") < 0) {
                animator.gameObject.SetActive(true);
                animatorBack.gameObject.SetActive(false);

                animator.SetFloat("WalkSpeed", -Input.GetAxis("Horizontal") + (player.blueOil >= 50 ? Input.GetAxis("Sprint") : 0));
                lastOrientaion = -1;
            } else if (Input.GetAxis("Vertical") > 0) {
                animator.gameObject.SetActive(false);
                animatorBack.gameObject.SetActive(true);

                animatorBack.SetFloat("WalkSpeed", Input.GetAxis("Vertical") + (player.blueOil >= 50 ? Input.GetAxis("Sprint") : 0));
            } else if (Input.GetAxis("Vertical") < 0) {
                animator.gameObject.SetActive(true);
                animatorBack.gameObject.SetActive(false);

                animator.SetFloat("WalkSpeed", -Input.GetAxis("Vertical") + (player.blueOil >= 50 ? Input.GetAxis("Sprint") : 0));
            } else {
                animator.gameObject.SetActive(true);
                animatorBack.gameObject.SetActive(false);
            }

            transform.localScale = new Vector3(lastOrientaion, 1, 1);
        }

        if(charController.isGrounded && !isRange) {
            if(Input.GetButtonDown("Fire1")) {
                RangeAttack();
            }
        }

        moveDirection.y -= gravity * 4 * Time.deltaTime;

        charController.Move(moveDirection * Time.deltaTime);

        if (jump) {
            jumpTimer += Time.deltaTime;
            if(jumpTimer >= 0.3f) {
                jump = false;
                jumpTimer = 0;
            }
        }
        
    }

    void Jump() {
        if(!jump) {
            jump = true;
            if (charController.isGrounded) {
                float jumper = Mathf.Clamp(player.yellowOil / 40, 1, 8);
                moveDirection.y = jumpSpeed * 2 * jumper;
                animator.SetBool("Jump", true);
                animator.SetBool("Air", true);
            }
        }
        else {
            JumpAttack();
            moveDirection.y = -8;
        }
    }

    void JumpAttack() {
        animator.SetBool("IsAttacking", true);
        isAttacking = true;
    }

    void RangeAttack() {
        animator.SetBool("RangeAttack", true);
        isRange = true;
    }

    void FixedUpdate() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetBool("IsAttacking")) {

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f && !hasAttacked) {
                hasAttacked = true;
                animator.SetBool("IsAttacking", false);

                Debug.Log("Player Attack");
                Attack();
            }

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f && hasAttacked) {
                isAttacking = false;
            }
        } else {
            isAttacking = false;
            hasAttacked = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RangeAttack") && animator.GetBool("RangeAttack")) {

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f && !hasAttacked) {
                hasAttacked = true;
                animator.SetBool("RangeAttack", false);

                Debug.Log("Player Range");
                Range();
            }

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f && hasAttacked) {
                //isRange = false;
            }
        } else if(!animator.GetCurrentAnimatorStateInfo(0).IsName("RangeAttack")) {
            isRange = false;
            hasAttacked = false;
        }
    }

    void Attack() {
        Collider[] hit = Physics.OverlapSphere(transform.position, Mathf.Clamp(GetComponent<Player>().redOil / 20, 3, 100), LayerMask.GetMask("Enemies"));
        hasAttacked = true;
        if (hit.Length > 0) {
            foreach (Collider enemy in hit) {
                Instantiate(energyBeam, enemy.transform.position + new Vector3(0, -1, 0), Quaternion.identity);

                if (enemy.GetComponent<Enemy>()) {
                    if (enemy.GetComponent<Enemy>().Damage(Mathf.Clamp(GetComponent<Player>().redOil / 2, 25, 100))) {
                        GetComponent<Player>().AddOil(enemy.GetComponent<Enemy>().id, enemy.GetComponent<Enemy>().oil);
                        Instantiate(enemy.GetComponent<Enemy>().bigEnemy ? explosionBig : explosion, enemy.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                        DestroyImmediate(enemy.gameObject);
                    }
                }
            }
        }
    }

    void Range() {
        hasAttacked = true;
        Instantiate(energyBeam, transform.position + new Vector3(lastOrientaion, 0, 0), Quaternion.Euler(new Vector3(0, 0, -90 * lastOrientaion)));

        RaycastHit[] hitAll = Physics.RaycastAll(transform.position, Vector3.right * lastOrientaion, 9, LayerMask.GetMask("Enemies"));

        if(hitAll.Length > 0) {
            foreach(RaycastHit hit in hitAll) {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy.Damage(Mathf.Clamp(GetComponent<Player>().redOil / 2, 25, 100))) {
                    Instantiate(enemy.bigEnemy ? explosionBig : explosion, enemy.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                    DestroyImmediate(enemy.gameObject);
                }
            }
        }
    }
    

}
