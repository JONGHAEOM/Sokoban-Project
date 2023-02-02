﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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


            int x = 3;
            int y = 3;

            const int Max_X = 70;
            const int Max_Y = 20;
            const int Min_X = 1;
            const int Min_Y = 1;

            Direction moveDirection = Direction.None;
            //박스의 위치
            int[] boxPositionsX = { 5, 3 };
            int[] boxPositionsY = { 5, 8 };

            // 플레이어가 무슨 박스를 밀고 있는지 저장하기 위한 변수

            int pushedBoxId = 0; // 1이면 박스1, 2면 박스2

            //벽을 만들자
            int[] wallPositionsX = { 7, 10, 23, 13, 40, 30, 60, 40, 30 };
            int[] wallPositionsY = { 7, 8, 17, 12, 10, 5, 17, 16, 15 };

            int WALL_COUNT = wallPositionsX.Length;
            //골을 그리자
            int[] goalPositionsX = { 9, 6 };
            int[] goalPositionsY = { 9, 6 };


            //박스가 골 위에 있즌지를 저장하기위한 변수
            //bool [] 만들어서 판단          
            bool[] isBoxOnGoal = new bool[boxPositionsX.Length];



            while (true)
            {
                Rander();

                // ProcessInput
                ConsoleKey key = Console.ReadKey().Key;

                // Update
                MovePlayer(key, ref x, ref y, ref moveDirection);
                Update();



                // 박스와 골의 처리
                int boxOnGaolCount = 0;
                int boxOnGoalCount = CountBoxOnGoal(in boxPositionsX, in boxPositionsY, ref isBoxOnGoal, in goalPositionsX, in goalPositionsY);



                int CountBoxOnGoal(in int[] boxPositionX, in int[] boxPositionY, ref bool[] isBoxOnGoal, in int[] goalPositionsX, in int[] goalPositionsY)
                {
                    int boxCount = boxPositionsX.Length;
                    int goalCount = goalPositionsX.Length;

                    int result = 0;
                    for (int boxId = 0; boxId < boxCount; ++boxId)
                    {
                        isBoxOnGoal[boxId] = false;

                        for (int goalld = 0; goalld < goalCount; ++goalld)
                        {
                            if (IsCollided(boxPositionsX[boxId], boxPositionsY[boxId], goalPositionsX[goalld], goalPositionsY[goalld]))
                            {
                                ++result;

                                isBoxOnGoal[boxId] = true;

                                break;
                            }
                        }
                    }

                    return result;

                }
                //골지점에 박스에 존재하나?
                for (int goalId = 0; goalId < GOAL_COUNT; goalId++)   // 모든 골 지점에 대해서
                {

                    isBoxOnGoal[goalId] = false;
                    //박스가 있는지 체크한다
                    for (int boxId = 0; boxId < boxPositionsX.Length; boxId++)   // 모든 박스에 대해서
                    {
                        //박스가 있는지 체크한다.
                        if (IsCollided(boxPositionsX[boxId], boxPositionsY[boxId], goalPositionsX[goalId], goalPositionsY[goalId]))
                        {
                            ++boxOnGaolCount;
                            isBoxOnGoal[boxId] = true;  //박스가 골위에 있다는 사실을 저장 해둔다

                            break;
                        }
                    }
                }

                if (boxOnGoalCount == GOAL_COUNT)
                {
                    Console.Clear();
                    Console.WriteLine("Clear");
                    break;
                }


                int Max(int a, int b)
                {
                    int result = (a > b) ? a : b;

                    return result;
                }
                int Min(int a, int b)
                {
                    int result = (a < b) ? a : b;
                    return result;
                }
                int VariadicMax(params int[] numbers)
                {
                    int result = numbers[0];
                    for (int i = 0; i < numbers.Length; ++i)
                    {
                        if (result < numbers[i])
                        {
                            result = numbers[i];
                        }
                    }
                    return result;
                }
                void Randerobject(int x, int y, string obj)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(obj);
                }
                void Rander()
                {
                    Console.Clear();
                    Randerobject(x, y, "P");
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
                    //테두리를 그려준다.
                    for (int i = 1; i < 21; ++i)
                    {
                        Randerobject(0, i, "X");
                    }
                    for (int i = 1; i < 21; ++i)
                    {
                        Randerobject(71, i, "X");
                    }
                    for (int i = 0; i < 72; ++i)
                    {
                        Randerobject(i, 0, "X");
                    }
                    for (int i = 0; i < 72; ++i)
                    {
                        Randerobject(i, 21, "X");
                    }


                    //벽을 그려준다
                    for (int wallId = 0; wallId < WALL_COUNT; ++wallId)
                    {
                        Randerobject(wallPositionsX[wallId], wallPositionsY[wallId], "W");
                    }
                }

                bool IsCollided(int x1, int y1, int x2, int y2)
                {
                    if (x1 == x2 && y1 == y2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                //Move to 시리즈 
                void MoveToLeftOfTarget(out int x, in int target) => x = Math.Max(Min_X, target - 1);
                void MoveToRightOfTarget(out int x, in int target) => x = Math.Min(target + 1, Max_X);
                void MoveToUpOfTarget(out int y, in int target) => y = Math.Max(Min_Y, target - 1);
                void MoveToDownOfTarget(out int y, in int target) => y = Math.Min(target + 1, Max_Y);

                void OnCollision(Action action)
                {
                    action();
                }

                void PushOut(Direction moveDirection, ref int objX, ref int objY, in int collidedObjX, in int collidedObjY)
                {
                    switch (moveDirection)
                    {
                        case Direction.Left:
                            MoveToRightOfTarget(out objX, in collidedObjX);
                            break;
                        case Direction.Right:
                            MoveToLeftOfTarget(out objX, in collidedObjX);
                            break;
                        case Direction.Up:
                            MoveToDownOfTarget(out objY, in collidedObjY);
                            break;
                        case Direction.Down:
                            MoveToUpOfTarget(out objY, in collidedObjY);
                            break;
                    }
                }

                void MoveBox(Direction moveDirection, ref int boxPositionsX, ref int boxPositionsY, in int x, in int y)
                {
                    switch (moveDirection)
                    {
                        case Direction.Left:
                            MoveToLeftOfTarget(out boxPositionsX, in x);

                            break;
                        case Direction.Right:
                            MoveToRightOfTarget(out boxPositionsX, in x);

                            break;
                        case Direction.Up:
                            MoveToUpOfTarget(out boxPositionsY, in y);

                            break;
                        case Direction.Down:
                            MoveToDownOfTarget(out boxPositionsY, in y);

                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine($"[Error] 플레이오 이동 방향 데이터가 오류입니다.: {moveDirection}");

                            break;
                    }
                }




                void MovePlayer(ConsoleKey key, ref int x, ref int y, ref Direction moveDirection)
                {
                    if (key == ConsoleKey.LeftArrow)
                    {
                        x = Max(Min_X, x - 1);
                        moveDirection = Direction.Left;
                    }
                    if (key == ConsoleKey.RightArrow)
                    {
                        x = Min(Max_X, x + 1);
                        moveDirection = Direction.Right;
                    }
                    if (key == ConsoleKey.UpArrow)
                    {
                        y = Max(Min_Y, y - 1);
                        moveDirection = Direction.Up;
                    }
                    if (key == ConsoleKey.DownArrow)
                    {
                        y = Min(Max_Y, y + 1);
                        moveDirection = Direction.Down;
                    }
                }
                void Update()
                {
                    for (int wallId = 0; wallId < WALL_COUNT; ++wallId)
                    {
                        if (false == IsCollided(x, y, wallPositionsX[wallId], wallPositionsY[wallId]))
                        {
                            continue;
                        }
                        OnCollision(() =>
                        {
                            PushOut(moveDirection, ref x, ref y, wallPositionsX[wallId], wallPositionsY[wallId]);

                        });
                    }


                    //플레이어가 박스에 닿았을때 --일단 기술부채 1월 18일 상환 
                    for (int i = 0; i < boxPositionsX.Length; ++i)
                    {
                        if (false == IsCollided(x, y, boxPositionsX[i], boxPositionsY[i]))
                        {
                            continue;
                        }
                        switch (moveDirection)
                        {
                            case Direction.Left:
                                MoveToLeftOfTarget(out boxPositionsX[i], in x);

                                break;
                            case Direction.Right:
                                MoveToRightOfTarget(out boxPositionsX[i], in x);

                                break;
                            case Direction.Up:
                                MoveToUpOfTarget(out boxPositionsY[i], in y);

                                break;
                            case Direction.Down:
                                MoveToDownOfTarget(out boxPositionsY[i], in y);

                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine($"[Error] 플레이오 이동 방향 데이터가 오류입니다.: {moveDirection}");

                                break;
                        }
                        pushedBoxId = i;
                        boxPositionsX[i] = boxPositionsX[i];
                        boxPositionsY[i] = boxPositionsY[i];
                    }


                    //swith문 사용가능
                    // 박스가 벽에 닿았을때 막히는 부분





                    // 박스가 벽에 닿았을때   -- 기술부채 1월 18일 상환
                    for (int i = 0; i < WALL_COUNT; ++i)
                    {
                        if (false == IsCollided(boxPositionsX[pushedBoxId], boxPositionsY[pushedBoxId], wallPositionsX[i], wallPositionsY[i]))
                        {
                            continue;
                        }
                        OnCollision(() =>
                        {
                            PushOut(moveDirection, ref boxPositionsX[pushedBoxId], ref boxPositionsY[pushedBoxId], wallPositionsX[i], wallPositionsY[i]);

                            PushOut(moveDirection, ref x, ref y, boxPositionsX[pushedBoxId], boxPositionsY[pushedBoxId]);
                        });
                    }
                    // 박스 1을 밀어서 박스 2에 닿은 건지, 박스 2를 밀어서 박스 1에 닿은건지?
                    for (int i = 0; i < boxPositionsX.Length; i++)
                    {
                        //같은 박스라면 처리할 필요가 없다
                        if (pushedBoxId == i)
                        {
                            continue;
                        }

                        if (false == IsCollided(boxPositionsX[pushedBoxId], boxPositionsY[pushedBoxId], boxPositionsX[i], boxPositionsY[i]))
                        {
                            continue;
                        }

                        OnCollision(() =>
                        {
                            PushOut(moveDirection, ref boxPositionsX[pushedBoxId], ref boxPositionsY[pushedBoxId], boxPositionsX[i], boxPositionsY[i]);
                            PushOut(moveDirection, ref x, ref y, boxPositionsX[pushedBoxId], boxPositionsY[pushedBoxId]);

                        });



                    }
                }
            }
        }
    }
}

