using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACOController  : AStarController
{
    private ACOCON aCOCON = new ACOCON();

    protected float DefaultPheromone = 0f;

    //config the ACOCON object 
    protected void ConfigACOCON(float alpha = 1.0f, float beta = 0.0001f,
        float evaporationFactor = 0.5f, float q = 0.0006f)
    {
        aCOCON.SetAlpha(alpha);
        aCOCON.SetBeta(beta);
        aCOCON.SetEvaporationFactor(evaporationFactor);
        aCOCON.SetQ(q);
    }

    //return the pheromone value
    private float getDefaultPheromone() {
        return aCOCON.DefaultPheromone;
    }

    //find ACOCON path
    protected List<ACOConnection> GenACOPath(int iterThreshold, int totNumAnts, GameObject[] waypointNodes, List<ACOConnection> connections,
        GameObject startNode, int maxPathLength)
    {
        return aCOCON.ACO(iterThreshold, totNumAnts, waypointNodes, connections, startNode, maxPathLength);
    }
}
