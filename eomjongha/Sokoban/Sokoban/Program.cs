using System;
class Program

{
    enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down

    }
    static void Main()
    {
        // 초기 세팅
        Console.ResetColor();                               // 컬러를 초기화한다.
        Console.CursorVisible = false;                      // 커서를 숨긴다.
        Console.Title = "홍성재의 파이어펀치";               // 타이틀을 설정한다.
        Console.BackgroundColor = ConsoleColor.DarkRed;     // 배경색을 설정한다.
        Console.ForegroundColor = ConsoleColor.Yellow;      // 글꼴색을 설정한다.
        Console.Clear();                                    // 출력된 모든 내용을 지운다.

        // 기호 상수 정의
        // 맵의 가로 범위, 세로 범위
        const int MAP_MIN_X = 0;
        const int MAP_MIN_Y = 0;
        const int MAP_MAX_X = 15;
        const int MAP_MAX_Y = 10;

        // 플레이어의 초기 좌표
        const int INITIAL_PLAYER_X = 0;
        const int INITIAL_PLAYER_Y = 0;
        // 플레이어의 기호(string literal)
        const string PLAYER_STRING = "P";

        // 박스의 초기 좌표
        const int INITIAL_BOX_X = 5;
        const int INITIAL_BOX_Y = 5;
        const int INITIAL_BOX1_X = 3;
        const int INITIAL_BOX1_Y = 3;
        const int INITIAL_BOX2_X = 9;
        const int INITIAL_BOX2_Y = 4;

        // 박스의 기호(string literal)
        const string BOX_STRING = "B";
        const string BOX1_STRING = "B";
        const string BOX2_STRING = "B";


        //벽의 초기 좌표
        const int INITIAL_WALL_X = 7;
        const int INITIAL_WALL_Y = 8;
        const string WALL_STRING = "@";
        int wallX = INITIAL_WALL_X;
        int wallY = INITIAL_WALL_Y;


        // 플레이어 좌표 설정
        int playerX = INITIAL_PLAYER_X;
        int playerY = INITIAL_PLAYER_Y;
        Direction playerDirection = Direction.None;

        // 박스 좌표 설정
        int boxX = INITIAL_BOX_X;
        int boxY = INITIAL_BOX_Y;
        int box1X = INITIAL_BOX1_X;
        int box1Y = INITIAL_BOX1_Y;
        int box2X = INITIAL_BOX2_X;
        int box2Y = INITIAL_BOX2_Y;


        //goal 좌표설정


        //goal
        const int INITAL_GOAL_X = 13;
        const int INITAL_GOAL_Y = 9;
        const string GOAL_STRING = "#";

        int GoalX = INITAL_GOAL_X;
        int GoalY = INITAL_GOAL_Y;


        // 가로 15 세로 10
        // 게임 루프 == 프레임(Frame)
        while (true)
        {
            // 이전 프레임을 지운다.
            Console.Clear();

            // --------------------------------- Render -----------------------------------------
            // 플레이어 출력하기
            Console.SetCursorPosition(playerX, playerY);
            Console.Write(PLAYER_STRING);

            // 박스 출력하기
            Console.SetCursorPosition(boxX, boxY);
            Console.Write(BOX_STRING);
            Console.SetCursorPosition(box1X, box1Y);
            Console.Write(BOX1_STRING);
            Console.SetCursorPosition(box2X, box2Y);
            Console.Write(BOX2_STRING);

            Console.SetCursorPosition(INITIAL_WALL_X, INITIAL_WALL_Y);
            Console.Write(WALL_STRING);

            // 골인 지점 출력하기
            Console.SetCursorPosition(GoalX, GoalY);
            Console.Write(GOAL_STRING);

            // --------------------------------- ProcessInput -----------------------------------------
            ConsoleKey key = Console.ReadKey().Key;

            // --------------------------------- Update -----------------------------------------
            // 플레이어 업데이트
            // 왼쪽 화살표 키를 눌렀을 떄
            if (key == ConsoleKey.LeftArrow)
            {
                // 왼쪽으로 이동
                playerX = Math.Max(MAP_MIN_X, playerX - 1);
                playerDirection = Direction.Left;
            }

            // 오른쪽 화살표 키를 눌렀을 때
            if (key == ConsoleKey.RightArrow)
            {
                playerX = Math.Min(playerX + 1, MAP_MAX_X);
                playerDirection = Direction.Right;
            }

            // 위쪽 화살표 키를 눌렀을 때
            if (key == ConsoleKey.UpArrow)
            {
                // 위로 이동
                playerY = Math.Max(MAP_MIN_Y, playerY - 1);
                playerDirection = Direction.Up;
            }

            // 아래쪽 화살표 키를 눌렀을 때
            if (key == ConsoleKey.DownArrow)
            {
                // 아래로 이동
                playerY = Math.Min(playerY + 1, MAP_MAX_Y);
                playerDirection = Direction.Down;
            }

            // 박스 업데이트
            // 플레이어가 이동한 후
            if (playerX == boxX && playerY == boxY) // 플레이어가 이동하고나니 박스가 있네?
            {
                // 박스를 움직여주면 됨.
                switch (playerDirection)
                {
                    case Direction.Left:
                        if (boxX == MAP_MIN_X)
                        {
                            playerX = MAP_MIN_X + 1;
                        }
                        else
                        {
                            boxX = boxX - 1;
                        }
                        break;
                    case Direction.Right:
                        if (boxX == MAP_MAX_X)
                        {
                            playerX = MAP_MAX_X - 1;
                        }
                        else
                        {
                            boxX = boxX + 1;
                        }
                        break;
                    case Direction.Up:
                        if (boxY == MAP_MIN_Y)
                        {
                            playerY = MAP_MIN_Y + 1;
                        }
                        else
                        {
                            boxY = boxY - 1;
                        }
                        break;
                    case Direction.Down:
                        if (boxY == MAP_MAX_Y)
                        {
                            playerY = MAP_MAX_Y - 1;
                        }
                        else
                        {
                            boxY = boxY + 1;
                        }
                        break;
                    default: //
                        Console.Clear();
                        Console.WriteLine($"[Error] 플레이어의 이동 방향이 잘못되었습니다. {playerDirection}");

                        return; // 프로그램 종료
                }

            }
            if (playerX == wallX && playerY == wallY)
            {
                switch(playerDirection)
                {
                    case Direction.Left:
                        playerX = playerX + 1;
                        break;
                    case Direction.Right:
                        playerX = playerX - 1;
                        break;
                    case Direction.Up:
                        playerY = playerY + 1;
                        break;
                    case Direction.Down:
                        playerY = playerY - 1;
                        break;

                    
                }
            }

            //박스의 경우
            if(boxX== wallX && boxY== wallY)
            {
                switch (playerDirection)
                {
                    case Direction.Left:
                        ++boxX;
                        ++playerX;
                        break;
                    case Direction.Right:
                        --boxX;
                        --playerX;
                        break;
                    case Direction.Up:
                        ++boxY;
                        ++playerY;
                        break;
                    case Direction.Down:
                        --boxY;
                        --playerY;
                        break;
                }

            }
            if (boxX==GoalX && boxY == GoalY )
            {
                goto LOOP_EXIT;
            }

        }
    LOOP_EXIT:
        System.Console.WriteLine("루프 끝");

    }
}



