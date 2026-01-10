using Assets.Scripts.Audio;
using Assets.Scripts.Data.Enums;
using Assets.Scripts.Data.Scriptables.Actions.Implementations;
using UnityEngine;

namespace Assets.Scripts.Environment.Curses
{
    public class CurseAudioManager: MonoBehaviour
    {
        [SerializeField] private ActionCurseEnumScriptable startCurseScriptable, stopCurseScriptable;
        [SerializeField] private MusicPlayer musicPlayer;
        [SerializeField] private int curseCountBeforePlay = 1;
        

        // TODO: This will likely be based on how spooked inhabitants are instead of active curses
        private int activeCurses = 0;

        private void Start()
        {
            startCurseScriptable.AddAction(HandleCurseStart);
            stopCurseScriptable.AddAction(HandleCurseStop);
        }

        private void HandleCurseStart(CurseTypeEnum curseType)
        {
            activeCurses += 1; ;

            if (activeCurses >= curseCountBeforePlay)            
                musicPlayer.HandleThematicRise();
        }

        private void HandleCurseStop(CurseTypeEnum curseType)
        {
            activeCurses -= 1;

            if (activeCurses < curseCountBeforePlay)
            {
                musicPlayer.HandleThematicFall();
            }
        }
    }
}
