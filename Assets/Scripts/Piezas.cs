using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class Piezas : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        //Cuando se inicie el juego de rigor por aquí empezamos!
        // Creo el nodo
        int[,] piezas= {{1,2,3}, 
                        {4,5,0},
                        {6,7,8}};//Creo el array

        Nodo root=new Nodo(piezas);// Creo el objeto con el array

        //this.BusquedaAnchura(root);
        List<Nodo> solucion = BusquedaProfundidad(root);
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

    /////////////
    /// Búsqueda en anchura
    /// ////////
    //private List<Nodo> BusquedaAnchura(Nodo root)
    //{
    //    //Variables para el algoritmo
    //    List<Nodo> Abiertos=new List<Nodo>();//Nodos que faltan por visitar
    //    List<Nodo> Cerrados=new List<Nodo>();// Nodos que ya he visitado
    //    List<Nodo> CaminoSoluccion = new List<Nodo>(); // Lista con el camino
    //    bool encontrado=false;
    //    Abiertos.Add(root);//añado el raíz
    //    int contador = 0;

    //    while(Abiertos.Count > 0 && !encontrado)
    //    {
    //        Nodo actual = Abiertos[0]; //cogemos el primer elemento
    //        Abiertos.RemoveAt(0);//eliminamos el elemento
    //        Cerrados.Add(actual);//ya visitamos este nodo
    //        //Tratamos el nodo actual. 
    //        if (actual.EsMeta())
    //        {
    //            Debug.Log("Hemos encontrado el nodo solución!!!!");
    //            encontrado = true;
    //            //tenemos que devolver el camino a la solución, es decir, la lista de los nodos
    //            // por los que tenemos que pasar
    //            Trazo(CaminoSoluccion, actual);
    //            Debug.Log("El camino solución tiene: "+CaminoSoluccion.Count);
    //            return CaminoSoluccion;
    //            //break;//Salimos del todo
    //        }//if

    //        //Expandimos el nodo actual
    //        actual.Expandir();
    //        for(int i =0; i<actual.hijos.Count; i++)// Recorremos todos los hijos
    //        {
    //            Nodo hijoActual=actual.hijos[i];
    //            if(!Contiene(Abiertos,hijoActual) && !Contiene(Cerrados, hijoActual))
    //            {
    //                //hijoActual.Imprime();
    //                Abiertos.Add(hijoActual);// Metemos como una posible solución
    //                contador++;
    //                //Debug.Log("Número de nodos metidos: "+contador);

    //            }//if

    //        }//for
    //        if (contador == 50000) break;
    //    }//while

    //    //No hemos encontrado solución
    //    Debug.Log("No hemos encontrado solución, Alma de cántaro.");
    //    return null;
            
        
       

    //}//BusquedaAnchura

   //BUSQUEDA EN PROFUNDIDAD

    private List<Nodo> BusquedaProfundidad(Nodo root)
    {
        //Variables para el algoritmo
        List<Nodo> Abiertos = new List<Nodo>();//Nodos que faltan por visitar
        List<Nodo> Cerrados = new List<Nodo>();// Nodos que ya he visitado
        List<Nodo> CaminoSoluccion = new List<Nodo>(); // Lista con el camino
        bool encontrado = false;
        Abiertos.Add(root);//añado el raíz
        //int contador = 0;


        while (Abiertos.Count > 0 && !encontrado)
        {
            Nodo actual = Abiertos[Abiertos.Count - 1]; //Obtenemos el ultimo elemento
            Abiertos.RemoveAt(Abiertos.Count - 1);//eliminamos el elemento
            Cerrados.Add(actual);//ya visitamos este nodo
                                 //Tratamos el nodo actual. 

            int profundidadMaxima = 25;
            //int limite = 0;

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

            //Expandimos el nodo actual mientras no supere la profundidad maxima.
            if (Profundidad(actual) < profundidadMaxima)
            {
                actual.Expandir();
                for (int i = 0; i < actual.hijos.Count; i++) // Recorremos todos los hijos
                {
                    Nodo hijoActual = actual.hijos[i];
                    if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
                    {
                        Abiertos.Add(hijoActual); // Añadimos a la lista de nodos a visitar
                        //contador++;
                    }
                }
                
            }
            //if (contador ==500) break;
        }//while

        //No hemos encontrado solución
        Debug.Log("No hemos encontrado solución, Alma de cántaro.");
        return null;
    }


    ///////////////////////////
    /// Búsqueda A*
    /// //////////////////////
    ///
    //private List<Nodo> BusquedaAasterisco(Nodo root)
    //{
    //    //Variables para el algoritmo
    //    List<Nodo> Abiertos = new List<Nodo>();//Nodos que faltan por visitar
    //    List<Nodo> Cerrados = new List<Nodo>();// Nodos que ya he visitado
    //    List<Nodo> CaminoSoluccion = new List<Nodo>(); // Lista con el camino
    //    int contador = 1;
    //    Abiertos.Add(root);//añado el raíz
    //    while (Abiertos.Count > 0)
    //    {
    //        //Nodo2 actual=Abiertos[0];//Cogemos el primero
    //        Nodo actual =//= getMenor(Abiertos);
    //        Cerrados.Add(actual);
    //        //Abiertos.RemoveAt(0);
    //        if (actual.EsMeta())
    //        {
    //            Debug.Log("Enhorabuena Champiñón! Has encontrado solución");
    //            Trazo(CaminoSoluccion, actual);
    //        }
    //        actual.Expandir();
    //        for (int i = 0; i < actual.hijos.Count; i++)
    //        {
    //            Nodo hijoActual = actual.hijos[i];
    //            contador++;
    //            if (!Contiene(Abiertos, hijoActual) && !Contiene(Cerrados, hijoActual))
    //            {
    //                Abiertos.Add(hijoActual);
    //            }//if
    //            Debug.Log(contador);
    //        }//for

    //        if (contador == 5000) break;

    //    }//while
    //    return CaminoSoluccion;
    //}//BusquedaAasterisco

    private int Profundidad(Nodo nodo)
    {
        int profundidad = 0;
        while (nodo.padre != null)
        {
            nodo = nodo.padre;
            profundidad++;
        }
        return profundidad; // Calculamos la profundidad del nodo
    }

    private bool Contiene(List<Nodo> lista,Nodo hijoActual)
    {
        /*   for (int i = 0; i < lista.Count; i++)
           {
               if (lista[i].EsMismoNodo(hijoActual.nodo))
                   return true;
           }

           return false;
        */
        foreach (Nodo nodo in lista)
        {
            if(nodo.EsMismoNodo(hijoActual.nodo)) { return true; }
        }
        return false;
    }//Contiene

    private bool Contiene(Stack<Nodo> pila, Nodo hijoActual)
    {
        /*   for (int i = 0; i < lista.Count; i++)
           {
               if (lista[i].EsMismoNodo(hijoActual.nodo))
                   return true;
           }

           return false;
        */
        foreach (Nodo nodo in pila)
        {
            if (nodo.EsMismoNodo(hijoActual.nodo)) { return true; }
        }
        return false;
    }//Contiene

    //Metodo que hace la solucion
    public void Trazo(List<Nodo> camino, Nodo n)
    {
        Debug.Log("Trazando el camino: ");
        Nodo actual = n;
        camino.Add(actual);
        while (actual.padre != null)
        {
            actual = actual.padre;
            camino.Add(actual);
        }
        //camino.Reverse();
        //Debug.Log("Número de pasos "+camino.Count);
    }






}//Piezas