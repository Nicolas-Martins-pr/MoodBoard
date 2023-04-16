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
    public Amelioration _amelioration;
    [SerializeField] ParticleSystem inkParticle;

    private WheelRotating _colorWheel;

    [SerializeField]
    private GameObject _enemy;
    private GameObject _body;
    private GameObject _head;
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private ParticlesController _particle;

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
    private RaycastHit _hit;

    private float _health = 50;

    
    public void Start()
    {
        _pairCouleur = GetComponent<PairCouleur>();
        _sphereMethod = GetComponent<SphereMethod>();
        _amelioration = this.gameObject.GetComponentInParent<Amelioration>();
    }

    public void OnEnable()
    {
        //Verifie la presence d'amelioration et les appliquent
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

        //Assigne les valeurs de combat a chaque nouveau combat
        _totalBlackColor = 0;
        _totalColor = 0;
        _niceAim = 0;
        _badAim = 0;
        _health = 50;
        _timerDuration = _combatData.timer;
        _timeIncreaseBlackness = _combatData.timerIncreaseBlackness;
        _niceAimValue = _combatData.niceAimValue;
        _badAimValue = _combatData.badAimValue;

        _timerDuration = 30;
        _niceAimValue = 6;
        _badAimValue = 3;

        StartCoroutine(TimerCombatCoroutine());
    }  
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition).origin, _camera.ScreenPointToRay(Input.mousePosition).direction,out _hit, 10, LayerMask.GetMask("Couleur"));

            if (_hit.collider != null && _hit.collider.gameObject.layer == LayerMask.NameToLayer("Couleur"))
            {
                // touche la bulle sur le perso

                Debug.Log("Couleur");
                //changer la couleur ici
                _actualColor = _colorWheel.GetActualColor();
                if (_pairCouleur.CouleurComplementaire(_actualColor, _hit.collider.gameObject.GetComponent<Renderer>().material.color))
                {
                    // _particle.paintColor = _hit.collider.gameObject.GetComponent<Renderer>().material.color;
                    inkParticle.Play();
                    //StartCoroutine(_sphereMethod.LaunchSphere());
                    _sphereMethod.DisableSphere();
                    Debug.Log("Couleur Complementaire");
                    _niceAim++;
                    _health += _niceAimValue;

                }
                else if (_badAimAnulator > 0)
                {
                    _badAimAnulator--;
                }
                else
                {
                    Debug.Log("Couleur non Complementaire");
                    _badAim++;
                    _health -= _badAimValue;
                    
                }

                Debug.Log("En Dedans !!");

            


                if (Input.GetMouseButtonDown(0))
                    inkParticle.Play();


            }
            else
            {
                // touche le perso mais pas la cible
                _badAim++;
                Debug.Log("En Dehors !!");
                _health -= _badAimValue;
            }

        _totalBlackColor = _badAim * _badAimValue;
        _totalColor = _niceAim * _niceAimValue;

        }
        

        if (Input.GetMouseButtonUp(0))
            inkParticle.Stop();
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

    public float GetTimeRemaining()
    {
        return _timeRemaining;
    }

    
    //V�rifie si le combat est fini
    private void EndCombatVerif()
    {



        /*        _totalBlackColor = _badAim * _badAimValue + ((_timerDuration - (_timeRemaining+_timerBonus)) * _timeIncreaseBlackness);
        */
        
        _totalBlackColor = _badAim * _badAimValue;

        _totalColor = _niceAim * _niceAimValue;


        if(_health > 99 || _health < 1 ){
            EndCombat();
        }

        if(_timeRemaining < 1){
            Debug.Log("FIN  TEMMPPPSSS");
            EndCombat();
        }
        /*
        Debug.Log(_totalColor + ", " + _totalBlackColor);*/
        /*        if (_score >= 100 )
                    EndCombat();*/


    }
    
    //Verifie le score et termine le combat
    private void EndCombat()
    {
        Debug.Log("Combat End");
        Debug.Log("Nice Aim : " + _niceAim);
        Debug.Log("Bad Aim : " + _badAim);
        Debug.Log("Health : " + _health);
        if (_totalBlackColor > _totalColor)
            Debug.Log("You lose");
        else
            Debug.Log("You win");
        
        //A regarder
        _sphereMethod.DisableSphere();
       // _enemy.gameObject.SetActive(false);
        Destroy(_enemy);
        gameObject.SetActive(false);

        //Réactive le mouvement
        LevelController.Instance.EndCombat();

    }
    public void SetEnemy(GameObject enemy)
    {
        _enemy = enemy;
       
    }
    public GameObject GetEnemy()
    {
        return _enemy;
    }

    public int GetColor()
    {
        return (int)(_totalColor);
    }

    public int GetBlack()
    {

        return (int) (_totalBlackColor);
    }

    public int GetHealth(){
        return (int) (_health);
    }
    

    public void SetColorWheel(WheelRotating colorWheel)
    {
        _colorWheel = colorWheel;
    }


}

