using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [SerializeField]
    private PairCouleur _pairCouleur;
    [SerializeField]
    private CombatData _combatData;

    
    private int _niceAim = 0;
    private int _badAim = 0;
    private float _timerDuration =50;
    [SerializeField]
    private Color _actualColor;
    private float _waitTime = 1f;
    public void OnAEnable()
    {
        
    }  
    public void Update()
    {
        EndCombatVerif();
        
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Couleur"))
                {    
                    
                    if (_pairCouleur.CouleurComplementaire(_actualColor, hit.collider.gameObject.GetComponent<Renderer>().material.color))
                    {
                        Debug.Log(_actualColor);
                        Debug.Log(hit.collider.gameObject.GetComponent<Renderer>().material.color);
                        Debug.Log("Couleur Complementaire");
                        _niceAim++;
                    }
                    else
                    {
                        Debug.Log(_actualColor);
                        Debug.Log(hit.collider.gameObject.GetComponent<Renderer>().material.color);
                        Debug.Log("Couleur non Complementaire");
                        _badAim++;
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") )
                {
                    Debug.Log("Enemy");
                    _badAim++;
                }
            }
        }
    }

    private IEnumerator TimerCombatCoroutine()
    {
        float _timeRemaining = _timerDuration;
        while (_timeRemaining > 0f)
        {
            yield return new WaitForSeconds(_waitTime);
            _timeRemaining--;
        }
    }
    private void EndCombatVerif()
    {
       
    }
}
