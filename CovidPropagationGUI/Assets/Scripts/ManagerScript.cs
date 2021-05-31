using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagerScript : MonoBehaviour
{
    public GameObject hospital;
    public GameObject school;
    public GameObject store;
    public GameObject supermarket;
    public GameObject restaurant;
    public GameObject home;
    public GameObject company;

    private List<GameObject> _topRow;
    private List<GameObject> _leftColumn;
    private List<GameObject> _rightColumn;
    private List<GameObject> _bottomRow;

    private List<GameObject> _sites;
    private Vector3 _centerZoneTopLeft;
    private Vector3 _centerZoneBottomRight;
    private Vector3 _perimeterZoneTopLeft;
    private Vector3 _perimeterZoneBottomRight;

    private GameObject _sitesParent;
    // Start is called before the first frame update
    void Start()
    {
        _sites = new List<GameObject>();
        _topRow = new List<GameObject>();
        _leftColumn = new List<GameObject>();
        _rightColumn = new List<GameObject>();
        _bottomRow = new List<GameObject>();

        _sitesParent = GameObject.Find("Sites");
        // x y z 
        // x z   /y
        int nbHospital = 2;
        int nbSchool = 5;
        int nbStore = 10;
        int nbSupermarket = 1;
        int nbRestaurant = 5;
        int nbHome = 50;
        int nbCompany = 5;

        _topRow.AddRange(CreateSite(home, nbHome));

        _rightColumn.AddRange(CreateSite(hospital, nbHospital));
        _rightColumn.AddRange(CreateSite(school, nbSchool));

        _leftColumn.AddRange(CreateSite(store, nbStore));
        _leftColumn.AddRange(CreateSite(supermarket, nbSupermarket));
        _leftColumn.AddRange(CreateSite(restaurant, nbRestaurant));

        _bottomRow.AddRange(CreateSite(company, nbCompany));

        PositioningBuildings();
    }

    private List<GameObject> CreateSite(GameObject site, int nbSites)
    {
        List<GameObject> lineOrRowSites = new List<GameObject>();
        for (int i = 0; i < nbSites; i++)
        {
            GameObject s = Instantiate(site, new Vector3(), Quaternion.identity);
            s.transform.parent = _sitesParent.transform;
            _sites.Add(s);
            lineOrRowSites.Add(s);
        }

        return lineOrRowSites;
    }

    // Update is called once per frame
    void Update()
    {
        //List<int> test = GameObject.Find("GUIManager").GetComponent<ScriptClient>().testTransfer;

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
        GameObject leftColumnLastElement = PositioningBuilding(_leftColumn, leftColPosition, space, false, topRowLastElement);
        GameObject rightColumnLastElement = PositioningBuilding(_rightColumn, rightColPosition, space, false, topRowLastElement);

        // if leftRowZ + size.z < rightRowZ + size.z
        // PositioningBuilding(_bottomRow, bottomRowPosition, space, true, rightColumnLastElement);
        //else
        PositioningBuilding(_bottomRow, bottomRowPosition, space, true, leftColumnLastElement);
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

            Debug.Log(newScale);
            b.transform.localScale = newScale;
        });
    }
}
