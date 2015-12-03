﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gallery : MonoBehaviour
{

    #region SuperHackSorryNiklasForEverything
    public UnityEngine.UI.InputField inputFieldPrice;
    public UnityEngine.UI.InputField inputFieldTitle;

    public GameObject PaintCanvasObject;

    public void ConfirmPainting()
    {
        // Make sure the text works 
        if (inputFieldTitle.text.Length <= 0) return;
        if (inputFieldPrice.text.Length <= 0) return;
        if (inputFieldTitle.text == "TITLE") return;

        PaintComponent paintComponent = PaintCanvasObject.GetComponent<PaintComponent>();
        Painting p = AddNewPainting(paintComponent.canvas);
        p.title = inputFieldTitle.text;
        p.price = int.Parse(inputFieldPrice.text);
        p.truePrice = p.price;
        p.nameFactor = Mathf.Clamp(-2 + (((p.title.GetHashCode() % 200) + 101) / 100), -1, 1) + 1; // temp change as it should go from 0-2 (or something like that)
        p.timeSpent = paintComponent.GetElapsedTime();
        paintComponent.ClearAll();

        inputFieldPrice.text = "25";
        inputFieldTitle.text = "TITLE";
    }
    #endregion

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
        SendMessageUpwards("AddMoney", painting.price);
        float priceDeviation = (float)painting.price / (float)painting.truePrice;
        if(priceDeviation >= 1.0f) {
            SendMessageUpwards("IncrementFamePoints");
        }
        else if(priceDeviation < 0.8f)
        {
            SendMessageUpwards("DecrementFamePoints");
        }
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
