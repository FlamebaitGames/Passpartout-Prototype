using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gallery : MonoBehaviour {


    public Painting[] paintings { get; private set; }
    public Customer[] customers { get; private set; }
    /// <summary>
    /// How many slots are left to put paintings on
    /// </summary>
    public int slotsLeft
    {
        get
        {
            int i = 0;
            foreach (Painting p in paintings)
            {
                if (!p.gameObject.activeSelf) i++;
            }
            return i;
        }
    }

    public Customer customerPrefab;

    private PathPoint[] galleryPath;
	// Use this for initialization
	void Start () {
        paintings = GetComponentsInChildren<Painting>();
        customers = GetComponentsInChildren<Customer>();
        foreach (Painting p in paintings)
        {
            p.gameObject.SetActive(false);
        }
        foreach (Customer c in customers)
        {
            c.gameObject.SetActive(false);
        }
        galleryPath = GetComponentInChildren<CustomerPath>().path;
	}

    private void PurchasePainting(Painting painting)
    {
        // TODO - add money to player & modify fame
        SendMessageUpwards("AddMoney", painting.price);
        painting.Remove();
    }

    

    /// <summary>
    /// Adds a new painting on the wall, will throw an error if no slots
    /// are left.
    /// </summary>
    /// <param name="canvas"></param>
    public Painting AddNewPainting(Texture canvas)
    {
        if (!(slotsLeft > 0))
        {
            Debug.LogError("No slots left to put paintings on!");
            Debug.Break();
        }
        
        foreach (Painting p in paintings)
        {
            if (!p.gameObject.activeSelf)
            {
                p.gameObject.SetActive(true);
                p.OverwriteNew(canvas);
                return p;
            }
        }
        return null;
        
    }

    //
    //      EVENTS BELOW
    //

    private void AddCustomer()
    {
        //Customer c = Instantiate<Customer>(customerPrefab); // may be replaced by permanent stash
        
        
        List<Customer> custs = new List<Customer>();
        foreach (Customer cust in customers)
        {
            if (!cust.gameObject.activeSelf) custs.Add(cust);
        }
        if (custs.Count == 0)
        {
            Debug.LogError("Ran out of customers!");
            Debug.Break();
        }
        Customer c = custs[Random.Range(0, custs.Count)];
        c.gameObject.SetActive(true);
        c.transform.position = galleryPath[0].point;
        c.BeginTraversingPath(galleryPath);
    }

    private void OnCustomerExit(Customer customer)
    {
        customer.gameObject.SetActive(false);
    }

    private void OnReset()
    {
        foreach (Customer c in GetComponentsInChildren<Customer>())
        {
            c.StopAllCoroutines();
            c.gameObject.SetActive(false);
        }
    }
}
