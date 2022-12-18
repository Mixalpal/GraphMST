using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphMST {

    public class findMocker
    {
        List<int> results;
        public findMocker()
        {
            results = new List<int>()
            {
                0, 1, 1, 0, 1, 2, 2, 1, 2, 3, 3, 2, 0, 1, 0, 1, 0, 0, 0, 2, 0, 0, 2, 0, 3, 0, 0, 3, 0
            };
        }
        public int findMock()
        {
            int result = results.ElementAt(results.Count - 1);
            results.RemoveAt(results.Count - 1);
            return result;
        }
    }


public class Edge // Грань графа
{
    public int weight;
    public int dest;
    public int src;
    public Edge next;
    public Edge(int weight, int src, int dest)
    {
        this.weight = weight;
        this.dest = dest;
        this.src = src;
        this.next = null;
    }
}
public class State
{
    public int parent;
    public int rank;
    public State(int parent, int rank)
    {
        this.parent = parent;
        this.rank = rank;
    }
}
    public class Graph
    {
        public int vertices;
        public findMocker mocker;
        public List<List<Edge>> graphEdge;
        
        public Graph(int vertices)
        {
            //mocker = new findMocker();
            if (vertices < 0 || vertices > 10)
            {
                Console.WriteLine("Введено некорректное число вершин графа.");
            }
            this.vertices = vertices;
            this.graphEdge = new List<List<Edge>>(vertices);
            for (int i = 0; i < this.vertices; ++i)
            {
                this.graphEdge.Add(new List<Edge>());
            }
        }
        public bool addEdge(int src, int dest, int w) //Добавление грани
        {
            if (dest < 0 || dest >= this.vertices || src < 0 || src >= this.vertices)
            {
                Console.WriteLine("Одна из введенных вершин не существует");
                return false;
            }
            this.graphEdge[src].Add(new Edge(w, src, dest));
            if (dest == src)
            {
                return true;
            }
            this.graphEdge[dest].Add(new Edge(w, dest, src));
            return true;
        }
        public void printGraph() //Вывод графа
        {
            Console.Write("\n Graph Adjacency List ");
            for (int i = 0; i < this.vertices; ++i)
            {
                Console.Write(" \n [" + i + "] :");
                // iterate edges of i node
                for (int j = 0; j < this.graphEdge[i].Count; ++j)
                {
                    Console.Write("  " + this.graphEdge[i][j].dest);
                }
            }
        }
        public int find(State[] subsets, int i)
        {
            //Console.WriteLine("\n" + subsets + "\n");
            if (subsets[i].parent != i)
            {
                subsets[i].parent = this.find(subsets, subsets[i].parent);
            }
            //Console.WriteLine("\n" + subsets[i].parent + "\n");
            return subsets[i].parent;
        }
        void findUnion(State[] subsets, int x, int y)
        {
            int a = this.find(subsets, x);
            int b = this.find(subsets, y);
            //int a = mocker.findMock();
            //int b = mocker.findMock();
            //Console.WriteLine(" a = " + a + " b = " + b);
            if (subsets[a].rank < subsets[b].rank)
            {
                subsets[a].parent = b;
            }
            else if (subsets[a].rank > subsets[b].rank)
            {
                subsets[b].parent = a;
            }
            else
            {
                subsets[b].parent = a;
                subsets[a].rank++;
            }
        }
        public int boruvkaMST()
        {
            int result = 0;
            int selector = this.vertices;
            State[] subsets = new State[this.vertices];
            Edge[] cheapest = new Edge[this.vertices];
            for (int v = 0; v < this.vertices; ++v)
            {
                subsets[v] = new State(v, 0);
            }
            while (selector > 1)
            {
                for (int v = 0; v < this.vertices; ++v)
                {
                    cheapest[v] = null;
                }
                for (int k = 0; k < this.vertices; k++)
                {
                    for (int i = 0; i < this.graphEdge[k].Count; ++i)
                    {
                        int set1 = this.find(subsets, this.graphEdge[k][i].src);
                        int set2 = this.find(subsets, this.graphEdge[k][i].dest);
                        //int set1 = mocker.findMock();
                        //int set2 = mocker.findMock();
                        if (set1 != set2)
                        {
                            if (cheapest[k] == null)
                            {
                                cheapest[k] = this.graphEdge[k][i];
                            }
                            else if (cheapest[k].weight >
                                     this.graphEdge[k][i].weight)
                            {
                                cheapest[k] = this.graphEdge[k][i];
                            }
                        }
                    }
                }
                for (int i = 0; i < this.vertices; i++)
                {
                    if (cheapest[i] != null)
                    {
                        int set1 = this.find(subsets, cheapest[i].src);
                        int set2 = this.find(subsets, cheapest[i].dest);
                        //int set1 = mocker.findMock();
                        //int set2 = mocker.findMock();
                        if (set1 != set2)
                        {
                            // Reduce a edge
                            selector--;
                            this.findUnion(subsets, set1, set2);
                            // Display the edge connection
                            Console.Write("\n Include Edge (" +
                                          cheapest[i].src + " - " +
                                          cheapest[i].dest + ") weight " +
                                          cheapest[i].weight);
                            // Add weight
                            result += cheapest[i].weight;
                        }
                    }
                }
            }
            Console.WriteLine("\n Calculated total weight of MST is " + result);
            return result;
        }
        public static void Main(String[] args)
        {
            string choise;
            Console.WriteLine("Введите количество вершин графа");
            choise = Console.ReadLine();
            if (choise.Any(c => char.IsNumber(c)) == false || Convert.ToInt32(choise) < 0 || Convert.ToInt32(choise) > 10)
            {
                Console.WriteLine("Введено некорректное количество вершин");
                return;
            }
            Graph g = new Graph(Convert.ToInt32(choise));

            Console.WriteLine("Введите данные вершины в формате *первая вершина* *вторая вершина* *вес вершины*ю Если вы хотите завершить ввод, введите \"КОНЕЦ\"");
            string[] choises = new string[3];

            choises = Console.ReadLine().Split();
            while (choises[0] != "КОНЕЦ")
            {

                if (choises.Length < 3 || choises[0] == "" || choises[1] == "" || choises[2] == "")
                {
                    Console.WriteLine("Не введено одно из значений");
                    return;
                }
                if (choises[0].Any(c => char.IsNumber(c)) == false || choises[1].Any(c => char.IsNumber(c)) == false || choises[2].Any(c => char.IsNumber(c)) == false)
                {
                    Console.WriteLine("Одно или несколько из значений введены некорректно");
                    return;
                }

                if (!g.addEdge(Convert.ToInt32(choises[0]) - 1, Convert.ToInt32(choises[1]) - 1, Convert.ToInt32(choises[2])))
                    return;
                choises = Console.ReadLine().Split();
            }
            //g.printGraph();

            g.boruvkaMST();
            
            Console.ReadLine();
        }
    }
}