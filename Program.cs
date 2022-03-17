using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            int columnLength = Intro();
            
            int[,] field = DrawField((columnLength), (int)(columnLength * 1.5));
            GamePlay(field);
        }

        static int[,] DrawField(int rowLength, int columnLength)
        {
            int[,] field = new int[rowLength, columnLength];

            for(int i = 0; i < field.GetUpperBound(0); i++)
            {
                for(int j = 0; j < field.GetUpperBound(1); j++)
                {
                    field[i, j] = 0;
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
            int rowLength = field.GetUpperBound(1);
            int currentUserNumber = 1;
            do
            {
                Console.WriteLine($"{(currentUserNumber == 1 ? "Первый" : "Второй")} игрок ходит");
                Console.WriteLine("Нажмине любую клавишу для броска кубиков");

                Console.ReadKey();

                var rand = new Random();

                int firstDiceNumber = 4;// rand.Next(1, 7);
                int secondDiceNumber = 2;// rand.Next(1, 7);

                int[,] userFigure = new int[firstDiceNumber, secondDiceNumber];

                int playerXCoordinate;
                int playerYCoordinate;
                int x;
                

                Console.WriteLine($"Ширина Вашей фигуры: {firstDiceNumber}\n" +
                    $"Высота Вашей фигуры: {secondDiceNumber}");

                int coordinateCount = 1;
                
                bool checkBorderInterception;
                bool checkFigureInterception = true;
                do
                {
                    playerXCoordinate = 0;
                    playerYCoordinate = 0;
                    bool coordinateAssignmentDone = false;
                    do
                    {
                        Console.Write($"Введите координату по {(coordinateCount == 1 ? "вертикали (Y)" : "горизонтали (X)")}: ");
                        bool done = false;
                        do
                        {
                            bool xCoordinateCheckBool = int.TryParse(Console.ReadLine(), out x);

                            if (!xCoordinateCheckBool || x < 1 || x > (coordinateCount == 1 ? (columnLength + 1) : (rowLength + 1)))
                            {
                                Console.Write($"Введите число между 1 и {(coordinateCount == 1 ? (columnLength + 1) : (rowLength + 1))}: ");
                                continue;
                            }

                            if (xCoordinateCheckBool) done = true;
                        }
                        while (!done);

                        if (coordinateCount == 1)
                        {
                            playerXCoordinate = x;
                        }
                        else
                        {
                            playerYCoordinate = x;
                            coordinateAssignmentDone = true;
                        }

                        coordinateCount *= -1;
                    }
                    while (!coordinateAssignmentDone);

                    checkBorderInterception = CheckBorderInterception(field, userFigure, playerXCoordinate, playerYCoordinate, out string message);

                    if (checkBorderInterception)
                    {
                        Console.WriteLine($"{message}");
                        continue;
                    }

                    checkFigureInterception = CheckFigureInterception(field, userFigure, playerXCoordinate, playerYCoordinate, out string interceptionWarningMessage);

                    if (checkFigureInterception)
                    {
                        Console.WriteLine($"{interceptionWarningMessage}");
                    }


                }
                while (checkBorderInterception && checkFigureInterception);
                

                int[,] currentField = FieldChanger(field, userFigure, playerXCoordinate, playerYCoordinate, currentUserNumber);

                

                ShowField(currentField);
                currentUserNumber *= -1; 
            }
            while (true);

        }

        static void ShowField(int[,] field)
        {
            int rowLength = field.GetUpperBound(0);
            int columnLength = field.GetUpperBound(1);

            int rowCount = 1;
            

            for (int i = 0; i <= rowLength; i++)
            {
                for (int j = 0; j <= columnLength; j++)
                {
                    if (field[i, j] == 1) Console.BackgroundColor = ConsoleColor.Green;
                    if (field[i, j] == 2) Console.BackgroundColor = ConsoleColor.Yellow;
                    if(j == 0)
                    {
                        Console.ResetColor();
                        Console.Write("{0,3}|", rowCount);
                        rowCount++;
                        if (field[i, j] == 1) Console.BackgroundColor = ConsoleColor.Green;
                        if (field[i, j] == 2) Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.Write("{0,3}", field[i, j]);
                    }
                    else
                    {
                        Console.Write("{0,3}", field[i, j]);
                    }
                    
                    Console.ResetColor();
                }

                Console.WriteLine();
            }

            Console.Write("---|");

            for (int i = 0; i <= columnLength; i++)
            {
                Console.Write("---");
            }

            Console.WriteLine();

            Console.Write("   |");

            for (int i = 1; i <= columnLength + 1; i++)
            {
                Console.Write("{0,3}", i);
            }

            Console.WriteLine();
        }

        static int[,] FieldChanger(int[,] currentField, int[,] userFigure, int userXCoordinate, int userYCoordinate, int currentUserNumber)
        {
            if(currentField is null || userFigure is null)
            {
                throw new ArgumentNullException(nameof(userFigure));
            }

            int fiedlNumberFiller = 2;

            if (currentUserNumber == 1)
                fiedlNumberFiller = 1;

            int userFigureMaxRowNumber = userFigure.GetUpperBound(1);
            int userFigureMaxColumnNumber = userFigure.GetUpperBound(0);

            for(int i = -1; i < userFigureMaxRowNumber; i++)
            {
                for(int j = -1; j < userFigureMaxColumnNumber; j++)
                {
                    currentField[i + userXCoordinate, j + userYCoordinate] = fiedlNumberFiller;
                }
            }

            return currentField;
        }

        static bool CheckBorderInterception(int[,] currentField, int[,] userFigure, int userXCoordinate, int userYCoordinate, out string message)
        {
            message = string.Empty;
            bool xBorderInterception = currentField.GetUpperBound(0) + 1 < (userFigure.GetUpperBound(0) + userXCoordinate);
            if (xBorderInterception)
            {
                message = "Фигура выхдить за рамки поля по координате Y";
                return true;
            }
                
            bool yBorderInterception = currentField.GetUpperBound(1) + 1 < (userFigure.GetUpperBound(1) + userYCoordinate);
            if (yBorderInterception)
            {
                message = "Фигура выхдить за рамки поля по координате X";
                return true;
            }

            return false;
        }

        static bool CheckFigureInterception(int[,] currentField, int[,] userFigure, int userYCoordinate, int userXCoordinate, out string message)
        {
            message = "Невозможно установить фигуру. Пересечение с другой фигурой";

            for(int i = userYCoordinate - 1; i < (userYCoordinate + userFigure.GetUpperBound(0)); i++)
            {
                for(int j = userXCoordinate - 1; j < (userXCoordinate + userFigure.GetUpperBound(1)); j++)
                {
                    if(currentField[i,j] != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
