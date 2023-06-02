using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GunMechanic : MonoBehaviour
{
    //Gun-stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //Bools for states
    bool shooting, readyToShoot, reloading;

    //References
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;


    //Graphics (muzzlelfash bulletholes and hud)
    public GameObject muzzleFlash;
    public GameObject bulletHoleGraphic;
    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        //text.SetText(bulletsLeft + "/ " + magazineSize);
    }
    private void Update()
    {
        MyInput();
    }
    private void MyInput()
    {
        //sausana vai nu spaidi vai turi
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) StartReload();

        //shoot
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;
       
        Debug.Log("shooting");
        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

       

        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            //Debug.Log(rayHit.collider.name);
            IDamageable damageable = rayHit.transform.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);
        }

        HandleMuzzleFlash();
        //Graphics

        Vector3 playerToHit = fpsCam.transform.position - rayHit.point;
        Quaternion rotation = Quaternion.LookRotation(playerToHit, Vector3.up);
        Instantiate(bulletHoleGraphic, rayHit.point + (rayHit.normal * .01f), rotation);


        //Instantiate(muzzleFlash, attackPoint.transform.position, attackPoint.transform.rotation);

        //Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        bulletsLeft--;
        bulletsShot--;
        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }


    private void HandleMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            GameObject flash = Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);
            flash.transform.parent = attackPoint;
            Destroy(flash, 0.05f);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }
    
    public void StartReload()
    {
        if (!reloading)
        {
            StartCoroutine(Reload());
        }
    }

    //korutina
    private IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
