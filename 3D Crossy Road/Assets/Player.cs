using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;
    [SerializeField] ParticleSystem dieParticles;
    [SerializeField, Range(0.1f, 1f)] float moveDuration = 0.2f;
    [SerializeField, Range(0.1f, 1f)] float jumpHeight = 0.5f;
    [SerializeField] private AudioSource jumpSfx;
    [SerializeField] private AudioSource DieSfx;


    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;
    [SerializeField] private int maxTravel;
    public int MaxTravel { get=> maxTravel; }
    [SerializeField] private int currentTravel;
    public int CurrentTravel { get => currentTravel; }
    public bool IsDie { get => this.enabled == false; }

    public void SetUp(int minZPos, int extent)
    {
        backBoundary = minZPos - 1;
        leftBoundary = - (extent + 1);
        rightBoundary = extent + 1;
    }

    private void Update()
    {
        var moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            jumpSfx.Play();
            moveDir += new Vector3(0, 0, 1);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            jumpSfx.Play();
            moveDir += new Vector3(0, 0, -1);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            jumpSfx.Play();
            moveDir += new Vector3(1, 0, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            jumpSfx.Play();
            moveDir += new Vector3(-1, 0, 0);
        }
        if (moveDir != Vector3.zero && IsJumping() == false)
            Jump(moveDir);

    }
    private void Jump(Vector3 targetDirection)
    {
        // Atur rotasi
        var targetPosition = transform.position + targetDirection;
        transform.LookAt(targetPosition);
        // transform.DOMoveY(2f, 0.1f).OnComplete(() => transform.DOMoveY(0, 0.1f));
        // transform.DOMove(TargetPosition, 0.2f);
        
        // Loncat ke atas
        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration / 2));
        moveSeq.Append(transform.DOMoveY(0, moveDuration / 2));

        // cek target pos klo ada pohon gabisa tembus
        if (Tree.AllPosition.Contains(targetPosition))
            return;
        // 
        if (targetPosition.z <= backBoundary || 
            targetPosition.x <= leftBoundary ||
            targetPosition.x >= rightBoundary)
            return;

        // Gerak maju/mundur/samping
        transform.DOMoveX(targetPosition.x, moveDuration);
        transform
        .DOMoveZ(targetPosition.z, moveDuration)
        .OnComplete(UpdateTravel);
    }

    private void UpdateTravel()
    {
        currentTravel = (int) this.transform.position.z;
        
        if(currentTravel > maxTravel)
            maxTravel = currentTravel;
        
        stepText.text = "STEP: " + maxTravel.ToString();
    }
    public bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled == false)
            return;

        // di execute sekali pada fram ketika nempel pertama kali
        var car = other.GetComponent<Car>();
        if (car != null)
        {
            DieSfx.Play();
           AnimateCrash(car);
        }
        if(other.tag == "Car")
        {
           // AnimateDie();
        }
    }

    private void AnimateCrash(Car car)
    {
        // var isRight = car.tranform.rotation.y == 90;

        // transform.DOMoveX(isRight ? 8 : -8, 0.2f);
        // transform
        // .DORotate(Vector3.forward*360, 1f)
        // .SetLoops(100, LoopType.Restart);

        // Gepeng
        transform.DOScaleY(0.1f, 0.2f);
        transform.DOScaleX(3, 0.2f);
        transform.DOScaleZ(2, 0.2f);
        
        this.enabled = false; 
        dieParticles.Play(); // error logic
    }
    private void OnTriggerStay(Collider other){
        // di execute setiap frame salama masih nempel/collide
        // Debug.Log("Stay");
    }
    private void OnTriggerExit(Collider other){
        // di execute sekali pada frame ketika tidak nempel
        // Debug.Log("Exit");
    }
}