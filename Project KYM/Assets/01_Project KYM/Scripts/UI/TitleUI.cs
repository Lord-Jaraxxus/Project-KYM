using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM 
{
    public class TitleUI : UIBase
    {
        public void OnClickStartButton() 
        {
            Main.Singleton.ChangeScene(SceneType.Ingame); // 게임 시작 버튼 클릭 시 Ingame 씬으로 변경
        }
        public void OnClickQuitButton() 
        {
            Main.Singleton.SystemQuit(); // 게임 종료 버튼 클릭 시 게임 종료
        }
    }
}
