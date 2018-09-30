using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Relation : MonoBehaviour
{
    [SerializeField] AlignmentToPlayer alignmentToPlayer;
    public AlignmentToPlayer GetAlignmentToPlayer()
    {
        return alignmentToPlayer;
    }
    public void SetAlingnmentToPlayer(AlignmentToPlayer newAlin)
    {
        alignmentToPlayer = newAlin;
    }
}
