using static PairCouleur;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Nouvelle Paire Couleur", menuName = "Paire Couleur")]
public class NouvellePaireCouleur : ScriptableObject
{
    public Color couleur1;
    public Color couleur2;
    public string nomCouleur1;
    public string nomCouleur2;

}