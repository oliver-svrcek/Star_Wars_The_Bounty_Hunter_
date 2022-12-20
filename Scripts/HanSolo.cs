using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=System.Random;

public class HanSolo : EnemyShooter
{
    private Vector3 TopPosition { get; set; } = new Vector3();
    private Vector3 MiddlePosition { get; set; } = new Vector3();
    private Vector3 BottomPosition { get; set; } = new Vector3();
    private GameObject DetoniteChargePrefab { get; set; } = null;
    private GameObject Coin1GameObject { get; set; } = null;
    private GameObject Coin2GameObject { get; set; } = null;
    private GameObject LevelEndGameObject { get; set; } = null;
    private GameObject HanSoloSurrenderGameObject { get; set; } = null;
    private List<Vector3> DetoniteChargePositions { get; set; } = new List<Vector3>();

    private new void Awake()
    {
        base.Awake();
        
        if (this.gameObject.transform.Find("Positions/Top") is null)
        {
            Debug.LogError(
                "ERROR: <HanSolo> - " + this.gameObject.transform.name + "/Positions/Top game object was " +
                "not found in game object hierarchy."
            );
            Application.Quit(1);
        }
        TopPosition = this.gameObject.transform.Find("Positions/Top").position;
        
        if (this.gameObject.transform.Find("Positions/Middle") is null)
        {
            Debug.LogError(
                "ERROR: <HanSolo> - " + this.gameObject.transform.name + "/Positions/Middle game object was " +
                "not found in game object hierarchy."
            );
            Application.Quit(1);
        }
        MiddlePosition = this.gameObject.transform.Find("Positions/Middle").position;
        
        if (this.gameObject.transform.Find("Positions/Bottom") is null)
        {
            Debug.LogError(
                "ERROR: <HanSolo> - " + this.gameObject.transform.name + "/Positions/Bottom game object was " +
                "not found in game object hierarchy."
            );
            Application.Quit(1);
        }
        BottomPosition = this.gameObject.transform.Find("Positions/Bottom").position;
        
        if ((DetoniteChargePrefab = Resources.Load("Prefabs/Objects/DetoniteCharge") as GameObject) is null)
        {
            Debug.LogError("ERROR: <HanSolo> - Prefabs/Objects/DetoniteCharge resource was not loaded.");
            Application.Quit(1);
        }
        
        if ((Coin1GameObject = GameObject.Find("Coin (1)")) is null)
        {
            Debug.LogError(
                "ERROR: <HanSolo> - Coin (1) game object was not found in the game object hierarchy."
            );
            Application.Quit(1);
        }

        if ((Coin2GameObject = GameObject.Find("Coin (2)")) is null)
        {
            Debug.LogError(
                "ERROR: <HanSolo> - Coin (2) game object was not found in the game object hierarchy."
            );
            Application.Quit(1);
        }
        
        if ((LevelEndGameObject = GameObject.Find("LevelEnd")) is null)
        {
            Debug.LogError(
                "ERROR: <HanSolo> - LevelEnd game object was not found in the game object hierarchy."
            );
            Application.Quit(1);
        }
        
        if ((HanSoloSurrenderGameObject = GameObject.Find("HanSoloSurrender")) is null)
        {
            Debug.LogError(
                "ERROR: <HanSolo> - HanSoloSurrender game object was not found in the game object hierarchy."
            );
            Application.Quit(1);
        }
        
        if (this.gameObject.transform.Find("DetoniteChargePositions") is null)
        {
            Debug.LogError(
                "ERROR: <HanSolo> - " + this.gameObject.transform.name + "DetoniteChargePositions game object was " +
                "not found in game object hierarchy."
            );
            Application.Quit(1);
        }
        
        foreach (Transform child in this.gameObject.transform.Find("DetoniteChargePositions"))
        {
            DetoniteChargePositions.Add(child.position);
        }
    }
    
    private new void Start()
    {
        base.Start();

        if (!UseOnlyEditorValues)
        {
            CanHeal = true;
            MaximumHealth = 200000;
            CurrentHealth = MaximumHealth;
            BulletDamage = 9999;
            BulletsPerShot = 1;
            BulletSpeed = 20f;
            ShootingRate = 1.25f;
            HealStartTime = 2f;
            HealPoints = 15;
            DeathSound = "HanSoloDeathSound";
        }

        LevelEndGameObject.SetActive(false);
        Coin1GameObject.SetActive(false);
        Coin2GameObject.SetActive(false);
        HanSoloSurrenderGameObject.SetActive(false);
        
        ShootCoroutine = StartCoroutine(Shoot());
        StartCoroutine(ChangePosition());
        StartCoroutine(SpawnDetoniteCharges());
    }

    private new void Update()
    {
        LookAtPlayer();
    }

    private new IEnumerator ChangePosition()
    {
        Random random = new Random();
        Vector3 lastPosition = BottomPosition;
        List<Vector3> positions;

        yield return new WaitForSeconds(5f);
        
        while (true)
        {
            positions = new List<Vector3> {TopPosition, MiddlePosition, BottomPosition};
            positions.Remove(lastPosition);
            
            var randomPositionIndex = random.Next(positions.Count);
            var randomPosition = positions[randomPositionIndex];
            
            this.transform.position = randomPosition;
            lastPosition = randomPosition;
            
            StopCoroutine(ShootCoroutine);
            ShootCoroutine = null;
            yield return new WaitForSeconds(0.75f);
            ShootCoroutine = StartCoroutine(Shoot());
            
            yield return new WaitForSeconds(5f);
        }
    }
    
    private IEnumerator SpawnDetoniteCharges()
    {
        Random random = new Random();
        
        while (true)
        {
            yield return new WaitForSeconds(25f);

            while (true)
            {
                if (random.Next(3) == 1)
                {
                    foreach (Vector3 DetoniteChargePosition in DetoniteChargePositions)
                    {
                        GameObject detoniteCharge = Instantiate(
                            DetoniteChargePrefab, DetoniteChargePosition, Quaternion.identity
                        );
                        detoniteCharge.GetComponent<DetoniteCharge>().Activate();
                    }
                    
                    break;
                }
                
                yield return new WaitForSeconds(1f);
            }
        }
    }
    
    public void Surrender()
    {
        HanSoloSurrenderGameObject.SetActive(true);
    }
}