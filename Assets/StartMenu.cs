using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    bool PressFireTextToggle;

    [SerializeField]
    SfxNames LogoSfx;
    [SerializeField]
    Text PressFireText;
    Image shoot1;
    Image space;
    Image rocks;
    Image In;
    Image space2;

    [SerializeField]
    public Action StartGame;

    void Awake()
    {
        var logo = transform.Find("Logo");
        shoot1 = logo.transform.Find("Shoot").GetComponent<Image>();
        space = logo.transform.Find("Space1").GetComponent<Image>();
        rocks = logo.transform.Find("Rocks").GetComponent<Image>();
        In = logo.transform.Find("In").GetComponent<Image>();
        space2 = logo.transform.Find("Space2").GetComponent<Image>();

        ToggleLogo(false);
    }

    void OnEnable()
    {
        StartCoroutine(SetupStartMenu_cr());
    }
    
    void OnDisable()
    {
        CancelInvoke("PressStartTextToggleEffect");
        ToggleLogo(false);
    }

    private IEnumerator SetupStartMenu_cr()
    {
        ToggleLogo(false);

        yield return new WaitForSeconds(0);
        SfxManager.PlaySfx(LogoSfx.ToString());
        shoot1.enabled = true;
        yield return new WaitForSeconds(1);
        space.enabled = true;
        yield return new WaitForSeconds(0.2f);
        rocks.enabled = true;
        yield return new WaitForSeconds(1f);
        In.enabled = true;
        yield return new WaitForSeconds(0.5f);
        space2.enabled = true;

        yield return new WaitForSeconds(1);
        InvokeRepeating("PressStartTextToggleEffect", 0, 0.2f);
        SfxManager.PlaySfx(SfxNames.SpaceCollision.ToString());

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && ShootInSpaceSceneManager.Me.state == GameState.StartMenu)
        {
            
            StopAllCoroutines();
            StartCoroutine(StartGame_cr());

        }
    }

    private IEnumerator StartGame_cr()
    {
       
        ToggleLogo(false);
        CancelInvoke("PressStartTextToggleEffect");
        PressFireText.enabled = true;    
        SfxManager.PlaySfx(SfxNames.SpaceCollision.ToString());
        yield return new WaitForSeconds(1f);
        PressFireText.enabled = false;
        StartGame();
        this.enabled = false;
    }
    
    private void ToggleLogo(bool enabled)
    {
        shoot1.enabled = enabled;
        space.enabled = enabled;
        rocks.enabled = enabled;
        In.enabled = enabled;
        space2.enabled = enabled;
    }

    void PressStartTextToggleEffect()
    {
        PressFireText.enabled = PressFireTextToggle;
        PressFireTextToggle = !PressFireTextToggle;
    }
}
