using System;
using System.Collections.Generic;
using System.Linq;

namespace TorresDeHanoi
{
    class Program
    {
        // Variables globales
        static int n; // Número de discos
        static int movimientos; // Número de movimientos realizados
        static bool rendido; // Indica si el jugador se ha rendido
        static Stack<int>[] torres; // Arreglo de pilas que representan las torres

        static void Main(string[] args)
        {
            // Pedir al usuario que elija el nivel de dificultad
            Console.WriteLine("Bienvenido al juego de las torres de Hanoi.");
            Console.WriteLine("Elige el nivel de dificultad:");
            Console.WriteLine("1. Fácil (3 discos)");
            Console.WriteLine("2. Normal (5 discos)");
            Console.WriteLine("3. Difícil (7 discos)");
            Console.Write("Ingresa el número del nivel: ");
            int nivel = int.Parse(Console.ReadLine());

            // Asignar el número de discos según el nivel
            switch (nivel)
            {
                case 1:
                    n = 3;
                    break;
                case 2:
                    n = 5;
                    break;
                case 3:
                    n = 7;
                    break;
                default:
                    Console.WriteLine("Nivel inválido. Se asignará el nivel fácil por defecto.");
                    n = 3;
                    break;
            }

            // Inicializar las variables globales
            movimientos = 0;
            rendido = false;
            torres = new Stack<int>[3];
            for (int i = 0; i < 3; i++)
            {
                torres[i] = new Stack<int>();
            }

            // Llenar la primera torre con los discos ordenados de mayor a menor
            for (int i = n; i >= 1; i--)
            {
                torres[0].Push(i);
            }

            // Mostrar las instrucciones del juego
            Console.WriteLine("Instrucciones:");
            Console.WriteLine("El objetivo del juego es mover todos los discos de la torre A a la torre C, siguiendo las reglas:");
            Console.WriteLine("- Solo se puede mover un disco a la vez.");
            Console.WriteLine("- No se puede colocar un disco más grande sobre uno más pequeño.");
            Console.WriteLine("- Se puede usar la torre B como auxiliar.");
            Console.WriteLine("Para mover un disco, usa las siguientes teclas:");
            Console.WriteLine("- A: Mover el disco superior de la torre A a la siguiente torre disponible en el sentido horario.");
            Console.WriteLine("- S: Mover el disco superior de la torre B a la siguiente torre disponible en el sentido horario.");
            Console.WriteLine("- D: Mover el disco superior de la torre C a la siguiente torre disponible en el sentido horario.");
            Console.WriteLine("Para rendirte y ver la solución, presiona la tecla R.");
            Console.WriteLine("¡Buena suerte!");

            // Mostrar el estado inicial del juego
            MostrarEstado();

            // Repetir hasta que el juego termine o el jugador se rinda
            while (!JuegoTerminado() && !rendido)
            {
                // Leer la tecla presionada por el usuario
                ConsoleKeyInfo tecla = Console.ReadKey(true);

                // Realizar la acción correspondiente según la tecla
                switch (tecla.Key)
                {
                    case ConsoleKey.A:
                        MoverDisco(0);
                        break;
                    case ConsoleKey.S:
                        MoverDisco(1);
                        break;
                    case ConsoleKey.D:
                        MoverDisco(2);
                        break;
                    case ConsoleKey.R:
                        Rendirse();
                        break;
                    default:
                        Console.WriteLine("Tecla inválida. Usa las teclas A, S, D o R.");
                        break;
                }
            }

            // Mostrar el resultado final del juego
            if (rendido)
            {
                Console.WriteLine("Te has rendido. Aquí está la solución:");
                Resolver(n, 0, 2, 1);
            }
            else
            {
                Console.WriteLine("¡Felicidades! Has resuelto el juego en {0} movimientos.", movimientos);
            }
        }

        // Método para mostrar el estado actual de las torres
        static void MostrarEstado()
        {
            Console.WriteLine("Estado actual de las torres:");
            Console.WriteLine("Torre A: {0}", string.Join(" ", torres[0].Reverse()));
            Console.WriteLine("Torre B: {0}", string.Join(" ", torres[1].Reverse()));
            Console.WriteLine("Torre C: {0}", string.Join(" ", torres[2].Reverse()));
            Console.WriteLine("Movimientos realizados: {0}", movimientos);
        }

        // Método para mover un disco desde una torre a la siguiente disponible en el sentido horario
        static void MoverDisco(int origen)
        {
            // Calcular el índice de la torre destino
            int destino = (origen + 1) % 3;

            // Verificar si la torre origen tiene algún disco
            if (torres[origen].Count == 0)
            {
                Console.WriteLine("La torre {0} está vacía. No se puede mover ningún disco.", (char)('A' + origen));
                return;
            }

            // Obtener el valor del disco a mover
            int disco = torres[origen].Peek();

            // Buscar la siguiente torre disponible en el sentido horario
            while (torres[destino].Count > 0 && torres[destino].Peek() < disco)
            {
                destino = (destino + 1) % 3;
            }

            // Verificar si se encontró una torre disponible
            if (destino == origen)
            {
                Console.WriteLine("No hay ninguna torre disponible para mover el disco {0}.", disco);
                return;
            }

            // Mover el disco de la torre origen a la torre destino
            torres[origen].Pop();
            torres[destino].Push(disco);
            movimientos++;

            // Mostrar el estado actual de las torres
            MostrarEstado();
        }

        // Método para verificar si el juego ha terminado
        static bool JuegoTerminado()
        {
            // El juego termina cuando todos los discos están en la torre C
            return torres[2].Count == n;
        }

        // Método para rendirse y mostrar la solución
        static void Rendirse()
        {
            // Cambiar el valor de la variable rendido a verdadero
            rendido = true;
        }

        // Método para resolver el juego de forma recursiva
        static void Resolver(int k, int origen, int destino, int auxiliar)
        {
            // Caso base: si k es cero, no hay nada que hacer
            if (k == 0) return;

            // Caso recursivo: resolver el problema para k-1 discos
            Resolver(k - 1, origen, auxiliar, destino);

            // Mover el disco k de la torre origen a la torre destino
            Console.WriteLine("Mover el disco {0} de la torre {1} a la torre {2}.", k, (char)('A' + origen), (char)('A' + destino));

            // Resolver el problema para k-1 discos
            Resolver(k - 1, auxiliar, destino, origen);
        }
    }
}