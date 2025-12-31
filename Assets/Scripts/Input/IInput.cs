using System;
using UnityEngine;

namespace Assets.Scripts.Input
{
    interface IInput
    {
        Vector2 GetMoveVector();

        Action CursePressed { get; set; }
        Action CurseReleased { get; set; }
        Action DashPressed { get; set; }
    }
}