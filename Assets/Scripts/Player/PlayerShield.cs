using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{

    [SerializeField]
    ParticleSystem shield;

    [SerializeField]
    float maxParticleLifeTime;
    [SerializeField]
    float minParticleLifeTime;

    [SerializeField]
    float maxParticleSpeed;
    [SerializeField]
    float minParticleSpeed;
    [SerializeField]
    float maxSize;
    [SerializeField]
    float minSize;

    [SerializeField]
    CircleCollider2D circleCollider;

    public PlayerShieldState State = PlayerShieldState.Unhealthy;

    public void SetShieldStrength(float shieldStrength)
    {

        var newMain = shield.main;
        var shieldPercentage = shieldStrength / 100;

        // calculate particle lifeTime
        newMain.startLifetime = GetShieldStrengthValue(minParticleLifeTime, maxParticleLifeTime, shieldPercentage);

        // calculate particle speed
        newMain.startSpeed = GetShieldStrengthValue(minParticleSpeed, maxParticleSpeed, shieldPercentage);

        // calculate particle size
        newMain.startSize = GetShieldStrengthValue(minSize, maxSize, shieldPercentage);

        // calculate color 
        var notRed = GetShieldStrengthValue(0f, 1f, shieldPercentage);
        newMain.startColor = new Color(1f, notRed, notRed, 0.3f);

        // Set shield state
        if (shieldStrength >= 50)
        {
            Debug.Log("Healthy");
            State = PlayerShieldState.Healthy;
        }
        else
        { 
            if (shieldStrength < 50 && shieldStrength > 30)
            {// 30 - 50
                State = PlayerShieldState.Unhealthy;                
            }

            else if (shieldStrength < 31 && shieldStrength > 10)
            {// 11 - 30
                if (circleCollider.enabled == false)
                {
                    ReActivateShield();
                }
                State = PlayerShieldState.Low;
            }


            else if (shieldStrength < 11 && shieldStrength > 0 && State != PlayerShieldState.Down)
            {// 1 - 10 ONLY IF SHIELD IS NOT DEACTIVATED
                 State = PlayerShieldState.Critical;
            }

            if (shieldStrength <= 0)
            {// Below 1 
                DeactivateShield();
            }
        }
    }

    public void ReActivateShield()
    {
        StartCoroutine(ShieldBurst_cr());
        shield.Play();
        SfxManager.PlaySfx(SfxNames.ReactivateShield.ToString(), 0, 0.5f);
        circleCollider.enabled = true;
    }

    void DeactivateShield()
    {
        State = PlayerShieldState.Down;
        shield.Stop();
        SfxManager.PlaySfx(SfxNames.DeactivateShield.ToString(), 0, 0.5f);
        circleCollider.enabled = false;
    }

    public IEnumerator ShieldBurst_cr()
    {

        var newMain = shield.main;
       
        // Save old  values
        var tempLifetime = newMain.startLifetime;
        var tempstartSpeed = newMain.startSpeed;

        // tripple values for burst
        newMain.startLifetime = maxParticleLifeTime;
        newMain.startSpeed = 2;

        // initiate emission burst
        var burst = new ParticleSystem.Burst(0.3f, 300);
        shield.emission.SetBursts(
            new ParticleSystem.Burst[]{
                burst
        });

        // wait for duration
        yield return new WaitForSeconds(0.5f);

        // delete burst
        burst = new ParticleSystem.Burst(0f, 0);
        shield.emission.SetBursts(
            new ParticleSystem.Burst[]{
                burst
        });

        // return old vales
        newMain.startLifetime = tempLifetime;
        newMain.startSpeed = tempstartSpeed;


    }

    float GetShieldStrengthValue(float minValue, float maxValue, float shieldPercentage)
    {
        var returnValue = (maxValue - minValue) * shieldPercentage + minValue;
        return returnValue;
    }
}
