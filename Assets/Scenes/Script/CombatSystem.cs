using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.WSA;

public class CombatSystem : MonoBehaviour
{
    [SerializeField]
    private PairCouleur _pairCouleur;
    [SerializeField]
    private CombatData _combatData;

    
    private int _niceAim = 0;
    private int _badAim = 0;

    private float _timeRemaining;

    [SerializeField]
    private Color _actualColor;
    private float _waitTime = 1f;

    private float _totalColored=0;
    private float _totalBlackColor =0;
    private float _totalColor= 0;

    private float _niceAimValue;
    private float _badAimValue;
    private float _timerDuration;
    private float _timeIncreaseBlackness;
    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private SphereController _sphereController;

    private float _score=0;
    public void OnEnable()
    {
        _timerDuration = _combatData.timer;
        _timeIncreaseBlackness = _combatData.timerIncreaseBlackness;
        _niceAimValue = _combatData.niceAimValue;
        _badAimValue = _combatData.badAimValue;
        StartCoroutine(TimerCombatCoroutine());
    }  
    public void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Couleur"))
                {    
                    //changer la couleur ici
                    if (true)
                    {
                        Debug.Log("Bonjour je vais te hanté");
                        StartCoroutine(_sphereController.LaunchSphere());
                        Debug.Log("Couleur Complementaire");
                        _niceAim++;
                        
                    }
                    else
                    {
                        Debug.Log("Couleur non Complementaire");
                        _badAim++;
                    }
                }
            }
        }
       // EndCombatVerif();
    }

    private IEnumerator TimerCombatCoroutine()
    {
         _timeRemaining = _timerDuration;
        while (_timeRemaining > 0f)
        {
            yield return new WaitForSeconds(_waitTime);
            _timeRemaining --;
        }
        
    }
    private void EndCombatVerif()
    {
        Debug.Log("Total color" + _totalColor);
        Debug.Log("Total black color" + _totalBlackColor);        
        _totalBlackColor = _badAim * _badAimValue + ((_timerDuration - _timeRemaining) * _timeIncreaseBlackness);
        _totalColor= _niceAim * _niceAimValue;
        _score = _totalColor + _totalBlackColor;
        if (_score >= 100 && (_totalColor >= 50f || _totalBlackColor >= 50f))
            EndCombat();


    }
    private void EndCombat()
    {
        Debug.Log("Combat End");
        Debug.Log("Nice Aim : " + _niceAim);
        Debug.Log("Bad Aim : " + _badAim);
        Debug.Log("Score : " + _score);
        if (_score-_totalBlackColor < 50)
            Debug.Log("You lose");
        else
            Debug.Log("You win");
        _niceAim = 0;
        _badAim = 0;
        //gameObject.SetActive(false);
    }
}
