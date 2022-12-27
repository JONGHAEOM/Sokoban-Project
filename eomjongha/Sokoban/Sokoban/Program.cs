﻿//초기 세팅
Console.ResetColor();                                       //컬러를 초기화
Console.CursorVisible = false;                             // 커서를 숨긴다
Console.Title = "홍성재의 파이어펀치";                      //타이틀을 설정한다
Console.BackgroundColor = ConsoleColor.DarkRed;
Console.ForegroundColor = ConsoleColor.Cyan;
Console.Clear();


int playerX = 0;

int playerY = 0;

int boxX = 5;
int boxY = 5;


//rkfh 15 tpfh 10


//게임 루프== 프레임(frame)
while (true)
{
    Console.Clear();
    //--------------------------------------------render
    //플레이어 출력- 커서를 이동한다음에 출력합니다
    
    Console.SetCursorPosition(playerX, playerY);
    Console.Write("P");
    Console.SetCursorPosition(boxX, boxY);
    Console.Write("B");
    


    //------------------------------Processinput

    ConsoleKey Key = Console.ReadKey() . Key;



    // -----------------------------Update
    // 오른쪽 화살표키를 눌렀을때

    
    if (Key == ConsoleKey.RightArrow)
    {
       
        playerX = Math.Min(playerX + 1, 30);
        {
            if(playerX + 1 ==boxX && playerY ==boxY )
            {
                boxX += 1;
            }
        }
    }
    else if(Key == ConsoleKey.LeftArrow)
    {
        playerX = Math.Max(playerX - 1, 0);
        if (playerX - 1 == boxX && playerY == boxY)
        {
            boxX -= 1;
        }
    }
    else if(Key == ConsoleKey.DownArrow)
    {
        playerY = Math.Min(playerY + 1, 30);

        if (playerX  == boxX && playerY + 1 == boxY)
        {
            boxY += 1;
        }
    }
    else if(Key == ConsoleKey.UpArrow)
    {
        playerY = Math.Max(playerY - 1, 0);
        if (playerX == boxX && playerY - 1 == boxY)
        {
            boxY -= 1;
        }
    }    
}




