using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.WSA;

[RequireComponent(typeof(SphereMethod))]
[RequireComponent(typeof(SphereController))]
[RequireComponent(typeof(PairCouleur))]
[RequireComponent(typeof(RaycastSphere))]


public class CombatSystem : MonoBehaviour
{
    
    [SerializeField]
    private PairCouleur _pairCouleur;
    [SerializeField]
    private CombatData _combatData;
    [SerializeField]
    private SphereMethod _sphereMethod;
    [SerializeField]
    private Amelioration _amelioration;
    [SerializeField]
    private WheelRotating _colorWheel;

    private int _niceAim = 0;
    private int _badAim = 0;

    private float _timeRemaining;

    [SerializeField]
    private string _actualColor;
    private float _waitTime = 1f;

    
    private float _totalBlackColor =0;
    private float _totalColor= 0;

    private float _niceAimValue;
    private float _badAimValue;
    private float _timerDuration;
    private float _timeIncreaseBlackness;
    
    private int _badAimAnulator;
    private float _timerBonus;


    private float _score=0;
    
    public void Start()
    {
        _pairCouleur = GetComponent<PairCouleur>();
        _sphereMethod = GetComponent<SphereMethod>();
        _amelioration = GetComponent<Amelioration>();
        

    }
    public void OnEnable()
    {
        //Verifie la présence d'amélioration et les appliquent
        if (_amelioration != null) { 
            if (_amelioration.hasBadAimAnnulator == true)
            {
                _badAimAnulator = 1;
            }
            else
            {
                _badAimAnulator = 0;
            }
            if (_amelioration.hasTimerBonus == true)
            {
                _timerBonus = 5;
            }
            else
            {
                _timerBonus = 0;
            }
        }
        //Assigne les valeurs de combat à chaque nouveau combat
        _totalBlackColor = 0;
        _totalColor = 0;
        _niceAim = 0;
        _badAim = 0;
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
                    Debug.Log("Couleur");
                    //changer la couleur ici
                    _actualColor = _colorWheel.GetActualColor();
                    if (_pairCouleur.CouleurComplementaire(_actualColor, hit.collider.gameObject.GetComponent<Renderer>().material.color))
                    {
                        //StartCoroutine(_sphereMethod.LaunchSphere());
                        _sphereMethod.DisableSphere();
                        Debug.Log("Couleur Complementaire");
                        _niceAim++;  
                    }
                    else if (_badAimAnulator > 0)
                    {
                        _badAimAnulator--;
                    }
                    else
                    {
                        Debug.Log("Couleur non Complementaire");
                        _badAim++;
                    }
                }
            }
        }
        EndCombatVerif();
    }
    
    //Le timer du combat
    private IEnumerator TimerCombatCoroutine()
    {
         _timeRemaining = _timerDuration;
        while (_timeRemaining > 0f)
        {
            yield return new WaitForSeconds(_waitTime);
            _timeRemaining --;
        }
        
    }
    
    //Vérifie si le combat est fini
    private void EndCombatVerif()
    {
       /* Debug.Log("Total color" + _totalColor);
        Debug.Log("Total black color" + _totalBlackColor);       */ 
        _totalBlackColor = _badAim * _badAimValue + ((_timerDuration - (_timeRemaining+_timerBonus)) * _timeIncreaseBlackness);
        _totalColor= _niceAim * _niceAimValue;
        _score = _totalColor + _totalBlackColor;
        if (_score >= 100 )
            EndCombat();


    }
    
    //Verifie le score et termine le combat
    private void EndCombat()
    {
   /*     Debug.Log("Combat End");
        Debug.Log("Nice Aim : " + _niceAim);
        Debug.Log("Bad Aim : " + _badAim);
        Debug.Log("Score : " + _score);*/
/*        if (_totalBlackColor > _totalColor) 
            Debug.Log("You lose");
        else
            Debug.Log("You win");
       */
        //gameObject.SetActive(false);
    }
}
