using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ManagerScript : MonoBehaviour
{
    public GameObject Camera;

    public GameObject hospital;
    public GameObject school;
    public GameObject store;
    public GameObject supermarket;
    public GameObject restaurant;
    public GameObject home;
    public GameObject company;
    public GameObject bus;

    public GameObject person;

    private List<Vector2> _sitesMin;
    private List<Vector2> _sitesMax;

    private List<GameObject> _persons;

    public Text dataReceived;
    public Text dataReceived1;
    public Text dataReceived2;
    public Text dataReceived3;
    int read = 0;

    // Start is called before the first frame update
    void Start()
    {
        _persons = new List<GameObject>();

        _sitesMin = new List<Vector2>();
        _sitesMax = new List<Vector2>();

        //DataPopulation populationDatas = JsonUtility.FromJson<DataPopulation>(@"{ ""NbPersons"":90,""IndexOfInfected"":[]}");
        //DataSites sitesDatas = JsonUtility.FromJson<DataSites>(@"{ ""SitesType"":[0,2,4,6,4,6,6,4,1,3,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],""SitesId"":[0,1,5,20,70,220,670,4,2,10,40,140,440,1340,8,3,15,60,210,660,2010,1,3,4,5,6,7,8,9,10,11,12,13,16,19,22,25,28,31,34,35,36,39,42,45,48,51,54,57,60,63,64,65,68,71,74,77,80,83,84,87,90,93,96,99,102,105,108,111,114,117,120,123,126,129,132,135,138,141,144,147,150,153,156,159,162,165,168,171,174,177,180,181,182,185,188,189,192,195,198,201,204,207,210,213,216,219,222,225,228,231,234,237,240,241,242,245,248,251,254,257,258,259,262,265,268,271,274,277,278,279,280]}");
        //DataSites sitesDatas = JsonUtility.FromJson<DataSites>(@"{ ""NbHouse"":1000,""NbCompany"":1000,""NbHospital"":1000,""NbRestaurant"":1000,""NbSchool"":100,""NbStore"":100,""NbSupermarket"":100}");

        //GetComponent<ScriptClient>().dataReceived3.text = populationDatas.NbPersons + " " + populationDatas.IndexOfInfected.Count;
        //GetComponent<ScriptClient>().dataReceived2.text = sitesDatas.SitesType.Count + " " + sitesDatas.SitesId.Count;
        //
        //CreateSites(sitesDatas);
        //CreatePopulation(populationDatas.NbPersons, populationDatas.IndexOfInfected);
    }

    // Update is called once per frame
    void Update()
    {
        //List<int> test = GameObject.Find("GUIManager").GetComponent<ScriptClient>().testTransfer;
        if (Input.GetMouseButton(1))
        {
            //DataIteration iterationDatas = JsonUtility.FromJson<DataIteration>(@"{ ""PersonsNewSite"":[0,5,5,5,15,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5, 
            //                                                                                           0,5,5,5,15,5,5,5,5,5,5,2000,5,5,5,5,5,5,5,5,5,
            //                                                                                           0,5,5,5,15,5,5,5,5,5,2000,5,5,5,5,5,5,3,5,5,5,
            //                                                                                           0,5,5,5,15,5,5,5,5,5,2000,5,3000,5,5,5,5,3,5,5,5,
            //                                                                                           0,5,5,5,5,5,5,5,5,5,3000,5,3000,5,5,5,5,5,5,5,5],
            //                                                                       ""PersonsNewState"":[0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,
            //                                                                                            0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,
            //                                                                                            0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,
            //                                                                                            0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,
            //                                                                                            0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3]}");

            //_persons.ForEach(p => {
            //    MovementScript pScript = p.GetComponent<MovementScript>();
            //    int pIndex = pScript.index;
            //    int siteIndex = iterationDatas.PersonsNewSite[pIndex];
            //    p.GetComponent<MovementScript>().SetTarget(_sitesMin[siteIndex], _sitesMax[siteIndex], siteIndex);
            //    if (pScript.state != iterationDatas.PersonsNewState[pIndex])
            //        pScript.SetState(iterationDatas.PersonsNewState[pIndex]);
            //});

            // _persons.ForEach(p => {
            //     int siteIndex = Random.Range(0, _sitesMin.Count);
            //     p.GetComponent<MovementScript>().SetTarget(_sitesMin[siteIndex], _sitesMax[siteIndex], siteIndex);
            // });
        }
    }
    bool creationDone = false;
    public void SetIterationDatas(DataIteration iterationDatas)
    {
        read = 0;
        if (creationDone)
        {
            _persons.ForEach(p => {
                MovementScript pScript = p.GetComponent<MovementScript>();
                int pIndex = pScript.index;
                int siteIndex = iterationDatas.PersonsNewSite[pIndex];

                dataReceived2.text = iterationDatas.PersonsNewSite.Count.ToString() + " " + read + " " + siteIndex + " " + _sitesMax.Count + " " + _sitesMin.Count;
                if (siteIndex < _sitesMin.Count && siteIndex >= 0)
                    p.GetComponent<MovementScript>().SetTarget(_sitesMin[siteIndex], _sitesMax[siteIndex], siteIndex);
                dataReceived3.text = iterationDatas.PersonsNewSite.Count.ToString() + " " + read + " " + siteIndex + " " + _sitesMax.Count + " " + _sitesMin.Count;
                if (pScript.state != iterationDatas.PersonsNewState[pIndex])
                    pScript.SetState(iterationDatas.PersonsNewState[pIndex]);


                read++;
            });
        }
    }

    public void CreatePopulation(int nbPeople, List<int> indexOfInfected)
    {
        for (int i = 0; i < nbPeople; i++)
        {
            GameObject p = Instantiate(person, new Vector3(0,0, -5), Quaternion.identity);
            MovementScript pScript = p.GetComponent<MovementScript>();

            if (indexOfInfected.Contains(i))
                pScript.SetState(3);

            pScript.index = i;
            _persons.Add(p);
            read++;
        }
        creationDone = true;
    }

    public void CreateSites(DataSites sites)
    {
        //House T
        float topStartX = -9;
        float topStopX = 9;
        float topStartY = -5;
        float topUnitSize = (topStartX*-1 + topStopX) / sites.NbHouse;
        float topXL = topStartX;
        float topXR = topStartX;
        for (int i = 0; i < sites.NbHouse; i++)
        {
            topXR += topUnitSize;
            CreateSite(new Vector2(topXL, topStartY+1), new Vector2(topXR, topStartY));
            topXL += topUnitSize;
        }

        // Hospital R
        float rightStartX = 9;
        float rightStartY = -4;
        float rightStopY = 4;
        float rightUnitSize = (rightStartY + rightStopY * -1) / (sites.NbHospital + sites.NbSchool);
        float rightYT = rightStartY;
        float rightYB = rightStartY;
        for (int i = 0; i < sites.NbHospital; i++)
        {
            rightYB -= rightUnitSize;
            CreateSite(new Vector2(rightStartX, rightYT), new Vector2(rightStartX + 1, rightYB));
            rightYT -= rightUnitSize;
        }
        // School R
        for (int i = 0; i < sites.NbSchool; i++)
        {
            rightYB -= rightUnitSize;
            CreateSite(new Vector2(rightStartX, rightYT), new Vector2(rightStartX + 1, rightYB));
            rightYT -= rightUnitSize;
        }

        float leftStartX = -10;
        float leftStartY = -4;
        float leftStopY = 4;
        float leftUnitSize = (leftStartY + leftStopY * -1) / (sites.NbStore + sites.NbRestaurant + sites.NbSupermarket);
        float leftYT = leftStartY;
        float leftYB = leftStartY;
        // Store L
        for (int i = 0; i < sites.NbStore; i++)
        {
            leftYB -= leftUnitSize;
            CreateSite(new Vector2(leftStartX, leftYT), new Vector2(leftStartX + 1, leftYB));
            leftYT -= leftUnitSize;
        }
        // Restaurant L
        for (int i = 0; i < sites.NbRestaurant; i++)
        {
            leftYB -= leftUnitSize;
            CreateSite(new Vector2(leftStartX, leftYT), new Vector2(leftStartX + 1, leftYB));
            leftYT -= leftUnitSize;
        }
        // Supermarket L
        for (int i = 0; i < sites.NbSupermarket; i++)
        {
            leftYB -= leftUnitSize;
            CreateSite(new Vector2(leftStartX, leftYT), new Vector2(leftStartX + 1, leftYB));
            leftYT -= leftUnitSize;
        }

        float BottomStartX = -9;
        float BottomStopX = 9;
        float BottomStartY = 5;
        float bottomUnitSize = (BottomStartX * -1 + BottomStopX) / sites.NbCompany;
        float bottomXL = BottomStartX;
        float bottomXR = BottomStartX;
        // Company B
        for (int i = 0; i < sites.NbCompany; i++)
        {
            bottomXR += bottomUnitSize;
            CreateSite(new Vector2(bottomXL, BottomStartY- 1), new Vector2(bottomXR, BottomStartY));
            bottomXL += bottomUnitSize;
        }

        PositioningBuildings(sites);
    }

    private void CreateSite(Vector2 min, Vector2 max)
    {
        _sitesMin.Add(min);
        _sitesMax.Add(max);
    }

    private void PositioningBuildings(DataSites sites)
    {
        // Top
        float topScaleX = 9;
        float topPositionY = 4.5f;
        GameObject houses = home;
        houses.transform.position = new Vector3(0, topPositionY, 1);
        houses.transform.localScale = new Vector2(topScaleX*2, 1);
        Instantiate(home);

        // Left
        float sumLeftScale = (sites.NbStore + sites.NbSupermarket + sites.NbRestaurant);
        float leftScaleYStore = 8f / sumLeftScale * sites.NbStore;
        float leftStartY = 4f;
        float leftPositionX = -9.5f;
        GameObject stores = store;
        stores.transform.position = new Vector3(leftPositionX, leftStartY - (leftScaleYStore / 2), 1);
        stores.transform.localScale = new Vector2(1, leftScaleYStore);
        Instantiate(stores);

        float leftScaleYRestaurant = 8f / sumLeftScale * sites.NbRestaurant;
        float leftStartYRestaurant = leftStartY - leftScaleYStore;
        GameObject restaurants = restaurant;
        restaurants.transform.position = new Vector3(leftPositionX, leftStartYRestaurant - (leftScaleYRestaurant / 2), 1);
        restaurants.transform.localScale = new Vector2(1, leftScaleYRestaurant);
        Instantiate(restaurants);

        float leftScaleYSupermarket = 8f / sumLeftScale * sites.NbSupermarket;
        float leftStartYSupermarket = leftStartYRestaurant - leftScaleYRestaurant;
        Debug.Log(leftStartYSupermarket);
        GameObject supermarkets = supermarket;
        supermarkets.transform.position = new Vector3(leftPositionX, leftStartYSupermarket - (leftScaleYSupermarket / 2), 1);
        supermarkets.transform.localScale = new Vector2(1, leftScaleYSupermarket);
        Instantiate(supermarkets);

        // Right
        float sumRightScale = (sites.NbSchool + sites.NbHospital);
        float rightScaleYHospital = 8f / sumRightScale * sites.NbHospital;
        float rightStartY = 4f;
        float rightPositionX = 9.5f;
        GameObject hospitals = hospital;
        hospitals.transform.position = new Vector3(rightPositionX, rightStartY - (rightScaleYHospital / 2), 1);
        hospitals.transform.localScale = new Vector2(1, rightScaleYHospital);
        Instantiate(hospitals);

        float rightScaleYSchool = 8f / sumRightScale * sites.NbSchool;
        float rightStartYSchool = rightStartY - rightScaleYHospital;
        GameObject schools = school;
        schools.transform.position = new Vector3(rightPositionX, rightStartYSchool - (rightScaleYSchool / 2), 1);
        schools.transform.localScale = new Vector2(1, rightScaleYSchool);
        Instantiate(schools);

        // Bottom
        float bottomScaleX = 9;
        float bottomPositionY = -4.5f;
        GameObject companies = company;
        companies.transform.position = new Vector3(0, bottomPositionY, 1);
        companies.transform.localScale = new Vector2(bottomScaleX * 2, 1);
        Instantiate(companies);
    }
}
