using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 200f;
    public float impaceForce = 50f;
    public float fireRate = 10f;

    private float nextBullet = 0f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public Camera fpsCam;


    private bool isReloading = false;
    public ParticleSystem muzzleFlash;

    Animator animator;


    private void Start(){
        animator = GetComponent<Animator>();
        currentAmmo = maxAmmo;
    }
    // Update is called once per frame
    void Update()
    {

        if (isReloading)
        {
            return;
        }
        if (currentAmmo <= 0)
        {
           StartCoroutine(Reload());
            return;
        }
        if (Input.GetButton("Fire1") /*&& Time.time >= nextBullet*/)
        {
            nextBullet = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    IEnumerator Reload(){
        Debug.Log("Reloading");
        isReloading = true;
        animator.SetBool("Reload", true);

        yield return new WaitForSeconds(1f);
        currentAmmo = maxAmmo;
        isReloading = false;
        animator.SetBool("Reload", false);
    }

    void Shoot(){
    if(muzzleFlash != null) {
        
            muzzleFlash.Play();
    }
        currentAmmo--;
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward, out hit ,range))
        {
            
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impaceForce);
            }
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetDamage(damage);
            }
        }
    }
}
