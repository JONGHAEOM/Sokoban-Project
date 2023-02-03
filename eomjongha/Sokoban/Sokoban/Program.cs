using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{

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

            //기호 상수 정의 
            const int Max_X = 70;
            const int Max_Y = 20;
            const int Min_X = 1;
            const int Min_Y = 1;

            // 플레이어 위치 좌표 
            // int playerX = 0;
            // int playerY = 0;
            Player player = new Player();

            Box[] boxes = new Box[2]
            {
                new Box{X=9, Y=9,BoxInGoal=false},
                new Box{X=11, Y=11,BoxInGoal=false}
            };

            // 플레이어가 무슨 박스를 밀고 있는지 저장하기 위한 변수
            int pushedBoxId = 0; // 1이면 박스1, 2면 박스2

            //벽을 만들자
            Wall[] walls = new Wall[2]
            {
                new Wall{X=6,Y=6},
                new Wall{X=10,Y=10}
            };


            //골을 그리자
            Goal[] goals = new Goal[2]
            {
                new Goal{X=6,Y=8},
                new Goal{X=9,Y=12}
            };

            while (true)
            {

                Rander();

                // ProcessInput
                ConsoleKey key = Console.ReadKey().Key;

                // Update
                MovePlayer(key, player);

                Update();

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

            void PushOut(Direction direction, ref int objX, ref int objY, in int collidedObjX, in int collidedObjY)
            {
                switch (player.MoveDirection)
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

            void MoveBox(Direction direction, Box box, Player player)
            {
                switch (player.MoveDirection)
                {
                    case Direction.Left:
                        MoveToLeftOfTarget(out box.X, in player.X);

                        break;
                    case Direction.Right:
                        MoveToRightOfTarget(out box.X, in player.X);

                        break;
                    case Direction.Up:
                        MoveToUpOfTarget(out box.Y, in player.Y);

                        break;
                    case Direction.Down:
                        MoveToDownOfTarget(out box.Y, in player.Y);

                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine($"[Error] 플레이오 이동 방향 데이터가 오류입니다.: {player.MoveDirection}");

                        break;
                }
            }

            void MovePlayer(ConsoleKey key, Player player)
            {
                if (key == ConsoleKey.LeftArrow)
                {
                    MoveToLeftOfTarget(out player.X, in player.X);
                    player.MoveDirection = Direction.Left;
                }
                if (key == ConsoleKey.RightArrow)
                {
                    MoveToRightOfTarget(out player.X, in player.X);
                    player.MoveDirection = Direction.Right;
                }
                if (key == ConsoleKey.UpArrow)
                {
                    MoveToUpOfTarget(out player.Y, in player.Y);
                    player.MoveDirection = Direction.Up;
                }
                if (key == ConsoleKey.DownArrow)
                {
                    MoveToDownOfTarget(out player.Y, in player.Y);
                    player.MoveDirection = Direction.Down;
                }
            }

            int CountBoxOnGoal(Box[] boxes, Goal[] goals)
            {
                int boxCount = boxes.Length;
                int goalCount = goals.Length;

                int result = 0;
                for (int boxId = 0; boxId < boxCount; ++boxId)
                {
                    boxes[boxId].BoxInGoal = false;

                    for (int goalld = 0; goalld < goalCount; ++goalld)
                    {
                        if (IsCollided(boxes[boxId].X, boxes[boxId].Y, goals[goalld].X, goals[goalld].Y))
                        {
                            ++result;

                            boxes[boxId].BoxInGoal = true;

                            break;
                        }
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
                Randerobject(player.X, player.Y, "P");
                //골을 그려준다
                int goalCount = goals.Length;
                for (int i = 0; i < goalCount; i++)
                {
                    Randerobject(goals[i].X, goals[i].Y, "G");
                }

                //박스를 그려준다 
                int boxCount = boxes.Length;
                for (int i = 0; i < boxCount; i++)
                {
                    string boxIcon = boxes[i].BoxInGoal ? "O" : "B";
                    Randerobject(boxes[i].X, boxes[i].Y, boxIcon);
                }
                //테두리
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
                int wallCount = walls.Length;
                for (int i = 0; i < wallCount; ++i)
                {
                    Randerobject(walls[i].X, walls[i].Y, "W");
                }
            }

            void Update()
            {
                // 플레이어가 벽에 닿았을때 
                int wallCount = walls.Length;
                for (int wallId = 0; wallId < wallCount; ++wallId)
                {
                    if (false == IsCollided(player.X, player.Y, walls[wallId].X, walls[wallId].Y))
                    {
                        continue;
                    }
                    OnCollision(() =>
                    {
                        PushOut(player.MoveDirection, ref player.X, ref player.Y, walls[wallId].X, walls[wallId].Y);
                    });
                }


                //플레이어가 박스에 닿았을때 
                int boxCount = boxes.Length;
                for (int i = 0; i < boxCount; ++i)
                {
                    if (false == IsCollided(player.X, player.Y, boxes[i].X, boxes[i].Y))
                    {
                        continue;
                    }
                    OnCollision(() =>
                    {
                        MoveBox(player.MoveDirection, boxes[i], player);
                    });
                    pushedBoxId = i;
                    break;
                }

                // 박스가 벽에 닿았을때  
                for (int i = 0; i < wallCount; ++i)
                {
                    if (false == IsCollided(boxes[pushedBoxId].X, boxes[pushedBoxId].Y, walls[i].X, walls[i].Y))
                    {
                        continue;
                    }
                    OnCollision(() =>
                    {
                        PushOut(player.MoveDirection, ref boxes[pushedBoxId].X, ref boxes[pushedBoxId].Y, walls[i].X, walls[i].Y);
                        PushOut(player.MoveDirection, ref player.X, ref player.Y, boxes[pushedBoxId].X, boxes[pushedBoxId].Y);
                    });
                }
                // 박스 1을 밀어서 박스 2에 닿은 건지, 박스 2를 밀어서 박스 1에 닿은건지?
                for (int i = 0; i < boxCount; i++)
                {
                    //같은 박스라면 처리할 필요가 없다
                    if (pushedBoxId == i)
                    {
                        continue;
                    }

                    //충돌이 없다면 처리할 필요 없다
                    else if (false == IsCollided(boxes[pushedBoxId].X, boxes[pushedBoxId].Y, boxes[i].X, boxes[i].Y))
                    {
                        continue;
                    }
                    else
                    {
                        OnCollision(() =>
                        {
                            PushOut(player.MoveDirection, ref boxes[pushedBoxId].X, ref boxes[pushedBoxId].Y, boxes[i].X, boxes[i].Y);
                            PushOut(player.MoveDirection, ref player.X, ref player.Y, boxes[pushedBoxId].X, boxes[pushedBoxId].Y);
                        });
                    }
                }
            }
        }
    }
}



