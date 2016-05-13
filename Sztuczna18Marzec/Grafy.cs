using System.Collections.Generic;

namespace Sztuczna18Marzec
{
    // skierowana
    public class Krawędź
    {
        public Wierzchołek PierwszyWierzchołek { get; set; }
        public Wierzchołek DrugiWierzchołek { get; set; }

    }

    public class Wierzchołek
    {
        public List<Krawędź> KrawędzieWychodzące { get; set; }

    }

    public class Grafy
    {
         
    }
}