using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            int columnLength = Intro();
            
            int[,] field = DrawField((columnLength + 2), (int)(columnLength * 1.5 + 2));
            GamePlay(field);
        }

        static int[,] DrawField(int rowLength, int columnLength)
        {
            int[,] field = new int[rowLength, columnLength];

            int rowCount = 1;
            int columnCount = 1;

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < columnLength - 1; j++)
                {
                    if(j == 0)
                    {
                        if (i < rowLength - 2)
                        {
                            field[i, j] = columnCount;
                            columnCount++;
                        }
                    }
                    else if (i == rowLength-1)
                    {
                        field[i, j] = rowCount;
                        rowCount++;
                    }
                    else if(i == rowLength - 2)
                    {

                    }
                    else
                    {
                        field[i, j] = 0;
                    }
                }
            }

            ShowField(field);

            return field;
        }

        static int Intro()
        {
            int columnLength;

            Console.WriteLine("Число ячеек по вертикали");
            for (; ; )
            {
                Console.WriteLine("Введите число от 20 до 30");
                bool columnLengthBool = int.TryParse(Console.ReadLine(), out columnLength);
                if (!columnLengthBool)
                {
                    Console.WriteLine("Ведите число");
                }
                else if (columnLength < 20 || columnLength > 30)
                    Console.WriteLine("Введите число между 20 и 30");
                else break;
            }

            Console.WriteLine();
            return columnLength;
        }

        static void GamePlay(int[,] field)
        {
            int columnLength = field.GetUpperBound(0);
            int rowLength = (int)(columnLength * 1.5); 
            Console.WriteLine("Нажмине любую клавишу для броска кубиков");
            
            Console.ReadKey();

            var rand = new Random();

            int firstDiceNumber = rand.Next(1, 6);
            int secondDiceNumber = rand.Next(1, 6);

            int[,] userFigure = new int[firstDiceNumber, secondDiceNumber];

            int playerXCoordinate = 0;
            int playerYCoordinate = 0;
            int x;

            Console.WriteLine($"Длина Вашей фигуры: {firstDiceNumber}\n" +
                $"Высота Вашей фигуры: {secondDiceNumber}");

            int coordinateCount = 0;
            do
            {
                Console.Write($"Введите координату по {(coordinateCount < 1 ? "вертикали" : "горизонтали")}: ");
                bool done = false;
                do
                {
                    bool xCoordinateCheckBool = int.TryParse(Console.ReadLine(), out x);

                    if (!xCoordinateCheckBool || x < 0 || x > (coordinateCount < 1 ? columnLength : rowLength))
                    {
                        Console.Write($"Введите число между 0 и {(coordinateCount < 1 ? columnLength : rowLength)}: ");
                        continue;
                    }
                        
                    if (xCoordinateCheckBool) done = true;
                }
                while (!done);

                if(coordinateCount < 1)
                {
                    playerYCoordinate = x;
                }
                else
                {
                    playerXCoordinate = x;
                }

                coordinateCount++;
            }
            while (coordinateCount < 2);

            FieldChanger(field, userFigure, playerXCoordinate, playerYCoordinate);
        }

        static void ShowField(int[,] field)
        {
            int rowLength = field.GetUpperBound(0);
            int columnLength = field.GetUpperBound(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < columnLength - 1; j++)
                {
                    if (j == 0)
                    {
                        if (i >= rowLength - 2)
                        {
                            Console.Write("   ");
                        }
                        else
                        {
                            Console.Write("{0,2}|", field[i, j]);
                        }
                    }
                    else if (i == rowLength - 1)
                    {
                        Console.Write("{0,3}", field[i, j]);
                    }
                    else if (i == rowLength - 2)
                    {
                        Console.Write("---");
                    }
                    else
                    {
                        Console.Write("{0,3}", field[i, j]);
                    }
                }

                Console.WriteLine();
            }
        }

        static void FieldChanger(int[,] currentField, int[,] userFigure, int userXCoordinate, int userYCoordinate)
        {
            if(currentField is null || userFigure is null)
            {
                throw new ArgumentNullException(nameof(userFigure));
            }

            int userFigureMaxRowNumber = userFigure.GetUpperBound(0);
            int userFigureMaxColumnNumber = userFigure.GetUpperBound(1);

            for(int i = 0; i <= userFigureMaxRowNumber; i++)
            {
                for(int j = 0; j <= userFigureMaxColumnNumber; j++)
                {
                    currentField[i + userXCoordinate, j + userYCoordinate] = 1;
                }
            }
        }
    }
}
