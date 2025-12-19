using Assets.Scripts.Data.Enums;
using System;

namespace Assets.Scripts.Environment.Curses
{
    internal interface ICursedObject
    {
        CurseTypeEnum ObjectCurseType { get; set; }
    }
}
