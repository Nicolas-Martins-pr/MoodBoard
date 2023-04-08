using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairCouleur : MonoBehaviour
{
    [SerializeField]
    private List<NouvellePaireCouleur> _listeCouleur = new List<NouvellePaireCouleur>();

    private Dictionary<Color, string> _couleur = new Dictionary<Color, string>();
    private Dictionary<string, string> _pairs = new Dictionary<string, string>();
    
    public void Start()
    {
        foreach (NouvellePaireCouleur paireCouleur in _listeCouleur)
        {
            _couleur.Add(paireCouleur.couleur1, paireCouleur.nomCouleur1);
            _couleur.Add(paireCouleur.couleur2, paireCouleur.nomCouleur2);
            _pairs.Add(paireCouleur.nomCouleur1, paireCouleur.nomCouleur2);
            _pairs.Add(paireCouleur.nomCouleur2, paireCouleur.nomCouleur1);
            
        }
    }

    public bool CouleurComplementaire(String color1, Color color2)
    {
        
        if ( !_couleur.TryGetValue(color2, out string nomCouleur2))
        {           
            return false;
        }
        Debug.Log(color1 + " " + nomCouleur2);
        if (_pairs.TryGetValue(color1, out string nomComplementaire2))
        {
            if (nomComplementaire2 == nomCouleur2)
            {
                return true;
            }
        }
        else if (_pairs.TryGetValue(nomCouleur2, out string nomComplementaire1))
        {
            if (nomComplementaire1 == color1)
            {
                return true;
            }
        }
        return false;
    }
 }
