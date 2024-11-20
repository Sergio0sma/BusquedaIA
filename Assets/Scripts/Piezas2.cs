using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class Piezas2 : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        //Cuando se inicie el juego de rigor por aquí empezamos!
        // Creo el nodo
        int[,] piezas = {{1,2,3},
                        {4,5,0},
                        {6,7,8}};//Creo el array

        Nodo2 root = new Nodo2(piezas);// Creo el objeto con el array

        //this.BusquedaAnchura(root);
        //List<Nodo2> solucion = BusquedaAnchura(root);
        //List<Nodo2> solucion = BusquedaProfundidad(root);
        //List<Nodo2> solucion = BusquedaProfundidadPila(root);
        //List<Nodo2> solucion = BusquedaAasterisco(root);
        List<Nodo2> solucion = BusquedaAasteriscoConCoste(root);

        if (solucion.Count > 0)
        {
            Debug.Log("Imprimiendo la solución: ");
            solucion.Reverse();
            for (int i = 0; i < solucion.Count; i++)
            {
                solucion[i].Imprime();

            }
        }
        else
        {
            Debug.Log("No hemos encontrado la solucion");
        }

    }//Start

    private List<Nodo2> BusquedaProfundidadPila(Nodo2 root)
    {
        //variables 
        Stack<(Nodo2, int)> Abiertos = new Stack<(Nodo2, int)>();// Metemos el nodo y su nivel
        List<Nodo2> Cerrados = new List<Nodo2>();
        List<Nodo2> CaminoSolucion = new List<Nodo2>();
        bool encontrado = false;
        Abiertos.Push((root, 0)); //metemos el nodo raíz y su nivel
        int contador = 0;
        int limite = 25; //Límite para bajar en el árbol


        while (Abiertos.Count > 0 && !encontrado)
        {
            var (actual, nivel) = Abiertos.Pop();//Cogemos el último elemento del pila y su nivel
            Cerrados.Add(actual);//Ya hemos visitado ese nodo
            if (actual.EsMeta())
            {
                Debug.Log("Hemos encontrado la solución.");
                encontrado = true;
                Trazo(CaminoSolucion, actual);
                return CaminoSolucion;
            }//if

            if (nivel < limite)
            {
                actual.Expandir();//Expandimos los nodos
                for (int i = 0; i < actual.hijos.Count; i++)
                {
                    Nodo2 hijoActual = actual.hijos[i];//Cogemos el hijo
                    if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual)) {

                        Abiertos.Push((hijoActual, nivel + 1));
                        contador++;// Para saber cuantos hijos creados llevamos
                    }//if
                }//for
            }

            Debug.Log("El número de hijos que hemos creado: " + contador);

        }//while

        if (Abiertos.Count == 0)
        {
            Debug.Log("no hemos encontrado la solución");
        }

        return CaminoSolucion;


    }//BusquedaProfundidadPila

    /// <summary>
    /// Búsqueda en profundidad
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    private List<Nodo2> BusquedaProfundidad(Nodo2 root)
    {
        List<Nodo2> Abiertos = new List<Nodo2>();
        List<Nodo2> Cerrados = new List<Nodo2>();
        List<Nodo2> CaminoSolucion = new List<Nodo2>();

        //int contador = 0;
        Abiertos.Add(root); // Añadimos el raíz
        bool encontrado = false;//-------- Recordar!!!!

        int limite = 25; //Límite para bajar en el árbol

        while (Abiertos.Count > 0 && !encontrado)
        {


            Nodo2 actual = Abiertos[0];//Cogemos el primer elemento de la lista
            Cerrados.Add(actual);//Lo metemos como ya visitado
            Abiertos.RemoveAt(0);//Quitamos el nodo de abiertos 

            if (actual.EsMeta())
            {
                Debug.Log("Enhorabuena!!! hemos encontrado la solución.");
                encontrado = true;
                Trazo(CaminoSolucion, actual);
                return CaminoSolucion;

            }//if

            if (Profundidad(actual) < limite)
            {
                actual.Expandir();//Expandimos el nodo

                for (int i = 0; i < actual.hijos.Count; i++)
                {
                    Nodo2 hijoActual = actual.hijos[i];//Cogemos el hijo
                    if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                    {
                        Abiertos.Insert(0, hijoActual);//Lo metemos en el inicio de la lista. Se mete al principio
                                                       //Esta es la principal diferencia con búsqueda en anchura

                    }//if
                }//for
            }//if


        }//while

        return CaminoSolucion;

    }//BusquedaProfundidad

    /////////////
    /// Búsqueda en anchura
    /// ////////
    private List<Nodo2> BusquedaAnchura(Nodo2 root)
    {
        //Variables para el algoritmo
        List<Nodo2> Abiertos = new List<Nodo2>();//Nodos que faltan por visitar
        List<Nodo2> Cerrados = new List<Nodo2>();// Nodos que ya he visitado
        List<Nodo2> CaminoSoluccion = new List<Nodo2>(); // Lista con el camino
        bool encontrado = false;
        Abiertos.Add(root);//añado el raíz
        int contador = 0;

        while (Abiertos.Count > 0 && !encontrado)
        {
            Nodo2 actual = Abiertos[0]; //cogemos el primer elemento
            Abiertos.RemoveAt(0);//eliminamos el elemento
            Cerrados.Add(actual);//ya visitamos este nodo
            //Tratamos el nodo actual. 
            if (actual.EsMeta())
            {
                Debug.Log("Hemos encontrado el nodo solución!!!!");
                encontrado = true;
                //tenemos que devolver el camino a la solución, es decir, la lista de los nodos
                // por los que tenemos que pasar
                Trazo(CaminoSoluccion, actual);
                Debug.Log("El camino solución tiene: " + CaminoSoluccion.Count);
                return CaminoSoluccion;
                //break;//Salimos del todo
            }//if

            //Expandimos el nodo actual
            actual.Expandir();
            for (int i = 0; i < actual.hijos.Count; i++)// Recorremos todos los hijos
            {
                Nodo2 hijoActual = actual.hijos[i];
                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {
                    //hijoActual.Imprime();
                    Abiertos.Add(hijoActual);// Metemos como una posible solución Se mete al final
                    contador++;
                    //Debug.Log("Número de nodos metidos: "+contador);

                }//if

            }//for
            if (contador == 50000) break;
        }//while

        //No hemos encontrado solución
        Debug.Log("No hemos encontrado solución, Alma de cántaro.");
        return null;




    }//BusquedaAnchura




    ///////////////////////////
    /// Búsqueda A*
    /// //////////////////////
    /// 
    private List<Nodo2> BusquedaAasterisco(Nodo2 root)
    {
        //Variables para el algoritmo
        List<Nodo2> Abiertos = new List<Nodo2>();//Nodos que faltan por visitar
        List<Nodo2> Cerrados = new List<Nodo2>();// Nodos que ya he visitado
        List<Nodo2> CaminoSoluccion = new List<Nodo2>(); // Lista con el camino
        int contador = 1;
        Abiertos.Add(root);//añado el raíz
        while (Abiertos.Count > 0)
        {
            //Nodo2 actual=Abiertos[0];//Cogemos el primero
            //Ordenamos por la heurística la lista de abiertos
            Abiertos.Sort((n1, n2) => n1.Heuristica.CompareTo(n2.Heuristica));
            Nodo2 actual = Abiertos[0];//Cogemos el primero
            Cerrados.Add(actual); //Lo metemos en cerrados
            Abiertos.RemoveAt(0);// Y lo eliminamos de abiertos


            if (actual.EsMeta())
            {
                Debug.Log("Enhorabuena Champiñón! Has encontrado solución");
                Trazo(CaminoSoluccion, actual);
                return CaminoSoluccion;
            }//if

            actual.Expandir();//Expandimos el nodo

            for (int i = 0; i < actual.hijos.Count; i++)
            {
                Nodo2 hijoActual = actual.hijos[i];
                contador++;
                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {
                    hijoActual.calculaMalColocadas();
                    Abiertos.Add(hijoActual);
                }//if
                Debug.Log(contador);
            }//for

            if (contador == 5000) break;

        }//while
        return CaminoSoluccion;
    }//BusquedaAasterisco



    private List<Nodo2> BusquedaAasteriscoConCoste(Nodo2 root)
    {
        //Variables para el algoritmo
        List<Nodo2> Abiertos = new List<Nodo2>();//Nodos que faltan por visitar
        List<Nodo2> Cerrados = new List<Nodo2>();// Nodos que ya he visitado
        List<Nodo2> CaminoSoluccion = new List<Nodo2>(); // Lista con el camino
        int contador = 1;
        Abiertos.Add(root);//añado el raíz
        while (Abiertos.Count > 0)
        {

            //Ordenamos por la heurística la lista de abiertos
            Abiertos.Sort((n1, n2) => (n1.costo + n1.manhattan).CompareTo(n2.costo + n2.manhattan));
            Nodo2 actual = Abiertos[0];//Cogemos el primero
            Cerrados.Add(actual); //Lo metemos en cerrados
            Abiertos.RemoveAt(0);// Y lo eliminamos de abiertos


            if (actual.EsMeta())
            {
                Debug.Log("Enhorabuena Champiñón! Has encontrado solución");
                Trazo(CaminoSoluccion, actual);
                return CaminoSoluccion;
            }//if

            actual.Expandir();//Expandimos el nodo

            for (int i = 0; i < actual.hijos.Count; i++)
            {
                Nodo2 hijoActual = actual.hijos[i];
                contador++;
                if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                {
                    hijoActual.costo = actual.costo + 1; //actualizamos el costo
                    hijoActual.CalcularHeuristicaManhattan();
                    Abiertos.Add(hijoActual);
                }//if
                Debug.Log(contador);
            }//for

            if (contador == 5000) break;

        }//while
        return CaminoSoluccion;
    }//BusquedaAasterisco

    private Nodo2 getMenor(List<Nodo2> abiertos)
    {
        Nodo2 nodoConMenorMalcolocadas = abiertos.OrderBy(n => n.malcolocadas).FirstOrDefault();
        abiertos.Remove(nodoConMenorMalcolocadas);
        nodoConMenorMalcolocadas.Imprime();
        return nodoConMenorMalcolocadas;
    }//getMenor

    private bool Contiene(List<Nodo2> lista, Nodo2 hijoActual)
    {

        foreach (Nodo2 nodo in lista)
        {
            if (nodo.EsMismoNodo(hijoActual.nodo)) { return true; }
        }
        return false;
    }//Contiene

    private bool Contiene(Stack<(Nodo2, int)> pila, Nodo2 hijoActual)
    {

        foreach ((Nodo2 nodo, int nivel) in pila)
        {
            if (nodo.EsMismoNodo(hijoActual.nodo)) { return true; }
        }
        return false;
    }//Contiene


    //Método que hace la solucion
    public void Trazo(List<Nodo2> camino, Nodo2 n)
    {
        Debug.Log("Trazando el camino: ");
        Nodo2 actual = n;
        camino.Add(actual);
        while (actual.padre != null)
        {
            actual = actual.padre;
            camino.Add(actual);
        }
        //camino.Reverse();
        //Debug.Log("Número de pasos "+camino.Count);
    }



    private int Profundidad(Nodo2 nodo)
    {
        int profundidad = 0;
        while (nodo.padre != null)
        {
            nodo = nodo.padre;
            profundidad++;
        }
        return profundidad; // Calculamos la profundidad del nodo
    }



                    
 
}//Piezas