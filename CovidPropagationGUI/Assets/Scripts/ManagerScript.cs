using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private List<GameObject> _topRow;
    private List<GameObject> _leftColumn;
    private List<GameObject> _rightColumn;
    private List<GameObject> _bottomRow;

    private List<GameObject> _sites;

    private List<GameObject> _persons;

    private GameObject _sitesParent;
    private GameObject _personsParent;

    // Start is called before the first frame update
    void Start()
    {
        _topRow = new List<GameObject>();
        _leftColumn = new List<GameObject>();
        _rightColumn = new List<GameObject>();
        _bottomRow = new List<GameObject>();

        _sites = new List<GameObject>();
        _persons = new List<GameObject>();

        _sitesParent = GameObject.Find("Sites");
        _personsParent = GameObject.Find("Persons");

        DataPopulation populationDatas = JsonUtility.FromJson<DataPopulation>(@"{ ""NbPersons"":100000,""IndexOfInfected"":[]}");
        //DataSites sitesDatas = JsonUtility.FromJson<DataSites>(@"{ ""SitesType"":[0,2,4,6,4,6,6,4,1,3,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],""SitesId"":[0,1,5,20,70,220,670,4,2,10,40,140,440,1340,8,3,15,60,210,660,2010,1,3,4,5,6,7,8,9,10,11,12,13,16,19,22,25,28,31,34,35,36,39,42,45,48,51,54,57,60,63,64,65,68,71,74,77,80,83,84,87,90,93,96,99,102,105,108,111,114,117,120,123,126,129,132,135,138,141,144,147,150,153,156,159,162,165,168,171,174,177,180,181,182,185,188,189,192,195,198,201,204,207,210,213,216,219,222,225,228,231,234,237,240,241,242,245,248,251,254,257,258,259,262,265,268,271,274,277,278,279,280]}");
        DataSites sitesDatas = JsonUtility.FromJson<DataSites>(@"{ ""nbHouseType"":10,""nbCompanyType"":10,""nbHospitalType"":10,""nbRestaurantType"":10,""nbSchoolType"":10,""nbStoreType"":10,""nbSupermarketType"":10}");

        GetComponent<ScriptClient>().dataReceived3.text = populationDatas.NbPersons + " " + populationDatas.IndexOfInfected.Count;
        GetComponent<ScriptClient>().dataReceived2.text = sitesDatas.SitesType.Count + " " + sitesDatas.SitesId.Count;
        
        CreateSites(sitesDatas);
        CreatePopulation(populationDatas.NbPersons, populationDatas.IndexOfInfected);
    }

    // Update is called once per frame
    void Update()
    {
        //List<int> test = GameObject.Find("GUIManager").GetComponent<ScriptClient>().testTransfer;
        if (Input.GetMouseButton(1))
        {
            DataIteration iterationDatas = JsonUtility.FromJson<DataIteration>(@"{ ""PersonsNewSite"":[0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5, 
                                                                                                       0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,
                                                                                                       0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,
                                                                                                       0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,
                                                                                                       0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5],
                                                                                   ""PersonsNewState"":[0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,
                                                                                                        0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,
                                                                                                        0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,
                                                                                                        0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,
                                                                                                        0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3,0,1,2,3]}");
            for (int i = 0; i < _persons.Count; i++)
            {
                GameObject newTarget = _sites.FirstOrDefault(s => s.name == iterationDatas.PersonsNewSite[i].ToString());
                if (newTarget != null)
                {
                    _persons[i].GetComponent<MovementScript>().SetTarget(newTarget.transform);
                }
                _persons[i].GetComponent<MovementScript>().SetState(iterationDatas.PersonsNewState[i]);
            }
        }
    }

    public void SetIterationDatas(DataIteration iterationDatas)
    {
        for (int i = 0; i < _persons.Count; i++)
        {
            GameObject newTarget = _sites.FirstOrDefault(s => s.name == iterationDatas.PersonsNewSite[i].ToString());
            if (newTarget != null)
            {
                _persons[i].GetComponent<MovementScript>().SetTarget(newTarget.transform);
            }
            _persons[i].GetComponent<MovementScript>().SetState(iterationDatas.PersonsNewState[i]);
        }
    }

    public void CreatePopulation(int nbPeople, List<int> indexOfInfected)
    {
        for (int i = 0; i < nbPeople; i++)
        {
            GameObject p = Instantiate(person, _sites[Random.Range(0, _sites.Count - 1)].transform.position, Quaternion.identity);
            p.transform.parent = _personsParent.transform;
            if (indexOfInfected.Contains(i))
                p.GetComponent<MovementScript>().SetState(3);

            _persons.Add(p);
        }
    }

    public void CreateSites(DataSites sites)
    {
        int id = 0;
        //House T
        for (int i = 0; i < sites.nbHouseType; i++)
        {
            GameObject site = home;
            _topRow.Add(CreateSite(site, id));
            id++;
        }

        // Hospital R
        for (int i = 0; i < sites.nbHospitalType; i++)
        {
            GameObject site = hospital;
            _rightColumn.Add(CreateSite(site, id));
            id++;
        }
        // School R
        for (int i = 0; i < sites.nbSchoolType; i++)
        {
            GameObject site = school;
            _rightColumn.Add(CreateSite(site, id));
            id++;
        }

        // Store L
        for (int i = 0; i < sites.nbStoreType; i++)
        {
            GameObject site = store;
            _leftColumn.Add(CreateSite(site, id));
            id++;
        }
        // Restaurant L
        for (int i = 0; i < sites.nbRestaurantType; i++)
        {
            GameObject site = restaurant;
            _leftColumn.Add(CreateSite(site, id));
            id++;
        }
        // Supermarket L
        for (int i = 0; i < sites.nbSupermarketType; i++)
        {
            GameObject site = supermarket;
            _leftColumn.Add(CreateSite(site, id));
            id++;
        }

        // Company B
        for (int i = 0; i < sites.nbCompanyType; i++)
        {
            GameObject site = company;
            _bottomRow.Add(CreateSite(site, id));
            id++;
        }

        PositioningBuildings();
    }

    private GameObject ConvertIntToType(int siteTypeInt)
    {
        GameObject result;
        switch (siteTypeInt)
        {
            default:
            case 0:
                result = home;
                break;
            case 1:
                result = hospital;
                break;
            case 2:
                result = restaurant;
                break;
            case 3:
                result = school;
                break;
            case 4:
                result = store;
                break;
            case 5:
                result = supermarket;
                break;
            case 6:
                result = company;
                break;
            case 7:
                result = bus;
                break;
        }

        return result;
    }

    private GameObject CreateSite(GameObject site, int id)
    {
        GameObject s = Instantiate(site, new Vector3(), Quaternion.identity);
        s.transform.parent = _sitesParent.transform;
        s.transform.name = id.ToString();
        _sites.Add(s);

        return s;
    }

    private void PositioningBuildings()
    {
        float space = 0.1f;
        Vector3 topRowPosition = new Vector3(-5, 0, 5);
        Vector3 leftColPosition = new Vector3(-5, 0, 5);
        Vector3 rightColPosition = new Vector3(5, 0, 5);
        Vector3 bottomRowPosition = new Vector3(-5, 0, -5);

        ReScale(_topRow);
        ReScale(_leftColumn);
        ReScale(_rightColumn);
        ReScale(_bottomRow);

        GameObject topRowLastElement = PositioningBuilding(_topRow, topRowPosition, space, true, null);

        leftColPosition.x -= _leftColumn[0].transform.localScale.x / 2;
        leftColPosition.z -= _topRow[0].transform.localScale.z / 2;

        GameObject leftColumnLastElement = PositioningBuilding(_leftColumn, leftColPosition, space, false, topRowLastElement);

        rightColPosition.x += _rightColumn[0].transform.localScale.x / 2;
        rightColPosition.z -= _topRow[_topRow.Count - 1].transform.localScale.z / 2;

        GameObject rightColumnLastElement = PositioningBuilding(_rightColumn, rightColPosition, space, false, topRowLastElement);

        bottomRowPosition.z = _leftColumn[_leftColumn.Count - 1].transform.position.z - _bottomRow[0].transform.localScale.z / 2 - _leftColumn[_leftColumn.Count - 1].transform.localScale.z / 2;

        PositioningBuilding(_bottomRow, bottomRowPosition, space, true, leftColumnLastElement);

        float topRowPositionZ = _topRow[0].transform.position.z;
        float bottomRowPositionZ = _bottomRow[0].transform.position.z;
        float leftColumnPositionX = _leftColumn[0].transform.position.x;
        float RightColumnPositionX = _rightColumn[0].transform.position.x;

        float camPositionX = leftColumnPositionX + (Mathf.Abs(leftColumnPositionX) + Mathf.Abs(RightColumnPositionX)) / 2;
        float camPositionZ = bottomRowPositionZ + (Mathf.Abs(topRowPositionZ) + Mathf.Abs(bottomRowPositionZ)) / 2;

        Camera.GetComponent<CameraScript>().SetCenter(new Vector3(camPositionX, 0, camPositionZ));
    }

    private GameObject PositioningBuilding(List<GameObject> buildings, Vector3 position, float space, bool isRow, GameObject lastElement)
    {
        GameObject prev = null;
        buildings.ForEach(b => {
            if (prev == null)
            {
                prev = new GameObject();
                prev.transform.position = position;
                prev.transform.localScale = new Vector3();
            }

            if (isRow)
                b.transform.position = new Vector3((b.transform.localScale.x / 2 + prev.transform.localScale.x / 2) + prev.transform.position.x, 0, prev.transform.position.z);
            else
                b.transform.position = new Vector3(prev.transform.position.x, 0, -(b.transform.localScale.z / 2 + prev.transform.localScale.z / 2) + prev.transform.position.z);

            prev = b;
        });

        return prev;
    }

    private void ReScale(List<GameObject> buildings)
    {
        float maxSize = 10;
        buildings.ForEach(b => {
            Vector3 newScale = new Vector3(maxSize / buildings.Count,
                                           b.transform.localScale.y,
                                           maxSize / buildings.Count);

            b.transform.localScale = newScale;
        });
    }
}
