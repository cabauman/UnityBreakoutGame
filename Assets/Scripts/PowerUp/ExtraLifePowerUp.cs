﻿using UnityEngine;

public class ExtraLifePowerUp : PowerUp
{
    public override string SpriteName { get; } = "ExtraLife";

    public override void ApplyEffect(Game game, Vector3 position)
    {
        UnityEngine.Debug.Log("Extra life!");
        game.NumLives.Value += 1;
    }
}
