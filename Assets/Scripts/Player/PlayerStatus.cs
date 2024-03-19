using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    public CharacterStatus characterStatus;

    private void Start()
    {
        characterStatus = this;
    }
}
