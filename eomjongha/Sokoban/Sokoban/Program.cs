using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }
    class Sokoban
    {
        static void Main()
        {
            // 초기 세팅 ( 어떤 게임이던지 이 과정을 거친다.)
            // 컬러를 초기화 하는 것 ( 영영 프롬프트에서 실행 시키는 게임-콘솔게임)
            Console.ResetColor(); // 색을 초기화
            Console.CursorVisible = false; // 커서를 숨긴다
            Console.Title = "My Sokoban"; // 타이틀을 설정한다'
            Console.BackgroundColor = ConsoleColor.Green; // 배경색을 설정한다
            Console.ForegroundColor = ConsoleColor.Magenta; // 글꼴의 색을 설정한다
            Console.Clear();



            const int GOAL_COUNT = 2;


            int playerX = 0;
            int playerY = 0;

            const int Max_X = 20;
            const int Max_Y = 10;
            const int Min_X = 0;
            const int Min_Y = 0;

            Direction playerMoveDirection = Direction.None;
            //박스의 위치
            int[] boxPositionsX = { 5, 3 };
            int[] boxPositionsY = { 5, 3 };

            // 플레이어가 무슨 박스를 밀고 있는지 저장하기 위한 변수

            int pushedBoxId = 0; // 1이면 박스1, 2면 박스2

            //벽을 만들자
            int[] wallPositionsX = { 7, 10 };
            int[] wallPositionsY = { 7, 8 };

            int WALL_COUNT = wallPositionsX.Length;
            //골을 그리자
            int[] goalPositionsX = { 9, 6 };
            int[] goalPositionsY = { 9, 6 };


            //박스가 골 위에 있즌지를 저장하기위한 변수
            //bool [] 만들어서 판단          
            bool[] isBoxOnGoal = new bool[boxPositionsX.Length];



            while (true)
            {
                // Render
                // 이전 프레임을 지우는 것 이 루프 한번이 한 화면을 그리기 위한 과정 
                Console.Clear();


                // 플레이어를 그린다. 먼저 커서를 옮기고 
                Console.SetCursorPosition(playerX, playerY);
                Console.Write("P");

                Randerobject(playerX, playerY, "P");

                //골을 그려준다
                for (int goalId = 0; goalId < GOAL_COUNT; ++goalId)
                {
                    Randerobject(goalPositionsX[goalId], goalPositionsY[goalId], "G");
                }

                //박스를 그려준다 
                for (int i = 0; i < boxPositionsX.Length; ++i)
                {
                    string boxsape = isBoxOnGoal[i] ? "O" : "B";
                    Randerobject(boxPositionsX[i], boxPositionsY[i], boxsape);
                   
                }
                //박스를 그릴때 판단하던 것을 골위에 박스가 존재하느지 체크할 떄 판단
                //판단지점이 다름 -> 데이터 저장 박스를 그릴때 어떤 boㅐl 객체를 생성 =>배열로전환

                //벽을 그려준다
                for (int wallId = 0; wallId < WALL_COUNT; ++wallId)
                {
                    Randerobject(wallPositionsX[wallId], wallPositionsY[wallId], "W");
                }

                // ProcessInput
                ConsoleKey Key = Console.ReadKey().Key;

                // Update

                if (Key == ConsoleKey.LeftArrow)
                {
                    playerX = Max(Min_X, playerX - 1);
                    playerMoveDirection = Direction.Left;
                }
                if (Key == ConsoleKey.RightArrow)
                {
                    playerX = Min(Max_X, playerX + 1);
                    playerMoveDirection = Direction.Right;
                }
                if (Key == ConsoleKey.UpArrow)
                {
                    playerY = Max(Min_Y, playerY - 1);
                    playerMoveDirection = Direction.Up;
                }
                if (Key == ConsoleKey.DownArrow)
                {
                    playerY = Min(Max_Y, playerY + 1);
                    playerMoveDirection = Direction.Down;
                }
                // Math.Max 와 Min을 사용하는 이유는 내가 정한 공간에서 벗어날때를 위한것
                //박스 옮기는 식 => 박스-> 박스의 좌표를 바꾼다.

                for (int i = 0; i < WALL_COUNT; ++i)
                {
                    int wallX = wallPositionsX[i];
                    int wallY = wallPositionsY[i];

                    if (playerX == wallX && playerY == wallY)
                        switch (playerMoveDirection)
                        {
                            case Direction.Left:
                                playerX = wallX + 1;
                                break;
                            case Direction.Right:
                                playerX = wallX - 1;
                                break;
                            case Direction.Up:
                                playerY = wallY + 1;
                                break;
                            case Direction.Down:
                                playerY = wallY - 1;
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine($"[Error] 플레이오 이동 방향 데이터가 오류입니다.: {playerMoveDirection}");

                                break;
                        }

                }


                //플레이어가 박스에 닿았을때 --일단 기술부채 1월 18일 상환 
                for (int i = 0; i < boxPositionsX.Length; ++i)
                {
                    if (playerX != boxPositionsX[i] || playerY != boxPositionsY[i])
                    {
                        continue;
                    }
                    if (boxPositionsX[i] == playerX && boxPositionsY[i] == playerY)
                    {
                        switch (playerMoveDirection)
                        {
                            case Direction.Left:
                                boxPositionsX[i] = Max(Min_X, boxPositionsX[i] - 1);
                                playerX = boxPositionsX[i] + 1;
                                break;


                            case Direction.Right:
                                boxPositionsX[i] = Min(boxPositionsX[i] + 1, Max_X);
                                playerX = boxPositionsX[i] - 1;
                                break;


                            case Direction.Up:
                                boxPositionsY[i] = Max(Min_Y, boxPositionsY[i] - 1);
                                playerY = boxPositionsY[i] + 1;
                                break;
                            case Direction.Down:
                                boxPositionsY[i] = Min(boxPositionsY[i] + 1, Max_Y);
                                playerY = boxPositionsY[i] - 1;
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine($"[Error] 플레이오 이동 방향 데이터가 오류입니다.: {playerMoveDirection}");

                                break;
                        }
                        pushedBoxId = i;

                    }
                    boxPositionsX[i] = boxPositionsX[i];
                    boxPositionsY[i] = boxPositionsY[i];
                    //swith문 사용가능
                    // 박스가 벽에 닿았을때 막히는 부분

                }



                // 박스가 벽에 닿았을때   -- 기술부채 1월 18일 상환
                for (int i = 0; i < WALL_COUNT; ++i)
                {
                    if (boxPositionsX[pushedBoxId] != wallPositionsX[i] || boxPositionsY[pushedBoxId] != wallPositionsY[i])
                    {
                        continue;
                    }
                    else
                    {
                        switch (playerMoveDirection)
                        {
                            case Direction.Left:
                                boxPositionsX[pushedBoxId] = wallPositionsX[i] + 1;
                                playerX = boxPositionsX[pushedBoxId] + 1;
                                break;
                            case Direction.Right:
                                boxPositionsX[pushedBoxId] = wallPositionsX[i] - 1;
                                playerX = boxPositionsX[pushedBoxId] - 1;
                                break;
                            case Direction.Up:
                                boxPositionsY[pushedBoxId] = wallPositionsY[i] + 1;
                                playerY = boxPositionsY[pushedBoxId] + 1;
                                break;
                            case Direction.Down:
                                boxPositionsY[pushedBoxId] = wallPositionsY[i] - 1;
                                playerY = boxPositionsY[pushedBoxId] - 1;
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine($"[Error] 플레이오 이동 방향 데이터가 오류입니다.: {playerMoveDirection}");

                                return;


                        }
                        break;


                    }


                }






                // 박스 1을 밀어서 박스 2에 닿은 건지, 박스 2를 밀어서 박스 1에 닿은건지?
                for (int collidedBoxId = 0; collidedBoxId < boxPositionsX.Length; ++collidedBoxId)
                {
                    if (pushedBoxId == collidedBoxId)
                    {
                        continue;
                    }
                    else if (boxPositionsX[pushedBoxId] == boxPositionsX[collidedBoxId] && boxPositionsY[pushedBoxId] == boxPositionsY[collidedBoxId])
                    {
                        switch (playerMoveDirection)
                        {
                            case Direction.Left:
                                boxPositionsX[pushedBoxId] = boxPositionsX[collidedBoxId] + 1;
                                playerX = boxPositionsX[pushedBoxId] + 1;
                                break;
                            case Direction.Right:
                                boxPositionsX[pushedBoxId] = boxPositionsX[collidedBoxId] - 1;
                                playerX = boxPositionsX[pushedBoxId] - 1;
                                break;
                            case Direction.Up:
                                boxPositionsY[pushedBoxId] = boxPositionsY[collidedBoxId] + 1;
                                playerY = boxPositionsY[pushedBoxId] + 1;
                                break;
                            case Direction.Down:
                                boxPositionsY[pushedBoxId] = boxPositionsY[collidedBoxId] - 1;
                                playerY = boxPositionsY[pushedBoxId] - 1;
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine($"[Error] 플레이어 이동 방향 데이터가 오류입니다. : {playerMoveDirection}");

                                return;
                        }
                        break;
                    }
                }

                // 박스와 골의 처리
                int boxOnGaolCount = 0;

                //골지점에 박스에 존재하나?
                for (int goalId = 0; goalId < GOAL_COUNT; goalId++)   // 모든 골 지점에 대해서
                {
                    //박스가 있는지 체크한다
                    for (int boxId = 0; boxId < boxPositionsX.Length; boxId++)   // 모든 박스에 대해서
                    {
                        //현재 박스가 goal위에 있는지 체크
                        isBoxOnGoal[boxId] = false;
                        //박스가 있는지 체크한다.
                        if (boxPositionsX[boxId] == goalPositionsX[goalId] && boxPositionsY[boxId] == goalPositionsY[goalId])
                        {
                            ++boxOnGaolCount;
                            isBoxOnGoal[boxId] = true;  //박스가 골위에 있다는 사실을 저장 해둔다

                            break;
                        }
                    }
                }
                int Max (int a, int b)
                {
                    int result = (a > b) ? a : b;

                    return result;
                }
                int Min (int a , int b)
                {
                    int result= (a < b) ? a : b;
                    return result;
                }
                int VariadicMax(params int[] numbers)
                {
                    int result = numbers[0];
                    for (int i =0; i< numbers.Length; ++i)
                    {
                        if (result < numbers[i])
                        {
                            result= numbers[i];
                        }
                    }
                    return result;
                }
                void Randerobject(int x, int y ,string obj)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(obj);
                }


                    if (boxOnGaolCount == GOAL_COUNT)
                {
                    Console.Clear();
                    Console.WriteLine("Clear");
                    break;
                }


                //박스포지션과 골포지션을 비교했을 때처럼
            }
        }
    }
}
